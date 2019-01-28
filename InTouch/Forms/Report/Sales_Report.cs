using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTouch.Classes.Connection;
using InTouch.Classes.GlobalVariables;
using InTouch.SubForms;
using System.Net;
using InTouch.Forms.Report;
using InTouch.Classes.MasterHelp;
using InTouch.Classes;
using InTouch.Forms.SubForms;

namespace InTouch
{
    public partial class Sales_Report : Form
    {
        private string cMode;
        private DataSet DS_SalesPersonRight, DS_SalesPersonLeft;
        private DataSet DS_CourseRight, DS_CourseLeft;

        public string CMode { get => cMode; set => cMode = value; }

        public Sales_Report()
        {
            InitializeComponent();
        }

        private void Sales_Report_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Sales_Report_Load(object sender, EventArgs e)
        {
            Load_Empty_DataSet();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            HelpFilter hp = new HelpFilter();

            string filterCourse = "";
            string filterSalesPerson = "";

            if (!this.txtCourse.Text.Trim().Equals(""))
                filterCourse = "c.SCourseCode = '" + this.txtCourse.Text.Trim() + "'";
            else
                filterCourse = hp.Get_Filter("c.SCourseCode", DS_CourseRight);

            if (!this.txtRepCode.Text.Trim().Equals(""))
                filterSalesPerson = "b.SRepCode = '" + this.txtRepCode.Text.Trim() + "'";
            else
                filterSalesPerson = hp.Get_Filter("b.SRepCode", DS_SalesPersonRight);

            //validate
            if (!filterCourse.Trim().Equals(""))
                filterCourse = " Where " + filterCourse;

            if (!filterSalesPerson.Trim().Equals(""))
                filterSalesPerson = " and " + filterSalesPerson;

            string filterDate = " and a.DDate>='" + txtDateFrom.Value.ToShortDateString() + "' and a.DDate <= '" + txtDateTo.Value.ToShortDateString() + "'";

            DatabaseConnection dbCon = new DatabaseConnection();
            string cQuery = "select c.SCourseName,iif(SRepName is null,'NONE',SRepName) as SRepName,sum(d.NAmount) as NAmount, "
                + " sum(d.NDiscount) as NDiscount from File_InvoiceH a inner join File_InvoiceD d on a.SRefNo = d.SRefNo "
                + " left join File_SalesPerson b on a.SRepCode = b.SRepCode inner join File_Course c on c.SCourseCode = "
                + " d.SCourseCode  " + filterCourse + " " + filterSalesPerson + " " + filterDate + " Group By SCourseName, SRepName";

            DataSet Sales_ReportDataSet = dbCon.Get_ReportData(cQuery, "DT_SalesReport");
            Report_View newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Sales_Report.rpt", Sales_ReportDataSet, "Payment Listing");

            if (Sales_ReportDataSet.Tables[0].Rows.Count > 0)
            {
                newReport.ShowInTaskbar = false;
                newReport.ShowDialog();
            }
            else
                MessageBox.Show("No Valid Data To Print");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCourse_TextChanged(object sender, EventArgs e)
        {
            object[][] HelpArray = new object[4][];

            HelpArray[0] = new object[3];
            HelpArray[0][0] = "";
            HelpArray[0][1] = "SCourseCode";
            HelpArray[0][2] = "";

            HelpArray[1] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[1][1] = "SCourseName";
            HelpArray[1][2] = "";

            HelpArray[2] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[2][1] = "NAmount";
            HelpArray[2][2] = "";

            HelpArray[3] = new object[3];
            HelpArray[3][0] = "";
            HelpArray[3][1] = "NCreditPeriod";
            HelpArray[3][2] = "";

            MasterHelp mHelp = new MasterHelp();
            string SWhere = "Where SCourseCode='" + txtCourse.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_Course(HelpArray, SWhere);

            this.txtCourse.Text = HelpArray[0][2].ToString();
            this.txtCourseName.Text = HelpArray[1][2].ToString();
        }

        private void txtCourse_DoubleClick(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SCourseCode,SCourseName,NAmount,NCreditPeriod from File_Course");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Course", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtCourse.Text = newFind.FindKeyValue;
        }

        private void txtRepCode_DoubleClick(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRepCode,SRepName,STelephoneNo,SMobileNo,SEmail from File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Sales Person", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtRepCode.Text = newFind.FindKeyValue;
        }

        private void txtRepCode_TextChanged(object sender, EventArgs e)
        {
            object[][] HelpArray = new object[4][];

            HelpArray[0] = new object[3];
            HelpArray[0][0] = "";
            HelpArray[0][1] = "SRepCode";
            HelpArray[0][2] = "";

            HelpArray[1] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[1][1] = "SRepName";
            HelpArray[1][2] = "";

            HelpArray[2] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[2][1] = "STelephoneNo";
            HelpArray[2][2] = "";

            HelpArray[3] = new object[3];
            HelpArray[3][0] = "";
            HelpArray[3][1] = "SMobileNo";
            HelpArray[3][2] = "";

            MasterHelp mHelp = new MasterHelp();
            string SWhere = "Where SRepCode='" + txtRepCode.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_SalesPerson(HelpArray, SWhere);

            this.txtRepCode.Text = HelpArray[0][2].ToString();
            this.txtRepName.Text = HelpArray[1][2].ToString();
        }

        private void txtDateTo_Leave(object sender, EventArgs e)
        {
            if (txtDateTo.Value < txtDateFrom.Value)
            {
                MessageBox.Show("Date To Cannot Be Lesser Than The Date From");
                txtDateTo.Value = txtDateFrom.Value;
            }
        }

       

        private void txtDateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (txtDateFrom.Value > txtDateTo.Value)
            {
                MessageBox.Show("Date To Cannot Be Greater Than The Date From");
                txtDateTo.Value = txtDateFrom.Value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson");
            GlobalVariables.HelpDataSetMain = FillData;

            FindRange newFind = new FindRange("Sales Person", DS_SalesPersonRight, DS_SalesPersonLeft);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            if (DS_SalesPersonRight.Tables[0].Rows.Count > 0)
                txtRepCode.Text = "";
        }

        private void txtDateTo_ValueChanged(object sender, EventArgs e)
        {
            if (txtDateTo.Value < txtDateFrom.Value)
            {
                MessageBox.Show("Date To Cannot Be Lesser Than The Date From");
                txtDateFrom.Value = txtDateTo.Value;
            }
        }
        private void Load_Empty_DataSet()
        {
            DatabaseConnection dbCon = new DatabaseConnection();

            DS_CourseRight = dbCon.Get("Select SCourseCode as Item_Code,SCourseName as Item_Name from file_Course Where 1=0");
            DS_SalesPersonRight = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson Where 1=0");

            DS_CourseLeft = dbCon.Get("Select SCourseCode Item_Code, SCourseName as ItemName from File_Course");
            DS_SalesPersonLeft = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SCourseCode as Item_Code,SCourseName as Item_Name from file_Course");
            GlobalVariables.HelpDataSetMain = FillData;

            FindRange newFind = new FindRange("Course", DS_CourseRight, DS_CourseLeft);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            if (DS_CourseRight.Tables[0].Rows.Count > 0)
                txtCourse.Text = "";
        }
    }
}
