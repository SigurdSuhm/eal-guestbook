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

namespace Digital_Guestbook
{
    /// <summary>
    /// Interaction logic for NewGuestbookWindow.xaml
    /// </summary>
    public partial class NewGuestbookWindow : Window
    {
        #region Properties

        public string GuestbookName { get; set; }

        public bool CreateGuestbook { get; private set; }

        #endregion

        public NewGuestbookWindow()
        {
            InitializeComponent();
            DataContext = this;

            GuestbookName = String.Empty;
            CreateGuestbook = false;
            txtName.Focus();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateGuestbook = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
