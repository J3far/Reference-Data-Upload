using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ReferenceDataUploader
{
    class DatabaseConnection
    {
        public const String WIN_AUTHENTICATION = "Windows Authentication";
        public const String SQL_AUTHENTICATION = "SQL Server Authentication";

        private const String APP_DEFAULT = "Bulk Upload";
        private const String MULTIPLE_ACTIVE_RESULT_SETS_DEFAULT = "True";
        private const String ASYNCHRONOUS_PROCESSING_DEFAULT = "True";
        private const String INTEGRATED_SECURITY_DEFAULT = "True";


        private String ConnectionString = "";

        public DatabaseConnection(String connectionName, String connectionString)
        {
            this.connectionString = connectionString;
            this.connectionName = connectionName;
        }

        public String connectionString
        {
            get
            {
                setConnectionString();
                return this.ConnectionString;
            }
            set
            {
                this.ConnectionString = value;
                parseConnection();
            }
        }
        public String connectionName { get; set; }
        public String dataSource { get; set; }
        public String integratedSecurity { get; set; }
        public String initialCatalog { get; set; }
        public String multipleActiveResultSets { get; set; }
        public String app { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
        public String asynchronousProcessing { get; set; }

        private void parseConnection()
        {
            if (String.IsNullOrEmpty(ConnectionString)) return;

            String[] data = ConnectionString.Split(';');
            foreach (String setting in data)
            {
                if (setting.StartsWith("data source")) dataSource = setting.Substring(setting.IndexOf("=") + 1);
                else if (setting.StartsWith("data source")) dataSource = setting.Substring(setting.IndexOf("=") + 1);
                else if (setting.StartsWith("initial catalog")) initialCatalog = setting.Substring(setting.IndexOf("=") + 1);
                else if (setting.StartsWith("integrated security")) integratedSecurity = setting.Substring(setting.IndexOf("=") + 1);
                else if (setting.StartsWith("multipleactiveresultsets")) multipleActiveResultSets = setting.Substring(setting.IndexOf("=") + 1);
                else if (setting.StartsWith("App")) app = setting.Substring(setting.IndexOf("=") + 1);
            }
        }

        private void setConnectionString()
        {
            this.ConnectionString = "data source=" + dataSource +
                ";initial catalog=" + initialCatalog +
                ";integrated security=" + (String.IsNullOrWhiteSpace(integratedSecurity) ? INTEGRATED_SECURITY_DEFAULT : integratedSecurity) +
                ";multipleactiveresultsets=" + (String.IsNullOrWhiteSpace(multipleActiveResultSets) ? MULTIPLE_ACTIVE_RESULT_SETS_DEFAULT : multipleActiveResultSets) +
                ";Asynchronous Processing=" + (String.IsNullOrWhiteSpace(asynchronousProcessing) ? ASYNCHRONOUS_PROCESSING_DEFAULT : asynchronousProcessing) +
                ";App=" + (String.IsNullOrWhiteSpace(app) ? APP_DEFAULT : app);
        }

        public bool isValidConnection()
        {

            try
            {
                String connectionString = "data source=" + dataSource +
                ";initial catalog=" + initialCatalog +
                ";integrated security=" + (String.IsNullOrWhiteSpace(integratedSecurity) ? INTEGRATED_SECURITY_DEFAULT : integratedSecurity) +
                ";multipleactiveresultsets=" + (String.IsNullOrWhiteSpace(multipleActiveResultSets) ? MULTIPLE_ACTIVE_RESULT_SETS_DEFAULT : multipleActiveResultSets) +
                ";App=" + (String.IsNullOrWhiteSpace(app) ? APP_DEFAULT : app);

                SqlConnection testConnection = new SqlConnection(connectionString);
                testConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
