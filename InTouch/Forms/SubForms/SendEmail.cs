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

namespace InTouch.SubForms
{
    public partial class SendEmail : Form
    {
        public string SendEmailKeyValue;
        DataTable DT_Grid;
        Report_View newReport;
        string fileName;
        private string cTo;
        private string cCC;
        private string cBCC;
        private string cSubject;
        private string cBody;
        private string userEmail;
        private string userPass;
        private string studentId;

        public SendEmail(string FileName, Report_View NewReport,string studentId)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            newReport = NewReport;
            fileName = FileName;
            this.labelAttachment.Text = FileName;
            this.studentId = studentId;


        }

        private void getEmail()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet emailDS = dbCon.Get("Select SEmail,SMailPassword from File_Profile");
            if (emailDS.Tables[0].Rows.Count > 0)
            {
                userEmail = emailDS.Tables[0].Rows[0][0].ToString().Trim();
                this.labelEmail.Text = userEmail;
                userPass = emailDS.Tables[0].Rows[0][1].ToString().Trim();
            }

            DataSet emailStd = dbCon.Get("Select SEmail from file_student where SStudentId='"+ studentId + "'");
            if (emailStd.Tables[0].Rows.Count > 0)
                this.txtTo.Text = emailStd.Tables[0].Rows[0][0].ToString().Trim();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cTo = txtTo.Text.Trim();
            cCC = txtCC.Text.Trim();
            cBCC = txtBCC.Text.Trim();
            cSubject = txtSubject.Text.Trim();
            cBody = txtBody.Text.Trim();

            if (newReport.Report_Export("Rpt_Student_Payment.pdf"))
            {
                Send_Email newEmail = new Send_Email();
                newEmail.sendEmail(fileName, cTo, cCC, cBCC, cSubject,cBody, userEmail, userPass);
              //  MessageBox.Show("Email Sent!");
                this.Close();
            }
            else
                MessageBox.Show("Sent Failed!");



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SendEmail_Load(object sender, EventArgs e)
        {
            getEmail();
        }
    }
}
