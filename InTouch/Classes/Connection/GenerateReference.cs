using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace InTouch.Classes.Connection
{
    class GenerateReference
    {
        public string GetReference(DatabaseConnection dbCon, string cSettings)
        {
            DataSet ReferenceDataSet = dbCon.Get("Select cCharVal,nNumVal,YEAR(GETDATE()) as CurrentYear "
                + " from File_UniqueID where cSettingsCode='" + cSettings + "'");

            string cPrefix = ReferenceDataSet.Tables[0].Rows[0]["cCharVal"].ToString();
            int nNumVal = Convert.ToInt32(ReferenceDataSet.Tables[0].Rows[0]["nNumVal"]);
            string cCurYear = ReferenceDataSet.Tables[0].Rows[0]["CurrentYear"].ToString();

            return cPrefix.Trim() + "/" + cCurYear.Trim() + "/" + (1000 + nNumVal);
        }

        public bool UpdateNumVal(DatabaseConnection dbCon,string cUnique) {
            return dbCon.ExecuteQuery("Update File_UniqueID Set nNumVal=nNumVal+1 Where cSettingsCode='"+cUnique+"'");
        }
    }
}
