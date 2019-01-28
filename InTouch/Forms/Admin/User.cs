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
    public partial class User : Form
    {
        private string cMode;
        private string userName;
        private string fullName;
        private string oldPassword;
        private string contactNo;

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
        double DchkPayList = 0;

        public string CMode { get => cMode; set => cMode = value; }
        public string UserName { get => userName; set => userName = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string OldPassword { get => oldPassword; set => oldPassword = value; }
        public string ContactNo { get => contactNo; set => contactNo = value; }

        public DataSet UserDataSet;

        public User()
        {
            InitializeComponent();
        }

        private void User_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void User_Load(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            UserDataSet = dbCon.Get("Select SUserName,SFullName,SPassword,SContactNo,chkUser,chkProfile,chkCourse,chkSalesPerson,"
                + "chkStudent,chkInquiry,chkInvoice,chkPayment,chkInvoiceListing,chkPaymentListing, chkOutstanding, chkSalesReport, "
                + "chkCourseWiseStudent, chkPayList from File_User WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
        }

        private bool Validate_Entries()
        {
            if (txtUserName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Username!");
                return false;
            }
            if (txtNewPassword.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Password!");
                return false;
            }
            if (!txtNewPassword.Text.Trim().Equals(txtConfirmPassword.Text.Trim()))
            {
                MessageBox.Show("Passwords doesn't match!");
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

            if (CMode.Equals("NEW"))
                Saved = SaveData(newDbConn);

            if (cMode.Equals("EDIT"))
            {
                if (DeleteData(newDbConn))
                    Saved = SaveData(newDbConn);
            }

            if (cMode.Equals("DELETE"))
            {
                Saved = DeleteData(newDbConn);
            }

            if (Saved)
            {
                newDbConn.CommitTransaction();
                newDbConn.CloseDBConnection();

                this.CMode = "INIT";
                InitProperties(cMode);
                DisableAllButtons();
                LoadData();
                ButtonControl();
            }
            else
            {
                newDbConn.RollBackTransaction();
                newDbConn.CloseDBConnection();
            }

            return Saved;
        }
        private void InitProperties(string cMode)
        {
            if (cMode.Equals("INIT"))
            {
                this.txtUserName.Enabled = true;
                this.txtFullName.ReadOnly = true;
                this.txtOldPassword.ReadOnly = true;
                this.txtNewPassword.ReadOnly = true;
                this.txtConfirmPassword.ReadOnly = true;
                this.txtContactNo.ReadOnly = true;
            }
            if (cMode.Equals("DELETE"))
            {
                this.txtUserName.Enabled = true;
                this.txtFullName.ReadOnly = true;
                this.txtOldPassword.ReadOnly = false;
                this.txtNewPassword.ReadOnly = true;
                this.txtConfirmPassword.ReadOnly = true;
                this.txtContactNo.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                this.txtUserName.Enabled = true;
                this.txtFullName.ReadOnly = false;
                this.txtOldPassword.ReadOnly = true;
                this.txtNewPassword.ReadOnly = false;
                this.txtConfirmPassword.ReadOnly = false;
                this.txtContactNo.ReadOnly = false;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtUserName.Enabled = false;
                this.txtFullName.ReadOnly = false;
                this.txtOldPassword.ReadOnly = false;
                this.txtNewPassword.ReadOnly = false;
                this.txtConfirmPassword.ReadOnly = false;
                this.txtContactNo.ReadOnly = false;
            }
        }

        private bool DeleteData(DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("DELETE FROM File_User where SUserName = '" + txtUserName.Text.Trim() + "'");
        }

        private bool SaveData(DatabaseConnection newDbConn)
        {
            Fill_DataSet(UserDataSet);
            if (newDbConn.Upd(UserDataSet, "File_User"))
            {
                //if (picStudent.Image != null)
                //{
                //    string fileName = this.txtCode.Text.Trim() + ".jpg";
                //    string folder = @"D:\Projects\DotNet\InTouch\InfiNet\Assets\Images\Student-Images\";
                //    string pathString = System.IO.Path.Combine(folder, fileName);
                //    Image a = picStudent.Image;
                //    a.Save(pathString);

                //    newDbConn.ExecuteQuery("Update File_Student Set SPicPath='" + pathString + "' where SStudentId='" + txtCode.Text + "'");
                //}
                return true;
            }
            else
                return false;
        }

        private void Fill_DataSet(DataSet FillDataSet)
        {
            if (FillDataSet.Tables[0].Rows.Count > 0)
                FillDataSet.Tables[0].Rows.Clear();

            //
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
                DchkSalesReport = 1;
            else
                DchkSalesReport = 0;

            if (chkCoursewise.Checked)
                DchkCourseWiseStudent = 1;
            else
                DchkCourseWiseStudent = 0;

            if (chkPayList.Checked)
                DchkPayList = 1;
            else
                DchkPayList = 0;
            
            //

            FillDataSet.Tables[0].Rows.Add();
            FillDataSet.Tables[0].Rows[0]["SUserName"] = txtUserName.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SFullName"] = txtFullName.Text.Trim();
            //FillDataSet.Tables[0].Rows[0]["SPassword"] = Cryptography.Encrypt(txtConfirmPassword.Text.Trim());
            FillDataSet.Tables[0].Rows[0]["SPassword"] = txtConfirmPassword.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SContactNo"] = txtContactNo.Text.Trim();

            FillDataSet.Tables[0].Rows[0]["chkUser"] = DchkUser;
            FillDataSet.Tables[0].Rows[0]["chkProfile"] = DchkProfile;
            FillDataSet.Tables[0].Rows[0]["chkCourse"] = DchkCourse;
            FillDataSet.Tables[0].Rows[0]["chkSalesPerson"] = DchkSalesPerson;
            FillDataSet.Tables[0].Rows[0]["chkStudent"] = DchkStudent;
            FillDataSet.Tables[0].Rows[0]["chkInquiry"] = DchkInquiry;
            FillDataSet.Tables[0].Rows[0]["chkInvoice"] = DchkInvoice;
            FillDataSet.Tables[0].Rows[0]["chkPayment"] = DchkPayment;
            FillDataSet.Tables[0].Rows[0]["chkInvoiceListing"] = DchkInvoiceListing;
            FillDataSet.Tables[0].Rows[0]["chkPaymentListing"] = DchkPaymentListing;
            FillDataSet.Tables[0].Rows[0]["chkOutstanding"] = DchkOutstanding;
            FillDataSet.Tables[0].Rows[0]["chkSalesReport"] = DchkSalesReport;
            FillDataSet.Tables[0].Rows[0]["chkCourseWiseStudent"] = DchkCourseWiseStudent;
            FillDataSet.Tables[0].Rows[0]["chkPayList"] = DchkPayList;
        }
        private void ButtonControl()
        {
            if (cMode.Equals("NEW") || cMode.Equals("EDIT") || cMode.Equals("APPROVE"))
            {
                this.btnPrint.Enabled = true;
                this.btnSave.Enabled = true;
            }
            if (cMode.Equals("PRINT"))
            {
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = true;
                this.btnSave.Enabled = false;
            }
            if (cMode.Equals("DELETE"))
            {
                this.btnDelete.Enabled = true;
            }
            if (cMode.Equals("VIEW"))
            {
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnSave.Enabled = false;
            }
            if (cMode.Equals("INIT"))
            {
                this.btnPrint.Enabled = true;
            }
        }

        private void DisableAllButtons()
        {
            this.btnDelete.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnSave.Enabled = false;

        }

        private void LoadData()
        {
            if (this.cMode.Equals("INIT"))
            {
                this.txtUserName.Text = "";
                this.txtFullName.Text = "";
                this.txtContactNo.Text = "";
                this.txtOldPassword.Text = "";
                this.txtNewPassword.Text = "";
                this.txtConfirmPassword.Text = "";

            }
            else
            {
                DataSet newDataSet = null;

                DatabaseConnection dbCon = new DatabaseConnection();
                newDataSet = dbCon.Get("Select * from File_User Where SUserName='" + txtUserName.Text.Trim() + "'");
                

                if (newDataSet.Tables[0].Rows.Count > 0)
                {
                    this.userName = newDataSet.Tables[0].Rows[0]["SUserName"].ToString();
                    this.fullName = newDataSet.Tables[0].Rows[0]["SFullName"].ToString();
                    this.oldPassword = newDataSet.Tables[0].Rows[0]["SPassword"].ToString();
                    this.contactNo = newDataSet.Tables[0].Rows[0]["SContactNo"].ToString();

                    if (newDataSet.Tables[0].Rows[0]["chkUser"].ToString().Trim().Equals("1"))
                        this.chkUser.Checked=true;
                    if (newDataSet.Tables[0].Rows[0]["chkProfile"].ToString().Trim().Equals("1"))
                        this.chkProfile.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkCourse"].ToString().Trim().Equals("1"))
                        this.chkCourse.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkSalesPerson"].ToString().Trim().Equals("1"))
                        this.chkSalesPerson.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkStudent"].ToString().Trim().Equals("1"))
                        this.chkStudent.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkInquiry"].ToString().Trim().Equals("1"))
                        this.chkInquiry.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkInvoice"].ToString().Trim().Equals("1"))
                        this.chkInvoice.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkPayment"].ToString().Trim().Equals("1"))
                        this.chkPayment.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkInvoiceListing"].ToString().Trim().Equals("1"))
                        this.chkInvoiceListing.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkPaymentListing"].ToString().Trim().Equals("1"))
                        this.chkPaymentListing.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkOutstanding"].ToString().Trim().Equals("1"))
                        this.chkOutstanding.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkSalesReport"].ToString().Trim().Equals("1"))
                        this.chkSalesReport.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkCourseWiseStudent"].ToString().Trim().Equals("1"))
                        this.chkCoursewise.Checked = true;
                    if (newDataSet.Tables[0].Rows[0]["chkPayList"].ToString().Trim().Equals("1"))
                        this.chkPayList.Checked = true;

                    this.txtFullName.Text = this.fullName;
                    this.txtContactNo.Text = this.contactNo;

                    this.cMode = "DELETE";
                    InitProperties(cMode);
                    DisableAllButtons();
                    ButtonControl();

                }
                else
                {
                    DialogResult dRes = MessageBox.Show("Do You Want To Create New User?", "Confirm", MessageBoxButtons.YesNo);
                    if (dRes == DialogResult.Yes)
                    {
                        this.cMode = "NEW";
                        InitProperties(cMode);
                        DisableAllButtons();
                        ButtonControl();
                    }
                    else
                    {
                        this.cMode = "INIT";
                        InitProperties(cMode);
                        DisableAllButtons();
                        LoadData();
                        ButtonControl();
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate_Entries())
                if (Commit_Action())
                {
                    MessageBox.Show("Updated Successfully!");
                    LoadData();
                }
                else
                    MessageBox.Show("Failed");

            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //DatabaseConnection dbCon = new DatabaseConnection();
            //DataSet Report_DataSet = dbCon.Get_ReportData("Select * from File_Profile", "DT_User");

            //Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_CompanyUser.rpt", Report_DataSet,"User");
            //openReport.ShowInTaskbar = false;
            //openReport.ShowDialog();
        }
        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (this.txtUserName.Text.Trim().ToUpper().Equals("INTOUCH"))
            {
                MessageBox.Show("Invalid Username");
                this.txtUserName.Text = "";
            }
            else
            {
                if (!this.txtUserName.Text.Trim().Equals(""))
                //if (Validate_UserName())
                {
                    this.cMode = "CREATE";
                    LoadData();
                }
                if (this.cMode.Equals("CLOSE"))
                    this.Close();
            }
        }
        private bool Validate_UserName()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet newDataSet = dbCon.Get("select SUserName from file_user where SUserName='" + txtUserName.Text.Trim() + "'");
            if (newDataSet.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("User Has Been Already Created!");
                return false;
            }
            else
                return true;
        }
        private void txtOldPassword_Leave(object sender, EventArgs e)
        {
            if (!this.cMode.Equals("NEW") && !this.cMode.Equals("INIT"))
                if (this.oldPassword.Trim().Equals(this.txtOldPassword.Text.Trim()))
                {
                    this.cMode = "EDIT";
                    InitProperties(cMode);
                    DisableAllButtons();
                    ButtonControl();
                }
                else
                {
                    MessageBox.Show("Incorrect Password!");
                    this.txtOldPassword.Text = "";
                }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dRes = MessageBox.Show("Do You Want To Delete?", "Confirm", MessageBoxButtons.YesNo);
            if (dRes == DialogResult.Yes)
            {
                this.cMode = "DELETE";
                if (Commit_Action())
                    MessageBox.Show("User Deleted");
                else
                    MessageBox.Show("User Delete Failed");
            }
        }
        private void txtUserName_DropDown(object sender, EventArgs e)
        {
            txtUserName.Items.Clear();

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet userDataSet = dbCon.Get("select SUserName,SFullName from file_user where SUserName<>'InTouch'");

            for (int i = 0; i < userDataSet.Tables[0].Rows.Count; i++) {
                string itemName = userDataSet.Tables[0].Rows[i]["SUserName"].ToString().Trim();
                txtUserName.Items.Add(itemName);
            }

        }

        private void txtUserName_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkPayList.Text, chkPayList);
        }

        private void chkInvoiceListing_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkInvoiceListing.Text, chkInvoiceListing);
        }

        private void chkUser_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkUser.Text, chkUser);
        }

        private void chkProfile_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkProfile.Text, chkProfile);
        }

        private void chkCourse_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkCourse.Text, chkCourse);
        }

        private void chkSalesPerson_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkSalesPerson.Text, chkSalesPerson);
        }

        private void chkStudent_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkStudent.Text, chkStudent);
        }

        private void chkInquiry_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkInquiry.Text, chkInquiry);
        }

        private void chkInvoice_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkInvoice.Text, chkInvoice);
        }

        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkPayment.Text, chkPayment);
        }

        private void chkPaymentListing_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkPaymentListing.Text, chkPaymentListing);
        }

        private void chkOutstanding_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkOutstanding.Text, chkOutstanding);
        }

        private void chkSalesReport_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkSalesReport.Text, chkSalesReport);
        }

        private void chkCoursewise_CheckedChanged(object sender, EventArgs e)
        {
            Update_Button_Access(chkCoursewise.Text, chkCoursewise);
        }

        private void Update_Button_Access(string formName,CheckBox chkBok) {
            this.labelAccess.Text = formName.Trim();

            if (chkBok.Checked)
            {
                this.chkNew.Checked = true;
                this.chkEdit.Checked = true;
                this.chkPrint.Checked = true;
                this.chkDelete.Checked = true;
                this.chkView.Checked = true;
                this.chkSave.Checked = true;
                this.chkReset.Checked = true;
            }
            else
            {
                this.chkNew.Checked = false;
                this.chkEdit.Checked = false;
                this.chkPrint.Checked = false;
                this.chkDelete.Checked = false;
                this.chkView.Checked = false;
                this.chkSave.Checked = false;
                this.chkReset.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            double DchkNew;

            double DchkEdit;

            double DchkPrint;
            double DchkDelete;

            double DchkView;

            double DchkSave;

            double DchkReset;

            if (chkNew.Checked)
                DchkNew = 1;
            else
                DchkNew = 0;

            if (chkEdit.Checked)
                DchkEdit = 1;
            else
                DchkEdit = 0;

            if (chkPrint.Checked)
                DchkPrint = 1;
            else
                DchkPrint = 0;


            if (chkDelete.Checked)
                DchkDelete = 1;
            else
                DchkDelete = 0;

            if (chkView.Checked)
                DchkView = 1;
            else
                DchkView = 0;

            if (chkSave.Checked)
                DchkSave = 1;
            else
                DchkSave = 0;

            if (chkReset.Checked)
                DchkReset = 1;
            else
                DchkReset = 0;

            try
            {
                DataSet DS_UAccess = dbCon.Get("Select SUserName from File_UserAccess where SUserName='" + txtUserName.Text.Trim() + "' and SFormName='" + labelAccess.Text.Trim() + "'");

                if (dbCon.OpenDBConnection())
                    dbCon.BeginTransaction();

                bool Saved = false;

                if (DS_UAccess.Tables[0].Rows.Count > 0)
                    Saved = dbCon.ExecuteQuery("UPDATE [dbo].[File_UserAccess] SET [chkNew] = '" + DchkNew + "',[chkEdit] = '" + DchkEdit + "',[chkPrint] = '" + DchkPrint + "', "
                        + " [chkDelete] = '" + DchkDelete + "',[chkView] = '" + DchkView + "',[chkSave] = '" + DchkSave + "',[chkReset] = '" + DchkReset + "' "
                        + " WHERE SUserName = '" + txtUserName.Text.Trim() + "' and SFormName = '" + labelAccess.Text.Trim() + "'");
                else
                    Saved = dbCon.ExecuteQuery("INSERT INTO [dbo].[File_UserAccess]([SUserName],[SFormName],[chkNew],[chkEdit],[chkPrint],[chkDelete] "
                        + " ,[chkView],[chkSave],[chkReset]) VALUES('" + this.txtUserName.Text.Trim() + "', '" + this.labelAccess.Text.Trim() + "', '" 
                        + DchkNew + "', '" + DchkEdit + "', '" + DchkPrint + "' "
                        + " , '" + DchkDelete + "', '" + DchkView + "', '" + DchkSave + "', '" + DchkReset + "')");

                if (Saved)
                {
                    dbCon.CommitTransaction();
                    MessageBox.Show("Updated!");
                }
                else
                    MessageBox.Show("Failed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbCon.RollBackTransaction();
            }
            finally
            {
                dbCon.CloseDBConnection();
            }
        }

        private void chkNew_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}