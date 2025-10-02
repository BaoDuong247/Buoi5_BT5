using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Buoi5_BT5
{
    public partial class Form1 : Form
    {
        private string currentFilePath = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void FontDialog_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;
            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richTextBox1.ForeColor = fontDlg.Color;
                richTextBox1.Font = fontDlg.Font;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbFont.Text = "Tahoma";
            cmbSize.Text = "14";
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                cmbFont.Items.Add(font.Name);
            }
            List<int> listSize = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (var s in listSize)
            {
                cmbSize.Items.Add(s);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            currentFilePath = null; 
            cmbFont.Text = "Tahoma";
            cmbSize.Text = "14";
            richTextBox1.Font = new Font("Tahoma", 14);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "Rich Text Format (*.rtf)|*.rtf|Tệp Văn bản (*.txt)|*.txt";
                saveDlg.DefaultExt = "rtf";

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                   
                    richTextBox1.SaveFile(saveDlg.FileName, RichTextBoxStreamType.RichText);
                    currentFilePath = saveDlg.FileName;
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else 
            {
                richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Tệp RTF (*.rtf)|*.rtf|Tệp Văn bản (*.txt)|*.txt|Tất cả tệp (*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (Path.GetExtension(openDlg.FileName).ToLower() == ".rtf")
                    {
                        richTextBox1.LoadFile(openDlg.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        richTextBox1.LoadFile(openDlg.FileName, RichTextBoxStreamType.PlainText);
                    }
                    currentFilePath = openDlg.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở tệp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ToggleFontStyle(FontStyle style)
        {
            Font currentFont = richTextBox1.SelectionFont ?? richTextBox1.Font;
            FontStyle newStyle;
            if (currentFont.Style.HasFlag(style))
            {
                newStyle = currentFont.Style & ~style;
            }
            else
            {
                newStyle = currentFont.Style | style; 
            }
            Font newFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.SelectionFont = newFont;
            }
            else
            {
                richTextBox1.SelectionFont = newFont;
            }
        }
        private void btnB_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void btnU_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
        }

        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFontName = cmbFont.SelectedItem?.ToString() ?? cmbFont.Text;

            if (!string.IsNullOrEmpty(selectedFontName))
            {
                Font currentFont = richTextBox1.SelectionFont ?? richTextBox1.Font;
                float currentSize = currentFont.Size;
                FontStyle currentStyle = currentFont.Style;
                try
                {
                    richTextBox1.SelectionFont = new Font(selectedFontName, currentSize, currentStyle);
                }
                catch (Exception)
                {
                 
                }
            }
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (float.TryParse(cmbSize.SelectedItem?.ToString() ?? cmbSize.Text, out float newSize))
            {
                Font currentFont = richTextBox1.SelectionFont ?? richTextBox1.Font;
                FontStyle currentStyle = currentFont.Style;
                string currentFontName = currentFont.FontFamily.Name;
                richTextBox1.SelectionFont = new Font(currentFontName, newSize, currentStyle);
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            btnNew_Click(sender, e);
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int wordCount = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            toolStripStatusLabel1.Text = $"Tổng số từ: {wordCount}";
        }
    }
}
