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
    public partial class Course : Form
    {
        private string cMode;
        DataSet CourseDataSet;

        public string CMode { get => cMode; set => cMode = value; }

        public Course()
        {
            InitializeComponent();
        }

        private void Course_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        
        private void Course_Load(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            CourseDataSet = dbCon.Get("SELECT SCourseCode,SCourseName,NAmount,NCreditPeriod,SAddUser,DAddDate,SAddMachine FROM File_Course WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
        }

        private bool Validate_Entries() {

            if (txtDescription.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Description");
                return false;
            }
            if (Convert.ToDouble(txtAmount.Text.Trim()) <= 0.00)
            {
                MessageBox.Show("Invalid Amount");
                return false;
            }
            if (txtCreditPeriod.Text.Trim().Equals(""))
            {
                txtCreditPeriod.Text = "0.00";
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
                if (SaveLogData("E", newDbConn))
                    if (DeleteData(newDbConn))
                        Saved = SaveData(newDbConn);
            }

            if (cMode.Equals("DELETE"))
            {
                if (SaveLogData("D", newDbConn))
                    Saved = DeleteData(newDbConn);
            }

            if (Saved)
            {
                newDbConn.CommitTransaction();
                newDbConn.CloseDBConnection();

                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                LoadData();
            }
            else
            {
                newDbConn.RollBackTransaction();
                newDbConn.CloseDBConnection();
            }

            return Saved;
        }

        private bool SaveData(DatabaseConnection newDbConn)
        {
            
            Fill_DataSet(CourseDataSet);
            return newDbConn.Upd(CourseDataSet, "File_Course");
        }

        private void Fill_DataSet(DataSet FillDataSet) {

            if (FillDataSet.Tables[0].Rows.Count > 0)
                FillDataSet.Tables[0].Rows.RemoveAt(0);

            if (txtCreditPeriod.Text.Trim().Equals(""))
                txtCreditPeriod.Text = "0";

            FillDataSet.Tables[0].Rows.Add();
            FillDataSet.Tables[0].Rows[0]["SCourseCode"] = txtCode.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SCourseName"] = txtDescription.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["NAmount"] = Convert.ToDouble(txtAmount.Text.Trim());
            FillDataSet.Tables[0].Rows[0]["NCreditPeriod"] = Convert.ToDouble(txtCreditPeriod.Text.Trim());
            FillDataSet.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSet.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSet.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;

        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("DELETE FROM File_Course where SCourseCode = '" + txtCode.Text + "'");
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_Course] ([SCourseCode],[SCourseName],[NAmount],[NCreditPeriod],[SAddUser], "
                + " [DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType]) (SELECT SCourseCode, SCourseName, NAmount,NCreditPeriod, "
                +   " SAddUser, DAddDate, SAddMachine,'" + GlobalVariables.GetUserName() + "', GetDate(), '" + Environment.MachineName 
                +   "', '" + logType + "' FROM File_Course WHERE SCourseCode = '" + this.txtCode.Text + "')");
        }
        private void LoadData()
        {
            DataSet newDataSet = null;

            if (cMode.Equals("INIT"))
            {
                this.txtCode.Text = "";
                this.txtDescription.Text = "";
                this.txtAmount.Text = "";
                this.txtCreditPeriod.Text = "";
            }
            else
            {
                if (!txtCode.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();
                    newDataSet = dbCon.Get("SELECT SCourseCode,SCourseName,NAmount,NCreditPeriod FROM File_Course WHERE SCourseCode='" + txtCode.Text + "'");

                    txtDescription.Text = newDataSet.Tables[0].Rows[0]["SCourseName"].ToString();
                    txtAmount.Text = newDataSet.Tables[0].Rows[0]["NAmount"].ToString();
                    txtCreditPeriod.Text = newDataSet.Tables[0].Rows[0]["NCreditPeriod"].ToString();
                }
            }
        }

        private void ButtonControl()
        {
            if (cMode.Equals("NEW") || cMode.Equals("EDIT") || cMode.Equals("APPROVE"))
            {
                this.btnSave.Enabled = true;
                this.btnReset.Enabled = true;
            }
            if (cMode.Equals("PRINT"))
            {
                this.btnReset.Enabled = true;
            }
            if (cMode.Equals("DELETE"))
            {

            }
            if (cMode.Equals("VIEW"))
            {
                this.btnReset.Enabled = true;
            }
            if (cMode.Equals("INIT"))
            {
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnFind.Enabled = true;
            }
        }

        private void DisableAllButtons()
        {
            this.btnNew.Enabled = false;
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPrint.Enabled = false;
            this.btnFind.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnReset.Enabled = false;

        }

        private void InitProperties(string cMode)
        {
            if (cMode.Equals("INIT"))
            {
                this.txtCode.ReadOnly = true;
                this.txtDescription.ReadOnly = true;
                this.txtAmount.ReadOnly = true;
                this.txtCreditPeriod.ReadOnly = true;
            }
            if (cMode.Equals("DELETE"))
            {
                this.txtCode.ReadOnly = true;
                this.txtDescription.ReadOnly = true;
                this.txtAmount.ReadOnly = true;
                this.txtCreditPeriod.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                this.txtCode.ReadOnly = true;
                this.txtDescription.ReadOnly = false;
                this.txtAmount.ReadOnly = false;
                this.txtCreditPeriod.ReadOnly = false;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtCode.ReadOnly = true;
                this.txtDescription.ReadOnly = false;
                this.txtAmount.ReadOnly = false;
                this.txtCreditPeriod.ReadOnly = false;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            MasterKey newMasterKey = new MasterKey("Course", "CourseCode", this);
            newMasterKey.ShowInTaskbar = false;
            newMasterKey.ShowDialog();
            txtCode.Text = newMasterKey.KeyValue;
            if (newMasterKey.IsClosed || txtCode.Text.Equals(""))
            {
                cMode = "INIT";
                InitProperties(cMode);
                DisableAllButtons();
                ButtonControl();
                LoadData();
            }
            this.txtDescription.Focus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.cMode = "EDIT";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("SELECT SCourseCode,SCourseName,NAmount,NCreditPeriod FROM File_Course");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Course", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtCode.Text = newFind.FindKeyValue;
            if (txtCode.Text.Trim().Equals(""))
            {
                this.cMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                this.InitProperties(cMode);
                return;
            }
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.cMode = "DELETE";
            DisableAllButtons();
            ButtonControl();
            InitProperties(cMode);
            LoadData();

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("SELECT SCourseCode as Course_Code,SCourseName as Course_Name,NAmount as Amount,NCreditPeriod as Credit_Period FROM File_Course");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Course", "Find", this);
            newFind.ShowDialog();

            this.txtCode.Text = newFind.FindKeyValue;
            if (txtCode.Text.Trim().Equals(""))
            {
                this.cMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                this.InitProperties(cMode);
                return;
            }
            //Validate Master Data
            Master_Data_Get mdDelete = new Master_Data_Get();
            if (!mdDelete.Validate_Master_Duplicate("Course", "SCourseCode", this.txtCode.Text.Trim()))
            {
                this.cMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(cMode);
                LoadData();
                return;
            }

            LoadData();

            DialogResult dRes = MessageBox.Show("Do You Want To Delete?", "Confirm", MessageBoxButtons.YesNo);
            if (dRes == DialogResult.Yes)
                this.Commit_Action();
            else
            {
                this.cMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(cMode);
                LoadData();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet Report_DataSet = dbCon.Get_ReportData("Select SCourseCode,SCourseName,NAmount,NCreditPeriod,SAddUser,DAddDate,b.* from file_Course a,File_Profile b", "DT_Course");

            Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Course.rpt", Report_DataSet, "Course");
            openReport.ShowInTaskbar = false;
            openReport.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("SELECT SCourseCode,SCourseName,NAmount,NCreditPeriod FROM File_Course");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Course", "Find Course", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtCode.Text = newFind.FindKeyValue;
            if (txtCode.Text.Trim().Equals(""))
            {
                this.cMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                this.InitProperties(cMode);
                return;
            }
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate_Entries())
                    if (Commit_Action())
                    {
                        MessageBox.Show("Success!");
                    }
                    else
                        MessageBox.Show("Failed");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.cMode = "INIT";
            DisableAllButtons();
            ButtonControl();
            InitProperties(CMode);
            LoadData();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void txtCreditPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && txtCreditPeriod.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtCreditPeriod_Leave(object sender, EventArgs e)
        {
            if (!txtCreditPeriod.Text.Trim().Equals(""))
            {
                if (!txtCreditPeriod.Text.Trim().Equals("."))
                    txtCreditPeriod.Text = string.Format("{0:n2}", double.Parse(txtCreditPeriod.Text));
                else
                    txtCreditPeriod.Text = "";
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && txtAmount.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            if (!txtAmount.Text.Trim().Equals(""))
            {
                if (!txtAmount.Text.Trim().Equals("."))
                    txtAmount.Text = string.Format("{0:n2}", double.Parse(txtAmount.Text));
                else
                    txtAmount.Text = "";
            }
        }

        private void txtAmount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && txtAmount.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtCreditPeriod_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && txtAmount.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }
    }
}
