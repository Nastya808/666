using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TextEditor
{
    public partial class MainForm : Form
    {
        private string currentFilePath;

        public MainForm()
        {
            InitializeComponent();
            InitializeToolbar();
            currentFilePath = null;
        }

        private void InitializeToolbar()
        {
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Items.Add(new ToolStripButton("Открыть", null, OpenFile));
            toolStrip.Items.Add(new ToolStripButton("Сохранить", null, SaveFile));
            toolStrip.Items.Add(new ToolStripButton("Новый документ", null, NewDocument));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Копировать", null, CopyText));
            toolStrip.Items.Add(new ToolStripButton("Вырезать", null, CutText));
            toolStrip.Items.Add(new ToolStripButton("Вставить", null, PasteText));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Отменить", null, UndoAction));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Настройки редактора", null, ShowSettings));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Поиск и замена", null, ShowFindReplaceDialog));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Увеличить шрифт", null, IncreaseFontSize));
            toolStrip.Items.Add(new ToolStripButton("Уменьшить шрифт", null, DecreaseFontSize));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("Информация о тексте", null, ShowTextInfo));

            Controls.Add(toolStrip);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = openFileDialog.FileName;
                    Text = $"Текстовый редактор - {currentFilePath}";
                    richTextBox.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void SaveFile(object sender, EventArgs e)
        {
            if (currentFilePath != null)
            {
                richTextBox.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
            }
            else
            {
                SaveFileAs(sender, e);
            }
        }

        private void SaveFileAs(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    Text = $"Текстовый редактор - {currentFilePath}";
                    richTextBox.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void NewDocument(object sender, EventArgs e)
        {
            richTextBox.Clear();
            currentFilePath = null;
            Text = "Текстовый редактор - Новый документ";
        }

        private void CopyText(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void CutText(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void PasteText(object sender, EventArgs e)
        {
            richTextBox.Paste();
        }

        private void UndoAction(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm(richTextBox))
            {
                settingsForm.ShowDialog();
            }
        }

        private void ShowFindReplaceDialog(object sender, EventArgs e)
        {
            using (FindReplaceForm findReplaceForm = new FindReplaceForm(richTextBox))
            {
                findReplaceForm.ShowDialog();
            }
        }

        private void IncreaseFontSize(object sender, EventArgs e)
        {
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont.FontFamily, richTextBox.SelectionFont.Size + 1, richTextBox.SelectionFont.Style);
        }

        private void DecreaseFontSize(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont.Size > 1)
            {
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont.FontFamily, richTextBox.SelectionFont.Size - 1, richTextBox.SelectionFont.Style);
            }
        }

        private void ShowTextInfo(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText))
            {
                MessageBox.Show($"Длина выделенного текста: {richTextBox.SelectedText.Length} символов", "Информация о тексте", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет выделенного текста", "Информация о тексте", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (richTextBox.Modified)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveFile(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
