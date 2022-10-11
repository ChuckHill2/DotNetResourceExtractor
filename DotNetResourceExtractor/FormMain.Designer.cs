
namespace DotNetResourceExtractor
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.m_btnExtract = new System.Windows.Forms.Button();
            this.m_btnExit = new System.Windows.Forms.Button();
            this.m_txtAssembly = new System.Windows.Forms.TextBox();
            this.m_lblAssembly = new System.Windows.Forms.Label();
            this.m_lblDestination = new System.Windows.Forms.Label();
            this.m_txtDestination = new System.Windows.Forms.TextBox();
            this.m_btnSelectAssembly = new System.Windows.Forms.Button();
            this.m_chkSeparateFolders = new System.Windows.Forms.CheckBox();
            this.m_btnSelectDestination = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.m_btnAbout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_btnExtract
            // 
            this.m_btnExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnExtract.Location = new System.Drawing.Point(286, 150);
            this.m_btnExtract.Name = "m_btnExtract";
            this.m_btnExtract.Size = new System.Drawing.Size(87, 27);
            this.m_btnExtract.TabIndex = 0;
            this.m_btnExtract.Text = "Extract";
            this.toolTip1.SetToolTip(this.m_btnExtract, "Begin the resource extraction.");
            this.m_btnExtract.UseVisualStyleBackColor = true;
            this.m_btnExtract.Click += new System.EventHandler(this.m_btnExtract_Click);
            // 
            // m_btnExit
            // 
            this.m_btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnExit.Location = new System.Drawing.Point(380, 150);
            this.m_btnExit.Name = "m_btnExit";
            this.m_btnExit.Size = new System.Drawing.Size(87, 27);
            this.m_btnExit.TabIndex = 1;
            this.m_btnExit.Text = "Exit";
            this.toolTip1.SetToolTip(this.m_btnExit, "Exit this application.");
            this.m_btnExit.UseVisualStyleBackColor = true;
            this.m_btnExit.Click += new System.EventHandler(this.m_btnExit_Click);
            // 
            // m_txtAssembly
            // 
            this.m_txtAssembly.AllowDrop = true;
            this.m_txtAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtAssembly.Location = new System.Drawing.Point(17, 29);
            this.m_txtAssembly.Name = "m_txtAssembly";
            this.m_txtAssembly.Size = new System.Drawing.Size(424, 21);
            this.m_txtAssembly.TabIndex = 2;
            this.toolTip1.SetToolTip(this.m_txtAssembly, resources.GetString("m_txtAssembly.ToolTip"));
            this.m_txtAssembly.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_txtAssembly_DragDrop);
            this.m_txtAssembly.DragEnter += new System.Windows.Forms.DragEventHandler(this.m_txtAssembly_DragEnter);
            this.m_txtAssembly.DoubleClick += new System.EventHandler(this.m_txtAssembly_DoubleClick);
            this.m_txtAssembly.Leave += new System.EventHandler(this.m_txtAssembly_Leave);
            // 
            // m_lblAssembly
            // 
            this.m_lblAssembly.AutoSize = true;
            this.m_lblAssembly.Location = new System.Drawing.Point(14, 10);
            this.m_lblAssembly.Name = "m_lblAssembly";
            this.m_lblAssembly.Size = new System.Drawing.Size(167, 15);
            this.m_lblAssembly.TabIndex = 3;
            this.m_lblAssembly.Text = "Assembly Filename to Extract";
            this.toolTip1.SetToolTip(this.m_lblAssembly, resources.GetString("m_lblAssembly.ToolTip"));
            // 
            // m_lblDestination
            // 
            this.m_lblDestination.AutoSize = true;
            this.m_lblDestination.Location = new System.Drawing.Point(14, 62);
            this.m_lblDestination.Name = "m_lblDestination";
            this.m_lblDestination.Size = new System.Drawing.Size(107, 15);
            this.m_lblDestination.TabIndex = 4;
            this.m_lblDestination.Text = "Destination Folder";
            this.toolTip1.SetToolTip(this.m_lblDestination, resources.GetString("m_lblDestination.ToolTip"));
            // 
            // m_txtDestination
            // 
            this.m_txtDestination.AllowDrop = true;
            this.m_txtDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtDestination.Location = new System.Drawing.Point(17, 81);
            this.m_txtDestination.Name = "m_txtDestination";
            this.m_txtDestination.Size = new System.Drawing.Size(424, 21);
            this.m_txtDestination.TabIndex = 5;
            this.toolTip1.SetToolTip(this.m_txtDestination, resources.GetString("m_txtDestination.ToolTip"));
            this.m_txtDestination.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_txtDestination_DragDrop);
            this.m_txtDestination.DragEnter += new System.Windows.Forms.DragEventHandler(this.m_txtDestination_DragEnter);
            this.m_txtDestination.DoubleClick += new System.EventHandler(this.m_txtDestination_DoubleClick);
            this.m_txtDestination.Leave += new System.EventHandler(this.m_txtDestination_Leave);
            // 
            // m_btnSelectAssembly
            // 
            this.m_btnSelectAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnSelectAssembly.BackgroundImage = global::DotNetResourceExtractor.Properties.Resources.SelectDll15;
            this.m_btnSelectAssembly.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_btnSelectAssembly.Location = new System.Drawing.Point(442, 28);
            this.m_btnSelectAssembly.Name = "m_btnSelectAssembly";
            this.m_btnSelectAssembly.Size = new System.Drawing.Size(23, 23);
            this.m_btnSelectAssembly.TabIndex = 6;
            this.toolTip1.SetToolTip(this.m_btnSelectAssembly, "Select  valid .NET assembly.\r\nOne can modify the filename with \r\nwildcards after " +
        "it has been selected.");
            this.m_btnSelectAssembly.UseVisualStyleBackColor = true;
            this.m_btnSelectAssembly.Click += new System.EventHandler(this.m_btnSelectAssembly_Click);
            // 
            // m_chkSeparateFolders
            // 
            this.m_chkSeparateFolders.AutoSize = true;
            this.m_chkSeparateFolders.Location = new System.Drawing.Point(17, 119);
            this.m_chkSeparateFolders.Name = "m_chkSeparateFolders";
            this.m_chkSeparateFolders.Size = new System.Drawing.Size(242, 19);
            this.m_chkSeparateFolders.TabIndex = 8;
            this.m_chkSeparateFolders.Text = "Separate sub-folders for each assembly";
            this.toolTip1.SetToolTip(this.m_chkSeparateFolders, "Create a separate sub-folder for the resources extracted from each assembly.\r\nIf " +
        "not, each extracted resource file will be prefixed with the assembly name.");
            this.m_chkSeparateFolders.UseVisualStyleBackColor = true;
            // 
            // m_btnSelectDestination
            // 
            this.m_btnSelectDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnSelectDestination.BackgroundImage = global::DotNetResourceExtractor.Properties.Resources.SelectFolder15;
            this.m_btnSelectDestination.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_btnSelectDestination.Location = new System.Drawing.Point(442, 80);
            this.m_btnSelectDestination.Name = "m_btnSelectDestination";
            this.m_btnSelectDestination.Size = new System.Drawing.Size(23, 23);
            this.m_btnSelectDestination.TabIndex = 7;
            this.toolTip1.SetToolTip(this.m_btnSelectDestination, "Select  a destination folder.");
            this.m_btnSelectDestination.UseVisualStyleBackColor = true;
            this.m_btnSelectDestination.Click += new System.EventHandler(this.m_btnSelectDestination_Click);
            // 
            // m_btnAbout
            // 
            this.m_btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_btnAbout.Location = new System.Drawing.Point(17, 150);
            this.m_btnAbout.Name = "m_btnAbout";
            this.m_btnAbout.Size = new System.Drawing.Size(87, 27);
            this.m_btnAbout.TabIndex = 9;
            this.m_btnAbout.Text = "About";
            this.toolTip1.SetToolTip(this.m_btnAbout, "About this application");
            this.m_btnAbout.UseVisualStyleBackColor = true;
            this.m_btnAbout.Click += new System.EventHandler(this.m_btnAbout_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 190);
            this.Controls.Add(this.m_btnAbout);
            this.Controls.Add(this.m_chkSeparateFolders);
            this.Controls.Add(this.m_btnSelectDestination);
            this.Controls.Add(this.m_btnSelectAssembly);
            this.Controls.Add(this.m_txtDestination);
            this.Controls.Add(this.m_lblDestination);
            this.Controls.Add(this.m_lblAssembly);
            this.Controls.Add(this.m_txtAssembly);
            this.Controls.Add(this.m_btnExit);
            this.Controls.Add(this.m_btnExtract);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::DotNetResourceExtractor.Properties.Resources.favicon;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(9999, 229);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(228, 229);
            this.Name = "FormMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = ".Net Resource Extractor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnExtract;
        private System.Windows.Forms.Button m_btnExit;
        private System.Windows.Forms.TextBox m_txtAssembly;
        private System.Windows.Forms.Label m_lblAssembly;
        private System.Windows.Forms.Label m_lblDestination;
        private System.Windows.Forms.TextBox m_txtDestination;
        private System.Windows.Forms.Button m_btnSelectAssembly;
        private System.Windows.Forms.Button m_btnSelectDestination;
        private System.Windows.Forms.CheckBox m_chkSeparateFolders;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button m_btnAbout;
    }
}

