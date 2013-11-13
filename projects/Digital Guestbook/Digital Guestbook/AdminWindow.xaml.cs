using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml;

namespace Digital_Guestbook {
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window {

		#region Fields
		private string fileName = AppDomain.CurrentDomain.BaseDirectory + "Guestbook1.xml";
		private List<Entry> _gbMsgList = new List<Entry>();
		#endregion

		public AdminWindow() {
			InitializeComponent();

			// Loading the Entry List
			LoadGuestbookFile(fileName);

			foreach(Entry entry in _gbMsgList) {
				//MessageBox.Show(entry.Name.ToString());
				//admMessageList.Items.Add(postListBuilder(entry));
				admMessageList.Children.Add(postListBuilder(entry));
			}

		}

		public void LoadGuestbookFile(string fileName) {
			
			if (!File.Exists(fileName)){
				//createGuestbookFile(fileName);
			}
			else {

				XmlDocument	fileDocument = new XmlDocument();
							fileDocument.Load(fileName);

				XmlNode		parentNode = fileDocument.DocumentElement;
				XmlNodeList	entryNodeList = parentNode.SelectNodes("Entry");

				foreach (XmlNode node in entryNodeList){
					
					int id;
					
					if(!int.TryParse(node["ID"].InnerText, out id)){
						// Error
					}

					string		name = node["Name"].InnerText;
					//string		mail = node["Email"].InnerText;
					string		text = node["Text"].InnerText;
					DateTime		dateTime = DateTime.Parse(node["DateTime"].InnerText);

					int rating;

					if (!int.TryParse(node["Rating"].InnerText, out rating)){
						// Error
					}

					_gbMsgList.Add(new Entry(id, text, name, rating, dateTime));
					
				}
			}
		}

		public Grid postListBuilder(Entry entry){
			
			ColumnDefinition	column1 = new ColumnDefinition();
							column1.Width = new GridLength(10, GridUnitType.Star);

			ColumnDefinition	column2 = new ColumnDefinition();
							column2.Width = new GridLength(85, GridUnitType.Star);

			ColumnDefinition	column3 = new ColumnDefinition();
							column3.Width = new GridLength(120, GridUnitType.Star);

			ColumnDefinition	column4 = new ColumnDefinition();
							column4.Width = new GridLength(120, GridUnitType.Star);

			Grid				mainGrid = new Grid();
							mainGrid.ColumnDefinitions.Add(column1);
							mainGrid.ColumnDefinitions.Add(column2);
							mainGrid.ColumnDefinitions.Add(column3);
							mainGrid.ColumnDefinitions.Add(column4);
							mainGrid.Margin = new Thickness(0,1,0,0);

			Grid				checkBoxGrid = new Grid();
							checkBoxGrid.Margin = new Thickness(1,0,0,0);
							checkBoxGrid.Background = new SolidColorBrush(Color.FromArgb(255,187,187,187));
							checkBoxGrid.SetValue(Grid.ColumnProperty, 0);

			CheckBox			checkBox = new CheckBox();
							checkBox.VerticalAlignment = VerticalAlignment.Center;
							checkBox.HorizontalAlignment = HorizontalAlignment.Center;
							checkBoxGrid.Children.Add(checkBox);

			TextBlock			nameBlock = new TextBlock();
							nameBlock.Background = new SolidColorBrush(Color.FromArgb(255,187,187,187));
							nameBlock.Margin = new Thickness(1,0,0,0);
							nameBlock.Padding = new Thickness(5,2,5,2);
							nameBlock.SetValue(Grid.ColumnProperty, 1);
							nameBlock.Text = entry.Name.ToString();
							nameBlock.Height = mainGrid.Height;

			TextBlock			mailBlock = new TextBlock();
							mailBlock.Background = new SolidColorBrush(Color.FromArgb(255,187,187,187));
							mailBlock.Margin = new Thickness(1,0,0,0);
							mailBlock.Padding = new Thickness(5,2,5,2);
							mailBlock.SetValue(Grid.ColumnProperty, 2);
							mailBlock.Text = "test@email.dk";
							mailBlock.Height = mainGrid.Height;

			TextBlock			timeBlock = new TextBlock();
							timeBlock.Background = new SolidColorBrush(Color.FromArgb(255,187,187,187));
							timeBlock.Margin = new Thickness(1,0,0,0);
							timeBlock.Padding = new Thickness(5,2,5,2);
							timeBlock.SetValue(Grid.ColumnProperty, 3);
							timeBlock.Text = entry.DateTime.ToLongDateString();
							timeBlock.Height = mainGrid.Height;
							
							mainGrid.Children.Add(checkBoxGrid);
							mainGrid.Children.Add(nameBlock);
							mainGrid.Children.Add(mailBlock);
							mainGrid.Children.Add(timeBlock);
			
			return mainGrid;

		}

	}
}
