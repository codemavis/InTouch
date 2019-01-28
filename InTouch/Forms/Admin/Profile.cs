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
using InTouch.Forms.Report;
using InTouch.SubForms;


namespace InTouch
{
    public partial class Profile : Form
    {
        double DchkUser = 0;
        double DchkProfile = 0;
        double DchkCourse = 0;
        double DchkSalesPerson = 0;
        double DchkStudent = 0;
        double DchkInquiry = 0;
        double DchkInvoice = 0;
        double DchkPayment = 0;
        double DchkInvoiceListing = 0;
        double DchkPaymentListing = 0;
        double DchkOutstanding = 0;
        double DchkSalesReport = 0;
        double DchkCourseWiseStudent = 0;
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void Profile_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private bool Validate_Entries() {

            if (txtCompanyName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Company");
                return false;
            }
            return true;
        }

        private bool Commit_Action()
        {
            DatabaseConnection newDbConn = new DatabaseConnection();

            newDbConn.OpenDBConnection();
            newDbConn.BeginTransaction();

            bool Saved = false;

            


            Saved = newDbConn.ExecuteQuery("UPDATE [dbo].[File_Profile] SET [SCompanyName] = '" + txtCompanyName.Text.Trim() + "',[SAddress1] = '" + txtAddress1.Text.Trim()
                + "',[SAddress2] = '" + txtAddress2.Text.Trim() + "',[SAddress3] = '" + txtAddress3.Text.Trim() + "',[STelephone1] = '" + txtTelephone.Text.Trim()
                + "',[STelephone2] = '" + txtMobile.Text.Trim() + "',[SEmail] = '" + txtEmail.Text.Trim() + "',[SWebsite] = '" + txtWebsite.Text.Trim() + "',[SNBTRegNo] = '"
                + txtNBT.Text.Trim() + "',[SVATRegNo] = '" + txtVAT.Text.Trim() + "',[SSVATRefNo] = '" + txtSVAT.Text.Trim() + "',[SMailPassword] = '" + txtMailPassword.Text.Trim()
                + "',[SFax] = '" + txtFax.Text.Trim() + "'   WHERE[SID] = '1'");

            if (Saved)
            {
                newDbConn.CommitTransaction();
                newDbConn.CloseDBConnection();

            }
            else
            {
                newDbConn.RollBackTransaction();
                newDbConn.CloseDBConnection();
            }

            return Saved;
        }
        private void LoadData()
        {
            DataSet newDataSet = null;

            DatabaseConnection dbCon = new DatabaseConnection();
            newDataSet = dbCon.Get("Select * from File_Profile");

            txtCompanyName.Text = newDataSet.Tables[0].Rows[0]["SCompanyName"].ToString();
            txtAddress1.Text = newDataSet.Tables[0].Rows[0]["SAddress1"].ToString();
            txtAddress2.Text = newDataSet.Tables[0].Rows[0]["SAddress2"].ToString();
            txtAddress3.Text = newDataSet.Tables[0].Rows[0]["SAddress3"].ToString();
            txtTelephone.Text = newDataSet.Tables[0].Rows[0]["STelephone1"].ToString();
            txtMobile.Text = newDataSet.Tables[0].Rows[0]["STelephone2"].ToString();
            txtEmail.Text = newDataSet.Tables[0].Rows[0]["SEmail"].ToString().Trim();
            txtFax.Text = newDataSet.Tables[0].Rows[0]["SFax"].ToString().Trim();
            txtWebsite.Text = newDataSet.Tables[0].Rows[0]["SWebsite"].ToString();
            txtMailPassword.Text = newDataSet.Tables[0].Rows[0]["SMailPassword"].ToString().Trim();

            if (chkUser.Checked)
                DchkUser = 1;
            else
                DchkUser = 0;

            if (chkProfile.Checked)
                DchkProfile = 1;
            else
                DchkProfile = 0;
            if (chkCourse.Checked)
                DchkCourse = 1;
            else
                DchkCourse = 0;
            if (chkSalesPerson.Checked)
                DchkSalesPerson = 1;
            else
                DchkSalesPerson = 0;
            if (chkStudent.Checked)
                DchkStudent = 1;
            else
                DchkStudent = 0;
            if (chkInquiry.Checked)
                DchkInquiry = 1;
            else
                DchkInquiry = 0;
            if (chkInvoice.Checked)
                DchkInvoice = 1;
            else
                DchkInvoice = 0;

            if (chkPayment.Checked)
                DchkPayment = 1;
            else
                DchkPayment = 0;
            if (chkInvoiceListing.Checked)
                DchkInvoiceListing = 1;
            else
                DchkInvoiceListing = 0;
            if (chkPaymentListing.Checked)
                DchkPaymentListing = 1;
            else
                DchkPaymentListing = 0;

            if (chkOutstanding.Checked)
                DchkOutstanding = 1;
            else
                DchkOutstanding = 0;

            if (chkSalesReport.Checked)
                DchkSalesReport = 0;
            else
                DchkSalesReport = 1;

            if (chkCoursewise.Checked)
                DchkCourseWiseStudent = 1;
            else
                DchkCourseWiseStudent = 0;



            txtNBT.Text = newDataSet.Tables[0].Rows[0]["SNBTRegNo"].ToString();
            txtVAT.Text = newDataSet.Tables[0].Rows[0]["SVATRegNo"].ToString();
            txtSVAT.Text = newDataSet.Tables[0].Rows[0]["SSVATRefNo"].ToString();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate_Entries())
                if (Commit_Action())
                {
                    MessageBox.Show("Updated Successfully!");
                }
                else
                    MessageBox.Show("Failed");

            LoadData();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet Report_DataSet = dbCon.Get_ReportData("Select * from File_Profile", "DT_Profile");
            try
            {
                Report_View openReport = new Report_View(GlobalVariables.ReportPath + "Rpt_CompanyProfile.rpt", Report_DataSet, "Profile");
                openReport.ShowInTaskbar = false;
                openReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
