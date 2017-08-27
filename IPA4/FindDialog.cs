using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IPA4
{
    public partial class FindDialog : Form
    {
        public FindDialog(RichTextBox txtControl, bool replace)
        {
            InitializeComponent();
            this.TopMost = true;
            txtFind.Focus();
            _txtControl = txtControl;
            Replace = replace;
            txtFind.Text = _txtControl.Text.Substring(_txtControl.SelectionStart, _txtControl.SelectionLength);

        }
        private string _text = "";
        static private bool _caseSensitive = false;
        static private bool _regularExpressions = false;
        private bool _replace = false;
        public string FindText
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }
        public bool Replace
        {
            set
            {
                _replace = value;
                txtReplace.Enabled = value;
                btnReplace.Visible = value;
            }
        }
        public bool CaseSensitive
        {
            get
            {
                return _caseSensitive;
            }

            set
            {
                _caseSensitive = value;
            }
        }
        static private int _currentIndex = 0;
        static public string CurrentSearchString = "";
        static public bool FindNext(RichTextBox txtControl)
        {
            return FindNext(CurrentSearchString, _caseSensitive, txtControl, _regularExpressions);
        }
        static public int CurrentIndex
        {
            set
            {
                _currentIndex = value;
            }
        }
        static public bool FindNext(string searchString, bool caseSensitive, RichTextBox txtControl, bool useRegularExpression)
        {
            // track the current search string
            CurrentSearchString = searchString;
            Regex regularExpression = null;

            // get the length of the search string
            int searchLength = searchString.Length;

            // handle case sensitive strings
            if (caseSensitive)
            {
                if (useRegularExpression)
                {
                    // we are using regular expressions, create a RegularExpression object
                    try
                    {
                        regularExpression = new Regex(searchString);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ανύπαρκτη Τυπική Έκφραση "+ex.ToString());
                        return false;
                        
                    }
                    // Now match the regular expression
                    Match match = regularExpression.Match(txtControl.Text, _currentIndex);

                    // if we successfully matched, get the index location of the match inside
                    // the textbox control and the length of the match
                    if (match.Success)
                    {
                        _currentIndex = match.Index;
                        searchLength = match.Length;
                    }
                    else
                    {
                        // no match
                        _currentIndex = -1;
                    }

                }
                else
                {
                    // not a regular expression search, just match the literal string
                    _currentIndex = txtControl.Text.IndexOf(searchString, _currentIndex);
                }
            }
            else
            {
                // this section is for case-insensitive searches
                if (useRegularExpression)
                {
                    try
                    {
                        // set the ignore case option for regular expressions
                        regularExpression = new Regex(searchString, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ανύπαρκτη τυπική έκφραση "+ex.ToString());
                        return false;
                    }

                    // Now match the regular expression
                    Match match = regularExpression.Match(txtControl.Text, _currentIndex);

                    // if we successfully matched, get the index location of the match inside
                    // the textbox control and the length of the match
                    if (match.Success)
                    {
                        _currentIndex = match.Index;
                        searchLength = match.Length;
                    }
                    else
                    {
                        // no match
                        _currentIndex = -1;
                    }
                }
                else
                {
                    // this search is for non-regular expressions case-insensitive
                    CultureInfo culture = new CultureInfo("en-us");
                    _currentIndex = culture.CompareInfo.IndexOf(txtControl.Text, searchString, _currentIndex, System.Globalization.CompareOptions.IgnoreCase);
                }
            }

            // if we found a match, select it in the multiline text box
            if (_currentIndex >= 0)
            {

                // (note: this should be refactored, but is shown in one place for the sake of the
                // article.)

                // select the matching text
                txtControl.SelectionStart = txtControl.Text.IndexOf("\n", _currentIndex) + 2;
                txtControl.SelectionLength = 0;
                txtControl.SelectionStart = _currentIndex;
                txtControl.SelectionLength = searchLength;

                _currentIndex += searchLength; // advance past selection
                txtControl.ScrollToCaret();  // scroll to selection
                //				_txtControl.Invalidate();
            }
            else
            {
                // no match, reached the end of the document
              //  MessageBox.Show("Τέλος Εγγράφου.");
                _currentIndex = 0;
                return false;
            }

            return true;
        }
        void ReplaceSelection(string text, bool scroll)
        {
            
           // FlickerFreeTextBox._Paint = false;
            int saveStart = _txtControl.SelectionStart;
            // replace text at found position (remember to go back one)
            _txtControl.Text = _txtControl.Text.Remove(_currentIndex - 1, _txtControl.SelectionLength);
            _txtControl.Text = _txtControl.Text.Insert(_currentIndex - 1, text);
            _txtControl.SelectionStart = _txtControl.Text.IndexOf("\n", saveStart) + 2;
            _txtControl.SelectionLength = 0;
            if (scroll)
            {
                _txtControl.ScrollToCaret(); // scroll past selection
            }
            _txtControl.SelectionStart = saveStart;
            _txtControl.SelectionLength = text.Length;
            //FlickerFreeTextBox._Paint = true;
        }
        bool CurrentSelectedTextIs(string txt)
        {
            if (_caseSensitive)
            {
                return (_txtControl.SelectedText == txt);
            }

            return (_txtControl.SelectedText.ToUpper() == txt.ToUpper());
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            _text = txtFind.Text;
            _caseSensitive = chkCaseSensitive.Checked;
            _regularExpressions = chkRegularExpression.Checked;
            _txtControl.Focus();
            FindNext(_text, _caseSensitive, _txtControl, _regularExpressions);
        }
        private void btnReplace_Click_1(object sender, EventArgs e)
        {
            _text = txtFind.Text;
            if (CurrentSelectedTextIs(_text))
            {
                _currentIndex = _txtControl.SelectionStart + 1;
                ReplaceSelection(txtReplace.Text, false);
                return;
            }

            _caseSensitive = chkCaseSensitive.Checked;
            _regularExpressions = chkRegularExpression.Checked;
            _txtControl.Focus();
            bool stillgoing = FindNext(_text, _caseSensitive, _txtControl, _regularExpressions);
            if (stillgoing)
            {
                ReplaceSelection(txtReplace.Text, true);
            }
        }
        private void chkontop_CheckedChanged(object sender, EventArgs e)
        {
            // Determine the CheckState of the check box.
            if (chkontop.CheckState == CheckState.Checked)
            {
                // If checked, do not allow items to be dragged onto the form.
                this.TopMost=true;
            }
            if (chkontop.CheckState == CheckState.Unchecked)
                this.TopMost = false;
     

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Help a = new Help();
            a.ShowDialog();
        }
    }
}
