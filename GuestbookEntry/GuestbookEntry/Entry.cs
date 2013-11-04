using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestbookEntry
{
    public class Entry
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }
        private string _text;

        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }
        private int _rating;

        public int Rating
        {
            get { return _rating; }
            private set { _rating = value; }
        }
        public Entry(int id, string text, string name, int rating)
        {
            Id = id;
            Name = name;
            Text = text;
            Rating = rating;

        }

    }
}
