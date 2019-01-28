using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Classes.FormControl
{
    class OpenForm
    {
        public void openChildForm(Form newForm, Form openForm, string formName)
        {

            Form openChildForm = null;
            FormCollection fc = Application.OpenForms;
            foreach (Form formOpen in fc)
            {
                if (formOpen.Name == formName)
                {
                    openChildForm = formOpen;
                }
            }
            if (openChildForm != null)
            {
                openChildForm.BringToFront();
                openChildForm.Show();
            }
            else
            {
                newForm.MdiParent = openForm;
                if (!formName.Equals("Notification"))
                    newForm.Dock = DockStyle.Fill;
                newForm.Show();
            }
        }
    }
}