using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using System.Data;
using BackEndCallLogNS;

namespace CALLlog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void LoginButton_Click_1(object sender, RoutedEventArgs e)
        {
            string username = UserNameText.Text;
            string password = PasswordText.Password;

            if (BackEndCallLog.IsValidUser(username,password))
            {

                Landing main_menu = new Landing(BackEndCallLog.IsADMIN(username,password));
                main_menu.Show();
                this.Hide();

                main_menu.Closed += mainmenu_closed;
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
           
        }

        private void mainmenu_closed (object sender, EventArgs e)
        { 
            ((Landing)sender).Closed -= mainmenu_closed;
            this.Show();
        }

        private void ClearButton_Click_1(object sender, RoutedEventArgs e)
        {
            UserNameText.Clear();
            PasswordText.Clear();
        }
    }
}
