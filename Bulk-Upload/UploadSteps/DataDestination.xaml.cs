using Microsoft.Win32;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;

namespace ReferenceDataUploader.UploadSteps
{
    /// <summary>
    /// Interaction logic for DataSource.xaml
    /// </summary>
    public partial class DataDestination : UserControl,ISwitchable
    {

        private Excel.Sheets datasource_sheets = null;
        private Excel.Worksheet active_worksheet = null;
        private List<string> workSheetsList = new List<string>();

        public DataDestination()
        {
            InitializeComponent();
            navigation_buttons.set_Buttons(true, false, true);

            if (workSheetsList.Count == 0) listView_Sheets.Visibility = Visibility.Hidden;
        }

        public void UtilizeState(object state)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)state;
            setMessage(false,"Work Sheet selected :  "+worksheet.Name);
        }

        public object TransferToNextpage()
        {
            return null;
        }

        public void SetPageSwitcher(PageSwitcher pageSwitcher)
        {
            navigation_buttons.parentPageSwitcher = pageSwitcher;
        }

        //private void Authenticatin_Index_Changed(object sender, SelectionChangedEventArgs e)
        //{
        //    if (combAuthentication.SelectedItem.ToString() == DatabaseConnection.WIN_AUTHENTICATION)
        //    {
        //        currentConnection.integratedSecurity = "True";

        //        currentConnection.userName = "";
        //        currentConnection.password = "";
        //        this.txtUserName.IsEnabled = false;
        //        this.txtPassword.IsEnabled = false;
        //        this.txtUserName.Text = "";
        //        this.txtPassword.Text = "";

        //    }
        //    else if (combAuthentication.SelectedItem.ToString() == DatabaseConnection.SQL_AUTHENTICATION)
        //    {
        //        currentConnection.integratedSecurity = "False";
        //        this.txtUserName.IsEnabled = true;
        //        this.txtPassword.IsEnabled = true;
        //    }
        //}

        //private void test_connection(object sender, RoutedEventArgs e)
        //{
        //    //test connetion
        //    if (currentConnection.isValidConnection()) setMessage(false, "Connection Successfull");
        //    else setMessage(true, "Connection Failed");
        //}

        private void setMessage(Boolean isError, String message)
        {
            //if (isError) lable_message.Foreground = Brushes.Red;
            //else lable_message.Foreground = Brushes.Black;

            //if (String.IsNullOrWhiteSpace(message)) lable_message.Visibility = Visibility.Hidden;
            //else lable_message.Visibility = Visibility.Visible;

            //lable_message.Content = message;
        }

        private void selected_worksheet_changed(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void databaseName_changed(object sender, TextChangedEventArgs e)
        //{
        //    currentConnection.initialCatalog = txtDatabaseName.Text;
        //}

        //private void servre_name_changed(object sender, TextChangedEventArgs e)
        //{
        //    currentConnection.dataSource = txtServer.Text;
        //}
    }
}
