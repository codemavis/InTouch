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
using InTouch.Forms.SubForms;
using InTouch.Classes;
using InTouch.Classes.MasterHelp;

namespace InTouch
{
    public partial class Invoice_Listing : Form
    {
        private DataSet DS_StudentRight, DS_StudentLeft;
        private DataSet DS_SalesPersonRight, DS_SalesPersonLeft;
        private DataSet DS_CourseRight, DS_CourseLeft;

        public Invoice_Listing()
        {
            InitializeComponent();
        }
        private void Invoice_Listing_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            HelpFilter hp = new HelpFilter();

            string filterStudent = "";
            string filterCourse = "";
            string filterSalesPerson = "";

            if (!this.txtStudent.Text.Trim().Equals(""))
                filterStudent = "a.SStudentId = '" + this.txtStudent.Text.Trim() + "'";
            else
                filterStudent = hp.Get_Filter("a.SStudentId", DS_StudentRight);

            if (!this.txtCourse.Text.Trim().Equals(""))
                filterCourse = "b.SCourseCode = '" + this.txtCourse.Text.Trim() + "'";
            else
                filterCourse = hp.Get_Filter("b.SCourseCode", DS_CourseRight);

            if (!this.txtRepCode.Text.Trim().Equals(""))
                filterSalesPerson = "e.SRepCode = '" + this.txtRepCode.Text.Trim() + "'";
            else
                filterSalesPerson = hp.Get_Filter("e.SRepCode", DS_SalesPersonRight);

            //validate
            if (!filterStudent.Trim().Equals(""))
                filterStudent = " and " + filterStudent;

            if (!filterCourse.Trim().Equals(""))
                filterCourse = " and " + filterCourse;

            if (!filterSalesPerson.Trim().Equals(""))
                filterSalesPerson = " and " + filterSalesPerson;

            DatabaseConnection dbCon = new DatabaseConnection();
            string cQuery = "select a.SRefNo,a.DDate,a.SStudentId,d.SName,b.SCourseCode,c.SCourseName,b.NAmount,"
                + "b.NDiscount,iif(SRepName is null, 'NONE', SRepName) as SRepName,a.SAddUser,a.DAddDate from "
                + " File_InvoiceH a inner join File_InvoiceD b on a.SRefNo = b.SRefNo inner join File_Course c on "
                + "b.SCourseCode = c.SCourseCode inner join File_Student d on d.SStudentId = a.SStudentId "
                + "left join File_SalesPerson e on e.SRepCode = a.SRepCode where a.DDate>='" + txtDateFrom.Value.ToShortDateString() + "' "
                + " and a.DDate<='" + txtDateTo.Value.ToShortDateString() + "' " + filterStudent + "  "
                + filterCourse + "  " + filterSalesPerson ;

            DataSet Invoice_ListingDataSet = dbCon.Get_ReportData(cQuery, "DT_InvoiceListing");

            if (Invoice_ListingDataSet.Tables[0].Rows.Count > 0)
            {
                Report_View newReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Invoice_Listing.rpt", Invoice_ListingDataSet, "Invoice Listing");
                newReport.ShowInTaskbar = false;
                newReport.ShowDialog();
            }
            else
                MessageBox.Show("No Data To Print");
        }

        private string Get_ClauseFilter(string cQuery, string cFilter)
        {
            if (cQuery.ToUpper().Contains("WHERE"))
                return cQuery + " and " + cFilter;
            else
                return cQuery + " where " + cFilter;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtStudent_DoubleClick(object sender, EventArgs e)
        {

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SAddress1,rtrim(SAddress2)+', '+ltrim(SAddress3) as SAddress2 from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtStudent.Text = newFind.FindKeyValue;
        }

        private void txtStudent_TextChanged(object sender, EventArgs e)
        {
            object[][] HelpArray = new object[4][];

            HelpArray[0] = new object[3];
            HelpArray[0][0] = "";
            HelpArray[0][1] = "SStudentId";
            HelpArray[0][2] = "";

            HelpArray[1] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[1][1] = "SName";
            HelpArray[1][2] = "";

            HelpArray[2] = new object[3];
            HelpArray[1][0] = "";
            HelpArray[2][1] = "SAddress1";
            HelpArray[2][2] = "";

            HelpArray[3] = new object[3];
            HelpArray[3][0] = "";
            HelpArray[3][1] = "SAddress2";
            HelpArray[3][2] = "";

            MasterHelp mHelp = new MasterHelp();
            string SWhere = "Where SStudentId='" + txtStudent.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_Student(HelpArray, SWhere);

            this.txtStudent.Text = HelpArray[0][2].ToString();
            this.txtSName.Text = HelpArray[1][2].ToString();
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

        private void btnStudent_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student");
            GlobalVariables.HelpDataSetMain = FillData;

            FindRange newFind = new FindRange("Student", DS_StudentRight, DS_StudentLeft);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            if (DS_StudentRight.Tables[0].Rows.Count > 0)
                txtStudent.Text = "";

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
        }

        private void Invoice_Listing_Load(object sender, EventArgs e)
        {
            Load_Empty_DataSet();
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SCourseCode as Course_Code,SCourseName as Course_Name from file_Course");
            GlobalVariables.HelpDataSetMain = FillData;

            FindRange newFind = new FindRange("Course", DS_CourseRight, DS_CourseLeft);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            if (DS_CourseRight.Tables[0].Rows.Count > 0)
                txtCourse.Text = "";
        }

        private void btnSalesPerson_Click(object sender, EventArgs e)
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

        private void Load_Empty_DataSet()
        {
            DatabaseConnection dbCon = new DatabaseConnection();

            DS_StudentRight = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student Where 1=0");
            DS_CourseRight = dbCon.Get("Select SCourseCode as Course_Code,SCourseName as Course_Name from file_Course Where 1=0");
            DS_SalesPersonRight = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson Where 1=0");

            DS_StudentLeft = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student");
            DS_CourseLeft = dbCon.Get("Select SCourseCode Course_Code, SCourseName as CourseName from File_Course");
            DS_SalesPersonLeft = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson");
        }
        private void txtDateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (txtDateFrom.Value > txtDateTo.Value)
            {
                MessageBox.Show("Date To Cannot Be Greater Than The Date From");
                txtDateTo.Value = txtDateFrom.Value;
            }
        }

        private void txtDateTo_ValueChanged(object sender, EventArgs e)
        {
            if (txtDateTo.Value < txtDateFrom.Value)
            {
                MessageBox.Show("Date To Cannot Be Lesser Than The Date From");
                txtDateFrom.Value = txtDateTo.Value;
            }
        }
    }
}