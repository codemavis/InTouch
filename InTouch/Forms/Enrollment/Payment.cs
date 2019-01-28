using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTouch.Classes.Connection;
using InTouch.Classes.GlobalVariables;
using InTouch.SubForms;
using System.Data;
using InTouch.Classes.MasterHelp;
using InTouch.Forms.Report;

namespace InTouch
{
    public partial class Payment : Form
    {
        private string cMode;
        DataSet PaymentDataSetH, PaymentDataSetD;
        GenerateReference newReference;

        public string CMode { get => cMode; set => cMode = value; }

        DataSet DS_UserButton ;

        public Payment()
        {
            InitializeComponent();
           
        }

        private void Payment_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }


        private void Payment_Load(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();

            DS_UserButton = dbCon.Get("select * from File_UserAccess where SFormName='" + this.label8.Text.Trim() + "' and SUserName='" + GlobalVariables.GetUserName().Trim() + "'");


            this.dataGridViewPayment.ColumnHeadersHeight = 30;
            this.dataGridViewPayment.Columns[0].Width = this.dataGridViewPayment.Width * 17 / 100;
            this.dataGridViewPayment.Columns[1].Width = this.dataGridViewPayment.Width * 15 / 100;
            this.dataGridViewPayment.Columns[2].Width = this.dataGridViewPayment.Width * 15 / 100;
            this.dataGridViewPayment.Columns[3].Width = this.dataGridViewPayment.Width * 15 / 100;
            this.dataGridViewPayment.Columns[4].Width = this.dataGridViewPayment.Width * 15 / 100;
            this.dataGridViewPayment.Columns[5].Width = this.dataGridViewPayment.Width * 15 / 100;

            PaymentDataSetH = dbCon.Get("select SRefNo,DDate,SStudentId,SPayMethod,NAmount,SRemarks,SAddUser,DAddDate,SAddMachine from File_PaymentH WHERE 1=0");
            PaymentDataSetD = dbCon.Get("select SRefNo,SInvNo,NAlloAmount from File_PaymentD WHERE 1=0");

            this.cMode = "INIT";
            InitProperties(cMode);
            DisableAllButtons();
            LoadData();
            ButtonControl();
            
        }

        private bool Validate_Entries()
        {
            if (txtStudentName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Student Name");
                return false;
            }
            if (txtPayMethod.Text.Trim().Equals(""))
            {
                MessageBox.Show("Invalid Payment Method");
                return false;
            }
            if (Convert.ToDouble(labelReceiptAmount.Text.Trim()) <= 0.00)
            {
                MessageBox.Show("Invalid Receipt Amount");
                return false;
            }
            return true;
        }
        private string Get_UniqueID()
        {
            DatabaseConnection newDbConn = new DatabaseConnection();
            newReference = new GenerateReference();
            return newReference.GetReference(newDbConn, "SPAY");
        }

        private bool Commit_Action()
        {
            string NewAutoGen = "";
            if (this.CMode.Equals("NEW"))
                NewAutoGen = Get_UniqueID();

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
                        Saved = newReference.UpdateNumVal(newDbConn, "SPAY");
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


                if (this.cMode.Equals("NEW"))
                {
                    MessageBox.Show(this.txtRefNo.Text);
                    Print_Data();
                }

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
            Fill_DataSet(PaymentDataSetH, PaymentDataSetD);
            if (newDbConn.Upd(PaymentDataSetH, "File_PaymentH"))
                if (newDbConn.Upd(PaymentDataSetD, "File_PaymentD"))
                    return Update_InvoiceBalance(newDbConn);
            return false;
        }
        private bool Update_InvoiceBalance(DatabaseConnection newDbConn)
        {
            if (dataGridViewPayment.RowCount > 0)
            {
                for (int i = 0; i < dataGridViewPayment.RowCount; i++)
                {
                    double nAmount = Convert.ToDouble(dataGridViewPayment.Rows[i].Cells["NAlloAmount"].Value.ToString().Trim());

                    if (nAmount > 0.00)
                    {
                        string cInvNo = dataGridViewPayment.Rows[i].Cells["SInvNo"].Value.ToString().Trim();
                        if (!newDbConn.ExecuteQuery("Update File_InvoiceH set NBalanceAmount=NBalanceAmount-" + nAmount + " Where SRefNo='" + cInvNo + "'"))
                            return false;
                    }
                }
            }
            return true;
        }

        private bool Remove_InvoiceBalance(DatabaseConnection newDbConn) {
            if (this.CMode.Equals("EDIT") || this.CMode.Equals("DELETE"))
            {
                if (dataGridViewPayment.RowCount > 0)
                {
                    for (int i = 0; i < dataGridViewPayment.RowCount; i++)
                    {
                        double nAmount = Convert.ToDouble(dataGridViewPayment.Rows[i].Cells["NAlloAmount"].Value.ToString().Trim());

                        if (nAmount > 0.00)
                        {
                            string cInvNo = dataGridViewPayment.Rows[i].Cells["SInvNo"].Value.ToString().Trim();
                            if (!newDbConn.ExecuteQuery("Update File_InvoiceH set NBalanceAmount=NBalanceAmount+" + nAmount + " Where SRefNo='" + cInvNo + "'"))
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private void Calculation()
        {
            try
            {
                if (this.cMode.Equals("NEW"))
                {
                    if (dataGridViewPayment.RowCount > 0)
                    {
                        double nBalAmount = 0, nTotalAmount = 0;

                        for (int i = 0; i < dataGridViewPayment.Rows.Count; i++)
                        {
                            if (dataGridViewPayment.Rows[i].Cells[4].Value == null || dataGridViewPayment.Rows[i].Cells[4].Value.ToString().Trim().Equals(""))
                                dataGridViewPayment.Rows[i].Cells[4].Value = 0.00;

                            dataGridViewPayment.Rows[i].Cells[5].Value = Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[3].Value) - Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[4].Value);

                            if (Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[5].Value) < 0.00)
                            {
                                MessageBox.Show("Allocation Amount Cannot Be Greater Than The Outstanding Amount");
                                dataGridViewPayment.Rows[i].Cells[4].Value = dataGridViewPayment.Rows[i].Cells[3].Value;
                                break;
                            }
                            nBalAmount += Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[5].Value);
                            nTotalAmount += Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[4].Value);
                        }
                        this.labelBalanceAmount.Text = nBalAmount + "";
                        this.labelReceiptAmount.Text = nTotalAmount + "";
                    }
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

            FillDataSetH.Tables[0].Rows.Add();
            FillDataSetH.Tables[0].Rows[0]["SRefNo"] = txtRefNo.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["DDate"] = dateTimePickerPay.Value.ToShortDateString();
            FillDataSetH.Tables[0].Rows[0]["SStudentId"] = txtStudent.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SPayMethod"] = txtPayMethod.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["NAmount"] = labelReceiptAmount.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SRemarks"] = txtRemarks.Text.Trim();
            FillDataSetH.Tables[0].Rows[0]["SAddUser"] = GlobalVariables.GetUserName();
            FillDataSetH.Tables[0].Rows[0]["DAddDate"] = DateTime.Now;
            FillDataSetH.Tables[0].Rows[0]["SAddMachine"] = Environment.MachineName;

            if (FillDataSetD.Tables[0].Rows.Count > 0)
                FillDataSetD.Tables[0].Rows.Clear();

            int j = 0;
            for (int i = 0; i < dataGridViewPayment.Rows.Count; i++)
            {
                if (Convert.ToDouble(dataGridViewPayment.Rows[i].Cells["NAlloAmount"].Value) > 0.00)
                {
                    FillDataSetD.Tables[0].Rows.Add();
                    FillDataSetD.Tables[0].Rows[j]["SRefNo"] = this.txtRefNo.Text.Trim();
                    FillDataSetD.Tables[0].Rows[j]["SInvNo"] = dataGridViewPayment.Rows[i].Cells["SInvNo"].FormattedValue.ToString().Trim();
                    FillDataSetD.Tables[0].Rows[j]["NAlloAmount"] = dataGridViewPayment.Rows[i].Cells["NAlloAmount"].FormattedValue.ToString().Trim();
                    j++;
                }
            }
        }
        private bool DeleteData(DatabaseConnection newDbConn)
        {
            if (newDbConn.ExecuteQuery("DELETE FROM File_PaymentH where SRefNo = '" + txtRefNo.Text + "'"))
                if (newDbConn.ExecuteQuery("DELETE FROM File_PaymentD where SRefNo = '" + txtRefNo.Text + "'"))
                    return Remove_InvoiceBalance(newDbConn);
            return false;
        }
        private bool SaveLogData(string logType, DatabaseConnection newDbConn)
        {
            return newDbConn.ExecuteQuery("INSERT INTO [dbo].[Log_Payment]([SRefNo],[DDate],[SStudentId],[SPayMethod],[NAmount],[SRemarks],[SAddUser] "
                + ",[DAddDate],[SAddMachine],[SDelUser],[DDelDate],[SDelMach],[SDelType]) "
                + "(Select SRefNo, DDate, SStudentId, SPayMethod, NAmount, SRemarks, SAddUser, DAddDate, SAddMachine, "
                + "'"+ GlobalVariables.GetUserName() + "', GETDATE(), '"+ Environment.MachineName + "', '"+logType+"' from File_PaymentH Where SRefNo = '" + txtRefNo.Text + "')");
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
                this.txtPayMethod.Text = "";
                this.txtAmount.Text = "";
                this.txtRemarks.Text = "";
               

                this.labelBalanceAmount.Text = "0.00";
                this.labelReceiptAmount.Text = "0.00";
                this.labelTotalDiscount.Text = "0.00";

                this.dataGridViewPayment.Rows.Clear();
            }
            //else if (this.cMode.Equals("PRINT")) {


            //    DatabaseConnection dbCon = new DatabaseConnection();
            //    DataSet Report_DataSet = dbCon.Get_ReportData("Select a.SRefNo,a.DDate,a.SStudentId,a.SPayMethod,a.NAmount,a.SRemarks,b.SInvNo,b.NAlloAmount "
            //        + " from File_PaymentH a, File_PaymentD b where a.SRefNo = b.SRefNo and SRefNo='" + txtRefNo.Text.Trim() + "'", "DT_Payment");

            //    Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Payment.rpt", Report_DataSet, "Payment");
            //    openReport.ShowInTaskbar = false;
            //    openReport.ShowDialog();
            //}
            else
            {
                if (!txtRefNo.Text.Trim().Equals(""))
                {
                    if (this.cMode != "NEW")
                    {

                        DatabaseConnection dbCon = new DatabaseConnection();
                        newDataSetH = dbCon.Get("select a.*,b.SName from File_PaymentH a,File_Student b where a.SStudentId=b.SStudentId "
                            + "and a.SRefNo='" + txtRefNo.Text + "'");

                        txtRefNo.Text = newDataSetH.Tables[0].Rows[0]["SRefNo"].ToString();
                        txtStudent.Text = newDataSetH.Tables[0].Rows[0]["SStudentId"].ToString();
                        txtStudentName.Text = newDataSetH.Tables[0].Rows[0]["SName"].ToString();
                        txtPayMethod.Text = newDataSetH.Tables[0].Rows[0]["SPayMethod"].ToString();
                        txtRemarks.Text = newDataSetH.Tables[0].Rows[0]["SRemarks"].ToString();

                        this.labelBalanceAmount.Text = "0.00";
                        labelReceiptAmount.Text = newDataSetH.Tables[0].Rows[0]["NAmount"].ToString();
                        this.labelTotalDiscount.Text = "0.00";

                        newDataSetD = dbCon.Get("Select b.SRefNo as SInvNo,b.DDate,b.NTotalAmount,a.NAlloAmount "
                            + "from File_PaymentD a,File_InvoiceH b where a.SInvNo=b.SRefNo and a.SRefNo='" + txtRefNo.Text + "'");
                        dataGridViewPayment.Rows.Clear();
                        for (int i = 0; i < newDataSetD.Tables[0].Rows.Count; i++)
                        {
                            dataGridViewPayment.Rows.Add();
                            dataGridViewPayment.Rows[i].Cells["SInvNo"].Value = newDataSetD.Tables[0].Rows[i]["SInvNo"].ToString();
                            dataGridViewPayment.Rows[i].Cells["DDate"].Value = newDataSetD.Tables[0].Rows[i]["DDate"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NTotalAmount"].Value = newDataSetD.Tables[0].Rows[i]["NTotalAmount"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NAlloAmount"].Value = newDataSetD.Tables[0].Rows[i]["NAlloAmount"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NBalanceAmount"].Value = "0.00";
                        }

                        //Print
                        //if (this.cMode.Equals("PRINT"))
                        //{
                        //    // DatabaseConnection dbCon = new DatabaseConnection();
                        //    DataSet Report_DataSet = dbCon.Get_ReportData("Select a.SRefNo,a.DDate,a.SStudentId,c.SName,a.SPayMethod,a.NAmount,"
                        //        + "a.SRemarks,b.NAlloAmount,b.SInvNo, b.NAlloAmount, d.DDate as DInvDate, e.SCourseCode,c.SAddress1,c.SAddress2,c.SAddress3, "
                        //        + "SCourseName, e.NAmount as NInvAmount from File_PaymentH a, File_PaymentD b, File_Student c, File_InvoiceH d, File_InvoiceD e, "
                        //        + "File_Course f where a.SRefNo = b.SRefNo and a.SStudentId = c.SStudentId and e.SRefNo = d.SRefNo and d.SRefNo = "
                        //        + "b.SInvNo and f.SCourseCode = e.SCourseCode and a.SRefNo='" + txtRefNo.Text.Trim() + "'", "DT_Payment");

                        //    Report_View openReport = new Report_View(GlobalVariables.ReportPath+"Rpt_Payment.rpt", Report_DataSet, "Payment");
                        //    openReport.ShowInTaskbar = false;
                        //    openReport.ShowDialog();

                        //    this.CMode = "INIT";
                        //    DisableAllButtons();
                        //    ButtonControl();
                        //    InitProperties(CMode);
                        //    LoadData();
                        //}
                    }
                }
                else
                {
                    if (!this.txtStudent.Text.Trim().Equals(""))
                    {
                        double NTotDis = 0;
                        DatabaseConnection dbCon = new DatabaseConnection();
                        newDataSetD = dbCon.Get("Select SRefNo as SInvNo,DDate,NTotalAmount,NBalanceAmount as NOutstanding,0.00 as NAlloAmount,"
                            + " 0.00 as NBalanceAmount,NTotalDiscount From File_InvoiceH Where NBalanceAmount>0.00 and DDate<='"
                            + dateTimePickerPay.Value.ToShortDateString().Trim() + "' and SStudentId = '" + txtStudent.Text + "' Order By DDate");
                        dataGridViewPayment.Rows.Clear();
                        for (int i = 0; i < newDataSetD.Tables[0].Rows.Count; i++)
                        {
                            dataGridViewPayment.Rows.Add();
                            dataGridViewPayment.Rows[i].Cells["SInvNo"].Value = newDataSetD.Tables[0].Rows[i]["SInvNo"].ToString();
                            dataGridViewPayment.Rows[i].Cells["DDate"].Value = newDataSetD.Tables[0].Rows[i]["DDate"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NTotalAmount"].Value = newDataSetD.Tables[0].Rows[i]["NTotalAmount"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NOutstanding"].Value = newDataSetD.Tables[0].Rows[i]["NOutstanding"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NAlloAmount"].Value = newDataSetD.Tables[0].Rows[i]["NAlloAmount"].ToString();
                            dataGridViewPayment.Rows[i].Cells["NBalanceAmount"].Value = newDataSetD.Tables[0].Rows[i]["NBalanceAmount"].ToString();
                            NTotDis += Convert.ToDouble(newDataSetD.Tables[0].Rows[i]["NTotalDiscount"].ToString().Trim());
                        }
                        this.labelTotalDiscount.Text = NTotDis + "";
                        this.Calculation();
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

        private void User_Button_Access()
        {
            if (this.DS_UserButton.Tables[0].Rows.Count > 0)
            {
                if (DS_UserButton.Tables[0].Rows[0]["chkNew"].ToString().Trim().Equals("0"))
                    this.btnNew.Visible = false;
                else
                    this.btnNew.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkEdit"].ToString().Trim().Equals("0"))
                    this.btnEdit.Visible = false;
                else
                    this.btnEdit.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkPrint"].ToString().Trim().Equals("0"))
                    this.btnPrint.Visible = false;
                else
                    this.btnPrint.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkDelete"].ToString().Trim().Equals("0"))
                    this.btnDelete.Visible = false;
                else
                    this.btnDelete.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkView"].ToString().Trim().Equals("0"))
                    this.btnFind.Visible = false;
                else
                    this.btnFind.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkSave"].ToString().Trim().Equals("0"))
                    this.btnSave.Visible = false;
                else
                    this.btnSave.Visible = true;

                if (DS_UserButton.Tables[0].Rows[0]["chkReset"].ToString().Trim().Equals("0"))
                    this.btnReset.Visible = false;
                else
                    this.btnReset.Visible = true;
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
            this.User_Button_Access();
        }

        private void InitProperties(string cMode)
        {
            if (cMode.Equals("INIT"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtPayMethod.Enabled = false;
                this.txtAmount.ReadOnly = true;
                this.txtRemarks.ReadOnly = true;

                this.dataGridViewPayment.ReadOnly = true;
            }
            if (cMode.Equals("DELETE"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = true;
                this.txtPayMethod.Enabled = false;
                this.txtAmount.ReadOnly = true;
                this.txtRemarks.ReadOnly = true;

                this.dataGridViewPayment.ReadOnly = true;
            }
            if (cMode.Equals("NEW"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = false;
                this.txtPayMethod.Enabled = true;
                this.txtAmount.ReadOnly = false;
                this.txtRemarks.ReadOnly = false;

                this.dataGridViewPayment.ReadOnly = false;
                this.dataGridViewPayment.Columns[0].ReadOnly = true;
                this.dataGridViewPayment.Columns[1].ReadOnly = true;
                this.dataGridViewPayment.Columns[2].ReadOnly = true;
                this.dataGridViewPayment.Columns[3].ReadOnly = true;
                this.dataGridViewPayment.Columns[4].ReadOnly = false;
                this.dataGridViewPayment.Columns[5].ReadOnly = true;
            }
            if (cMode.Equals("EDIT"))
            {
                //this.txtRefNo.ReadOnly = true;
                this.txtStudent.ReadOnly = false;
                this.txtPayMethod.Enabled = true;
                this.txtAmount.ReadOnly = false;
                this.txtRemarks.ReadOnly = false;

                this.dataGridViewPayment.Columns[0].ReadOnly = true;
                this.dataGridViewPayment.Columns[1].ReadOnly = true;
                this.dataGridViewPayment.Columns[2].ReadOnly = true;
                this.dataGridViewPayment.Columns[3].ReadOnly = true;
                this.dataGridViewPayment.Columns[4].ReadOnly = false;
                this.dataGridViewPayment.Columns[4].ReadOnly = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.cMode = "NEW";

            InitProperties(cMode);
            DisableAllButtons();
            ButtonControl();
            
            LoadData();

            this.txtStudentName.Focus();

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //this.cMode = "EDIT";
            //DisableAllButtons();
            //ButtonControl();

            //this.InitProperties(cMode);

            //DatabaseConnection dbCon = new DatabaseConnection();
            //DataSet FillData = dbCon.Get("Select SRefNo,DDate,SStudentId,SPayMethod,NAmount,SRemarks from File_PaymentH");
            //GlobalVariables.HelpDataSet = FillData;

            //Find newFind = new Find("Payment", "Find", this);
            //newFind.ShowInTaskbar = false;
            //newFind.ShowDialog();

            //txtRefNo.Text = newFind.FindKeyValue;

            MessageBox.Show("Edit Option Won't Work For Payment");
            txtRefNo.Text = "";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.cMode = "DELETE";
            DisableAllButtons();
            ButtonControl();
            
            InitProperties(cMode);
            LoadData();

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRefNo,DDate,SStudentId,SPayMethod,NAmount,SRemarks from File_PaymentH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Payment", "Find", this);
            newFind.ShowDialog();

            this.txtRefNo.Text = newFind.FindKeyValue;
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
            DataSet FillData = dbCon.Get("Select * from File_PaymentH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Invoice", "Find Payment", this);
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

            Print_Data();

            this.cMode = "INIT";
            DisableAllButtons();
            ButtonControl();
            
            InitProperties(cMode);
            LoadData();
        }

        private void Print_Data()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            string cQuery = "Select a.SRefNo,a.DDate,a.SStudentId,c.SName,a.SPayMethod,a.NAmount,"
                                + "a.SRemarks,b.NAlloAmount,b.SInvNo, b.NAlloAmount, d.DDate as DInvDate, e.SCourseCode,d.NBalanceAmount,c.SAddress1,c.SAddress2,c.SAddress3,a.SAddUser,a.DAddDate, "
                                + "SCourseName, e.NAmount as NInvAmount from File_PaymentH a, File_PaymentD b, File_Student c, File_InvoiceH d, File_InvoiceD e, "
                                + "File_Course f where a.SRefNo = b.SRefNo and a.SStudentId = c.SStudentId and e.SRefNo = d.SRefNo and d.SRefNo = "
                                + "b.SInvNo and f.SCourseCode = e.SCourseCode and a.SRefNo='" + txtRefNo.Text.Trim() + "'";

            DataSet Payment_ListingDataSet = dbCon.Get_ReportData(cQuery, "DT_Payment");

            Report_View newReport;

            newReport = new Report_View(GlobalVariables.ReportPath + "Rpt_Payment.rpt", Payment_ListingDataSet, "DT_Payment");

            newReport.ShowInTaskbar = false;
            newReport.ShowDialog();

        }
        

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.cMode = "VIEW";
            DisableAllButtons();
            ButtonControl();
            
            this.InitProperties(cMode);

            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = dbCon.Get("Select SRefNo,DDate,SStudentId,SPayMethod,NAmount,SRemarks from File_PaymentH Order By DDate Desc,SRefNo Desc");
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Payment", "Find Payment", this);
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
                        if (!this.cMode.Equals("NEW"))
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

        private void dataGridViewPayment_CellEnter(object sender, DataGridViewCellEventArgs e)
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
            HelpArray[1][0] = "";
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

            LoadData();
        }

        private void btnSetOff_Click(object sender, EventArgs e)
        {
            if (!this.txtAmount.Text.Trim().Equals(""))
            {
                double NTotalAmt = Convert.ToDouble(this.txtAmount.Text.Trim());
                double NReceiptAmt = 0.00;

                if (dataGridViewPayment.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridViewPayment.RowCount; i++)
                    {
                        if (Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[3].Value) < NTotalAmt)
                        {
                            dataGridViewPayment.Rows[i].Cells[4].Value = dataGridViewPayment.Rows[i].Cells[3].Value;
                            NTotalAmt -= Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[3].Value);
                            NReceiptAmt += Convert.ToDouble(dataGridViewPayment.Rows[i].Cells[3].Value);
                        }
                        else
                        {
                            dataGridViewPayment.Rows[i].Cells[4].Value = NTotalAmt;
                            NReceiptAmt += NTotalAmt;
                            break;
                        }
                    }
                    this.labelReceiptAmount.Text = NReceiptAmt + "";
                }
            }
        }

        private void dateTimePickerPay_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridViewPayment_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
                if (dataGridViewPayment.CurrentCell != null)
                {
                    this.Calculation();
                }
        }
    }
}
