using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Classes.Connection
{
    class Master_Data_Get
    {

        public bool Validate_Master_Duplicate(string FileName,string FieldName,object FieldValue)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet dupDataSet = dbCon.Get("SELECT a.Name FROM Sys.Objects a,Sys.Columns b "
                + "WHERE a.Object_Id=b.Object_id AND b.Name='"+ FieldName + "' and a.name <> "
                + "'File_"+ FileName+"'" + " and a.name Not Like '%Log_%'");

            DataSet FileDataSet;

            if (dupDataSet.Tables[0].Columns.Count > 0)
            {
                for (int i = 0; i < dupDataSet.Tables[0].Columns.Count; i++)
                {
                    string nFile = dupDataSet.Tables[0].Rows[i][0].ToString().Trim();

                    FileDataSet = dbCon.Get("Select " + FieldName + " From " + nFile
                        + " Where " + FieldName + " = '" + FieldValue + "'");

                    if (FileDataSet.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show(FieldValue + " Already Used In File " + nFile);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
