#region Using Statements

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

#endregion

namespace Digital_Guestbook
{
    /// <summary>
    /// Interaction logic for the application main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        // Current guestbook object
        private Guestbook _currentGuestbook;

        // Flag for clearing the initial text from the text box
        private bool _textCleared;

        // Currently selected entry rating
        private int _selectedRating;

        #endregion

        #region Properties

        public Guestbook CurrentGuestbook
        {
            get { return _currentGuestbook; }
            set
            {
                _currentGuestbook = value;
                updateUi();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the main window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _textCleared = false;
            _selectedRating = 0;

            // Load guestbook from Xml
            loadActiveGuestbook();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the content of the rich text box as a string.
        /// </summary>
        /// <param name="rtb">Rich text box to get text from.</param>
        /// <returns>String containing the rich text box contents.</returns>
        private string getStringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        /// <summary>
        /// Updates the UI with all the entries on the current guestbook page.
        /// </summary>
        private void updateEntriesView()
        {
            gbContent.Children.Clear();

            foreach (UIElement element in _currentGuestbook.GetGuestbookUIElements())
            {
                gbContent.Children.Add(element);
            }
        }

        /// <summary>
        /// Updates next and previous buttons in the entry view.
        /// </summary>
        private void updatePageButtons()
        {
            btnPrevPage.IsEnabled = (_currentGuestbook.CurrentPage != 0);
            btnNextPage.IsEnabled = (_currentGuestbook.CurrentPage != _currentGuestbook.LastPage);
        }

        private void clearUi()
        {
            // Clear UI so a new entry can be created
            writeMessageNameTextBox.Clear();
            writeNewMessageTextBox.Document.Blocks.Clear();

            BitmapImage deactivatedStarSource = new BitmapImage();
            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("resources/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;
                currentRatingStar.Source = deactivatedStarSource;
            }

            _selectedRating = 0;
        }

        private void updateUi()
        {
            updatePageButtons();
            updateEntriesView();
        }

        private void loadActiveGuestbook()
        {
            XmlDocument guestbookDocument = new XmlDocument();

            if (!File.Exists("Guestbooks.xml"))
            {
                AdminWindow adminWindow = new AdminWindow(this);
                adminWindow.ShowDialog();
            }

            guestbookDocument.Load("Guestbooks.xml");

            foreach (XmlNode guestbookNode in guestbookDocument.DocumentElement.SelectNodes("Guestbook"))
            {
                if (bool.Parse(guestbookNode["IsActive"].InnerText))
                {
                    _currentGuestbook = new Guestbook(guestbookNode["Name"].InnerText,
                        guestbookNode["FileName"].InnerText,
                        DateTime.Parse(guestbookNode["DateCreated"].InnerText));

                    _currentGuestbook.LoadGuestbookFile();

                    break;
                }
            }

            updateUi();
        }

        private bool contentErrorCatcher()
        {
            bool result = true;

            // Checks if the name textbox contains anything
            if (!string.IsNullOrWhiteSpace(writeMessageNameTextBox.Text))
            {
                lblNoNameDot.Visibility = Visibility.Hidden;
                txtblockNoNameText.Visibility = Visibility.Hidden;
            }
            else
            {
                lblNoNameDot.Visibility = Visibility.Visible;
                txtblockNoNameText.Visibility = Visibility.Visible;

                result = false;
            }

            // Check if the write message textbox contains anything
            if (!(string.IsNullOrWhiteSpace(getStringFromRichTextBox(writeNewMessageTextBox)) || getStringFromRichTextBox(writeNewMessageTextBox).Equals("Din besked skrives her...\r\n")))
            {
                lblNoTextDot.Visibility = Visibility.Hidden;
                txtblockNoTextText.Visibility = Visibility.Hidden;
            }
            else
            {
                lblNoTextDot.Visibility = Visibility.Visible;
                txtblockNoTextText.Visibility = Visibility.Visible;

                result = false;
            }

            return result;
        }

        #endregion

        #region Events

        /// <summary>
        /// Click event for the button posting an entry.
        /// </summary>
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the Name and Message textboxes' content are valid
            if(contentErrorCatcher())
            {
                // Add entry to the guestbook
                _currentGuestbook.AddEntry(new Entry(-1, getStringFromRichTextBox(writeNewMessageTextBox).Trim(), writeMessageNameTextBox.Text, _selectedRating, DateTime.Now, true));
                updateUi();

                // Save guestbook file
                _currentGuestbook.SaveGuestbookFile();

                MessageBox.Show("Din besked er blevet tilføjet.", "Besked tilføjet", MessageBoxButton.OK, MessageBoxImage.Information);

                clearUi();
            }
        }

        /// <summary>
        /// Focus event for the rich text box.
        /// </summary>
        private void writeNewMessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear text if it has not already been done
            if (!_textCleared)
            {
                writeNewMessageTextBox.Document.Blocks.Clear();
                _textCleared = true;
            }
        }

        /// <summary>
        /// Click event for the previous page button.
        /// </summary>
        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            _currentGuestbook.CurrentPage--;
            updatePageButtons();
            updateEntriesView();
        }

        /// <summary>
        /// Click event for the next page button.
        /// </summary>
        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            _currentGuestbook.CurrentPage++;
            updatePageButtons();
            updateEntriesView();
        }

        /// <summary>
        /// Mouse enter event for the rating stars.
        /// </summary>
        private void rating_MouseEnter(object sender, MouseEventArgs e)
        {
            // Image sources for active and non-active stars
            BitmapImage activeStarSource = new BitmapImage();
            BitmapImage deactivatedStarSource = new BitmapImage();

            activeStarSource.BeginInit();
            activeStarSource.UriSource = new Uri("resources/rating_star_active.png", UriKind.Relative);
            activeStarSource.EndInit();

            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("resources/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            ImageSource sourceToUse = activeStarSource;

            // Run through all rating stars
            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;

                if (currentRatingStar != null)
                {
                    // Set image source
                    currentRatingStar.Source = sourceToUse;

                    // Change to the non-active source if we have reached the star that currently has mouse focus
                    if (currentRatingStar == (sender as Image))
                        sourceToUse = deactivatedStarSource;
                }
            }
        }

        /// <summary>
        /// Mouse down event for the rating stars.
        /// </summary>
        private void rating_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                if (stpRating.Children[i] == (sender as Image))
                {
                    // Set selected rating
                    _selectedRating = i + 1;
                    break;
                }
            }
        }

        /// <summary>
        /// Mouse leave event for the rating stars.
        /// </summary>
        private void rating_MouseLeave(object sender, MouseEventArgs e)
        {
            // Image sources for active and non-active stars
            BitmapImage activeStarSource = new BitmapImage();
            BitmapImage deactivatedStarSource = new BitmapImage();

            activeStarSource.BeginInit();
            activeStarSource.UriSource = new Uri("resources/rating_star_active.png", UriKind.Relative);
            activeStarSource.EndInit();

            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("resources/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            // Set the appropriate source for the rating stars
            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;

                if (i < _selectedRating)
                    currentRatingStar.Source = activeStarSource;
                else
                    currentRatingStar.Source = deactivatedStarSource;
            }
        }

        /// <summary>
        /// Click event for the admin button.
        /// </summary>
        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            // Show the admin window as a dialog
            AdminWindow adminWindow = new AdminWindow(this);
            adminWindow.ShowDialog();
            updateUi();
        }

        #endregion
    }
}
