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

namespace Digital_Guestbook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private Guestbook _currentGuestbook;

        private bool _textCleared;
        private int _selectedRating;

        public AdminWindow adminWindow;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            _textCleared = false;
            _selectedRating = 0;

            _currentGuestbook = new Guestbook();
            _currentGuestbook.LoadGuestbookFile("Guestbook1.xml");
            updatePageButtons();
            updateEntriesView();
        }

        #endregion

        #region Private methods

        // Metoden skal returnere en rating alt efter hvor mange stjerner der er valgt
        private int calculateRating()
        {
            // To do..
            return 0;
        }

        // Metoden tager alt det content der er i en richtextbox og returnerer det som en string
        private string getStringFromRichTextBox(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        private void updateEntriesView()
        {
            gbContent.Children.Clear();

            foreach (UIElement element in _currentGuestbook.GetGuestbookUIElements())
            {
                gbContent.Children.Add(element);
            }
        }

        private void updatePageButtons()
        {
            btnPrevPage.IsEnabled = (_currentGuestbook.CurrentPage != 0);
            btnNextPage.IsEnabled = (_currentGuestbook.CurrentPage != _currentGuestbook.LastPage);
        }

        #endregion

        #region Events

        //Click event for 'Send besked'-knappen der tilføjer en ny Entry til listen 'entryList' der er et field
        // i main-klassen.
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentGuestbook.Add(new Entry(-1, getStringFromRichTextBox(writeNewMessageTextBox).Trim(), writeMessageNameTextBox.Text, _selectedRating, DateTime.Now));
            updatePageButtons();
            updateEntriesView();

            _currentGuestbook.SaveGuestbookFile("Guestbook1.xml");

            MessageBox.Show("Din besked er blevet tilføjet.", "Besked tilføjet", MessageBoxButton.OK, MessageBoxImage.Information);

            writeMessageNameTextBox.Clear();
            writeNewMessageTextBox.Document.Blocks.Clear();

            BitmapImage deactivatedStarSource = new BitmapImage();
            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("recourses/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;
                currentRatingStar.Source = deactivatedStarSource;
            }

            _selectedRating = 0;
        }

        private void writeNewMessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!_textCleared)
            {
                writeNewMessageTextBox.Document.Blocks.Clear();
                _textCleared = true;
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            _currentGuestbook.CurrentPage--;
            updatePageButtons();
            updateEntriesView();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            _currentGuestbook.CurrentPage++;
            updatePageButtons();
            updateEntriesView();
        }

        private void rating_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage activeStarSource = new BitmapImage();
            BitmapImage deactivatedStarSource = new BitmapImage();

            activeStarSource.BeginInit();
            activeStarSource.UriSource = new Uri("recourses/rating_star_active.png", UriKind.Relative);
            activeStarSource.EndInit();

            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("recourses/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            ImageSource sourceToUse = activeStarSource;

            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;

                if (currentRatingStar != null)
                {
                    currentRatingStar.Source = sourceToUse;

                    if (currentRatingStar == (sender as Image))
                        sourceToUse = deactivatedStarSource;
                }
            }
        }

        private void rating_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                if (stpRating.Children[i] == (sender as Image))
                {
                    _selectedRating = i + 1;
                    break;
                }
            }
        }

        private void rating_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage activeStarSource = new BitmapImage();
            BitmapImage deactivatedStarSource = new BitmapImage();

            activeStarSource.BeginInit();
            activeStarSource.UriSource = new Uri("recourses/rating_star_active.png", UriKind.Relative);
            activeStarSource.EndInit();

            deactivatedStarSource.BeginInit();
            deactivatedStarSource.UriSource = new Uri("recourses/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStarSource.EndInit();

            for (int i = 0; i < 5; i++)
            {
                Image currentRatingStar = stpRating.Children[i] as Image;

                if (i < _selectedRating)
                    currentRatingStar.Source = activeStarSource;
                else
                    currentRatingStar.Source = deactivatedStarSource;
            }
        }

        #endregion

        private void openAdminWindow(object sender, RoutedEventArgs e)
        {
            // AdminWindow, Showing
            adminWindow = new AdminWindow();
            adminWindow.ShowDialog();
        }
    }
}
