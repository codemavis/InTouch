using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Controls
{
    class DragControl : Component
    {
        private Control HandleControl;

        public Control SelectControl
        {
            get
            {
                return this.HandleControl;
            }
            set
            {
                this.HandleControl = value;
                this.HandleControl.MouseDown += new MouseEventHandler(this.DragForm_MouseDown);
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr a, int msg, int wParam, int lOaram);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();


        private void DragForm_MouseDown(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                DragControl.ReleaseCapture();
                DragControl.SendMessage(this.SelectControl.FindForm().Handle, 161, 2, 0);
            }
        }

        private void InitializeComponent()
        {

        }
    }
}
