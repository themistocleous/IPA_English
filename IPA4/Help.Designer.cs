namespace IPA4
{
    partial class Help
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.rtxTextHelp = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtxTextHelp
            // 
            this.rtxTextHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxTextHelp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.rtxTextHelp.Location = new System.Drawing.Point(0, 0);
            this.rtxTextHelp.Name = "rtxTextHelp";
            this.rtxTextHelp.ReadOnly = true;
            this.rtxTextHelp.Size = new System.Drawing.Size(320, 538);
            this.rtxTextHelp.TabIndex = 0;
            this.rtxTextHelp.Text = resources.GetString("rtxTextHelp.Text");
            this.rtxTextHelp.TextChanged += new System.EventHandler(this.rtxTextHelp_TextChanged);
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 538);
            this.Controls.Add(this.rtxTextHelp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Help";
            this.Text = "Help";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Help_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxTextHelp;
    }
}