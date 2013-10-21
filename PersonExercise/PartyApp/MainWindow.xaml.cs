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

namespace PartyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AddPersonDialog dialog = new AddPersonDialog();

        public MainWindow()
        {
            InitializeComponent();

            personsListBox.ItemsSource = dialog.Persons;
            dialog.IsVisibleChanged += dialog_IsVisibleChanged;
        }

        void dialog_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            personsListBox.Items.Refresh();
        }

        private void addPersonButton_Click(object sender, RoutedEventArgs e)
        {
            dialog.Show();
        }
    }
}
