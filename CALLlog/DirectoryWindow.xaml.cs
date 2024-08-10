using BackEndCallLogNS;
using System;
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
using System.Windows.Shapes;


namespace CALLlog
{
    /// <summary>
    /// Interaction logic for DirectoryWindow.xaml
    /// </summary>
    public partial class DirectoryWindow : Window
    {
        private string mode = "search";
        public DirectoryWindow()
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
                editDirectoryID.IsEnabled = false;
                editName.IsEnabled = false;
                editTelephone.IsEnabled = false;

                confirmButtton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;

                editSearchTelephone.Clear();
                editDirectoryID.Clear();
                editName.Clear();
                editTelephone.Clear();
                ;
            }
            else if (mode == "edit")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editDirectoryID.IsEnabled = true;
                editName.IsEnabled = true;
                editTelephone.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
            else if (mode == "add")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editName.IsEnabled = true;
                editTelephone.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;

                editDirectoryID.Clear();
                editName.Clear();
                editTelephone.Clear();
            }
            else if (mode == "delete")
            {
                addButton.IsEnabled = false;
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                searchButton.IsEnabled = false;

                editName.IsEnabled = false;
                editTelephone.IsEnabled = false;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
        }
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(editSearchTelephone.Text))
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                    ("Telephone must be provided to Search", "Oops!",
                        System.Windows.MessageBoxButton.OK);
            }
            else
            {
                DataSet? ds = BackEndCallLog.RetrieveDirectory(editSearchTelephone.Text);
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
                        editDirectoryID.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        editName.Text = (string)ds.Tables[0].Rows[0].ItemArray[1];
                        editTelephone.Text = (string)ds.Tables[0].Rows[0].ItemArray[2];
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
                BackEndCallLog.UpdateDirectory(editDirectoryID.Text,
                                                 editName.Text,
                                                 editTelephone.Text);

                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("Contact UPDATED", "Message",
                            System.Windows.MessageBoxButton.OK);

            }
            else if (mode == "add")
            {
                if (!BackEndCallLog.validateTelephone(editTelephone.Text))
                {
                    BackEndCallLog.InsertDirectory(editName.Text,
                                                                     editTelephone.Text);

                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Contact ADDED", "Message",
                                System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Telephone ALREADY Register in the Directory", "Message",
                                System.Windows.MessageBoxButton.OK);
                }
                
            }
            else if (mode == "delete")
            {
                if (BackEndCallLog.validateContactCallLog(editDirectoryID.Text) == true)
                {
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("Contact CANNOT BE DELETED. It has Call Logs Registered", "Message",
                            System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    BackEndCallLog.DeleteDirectory(editDirectoryID.Text);
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Contact DELETED", "Message",
                                System.Windows.MessageBoxButton.OK);
                }

            }
            mode = "search";
            setWindowsMode(mode);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = "search";
            setWindowsMode(mode);
        }
    }
}


