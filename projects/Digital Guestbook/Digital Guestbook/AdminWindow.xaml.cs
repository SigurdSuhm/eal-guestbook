#region Using Statements

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

#endregion

namespace Digital_Guestbook
{
    public class GuestbookDescriptor
    {
        public string Name { get; set; }
        public string FileName { get; set; }
    }

    /// <summary>
    /// Interaction logic for the admin window.
    /// </summary>
    public partial class AdminWindow : Window
    {
        #region Constants

        private const int MAX_GUESTBOOK_NUMBER = 500;
        private const string GUESTBOOK_FILE_NAME = "Guestbooks.xml";

        #endregion

        #region Fields

        private ObservableCollection<GuestbookDescriptor> guestbookCollection;
        private Guestbook selectedGuestbook;

        #endregion

        #region Properties

        public MainWindow ParentWindow { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the admin window.
        /// </summary>
        /// <param name="guestbook"></param>
        public AdminWindow(MainWindow parent)
        {
            InitializeComponent();

            ParentWindow = parent;
            
            guestbookCollection = new ObservableCollection<GuestbookDescriptor>();
            cmbGuestbooks.ItemsSource = guestbookCollection;
            loadGuestbookFile(GUESTBOOK_FILE_NAME);

            foreach (GuestbookDescriptor gbd in guestbookCollection)
            {
                if (gbd.FileName == ParentWindow.CurrentGuestbook.FileName)
                {
                    cmbGuestbooks.SelectedItem = gbd;
                    break;
                }
            }

			// Sikre at der default gæstebogen altid er i guestbooks filen
			if(cmbGuestbooks.Items.Count == 0){
				GuestbookDescriptor newDefaultGb = new GuestbookDescriptor();
									newDefaultGb.Name = "Standard";
									newDefaultGb.FileName = "Guestbook1.xml";
					
				guestbookCollection.Add(newDefaultGb);
				saveGuestbookFile(GUESTBOOK_FILE_NAME);
			}

            selectedGuestbook = ParentWindow.CurrentGuestbook; 
            
            if (ParentWindow.CurrentGuestbook != null)
            {
                lsvEntries.ItemsSource = selectedGuestbook.EntryList;
            }

            skjulBtn.IsEnabled = false;
            sletBtn.IsEnabled = false;
            visBtn.IsEnabled = false;
        }

        #endregion

        #region Private Methods

        private void loadGuestbookFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                // Create empty guestbook file
                using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartDocument();
                    writer.WriteElementString("GuestbookList", String.Empty);
                    writer.WriteEndDocument();
                }
            }
            else
            {
                XmlDocument guestbookDoc = new XmlDocument();
                guestbookDoc.Load(fileName);

                foreach (XmlNode guestbookNode in guestbookDoc.DocumentElement.SelectNodes("Guestbook"))
                {
                    GuestbookDescriptor guestbook = new GuestbookDescriptor();
                    guestbook.Name = guestbookNode["Name"].InnerText;
                    guestbook.FileName = guestbookNode["FileName"].InnerText;

                    guestbookCollection.Add(guestbook);
                }
            }
        }

        private void saveGuestbookFile(string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement("GuestbookList");

                foreach (GuestbookDescriptor guestbook in guestbookCollection)
                {
                    writer.WriteStartElement("Guestbook");

                    writer.WriteElementString("Name", guestbook.Name);
                    writer.WriteElementString("FileName", guestbook.FileName);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

		private void GBActivator(){
			ParentWindow.CurrentGuestbook = selectedGuestbook;
		}

        /// <summary>
        /// Creates the UI element for individual guestbook entries in the admin window.
        /// </summary>
        /// <param name="entry">Entry to create UI element for.</param>
        /// <returns>UI element for the specified entry.</returns>
        private Grid postListBuilder(Entry entry)
        {
            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(10, GridUnitType.Star);

            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(85, GridUnitType.Star);

            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(120, GridUnitType.Star);

            ColumnDefinition column4 = new ColumnDefinition();
            column4.Width = new GridLength(120, GridUnitType.Star);

            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(column1);
            mainGrid.ColumnDefinitions.Add(column2);
            mainGrid.ColumnDefinitions.Add(column3);
            mainGrid.ColumnDefinitions.Add(column4);
            mainGrid.Margin = new Thickness(0, 1, 0, 0);

            Grid checkBoxGrid = new Grid();
            checkBoxGrid.Margin = new Thickness(1, 0, 0, 0);
            checkBoxGrid.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
            checkBoxGrid.SetValue(Grid.ColumnProperty, 0);

            CheckBox checkBox = new CheckBox();
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.HorizontalAlignment = HorizontalAlignment.Center;
            checkBoxGrid.Children.Add(checkBox);

            TextBlock nameBlock = new TextBlock();
            nameBlock.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
            nameBlock.Margin = new Thickness(1, 0, 0, 0);
            nameBlock.Padding = new Thickness(5, 2, 5, 2);
            nameBlock.SetValue(Grid.ColumnProperty, 1);
            nameBlock.Text = entry.Name.ToString();
            nameBlock.Height = mainGrid.Height;

            TextBlock mailBlock = new TextBlock();
            mailBlock.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
            mailBlock.Margin = new Thickness(1, 0, 0, 0);
            mailBlock.Padding = new Thickness(5, 2, 5, 2);
            mailBlock.SetValue(Grid.ColumnProperty, 2);
            mailBlock.Text = "test@email.dk";
            mailBlock.Height = mainGrid.Height;

            TextBlock timeBlock = new TextBlock();
            timeBlock.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
            timeBlock.Margin = new Thickness(1, 0, 0, 0);
            timeBlock.Padding = new Thickness(5, 2, 5, 2);
            timeBlock.SetValue(Grid.ColumnProperty, 3);
            timeBlock.Text = entry.DateTime.ToLongDateString();
            timeBlock.Height = mainGrid.Height;

            mainGrid.Children.Add(checkBoxGrid);
            mainGrid.Children.Add(nameBlock);
            mainGrid.Children.Add(mailBlock);
            mainGrid.Children.Add(timeBlock);

            return mainGrid;
        }

        #endregion

        #region Events

        private void btnNewGuestbook_Click(object sender, RoutedEventArgs e)
        {
            NewGuestbookWindow newGuestbookWindow = new NewGuestbookWindow();
            newGuestbookWindow.ShowDialog();

            if (newGuestbookWindow.CreateGuestbook)
            {
                // Generate file name
                string fileName = "Guestbook";

                for (int i = 1; i < MAX_GUESTBOOK_NUMBER; i++)
                {
                    bool fileNameInUse = false;

                    foreach (GuestbookDescriptor guestbook in guestbookCollection)
                    {
                        if (guestbook.FileName == (fileName + i + ".xml"))
                        {
                            fileNameInUse = true;
                            break;
                        }
                    }

                    if (!fileNameInUse)
                    {
                        fileName += i + ".xml";
                        break;
                    }
                }

                if (fileName == "Guestbook")
                    throw new Exception("Too many guestbooks have been created");

                

                GuestbookDescriptor newGuestbook = new GuestbookDescriptor() 
                { 
                    Name = newGuestbookWindow.GuestbookName, 
                    FileName = fileName 
                };
                guestbookCollection.Add(newGuestbook);
                saveGuestbookFile(GUESTBOOK_FILE_NAME);

                cmbGuestbooks.SelectedItem = newGuestbook;

                MessageBoxResult result = MessageBox.Show("Vil du aktivere den nye gæstebog?", "Gæstebog oprettet", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    ParentWindow.CurrentGuestbook = selectedGuestbook;
            }
        }

        private void cmbGuestbooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if(cmbGuestbooks.SelectedItem != null){
				Guestbook guestbook = new Guestbook((cmbGuestbooks.SelectedItem as GuestbookDescriptor).Name, (cmbGuestbooks.SelectedItem as GuestbookDescriptor).FileName);
				guestbook.LoadGuestbookFile();

				selectedGuestbook = guestbook;
				lsvEntries.ItemsSource = selectedGuestbook.EntryList;
			}
			else {
				if(cmbGuestbooks.Items.Count >= 1){
					cmbGuestbooks.SelectedIndex = 0;
				}
				else {
					if(cmbGuestbooks.Items.Count == 0){
						
					}
				}
			}
        }

        #endregion

        private void active_guestbookBtn_Click(object sender, RoutedEventArgs e)
        {
            GBActivator();
            MessageBox.Show(String.Format("Gæstebogen {0} er nu den aktive gæstebog.", selectedGuestbook.Name), "Gæstebog aktiveret", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void skjulBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lsvEntries.SelectedItem != null)
            {
                (lsvEntries.SelectedItem as Entry).IsShown = !(lsvEntries.SelectedItem as Entry).IsShown;
                lsvEntries.Items.Refresh();

                selectedGuestbook.SaveGuestbookFile();

                if ((lsvEntries.SelectedItem as Entry).IsShown)
                    skjulBtn.Content = "Skjul";
                else
                    skjulBtn.Content = "Vis";
            }
        }

        private void delete_GuestbookBtn_Click(object sender, RoutedEventArgs e)
        {
			GuestbookDescriptor selectedGB = (GuestbookDescriptor)cmbGuestbooks.SelectedItem;
            guestbookCollection.Remove(selectedGB);
            saveGuestbookFile(GUESTBOOK_FILE_NAME);
			if(File.Exists(selectedGB.FileName)) File.Delete(selectedGB.FileName);

			if(cmbGuestbooks.Items.Count == 0){
				
				MessageBox.Show("Standard gæstebogen blev tømt!");

				GuestbookDescriptor newGbd = new GuestbookDescriptor();
									newGbd.Name = "Standard";
									newGbd.FileName = "Guestbook1.xml";

				Guestbook			newGb = new Guestbook(newGbd.Name, newGbd.FileName);
					
				guestbookCollection.Add(newGbd);
				saveGuestbookFile(GUESTBOOK_FILE_NAME);
				cmbGuestbooks.SelectedIndex = 0;

				selectedGuestbook = newGb;
				GBActivator();
			}

        }

        private void lsvEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvEntries.SelectedItem != null)
            {
                if ((lsvEntries.SelectedItem as Entry).IsShown)
                    skjulBtn.Content = "Skjul";
                else
                    skjulBtn.Content = "Vis";

                skjulBtn.IsEnabled = true;
                sletBtn.IsEnabled = true;
                visBtn.IsEnabled = true;
            }
            else
            {
                skjulBtn.IsEnabled = false;
                sletBtn.IsEnabled = false;
                visBtn.IsEnabled = false;
            }
        }

        private void visBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lsvEntries.SelectedItem != null)
            {
                ShowEntryInfo showEntryDialog = new ShowEntryInfo();
                showEntryDialog.DataContext = (lsvEntries.SelectedItem as Entry);
                showEntryDialog.ShowDialog();
            }
        }
    }
}
