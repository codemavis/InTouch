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
    public partial class Invoice : Form
    {
        private string cMode;
        DataSet InvoiceDataSetH, InvoiceDataSetD;
        GenerateReference newReference;
        private string cInqNo="";
      //  Image_View newImg;

        public string CMode { get => cMode; set => cMode = value; }

        public Invoice()
        {
            InitializeComponent();            
        }

        private void Invoice_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        
        private void Invoice_Load(object sender, EventArgs e)
        {
            this.dataGridViewInvoice.Columns[0].Width = this.dataGridViewInvoice.Width * 5 / 100;
            this.dataGridViewInvoice.Columns[1].Width = this.dataGridViewInvoice.Width * 15 / 100;
            this.dataGridViewInvoice.Columns[2].Width = this.dataGridViewInvoice.Width * 39 / 100;
            this.dataGridViewInvoice.Columns[3].Width = this.dataGridViewInvoice.Width * 12 / 100;
            this.dataGridViewInvoice.Columns[4].Width = this.dataGridViewInvoice.Width * 12 / 100;
            this.dataGridViewInvoice.Columns[5].Width = this.dataGridViewInvoice.Width * 12 / 100;

            DatabaseConnection dbCon = new DatabaseConnection();
            InvoiceDataSetH = dbCon.Get("Select SRefNo,DDate,SStudentId,SComments,SRepCode,NPayInDays,NTotalDiscount,"
                + "NTotalAmount,SAddUser,DAddDate,SAddMachine,NBalanceAmount,SInqRef from File_InvoiceH WHERE 1=0");

            InvoiceDataSetD = dbCon.Get("Select SRefNo,NSeqNo,SCourseCode,NRate,NDiscount,NAmount from File_InvoiceD WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            LoadData();
            ButtonControl();
        }

        private bool Validate_Entries()
        {
            if (txtStudent.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student Code");
                return false;
            }

            for (int i = 0; i < dataGridViewInvoice.RowCount; i++)
            {
                if ((dataGridViewInvoice.Rows[i].Cells[1].Value == null)
                    || (dataGridViewInvoice.Rows[i].Cells[1].Value.ToString().Trim().Equals(""))
                    || (dataGridViewInvoice.Rows[i].Cells[3].Value == null)
                    || (dataGridViewInvoice.Rows[i].Cells[3].Value.ToString().Trim().Equals("")))
                {
                    MessageBox.Show("Invalid Records Will Be Ommited!");
                    dataGridViewInvoice.Rows.RemoveAt(i);
                }
            }

            if (dataGridViewInvoice.Rows.Count <= 0)
            {
                MessageBox.Show("No Valid Records To Save!");
                return false;
            }

            if (txtPayInDays.Text.Trim().Equals(""))
                txtPayInDays.Text = "0.00";

            return true;
        }

        private bool Validate_Transactions()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            if (dbCon.Get("Select SRefNo from File_PaymentD where SInvNo='" + txtRefNo.Text.Trim() + "'").Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Payment Has Been Done, Cannot Edit / Delete");
                return false;
            }
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
            Fill_DataSet(InvoiceDataSetH, InvoiceDataSetD);
            if (newDbConn.Upd(InvoiceDataSetH, "File_InvoiceH"))
                return newDbConn.Upd(InvoiceDataSetD, "File_InvoiceD");
            else
                return false;
        }

        private void Calculation()
        {
            try
            {
                if (dataGridViewInvoice.RowCount > 0)
                {
                    decimal cSubTotal = 0, cDiscount = 0;

                    for (int i = 0; i < dataGridViewInvoice.Rows.Count; i++)
                    {
                        if ((dataGridViewInvoice.Rows[i].Cells["NRate"].Value == null) || (dataGridViewInvoice.Rows[i].Cells["NRate"].Value.ToString().Trim() == ""))
                            dataGridViewInvoice.Rows[i].Cells["NRate"].Value = 0.00;
                        else if (dataGridViewInvoice.Rows[i].Cells["NRate"].Value.GetType() == typeof(String))
                            dataGridViewInvoice.Rows[i].Cells["NRate"].Value = Convert.ToDouble(dataGridViewInvoice.Rows[i].Cells["NRate"].Value);

                        if (dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value == null || dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value.ToString().Trim() == "")
                            dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value = 0.00;
                        else if (dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value.GetType() == typeof(String))
                            dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value = Convert.ToDouble(dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value);


                        cSubTotal += Convert.ToDecimal(dataGridViewInvoice.Rows[i].Cells["NRate"].Value);
                        cDiscount += Convert.ToDecimal(dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value);
                        dataGridViewInvoice.Rows[i].Cells["NAmount"].Value = Convert.ToDouble(dataGridViewInvoice.Rows[i].Cells["NRate"].Value) - Convert.ToDouble(dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value);
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
        private void Fill_DataSet(DataSet FillDataSetH, DataSet FillDataSetD)
        {
            if (FillDataSetH.Tables[0].Rows.Count > 0)
                FillDataSetH.Tables[0].Rows.RemoveAt(0);

            string nPayInDays="0";
            if (!txtPayInDays.Text.Trim().Equals(""))
                nPayInDays = txtPayInDays.Text.Trim();


            FillDataSetH.Tables[0].Rows.Add();
            FillDataSetH.Tables[0].Rows[0]["SRefNo"] = txtRefNo.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["DDate"] = dateTimePickerInv.Value.ToShortDateString();
            FillDataSetH.Tables[0].Rows[0]["SStudentId"] = txtStudent.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SComments"] = txtComments.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SRepCode"] = txtRepCode.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["NPayInDays"] = nPayInDays;
            FillDataSetH.Tables[0].Rows[0]["NTotalDiscount"] = labelDiscount.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["NTotalAmount"] = labelTotal.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSetH.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSetH.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;
            FillDataSetH.Tables[0].Rows[0]["NBalanceAmount"] = labelTotal.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SInqRef"] = this.cInqNo;


            if (FillDataSetD.Tables[0].Rows.Count > 0)
                FillDataSetD.Tables[0].Rows.Clear();

            for (int i = 0; i < dataGridViewInvoice.Rows.Count; i++)
            {
                FillDataSetD.Tables[0].Rows.Add();
                FillDataSetD.Tables[0].Rows[i]["SRefNo"] = this.txtRefNo.Text.Trim();
                FillDataSetD.Tables[0].Rows[i]["NSeqNo"] = dataGridViewInvoice.Rows[i].Cells["NSeqNo"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["SCourseCode"] = dataGridViewInvoice.Rows[i].Cells["SCourseCode"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NRate"] = dataGridViewInvoice.Rows[i].Cells["NRate"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NDiscount"] = dataGridViewInvoice.Rows[i].Cells["NDiscount"].FormattedValue.ToString().Trim();
                FillDataSetD.Tables[0].Rows[i]["NAmount"] = dataGridViewInvoice.Rows[i].Cells["NAmount"].FormattedValue.ToString().Trim();
            }
        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            if (newDbConn.ExecuteQuery("DELETE FROM File_InvoiceH where SRefNo = '" + txtRefNo.Text + "'"))
                return newDbConn.ExecuteQuery("DELETE FROM File_InvoiceD where SRefNo = '" + txtRefNo.Text + "'");
            else
                return false;
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_Invoice]([SRefNo],[DDate],[SStudentId],[SComments],[SRepCode],[NPayInDays],"
                + "[NTotalDiscount],[NTotalAmount],[SAddUser],[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType]) (Select [SRefNo] "
                + ",[DDate],[SStudentId],[SComments],[SRepCode],[NPayInDays],[NTotalDiscount],[NTotalAmount],[SAddUser],[DAddDate],[SAddMachine]"
                + ", '" + GlobalVariables.GetUserName() + "', GetDate(), '" + Environment.MachineName + "', '" + logType
                + "' from File_InvoiceH Where SRefNo='" + this.txtRefNo.Text + "')");
        }
        private void LoadData()
        {
            DataSet newDataSetH = null;
            DataSet newDataSetD = null;

            if (cMode.Equals("INIT"))
            {
                this.txtRefNo.Text = "";
                this.txtStudent.Text = "";
                this.txtStudentName.Text = "";
                this.txtAddress1.Text = "";
                this.txtAddress2.Text = "";
                this.txtSalesPerson.Text = "";
                this.txtRepCode.Text = "";
                this.txtTerms.Text = "";
                this.txtPayInDays.Text = "";
                this.txtComments.Text = "";

                this.labelSubTotal.Text = "0.00";
                this.labelDiscount.Text = "0.00";
                this.labelTotal.Text = "0.00";

                this.dataGridViewInvoice.Rows.Clear();
            }
            else
            {
                if (!txtRefNo.Text.Trim().Equals(""))
                {
                    DatabaseConnection dbCon = new DatabaseConnection();
                    
                    newDataSetD = dbCon.Get("Select a.SRefNo,a.NSeqNo,a.SCourseCode,b.SCourseName,a.NRate,a.NDiscount,a.NAmount from File_InvoiceD a,File_Course b "
                        + " Where a.SCourseCode = b.SCourseCode and a.SRefNo = '" + txtRefNo.Text + "' Order By NSeqNo");
                    dataGridViewInvoice.Rows.Clear();
                    for (int i = 0; i < newDataSetD.Tables[0].Rows.Count; i++)
                    {
                        dataGridViewInvoice.Rows.Add();
                        dataGridViewInvoice.Rows[i].Cells["NSeqNo"].Value = newDataSetD.Tables[0].Rows[i]["NSeqNo"].ToString();
                        dataGridViewInvoice.Rows[i].Cells["SCourseCode"].Value = newDataSetD.Tables[0].Rows[i]["SCourseCode"].ToString();
                        dataGridViewInvoice.Rows[i].Cells["SCourseName"].Value = newDataSetD.Tables[0].Rows[i]["SCourseName"].ToString();
                        dataGridViewInvoice.Rows[i].Cells["NRate"].Value = newDataSetD.Tables[0].Rows[i]["NRate"].ToString();
                        dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value = newDataSetD.Tables[0].Rows[i]["NDiscount"].ToString();
                        dataGridViewInvoice.Rows[i].Cells["NAmount"].Value = newDataSetD.Tables[0].Rows[i]["NAmount"].ToString();
                    }

                    newDataSetH = dbCon.Get("Select a.*,b.SName,b.SAddress1,(rtrim(b.SAddress2)+', '+rtrim(b.SAddress3)) as SAddress2"
                        + " from File_InvoiceH a, File_Student b where a.SStudentId = b.SStudentId and a.SRefNo='" + txtRefNo.Text + "'");

                    txtRefNo.Text = newDataSetH.Tables[0].Rows[0]["SRefNo"].ToString();
                    txtStudent.Text = newDataSetH.Tables[0].Rows[0]["SStudentId"].ToString();
                    txtAddress1.Text = newDataSetH.Tables[0].Rows[0]["SAddress1"].ToString();
                    txtAddress2.Text = newDataSetH.Tables[0].Rows[0]["SAddress2"].ToString();
                    txtRepCode.Text = newDataSetH.Tables[0].Rows[0]["SRepCode"].ToString();
                    txtTerms.Text = "Pay In Days";
                    txtPayInDays.Text = newDataSetH.Tables[0].Rows[0]["NPayInDays"].ToString();
                    txtComments.Text = newDataSetH.Tables[0].Rows[0]["SComments"].ToString();
                    dateTimePickerInv.Text = newDataSetH.Tables[0].Rows[0]["DDate"].ToString().Trim();

                    this.labelDiscount.Text = newDataSetH.Tables[0].Rows[0]["NTotalDiscount"].ToString();
                    this.labelTotal.Text = newDataSetH.Tables[0].Rows[0]["NTotalAmount"].ToString();
                    this.labelSubTotal.Text = (Convert.ToDouble(this.labelTotal.Text) - Convert.ToDouble(this.labelDiscount.Text)) + "";
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

                if (cMode.Equals("NEW"))
                    this.btnInquiry.Enabled = true;
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
            this.btnInquiry.Enabled = false;
            this.btnAddRow.Enabled = false;
        }

        private void InitProperties(string cMode)
        {
            if (cMode.Equals("INIT"))
            {
                this.txtStudent.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtRepCode.ReadOnly = true;
                this.txtTerms.ReadOnly = true;
                this.txtPayInDays.ReadOnly = true;
                this.txtComments.ReadOnly = true;
                this.dateTimePickerInv.Enabled = false;
                this.dataGridViewInvoice.ReadOnly = true;

            }
            if (cMode.Equals("DELETE"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtAddress1.ReadOnly = true;
                this.txtAddress2.ReadOnly = true;
                this.txtRepCode.ReadOnly = true;
                this.txtTerms.ReadOnly = true;
                this.txtPayInDays.ReadOnly = true;
                this.txtComments.ReadOnly = true;
                this.dateTimePickerInv.Enabled = false;
                this.dataGridViewInvoice.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtRepCode.ReadOnly = false;
                this.txtTerms.ReadOnly = false;
                this.txtPayInDays.ReadOnly = false;
                this.dateTimePickerInv.Enabled = true;
                this.txtComments.ReadOnly = false;

                this.dataGridViewInvoice.ReadOnly = false;

                this.dataGridViewInvoice.Columns[0].ReadOnly = false;
                this.dataGridViewInvoice.Columns[1].ReadOnly = true;
                this.dataGridViewInvoice.Columns[2].ReadOnly = true;
                this.dataGridViewInvoice.Columns[3].ReadOnly = true;
                this.dataGridViewInvoice.Columns[4].ReadOnly = false;
                this.dataGridViewInvoice.Columns[5].ReadOnly = true;
            }
            if (cMode.Equals("EDIT"))
            {
                this.txtRefNo.ReadOnly = false;
                this.txtStudent.ReadOnly = false;
                this.txtAddress1.ReadOnly = false;
                this.txtAddress2.ReadOnly = false;
                this.txtRepCode.ReadOnly = false;
                this.txtTerms.ReadOnly = false;
                this.txtPayInDays.ReadOnly = false;
                this.txtComments.ReadOnly = false;
                this.dateTimePickerInv.Enabled = true;

                this.dataGridViewInvoice.ReadOnly = false;

                this.dataGridViewInvoice.Columns[0].ReadOnly = false;
                this.dataGridViewInvoice.Columns[1].ReadOnly = true;
                this.dataGridViewInvoice.Columns[2].ReadOnly = true;
                this.dataGridViewInvoice.Columns[3].ReadOnly = true;
                this.dataGridViewInvoice.Columns[4].ReadOnly = false;
                this.dataGridViewInvoice.Columns[5].ReadOnly = true;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            LoadData();

            this.txtStudent.Focus();

            this.txtTerms.Text = "Pay In Days";
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.cMode = "EDIT";
            DisableAllButtons();
            ButtonControl();
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select * from File_InvoiceH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Invoice", "Find Invoice", this);
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
            DataSet FillData = dbCon.Get("Select * from File_InvoiceH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Invoice", "Find Invoice", this);
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
            DataSet FillData = dbCon.Get("Select * from File_InvoiceH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Invoice", "Find Invoice", this);
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
                + "d.SCourseName from File_InvoiceH a, File_InvoiceD b,File_Student c, File_Course d  where a.SRefNo = b.SRefNo and "
                + "a.SStudentId = c.SStudentId and d.SCourseCode = b.SCourseCode and a.SRefNo='" + txtRefNo.Text.Trim() + "'";

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet Payment_ListingDataSet = dbCon.Get_ReportData(cQuery, "DT_Invoice");

            Report_View newReport;

            if (this.radioButton1.Checked)
                newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Invoice.rpt", Payment_ListingDataSet, "Invoice");
            else
                newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Invoice_2.rpt", Payment_ListingDataSet, "Invoice");

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
            DataSet FillData = dbCon.Get("Select * from File_InvoiceH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Invoice", "Find Invoice", this);
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

        private void dataGridViewInvoice_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["NDiscount"].Value = 0.00;
            e.Row.Cells["NAmount"].Value = 0.00;
            e.Row.Cells["NRate"].Value = 0.00;
        }

        private void dataGridViewInvoice_CellEnter(object sender, DataGridViewCellEventArgs e)
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

            txtStudent.Text = newFind.FindKeyValue;
        }

        private void dataGridViewInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.cMode == "NEW" || this.cMode == "EDIT")
            {
                if (dataGridViewInvoice.RowCount > 0)
                {
                    if (e.ColumnIndex == 1)
                    {

                        DatabaseConnection dbCon = new DatabaseConnection();
                        DataSet FillData = dbCon.Get("Select SCourseCode,SCourseName,NAmount,NCreditPeriod from File_Course");
                        GlobalVariables.HelpDataSet = FillData;

                        Find newFind = new Find("Student", "Find", this);
                        newFind.ShowInTaskbar = false;
                        newFind.ShowDialog();

                        dataGridViewInvoice.CurrentCell.Value = newFind.FindKeyValue;
                        //dataGridViewInvoice.Rows.Add();
                    }
                    this.Calculation();
                }
            }
        }

        //private bool Validate_Duplicate(string currentValue)
        //{
        //    int n = 0;
        //    if (dataGridViewInvoice.RowCount > 0)
        //    {
        //        for (int i = 0; i < dataGridViewInvoice.RowCount; i++)
        //        {
        //            if (dataGridViewInvoice.Rows[i].Cells[0].Value != null)
        //            {
        //                if (dataGridViewInvoice.Rows[i].Cells[1].Value.ToString().Trim().Equals(currentValue))
        //                {
        //                    n++;
        //                }
        //            }
        //        }
        //        if (n > 1)
        //        {
        //            MessageBox.Show("Duplicate Course Code " + currentValue);
        //            dataGridViewInvoice.Rows.RemoveAt(dataGridViewInvoice.CurrentRow.Index);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private void dataGridViewInvoice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewInvoice.RowCount > 0)
            {
                if (e.ColumnIndex == 1)
                {
                    if (dataGridViewInvoice.CurrentCell!=null)
                    if (!Validate_Duplicate(dataGridViewInvoice.CurrentCell.Value.ToString().Trim()))
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

                    if (dataGridViewInvoice.CurrentCell != null)
                    {
                        if (!dataGridViewInvoice.CurrentCell.Value.ToString().Trim().Equals(""))
                        {
                            string SWhere = "Where SCourseCode='" + dataGridViewInvoice.CurrentCell.Value.ToString().Trim() + "'";
                            object uHelpArray = mHelp.Get_Course(HelpArray, SWhere);

                            dataGridViewInvoice.Rows[e.RowIndex].Cells["SCourseName"].Value = HelpArray[1][2].ToString();
                            dataGridViewInvoice.Rows[e.RowIndex].Cells["NRate"].Value = HelpArray[2][2];
                            this.txtPayInDays.Text = HelpArray[3][2].ToString();
                        }
                        this.Calculation();
                    }
                }
                if (e.ColumnIndex == 4)
                    if (dataGridViewInvoice.CurrentCell != null)
                    {
                        this.Calculation();
                    }
            }
        }

        private bool Validate_Duplicate(string currentValue) {
            int n = 0;
            if (dataGridViewInvoice.RowCount > 0) {
                for (int i = 0; i < dataGridViewInvoice.RowCount; i++)
                {
                    if (dataGridViewInvoice.Rows[i].Cells[0].Value != null)
                    {
                        if (dataGridViewInvoice.Rows[i].Cells[1].Value != null)
                        {
                            if (dataGridViewInvoice.Rows[i].Cells[1].Value.ToString().Trim().Equals(currentValue))
                            {
                                n++;
                            }
                        }
                    }
                }
                if (n > 1)
                {
                    MessageBox.Show("Duplicate Course Code "+currentValue);
                    dataGridViewInvoice.Rows.RemoveAt(dataGridViewInvoice.CurrentRow.Index);
                    return false;
                }
            }
            return true;
        }

        private void txtPayInDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && txtPayInDays.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRefNo,DDate,SStudentId,NTotalDiscount,NTotalAmount,SAddUser from File_InquiryH "
                + "Where SRefNo Not In (Select SInqRef From File_InvoiceH where SInqRef is not null)");
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
                        this.txtStudent.Text = FillDataH.Tables[0].Rows[0]["SStudentId"].ToString().Trim();
                    }
                    //Get Detail Data
                    DataSet FillDataD = dbCon.Get("Select SRefNo,NSeqNo,a.SCourseCode,SCourseName,NRate,NDiscount,a.NAmount from File_InquiryD a,File_Course b where a.SCourseCode=b.SCourseCode and SRefNo='" + newFind.FindKeyValue.Trim() + "'");
                    if (FillDataD.Tables[0].Rows.Count > 0)
                    {
                        dataGridViewInvoice.Rows.Clear();
                        for (int i = 0; i < FillDataD.Tables[0].Rows.Count; i++)
                        {
                            dataGridViewInvoice.Rows.Add();
                            dataGridViewInvoice.Rows[i].Cells["NSeqNo"].Value = FillDataD.Tables[0].Rows[i]["NSeqNo"].ToString().Trim();
                            dataGridViewInvoice.Rows[i].Cells["SCourseCode"].Value = FillDataD.Tables[0].Rows[i]["SCourseCode"].ToString().Trim();
                            dataGridViewInvoice.Rows[i].Cells["SCourseName"].Value = FillDataD.Tables[0].Rows[i]["SCourseName"].ToString().Trim();
                            dataGridViewInvoice.Rows[i].Cells["NRate"].Value = FillDataD.Tables[0].Rows[i]["NRate"].ToString().Trim();
                            dataGridViewInvoice.Rows[i].Cells["NDiscount"].Value = FillDataD.Tables[0].Rows[i]["NDiscount"].ToString().Trim();
                            dataGridViewInvoice.Rows[i].Cells["NAmount"].Value = FillDataD.Tables[0].Rows[i]["NAmount"].ToString().Trim();
                        }
                    }
                }
            }
        }

        private void dataGridViewInvoice_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.dataGridViewInvoice.Rows.Count > 0)
            {
                for (int i = 0; i < this.dataGridViewInvoice.Rows.Count; i++)
                    this.dataGridViewInvoice.Rows[i].Cells[0].Value = i+1;
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewInvoice.RowCount > 0)
            {
                if ((dataGridViewInvoice.Rows[dataGridViewInvoice.RowCount - 1].Cells[1].Value != null) 
                    && (!dataGridViewInvoice.Rows[dataGridViewInvoice.RowCount - 1].Cells[1].Value.ToString().Trim().Equals("")))
                    this.dataGridViewInvoice.Rows.Add();
            }
            else
                this.dataGridViewInvoice.Rows.Add();
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


            MasterHelp mHelp = new MasterHelp();
            string SWhere = "Where SRepCode='" + txtRepCode.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_SalesPerson(HelpArray, SWhere);

            this.txtRepCode.Text = HelpArray[0][2].ToString();
            this.txtSalesPerson.Text = HelpArray[1][2].ToString();
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
            string SWhere = "Where SStudentId='" + txtStudent.Text.Trim() + "'";
            object uHelpArray = mHelp.Get_Student(HelpArray, SWhere);

            this.txtStudent.Text = HelpArray[0][2].ToString();
            this.txtStudentName.Text = HelpArray[1][2].ToString();
            this.txtAddress1.Text = HelpArray[2][2].ToString();
            this.txtAddress2.Text = HelpArray[3][2].ToString();
        }

        private void txtRepCode_DoubleClick(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRepCode,SRepName from File_SalesPerson");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Sales Person", "Find", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            txtRepCode.Text = newFind.FindKeyValue;
        }

        private void dataGridViewInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        //    if (e.ColumnIndex == 6) {
        //        string item = dataGridViewInvoice.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
        //        string path = GlobalVariables.AssetPath + @"Images\Student_Images\" + item + ".jpg";
        //        newImg = new Image_View(path, dataGridViewInvoice.Rows[e.RowIndex].Cells[2].Value.ToString().Trim());
        //        newImg.ShowInTaskbar = false;
        //        newImg.ShowDialog();
        //    }
        }

        private void dataGridViewInvoice_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
