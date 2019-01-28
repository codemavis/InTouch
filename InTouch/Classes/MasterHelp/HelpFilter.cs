using InTouch.Classes.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InTouch.Classes
{
    
    class HelpFilter
    {
        private string ssName;
        public string Get_Filter(string cName, DataSet DS_Filter)
        {
            string cFilter = "(";
            for (int i = 0; i < DS_Filter.Tables[0].Rows.Count; i++)
            {
                cFilter += cName + "='" + DS_Filter.Tables[0].Rows[i][0].ToString().Trim() + "' OR ";
            }
            if (cFilter.Length > 3)
                cFilter = cFilter.Remove(cFilter.Length - 3);

            cFilter += ")";

            if (cFilter.Trim().Length < 5)
                cFilter = "";

            return cFilter;

        }
    }
}
