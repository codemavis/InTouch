using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Forms.SubForms
{
    public partial class Image_View : Form
    {
        string imagePath = "",caption="Image";
        public Image_View(string imgPath,string formName)
        {
            InitializeComponent();
            imagePath = imgPath;
            caption = formName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Image_View_Load(object sender, EventArgs e)
        {
            this.labelCaption.Text = caption;
            if (File.Exists(imagePath))
                this.picItem.Image = new Bitmap(imagePath.ToString().Trim());
        }
    }
}
