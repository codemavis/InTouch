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
    public partial class SalesPerson : Form
    {
        private string cMode;
        DataSet SalesPersonDataSet;

        public string CMode { get => cMode; set => cMode = value; }

        public SalesPerson()
        {
            InitializeComponent();
        }

        private void SalesPerson_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        
        private void SalesPerson_Load(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            SalesPersonDataSet = dbCon.Get("SELECT SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo, "
                + " SEmail,SAddUser,DAddDate,SAddMachine FROM File_SalesPerson WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
        }

        private bool Validate_Entries() {

            if (txtName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Sales Person Name");
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
            
            Fill_DataSet(SalesPersonDataSet);
            return newDbConn.Upd(SalesPersonDataSet, "File_SalesPerson");
        }

        private void Fill_DataSet(DataSet FillDataSet) {

            if (FillDataSet.Tables[0].Rows.Count > 0)
                FillDataSet.Tables[0].Rows.RemoveAt(0);

            FillDataSet.Tables[0].Rows.Add();
            FillDataSet.Tables[0].Rows[0]["SRepCode"] = txtCode.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SRepName"] = txtName.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress1"] = txtAddress1.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress2"] = txtAddress2.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress3"] = txtAddress3.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["STelephoneNo"] = txtTelNo.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SMobileNo"] = txtMobileNo.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SEmail"] = txtEmail.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSet.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSet.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;

        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("DELETE FROM File_SalesPerson where SRepCode = '" + txtCode.Text + "'");
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_SalesPerson] ([SRepCode],[SRepName],[SAddress1],[SAddress2],[SAddress3], "
                + "[STelephoneNo],[SMobileNo],[SEmail],[SAddUser],[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType]) "
                + "(SELECT SRepCode, SRepName, SAddress1, SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail, SAddUser, DAddDate, SAddMachine,'"
                + GlobalVariables.GetUserName() + "', GetDate(), '" + Environment.MachineName + "', '" + logType
                + "' FROM File_SalesPerson WHERE SRepCode = '" + this.txtCode.Text + "')");
        }
        private void LoadData()
        {
            DataSet newDataSet = null;

            if (cMode.Equals("INIT"))
            {
                this.txtCode.Text = "";
                this.txtName.Text = "";
                this.txtAddress1.Text = "";
                this.txtAddress2.Text = "";
                this.txtAddress3.Text = "";
                this.txtTelNo.Text = "";
                this.txtMobileNo.Text = "";
                this.txtEmail.Text = "";

            }
            else
            {
                if (!txtCode.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();
                    newDataSet = dbCon.Get("SELECT SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail "
                        + "FROM File_SalesPerson WHERE SRepCode='" + txtCode.Text + "'");

                    txtName.Text = newDataSet.Tables[0].Rows[0]["SRepName"].ToString();
                    txtAddress1.Text = newDataSet.Tables[0].Rows[0]["SAddress1"].ToString();
                    txtAddress2.Text = newDataSet.Tables[0].Rows[0]["SAddress2"].ToString();
                    txtAddress3.Text = newDataSet.Tables[0].Rows[0]["SAddress3"].ToString();
                    txtTelNo.Text = newDataSet.Tables[0].Rows[0]["STelephoneNo"].ToString();
                    txtMobileNo.Text = newDataSet.Tables[0].Rows[0]["SMobileNo"].ToString();
                    txtEmail.Text = newDataSet.Tables[0].Rows[0]["SEmail"].ToString();
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
                this.txtName.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
            }
            if (cMode.Equals("DELETE"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtAddress3.ReadOnly = false;
                this.txtTelNo.ReadOnly = false;
                this.txtMobileNo.ReadOnly = false;
                this.txtEmail.ReadOnly = false;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtAddress3.ReadOnly = false;
                this.txtTelNo.ReadOnly = false;
                this.txtMobileNo.ReadOnly = false;
                this.txtEmail.ReadOnly = false;

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            MasterKey newMasterKey = new MasterKey("SalesPerson", "RepCode", this);
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

            this.txtName.Focus();

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.cMode = "EDIT";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("SELECT SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail FROM File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("SalesPerson", "Find", this);
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
            DataSet FillData = dbCon.Get("SELECT SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail FROM File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("SalesPerson", "Find", this);
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
            if (!mdDelete.Validate_Master_Duplicate("SalesPerson", "SRepCode", this.txtCode.Text.Trim()))
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
            DataSet Report_DataSet = dbCon.Get_ReportData("Select SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail,SAddUser,DAddDate from File_SalesPerson", "DT_SalesPerson");

            Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_SalesPerson.rpt", Report_DataSet, "Sales Person");
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
            DataSet FillData = dbCon.Get("SELECT SRepCode,SRepName,SAddress1,SAddress2,SAddress3,STelephoneNo,SMobileNo,SEmail FROM File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("SalesPerson", "Find SalesPerson", this);
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
            if (Validate_Entries())
                if (Commit_Action())
                {
                    MessageBox.Show("Success!");
                }
                else
                    MessageBox.Show("Failed");
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

        private void btnReset_Click_1(object sender, EventArgs e)
        {

        }
    }
}
