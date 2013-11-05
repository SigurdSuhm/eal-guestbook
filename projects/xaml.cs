using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;

namespace Xmlsave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        public void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "MessageSubmit";
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Xml Files  (.xml)|*.xml";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string path = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(path))
                {
                    writePersonXmlFile(path);   
                }
            }

        
        }
        private void writePersonXmlFile(string path)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(path, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Guestbook");
                foreach (Guestbook currentEntry in _entryList)
                {
                    xmlWriter.WriteStartElement("Entry");
                    xmlWriter.WriteStartElementString("Name", currentEntry.Name);
                    xmlWriter.WriteStartElementString("ID", currentEntry.ID.Tostring());
                    xmlWriter.WriteStartElementString("Text", currentEntry.Text);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
        
        }


    }
}
