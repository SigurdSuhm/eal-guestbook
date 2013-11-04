using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GuestbookEntry
{
    public class Guestbook
    {
        private List<Entry> _entryList;

        public Guestbook() 
        {
            _entryList = new List<Entry>();

        }
        public void AddEntry(Entry entry) 
        {
            _entryList.Add(entry);
        }

        public void LoadGuestbookFile(string fileName)
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

        public void Print()
        {
            foreach (Entry entry in _entryList)
                MessageBox.Show(entry.Name);
        }
    }
}
