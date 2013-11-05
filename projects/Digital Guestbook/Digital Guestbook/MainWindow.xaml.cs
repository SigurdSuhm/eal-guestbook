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
using Digital_Guestbook.Classes;

namespace Digital_Guestbook
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        #region Fields
        Guestbook guestBook1;
        List<Entry> entryList;
        private static int messageId;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            guestBook1 = new Guestbook();
            entryList = new List<Entry>();
        } 
        #endregion

        #region Public Methods
        public static DateTime TimeStampToDateTime(double timestamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();

            return dtDateTime;
        }

        public static double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
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
        #endregion

        #region Events
        //Click event for 'Send besked'-knappen der tilføjer en ny Entry til listen 'entryList' der er et field
        // i main-klassen.
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            entryList.Add(new Entry(messageId++, getStringFromRichTextBox(writeNewMessageTextBox), writeMessageNameTextBox.Text, calculateRating()));
        }
        #endregion
    }
}
