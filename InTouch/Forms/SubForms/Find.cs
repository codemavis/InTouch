using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTouch.Classes.GlobalVariables;

namespace InTouch.SubForms
{
    public partial class Find : Form
    {
        public string FindKeyValue;
        DataTable DT_Grid;
        public Find(string cHead, string keyText, Form parentFor)
        {
            InitializeComponent();
            this.labelHead.Text = cHead;
            this.labelKey.Text = keyText;
            StartPosition = FormStartPosition.CenterParent;

        }
        private void Find_Load(object sender, EventArgs e)
        {
            this.dataGridViewHelp.AutoGenerateColumns = true;
            this.dataGridViewHelp.AllowUserToAddRows = false;
            DT_Grid = GlobalVariables.HelpDataSet.Tables[0];
            

            if (DT_Grid.Rows.Count > 0)
            {
                this.dataGridViewHelp.DataSource = DT_Grid;

                for (int i = 0; i < dataGridViewHelp.ColumnCount; i++)
                {
                    this.dataGridViewHelp.Columns[i].Width = this.dataGridViewHelp.Rows[0].Cells[i].ToString().Length * 4;
                }
            }
            else
            {
                MessageBox.Show("No Data Found");
                this.Close();
            }

            this.txtKey.Focus();
        }

        private void labelKey_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewHelp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridViewHelp.CurrentRow.Index;
            FindKeyValue = dataGridViewHelp.Rows[index].Cells[0].Value.ToString();

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridViewHelp.DataSource;

            string cFilter = "";

            for (int i = 0; i < dataGridViewHelp.Columns.Count; i++)
            {
                cFilter += "CONVERT(" + dataGridViewHelp.Columns[i].DataPropertyName + ",System.String) like '%" + txtKey.Text.Trim().Replace("'", "''") + "%' OR ";
            }

            cFilter = cFilter.Remove(cFilter.Length - 3);

            bs.Filter = string.Format(cFilter);
        }

        private void dataGridViewHelp_KeyDown(object sender, KeyEventArgs e)
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
