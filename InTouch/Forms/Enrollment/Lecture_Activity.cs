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
using InTouch.DataSets;
using InTouch.Classes.MasterHelp;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using InTouch.Forms.Report;
using InTouch.Forms.SubForms;

namespace InTouch
{
    public partial class Lecture_Activity : Form
    {
        private string cMode;
        DataSet Lecture_ActivityDataSetH, Lecture_ActivityDataSetD;
        GenerateReference newReference;
        private string cInqNo="";
      //  Image_View newImg;

        public string CMode { get => cMode; set => cMode = value; }

        public Lecture_Activity()
        {
            InitializeComponent();            
        }

        private void Lecture_Activity_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        
        private void Lecture_Activity_Load(object sender, EventArgs e)
        {
            this.dataGridViewLecture_Activity.Columns[0].Width = this.dataGridViewLecture_Activity.Width * 5 / 100;
            this.dataGridViewLecture_Activity.Columns[1].Width = this.dataGridViewLecture_Activity.Width * 25 / 100;
            this.dataGridViewLecture_Activity.Columns[2].Width = this.dataGridViewLecture_Activity.Width * 70 / 100;

            DatabaseConnection dbCon = new DatabaseConnection();
            Lecture_ActivityDataSetH = dbCon.Get("Select SRefNo,DDate,SCourseCode,SLecturerCode,NInTime,NOutTime," +
                "SComments,SAddUser,DAddDate,SAddMachine,IRecordId  From File_LectureActivityH WHERE 1=0");

            Lecture_ActivityDataSetD = dbCon.Get("Select SRefNo,SStudentId,IRecordId From File_LectureActivityD WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            LoadData();
            ButtonControl();
        }

        private bool Validate_Entries()
        {
            if (txtCourseCode.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student Code");
                return false;
            }

            for (int i = 0; i < dataGridViewLecture_Activity.RowCount; i++)
            {
                if ((dataGridViewLecture_Activity.Rows[i].Cells[1].Value == null)
                    || (dataGridViewLecture_Activity.Rows[i].Cells[1].Value.ToString().Trim().Equals("")))
                {
                    MessageBox.Show("Invalid Records Will Be Ommited!");
                    dataGridViewLecture_Activity.Rows.RemoveAt(i);
                }
            }

            if (dataGridViewLecture_Activity.Rows.Count <= 0)
            {
                MessageBox.Show("No Valid Records To Save!");
                return false;
            }
            

            return true;
        }

        private bool Validate_Transactions()
        {
           
            return true;
        }

        private string Get_UniqueID()
        {
            DatabaseConnection newDbConn = new DatabaseConnection();
            newReference = new GenerateReference();
            return newReference.GetReference(newDbConn, "SINV");
        }

        private bool Commit_Action()
        {
            string NewAutoGen = Get_UniqueID();
            DatabaseConnection newDbConn = new DatabaseConnection();

            newDbConn.OpenDBConnection();
            newDbConn.BeginTransaction();

            bool Saved = false;

            if (CMode.Equals("NEW"))
            {
                if (NewAutoGen == "")
                    return false;
                else
                {
                    this.txtRefNo.Text = NewAutoGen;
                    if (SaveData(newDbConn))
                    {
                        newReference = new GenerateReference();
                        Saved = newReference.UpdateNumVal(newDbConn, "SINV");
                    }
                }
            }

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

                MessageBox.Show(this.txtRefNo.Text.Trim());
                Print_Data();

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
            Fill_DataSet(Lecture_ActivityDataSetH, Lecture_ActivityDataSetD);
            if (newDbConn.Upd(Lecture_ActivityDataSetH, "File_LectureActivityH"))
                return newDbConn.Upd(Lecture_ActivityDataSetD, "File_LectureActivityD");
            else
                return false;
        }

        private void Calculation()
        {
        }
        private void Fill_DataSet(DataSet FillDataSetH, DataSet FillDataSetD)
        {
            if (FillDataSetH.Tables[0].Rows.Count > 0)
                FillDataSetH.Tables[0].Rows.RemoveAt(0);
            
            FillDataSetH.Tables[0].Rows.Add();
            FillDataSetH.Tables[0].Rows[0]["SRefNo"] = txtRefNo.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["DDate"] = dateTimePickerInv.Value.ToShortDateString();
            FillDataSetH.Tables[0].Rows[0]["SCourseCode"] = txtCourseCode.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SLecturerCode"] = this.cInqNo;
            FillDataSetH.Tables[0].Rows[0]["SComments"] = txtComments.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSetH.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSetH.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;


            if (FillDataSetD.Tables[0].Rows.Count > 0)
                FillDataSetD.Tables[0].Rows.Clear();

            for (int i = 0; i < dataGridViewLecture_Activity.Rows.Count; i++)
            {
                FillDataSetD.Tables[0].Rows.Add();
                FillDataSetD.Tables[0].Rows[i]["SRefNo"] = this.txtRefNo.Text.Trim();
                FillDataSetD.Tables[0].Rows[i]["SStudentId"] = dataGridViewLecture_Activity.Rows[i].Cells["SStudentId"].FormattedValue.ToString().Trim();
            }
        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            if (newDbConn.ExecuteQuery("DELETE FROM File_LectureActivityH where SRefNo = '" + txtRefNo.Text + "'"))
                return newDbConn.ExecuteQuery("DELETE FROM File_LectureActivityD where SRefNo = '" + txtRefNo.Text + "'");
            else
                return false;
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_LectureActivity]([SRefNo],SStudentId,[DDate],[SCourseCode],[SLecturerCode],[NInTime],[NOutTime]," +
                "[SComments],[SAddUser],[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType])(Select a.SRefNo,SStudentId," +
                "DDate,SCourseCode,SLecturerCode,NInTime,NOutTime,SComments,SAddUser,DAddDate,SAddMachine,'" + GlobalVariables.GetUserName() + "',GetDate()," +
                "'" + Environment.MachineName + "','" + logType + "' from File_LectureActivityH a,File_LectureActivityD b where a.SRefNo=b.SRefNo " +
                "and a.SRefNo='" + this.txtRefNo.Text + "')");
           
        }
        private void LoadData()
        {
            DataSet newDataSetH = null;
            DataSet newDataSetD = null;

            if (cMode.Equals("INIT"))
            {
                this.txtRefNo.Text = "";
                this.txtCourseCode.Text = "";
                this.txtCourseName.Text = "";
        
                this.txtComments.Text = "";
                
                this.dataGridViewLecture_Activity.Rows.Clear();
            }
            else
            {
                if (!txtRefNo.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();

                    newDataSetD = dbCon.Get("Select SRefNo,a.SStudentId,SName from File_LectureActivityD a,File_Student b where SRefNo='" + txtRefNo.Text.Trim() + "' " +
                        "and a.SStudentId=b.SStudentId Order By a.IRecordId");
                    dataGridViewLecture_Activity.Rows.Clear();
                    for (int i = 0; i < newDataSetD.Tables[0].Rows.Count; i++)
                    {
                        dataGridViewLecture_Activity.Rows.Add();
                        dataGridViewLecture_Activity.Rows[i].Cells["NSeqNo"].Value = i + 1;
                        dataGridViewLecture_Activity.Rows[i].Cells["SStudentId"].Value = newDataSetD.Tables[0].Rows[i]["SStudentId"].ToString();
                        dataGridViewLecture_Activity.Rows[i].Cells["SName"].Value = newDataSetD.Tables[0].Rows[i]["SName"].ToString();
                    }

                    newDataSetH = dbCon.Get("Select SRefNo,DDate,a.SCourseCode,a.SLecturerCode,NInTime,NOutTime,SComments,a.IRecordId " +
                        "from File_LectureActivityH a, File_Course b, File_Lecturer c where SRefNo = '"+txtRefNo.Text.Trim()+ "' and " +
                        "a.SCourseCode = b.SCourseCode and a.SCourseCode=b.SCourseCode");

                    txtRefNo.Text = newDataSetH.Tables[0].Rows[0]["SRefNo"].ToString();
                    txtCourseCode.Text = newDataSetH.Tables[0].Rows[0]["SCourseCode"].ToString();
                    txtLecturerCode.Text = newDataSetH.Tables[0].Rows[0]["SLecturerCode"].ToString();

                    txtComments.Text = newDataSetH.Tables[0].Rows[0]["SComments"].ToString();
                    dateTimePickerInv.Text = newDataSetH.Tables[0].Rows[0]["DDate"].ToString().Trim();

                    txtInTime.Text = newDataSetH.Tables[0].Rows[0]["NInTime"].ToString();
                    txtOutTime.Text = newDataSetH.Tables[0].Rows[0]["NOutTime"].ToString();

                }
            }
        }
        private void ButtonControl()
        {
            if (cMode.Equals("NEW") || cMode.Equals("EDIT") || cMode.Equals("APPROVE"))
            {
                this.btnSave.Enabled = true;
                this.btnReset.Enabled = true;
                this.btnAddRow.Enabled = true;
                
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
            this.btnAddRow.Enabled = false;
        }

        private void InitProperties(string cMode)
        {
            if (cMode.Equals("INIT"))
            {
                this.txtCourseCode.ReadOnly = true;
                this.txtComments.ReadOnly = true;
                this.dateTimePickerInv.Enabled = false;
                this.dataGridViewLecture_Activity.ReadOnly = true;

            }
            if (cMode.Equals("DELETE"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtCourseCode.ReadOnly = true;
                this.txtComments.ReadOnly = true;
                this.dateTimePickerInv.Enabled = false;
                this.dataGridViewLecture_Activity.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtCourseCode.ReadOnly = false;
                this.dateTimePickerInv.Enabled = true;
                this.txtComments.ReadOnly = false;

                this.dataGridViewLecture_Activity.ReadOnly = false;

                this.dataGridViewLecture_Activity.Columns[0].ReadOnly = false;
                this.dataGridViewLecture_Activity.Columns[1].ReadOnly = true;
                this.dataGridViewLecture_Activity.Columns[2].ReadOnly = true;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtRefNo.ReadOnly = false;
                this.txtCourseCode.ReadOnly = false;
                this.txtComments.ReadOnly = false;
                this.dateTimePickerInv.Enabled = true;

                this.dataGridViewLecture_Activity.ReadOnly = false;

                this.dataGridViewLecture_Activity.Columns[0].ReadOnly = false;
                this.dataGridViewLecture_Activity.Columns[1].ReadOnly = true;
                this.dataGridViewLecture_Activity.Columns[2].ReadOnly = true;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            this.txtCourseCode.Focus();
            
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.cMode = "EDIT";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_LectureActivityH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Lecture_Activity", "Find Lecture_Activity", this);
            newFind.ShowDialog();

            txtRefNo.Text = newFind.FindKeyValue;
            if (txtRefNo.Text.Trim().Equals(""))
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                return;
            }

            if (!Validate_Transactions())
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
            }
            LoadData();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.cMode = "DELETE";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_LectureActivityH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Lecture_Activity", "Find Lecture_Activity", this);
            newFind.ShowDialog();

            txtRefNo.Text = newFind.FindKeyValue;
            if (txtRefNo.Text.Trim().Equals(""))
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                return;
            }
            if (!Validate_Transactions())
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                LoadData();
            }
            else
            {
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
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_LectureActivityH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Lecture_Activity", "Find Lecture_Activity", this);
            newFind.ShowDialog();

            txtRefNo.Text = newFind.FindKeyValue;
            if (txtRefNo.Text.Trim().Equals(""))
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                return;
            }

            LoadData();
            try
            {
                Print_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Print_Data()
        {

            string cQuery = "Select a.SRefNo,a.DDate,a.SStudentId,a.SComments,a.SRepCode,a.NPayInDays,a.NTotalDiscount,a.NTotalAmount, "
                + "a.SAddUser,a.DAddDate,b.NSeqNo,b.SCourseCode,b.NRate,b.NDiscount,b.NAmount,c.SName,c.SAddress1,c.SAddress2,c.SAddress3, "
                + "d.SCourseName from File_LectureActivityH a, File_Lecture_ActivityD b,File_Student c, File_Course d  where a.SRefNo = b.SRefNo and "
                + "a.SStudentId = c.SStudentId and d.SCourseCode = b.SCourseCode and a.SRefNo='" + txtRefNo.Text.Trim() + "'";

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet Payment_ListingDataSet = dbCon.Get_ReportData(cQuery, "DT_Lecture_Activity");

            Report_View newReport;
            newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Lecture_Activity_2.rpt", Payment_ListingDataSet, "Lecture_Activity");

            newReport.ShowInTaskbar = false;
            newReport.ShowDialog();

            this.cMode = "INIT";
            DisableAllButtons();
            ButtonControl();
            InitProperties(cMode);
            LoadData();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_LectureActivityH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Lecture_Activity", "Find Lecture_Activity", this);
            newFind.ShowDialog();

            txtRefNo.Text = newFind.FindKeyValue;
            if (txtRefNo.Text.Trim().Equals(""))
            {
                this.CMode = "INIT";
                DisableAllButtons();
                ButtonControl();
                InitProperties(CMode);
                return;
            }

            LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate_Entries())
                    if (!Commit_Action())
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

        private void dataGridViewLecture_Activity_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["NDiscount"].Value = 0.00;
            e.Row.Cells["NAmount"].Value = 0.00;
            e.Row.Cells["NRate"].Value = 0.00;
        }

        private void dataGridViewLecture_Activity_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Calculation();
        }

        private void txtStudent_DoubleClick(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SStudentId,SName,SAddress1,rtrim(SAddress2)+', '+ltrim(SAddress3) as SAddress2 from File_Student");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Student", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtCourseCode.Text = newFind.FindKeyValue;
        }

        private void dataGridViewLecture_Activity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.cMode == "NEW" || this.cMode == "EDIT")
            {
                if (dataGridViewLecture_Activity.RowCount > 0)
                {
                    if (e.ColumnIndex == 1)
                    {

                        DatabaseConnection dbCon = new DatabaseConnection();
                        DataSet FillData = dbCon.Get("Select SStudentId,SName,SContactPerson,STelNo,SMobileNo,SEmail,SAdditionalInfo from File_Student");
                        GlobalVariables.HelpDataSet = FillData;

                        Find newFind = new Find("Student", "Find", this);
                        newFind.ShowInTaskbar = false;
                        newFind.ShowDialog();

                        dataGridViewLecture_Activity.CurrentCell.Value = newFind.FindKeyValue;
                        //dataGridViewLecture_Activity.Rows.Add();
                    }
                    this.Calculation();
                }
            }
        }

        private void dataGridViewLecture_Activity_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewLecture_Activity.RowCount > 0)
            {
                if (e.ColumnIndex == 1)
                {
                    if (dataGridViewLecture_Activity.CurrentCell!=null)
                    if (!Validate_Duplicate(dataGridViewLecture_Activity.CurrentCell.Value.ToString().Trim()))
                        return;
                    
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
                    HelpArray[2][0] = "";
                    HelpArray[2][1] = "SAddress1";
                    HelpArray[2][2] = "";

                    HelpArray[3] = new object[3];
                    HelpArray[3][0] = "";
                    HelpArray[3][1] = "SAddress2";
                    HelpArray[3][2] = "";

                    MasterHelp mHelp = new MasterHelp();

                    if (dataGridViewLecture_Activity.CurrentCell != null)
                    {
                        if (!dataGridViewLecture_Activity.CurrentCell.Value.ToString().Trim().Equals(""))
                        {
                            string SWhere = "Where SStudentId='" + dataGridViewLecture_Activity.CurrentCell.Value.ToString().Trim() + "'";
                            object uHelpArray = mHelp.Get_Student(HelpArray, SWhere);

                          //  dataGridViewLecture_Activity.Rows[e.RowIndex].Cells["SStudentId"].Value = HelpArray[1][2].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[e.RowIndex].Cells["SName"].Value = HelpArray[1][2];
                        }
                    }
                }
            }
        }

        private bool Validate_Duplicate(string currentValue) {
            int n = 0;
            if (dataGridViewLecture_Activity.RowCount > 0) {
                for (int i = 0; i < dataGridViewLecture_Activity.RowCount; i++)
                {
                    if (dataGridViewLecture_Activity.Rows[i].Cells[0].Value != null)
                    {
                        if (dataGridViewLecture_Activity.Rows[i].Cells[1].Value != null)
                        {
                            if (dataGridViewLecture_Activity.Rows[i].Cells[1].Value.ToString().Trim().Equals(currentValue))
                            {
                                n++;
                            }
                        }
                    }
                }
                if (n > 1)
                {
                    MessageBox.Show("Duplicate Student Code "+currentValue);
                    dataGridViewLecture_Activity.Rows.RemoveAt(dataGridViewLecture_Activity.CurrentRow.Index);
                    return false;
                }
            }
            return true;
        }

        private void txtPayInDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRefNo,DDate,SStudentId,NTotalDiscount,NTotalAmount,SAddUser from File_InquiryH "
                + "Where SRefNo Not In (Select SInqRef From File_LectureActivityH where SInqRef is not null)");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Inquiry", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            this.cInqNo = newFind.FindKeyValue;

            txtComments.Text = newFind.FindKeyValue;
            if (newFind.FindKeyValue != null)
            {
                if (!newFind.FindKeyValue.Trim().Equals(""))
                {
                    //Get Header Data
                    DataSet FillDataH = dbCon.Get("Select SRefNo,DDate,SStudentId,NTotalDiscount,NTotalAmount from File_InquiryH Where SRefNo='" + newFind.FindKeyValue.Trim() + "'");
                    if (FillDataH.Tables[0].Rows.Count > 0)
                    {
                        this.txtCourseCode.Text = FillDataH.Tables[0].Rows[0]["SStudentId"].ToString().Trim();
                    }
                    //Get Detail Data
                    DataSet FillDataD = dbCon.Get("Select SRefNo,NSeqNo,a.SCourseCode,SCourseName,NRate,NDiscount,a.NAmount from File_InquiryD a,File_Course b where a.SCourseCode=b.SCourseCode and SRefNo='" + newFind.FindKeyValue.Trim() + "'");
                    if (FillDataD.Tables[0].Rows.Count > 0)
                    {
                        dataGridViewLecture_Activity.Rows.Clear();
                        for (int i = 0; i < FillDataD.Tables[0].Rows.Count; i++)
                        {
                            dataGridViewLecture_Activity.Rows.Add();
                            dataGridViewLecture_Activity.Rows[i].Cells["NSeqNo"].Value = FillDataD.Tables[0].Rows[i]["NSeqNo"].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[i].Cells["SCourseCode"].Value = FillDataD.Tables[0].Rows[i]["SCourseCode"].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[i].Cells["SCourseName"].Value = FillDataD.Tables[0].Rows[i]["SCourseName"].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[i].Cells["NRate"].Value = FillDataD.Tables[0].Rows[i]["NRate"].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[i].Cells["NDiscount"].Value = FillDataD.Tables[0].Rows[i]["NDiscount"].ToString().Trim();
                            dataGridViewLecture_Activity.Rows[i].Cells["NAmount"].Value = FillDataD.Tables[0].Rows[i]["NAmount"].ToString().Trim();
                        }
                    }
                }
            }
        }

        private void dataGridViewLecture_Activity_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.dataGridViewLecture_Activity.Rows.Count > 0)
            {
                for (int i = 0; i < this.dataGridViewLecture_Activity.Rows.Count; i++)
                    this.dataGridViewLecture_Activity.Rows[i].Cells[0].Value = i+1;
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewLecture_Activity.RowCount > 0)
            {
                if ((dataGridViewLecture_Activity.Rows[dataGridViewLecture_Activity.RowCount - 1].Cells[1].Value != null) 
                    && (!dataGridViewLecture_Activity.Rows[dataGridViewLecture_Activity.RowCount - 1].Cells[1].Value.ToString().Trim().Equals("")))
                    this.dataGridViewLecture_Activity.Rows.Add();
            }
            else
                this.dataGridViewLecture_Activity.Rows.Add();
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
            HelpArray[2][0] = "";
            HelpArray[2][1] = "STelephoneNo";
            HelpArray[2][2] = "";

            HelpArray[3] = new object[3];
            HelpArray[3][0] = "";
            HelpArray[3][1] = "SMobileNo";
            HelpArray[3][2] = "";
            
            //MasterHelp mHelp = new MasterHelp();
            //string SWhere = "Where SRepCode='" + txtRepCode.Text.Trim() + "'";
            //object uHelpArray = mHelp.Get_SalesPerson(HelpArray, SWhere);

            //this.txtRepCode.Text = HelpArray[0][2].ToString();
            //this.txtSalesPerson.Text = HelpArray[1][2].ToString();
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
            HelpArray[2][0] = "";
            HelpArray[2][1] = "SAddress1";
            HelpArray[2][2] = "";

            HelpArray[3] = new object[3];
            HelpArray[3][0] = "";
            HelpArray[3][1] = "SAddress2";
            HelpArray[3][2] = "";

            MasterHelp mHelp = new MasterHelp();
            string SWhere = "Where SStudentId='" + txtCourseCode.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_Student(HelpArray, SWhere);

            this.txtCourseCode.Text = HelpArray[0][2].ToString();
            this.txtCourseName.Text = HelpArray[1][2].ToString();
        }

        private void txtRepCode_DoubleClick(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRepCode,SRepName from File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Sales Person", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();
            
        }

        private void dataGridViewLecture_Activity_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        //    if (e.ColumnIndex == 6) {
        //        string item = dataGridViewLecture_Activity.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
        //        string path = GlobalVariables.AssetPath + @"Images\Student_Images\" + item + ".jpg";
        //        newImg = new Image_View(path, dataGridViewLecture_Activity.Rows[e.RowIndex].Cells[2].Value.ToString().Trim());
        //        newImg.ShowInTaskbar = false;
        //        newImg.ShowDialog();
        //    }
        }

        private void dataGridViewLecture_Activity_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
