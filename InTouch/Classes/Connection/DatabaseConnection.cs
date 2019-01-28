using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InTouch.Classes.Connection
{
    class DatabaseConnection
    {
        public SqlConnection Con { get; set; }
        public SqlTransaction NewTransaction { get; set; }

        static string cons = System.IO.File.ReadAllText("C:\\conString.txt");

        string ConnectionString = cons.Trim();



        //string ConnectionString = "server=SUJEE-PC\\SQLEXPRESS;Initial Catalog=InTouch;User ID=sa;Password=sql@123;Integrated Security=True";

        //string ConnectionString = "server=10.1.0.253,1433;Initial Catalog=InTouch;User ID=InTouch;Password=123;Integrated Security=True"; 
        //string ConnectionString = "server=VSJ-ACCOUNTS\\SQLEXPRESS;Initial Catalog=InTouch;User Id=sa;Password=InTouch@123";

        public bool OpenDBConnection()
        {
            Con = new SqlConnection(ConnectionString);
            Con.Open();

            if (Con.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        public bool CloseDBConnection()
        {
            Con.Close();
            if (Con.State == ConnectionState.Closed)
                return true;
            else
                return false;
        }

        public void BeginTransaction()
        {
            NewTransaction = Con.BeginTransaction();
        }

        public void CommitTransaction()
        {
            NewTransaction.Commit();
        }

        public void RollBackTransaction()
        {
            NewTransaction.Rollback();
        }

        public DataSet Get(string cQuery)
        {
            OpenDBConnection();
            DataSet NDataset = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cQuery, Con.ConnectionString);
            CloseDBConnection();
            da.Fill(NDataset);
            return NDataset;
        }

        public DataSet Get_ReportData(string cQuery, string cTableName)
        {
            OpenDBConnection();
            DataSet NDataset = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cQuery, Con.ConnectionString);
            CloseDBConnection();
            da.Fill(NDataset, cTableName);
            return NDataset;
        }

        public bool ExecuteQuery(string newQuery) {
            SqlCommand command = new SqlCommand(newQuery, Con, NewTransaction);
            int i = command.ExecuteNonQuery();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool Upd(DataSet SDatSet,string TableName) {
            
            int columnCount = SDatSet.Tables[0].Columns.Count;
            int rowCount = SDatSet.Tables[0].Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {
                string insertFields = "";
                string insertValues = "";

                for (int j = 0; j < columnCount; j++)
                {
                    string columnName = SDatSet.Tables[0].Columns[j].ColumnName;

                    string dataType = columnName[0].ToString();

                    string iValue = SDatSet.Tables[0].Rows[i][columnName].ToString().Trim();

                    if (dataType.ToUpper().Equals("S"))
                        iValue = "'" + iValue + "'";
                    else if (dataType.ToUpper().Equals("I"))
                        iValue = "'" + iValue + "'";
                    else if (dataType.ToUpper().Equals("D"))
                        iValue = "'" + iValue + "'";
                    else if (dataType.ToUpper().Equals("N"))
                        iValue = "'" + iValue + "'";
                    else
                        iValue = "'" + iValue + "'";

                    insertFields = insertFields + columnName + ",";
                    insertValues = insertValues + iValue + ",";
                }
                insertFields = insertFields.Remove(insertFields.LastIndexOf(","), ",".Length);
                insertValues = insertValues.Remove(insertValues.LastIndexOf(","), ",".Length);

                if (!ExecuteQuery("INSERT INTO " + TableName + " (" + insertFields + ") VALUES (" + insertValues + ")"))
                    return false;
            }
            return true;
        }
    }
}
