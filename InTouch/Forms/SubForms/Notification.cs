using InTouch.Classes.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Forms.SubForms
{
    public partial class Notification : Form
    {
        public bool Hided { get; private set; }
        int PW = 0;
        string cstr = "";
        DataSet newDataSet;
        public Notification(DataSet DS_Note)
        {
            InitializeComponent();
            PW = 150;
            Hided = true;
            newDataSet = DS_Note;
        }

        
        private bool Load_Note(string str) {
           // DatabaseConnection dbCon = new DatabaseConnection();
            //string str = " Select c.SName,d.SCourseName,iif(max(f.SRefNo) is Null, '', max(f.SRefNo)) as Payment_RefNo, "
            //    + " iif(max(f.DDate) is null,'',max(f.ddate)) as Payment_Date,NBalanceAmount as Balance_Amount, "
            //    + " (datediff(DAY,max(f.DDate),getdate())-((count(e.SRefNo))*NPayInDays)) as Overdue_Days from "
            //    + " File_InvoiceH a inner join File_InvoiceD b on a.SRefNo=b.SRefNo inner join File_Student c "
            //    + " on c.SStudentId=a.SStudentId inner join File_Course d on d.SCourseCode=b.SCourseCode "
            //    + " left join File_PaymentD e on e.SInvNo=a.SRefNo left join File_PaymentH f on f.SRefNo=e.SRefNo "
            //    + " where a.NBalanceAmount>0.00 group by a.SRefNo,a.DDate,a.SStudentId,c.SName,NBalanceAmount,NPayInDays, "
            //    + " d.SCourseName,SCourseName having (datediff(DAY,max(f.DDate),getdate())) > ((count(e.SRefNo))*NPayInDays) ";
          //  DataSet newDataSet = dbCon.Get(str);

            this.dataGridViewCredit.ColumnHeadersHeight = 30;

            if (newDataSet.Tables[0].Rows.Count > 0)
            {
                this.dataGridViewCredit.AutoGenerateColumns = true;
                this.dataGridViewCredit.AllowUserToAddRows = false;
                this.dataGridViewCredit.DataSource = newDataSet.Tables[0];
                this.dataGridViewCredit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Notification_Load(object sender, EventArgs e)
        {
            Load_Note(this.cstr);
            timer1.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Hided)
            {
                this.Height = this.Height + 150;
                if (this.Height >= PW)
                {
                    timer1.Stop(); Hided = false;
                    this.Refresh();
                }
            }
            else
            {
                this.Height = this.Height - 150;
                if (this.Height <= 0)
                {
                    timer1.Stop();
                    Hided = true;
                    this.Refresh();
                }
            }
        }

        private void Notification_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult dRes = MessageBox.Show("Do You Want To Close?", "Confirm", MessageBoxButtons.YesNo);
                if (dRes == DialogResult.Yes)
                    this.Close();
            }
        }
    }
}
