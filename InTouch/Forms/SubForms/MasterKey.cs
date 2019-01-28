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

namespace InTouch.SubForms
{
    public partial class MasterKey : Form
    {
        public string KeyValue;
        public bool IsClosed=false;
        public MasterKey(string cHead, string keyText, Form parentForm)
        {
            InitializeComponent();
            this.labelHead.Text = cHead;
            this.labelKey.Text = keyText;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            KeyValue = this.txtKey.Text.Trim();

            if (KeyValue.Equals(""))
                MessageBox.Show("Invalid Key Value");
            else if (!Validate_Duplicate())
                MessageBox.Show(KeyValue+" Exists, Cannot Duplicate!");
            else
                this.Close();
        }

        private bool Validate_Duplicate()
        {
            DatabaseConnection dbCon = new DatabaseConnection();
            DataSet newDataSet = dbCon.Get("Select * from File_" + this.labelHead.Text.Trim() + " where S" + this.labelKey.Text.Trim() + "='" + txtKey.Text.Trim() + "'");
            if (newDataSet.Tables[0].Rows.Count > 0)
                return false;
            else
                return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.IsClosed = true;
            this.Close();
        }
    }
}
