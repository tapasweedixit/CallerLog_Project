using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using BackEndCallLogNS;

namespace CALLlog
{
    /// <summary>
    /// Interaction logic for Landing.xaml
    /// </summary>
    public partial class Landing : Window
    {

        public Landing(bool isAdmin)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            if (!isAdmin)
            {
                Users.IsEnabled= false;
            }
        }

        
        private void Users_Click(object sender, RoutedEventArgs e)
        {
           UserList user = new UserList();
            user.Show();
            user.Closed += userlist_Closed;
            this.Hide();
        }

        private void userlist_Closed(object sender, EventArgs e)
        {
            ((UserList)sender).Closed -= userlist_Closed;
            this.Show();
        }
        
        

        private void Directory_Click(object sender, RoutedEventArgs e)
        {
            DirectoryWindow directory = new DirectoryWindow();
            directory.Show();
            this.Hide();
            directory.Closed += directory_closed;
        }

        private void directory_closed(object sender, EventArgs e)
        {
            ((DirectoryWindow)sender).Closed -= directory_closed;
            this.Show();
        }

        private void CallLog_Click(object sender, RoutedEventArgs e)
        {
            CallLogList log =   new CallLogList();
            log.Show();
            this.Hide();
            log.Closed += log_closed;
        }

        private void log_closed(object sender, EventArgs e)
        {
            ((CallLogList)sender).Closed -= log_closed;
            this.Show();
        }
    }
}
