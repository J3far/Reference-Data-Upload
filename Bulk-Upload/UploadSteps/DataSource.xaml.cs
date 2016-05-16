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
        private String excel_file_path = "";

        //private Dictionary<String,DatabaseConnection> dbConnections;
        private DatabaseConnection currentConnection = null;
        //private ConnectionStringSettingsCollect currentConnections;

        public DataSource()
        {
            InitializeComponent();

            navigation_buttons.set_Buttons(true, false, false);
            

            currentConnection = new DatabaseConnection("", "");

            this.combAuthentication.Items.Add(DatabaseConnection.WIN_AUTHENTICATION);
            this.combAuthentication.Items.Add(DatabaseConnection.SQL_AUTHENTICATION);

            this.combAuthentication.SelectedIndex = combAuthentication.Items.IndexOf(DatabaseConnection.WIN_AUTHENTICATION);
            this.Authenticatin_Index_Changed(null, null);
        }

        public void UtilizeState(object state)
        {
            ;
        }

        public object TransferToNextpage()
        {
            return active_worksheet;
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
            try {
                workbook = excelApplication.Workbooks.Open(path);
            }catch(Exception )
            {
                return null;
            }
            return workbook;
        }

        private void broswe_file(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "*.csv|*.xlsx";
            openFileDialog1.Title = "Select a Cursor File";
            if (openFileDialog1.ShowDialog() == true) text_excel_file_path.Text = openFileDialog1.FileName;
            check_source_file(null, null);
            //get the excel file
            //datasource_sheets = getExcelWorkBookFromFileBrowser().Sheets;
            //validate_source_file();
            ////for(int i = 1; i <= datasource_sheets.Count;i++)
            //{
            //    active_worksheet = datasource_sheets[i];
            //    workSheetsList.Add(active_worksheet.Name);
            //}

            ////listView_Sheets.ItemsSource = workSheetsList;
            ////number_of_worksheets_changed();
        }

        private void got_focus(object sender, RoutedEventArgs e)
        {
            browse_button_selected = true;
        }

        private void lost_focus(object sender, RoutedEventArgs e)
        {
            browse_button_selected = false;
        }

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

        private void read_source_file()
        {
            
            if (getExcelWorkBookFromFilePath(text_excel_file_path.Text) == null)
            {
                setMessage(true, "Invalid source file, please specify a csv file.");
                navigation_buttons.set_Buttons(true, false, false);
            }
            else
            {
                datasource_sheets = getExcelWorkBookFromFilePath(text_excel_file_path.Text).Sheets;
                setMessage(false, "Source file loaded successfully.");
                navigation_buttons.set_Buttons(true, true, false);

            }
        }

        private void check_source_file(object sender, RoutedEventArgs e)
        {
            if (text_excel_file_path.Text != excel_file_path)
            {
                excel_file_path = text_excel_file_path.Text;
                read_source_file();
            }
        }
    }
}
