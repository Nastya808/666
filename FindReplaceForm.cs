using System;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class FindReplaceForm : Form
    {
        private RichTextBox richTextBox;

        public FindReplaceForm(RichTextBox richTextBox)
        {
            InitializeComponent();
            this.richTextBox = richTextBox;
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            FindText(txtFind.Text, chkMatchCase.Checked, chkWholeWord.Checked, true);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionLength > 0)
            {
                richTextBox.SelectedText = txtReplace.Text;
            }

            FindText(txtFind.Text, chkMatchCase.Checked, chkWholeWord.Checked, false);
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            richTextBox.Text = richTextBox.Text.Replace(txtFind.Text, txtReplace.Text);
        }

        private void FindText(string searchText, bool matchCase, bool wholeWord, bool findNext)
        {
            int start = richTextBox.SelectionStart;
            int length = richTextBox.SelectionLength;

            StringComparison comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            int index;
            if (wholeWord)
            {
                index = richTextBox.Text.IndexOfWholeWord(searchText, start, comparison);
            }
            else
            {
                index = richTextBox.Text.IndexOf(searchText, start, comparison);
            }

            if (index != -1)
            {
                richTextBox.Select(index, searchText.Length);
                richTextBox.ScrollToCaret();
            }
            else
            {
                if (findNext)
                {
                    MessageBox.Show("Текст не найден", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
