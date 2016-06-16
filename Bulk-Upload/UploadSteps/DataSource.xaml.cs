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

namespace ReferenceDataUploader.UploadSteps
{
    /// <summary>
    /// Interaction logic for DataSource.xaml
    /// </summary>
    public partial class DataSource : UserControl,ISwitchable
    {
        private bool browse_button_selected = false;
        private Excel.Sheets datasource_sheets = null;
        private Excel.Worksheet active_worksheet = null;
        private List<string> workSheetsList = new List<string>();
        private Dictionary<string, object> toTransfere = new Dictionary<string, object>();

        //private Dictionary<String,DatabaseConnection> dbConnections;
        private DatabaseConnection currentConnection = null;
        //private ConnectionStringSettingsCollect currentConnections;

        public DataSource()
        {
            InitializeComponent();
            navigation_buttons.set_Buttons(true, true, false);
            

            currentConnection = new DatabaseConnection("", "");

            this.combAuthentication.Items.Add(DatabaseConnection.WIN_AUTHENTICATION);
            this.combAuthentication.Items.Add(DatabaseConnection.SQL_AUTHENTICATION);

            this.combAuthentication.SelectedIndex = combAuthentication.Items.IndexOf(DatabaseConnection.WIN_AUTHENTICATION);
            this.Authenticatin_Index_Changed(null, null);
        }

        public void UtilizeState(Dictionary<string, object> state)
        {
            ;
        }

        public Dictionary<string, object> TransferToNextpage()
        {
            toTransfere.Add(Constants.DATA_SOURCE, datasource_sheets);
            toTransfere.Add(Constants.DATA_BASE_CONNETION, currentConnection);
            return toTransfere;
        }

        public void SetPageSwitcher(PageSwitcher pageSwitcher)
        {
            navigation_buttons.parentPageSwitcher = pageSwitcher;
        }

        private void broswe_file(object sender, KeyEventArgs e)
        {

            if (browse_button_selected && e.Key == Key.Enter) getExcelWorkBookFromFileBrowser();
        }

        private Excel.Workbook getExcelWorkBookFromFileBrowser()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "*.csv|*.xlsx";
            openFileDialog1.Title = "Select a Cursor File";
            

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
            if (openFileDialog1.ShowDialog() == true)
            {
                text_excel_file_path.Text = openFileDialog1.FileName;
                return getExcelWorkBookFromFilePath(openFileDialog1.FileName);
            }
            return null;
        }

        private Excel.Workbook getExcelWorkBookFromFilePath(String path)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook workbook = null;

             // Assign the cursor in the Stream to the Form's Cursor property.
             workbook = excelApplication.Workbooks.Open(path);
            return workbook;
        }

        private void broswe_file(object sender, RoutedEventArgs e)
        {
            datasource_sheets = getExcelWorkBookFromFileBrowser().Sheets;

            for(int i = 1; i <= datasource_sheets.Count;i++)
            {
                active_worksheet = datasource_sheets[i];
                workSheetsList.Add(active_worksheet.Name);
            }

            //listView_Sheets.ItemsSource = workSheetsList;
            //number_of_worksheets_changed();
        }

        private void got_focus(object sender, RoutedEventArgs e)
        {
            browse_button_selected = true;
        }

        private void lost_focus(object sender, RoutedEventArgs e)
        {
            browse_button_selected = false;
        }

        private void excel_path_changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            getExcelWorkBookFromFilePath(text_excel_file_path.Text);
        }

        //private void selected_worksheet_changed(object sender, SelectionChangedEventArgs e)
        //{
        //    active_worksheet = datasource_sheets.Item[listView_Sheets.SelectedIndex+1];
        //}

        //private void number_of_worksheets_changed()
        //{
        //    if (workSheetsList.Count == 0) listView_Sheets.Visibility = Visibility.Hidden;
        //    else listView_Sheets.Visibility = Visibility.Visible;
        //}

        private void Authenticatin_Index_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (combAuthentication.SelectedItem.ToString() == DatabaseConnection.WIN_AUTHENTICATION)
            {
                currentConnection.integratedSecurity = "True";

                currentConnection.userName = "";
                currentConnection.password = "";
                this.txtUserName.IsEnabled = false;
                this.txtPassword.IsEnabled = false;
                this.txtUserName.Text = "";
                this.txtPassword.Text = "";

            }
            else if (combAuthentication.SelectedItem.ToString() == DatabaseConnection.SQL_AUTHENTICATION)
            {
                currentConnection.integratedSecurity = "False";
                this.txtUserName.IsEnabled = true;
                this.txtPassword.IsEnabled = true;
            }
        }

        private void test_connection(object sender, RoutedEventArgs e)
        {
            //test connetion
            if (currentConnection.isValidConnection()) setMessage(false, "Connection Successfull");
            else setMessage(true, "Connection Failed");
        }

        private void setMessage(Boolean isError, String message)
        {
            if (isError) lable_message.Foreground = Brushes.Red;
            else lable_message.Foreground = Brushes.Black;

            if (String.IsNullOrWhiteSpace(message)) lable_message.Visibility = Visibility.Hidden;
            else lable_message.Visibility = Visibility.Visible;

            lable_message.Content = message;
        }

        private void databaseName_changed(object sender, TextChangedEventArgs e)
        {
            currentConnection.initialCatalog = txtDatabaseName.Text;
        }

        private void servre_name_changed(object sender, TextChangedEventArgs e)
        {
            currentConnection.dataSource = txtServer.Text;
        }

        private void userName_changed(object sender, TextChangedEventArgs e)
        {
            currentConnection.userName = txtUserName.Text;
        }

        private void password_changed(object sender, TextChangedEventArgs e)
        {
            currentConnection.password = txtPassword.Text;
        }
    }
}
