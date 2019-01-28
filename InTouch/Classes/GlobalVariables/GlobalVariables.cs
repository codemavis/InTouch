using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace InTouch.Classes.GlobalVariables
{
    static class GlobalVariables
    {
        public static DataSet HelpDataSet;
        public static DataSet HelpDataSetLeft;
        public static DataSet HelpDataSetRight;
        public static DataSet HelpDataSetMain;
        public static DataSet HelpDataSetTemp;

        private static string UserName;

        public static string GetUserName()
        {
            return UserName.ToUpper();
        }
        public static void SetUserName(string value)
        {
            UserName = value;
        }

        public static DataTable DT_HelpCourse = new DataTable("HelpCourse");

        public static DataSet Help_DataSet_New = new DataSet("Help");

        public static string ReportPath;
        public static string AssetPath;
    }
}
