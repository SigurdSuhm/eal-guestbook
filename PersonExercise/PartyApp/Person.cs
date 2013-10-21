using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyApp
{
    public class Person
    {
        private string _name;

        private int _phoneNumber;

        decimal _estimatedEatCost;

        public string Name
        {
            get { return _name; }
        }

        public int PhoneNumber
        {
            get { return _phoneNumber; }
        }

        public decimal EstimatedEatCost
        {
            get { return _estimatedEatCost; }
        }

        public Person(string name, int phoneNumber, decimal estimatedEatCost)
        {
            _name = name;
            _phoneNumber = phoneNumber;
            _estimatedEatCost = estimatedEatCost;
        }

        public override string ToString()
        {
            return "name: " + _name + "\t" + "phone: " + _phoneNumber + "\t" + "food cost: " + _estimatedEatCost;
        }
    }
}
