using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using InTouch.Classes.Connection;

namespace InTouch.Classes.MasterHelp
{
    class MasterHelp
    {
        public object Get_Student(object[][] helpArray, string sWhere)
        {

            DatabaseConnection dbCon = new DatabaseConnection();

            string sQuery = "Select SStudentId,SName,SAddress1,rtrim(SAddress2) + ', ' + ltrim(SAddress3) as SAddress2 from File_Student " + sWhere;

            DataSet Help_DataSet = dbCon.Get(sQuery);

            if (Help_DataSet.Tables[0].Rows.Count <= 0)
                return helpArray;

            helpArray[0][2] = Help_DataSet.Tables[0].Rows[0]["SStudentId"];
            helpArray[1][2] = Help_DataSet.Tables[0].Rows[0]["SName"];
            helpArray[2][2] = Help_DataSet.Tables[0].Rows[0]["SAddress1"];
            helpArray[3][2] = Help_DataSet.Tables[0].Rows[0]["SAddress2"];
            return helpArray;
        }

        public object Get_Course(object[][] helpArray, string sWhere)
        {

            DatabaseConnection dbCon = new DatabaseConnection();

            string sQuery = "Select SCourseCode,SCourseName,NAmount,NCreditPeriod from File_Course " + sWhere;

            DataSet Help_DataSet = dbCon.Get(sQuery);

            if (Help_DataSet.Tables[0].Rows.Count <= 0)
                return helpArray;

            helpArray[0][2] = Help_DataSet.Tables[0].Rows[0]["SCourseCode"];
            helpArray[1][2] = Help_DataSet.Tables[0].Rows[0]["SCourseName"];
            helpArray[2][2] = Help_DataSet.Tables[0].Rows[0]["NAmount"];
            helpArray[3][2] = Help_DataSet.Tables[0].Rows[0]["NCreditPeriod"];

            return helpArray;
        }

        internal object Get_SalesPerson(object[][] helpArray, string sWhere)
        {
            DatabaseConnection dbCon = new DatabaseConnection();

            string sQuery = "Select SRepCode,SRepName,STelephoneNo,SMobileNo,SEmail from File_SalesPerson " + sWhere;

            DataSet Help_DataSet = dbCon.Get(sQuery);

            if (Help_DataSet.Tables[0].Rows.Count <= 0)
                return helpArray;

            helpArray[0][2] = Help_DataSet.Tables[0].Rows[0]["SRepCode"];
            helpArray[1][2] = Help_DataSet.Tables[0].Rows[0]["SRepName"];
            helpArray[2][2] = Help_DataSet.Tables[0].Rows[0]["STelephoneNo"];
            helpArray[3][2] = Help_DataSet.Tables[0].Rows[0]["SMobileNo"];

            return helpArray;
        }
    }
}

