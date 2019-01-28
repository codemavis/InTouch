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
    public partial class Outstanding_Report : Form
    {
        private string cMode;
        private DataSet DS_StudentRight, DS_StudentLeft;
        private DataSet DS_SalesPersonRight, DS_SalesPersonLeft;

        public string CMode { get => cMode; set => cMode = value; }

        public Outstanding_Report()
        {
            InitializeComponent();
        }

        private void Outstanding_Report_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string filterStudent = "";
            string filterSalesPerson = "";

            HelpFilter hp = new HelpFilter();

            if (!this.txtStudent.Text.Trim().Equals(""))
                filterStudent = "a.SStudentId = '" + this.txtStudent.Text.Trim() + "'";
            else
                filterStudent = hp.Get_Filter("a.SStudentId", DS_StudentRight);

            if (!this.txtRepCode.Text.Trim().Equals(""))
                filterSalesPerson = "SRepCode = '" + this.txtRepCode.Text.Trim() + "'";
            else
                filterSalesPerson = hp.Get_Filter("SRepCode", DS_SalesPersonRight);

            //validate
            if (!filterStudent.Trim().Equals(""))
                filterStudent = " and " + filterStudent;

            if (!filterSalesPerson.Trim().Equals(""))
                filterSalesPerson = " and " + filterSalesPerson;

            string filterDate = " and a.DDate>='" + txtDateFrom.Value.ToShortDateString() + "' and a.DDate <= '" + txtDateTo.Value.ToShortDateString() + "'";

            DatabaseConnection dbCon = new DatabaseConnection();
            string cQuery = "";
            DataSet Outstanding_ReportDataSet;
            Report_View newReport;

            if (this.radioButton1.Checked)
            {
                cQuery = "select Convert(Date,DDate) as DDate,SName,SRefNo,NTotalAmount,NBalanceAmount,000000000000.00 as NAmount, "
                   + " 'I' as T_Type from File_InvoiceH a,File_Student c where a.SStudentId = c.SStudentId "
                   + " and NBalanceAmount> 0.00 "+ filterDate + " " + filterStudent + " " + filterSalesPerson 
                   + " union Select DDate,SName,SRefNo,0 as NTotalAmount, "
                   + " 0 as NBalanceAmount,a.NAmount,'P' as T_Type from File_PaymentH a, "
                   + " File_Student d where d.SStudentId = a.SStudentId "+ filterDate + " " + filterStudent + " " + " order by DDate";

                Outstanding_ReportDataSet = dbCon.Get_ReportData(cQuery, "DT_Outstanding_Detail");

                newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Outstanding_Detail.rpt", Outstanding_ReportDataSet, "Payment Listing");
            }
            else
            {
                cQuery = "select SName,SRefNo,NTotalAmount,NBalanceAmount from File_InvoiceH a,File_Student c "
                   + " where a.SStudentId = c.SStudentId and NBalanceAmount> 0.00 "+ filterDate + " " + filterStudent + " " + filterSalesPerson;


                Outstanding_ReportDataSet = dbCon.Get_ReportData(cQuery, "DT_Outstanding");

                newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Outstanding_Summary.rpt", Outstanding_ReportDataSet, "Payment Listing");
            }

            if (Outstanding_ReportDataSet.Tables[0].Rows.Count > 0)
            {
                newReport.ShowInTaskbar = false;
                newReport.ShowDialog();
            }
            else
                MessageBox.Show("No Data To Print");

            //if (radioButton1.Checked && !this.txtStudent.Text.Trim().Equals(""))
            //{
            //    //Email
            //    DialogResult dRes = MessageBox.Show("Do You Want To Email This Report?", "Confirm", MessageBoxButtons.YesNo);
            //    if (dRes == DialogResult.Yes)
            //    {
            //        string studentId = this.txtStudent.Text.Trim();
            //        SendEmail newMail = new SendEmail("Rpt_Outstanding_Detail.pdf", newReport,studentId);
            //        newMail.ShowInTaskbar = false;
            //        newMail.ShowDialog();
            //    }
            //    //
            //}
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
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

        private void Outstanding_Report_Load(object sender, EventArgs e)
        {
            Load_Empty_DataSet();
        }

        private void Load_Empty_DataSet()
        {

            DatabaseConnection dbCon = new DatabaseConnection();

            DS_StudentRight = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student Where 1=0");
            DS_SalesPersonRight = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson Where 1=0");

            DS_StudentLeft = dbCon.Get("Select SStudentId as Student_ID,SName as Student_Name from File_Student");
            DS_SalesPersonLeft = dbCon.Get("Select SRepCode as SalesPerson,SRepName as SalesPerson_Name from File_SalesPerson");
        }
    }
}
