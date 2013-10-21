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
using System.Windows.Shapes;

namespace PartyApp
{
    /// <summary>
    /// Interaction logic for AddPerson.xaml
    /// </summary>
    public partial class AddPersonDialog : Window
    {
        List<Person> persons = new List<Person>();

        public List<Person> Persons
        {
            get { return persons; }
        }
        
        public AddPersonDialog()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name!", "Error in name");
            }
            else if (phoneNumberTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter a phone number!", "Error in phone number");
            }
            else if (foodCostTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter your cost in food!", "Error in food cost");
            }
            else
            {
                int phoneNumber;
                decimal estimatedEatCost;

                if (int.TryParse(phoneNumberTextBox.Text, out phoneNumber))
                {
                    if (decimal.TryParse(foodCostTextBox.Text, out estimatedEatCost))
                    {
                        persons.Add(new Person(nameTextBox.Text, phoneNumber, estimatedEatCost));
                        clearForm();
                        Hide();
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            clearForm();
            Hide();
        }

        private void clearForm()
        {
            nameTextBox.Text = string.Empty;
            phoneNumberTextBox.Text = string.Empty;
            foodCostTextBox.Text = string.Empty;
        }
    }
}
