#region Using Statements

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;
using System.IO; 

#endregion

namespace Digital_Guestbook
{
    /// <summary>
    /// Domain logic for guestbooks.
    /// </summary>
    public class Guestbook
    {
        #region Constants

        // Maximum ID for guestbook entries
        private const int MAX_ID = 5000;
        // Entries to show per page in the UI
        private const int ENTRIES_PER_PAGE = 5;

        #endregion

        #region Fields

        // List of guestbook entries
        private List<Entry> _entryList;

        // Current page to show in the UI
        private int _currentPage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the entry list of the guestbook.
        /// </summary>
        public List<Entry> EntryList
        {
            get { return _entryList; }
        }

        /// <summary>
        /// Gets or sets the current page of the guestbook to show in the UI.
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                // Check for invalid index
                if (value < 0 || value > (_entryList.Count / ENTRIES_PER_PAGE))
                {
                    throw new IndexOutOfRangeException("Bad index for the guestbook page.");
                }

                _currentPage = value;
            }
        }

        /// <summary>
        /// Gets the number of the last available page.
        /// </summary>
        public int LastPage
        {
            get { return ((_entryList.Count - 1) / ENTRIES_PER_PAGE); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new guestbook.
        /// </summary>
        public Guestbook()
        {
            _entryList = new List<Entry>();

            _currentPage = 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an entry to the guestbook.
        /// </summary>
        /// <param name="entry"></param>
        public void Add(Entry entry)
        {
            int newEntryID = entry.ID;

            // ID of -1 means that an ID has to be determined
            if (entry.ID == -1)
            {
                // Run through all available IDs
                for (int i = 0; i < MAX_ID; i++)
                {
                    bool idInUse = false;

                    // Check if the ID is in use
                    foreach (Entry curEntry in _entryList)
                    {
                        if (curEntry.ID == i)
                        {
                            idInUse = true;
                            break;
                        }
                    }

                    if (!idInUse)
                    {
                        newEntryID = i;
                        break;
                    }
                }
            }

            // Post entry to entry list
            Entry newEntry = new Entry(newEntryID, entry.Text, entry.Name, entry.Rating, entry.DateTime);
            _entryList.Add(newEntry);
        }

        /// <summary>
        /// Loads a guestbook from the specified Xml file.
        /// </summary>
        /// <param name="fileName">Path to the Xml file.</param>
        public void LoadGuestbookFile(string fileName)
        {
            // Check if file exists
            if (!File.Exists(fileName))
            {
                // Create new Xml file with only a parent node
                createGuestbookFile(fileName);
            }
            else
            {
                // Load Xml file
                XmlDocument fileDocument = new XmlDocument();
                fileDocument.Load(fileName);

                XmlNode parentNode = fileDocument.DocumentElement;
                XmlNodeList entryNodeList = parentNode.SelectNodes("Entry");

                // Get all entry nodes
                foreach (XmlNode curNode in entryNodeList)
                {
                    // Read entry content
                    int id = int.Parse(curNode["ID"].InnerText);
                    string text = curNode["Text"].InnerText;
                    string name = curNode["Name"].InnerText;
                    int rating = int.Parse(curNode["Rating"].InnerText);
                    DateTime dateTime = DateTime.Parse(curNode["DateTime"].InnerText);

                    _entryList.Add(new Entry(id, text, name, rating, dateTime));
                }
            }
        }

        /// <summary>
        /// Saves the guestbook to an Xml file at the specified path.
        /// </summary>
        /// <param name="fileName">Path to the Xml file to save to.</param>
        public void SaveGuestbookFile(string fileName)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;

                // Root element
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Guestbook");

                // Run through all entries in the entry list
                foreach (Entry currentEntry in _entryList)
                {
                    xmlWriter.WriteStartElement("Entry");

                    // Write entry content
                    xmlWriter.WriteElementString("Name", currentEntry.Name);
                    xmlWriter.WriteElementString("ID", currentEntry.ID.ToString());
                    xmlWriter.WriteElementString("Text", currentEntry.Text);
                    xmlWriter.WriteElementString("Rating", currentEntry.Rating.ToString());
                    xmlWriter.WriteElementString("DateTime", currentEntry.DateTime.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        /// <summary>
        /// Gets a collection of UI elements for the entries on the current page.
        /// </summary>
        /// <returns>List of UI elements.</returns>
        public List<UIElement> GetGuestbookUIElements()
        {
            List<UIElement> elementsList = new List<UIElement>();

            for (int i = 0; i < ENTRIES_PER_PAGE; i++)
            {
                if ((i + (ENTRIES_PER_PAGE * _currentPage)) >= _entryList.Count)
                    break;

                elementsList.Add(Entry.MessageBuilder(_entryList[i + (ENTRIES_PER_PAGE * _currentPage)]));
            }

            return elementsList;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new file for the guestbook.
        /// </summary>
        /// <param name="fileName">Path to the Xml file to be created.</param>
        private void createGuestbookFile(string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteElementString("Guestbook", string.Empty);
                writer.WriteEndDocument();
            }
        }

        #endregion
    }
}
