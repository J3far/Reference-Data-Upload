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
using System.Data.SqlClient;

namespace ReferenceDataUploader.UploadSteps
{
    /// <summary>
    /// Interaction logic for DataSource.xaml
    /// </summary>
    public partial class DataDestination : UserControl,ISwitchable
    {

        private Excel.Sheets datasource_sheets = null;
        private Excel.Worksheet active_worksheet = null;
        private DatabaseConnection currentConnetion = null;
        private SqlDataReader dataReader = null;
        private List<string> workSheetsList = new List<string>();
        private List<string> tablesList = new List<string>();
        private object tempObject = null;
        private string sqlQuery = "select t.TABLE_NAME [Table Name],convert(nvarchar(10),qn.QestID) [QestID],qn.Value [QESTLab Object] from INFORMATION_SCHEMA.TABLES t"+Environment.NewLine+
                "inner join qestobjects qt on qt.Property = 'tablename' and t.TABLE_NAME = qt.Value" + Environment.NewLine +
                "inner join qestObjects qn on qn.Property = 'Name' and qt.QestID = qn.QestID" + Environment.NewLine +
                "where TABLE_TYPE = 'BASE TABLE'";
        private int table_name_index = 0;
        private int qestid_index = 1;
        private int qestlab_object_index = 2;

        private ListViewItem table_name = new ListViewItem();
        private ListViewItem qest_object_id = new ListViewItem();
        private ListViewItem qest_object_name = new ListViewItem();


        public DataDestination()
        {
            InitializeComponent();
            navigation_buttons.set_Buttons(true, false, true);

            //checkControlsVisibility();
        }

        public void UtilizeState(Dictionary<string, object> state)
        {
            if (state.TryGetValue(Constants.DATA_SOURCE, out tempObject)) datasource_sheets = (Excel.Sheets)tempObject;
            if (state.TryGetValue(Constants.DATA_BASE_CONNETION, out tempObject)) currentConnetion = (DatabaseConnection)tempObject;

            update_source_listView();
            update_destination_listView();
        }

        private void update_source_listView()
        {
            if (datasource_sheets == null) return;

            //get the list of teh worksheets from teh excel
            for (int i = 1; i <= datasource_sheets.Count; i++)
                workSheetsList.Add(datasource_sheets[i].Name);

            listView_source.ItemsSource = workSheetsList;
        }

        private void update_destination_listView()
        {
            //get the database tables
            dataReader = currentConnetion.executeQuery(sqlQuery);
            if (dataReader == null) return;

            while (dataReader.Read())
            {
                //combine all in one column
                //table_name.Add(dataReader.GetString(table_name_index));
                listView_destination.Items.Add(dataReader.GetString(qestid_index));
                listView_destination.Items.Add(dataReader.GetString(qestlab_object_index));
                
            }

            dataReader.Close();
            //listView_destination.ItemsSource = tablesList;
            
            //checkControlsVisibility();
        }

        public Dictionary<string, object> TransferToNextpage()
        {
            return null;
        }

        public void SetPageSwitcher(PageSwitcher pageSwitcher)
        {
            navigation_buttons.parentPageSwitcher = pageSwitcher;
        }


        private void selected_worksheet_changed(object sender, SelectionChangedEventArgs e)
        {
            active_worksheet = datasource_sheets.Item[listView_source.SelectedIndex + 1];
        }

        private void checkControlsVisibility()
        {
            if (workSheetsList.Count == 0) listView_source.Visibility = Visibility.Hidden;
            else listView_source.Visibility = Visibility.Visible;

            if (tablesList.Count == 0) listView_destination.Visibility = Visibility.Hidden;
            else listView_destination.Visibility = Visibility.Visible;
        }

        private void selected_table_changed(object sender, SelectionChangedEventArgs e)
        {

        }

        private void seacrhbox_lostfocus(object sender, RoutedEventArgs e)
        {

        }

        private void searchbox_keyup(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) update_destination_listView();
        }

        private void search_clicked(object sender, RoutedEventArgs e)
        {
            update_destination_listView();
        }

        private void search_keyup(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) update_destination_listView();
        }
    }
}
