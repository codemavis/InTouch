using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Forms.Report
{
    public partial class Report_View : Form
    {
        ReportDocument crystal = new ReportDocument();
        DataSet Report_ViewDataSet;
        string Report_Path;
        string heading;
        public Report_View(string ReportPath, DataSet Report_DataSet, string cHeader)
        {
            InitializeComponent();
            this.labelHead.Text = cHeader;
            Report_ViewDataSet = Report_DataSet;
            Report_Path = ReportPath;
            heading = cHeader;
        }

        private void Report_View_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void Report_View_Load(object sender, EventArgs e)
        {
            crystal.Load(Report_Path);
            crystal.SetDataSource(Report_ViewDataSet);
            crystalReportViewer1.ReportSource = crystal;
            crystalReportViewer1.RefreshReport();
        }

        public bool Report_Export(string FileName) {
            crystal.Load(Report_Path);
            crystal.SetDataSource(Report_ViewDataSet);
            crystalReportViewer1.ReportSource = crystal;
            //crystalReportViewer1.RefreshReport();

            crystal.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, FileName);
            return true;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (Width != Screen.PrimaryScreen.WorkingArea.Width)
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width;
                Height = Screen.PrimaryScreen.WorkingArea.Height;
            }
            else
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Left = Top = 0;
                Width = Screen.PrimaryScreen.WorkingArea.Width / 3;
                Height = Screen.PrimaryScreen.WorkingArea.Height / 3;
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
