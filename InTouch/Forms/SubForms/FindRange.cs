using InTouch.Classes.Connection;
using InTouch.Classes.GlobalVariables;
using InTouch.SubForms;
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
    public partial class FindRange : Form
    {
        DataSet nRawDataSet, nFillDataSet;
        public FindRange(string cHead,DataSet RawDataSet,DataSet FillDataSet)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            labelHead.Text = cHead;
            nRawDataSet = RawDataSet;
            nFillDataSet = FillDataSet;
        }

        private void FindRange_Load(object sender, EventArgs e)
        {
            
            GlobalVariables.HelpDataSetLeft = nFillDataSet.Clone();
            GlobalVariables.HelpDataSetLeft.Merge(nFillDataSet);

            this.dataGridViewLeft.AutoGenerateColumns = true;
            this.dataGridViewLeft.AllowUserToAddRows = false;

            GlobalVariables.HelpDataSetRight = GlobalVariables.HelpDataSetLeft.Clone();
            GlobalVariables.HelpDataSetRight.Merge(nRawDataSet);

            GlobalVariables.HelpDataSetTemp = GlobalVariables.HelpDataSetLeft.Clone();
            
            this.dataGridViewLeft.DataSource = GlobalVariables.HelpDataSetLeft.Tables[0];
            this.dataGridViewLeft.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.dataGridViewRight.DataSource = GlobalVariables.HelpDataSetRight.Tables[0];
            this.dataGridViewRight.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.nRawDataSet.Clear();
            this.nFillDataSet.Clear();
            nFillDataSet.Merge(GlobalVariables.HelpDataSetMain);
            this.Close();
        }

        private void btnAllToLeft_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewRight.RowCount > 0)
            {
                GlobalVariables.HelpDataSetLeft = GlobalVariables.HelpDataSetMain.Copy();
                GlobalVariables.HelpDataSetRight.Clear();
                this.dataGridViewLeft.DataSource = GlobalVariables.HelpDataSetLeft.Tables[0];
            }
        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            Left_To_Right();
        }

        private void Left_To_Right() {
            if (this.dataGridViewLeft.CurrentCell != null)
            {
                GlobalVariables.HelpDataSetRight.Tables[0].ImportRow(GlobalVariables.HelpDataSetLeft.Tables[0].Rows[dataGridViewLeft.CurrentCell.RowIndex]);

                GlobalVariables.HelpDataSetLeft.Tables[0].Rows[dataGridViewLeft.CurrentCell.RowIndex].Delete();

                GlobalVariables.HelpDataSetTemp.Tables[0].Clear();

                for (int i = 0; i < dataGridViewLeft.Rows.Count; i++)
                {
                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows.Add();

                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows[i][GlobalVariables.HelpDataSetTemp.Tables[0].Columns[0].ColumnName] = dataGridViewLeft.Rows[i].Cells[0].Value.ToString();
                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows[i][GlobalVariables.HelpDataSetTemp.Tables[0].Columns[1].ColumnName] = dataGridViewLeft.Rows[i].Cells[1].Value.ToString();
                }
                GlobalVariables.HelpDataSetLeft.Tables[0].Clear();
                GlobalVariables.HelpDataSetLeft = GlobalVariables.HelpDataSetTemp.Copy();
                GlobalVariables.HelpDataSetTemp.Tables[0].Clear();

                this.dataGridViewLeft.DataSource = GlobalVariables.HelpDataSetLeft.Tables[0];
            }
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {
            Right_To_Left();
        }

        private void Right_To_Left() {
            if (this.dataGridViewRight.CurrentCell != null)
            {
                GlobalVariables.HelpDataSetLeft.Tables[0].ImportRow(GlobalVariables.HelpDataSetRight.Tables[0].Rows[dataGridViewRight.CurrentCell.RowIndex]);

                GlobalVariables.HelpDataSetRight.Tables[0].Rows[dataGridViewRight.CurrentCell.RowIndex].Delete();

                GlobalVariables.HelpDataSetTemp = GlobalVariables.HelpDataSetRight.Clone();
                GlobalVariables.HelpDataSetTemp.Tables[0].Clear();

                for (int i = 0; i < dataGridViewRight.Rows.Count; i++)
                {
                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows.Add();

                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows[i][GlobalVariables.HelpDataSetTemp.Tables[0].Columns[0].ColumnName] = dataGridViewRight.Rows[i].Cells[0].Value.ToString();
                    GlobalVariables.HelpDataSetTemp.Tables[0].Rows[i][GlobalVariables.HelpDataSetTemp.Tables[0].Columns[1].ColumnName] = dataGridViewRight.Rows[i].Cells[1].Value.ToString();
                }
                GlobalVariables.HelpDataSetRight.Tables[0].Clear();
                GlobalVariables.HelpDataSetRight = GlobalVariables.HelpDataSetTemp.Copy();
                GlobalVariables.HelpDataSetTemp.Tables[0].Clear();
                this.dataGridViewRight.DataSource = GlobalVariables.HelpDataSetRight.Tables[0];
            }
        }

        private void btnAllToRight_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewLeft.RowCount > 0) {

                GlobalVariables.HelpDataSetRight = GlobalVariables.HelpDataSetMain.Copy();
                
                GlobalVariables.HelpDataSetLeft.Clear();

                this.dataGridViewRight.DataSource = GlobalVariables.HelpDataSetRight.Tables[0];
            }
        }

        private void dataGridViewLeft_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Left_To_Right();
        }

        private void dataGridViewRight_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Right_To_Left();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet FillData = GlobalVariables.HelpDataSetLeft.Copy();
            GlobalVariables.HelpDataSet = FillData;

            Find newFind = new Find("Filter", "Find Filter", this);
            newFind.ShowInTaskbar = false;
            newFind.ShowDialog();

            if (newFind.FindKeyValue != null)
            {
                int i = 0;
                string findValue = newFind.FindKeyValue.ToString().Trim();

                for (i=0; i < dataGridViewLeft.Rows.Count; i++) {
                    if (dataGridViewLeft.Rows[i].Cells[0].Value.ToString().Trim() == findValue)
                        break;
                }

                dataGridViewLeft.CurrentCell = dataGridViewLeft.Rows[i].Cells[0];
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            nRawDataSet.Clear();
            nFillDataSet.Clear();
            this.nRawDataSet.Merge(GlobalVariables.HelpDataSetRight);
            this.nFillDataSet.Merge(GlobalVariables.HelpDataSetLeft);

            GlobalVariables.HelpDataSetLeft.Clear();
            GlobalVariables.HelpDataSetRight.Clear();
            GlobalVariables.HelpDataSetMain.Clear();

            this.Close();
        }
    }
}
