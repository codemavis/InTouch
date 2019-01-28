namespace InTouch.Forms.SubForms
{
    partial class FindRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindRange));
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelHead = new System.Windows.Forms.Label();
            this.panelTool = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridViewLeft = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridViewRight = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.btnAllToRight = new System.Windows.Forms.ToolStripButton();
            this.btnToRight = new System.Windows.Forms.ToolStripButton();
            this.btnToLeft = new System.Windows.Forms.ToolStripButton();
            this.btnAllToLeft = new System.Windows.Forms.ToolStripButton();
            this.dragControl1 = new InTouch.Controls.DragControl();
            this.panelTop.SuspendLayout();
            this.panelTool.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panelTop.Controls.Add(this.labelHead);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(863, 33);
            this.panelTop.TabIndex = 4;
            // 
            // labelHead
            // 
            this.labelHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHead.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelHead.Location = new System.Drawing.Point(23, 5);
            this.labelHead.Name = "labelHead";
            this.labelHead.Size = new System.Drawing.Size(136, 19);
            this.labelHead.TabIndex = 10;
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.toolStrip1);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTool.Location = new System.Drawing.Point(0, 33);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(863, 60);
            this.panelTool.TabIndex = 7;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelect,
            this.btnFind,
            this.btnCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(863, 60);
            this.toolStrip1.TabIndex = 45;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnFind
            // 
            this.btnFind.AutoSize = false;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.Color.Black;
            this.btnFind.Image = global::InTouch.Properties.Resources.view;
            this.btnFind.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(109, 55);
            this.btnFind.Text = "&Find";
            this.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSize = false;
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Image = global::InTouch.Properties.Resources.check_select;
            this.btnSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(109, 55);
            this.btnSelect.Text = "&Select";
            this.btnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = false;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = global::InTouch.Properties.Resources.delete;
            this.btnCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 55);
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(30, 357);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(833, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(30, 357);
            this.panel2.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridViewLeft);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(30, 93);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(375, 357);
            this.panel3.TabIndex = 9;
            // 
            // dataGridViewLeft
            // 
            this.dataGridViewLeft.AllowUserToAddRows = false;
            this.dataGridViewLeft.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewLeft.ColumnHeadersHeight = 30;
            this.dataGridViewLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLeft.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLeft.Name = "dataGridViewLeft";
            this.dataGridViewLeft.ReadOnly = true;
            this.dataGridViewLeft.Size = new System.Drawing.Size(375, 357);
            this.dataGridViewLeft.TabIndex = 0;
            this.dataGridViewLeft.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLeft_CellDoubleClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridViewRight);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(458, 93);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(375, 357);
            this.panel4.TabIndex = 10;
            // 
            // dataGridViewRight
            // 
            this.dataGridViewRight.AllowUserToAddRows = false;
            this.dataGridViewRight.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRight.ColumnHeadersHeight = 30;
            this.dataGridViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRight.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRight.Name = "dataGridViewRight";
            this.dataGridViewRight.ReadOnly = true;
            this.dataGridViewRight.Size = new System.Drawing.Size(375, 357);
            this.dataGridViewRight.TabIndex = 1;
            this.dataGridViewRight.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRight_CellDoubleClick);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(405, 93);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(53, 357);
            this.panel5.TabIndex = 10;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.toolStrip4);
            this.panel6.Location = new System.Drawing.Point(0, 83);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(53, 262);
            this.panel6.TabIndex = 0;
            // 
            // toolStrip4
            // 
            this.toolStrip4.AutoSize = false;
            this.toolStrip4.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAllToRight,
            this.btnToRight,
            this.btnToLeft,
            this.btnAllToLeft});
            this.toolStrip4.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip4.Location = new System.Drawing.Point(0, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(53, 262);
            this.toolStrip4.TabIndex = 48;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // btnAllToRight
            // 
            this.btnAllToRight.AutoSize = false;
            this.btnAllToRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAllToRight.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllToRight.ForeColor = System.Drawing.Color.Black;
            this.btnAllToRight.Image = ((System.Drawing.Image)(resources.GetObject("btnAllToRight.Image")));
            this.btnAllToRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAllToRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAllToRight.Name = "btnAllToRight";
            this.btnAllToRight.Size = new System.Drawing.Size(50, 54);
            this.btnAllToRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAllToRight.Click += new System.EventHandler(this.btnAllToRight_Click);
            // 
            // btnToRight
            // 
            this.btnToRight.AutoSize = false;
            this.btnToRight.BackColor = System.Drawing.Color.Transparent;
            this.btnToRight.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToRight.ForeColor = System.Drawing.Color.Black;
            this.btnToRight.Image = ((System.Drawing.Image)(resources.GetObject("btnToRight.Image")));
            this.btnToRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnToRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToRight.Name = "btnToRight";
            this.btnToRight.Size = new System.Drawing.Size(50, 54);
            this.btnToRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnToRight.Click += new System.EventHandler(this.btnToRight_Click);
            // 
            // btnToLeft
            // 
            this.btnToLeft.AutoSize = false;
            this.btnToLeft.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToLeft.ForeColor = System.Drawing.Color.Black;
            this.btnToLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnToLeft.Image")));
            this.btnToLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnToLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToLeft.Name = "btnToLeft";
            this.btnToLeft.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.btnToLeft.Size = new System.Drawing.Size(50, 54);
            this.btnToLeft.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.btnToLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnToLeft.Click += new System.EventHandler(this.btnToLeft_Click);
            // 
            // btnAllToLeft
            // 
            this.btnAllToLeft.AutoSize = false;
            this.btnAllToLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAllToLeft.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllToLeft.ForeColor = System.Drawing.Color.Black;
            this.btnAllToLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnAllToLeft.Image")));
            this.btnAllToLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAllToLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAllToLeft.Name = "btnAllToLeft";
            this.btnAllToLeft.Size = new System.Drawing.Size(50, 54);
            this.btnAllToLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAllToLeft.Click += new System.EventHandler(this.btnAllToLeft_Click);
            // 
            // dragControl1
            // 
            this.dragControl1.SelectControl = this.panelTop;
            // 
            // FindRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(863, 450);
            this.ControlBox = false;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTool);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FindRange";
            this.Text = "FindRange";
            this.Load += new System.EventHandler(this.FindRange_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTool.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private Controls.DragControl dragControl1;
        private System.Windows.Forms.Label labelHead;
        private System.Windows.Forms.Panel panelTool;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton btnToLeft;
        private System.Windows.Forms.ToolStripButton btnAllToRight;
        private System.Windows.Forms.ToolStripButton btnToRight;
        private System.Windows.Forms.ToolStripButton btnAllToLeft;
        private System.Windows.Forms.DataGridView dataGridViewLeft;
        private System.Windows.Forms.DataGridView dataGridViewRight;
    }
}