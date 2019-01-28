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
    public partial class Student : Form
    {
        private string cMode;
        DataSet StudentDataSet;

        public string CMode { get => cMode; set => cMode = value; }

        public Student()
        {
            InitializeComponent();
        }

        private void Student_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }


        private void Student_Load(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            StudentDataSet = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo, "
                + "SMobileNo,SEmail,SAdditionalInfo,SPicPath,SAddUser,DAddDate,SAddMachine from File_Student WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
        }

        private bool Validate_Entries() {

            if (txtName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student Name");
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

            Fill_DataSet(StudentDataSet);
            if (newDbConn.Upd(StudentDataSet, "File_Student"))
            {
                if (picStudent.Image != null)
                {
                    string fileName = this.txtCode.Text.Trim() + ".jpg";
                    string assetPath = GlobalVariables.AssetPath.Trim();
                    string folder = assetPath + @"Images\Student_Images\";
                    try
                    {
                        if (System.IO.File.Exists(folder + fileName))
                        {
                            // System.IO.File.Replace(picStudent.Image.ToString(), folder + fileName, @"C:\Users\Sujee\Desktop\MetadataAsSource\");
                            //   System.IO.File.Delete(folder + System.IO.Path.GetFileName(fileName));
                        }

                        string pathString = System.IO.Path.Combine(folder, fileName);
                        Image a = picStudent.Image;


                        a.Save(pathString);
                        newDbConn.ExecuteQuery("Update File_Student Set SPicPath='" + pathString + "' where SStudentId='" + txtCode.Text + "'");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("File in use!"); //e.Message
                        return false; 
                    }
                }
                return true;
            }
            else
                return false;
        }

        private void Fill_DataSet(DataSet FillDataSet) {

            if (FillDataSet.Tables[0].Rows.Count > 0)
                FillDataSet.Tables[0].Rows.RemoveAt(0);

            FillDataSet.Tables[0].Rows.Add();
            FillDataSet.Tables[0].Rows[0]["SStudentId"] = txtCode.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SName"] = txtName.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SContactPerson"] = txtContactPerson.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress1"] = txtAddress1.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress2"] = txtAddress2.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddress3"] = txtAddress3.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["STelNo"] = txtTelNo.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SMobileNo"] = txtMobileNo.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SEmail"] = txtEmail.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAdditionalInfo"] = txtAdditionalInfo.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SPicPath"] = txtFilePath.Text.Trim();
            FillDataSet.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSet.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSet.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;
        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("DELETE FROM File_Student where SStudentId = '" + txtCode.Text + "'");
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_Student] ([SStudentId],[SName],[SContactPerson],[SAddress1],[SAddress2],[SAddress3], "
                + "[STelNo],[SMobileNo],[SEmail],[SAdditionalInfo],[SPicPath],[SAddUser],[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType]) "
                + "(SELECT SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SPicPath, "
                + " SAddUser, DAddDate, SAddMachine,'" + GlobalVariables.GetUserName() + "', GetDate(), '" + Environment.MachineName + "', '" + logType
                + "' FROM File_Student WHERE SStudentId = '" + this.txtCode.Text + "')");
        }
        private void LoadData()
        {
            DataSet newDataSet = null;

            if (cMode.Equals("INIT"))
            {
                this.txtCode.Text = "";
                this.txtName.Text = "";
                this.txtContactPerson.Text = "";
                this.txtAddress1.Text = "";
                this.txtAddress2.Text = "";
                this.txtAddress3.Text = "";
                this.txtTelNo.Text = "";
                this.txtMobileNo.Text = "";
                this.txtEmail.Text = "";
                this.txtAdditionalInfo.Text = "";
                this.txtFilePath.Text = "";

                this.picStudent.Image = null;
            }
            else
            {
                if (!txtCode.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();
                    newDataSet = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,"
                        + "SAdditionalInfo,SPicPath from File_Student WHERE SStudentId='" + txtCode.Text + "'");

                    txtName.Text = newDataSet.Tables[0].Rows[0]["SName"].ToString();
                    txtContactPerson.Text = newDataSet.Tables[0].Rows[0]["SContactPerson"].ToString();
                    txtAddress1.Text = newDataSet.Tables[0].Rows[0]["SAddress1"].ToString();
                    txtAddress2.Text = newDataSet.Tables[0].Rows[0]["SAddress2"].ToString();
                    txtAddress3.Text = newDataSet.Tables[0].Rows[0]["SAddress3"].ToString();
                    txtTelNo.Text = newDataSet.Tables[0].Rows[0]["STelNo"].ToString();
                    txtMobileNo.Text = newDataSet.Tables[0].Rows[0]["SMobileNo"].ToString();
                    txtEmail.Text = newDataSet.Tables[0].Rows[0]["SEmail"].ToString();
                    txtAdditionalInfo.Text = newDataSet.Tables[0].Rows[0]["SAdditionalInfo"].ToString();
                    txtFilePath.Text = newDataSet.Tables[0].Rows[0]["SPicPath"].ToString();
                    if (!txtFilePath.Text.Trim().Equals(""))
                        this.picStudent.Image = new Bitmap(txtFilePath.Text.ToString().Trim());

                    if (this.cMode.Equals("PRINT"))
                    {
                        DataSet Report_DataSet = dbCon.Get_ReportData("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo, "
                            +" SEmail, SAdditionalInfo, SPicPath, SAddUser, DAddDate from File_Student Where SStudentId='" + txtCode.Text.Trim() + "'", "DT_Student");

                        Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Student.rpt", Report_DataSet, "Student");
                        openReport.ShowInTaskbar = false;
                        openReport.ShowDialog();

                        this.CMode = "INIT";
                        DisableAllButtons();
                        ButtonControl();
                        InitProperties(CMode);
                        LoadData();
                    }
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
                this.btnReset.Enabled = true;
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
                this.txtContactPerson.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
                this.txtAdditionalInfo.ReadOnly = true;
                this.txtFilePath.ReadOnly = true;

                this.btnUpload.Enabled = false;
            }
            if (cMode.Equals("DELETE"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = true;
                this.txtContactPerson.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
                this.txtAdditionalInfo.ReadOnly = true;
                this.txtFilePath.ReadOnly = true;

                this.btnUpload.Enabled = false;
            }
            if (cMode.Equals("NEW"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = false;
                this.txtContactPerson.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtAddress3.ReadOnly = false;
                this.txtTelNo.ReadOnly = false;
                this.txtMobileNo.ReadOnly = false;
                this.txtEmail.ReadOnly = false;
                this.txtAdditionalInfo.ReadOnly = false;
                this.txtFilePath.ReadOnly = true;

                this.btnUpload.Enabled = true;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtCode.ReadOnly = true;
                this.txtName.ReadOnly = false;
                this.txtContactPerson.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtAddress3.ReadOnly = false;
                this.txtTelNo.ReadOnly = false;
                this.txtMobileNo.ReadOnly = false;
                this.txtEmail.ReadOnly = false;
                this.txtAdditionalInfo.ReadOnly = false;
                this.txtFilePath.ReadOnly = true;

                this.btnUpload.Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            MasterKey newMasterKey = new MasterKey("Student", "StudentId", this);
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
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SPicPath from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find", this);
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
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SPicPath from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find", this);
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
            if (!mdDelete.Validate_Master_Duplicate("Student", "SStudentId", this.txtCode.Text.Trim()))
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
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SPicPath from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find Student", this);
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

            this.cMode = "PRINT";
            LoadData();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SPicPath from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find Student", this);
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
        
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg;*.jpeg;*.gif;*.bmp) |*.jpg;*.jpeg;*.gif;*.bmp";

            if (open.ShowDialog() == DialogResult.OK) {
                picStudent.Image = new Bitmap(open.FileName);
                txtFilePath.Text = open.FileName;
            }
        }
    }
}
