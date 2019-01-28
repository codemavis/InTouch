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
using InTouch.Forms.SubForms;
using InTouch.Classes;

namespace InTouch
{
    public partial class Student_Course : Form
    {
        private DataSet DS_StudentRight, DS_CourseRight;
        private DataSet DS_StudentLeft, DS_CourseLeft;

        public Student_Course()
        {
            InitializeComponent();
        }
        private void Student_Course_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            HelpFilter hp = new HelpFilter();

            string filterStudent = "";
            string filterCourse = "";

            if (!this.txtStudent.Text.Trim().Equals(""))
                filterStudent = "a.SStudentId = '" + this.txtStudent.Text.Trim() + "'";
            else
                filterStudent = hp.Get_Filter("a.SStudentId", DS_StudentRight);

            if (!this.txtCourse.Text.Trim().Equals(""))
                filterCourse = "c.SCourseCode = '" + this.txtCourse.Text.Trim() + "'";
            else
                filterCourse = hp.Get_Filter("c.SCourseCode", DS_CourseRight);

            //validate
            if (!filterStudent.Trim().Equals(""))
                filterStudent = " and " + filterStudent;

            if (!filterCourse.Trim().Equals(""))
                filterCourse = " and " + filterCourse;

            string filterDate = " and a.DDate>='" + txtDateFrom.Value.ToShortDateString() + "' and a.DDate <= '" + txtDateTo.Value.ToShortDateString() + "'";

            DatabaseConnection dbCon = new DatabaseConnection();
            string cQuery;

            if (this.radioButton1.Checked)
            cQuery = "Select c.SCourseCode,SCourseName,a.SStudentId,SName,STelNo,SMobileNo,SEmail "
                + " from File_InvoiceH a,File_InvoiceD e, File_Student b,File_Course c  where "
                + " a.SStudentId = b.SStudentId and c.SCourseCode = e.SCourseCode and a.SRefNo = e.SRefNo "
                + " and a.NBalanceAmount > 0 " + filterDate + " " + filterStudent + " " + filterCourse;
            else
                cQuery = "Select c.SCourseCode,SCourseName,a.SStudentId,SName,STelNo,SMobileNo,SEmail "
                + " from File_InvoiceH a,File_InvoiceD e, File_Student b,File_Course c  where "
                + " a.SStudentId = b.SStudentId and c.SCourseCode = e.SCourseCode and a.SRefNo = e.SRefNo "
                + filterDate + " " + filterStudent + " " + filterCourse;

            DataSet Student_CourseDataSet = dbCon.Get_ReportData(cQuery, "DT_StudentCourse");

            Report_View newReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Student_Course.rpt", Student_CourseDataSet, "Invoice Listing");

            if (Student_CourseDataSet.Tables[0].Rows.Count > 0)
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

        private void btnCourse_Click(object sender, EventArgs e)
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

        private void Student_Course_Load(object sender, EventArgs e)
        {
            Load_Empty_DataSet();
        }

        private void Load_Empty_DataSet()
        {

            DatabaseConnection dbCon = new DatabaseConnection();

            DS_StudentRight = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student Where 1=0");
            DS_CourseRight = dbCon.Get("Select SCourseCode as Item_Code,SCourseName as Item_Name from file_Course Where 1=0");

            DS_StudentLeft = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student");
            DS_CourseLeft = dbCon.Get("Select SCourseCode as Item_Code,SCourseName as Item_Name from file_Course");
        }
    }
}