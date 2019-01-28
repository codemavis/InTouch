namespace InTouch
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnSlide = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.labelUserName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.btnUser = new System.Windows.Forms.ToolStripButton();
            this.btnProfile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.btnCourse = new System.Windows.Forms.ToolStripButton();
            this.btnSalesRep = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.btnStudent = new System.Windows.Forms.ToolStripButton();
            this.btnInquiry = new System.Windows.Forms.ToolStripButton();
            this.btnInvoice = new System.Windows.Forms.ToolStripButton();
            this.btnPayment = new System.Windows.Forms.ToolStripButton();
            this.btnLectureActivity = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.btnInvoiceListing = new System.Windows.Forms.ToolStripButton();
            this.btnPaymentListing = new System.Windows.Forms.ToolStripButton();
            this.btnOutstanding = new System.Windows.Forms.ToolStripButton();
            this.btnSalesReport = new System.Windows.Forms.ToolStripButton();
            this.btnStudentPayment = new System.Windows.Forms.ToolStripButton();
            this.btnItemWiseCustomer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBackup = new System.Windows.Forms.ToolStripButton();
            this.btnLogOff = new System.Windows.Forms.ToolStripButton();
            this.dragControl1 = new InTouch.Controls.DragControl();
            this.panelHeader.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panelHeader.Controls.Add(this.btnMaximize);
            this.panelHeader.Controls.Add(this.btnMinimize);
            this.panelHeader.Controls.Add(this.btnSlide);
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1196, 52);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.DoubleClick += new System.EventHandler(this.panelHeader_DoubleClick);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximize.ForeColor = System.Drawing.Color.White;
            this.btnMaximize.Image = global::InTouch.Properties.Resources.maximize;
            this.btnMaximize.Location = new System.Drawing.Point(1092, 4);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(49, 46);
            this.btnMaximize.TabIndex = 11;
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Image = global::InTouch.Properties.Resources.minimize;
            this.btnMinimize.Location = new System.Drawing.Point(1041, 4);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(49, 46);
            this.btnMinimize.TabIndex = 10;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnSlide
            // 
            this.btnSlide.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnSlide.FlatAppearance.BorderSize = 0;
            this.btnSlide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSlide.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSlide.ForeColor = System.Drawing.Color.White;
            this.btnSlide.Image = global::InTouch.Properties.Resources.bars;
            this.btnSlide.Location = new System.Drawing.Point(268, 4);
            this.btnSlide.Margin = new System.Windows.Forms.Padding(4);
            this.btnSlide.Name = "btnSlide";
            this.btnSlide.Size = new System.Drawing.Size(49, 46);
            this.btnSlide.TabIndex = 9;
            this.btnSlide.UseVisualStyleBackColor = false;
            this.btnSlide.Click += new System.EventHandler(this.btnSlide_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = global::InTouch.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(1143, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(49, 46);
            this.btnClose.TabIndex = 8;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "InTouch Solutions";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Gray;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 945);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1196, 25);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 20);
            // 
            // StatusLabel
            // 
            this.StatusLabel.ForeColor = System.Drawing.Color.White;
            this.StatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(54, 20);
            this.StatusLabel.Text = " Ready";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackgroundImage = global::InTouch.Properties.Resources.left_back;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.labelUserName,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.btnUser,
            this.btnProfile,
            this.toolStripSeparator3,
            this.toolStripLabel3,
            this.btnCourse,
            this.btnSalesRep,
            this.toolStripSeparator4,
            this.toolStripLabel6,
            this.btnStudent,
            this.btnInquiry,
            this.btnInvoice,
            this.btnPayment,
            this.btnLectureActivity,
            this.toolStripSeparator5,
            this.toolStripLabel5,
            this.btnInvoiceListing,
            this.btnPaymentListing,
            this.btnOutstanding,
            this.btnSalesReport,
            this.btnStudentPayment,
            this.btnItemWiseCustomer,
            this.toolStripSeparator6,
            this.btnBackup,
            this.btnLogOff});
            this.toolStrip1.Location = new System.Drawing.Point(0, 52);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(317, 893);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(315, 6);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = false;
            this.labelUserName.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserName.Image = ((System.Drawing.Image)(resources.GetObject("labelUserName.Image")));
            this.labelUserName.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.labelUserName.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(211, 40);
            this.labelUserName.Text = "         Shane";
            this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox1.Margin = new System.Windows.Forms.Padding(3, 0, 1, 0);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(287, 27);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel2.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(217, 22);
            this.toolStripLabel2.Text = "     Admin";
            this.toolStripLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUser
            // 
            this.btnUser.AutoSize = false;
            this.btnUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUser.Image = ((System.Drawing.Image)(resources.GetObject("btnUser.Image")));
            this.btnUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUser.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(217, 25);
            this.btnUser.Text = "     User";
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnProfile
            // 
            this.btnProfile.AutoSize = false;
            this.btnProfile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnProfile.Image = ((System.Drawing.Image)(resources.GetObject("btnProfile.Image")));
            this.btnProfile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfile.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(217, 25);
            this.btnProfile.Text = "     Profile";
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.AutoSize = false;
            this.toolStripLabel3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel3.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(217, 22);
            this.toolStripLabel3.Text = "     Master Data";
            this.toolStripLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCourse
            // 
            this.btnCourse.AutoSize = false;
            this.btnCourse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCourse.Image = ((System.Drawing.Image)(resources.GetObject("btnCourse.Image")));
            this.btnCourse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCourse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCourse.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnCourse.Name = "btnCourse";
            this.btnCourse.Size = new System.Drawing.Size(217, 25);
            this.btnCourse.Text = "      Course";
            this.btnCourse.Click += new System.EventHandler(this.btnCourse_Click);
            // 
            // btnSalesRep
            // 
            this.btnSalesRep.AutoSize = false;
            this.btnSalesRep.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSalesRep.Image = ((System.Drawing.Image)(resources.GetObject("btnSalesRep.Image")));
            this.btnSalesRep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesRep.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSalesRep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSalesRep.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnSalesRep.Name = "btnSalesRep";
            this.btnSalesRep.Size = new System.Drawing.Size(217, 25);
            this.btnSalesRep.Text = "     Sales Person";
            this.btnSalesRep.Click += new System.EventHandler(this.btnSalesRep_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AutoSize = false;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.AutoSize = false;
            this.toolStripLabel6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel6.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(217, 22);
            this.toolStripLabel6.Text = "     Enrollment";
            this.toolStripLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStudent
            // 
            this.btnStudent.AutoSize = false;
            this.btnStudent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnStudent.Image = ((System.Drawing.Image)(resources.GetObject("btnStudent.Image")));
            this.btnStudent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStudent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStudent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStudent.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnStudent.Name = "btnStudent";
            this.btnStudent.Size = new System.Drawing.Size(217, 25);
            this.btnStudent.Text = "     Student";
            this.btnStudent.Click += new System.EventHandler(this.btnStudent_Click);
            // 
            // btnInquiry
            // 
            this.btnInquiry.AutoSize = false;
            this.btnInquiry.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnInquiry.Image = ((System.Drawing.Image)(resources.GetObject("btnInquiry.Image")));
            this.btnInquiry.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInquiry.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnInquiry.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInquiry.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnInquiry.Name = "btnInquiry";
            this.btnInquiry.Size = new System.Drawing.Size(217, 25);
            this.btnInquiry.Text = "     Inquiry";
            this.btnInquiry.Click += new System.EventHandler(this.btnInquiry_Click);
            // 
            // btnInvoice
            // 
            this.btnInvoice.AutoSize = false;
            this.btnInvoice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnInvoice.Image = ((System.Drawing.Image)(resources.GetObject("btnInvoice.Image")));
            this.btnInvoice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInvoice.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInvoice.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(217, 25);
            this.btnInvoice.Text = "     Invoice";
            this.btnInvoice.Click += new System.EventHandler(this.btnInvoice_Click);
            // 
            // btnPayment
            // 
            this.btnPayment.AutoSize = false;
            this.btnPayment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPayment.Image = ((System.Drawing.Image)(resources.GetObject("btnPayment.Image")));
            this.btnPayment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPayment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPayment.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(217, 25);
            this.btnPayment.Text = "     Payment";
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnLectureActivity
            // 
            this.btnLectureActivity.AutoSize = false;
            this.btnLectureActivity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLectureActivity.Image = ((System.Drawing.Image)(resources.GetObject("btnLectureActivity.Image")));
            this.btnLectureActivity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLectureActivity.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLectureActivity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLectureActivity.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnLectureActivity.Name = "btnLectureActivity";
            this.btnLectureActivity.Size = new System.Drawing.Size(217, 25);
            this.btnLectureActivity.Text = "     Lecture Activity";
            this.btnLectureActivity.Click += new System.EventHandler(this.btnLectureActivity_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AutoSize = false;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.AutoSize = false;
            this.toolStripLabel5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel5.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(217, 22);
            this.toolStripLabel5.Text = "     Reports";
            this.toolStripLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInvoiceListing
            // 
            this.btnInvoiceListing.AutoSize = false;
            this.btnInvoiceListing.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnInvoiceListing.Image = ((System.Drawing.Image)(resources.GetObject("btnInvoiceListing.Image")));
            this.btnInvoiceListing.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInvoiceListing.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnInvoiceListing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInvoiceListing.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnInvoiceListing.Name = "btnInvoiceListing";
            this.btnInvoiceListing.Size = new System.Drawing.Size(217, 25);
            this.btnInvoiceListing.Text = "     Invoice Listing";
            this.btnInvoiceListing.Click += new System.EventHandler(this.btnInvoiceListing_Click);
            // 
            // btnPaymentListing
            // 
            this.btnPaymentListing.AutoSize = false;
            this.btnPaymentListing.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPaymentListing.Image = ((System.Drawing.Image)(resources.GetObject("btnPaymentListing.Image")));
            this.btnPaymentListing.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaymentListing.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPaymentListing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaymentListing.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnPaymentListing.Name = "btnPaymentListing";
            this.btnPaymentListing.Size = new System.Drawing.Size(217, 25);
            this.btnPaymentListing.Text = "     Payment Listing";
            this.btnPaymentListing.Click += new System.EventHandler(this.btnPaymentListing_Click);
            // 
            // btnOutstanding
            // 
            this.btnOutstanding.AutoSize = false;
            this.btnOutstanding.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnOutstanding.Image = ((System.Drawing.Image)(resources.GetObject("btnOutstanding.Image")));
            this.btnOutstanding.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutstanding.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOutstanding.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutstanding.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnOutstanding.Name = "btnOutstanding";
            this.btnOutstanding.Size = new System.Drawing.Size(217, 25);
            this.btnOutstanding.Text = "     Outstanding";
            this.btnOutstanding.Click += new System.EventHandler(this.btnOutstanding_Click);
            // 
            // btnSalesReport
            // 
            this.btnSalesReport.AutoSize = false;
            this.btnSalesReport.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSalesReport.Image = ((System.Drawing.Image)(resources.GetObject("btnSalesReport.Image")));
            this.btnSalesReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSalesReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSalesReport.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.Size = new System.Drawing.Size(217, 25);
            this.btnSalesReport.Text = "     Sales Report";
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click);
            // 
            // btnStudentPayment
            // 
            this.btnStudentPayment.AutoSize = false;
            this.btnStudentPayment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnStudentPayment.Image = ((System.Drawing.Image)(resources.GetObject("btnStudentPayment.Image")));
            this.btnStudentPayment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStudentPayment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStudentPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStudentPayment.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnStudentPayment.Name = "btnStudentPayment";
            this.btnStudentPayment.Size = new System.Drawing.Size(217, 25);
            this.btnStudentPayment.Text = "      Student Payment Listing ";
            this.btnStudentPayment.Click += new System.EventHandler(this.btnStudentPayment_Click);
            // 
            // btnItemWiseCustomer
            // 
            this.btnItemWiseCustomer.AutoSize = false;
            this.btnItemWiseCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnItemWiseCustomer.Image = ((System.Drawing.Image)(resources.GetObject("btnItemWiseCustomer.Image")));
            this.btnItemWiseCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnItemWiseCustomer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnItemWiseCustomer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnItemWiseCustomer.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnItemWiseCustomer.Name = "btnItemWiseCustomer";
            this.btnItemWiseCustomer.Size = new System.Drawing.Size(217, 25);
            this.btnItemWiseCustomer.Text = "     Course wise Student";
            this.btnItemWiseCustomer.Click += new System.EventHandler(this.btnItemWiseCustomer_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(315, 6);
            // 
            // btnBackup
            // 
            this.btnBackup.AutoSize = false;
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackup.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnBackup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBackup.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(217, 25);
            this.btnBackup.Text = "Backup";
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnLogOff
            // 
            this.btnLogOff.AutoSize = false;
            this.btnLogOff.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOff.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLogOff.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOff.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLogOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogOff.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.btnLogOff.Name = "btnLogOff";
            this.btnLogOff.Size = new System.Drawing.Size(217, 25);
            this.btnLogOff.Text = "Log Off";
            this.btnLogOff.Click += new System.EventHandler(this.btnLogOff_Click);
            // 
            // dragControl1
            // 
            this.dragControl1.SelectControl = this.panelHeader;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::InTouch.Properties.Resources.HomePage1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1196, 970);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "InTouch";
            this.Load += new System.EventHandler(this.Main_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private Controls.DragControl dragControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnSlide;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelUserName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btnProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton btnCourse;
        private System.Windows.Forms.ToolStripButton btnSalesRep;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnStudent;
        private System.Windows.Forms.ToolStripButton btnInvoice;
        private System.Windows.Forms.ToolStripButton btnPayment;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripButton btnOutstanding;
        private System.Windows.Forms.ToolStripButton btnPaymentListing;
        private System.Windows.Forms.ToolStripButton btnSalesReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton btnInvoiceListing;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripButton btnUser;
        private System.Windows.Forms.ToolStripButton btnInquiry;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton btnItemWiseCustomer;
        private System.Windows.Forms.ToolStripButton btnStudentPayment;
        private System.Windows.Forms.ToolStripButton btnLogOff;
        private System.Windows.Forms.ToolStripButton btnBackup;
        private System.Windows.Forms.ToolStripButton btnLectureActivity;
    }
}