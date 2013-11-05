using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Digital_Guestbook.Classes
{
    public class Entry
    {
        #region Fields
        private int _id;
        private string _text;
        private string _name;
        private int _rating;
        #endregion

        #region Properties
        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public int Rating
        {
            get { return _rating; }
            private set { _rating = value; }
        } 
        #endregion

        #region Constructors
        public Entry(int id, string text, string name, int rating)
        {
            Id = id;
            Name = name;
            Text = text;
            Rating = rating;
        } 
        #endregion

        #region Private Methods
        public static Border messageBuilder(string name, double timestamp, string text)
        {

            Border msgBorder = new Border();
            msgBorder.Margin = new Thickness(5, 0, 5, 5);
            msgBorder.BorderThickness = new Thickness(0, 0, 0, 1);
            msgBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(90, 90, 90));

            StackPanel msgStack = new StackPanel();
            msgStack.Margin = new Thickness(0, 5, 0, 5);
            msgStack.Orientation = Orientation.Vertical;

            Grid msgTopGrid = new Grid();
            msgTopGrid.Margin = new Thickness(0, 0, 0, 3);

            StackPanel msgTopStack = new StackPanel();
            msgTopStack.Margin = new Thickness(3, 0, 3, 0);
            msgTopStack.Orientation = Orientation.Horizontal;

            Label msgName = new Label();
            msgName.FontStyle = FontStyles.Italic;
            msgName.FontWeight = FontWeights.Bold;
            msgName.FontSize = 11;
            msgName.Padding = new Thickness(0);
            msgName.VerticalAlignment = VerticalAlignment.Top;
            msgName.HorizontalAlignment = HorizontalAlignment.Left;
            msgName.Content = name.ToString();

            String msgInfoString = "skrev d. " + MainWindow.TimeStampToDateTime(timestamp).ToString("M") + ".";

            Label msgInfo = new Label();
            msgInfo.FontSize = 11;
            msgInfo.FontStyle = FontStyles.Italic;
            msgInfo.Padding = new Thickness(0);
            msgInfo.Margin = new Thickness(5, 0, 0, 0);
            msgInfo.VerticalAlignment = VerticalAlignment.Top;
            msgInfo.HorizontalAlignment = HorizontalAlignment.Left;
            msgInfo.Content = msgInfoString;

            BitmapImage msgRatingBitmap = new BitmapImage();
            msgRatingBitmap.BeginInit();
            msgRatingBitmap.UriSource = new Uri("recourses/rating_stars.png", UriKind.Relative);
            msgRatingBitmap.EndInit();

            Image msgRatingStars = new Image();
            msgRatingStars.Source = msgRatingBitmap;
            msgRatingStars.Width = 75;
            msgRatingStars.Margin = new Thickness(0, 0, 5, 0);
            msgRatingStars.HorizontalAlignment = HorizontalAlignment.Right;
            msgRatingStars.VerticalAlignment = VerticalAlignment.Top;

            TextBlock msgText = new TextBlock();
            msgText.FontStyle = FontStyles.Italic;
            msgText.TextWrapping = TextWrapping.Wrap;
            msgText.Text = text.ToString();
            msgText.VerticalAlignment = VerticalAlignment.Top;
            msgText.Padding = new Thickness(3, 0, 3, 0);
            msgText.Margin = new Thickness(0, 0, 0, 3);


            // Inserting Elements
            msgTopStack.Children.Add(msgName);
            msgTopStack.Children.Add(msgInfo);

            msgTopGrid.Children.Add(msgTopStack);
            msgTopGrid.Children.Add(msgRatingStars);

            msgStack.Children.Add(msgTopGrid);
            msgStack.Children.Add(msgText);

            msgBorder.Child = msgStack;


            return msgBorder;
        }
        #endregion
    }
}
