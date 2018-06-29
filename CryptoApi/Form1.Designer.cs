namespace CryptoApi
{
    partial class Form1
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
            this.FindSrcbutton = new System.Windows.Forms.Button();
            this.SrcgroupBox = new System.Windows.Forms.GroupBox();
            this.TaskgroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ProtectionMethodcomboBox = new System.Windows.Forms.ComboBox();
            this.BlockSizecomboBox = new System.Windows.Forms.ComboBox();
            this.PasswordmaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.KeySizecomboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Processbutton = new System.Windows.Forms.Button();
            this.desttextBox = new System.Windows.Forms.TextBox();
            this.findrezbutton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.srctextBox = new System.Windows.Forms.TextBox();
            this.JobTypecomboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.srcopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dstsaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.recfolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SrcgroupBox.SuspendLayout();
            this.TaskgroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // FindSrcbutton
            // 
            this.FindSrcbutton.Location = new System.Drawing.Point(478, 13);
            this.FindSrcbutton.Name = "FindSrcbutton";
            this.FindSrcbutton.Size = new System.Drawing.Size(97, 23);
            this.FindSrcbutton.TabIndex = 0;
            this.FindSrcbutton.Text = "обзор...";
            this.FindSrcbutton.UseVisualStyleBackColor = true;
            this.FindSrcbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SrcgroupBox
            // 
            this.SrcgroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.SrcgroupBox.Controls.Add(this.TaskgroupBox);
            this.SrcgroupBox.Controls.Add(this.Processbutton);
            this.SrcgroupBox.Controls.Add(this.desttextBox);
            this.SrcgroupBox.Controls.Add(this.findrezbutton);
            this.SrcgroupBox.Controls.Add(this.label4);
            this.SrcgroupBox.Controls.Add(this.label2);
            this.SrcgroupBox.Controls.Add(this.srctextBox);
            this.SrcgroupBox.Controls.Add(this.JobTypecomboBox);
            this.SrcgroupBox.Controls.Add(this.FindSrcbutton);
            this.SrcgroupBox.Controls.Add(this.label1);
            this.SrcgroupBox.Location = new System.Drawing.Point(11, 12);
            this.SrcgroupBox.Name = "SrcgroupBox";
            this.SrcgroupBox.Size = new System.Drawing.Size(585, 179);
            this.SrcgroupBox.TabIndex = 1;
            this.SrcgroupBox.TabStop = false;
            this.SrcgroupBox.Text = "Шифрование/дешифрование";
            // 
            // TaskgroupBox
            // 
            this.TaskgroupBox.Controls.Add(this.label3);
            this.TaskgroupBox.Controls.Add(this.ProtectionMethodcomboBox);
            this.TaskgroupBox.Controls.Add(this.BlockSizecomboBox);
            this.TaskgroupBox.Controls.Add(this.PasswordmaskedTextBox);
            this.TaskgroupBox.Controls.Add(this.label5);
            this.TaskgroupBox.Controls.Add(this.label7);
            this.TaskgroupBox.Controls.Add(this.KeySizecomboBox);
            this.TaskgroupBox.Controls.Add(this.label6);
            this.TaskgroupBox.Location = new System.Drawing.Point(9, 99);
            this.TaskgroupBox.Name = "TaskgroupBox";
            this.TaskgroupBox.Size = new System.Drawing.Size(420, 71);
            this.TaskgroupBox.TabIndex = 16;
            this.TaskgroupBox.TabStop = false;
            this.TaskgroupBox.Text = "Опции";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Алгоритм:";
            // 
            // ProtectionMethodcomboBox
            // 
            this.ProtectionMethodcomboBox.FormattingEnabled = true;
            this.ProtectionMethodcomboBox.Items.AddRange(new object[] {
            "DES",
            "3DES",
            "RC2",
            "Rijndael"});
            this.ProtectionMethodcomboBox.Location = new System.Drawing.Point(74, 15);
            this.ProtectionMethodcomboBox.Name = "ProtectionMethodcomboBox";
            this.ProtectionMethodcomboBox.Size = new System.Drawing.Size(145, 21);
            this.ProtectionMethodcomboBox.TabIndex = 6;
            this.ProtectionMethodcomboBox.SelectedIndexChanged += new System.EventHandler(this.ProtectionMethodcomboBoxSelectedIndexChanged);
            // 
            // BlockSizecomboBox
            // 
            this.BlockSizecomboBox.FormattingEnabled = true;
            this.BlockSizecomboBox.Items.AddRange(new object[] {
            "Шифрование",
            "Дешифрование"});
            this.BlockSizecomboBox.Location = new System.Drawing.Point(339, 41);
            this.BlockSizecomboBox.Name = "BlockSizecomboBox";
            this.BlockSizecomboBox.Size = new System.Drawing.Size(70, 21);
            this.BlockSizecomboBox.TabIndex = 14;
            // 
            // PasswordmaskedTextBox
            // 
            this.PasswordmaskedTextBox.Location = new System.Drawing.Point(74, 41);
            this.PasswordmaskedTextBox.Name = "PasswordmaskedTextBox";
            this.PasswordmaskedTextBox.PasswordChar = '*';
            this.PasswordmaskedTextBox.Size = new System.Drawing.Size(145, 22);
            this.PasswordmaskedTextBox.TabIndex = 10;
            this.PasswordmaskedTextBox.Text = "11111111";
            this.PasswordmaskedTextBox.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.PasswordmaskedTextBox_MaskInputRejected);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Пароль:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(225, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Размер блока, бит:";
            // 
            // KeySizecomboBox
            // 
            this.KeySizecomboBox.FormattingEnabled = true;
            this.KeySizecomboBox.Items.AddRange(new object[] {
            "Шифрование",
            "Дешифрование"});
            this.KeySizecomboBox.Location = new System.Drawing.Point(339, 15);
            this.KeySizecomboBox.Name = "KeySizecomboBox";
            this.KeySizecomboBox.Size = new System.Drawing.Size(70, 21);
            this.KeySizecomboBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Размер ключа, бит:";
            // 
            // Processbutton
            // 
            this.Processbutton.Location = new System.Drawing.Point(435, 72);
            this.Processbutton.Name = "Processbutton";
            this.Processbutton.Size = new System.Drawing.Size(140, 35);
            this.Processbutton.TabIndex = 3;
            this.Processbutton.Text = "Обработать";
            this.Processbutton.UseVisualStyleBackColor = true;
            this.Processbutton.Click += new System.EventHandler(this.Processbutton_Click);
            // 
            // desttextBox
            // 
            this.desttextBox.Location = new System.Drawing.Point(105, 43);
            this.desttextBox.Name = "desttextBox";
            this.desttextBox.ReadOnly = true;
            this.desttextBox.Size = new System.Drawing.Size(367, 22);
            this.desttextBox.TabIndex = 4;
            // 
            // findrezbutton
            // 
            this.findrezbutton.Location = new System.Drawing.Point(478, 43);
            this.findrezbutton.Name = "findrezbutton";
            this.findrezbutton.Size = new System.Drawing.Size(97, 23);
            this.findrezbutton.TabIndex = 3;
            this.findrezbutton.Text = "обзор...";
            this.findrezbutton.UseVisualStyleBackColor = true;
            this.findrezbutton.Click += new System.EventHandler(this.findrezbutton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(102, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "тип операции:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "итоговый файл:";
            // 
            // srctextBox
            // 
            this.srctextBox.Location = new System.Drawing.Point(105, 15);
            this.srctextBox.Name = "srctextBox";
            this.srctextBox.ReadOnly = true;
            this.srctextBox.Size = new System.Drawing.Size(367, 22);
            this.srctextBox.TabIndex = 1;
            // 
            // JobTypecomboBox
            // 
            this.JobTypecomboBox.FormattingEnabled = true;
            this.JobTypecomboBox.Items.AddRange(new object[] {
            "Шифрование",
            "Дешифрование"});
            this.JobTypecomboBox.Location = new System.Drawing.Point(194, 72);
            this.JobTypecomboBox.Name = "JobTypecomboBox";
            this.JobTypecomboBox.Size = new System.Drawing.Size(160, 21);
            this.JobTypecomboBox.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "исходный файл:";
            // 
            // recfolderBrowserDialog
            // 
            this.recfolderBrowserDialog.HelpRequest += new System.EventHandler(this.recfolderBrowserDialog_HelpRequest);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(607, 202);
            this.Controls.Add(this.SrcgroupBox);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(615, 235);
            this.Name = "Form1";
            this.Text = "CryptoAPI";
            this.SrcgroupBox.ResumeLayout(false);
            this.SrcgroupBox.PerformLayout();
            this.TaskgroupBox.ResumeLayout(false);
            this.TaskgroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.GroupBox TaskgroupBox;
        private System.Windows.Forms.FolderBrowserDialog recfolderBrowserDialog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox KeySizecomboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox BlockSizecomboBox;

        #endregion

        private System.Windows.Forms.Button FindSrcbutton;
        private System.Windows.Forms.GroupBox SrcgroupBox;
        private System.Windows.Forms.TextBox srctextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox desttextBox;
        private System.Windows.Forms.Button findrezbutton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ProtectionMethodcomboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox JobTypecomboBox;
        private System.Windows.Forms.Button Processbutton;
        private System.Windows.Forms.MaskedTextBox PasswordmaskedTextBox;
        private System.Windows.Forms.OpenFileDialog srcopenFileDialog;
        private System.Windows.Forms.SaveFileDialog dstsaveFileDialog;
    }
}

