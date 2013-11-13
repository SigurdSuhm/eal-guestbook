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
	public partial class MainWindow : Window {
	   #region Fields

	   private Guestbook _currentGuestbook;

	   private bool _textCleared;

	   public AdminWindow adminWindow = new AdminWindow();

	   #endregion

	   #region Constructors

	   public MainWindow()
	   {
		  InitializeComponent();

		  _textCleared = false;

		  _currentGuestbook = new Guestbook();
		  _currentGuestbook.LoadGuestbookFile("Guestbook1.xml");
		  updatePageButtons();
		  updateEntriesView();
	   } 

	   #endregion

	   #region Public Methods

	   //public static DateTime TimeStampToDateTime(double timestamp)
	   //{
	   //    // Unix timestamp is seconds past epoch
	   //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
	   //    dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();

	   //    return dtDateTime;
	   //}

	   //public static double ConvertToTimestamp(DateTime value)
	   //{
	   //    //create Timespan by subtracting the value provided from
	   //    //the Unix Epoch
	   //    TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

	   //    //return the total seconds (which is a UNIX timestamp)
	   //    return (double)span.TotalSeconds;
	   //} 

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
		  _currentGuestbook.Add(new Entry(-1, getStringFromRichTextBox(writeNewMessageTextBox).Trim(), writeMessageNameTextBox.Text, calculateRating(), DateTime.Now));
		  updatePageButtons();
		  updateEntriesView();

		  _currentGuestbook.SaveGuestbookFile("Guestbook1.xml");

		  MessageBox.Show("Din besked er blevet tilføjet.", "Besked tilføjet", MessageBoxButton.OK, MessageBoxImage.Information);

		  writeMessageNameTextBox.Clear();
		  writeNewMessageTextBox.Document.Blocks.Clear();
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

	   #endregion

	   private void openAdminWindow(object sender, RoutedEventArgs e) {
		   // AdminWindow, Showing
		   adminWindow.Show();
	   }

	}
}
