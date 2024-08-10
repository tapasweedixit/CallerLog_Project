using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using BackEndCallLogNS;

namespace CALLlog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserList : Window
    {
        private string mode = "search";
        public UserList()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            setWindowsMode("search");

        }

        private void setWindowsMode(string mode)
        {
            if (mode == "search")
            {
                searchButton.IsEnabled = true;
                addButton.IsEnabled = true;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                editUserID.IsEnabled = false;
                editUsername.IsEnabled = false;
                editFullName.IsEnabled = false;
                editPassword.IsEnabled = false;
                editIsAdmin.IsEnabled = false;
                confirmButtton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;

                editSearchUsername.Clear();
                editUserID.Clear();
                editUsername.Clear();
                editFullName.Clear();
                editPassword.Clear();
                editIsAdmin.IsChecked = false;
            }
            else if (mode == "edit")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editUsername.IsEnabled = true;
                editFullName.IsEnabled = true;
                editPassword.IsEnabled = true;
                editIsAdmin.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
            else if (mode == "add")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editUsername.IsEnabled = true;
                editFullName.IsEnabled = true;
                editPassword.IsEnabled = true;
                editIsAdmin.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;

                editUserID.Clear();
                editUsername.Clear();
                editFullName.Clear();
                editPassword.Clear();
                editIsAdmin.IsChecked = false;
            }
            else if (mode == "delete")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editUsername.IsEnabled = false;
                editFullName.IsEnabled = false;
                editPassword.IsEnabled = false;
                editIsAdmin.IsEnabled = false;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
        }
        
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = "add";
            setWindowsMode(mode);
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = "edit";
            setWindowsMode(mode);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = "delete";
            setWindowsMode(mode);
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "edit")
            {
                BackEndCallLog.UpdateUser(editUserID.Text,
                                            editUsername.Text,
                                            editFullName.Text,
                                            editPassword.Text,
                                            editIsAdmin.IsChecked);
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("User UPDATED", "Message",
                            System.Windows.MessageBoxButton.OK);

            }
            else if (mode == "add")
            {
                BackEndCallLog.InsertUser(editUsername.Text,
                                            editFullName.Text,
                                            editPassword.Text,
                                            editIsAdmin.IsChecked);
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("User ADDED", "Message",
                            System.Windows.MessageBoxButton.OK);
            }
            else if (mode == "delete")
            {
                BackEndCallLog.DeleteUser(editUserID.Text);

                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("User DELETED", "Message",
                            System.Windows.MessageBoxButton.OK);
            }
            mode = "search";

            setWindowsMode(mode);

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = "search";
            setWindowsMode(mode);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(editSearchUsername.Text))
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                    ("Username must be provided to Search", "Oops!",
                        System.Windows.MessageBoxButton.OK);
            }
            else
            {
                DataSet ds = BackEndCallLog.RetrieveUser(editSearchUsername.Text);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("No Results found", "Oops!",
                            System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    try
                    {
                        editUserID.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        editUsername.Text = (string)ds.Tables[0].Rows[0].ItemArray[1];
                        editFullName.Text = (string)ds.Tables[0].Rows[0].ItemArray[2];
                        editPassword.Text = (string)ds.Tables[0].Rows[0].ItemArray[3];
                        editIsAdmin.IsChecked = (bool)ds.Tables[0].Rows[0].ItemArray[4];
                    }
                    catch (Exception ex)
                    {
                        MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            (ex.Message, "Exception",
                                System.Windows.MessageBoxButton.OK);
                        return;
                    }

                    editButton.IsEnabled = true;
                    deleteButton.IsEnabled = true;
                }
            }
        }
    }
}
