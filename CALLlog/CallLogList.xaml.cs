using BackEndCallLogNS;
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
using System.Data;
using System.Text.RegularExpressions;

namespace CALLlog
{

    // Interaction logic for CallLogList.xaml

    public partial class CallLogList : Window
    {
        public CallLogList()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            UpdateDataGrid();
        }

        private void UpdateDataGrid(string searchText = "")
        {

            bool searchIsNumeric = Regex.IsMatch(searchText, @"^[0-9]+$");

            if (searchText == "")
            {
                DataSet? ds = BackEndCallLog.RetrieveCallLog(searchText);
                if (ds != null)
                {
                    callLogListGrid.ItemsSource = ds.Tables[0].DefaultView;
                }
            }
            else if (searchIsNumeric)
            {
                DataSet? ds = BackEndCallLog.QueryCallLogByTelephone(searchText);
                if (ds != null)
                {
                    callLogListGrid.ItemsSource = ds.Tables[0].DefaultView;
                }
            }
            else
            {
                DataSet? ds = BackEndCallLog.QueryCallLogByName(searchText);
                if (ds != null)
                {
                    callLogListGrid.ItemsSource = ds.Tables[0].DefaultView;
                }
            }
        }
        private void newCallLogBtn_Click(object sender, RoutedEventArgs e)
        {
            CallLogWindow objCallLogWindow = new CallLogWindow("add");
            objCallLogWindow.ShowDialog();
            UpdateDataGrid();
        }

        private void EditCallLogBtn_Click(object sender, RoutedEventArgs e)
        {
            if (callLogListGrid.SelectedItem != null)
            {
                DataRowView? dataRow = (DataRowView)callLogListGrid.SelectedItem;
                string cellValue = dataRow.Row.ItemArray[0].ToString();

                if (cellValue != null)
                {
                    CallLogWindow objCallLogWindow = new CallLogWindow("edit", cellValue);
                    objCallLogWindow.ShowDialog();
                    UpdateDataGrid();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(editSearch.Text);
        }

        private void DeleteCallLogButton_Click(object sender, RoutedEventArgs e)
        {
            if (callLogListGrid.SelectedItem != null)
            {
                DataRowView dataRow = (DataRowView)callLogListGrid.SelectedItem;
                string cellValue = dataRow.Row.ItemArray[0].ToString();

                if (cellValue != null)
                {
                    CallLogWindow objCallLogWindow = new CallLogWindow("delete", cellValue);
                    objCallLogWindow.ShowDialog();
                    UpdateDataGrid();
                }
            }
        }
    }
}
