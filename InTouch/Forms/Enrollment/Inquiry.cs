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

namespace InTouch
{
    public partial class Inquiry : Form
    {
        private string cMode;
        DataSet InquiryDataSetH, InquiryDataSetD, InquiryStudentDataSet;
        GenerateReference newReference;

        public string CMode { get => cMode; set => cMode = value; }

        public Inquiry()
        {
            InitializeComponent();
        }

        private void Inquiry_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Inquiry_Load(object sender, EventArgs e)
        {
            this.dataGridViewInquiry.Columns[0].Width = this.dataGridViewInquiry.Width * 5 / 100;
            this.dataGridViewInquiry.Columns[1].Width = this.dataGridViewInquiry.Width * 15 / 100;
            this.dataGridViewInquiry.Columns[2].Width = this.dataGridViewInquiry.Width * 39 / 100;
            this.dataGridViewInquiry.Columns[3].Width = this.dataGridViewInquiry.Width * 12 / 100;
            this.dataGridViewInquiry.Columns[4].Width = this.dataGridViewInquiry.Width * 12 / 100;
            this.dataGridViewInquiry.Columns[5].Width = this.dataGridViewInquiry.Width * 12 / 100;

            DatabaseConnection dbCon = new DatabaseConnection();
            InquiryDataSetH = dbCon.Get("Select SRefNo,DDate,SStudentId,NTotalDiscount,NTotalAmount,SAddUser,DAddDate,SAddMachine from File_InquiryH WHERE 1=0");

            InquiryDataSetD = dbCon.Get("Select SRefNo,NSeqNo,SCourseCode,NRate,NDiscount,NAmount from File_InquiryD WHERE 1=0");

            InquiryStudentDataSet = dbCon.Get("Select SStudentId,SName,SContactPerson,SAddress1,SAddress2,SAddress3,STelNo,SMobileNo,SEmail,SAdditionalInfo,SAddUser,DAddDate,SAddMachine from File_Student Where 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            LoadData();
            ButtonControl();
        }

        private bool Validate_Entries()
        {
            if (txtStudentID.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student ID");
                return false;
            }

            if (txtStudent.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student Name");
                return false;
            }

            for (int i = 0; i < dataGridViewInquiry.RowCount; i++)
            {
                if ((dataGridViewInquiry.Rows[i].Cells[1].Value == null) || (dataGridViewInquiry.Rows[i].Cells[1].Value.ToString().Trim().Equals("")))
                {
                    MessageBox.Show("Invalid Records Will Be Ommited!");
                    dataGridViewInquiry.Rows.RemoveAt(i);
                }
            }

            if (dataGridViewInquiry.Rows.Count <= 0)
            {
                MessageBox.Show("No Valid Records To Save!");
                return false;
            }

            return true;
        }
        private bool Validate_Transactions()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            if (dbCon.Get("Select SRefNo from File_InvoiceH where SInqRef='" + txtRefNo.Text.Trim() + "'").Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Invoice Has Been Raised, Cannot Edit / Delete");
                return false;
            }
            return true;
        }
        private string Get_UniqueID()
        {
            DatabaseConnection newDbConn = new DatabaseConnection();
            newReference = new GenerateReference();
            return newReference.GetReference(newDbConn, "SINQ");
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
                        Saved = newReference.UpdateNumVal(newDbConn, "SINQ");
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
            Fill_DataSet(InquiryDataSetH, InquiryDataSetD, InquiryStudentDataSet);
            if (newDbConn.Upd(InquiryDataSetH, "File_InquiryH"))
            {
                if (this.CMode.Equals("NEW"))
                {
                    newDbConn.Upd(InquiryStudentDataSet, "File_Student");
                }
                return newDbConn.Upd(InquiryDataSetD, "File_InquiryD");
            }
            return false;
        }

        private void Calculation()
        {
            try
            {
                if (dataGridViewInquiry.RowCount > 0)
                {
                    decimal cSubTotal = 0, cDiscount = 0;

                    for (int i = 0; i < dataGridViewInquiry.Rows.Count; i++)
                    {
                        if ((dataGridViewInquiry.Rows[i].Cells["NRate"].Value == null) || (dataGridViewInquiry.Rows[i].Cells["NRate"].Value.ToString().Trim() == ""))
                            dataGridViewInquiry.Rows[i].Cells["NRate"].Value = 0.00;
                        else if (dataGridViewInquiry.Rows[i].Cells["NRate"].Value.GetType() == typeof(String))
                            dataGridViewInquiry.Rows[i].Cells["NRate"].Value = Convert.ToDouble(dataGridViewInquiry.Rows[i].Cells["NRate"].Value);

                        if (dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value == null || dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value.ToString().Trim() == "")
                            dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value = 0.00;
                        else if (dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value.GetType() == typeof(String))
                            dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value = Convert.ToDouble(dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value);
                        
                        cSubTotal += Convert.ToDecimal(dataGridViewInquiry.Rows[i].Cells["NRate"].Value);
                        cDiscount += Convert.ToDecimal(dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value);
                        dataGridViewInquiry.Rows[i].Cells["NAmount"].Value = Convert.ToDouble(dataGridViewInquiry.Rows[i].Cells["NRate"].Value) - Convert.ToDouble(dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value);
                    }

                    this.labelSubTotal.Text = cSubTotal + "";
                    this.labelDiscount.Text = cDiscount + "";
                    this.labelTotal.Text = (cSubTotal - cDiscount) + "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Fill_DataSet(DataSet FillDataSetH, DataSet FillDataSetD,DataSet FillDataSetStudent)
        {
            if (FillDataSetH.Tables[0].Rows.Count > 0)
                FillDataSetH.Tables[0].Rows.RemoveAt(0);

            FillDataSetH.Tables[0].Rows.Add();
            FillDataSetH.Tables[0].Rows[0]["SRefNo"] = txtRefNo.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["DDate"] = dateTimePickerInq.Value.ToShortDateString();
            FillDataSetH.Tables[0].Rows[0]["SStudentId"] = txtStudentID.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["NTotalDiscount"] = labelDiscount.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["NTotalAmount"] = labelTotal.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSetH.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSetH.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;

            if (FillDataSetD.Tables[0].Rows.Count > 0)
                FillDataSetD.Tables[0].Rows.Clear();

            for (int i = 0; i < dataGridViewInquiry.Rows.Count; i++)
            {
                FillDataSetD.Tables[0].Rows.Add();
                FillDataSetD.Tables[0].Rows[i]["SRefNo"] = this.txtRefNo.Text.Trim();
                FillDataSetD.Tables[0].Rows[i]["NSeqNo"] = dataGridViewInquiry.Rows[i].Cells["NSeqNo"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["SCourseCode"] = dataGridViewInquiry.Rows[i].Cells["SCourseCode"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NRate"] = dataGridViewInquiry.Rows[i].Cells["NRate"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NDiscount"] = dataGridViewInquiry.Rows[i].Cells["NDiscount"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NAmount"] = dataGridViewInquiry.Rows[i].Cells["NAmount"].FormattedValue.ToString().Trim();
            }

            if (FillDataSetStudent.Tables[0].Rows.Count > 0)
                FillDataSetStudent.Tables[0].Rows.Clear();

            FillDataSetStudent.Tables[0].Rows.Add();
            FillDataSetStudent.Tables[0].Rows[0]["SStudentId"] = txtStudentID.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SName"] = txtStudent.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SContactPerson"] = txtContactPerson.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SAddress1"] = txtAddress1.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SAddress2"] = txtAddress2.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SAddress3"] = txtAddress3.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["STelNo"] = txtTelNo.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SMobileNo"] = txtMobileNo.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SEmail"] = txtEmail.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SAdditionalInfo"] = txtAdditionalInfo.Text.Trim();
            FillDataSetStudent.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSetStudent.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSetStudent.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;
        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            if (newDbConn.ExecuteQuery("DELETE FROM File_InquiryH where SRefNo = '" + txtRefNo.Text.Trim() + "'"))
                return newDbConn.ExecuteQuery("DELETE FROM File_InquiryD where SRefNo = '" + txtRefNo.Text.Trim() + "'");
            else
                return false;
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO[dbo].[Log_Inquiry] ([SRefNo],[DDate],[SStudentId],[NTotalDiscount],[NTotalAmount],[SAddUser] "
            + ",[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType])(Select[SRefNo],[DDate]"
           + ",[SStudentId],[NTotalDiscount],[NTotalAmount],[SAddUser],[DAddDate],[SAddMachine], '" + GlobalVariables.GetUserName() + "'"
           + ", GetDate(), '" + Environment.MachineName + "', '" + logType + "' from File_InquiryH Where SRefNo = '" + this.txtRefNo.Text + "')");
        }
        private void LoadData()
        {
            DataSet newDataSetH = null;
            DataSet newDataSetD = null;

            if (cMode.Equals("INIT"))
            {                
                this.txtRefNo.Text = "";
                this.txtStudent.Text = "";
                this.txtStudentID.Text = "";
                this.txtContactPerson.Text = "";
                this.txtAddress1.Text = "";
                this.txtAddress2.Text = "";
                this.txtAddress3.Text = "";
                this.txtTelNo.Text = "";
                this.txtMobileNo.Text = "";
                this.txtEmail.Text = "";
                this.txtAdditionalInfo.Text = "";

                this.labelSubTotal.Text = "0.00";
                this.labelDiscount.Text = "0.00";
                this.labelTotal.Text = "0.00";

                this.dataGridViewInquiry.Rows.Clear();
            }
            else
            {
                if (!txtRefNo.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();
                    newDataSetH = dbCon.Get("Select a.SRefNo,a.DDate,a.SStudentId,b.SContactPerson,b.SName,b.SAddress1,b.SAddress2,b.SAddress3, "
                        + " b.STelNo, b.SMobileNo, b.SEmail, b.SAdditionalInfo, NTotalAmount, NTotalDiscount, "
                        + " (NTotalAmount - NTotalDiscount) as NSubTotal from File_InquiryH a, File_Student b "
                        + " where a.SStudentId = b.SStudentId and a.SRefNo='" + txtRefNo.Text + "'");

                    this.txtRefNo.Text = newDataSetH.Tables[0].Rows[0]["SRefNo"].ToString();
                    this.txtStudent.Text = newDataSetH.Tables[0].Rows[0]["SName"].ToString();
                    this.txtStudentID.Text = newDataSetH.Tables[0].Rows[0]["SStudentId"].ToString();
                    this.txtContactPerson.Text = newDataSetH.Tables[0].Rows[0]["SContactPerson"].ToString();
                    this.txtAddress1.Text = newDataSetH.Tables[0].Rows[0]["SAddress1"].ToString();
                    this.txtAddress2.Text = newDataSetH.Tables[0].Rows[0]["SAddress2"].ToString();
                    this.txtAddress3.Text = newDataSetH.Tables[0].Rows[0]["SAddress3"].ToString();
                    this.txtTelNo.Text = newDataSetH.Tables[0].Rows[0]["STelNo"].ToString();
                    this.txtMobileNo.Text = newDataSetH.Tables[0].Rows[0]["SMobileNo"].ToString();
                    this.txtEmail.Text = newDataSetH.Tables[0].Rows[0]["SEmail"].ToString();
                    this.txtAdditionalInfo.Text = newDataSetH.Tables[0].Rows[0]["SAdditionalInfo"].ToString();

                    this.labelSubTotal.Text = newDataSetH.Tables[0].Rows[0]["NSubTotal"].ToString(); ;
                    this.labelDiscount.Text = newDataSetH.Tables[0].Rows[0]["NTotalDiscount"].ToString(); ;
                    this.labelTotal.Text = newDataSetH.Tables[0].Rows[0]["NTotalAmount"].ToString(); ;

                    newDataSetD = dbCon.Get("Select a.SRefNo,a.NSeqNo,a.SCourseCode,b.SCourseName,a.NRate,a.NDiscount,a.NAmount from File_InquiryD a,File_Course b "
                        + " Where a.SCourseCode = b.SCourseCode and a.SRefNo = '" + txtRefNo.Text + "' Order By NSeqNo");

                    dataGridViewInquiry.Rows.Clear();
                    for (int i = 0; i < newDataSetD.Tables[0].Rows.Count; i++)
                    {
                        dataGridViewInquiry.Rows.Add();
                        dataGridViewInquiry.Rows[i].Cells["NSeqNo"].Value = newDataSetD.Tables[0].Rows[i]["NSeqNo"].ToString().Trim();
                        dataGridViewInquiry.Rows[i].Cells["SCourseCode"].Value = newDataSetD.Tables[0].Rows[i]["SCourseCode"].ToString().Trim();
                        dataGridViewInquiry.Rows[i].Cells["SCourseName"].Value = newDataSetD.Tables[0].Rows[i]["SCourseName"].ToString().Trim();
                        dataGridViewInquiry.Rows[i].Cells["NRate"].Value = newDataSetD.Tables[0].Rows[i]["NRate"].ToString().Trim();
                        dataGridViewInquiry.Rows[i].Cells["NDiscount"].Value = newDataSetD.Tables[0].Rows[i]["NDiscount"].ToString().Trim();
                        dataGridViewInquiry.Rows[i].Cells["NAmount"].Value = newDataSetD.Tables[0].Rows[i]["NAmount"].ToString().Trim();
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
                this.btnAddRow.Enabled = true;
            }
            if (cMode.Equals("PRINT"))
            {
                this.btnReset.Enabled = true;
                this.btnAddRow.Enabled = false;
            }
            if (cMode.Equals("DELETE"))
            {
                this.btnReset.Enabled = true;
                this.btnAddRow.Enabled = false;
            }
            if (cMode.Equals("VIEW"))
            {
                this.btnReset.Enabled = true;
                this.btnAddRow.Enabled = false;
            }
            if (cMode.Equals("INIT"))
            {
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnFind.Enabled = true;
                this.btnAddRow.Enabled = false;
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
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtStudentID.ReadOnly = true;
                this.txtContactPerson.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
                this.txtAdditionalInfo.ReadOnly = true;
                this.dateTimePickerInq.Enabled = false;
                this.dataGridViewInquiry.ReadOnly = true;
            }
            if (cMode.Equals("DELETE"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtStudentID.ReadOnly = true;
                this.txtContactPerson.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
                this.txtAdditionalInfo.ReadOnly = true;
                this.dateTimePickerInq.Enabled = false;
                this.dataGridViewInquiry.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = false;
                this.txtStudentID.ReadOnly = false;
                this.txtContactPerson.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtAddress3.ReadOnly = false;
                this.txtTelNo.ReadOnly = false;
                this.txtMobileNo.ReadOnly = false;
                this.txtEmail.ReadOnly = false;
                this.txtAdditionalInfo.ReadOnly = false;
                this.dateTimePickerInq.Enabled = true;
                this.dataGridViewInquiry.ReadOnly = false;

                this.dataGridViewInquiry.Columns[0].ReadOnly = false;
                this.dataGridViewInquiry.Columns[1].ReadOnly = true;
                this.dataGridViewInquiry.Columns[2].ReadOnly = true;
                this.dataGridViewInquiry.Columns[3].ReadOnly = true;
                this.dataGridViewInquiry.Columns[4].ReadOnly = false;
                this.dataGridViewInquiry.Columns[5].ReadOnly = true;
            }
            if (cMode.Equals("EDIT"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtStudentID.ReadOnly = true;
                this.txtContactPerson.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtAddress3.ReadOnly = true;
                this.txtTelNo.ReadOnly = true;
                this.txtMobileNo.ReadOnly = true;
                this.txtEmail.ReadOnly = true;
                this.txtAdditionalInfo.ReadOnly = true;
                this.dataGridViewInquiry.ReadOnly = false;
                this.dateTimePickerInq.Enabled = false;

                this.dataGridViewInquiry.Columns[0].ReadOnly = false;
                this.dataGridViewInquiry.Columns[1].ReadOnly = true;
                this.dataGridViewInquiry.Columns[2].ReadOnly = true;
                this.dataGridViewInquiry.Columns[3].ReadOnly = true;
                this.dataGridViewInquiry.Columns[4].ReadOnly = false;
                this.dataGridViewInquiry.Columns[5].ReadOnly = true;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            this.txtStudentID.Focus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.cMode = "EDIT";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_InquiryH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Inquiry", "Find Inquiry", this);
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
            DataSet FillData = dbCon.Get("Select * from File_InquiryH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Inquiry", "Find Inquiry", this);
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

            //this.cMode = "VIEW";
            //DisableAllButtons();
            //ButtonControl();
            //this.InitProperties(cMode);

            //DatabaseConnection dbCon = new DatabaseConnection();
            //DataSet FillData = dbCon.Get("Select * from File_InquiryH");
            //GlobalVariables.HelpDataSet = FillData;

            //Find newFind = new Find("Inquiry", "Find Inquiry", this);
            //newFind.ShowDialog();

            //txtRefNo.Text = newFind.FindKeyValue;
            //if (txtRefNo.Text.Trim().Equals(""))
            //{
            //    this.CMode = "INIT";
            //    DisableAllButtons();
            //    ButtonControl();
            //    InitProperties(CMode);
            //    return;
            //}

            //LoadData();

            //string cQuery = "Select a.SRefNo,a.DDate,a.SStudentId,a.SComments,a.SRepCode,a.NPayInDays,a.NTotalDiscount,a.NTotalAmount, "
            //    + "a.SAddUser,a.DAddDate,b.NSeqNo,b.SCourseCode,b.NRate,b.NDiscount,b.NAmount,c.SName,c.SAddress1,c.SAddress2,c.SAddress3, "
            //    + "d.SCourseName from File_InquiryH a, File_InquiryD b,File_Student c, File_Course d  where a.SRefNo = b.SRefNo and "
            //    + "a.SStudentId = c.SStudentId and d.SCourseCode = b.SCourseCode and a.SRefNo='" + txtRefNo.Text.Trim() + "'";

            //DataSet Payment_ListingDataSet = dbCon.Get_ReportData(cQuery, "DT_Inquiry");

            //Report_View newReport;

            //newReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Inquiry.rpt", Payment_ListingDataSet, "Inquiry");

            //newReport.ShowInTaskbar = false;
            //newReport.ShowDialog();

            //this.cMode = "INIT";
            //DisableAllButtons();
            //ButtonControl();
            //InitProperties(cMode);
            //LoadData();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_InquiryH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Inquiry", "Find Inquiry", this);
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

        private void dataGridViewInquiry_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["NDiscount"].Value = 0.00;
            e.Row.Cells["NAmount"].Value = 0.00;
            e.Row.Cells["NRate"].Value = 0.00;
        }

        private void dataGridViewInquiry_CellEnter(object sender, DataGridViewCellEventArgs e)
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

            //txtStudent.Text = newFind.FindKeyValue;
        }

        private void dataGridViewInquiry_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.cMode == "NEW" || this.cMode == "EDIT")
            {
                if (dataGridViewInquiry.RowCount > 0)
                {
                    if (e.ColumnIndex == 1)
                    {

                        DatabaseConnection dbCon = new DatabaseConnection();
                        DataSet FillData = dbCon.Get("Select SCourseCode,SCourseName,NAmount,NCreditPeriod from File_Course");
                        GlobalVariables.HelpDataSet = FillData;

                        Find newFind = new Find("Student", "Find", this);
                        newFind.ShowInTaskbar = false;
                        newFind.ShowDialog();

                        dataGridViewInquiry.CurrentCell.Value = newFind.FindKeyValue;
                    }
                    this.Calculation();
                }
            }
        }

        private void dataGridViewInquiry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewInquiry.RowCount > 0)
            {
                if (e.ColumnIndex == 1)
                {
                    if (dataGridViewInquiry.CurrentCell!=null)
                    if (!Validate_Duplicate(dataGridViewInquiry.CurrentCell.Value.ToString().Trim()))
                        return;
                    

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
                    HelpArray[2][0] = "";
                    HelpArray[2][1] = "NAmount";
                    HelpArray[2][2] = "";

                    HelpArray[3] = new object[3];
                    HelpArray[3][0] = "";
                    HelpArray[3][1] = "NCreditPeriod";
                    HelpArray[3][2] = "";

                    MasterHelp mHelp = new MasterHelp();

                    if (dataGridViewInquiry.Rows[e.RowIndex].Cells[e.ColumnIndex] != null)
                    {
                        if (!dataGridViewInquiry.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim().Equals(""))
                        {
                            string SWhere = "Where SCourseCode='" + dataGridViewInquiry.CurrentCell.Value.ToString().Trim() + "'";
                            object uHelpArray = mHelp.Get_Course(HelpArray, SWhere);

                            dataGridViewInquiry.Rows[e.RowIndex].Cells["SCourseName"].Value = HelpArray[1][2].ToString();
                            dataGridViewInquiry.Rows[e.RowIndex].Cells["NRate"].Value = HelpArray[2][2];
                        }
                        this.Calculation();
                    }
                }
                if (e.ColumnIndex == 4)
                    if (dataGridViewInquiry.CurrentCell != null)
                    {
                        this.Calculation();
                    }
            }
        }

        private bool Validate_Duplicate(string currentValue) {
            int n = 0;
            if (dataGridViewInquiry.RowCount > 0) {
                for (int i = 0; i < dataGridViewInquiry.RowCount; i++) {
                    if (dataGridViewInquiry.Rows[i].Cells[0].Value != null)
                    {
                        if (dataGridViewInquiry.Rows[i].Cells[1].Value.ToString().Trim().Equals(currentValue)) {
                            n++;
                        }
                    }
                }
                if (n > 1)
                {
                    MessageBox.Show("Duplicate Course Code "+currentValue);
                    dataGridViewInquiry.Rows.RemoveAt(dataGridViewInquiry.CurrentRow.Index);
                    return false;
                }
            }
            return true;
        }

        private void txtPayInDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            //if (ch == 46 && txtPayInDays.Text.IndexOf('.') != -1)
            //{
            //    e.Handled = true;
            //    return;
            //}

            //if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            //{
            //    e.Handled = true;
            //}
        }

        private void txtStudent_TextChanged(object sender, EventArgs e)
        {
            //object[][] HelpArray = new object[4][];

            //HelpArray[0] = new object[3];
            //HelpArray[0][0] = "";
            //HelpArray[0][1] = "SStudentId";
            //HelpArray[0][2] = "";

            //HelpArray[1] = new object[3];
            //HelpArray[1][0] = "";
            //HelpArray[1][1] = "SName";
            //HelpArray[1][2] = "";

            //HelpArray[2] = new object[3];
            //HelpArray[2][0] = "";
            //HelpArray[2][1] = "SAddress1";
            //HelpArray[2][2] = "";

            //HelpArray[3] = new object[3];
            //HelpArray[3][0] = "";
            //HelpArray[3][1] = "SAddress2";
            //HelpArray[3][2] = "";

            //MasterHelp mHelp = new MasterHelp();
            ////string SWhere = "Where SStudentId='" + txtStudent.Text.Trim() + "'";
            ////object uHelpArray = mHelp.Get_Student(HelpArray, SWhere);

            ////this.txtStudent.Text = HelpArray[0][2].ToString();
            ////this.txtStudentName.Text = HelpArray[1][2].ToString();
            ////this.txtAddress1.Text = HelpArray[2][2].ToString();
            //this.txtAddress2.Text = HelpArray[3][2].ToString();
        }

        private void txtContactPerson_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerInv_ValueChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txtStudentID_Leave(object sender, EventArgs e)
        {
            if (this.CMode.Equals("New"))
            {
                string KeyValue = this.txtStudentID.Text.Trim();
                if (KeyValue.Equals(""))
                    MessageBox.Show("Invalid Student Name");
                else if (!Validate_Duplicate())
                {
                    MessageBox.Show(KeyValue + " Exists, Cannot Duplicate!");
                    this.txtStudentID.Text = "";
                }
            }
        }
        private bool Validate_Duplicate()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet newDataSet = dbCon.Get("Select * from File_Student where SStudentId = '" + txtStudentID.Text.Trim() + "'");
            if (newDataSet.Tables[0].Rows.Count > 0)
                return false;
            else
                return true;
        }

        private void dataGridViewInquiry_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.dataGridViewInquiry.Rows.Count > 0)
            {
                for (int i = 0; i < this.dataGridViewInquiry.Rows.Count; i++)
                    this.dataGridViewInquiry.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewInquiry.RowCount > 0)
            {
                if ((dataGridViewInquiry.Rows[dataGridViewInquiry.RowCount - 1].Cells[1].Value != null) 
                    && (!dataGridViewInquiry.Rows[dataGridViewInquiry.RowCount - 1].Cells[1].Value.ToString().Trim().Equals("")))
                    this.dataGridViewInquiry.Rows.Add();
            }
            else
                this.dataGridViewInquiry.Rows.Add();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
