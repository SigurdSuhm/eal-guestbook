using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.IO;

namespace Digital_Guestbook
{
    public class Guestbook
    {
        private const int MAX_ID = 5000;
        private const int ENTRIES_PER_PAGE = 5;

        private List<Entry> _entryList;

        private int _currentPage;

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

        public int LastPage
        {
            get { return (_entryList.Count / ENTRIES_PER_PAGE); }
        }

        public Guestbook()
        {
            _entryList = new List<Entry>();

            _currentPage = 0;
        }

        public void Add(Entry entry)
        {
            int newEntryID = entry.ID;

            if (entry.ID == -1)
            {
                for (int i = 0; i < MAX_ID; i++)
                {
                    bool idInUse = false;

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

            Entry newEntry = new Entry(newEntryID, entry.Text, entry.Name, entry.Rating);
            newEntry.DateTime = DateTime.Now;

            _entryList.Add(newEntry);
        }

        public void LoadGuestbookFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                createGuestbookFile(fileName);
            }
            else
            {
                XmlDocument fileDocument = new XmlDocument();
                fileDocument.Load(fileName);

                XmlNode parentNode = fileDocument.DocumentElement;
                XmlNodeList entryNodeList = parentNode.SelectNodes("Entry");

                foreach (XmlNode curNode in entryNodeList)
                {
                    int id;
                    if (!int.TryParse(curNode["ID"].InnerText, out id))
                    {
                        // Error
                    }

                    string name = curNode["Name"].InnerText;
                    string text = curNode["Text"].InnerText;

                    int rating;
                    if (!int.TryParse(curNode["Rating"].InnerText, out rating))
                    {
                        // Error
                    }

                    _entryList.Add(new Entry(id, text, name, rating));
                } 
            }
        }

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

        public void SaveGuestbookFile(string fileName)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Guestbook");

                foreach (Entry currentEntry in _entryList)
                {
                    xmlWriter.WriteStartElement("Entry");

                    xmlWriter.WriteElementString("Name", currentEntry.Name);
                    xmlWriter.WriteElementString("ID", currentEntry.ID.ToString());
                    xmlWriter.WriteElementString("Text", currentEntry.Text);
                    xmlWriter.WriteElementString("Rating", currentEntry.Rating.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        public List<UIElement> GetGuestbookUIElements()
        {
            List<UIElement> elementsList = new List<UIElement>();

            for (int i = 0; i < ENTRIES_PER_PAGE; i++)
            {
                if ((i + (ENTRIES_PER_PAGE * _currentPage)) >= _entryList.Count)
                    break;

                elementsList.Add(ViewMessages.MessageBuilder(_entryList[i + (ENTRIES_PER_PAGE * _currentPage)]));
            }

            return elementsList;
        }
    }
}
