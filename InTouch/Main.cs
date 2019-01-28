using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTouch.Classes.Connection;
using InTouch.Classes.Effects;
using InTouch.Classes.FormControl;
using InTouch.Classes.GlobalVariables;
using InTouch.Forms.Admin;
using InTouch.Forms.SubForms;

namespace InTouch
{
    public partial class Main : Form
    {
        Form CXForm;
        OpenForm openForm;
        public Main(Form XForm)
        {
            InitializeComponent();
            CXForm = XForm;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Opacity = 0;

            Fade fade = new Fade();
            fade.formLoad(this, CXForm);

            //
            string currentPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            
            GlobalVariables.ReportPath = currentPath + @"\InTouch_Files\Reports\Crystal\";
            GlobalVariables.AssetPath = currentPath + @"\InTouch_Files\Assets\";
            //GlobalVariables.ReportPath = @"C:\InTouch\Reports\Crystal\";
            //GlobalVariables.AssetPath = @"C:\InTouch\Assets\";

            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;

            this.labelUserName.Text = "   " + GlobalVariables.GetUserName();

            userAccess_validate();

            openForm = new OpenForm();
            
            this.open_Notification();
        }

        private void userAccess_validate() {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet accessDS = dbCon.Get("Select chkUser,chkProfile,chkCourse,chkSalesPerson,chkStudent,chkInquiry,chkInvoice,chkPayment,chkInvoiceListing,"
                + "chkPaymentListing, chkOutstanding, chkSalesReport, chkCourseWiseStudent, chkPayList from File_User where SUserName='"+labelUserName.Text.Trim()+"' ");

            if (accessDS.Tables[0].Rows[0]["chkUser"].ToString().Trim().Equals("0"))
                this.btnUser.Enabled = false;

            if (accessDS.Tables[0].Rows[0]["chkProfile"].ToString().Trim().Equals("0"))
                this.btnUser.Enabled = false;

            if (accessDS.Tables[0].Rows[0]["chkCourse"].ToString().Trim().Equals("0"))
                this.btnUser.Enabled = false;

            if (accessDS.Tables[0].Rows[0]["chkSalesPerson"].ToString().Trim().Equals("0"))
                this.btnSalesRep.Enabled = false;

            if (accessDS.Tables[0].Rows[0]["chkStudent"].ToString().Trim().Equals("0"))
                this.btnStudent.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkInquiry"].ToString().Trim().Equals("0"))
                this.btnInquiry.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkInvoice"].ToString().Trim().Equals("0"))
                this.btnInvoice.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkPayment"].ToString().Trim().Equals("0"))
                this.btnPayment.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkInvoiceListing"].ToString().Trim().Equals("0"))
                this.btnInvoiceListing.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkPaymentListing"].ToString().Trim().Equals("0"))
                this.btnPaymentListing.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkOutstanding"].ToString().Trim().Equals("0"))
                this.btnOutstanding.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkSalesReport"].ToString().Trim().Equals("0"))
                this.btnSalesReport.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkCourseWiseStudent"].ToString().Trim().Equals("0"))
                this.btnItemWiseCustomer.Enabled = false;
            if (accessDS.Tables[0].Rows[0]["chkPayList"].ToString().Trim().Equals("0"))
                this.btnStudentPayment.Enabled = false;
        }

        private void open_Notification()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            string str = " Select c.SName,d.SCourseName,iif(max(f.SRefNo) is Null, '', max(f.SRefNo)) as Payment_RefNo, "
                + " iif(max(f.DDate) is null,'',max(f.ddate)) as Payment_Date,NBalanceAmount as Balance_Amount, "
                + " (datediff(DAY,max(f.DDate),getdate())-1-((count(e.SRefNo))*NPayInDays)) as Overdue_Days from "
                + " File_InvoiceH a inner join File_InvoiceD b on a.SRefNo=b.SRefNo inner join File_Student c "
                + " on c.SStudentId=a.SStudentId inner join File_Course d on d.SCourseCode=b.SCourseCode "
                + " left join File_PaymentD e on e.SInvNo=a.SRefNo left join File_PaymentH f on f.SRefNo=e.SRefNo "
                + " where a.NBalanceAmount>0.00 group by a.SRefNo,a.DDate,a.SStudentId,c.SName,NBalanceAmount,NPayInDays, "
                + " d.SCourseName,SCourseName having (datediff(DAY,max(f.DDate),getdate()))-1 > ((count(e.SRefNo))*NPayInDays) ";
            DataSet newDataSet = dbCon.Get(str);

            if (newDataSet.Tables[0].Rows.Count > 0)
            {
                Notification newForm = new Notification(newDataSet);
                openForm.openChildForm(newForm, this, "Notification");
            }


            //Notification newForm = new Notification();
            //newForm.ShowInTaskbar = false;
            //newForm.MdiParent = this;
            //newForm.Show();
        }

        private void Display_Notification()
        {

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dRes = MessageBox.Show("Do You Want To Exit?", "Confirm", MessageBoxButtons.YesNo);
            if (dRes == DialogResult.Yes)
                Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (Width != Screen.PrimaryScreen.WorkingArea.Width)
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width;
                Height = Screen.PrimaryScreen.WorkingArea.Height;
            }
            else
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width / 2;
                Height = Screen.PrimaryScreen.WorkingArea.Height / 2;
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            Profile newForm = new Profile();
            openForm.openChildForm(newForm, this, "Profile");
        }

        private void btnOutstanding_Click(object sender, EventArgs e)
        {
            Outstanding_Report newForm = new Outstanding_Report();
            openForm.openChildForm(newForm, this, "Outstanding_Report");
        }

        private void btnSalesRep_Click(object sender, EventArgs e)
        {
            SalesPerson newForm = new SalesPerson();
            openForm.openChildForm(newForm, this, "SalesPerson");
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            Invoice newForm = new Invoice();
            openForm.openChildForm(newForm, this, "Invoice");
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Payment newForm = new Payment();
            openForm.openChildForm(newForm, this, "Payment");
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            Course newForm = new Course();
            openForm.openChildForm(newForm, this, "Course");
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            Student newForm = new Student();
            openForm.openChildForm(newForm, this, "Student");
        }

        private void btnInvoiceListing_Click(object sender, EventArgs e)
        {
            Invoice_Listing newForm = new Invoice_Listing();
            openForm.openChildForm(newForm, this, "Invoice_Listing");
        }

        private void btnPaymentListing_Click(object sender, EventArgs e)
        {
            Payment_Listing newForm = new Payment_Listing();
            openForm.openChildForm(newForm, this, "Payment_Listing");
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            Sales_Report newForm = new Sales_Report();
            openForm.openChildForm(newForm, this, "Sales_Report");
        }
        private void btnUser_Click(object sender, EventArgs e)
        {
            User newForm = new User();
            openForm.openChildForm(newForm, this, "User");
        }

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            Inquiry newForm = new Inquiry();
            openForm.openChildForm(newForm, this, "Inquiry");
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            this.open_Notification();
        }

        private void panelHeader_DoubleClick(object sender, EventArgs e)
        {
            if (Width != Screen.PrimaryScreen.WorkingArea.Width)
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width;
                Height = Screen.PrimaryScreen.WorkingArea.Height;
            }
            else
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width / 2;
                Height = Screen.PrimaryScreen.WorkingArea.Height / 2;
            }
        }
        private void btnItemWiseCustomer_Click(object sender, EventArgs e)
        {
            Student_Course newForm = new Student_Course();
            openForm.openChildForm(newForm, this, "Student_Course");
        }

        private void btnStudentPayment_Click(object sender, EventArgs e)
        {
            Student_Payment newForm = new Student_Payment();
            openForm.openChildForm(newForm, this, "Student_Payment");
        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            try
            {
                Login newLogin = new Login();
                this.Close();
                newLogin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string path, str;
            DatabaseConnection dbCon = new DatabaseConnection();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.SelectedPath + "\\";
                try
                {
                    dbCon.OpenDBConnection();

                    str = "declare @date nvarchar(150), @str nvarchar(150) select "
                        + " @date = replace(convert(char(20),getdate()),':',' ') set @str = "
                        + " rtrim('" + path + "InTouch - '+@date) backup database InTouch "
                        + " to disk = @str";

                    SqlCommand command = new SqlCommand(str, dbCon.Con);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dbCon.CloseDBConnection();
                }
            }
        }

        private void btnLectureActivity_Click(object sender, EventArgs e)
        {
            Lecture_Activity newForm = new Lecture_Activity();
            openForm.openChildForm(newForm, this, "Lecture_Activity");
        }
    }
}

