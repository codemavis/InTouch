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
using InTouch.Classes.Effects;
using InTouch.Classes.Connection;

namespace InTouch
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            //string password = Cryptography.Decrypt(this.txtPassword.Text.Trim());
            string password = this.txtPassword.Text.Trim();

            DataSet newDataSet=null;

            DatabaseConnection dbCon = new DatabaseConnection();
            try
            {
                newDataSet = dbCon.Get("Select SUserName from File_User Where SUserName='" + userName + "' and SPassword='" + password + "'");

                if (newDataSet.Tables[0].Rows.Count > 0)
                {
                    GlobalVariables.SetUserName(userName);

                    MessageBox.Show("Welcome " + newDataSet.Tables[0].Rows[0][0].ToString().Trim());

                    Main newMain = new Main(this);
                    newMain.Show();
                }
                else
                {
                    MessageBox.Show("Invalid password!");
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            

            //GlobalVariables newLogin = new GlobalVariables();

            //DataSet newList = dbCon.GetData("select userid,password from fPass where userid='"+ userName + "' and password='"+ password + "'");
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Opacity = 0;

            Fade fade = new Fade();
            fade.formLoad(this, null);

            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
        }
    }
}
