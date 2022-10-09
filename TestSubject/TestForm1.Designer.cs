
namespace TestSubject
{
    partial class TestForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm1));
            this.m_bnTest = new System.Windows.Forms.Button();
            this.m_pbTest = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbTest)).BeginInit();
            this.SuspendLayout();
            // 
            // m_bnTest
            // 
            this.m_bnTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_bnTest.BackgroundImage")));
            this.m_bnTest.Location = new System.Drawing.Point(12, 12);
            this.m_bnTest.Name = "m_bnTest";
            this.m_bnTest.Size = new System.Drawing.Size(75, 54);
            this.m_bnTest.TabIndex = 0;
            this.m_bnTest.Text = "Test Button";
            this.m_bnTest.UseVisualStyleBackColor = true;
            // 
            // m_pbTest
            // 
            this.m_pbTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_pbTest.BackgroundImage")));
            this.m_pbTest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_pbTest.Image = ((System.Drawing.Image)(resources.GetObject("m_pbTest.Image")));
            this.m_pbTest.Location = new System.Drawing.Point(134, 16);
            this.m_pbTest.Name = "m_pbTest";
            this.m_pbTest.Size = new System.Drawing.Size(292, 304);
            this.m_pbTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.m_pbTest.TabIndex = 1;
            this.m_pbTest.TabStop = false;
            // 
            // TestForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 450);
            this.Controls.Add(this.m_pbTest);
            this.Controls.Add(this.m_bnTest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm1";
            this.Text = "TestForm1";
            ((System.ComponentModel.ISupportInitialize)(this.m_pbTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_bnTest;
        private System.Windows.Forms.PictureBox m_pbTest;
    }
}