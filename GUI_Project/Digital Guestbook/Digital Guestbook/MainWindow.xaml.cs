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
using Digital_Guestbook.classes;

namespace Digital_Guestbook {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();

			DateTime	date = DateTime.Now;
			double	timestamp = ConvertToTimestamp(date);

			gbContent.Children.Add(ViewMessages.messageBuilder("Jørgen Jensen", timestamp, "Etiam tincidunt sapien lacus, eu ultrices quam semper vel. Proin eleifend tempus mi, sed adipiscing nisi venenatis ac. Proin mattis, mauris a fringilla elementum, mi metus dapibus lectus, consequat rhoncus elit eros ut sapien. Proin elementum arcu ut odio pretium, quis egestas elit pellentesque."));
		}

		public static DateTime TimeStampToDateTime(double timestamp) {
		
			// Unix timestamp is seconds past epoch
			System.DateTime	dtDateTime = new DateTime(1970,1,1,0,0,0,0);
							dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();

			return dtDateTime;
		
		}

		public static double ConvertToTimestamp(DateTime value){
		    //create Timespan by subtracting the value provided from
		    //the Unix Epoch
		    TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

		    //return the total seconds (which is a UNIX timestamp)
		    return (double)span.TotalSeconds;
		}

	}

}
