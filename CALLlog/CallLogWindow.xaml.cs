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
    /// Interaction logic for CallLogWindow.xaml
    /// </summary>
    public partial class CallLogWindow : Window
    {
        private string mode = "search";
        DataSet? dataSetDirectory = new DataSet();
        public CallLogWindow(string mode, string idCall = "")
        {
            InitializeComponent();
            dataSetDirectory = BackEndCallLog.RetrieveDirectory("");
            editContactID.ItemsSource = dataSetDirectory.Tables[0].DefaultView;
            editContactID.DisplayMemberPath = dataSetDirectory.Tables[0].Columns[1].ToString();
            editContactID.SelectedValuePath = dataSetDirectory.Tables[0].Columns[0].ToString();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.mode = mode;
            setWindowsMode(mode, idCall);
        }

        private void setWindowsMode(string mode, string idCall = "")
        {
            if (mode == "search")
            {
                editCallLogID.IsEnabled = false;
                editContactID.IsEnabled = false;
                //editContactName.IsEnabled = false;
                //editContactTelephone.IsEnabled = false;
                editTypeOfCall.IsEnabled = false;
                editDateTime.IsEnabled = false;
                editDuration.IsEnabled = false;
                confirmButtton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;

                editCallLogID.Clear();
                //editContactName.Clear();
                //editContactTelephone.Clear();
                editTypeOfCall.SelectedValue = null;
                editDateTime.SelectedDate = null;
                editDuration.Clear();
            }
            else if (mode == "edit")
            {
                if (!search(idCall))
                {
                    this.Close();
                }
                editCallLogID.IsEnabled = false;
                editContactID.IsEnabled = true;
                //editContactName.IsEnabled = false;
                //editContactTelephone.IsEnabled = false;
                editTypeOfCall.IsEnabled = true;
                editDateTime.IsEnabled = true;
                editDuration.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
            else if (mode == "add")
            {
                editCallLogID.IsEnabled = false;
                editContactID.IsEnabled = true;
                //editContactName.IsEnabled = false;
                //editContactTelephone.IsEnabled = false;
                editTypeOfCall.IsEnabled = true;
                editDateTime.IsEnabled = true;
                editDuration.IsEnabled = true;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;

                editCallLogID.Clear();
                editContactID.SelectedValue = null;
                //editContactName.Clear();
                //editContactTelephone.Clear();
                editTypeOfCall.SelectedValue = null;
                editDateTime.SelectedDate = null;
                editDuration.Clear();
            }
            else if (mode == "delete")
            {
                if (!search(idCall))
                {
                    this.Close();
                }
                //editContactName.IsEnabled = false;
                editContactID.IsEnabled = false;
                //editContactTelephone.IsEnabled = false;
                editTypeOfCall.IsEnabled = false;
                editDateTime.IsEnabled = false;
                editDuration.IsEnabled = false;

                confirmButtton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
        }

        private int AssignTypeOfCall(string typeOfCall)
        {
            if (typeOfCall == "Incoming")
            {
                return 0;
            }
            else if (typeOfCall == "Outgoing")
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        private bool search(string searchText = "")
        {
            DataSet? ds = BackEndCallLog.RetrieveCallLog(searchText);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                    ("No Results found", "Oops!",
                        System.Windows.MessageBoxButton.OK);
                return false;
            }
            else
            {
                try
                {
                    editCallLogID.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    editContactID.SelectedValue = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    string typeOfCall = (string)ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    editTypeOfCall.SelectedIndex = AssignTypeOfCall(typeOfCall);
                    editDateTime.SelectedDate = (DateTime)ds.Tables[0].Rows[0].ItemArray[5];
                    editDuration.Text = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                }
                catch (Exception ex)
                {
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        (searchText + "-" + ex.Message, "Exception",
                            System.Windows.MessageBoxButton.OK);
                    return false;
                }
                return true;
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "edit")
            {
                ComboBoxItem selectedItem = (ComboBoxItem)editTypeOfCall.SelectedItem;
                string typeOfCall = selectedItem.Content.ToString();

                if (ValidateFields())
                {
                    BackEndCallLog.UpdateCallLog(editCallLogID.Text,
                                                 editContactID.SelectedValue.ToString(),
                                                 typeOfCall,
                                                 editDateTime.Text,
                                                 editDuration.Text);
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Call Log UPDATED", "Message",
                                System.Windows.MessageBoxButton.OK);
                    this.Close();
                }
            }
            else if (mode == "add")
            {
                ComboBoxItem? selectedItem = (ComboBoxItem)editTypeOfCall.SelectedItem;
                string typeOfCall = "";
                if (selectedItem != null)
                {
                    typeOfCall = selectedItem.Content.ToString();
                }

                if (ValidateFields())
                {
                    BackEndCallLog.InsertCallLog(editContactID.SelectedValue.ToString(),
                                                   typeOfCall,
                                                   editDateTime.Text,
                                                   editDuration.Text);
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("call Log ADDED", "Message",
                                System.Windows.MessageBoxButton.OK);
                    this.Close();
                }
            }
            else if (mode == "delete")
            {
                BackEndCallLog.DeleteCallLog(editCallLogID.Text);

                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        ("Call Log DELETED", "Message",
                            System.Windows.MessageBoxButton.OK);
                this.Close();
            }
        }

        private bool ValidateFields()
        {
            if (editContactID.SelectedIndex == -1)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Contact Name Required", "Message",
                                System.Windows.MessageBoxButton.OK);
                return false;
            }
            bool validContact = BackEndCallLog.validateContactID(editContactID.SelectedValue.ToString());
            if (!validContact)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Contact ID NOT Register in Directory", "Message",
                                System.Windows.MessageBoxButton.OK);
                return false;
            }
            if (editTypeOfCall.SelectedIndex == -1)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Select the Type of Call", "Message",
                                System.Windows.MessageBoxButton.OK);
                return false;
            }
            if (editDateTime.Text.Length == 0)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Date is Required", "Message",
                                System.Windows.MessageBoxButton.OK);
                return false;
            }
            int n;
            bool isNumeric = int.TryParse(editDuration.Text, out n);
            if (editDuration.Text.Length == 0 || !isNumeric)
            {
                MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                            ("Duration of the Call NOT Valid ", "Message",
                                System.Windows.MessageBoxButton.OK);
                return false;
            }
            return true;
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
