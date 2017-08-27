namespace IPA4
{
    partial class FindDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDialog));
            this.chkRegularExpression = new System.Windows.Forms.CheckBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtFind = new System.Windows.Forms.RichTextBox();
            this.txtReplace = new System.Windows.Forms.RichTextBox();
            this.chkontop = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkRegularExpression
            // 
            this.chkRegularExpression.Location = new System.Drawing.Point(2, 123);
            this.chkRegularExpression.Name = "chkRegularExpression";
            this.chkRegularExpression.Size = new System.Drawing.Size(179, 32);
            this.chkRegularExpression.TabIndex = 17;
            this.chkRegularExpression.Text = "Χρήση τυπικών εκφράσεων";
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(301, 54);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(96, 45);
            this.btnReplace.TabIndex = 16;
            this.btnReplace.Text = "Αντικατάσταση Επομένου";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click_1);
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.Location = new System.Drawing.Point(2, 103);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(212, 24);
            this.chkCaseSensitive.TabIndex = 14;
            this.chkCaseSensitive.Text = "Ταίριασμα πεζών-κεφαλαίων";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(301, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Ακύρωση";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(301, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 45);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "Εύρεση Επομένου";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(2, 3);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(293, 45);
            this.txtFind.TabIndex = 19;
            this.txtFind.Text = "";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(2, 54);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(294, 45);
            this.txtReplace.TabIndex = 20;
            this.txtReplace.Text = "";
            // 
            // chkontop
            // 
            this.chkontop.AutoSize = true;
            this.chkontop.Checked = true;
            this.chkontop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkontop.Location = new System.Drawing.Point(2, 152);
            this.chkontop.Name = "chkontop";
            this.chkontop.Size = new System.Drawing.Size(153, 17);
            this.chkontop.TabIndex = 21;
            this.chkontop.Text = "Πάντοτε στην επιφάνεια";
            this.chkontop.UseVisualStyleBackColor = true;
            this.chkontop.CheckedChanged += new System.EventHandler(this.chkontop_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(300, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Βοήθεια";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 172);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkontop);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.chkRegularExpression);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.chkCaseSensitive);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FindDialog";
            this.Text = "Εύρεση και Αντικατάσταση";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkRegularExpression;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RichTextBox _txtControl;
        private System.Windows.Forms.RichTextBox txtFind;
        private System.Windows.Forms.RichTextBox txtReplace;
        private System.Windows.Forms.CheckBox chkontop;
        private System.Windows.Forms.Button button1;

    }
}