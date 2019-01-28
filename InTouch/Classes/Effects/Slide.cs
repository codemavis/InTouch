using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Classes.Effects
{
    class Slide
    {

        Timer NewTimer = new Timer();
        Panel SlidePanel;
        int panelWidth;
        bool Hidden;
        public void StartTimer(Panel slidePanel)
        {
            SlidePanel = slidePanel;
            panelWidth = slidePanel.Width;
            NewTimer.Start();
        }

        private void NewTimer_Tick(object sender, EventArgs e)
        {
            if (Hidden)
            {
                SlidePanel.Width = SlidePanel.Width + 15;
                // panelSlide.Width = panelSlide.Width - 1;
                if (SlidePanel.Width >= panelWidth)
                {
                    NewTimer.Stop();
                    Hidden = false;
                    //this.Refresh();

                }
            }
            else
            {
                SlidePanel.Width = SlidePanel.Width - 15;
                //  panelSlide.Width = panelSlide.Width + 1;
                if (SlidePanel.Width <= 0)
                {
                    NewTimer.Stop();
                    Hidden = true;
                  //  this.Refresh();
                }
            }
        }

    }
}
