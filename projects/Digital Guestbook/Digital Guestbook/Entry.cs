#region Using Statements

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging; 

#endregion

namespace Digital_Guestbook
{
    /// <summary>
    /// Domain logic for entries in a guestbook.
    /// </summary>
    public class Entry
    {
        #region Fields

        // Unique entry ID
        private int _id;
        // Entry text
        private string _text;
        // Name of the user who posted the entry
        private string _name;
        // Entry rating
        private int _rating;
        // Entry time stamp
        private DateTime _dateTime;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the boolean value determining if the entry is shown.
        /// </summary>
        public bool IsShown { get; set; }

        /// <summary>
        /// Gets the ID of the guestbook entry.
        /// </summary>
        public int ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        /// <summary>
        /// Gets the text of the guestbook entry.
        /// </summary>
        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }

        /// <summary>
        /// Gets the name of the user who posted the guestbook entry.
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        /// <summary>
        /// Gets the rating of the guestbook entry.
        /// </summary>
        public int Rating
        {
            get { return _rating; }
            private set { _rating = value; }
        }

        /// <summary>
        /// Gets the time stamp of the guestbook entry.
        /// </summary>
        public DateTime DateTime
        {
            get { return _dateTime; }
            private set { _dateTime = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new guestbook entry.
        /// </summary>
        public Entry(int id, string text, string name, int rating, DateTime dateTime, bool isShown)
        {
            ID = id;
            Name = name;
            Text = text;
            Rating = rating;
            DateTime = dateTime;
            IsShown = isShown;
        } 

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a UI element for viewing the specified guestbook entry.
        /// </summary>
        /// <param name="entry">Entry that the UI element should be created for.</param>
        /// <returns>UI element of the type Border.</returns>
        public static Border MessageBuilder(Entry entry)
        {
            // Main border
            Border border = new Border();
            border.Margin = new Thickness(5, 0, 5, 5);
            border.BorderThickness = new Thickness(0, 0, 0, 1);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(90, 90, 90));
            border.Visibility = entry.IsShown ? Visibility.Visible : Visibility.Collapsed;

            // Main stack panel spanding the entire border
            StackPanel mainStackPanel = new StackPanel();
            mainStackPanel.Margin = new Thickness(0, 5, 0, 5);
            mainStackPanel.Orientation = Orientation.Vertical;

            // Grid for the entry header
            Grid headerGrid = new Grid();
            headerGrid.Margin = new Thickness(0, 0, 0, 3);

            // Stack panel for the name and time stamp
            StackPanel titleStack = new StackPanel();
            titleStack.Margin = new Thickness(3, 0, 3, 0);
            titleStack.Orientation = Orientation.Horizontal;

            // Label for the user name
            Label nameLabel = new Label();
            nameLabel.FontStyle = FontStyles.Italic;
            nameLabel.FontWeight = FontWeights.Bold;
            nameLabel.FontSize = 11;
            nameLabel.Padding = new Thickness(0);
            nameLabel.VerticalAlignment = VerticalAlignment.Top;
            nameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            nameLabel.Content = entry.Name;

            String timeStampString = "skrev d. " + entry.DateTime.ToString("M") + ".";

            // Label for the time stamp
            Label timeStampLabel = new Label();
            timeStampLabel.FontSize = 11;
            timeStampLabel.FontStyle = FontStyles.Italic;
            timeStampLabel.Padding = new Thickness(0);
            timeStampLabel.Margin = new Thickness(5, 0, 0, 0);
            timeStampLabel.VerticalAlignment = VerticalAlignment.Top;
            timeStampLabel.HorizontalAlignment = HorizontalAlignment.Left;
            timeStampLabel.Content = timeStampString;

            // Stack panel for the rating
            StackPanel ratingStack = new StackPanel();
            ratingStack.Orientation = Orientation.Horizontal;
            ratingStack.Width = 100;
            ratingStack.Margin = new Thickness(0, 0, 5, 0);
            ratingStack.HorizontalAlignment = HorizontalAlignment.Right;
            ratingStack.VerticalAlignment = VerticalAlignment.Top;

            // Active star rating image
            BitmapImage activeStar = new BitmapImage();
            activeStar.BeginInit();
            activeStar.UriSource = new Uri("resources/rating_star_active.png", UriKind.Relative);
            activeStar.EndInit();

            // Active star rating image
            BitmapImage deactivatedStar = new BitmapImage();
            deactivatedStar.BeginInit();
            deactivatedStar.UriSource = new Uri("resources/rating_star_deactivated.png", UriKind.Relative);
            deactivatedStar.EndInit();

            // Set image source for the rating stars
            for (int i = 0; i < 5; i++)
            {
                Image currentStar = new Image();
                currentStar.HorizontalAlignment = HorizontalAlignment.Left;
                currentStar.Width = 20;
                currentStar.Height = 20;

                if (i < entry.Rating)
                    currentStar.Source = activeStar;
                else
                    currentStar.Source = deactivatedStar;

                ratingStack.Children.Add(currentStar);
            }

            // Text block containing the entry text
            TextBlock textBlock = new TextBlock();
            textBlock.FontStyle = FontStyles.Italic;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = entry.Text;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.Padding = new Thickness(3, 0, 3, 0);
            textBlock.Margin = new Thickness(0, 0, 0, 3);


            // Title elements
            titleStack.Children.Add(nameLabel);
            titleStack.Children.Add(timeStampLabel);

            // Header elements
            headerGrid.Children.Add(titleStack);
            headerGrid.Children.Add(ratingStack);

            // Top level elements
            mainStackPanel.Children.Add(headerGrid);
            mainStackPanel.Children.Add(textBlock);

            border.Child = mainStackPanel;

            return border;
        }

        #endregion
    }
}
