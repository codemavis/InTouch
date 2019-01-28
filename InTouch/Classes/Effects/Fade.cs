using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Classes.Effects
{
    class Fade
    {
        Timer t1 = new Timer();
        Timer t2 = new Timer();
        Form loadForm, cxForm;
        public void formLoad(Form loadfrm, Form xFrm)
        {
            loadForm = loadfrm;
            cxForm = xFrm;
            t1.Tick += new EventHandler(fadeIn);
            t1.Interval = 10;
            t1.Start();
        }


        void fadeIn(object sender, EventArgs e)
        {
            if (loadForm.Opacity >= 1)
            {
                t1.Stop();
                if (cxForm != null)
                    cxForm.Visible = false;
            }
            else
                loadForm.Opacity += 0.05;
        }
    }
}
