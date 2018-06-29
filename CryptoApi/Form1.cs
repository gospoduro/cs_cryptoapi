using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace CryptoApi
{
    public partial class Form1 : Form
    {
        // Encrypting
        CryptoProviderInfo cpi = null;
              
        public Form1()
        {
            InitializeComponent();
            
            ProtectionMethodcomboBox.SelectedIndex = 0;
            JobTypecomboBox.SelectedIndex = 0;
            KeySizecomboBox.SelectedIndex = 0;
            BlockSizecomboBox.SelectedIndex = 0;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (srcopenFileDialog.ShowDialog() == DialogResult.OK)
            {
                srctextBox.Text = srcopenFileDialog.FileName;
            }
        }

        private void findrezbutton_Click(object sender, EventArgs e)
        {
            if (dstsaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                desttextBox.Text = dstsaveFileDialog.FileName;
            }
        }

        private void Processbutton_Click(object sender, EventArgs e)
        {
            if (cpi == null) return;
            
            // interface checking
            if (srctextBox.Text.Length == 0)
            {
                MessageBox.Show("Введите имя исходного файла!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (KeySizecomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Укажите размер ключа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (BlockSizecomboBox.Enabled)
                if (BlockSizecomboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Укажите размер блока!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            
            if (desttextBox.Text.Length == 0)
            {
                MessageBox.Show("Введите имя конечного файла!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ProtectionMethodcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите метод шифрования из списка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (JobTypecomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип операции из списка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (PasswordmaskedTextBox.Text.Length < 7)
            {
                MessageBox.Show("Введите пароль еще раз", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string tempFile = Application.ExecutablePath + ".temp";
               
            try
            {
                if (JobTypecomboBox.SelectedIndex == 0) 
                    {SymEncryptor sy = new SymEncryptor(cpi.SymAlgorithm,srctextBox.Text, desttextBox.Text, Convert.ToInt32(BlockSizecomboBox.SelectedItem), Convert.ToInt32(KeySizecomboBox.SelectedItem), CipherMode.CBC,PasswordmaskedTextBox.Text);}
                    else {SymDecryptor sd = new SymDecryptor(cpi.SymAlgorithm,srctextBox.Text, desttextBox.Text, Convert.ToInt32(BlockSizecomboBox.SelectedItem), Convert.ToInt32(KeySizecomboBox.SelectedItem), CipherMode.CBC,PasswordmaskedTextBox.Text);};
            }
            catch (Exception exc)
            {
                MessageBox.Show("При обработке произошли ошибки: " + exc.Message);
            }
            
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        
        void ProtectionMethodcomboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProtectionMethodcomboBox.SelectedIndex != -1)
            {
                cpi = new CryptoProviderInfo((string)ProtectionMethodcomboBox.SelectedItem);
                // KEY SIZES
                KeySizecomboBox.Items.Clear();
                for (int i = 0; i < cpi.KeySizes.Length; i++)
                    KeySizecomboBox.Items.Add(cpi.KeySizes[i].ToString());
                
                // BLOCK SIZES
                BlockSizecomboBox.Items.Clear();
                for (int i = 0; i < cpi.DataSizes.Length; i++)
                    BlockSizecomboBox.Items.Add(cpi.DataSizes[i].ToString());
            }
        }
        
        void ChangeRecParametersbuttonClick(object sender, EventArgs e)
        {
        }
 
        private void recfolderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void PasswordmaskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }        
    }
}
