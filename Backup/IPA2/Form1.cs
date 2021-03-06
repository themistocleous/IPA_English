using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;
using SpeechLib;


namespace IPA2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Exit()
        {
            //Application.Exit();
            this.Close();
        }
        private void Save()
        {
            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFileDialog1.Filter = "Απλό Κείμενο (Unicode)|*.txt|Απλό Κείμενο|*.txt|Μορφή Εμπλουτισμένου Κειμένου (*.rtf)|*.rtf|Όλα τα Αρχεία (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Αποθήκευση";
            
            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFileDialog1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                if (saveFileDialog1.FilterIndex == 1)
                    rtxText.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.UnicodePlainText);
                if (saveFileDialog1.FilterIndex == 2)
                    rtxText.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                if (saveFileDialog1.FilterIndex == 3)
                    rtxText.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
            }
        }
        private void Open()
        {// Create an OpenFileDialog to request a file to open.
            OpenFileDialog openFile1 = new OpenFileDialog();

            // Initialize the OpenFileDialog to look for TXT files.
            openFile1.DefaultExt = "*.txt";
            openFile1.Filter = "Αρχεία Κειμένου (Unicode)|*.txt|Αρχεία Απλού Κειμένου|*.txt|Μορφή Εμπλουτισμένου Κειμένου (*.rtf)|*.rtf|Όλα τα Αρχεία (*.*)|*.*";

            // Determine whether the user selected a file from the OpenFileDialog.
            if (openFile1.ShowDialog() == DialogResult.OK && openFile1.FileName.Length > 0)
            {
                if (saveFileDialog1.FilterIndex == 1)
                    rtxText.LoadFile(openFile1.FileName, RichTextBoxStreamType.UnicodePlainText);
               
                if (saveFileDialog1.FilterIndex == 2)
                    rtxText.LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);
                
                if (saveFileDialog1.FilterIndex == 3)
                    rtxText.LoadFile(openFile1.FileName, RichTextBoxStreamType.RichText);
            }
        }
        
        private void About()
        {
            // The string text shown on a form
            string message = "Το πρόγραμμα αυτό επιτρέπει την μεταφορά κειμένου σε ΔΦΑ στην κοινή ελληνική και στην κυπριακή διάλεκτο. Αποθηκεύστε σε Unicode το κείμενο σας πριν το ανοίξετε με αυτό το πρόγραμμα. \nΧαράλαμπος Θεμιστοκλεόυς 2007. \n\nThis program turns a text in standard orthography to IPA (Athenian Greek or Cyprus Dialect). Save your text as Unicode first, before using.\nCharalampos Themistocleous.";
            // The caption for the form's title
            string caption = "Μετατροπή Ελληνικών σε Διεθνές Φωνητικό Αλφάβητο 0.1.";
            // A Button to close the form
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            // An icon by the text
            MessageBox.Show(message, caption, buttons, MessageBoxIcon.Asterisk);
        }
        private void Statistica()
        {
            string message = "\nΣτατιστικά στοιχεία"+
                             "\n--------------------"+            
                "\nΛέξεις περίπου:       \t\t" + splitwords(rtxText.Text).Length.ToString() +
                "\nΧαρακτήρες (χωρίς κενά):\t" + DeleteSpace(rtxText.Text).Length.ToString() +
                "\nXαρακτήρες (με κενά):   \t" + rtxText.TextLength.ToString() +
                "\nΠροτάσεις περίπου:      \t" + splitsent(rtxText.Text).Length.ToString()+
                "\nΠαράγραφοι περίπου:     \t" + CountParagraphs(rtxText.Text)+".";
            // The caption for the form's title
            string caption = "Καταμέτρηση Λέξεων";
            // A Button to close the form
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            // An icon by the text
            MessageBox.Show(message, caption, buttons);
        }
        private static string CountParagraphs(string aString)
        {
            aString = aString.Replace("\n\n\n", "\n");
            aString = aString.Replace("\n\n", "\n");
            aString = aString.Trim();
            string[] paragraphs = aString.Split('\n');
            return paragraphs.Length.ToString();
        }
        private string RemoveParagraphs(string aString)
        {
            aString = aString.Replace("\n", " ");

            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        }
        private bool NotLowerCase(string aString)
        {
            if (aString != "" && aString[0].ToString().ToLower() == aString[0].ToString() &&
                aString[0].ToString().ToUpper() == aString[0].ToString())
            {
                // the first letter IS not a letter :) it is a number 1,2,3... or a symbol like % # ...
                return true;
            }
            else
            {
                // here it IS a letter a,b,c,d.....
                if (aString != "" && aString[0].ToString().ToLower() == aString[0].ToString())
                    return false; //it IS lowercase
                else
                    return true; // it IS uppercase
            }
        }
        private string[] splitsent(string instr)
        {
            string[] tokens = instr.Trim().Split(' ');
            // create an arraylist to store sentences
            ArrayList sentence = new ArrayList();
            // itterate through the tokens to find words ending with a dot
            // copy the sentence to the arraylist
            string tempSentence = "";
            for (int i = 0; i < tokens.Length - 1; i++)
            {

                if (tokens[i] != "" && !(tokens[i].IndexOf(".") < tokens[i].Length - 1) &&
                    NotLowerCase(tokens[i + 1]) && (tokens[i][tokens[i].Length - 1] == '.' ||
                    tokens[i][tokens[i].Length - 1] == '!' || tokens[i][tokens[i].Length - 1] == ';' || tokens[i][tokens[i].Length - 1] == '·'))
                {
                    tempSentence += tokens[i].Substring(0, tokens[i].Length - 1) + " " + tokens[i].Substring(tokens[i].Length - 1, 1);
                    sentence.Add(tempSentence.TrimStart());
                    tempSentence = "";
                }
                else
                {
                    tempSentence += tokens[i] + " ";
                }
            }

            tempSentence += tokens[tokens.Length - 1].Substring(0, tokens[tokens.Length - 1].Length - 1) + " " + tokens[tokens.Length - 1].Substring(tokens[tokens.Length - 1].Length - 1, 1);
            sentence.Add(tempSentence.TrimStart());

            return (string[])sentence.ToArray(typeof(string));


            //return tokens; // when this works.... you should return the arraylist and not the tokens
        }
        private string[] splitwords(string aString)
        {
          // aString = aString.Replace("   ", "  ");
          // aString = aString.Replace("  ", " ");
            aString = aString.Trim();
            string[] words = aString.Split(' ');
            return words;
        } 
        private static string Reverser(string ins)
        {
            string reversed = "";

            ins = ins.Replace("\t", " ");
            ins = ins.Replace("\\", " ");
            ins = ins.Replace("/", " ");
            ins = ins.Replace("(", " ");
            ins = ins.Replace(")", " ");
            ins = ins.Replace("{", " ");
            ins = ins.Replace("}", " ");
            ins = ins.Replace("[", " ");
            ins = ins.Replace("]", " ");
            ins = ins.Replace("*", " ");
            ins = ins.Replace("#", " ");
            ins = ins.Replace("&", " ");
            ins = ins.Replace("-", " ");
            ins = ins.Replace("_", " ");
            ins = ins.Replace(",", " ");
            ins = ins.Replace(":", " ");
            ins = ins.Replace("...", " ");
            ins = ins.Replace("@", " ");
            ins = ins.Replace("«", " ");
            ins = ins.Replace("»", " ");



            for (int i = ins.Length - 1; i >= 0; i--)
            {
                // someText[i] is the string index, 
                // acts very similar to an array 
                reversed += ins[i];
            }
            return reversed;
        } // Αντιστροφή Κειμένου.

        private string DeleteSpace(string aString)
        {
            aString = aString.Replace("\n", "");
            aString = aString.Replace("\r", "");
            aString = aString.Replace("\t", "");
            aString = aString.Replace(" ", "");
            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } // Διαγραφή Διαστήματος.
        private string TrimEndText(string aString)
        {
            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } // Διαγραφή Κενού Διαστήματος στο Τέλος.

        private string toGRIPA(string aString)
        {
            //modification 9/8/2008 Charalabos Themistocleous
            //remove uppercase
            UseWaitCursor = true;
            //
            aString = aString.Replace("Β", "β");
            aString = aString.Replace("Γ", "γ");
            aString = aString.Replace("Δ", "δ");
            aString = aString.Replace("Ζ", "ζ");
            aString = aString.Replace("Θ", "θ");
            aString = aString.Replace("Κ", "κ");
            aString = aString.Replace("Λ", "λ");
            aString = aString.Replace("Μ", "μ");
            aString = aString.Replace("Ν", "ν");
            aString = aString.Replace("Ξ", "ξ");
            aString = aString.Replace("Ο", "ο");
            aString = aString.Replace("Ο", "ό");
            aString = aString.Replace("Π", "π");
            aString = aString.Replace("Ρ", "ρ");
            aString = aString.Replace("Σ", "σ");
            aString = aString.Replace("Τ", "τ");
            aString = aString.Replace("Φ", "φ");
            aString = aString.Replace("Ψ", "ψ");
            aString = aString.Replace("Χ", "x");

            aString = aString.Replace("Α", "α");
            aString = aString.Replace("Ά", "ά");
            aString = aString.Replace("Η", "η");
            aString = aString.Replace("Ή", "ή");
            aString = aString.Replace("Ι", "ι");
            aString = aString.Replace("Ί", "ί");
            aString = aString.Replace("Υ", "υ");
            aString = aString.Replace("Ύ", "ύ");
            aString = aString.Replace("Ε", "ε");
            aString = aString.Replace("Έ", "έ");
            aString = aString.Replace("Ω", "ω");
            aString = aString.Replace("Ώ", "ώ");

            //extra rules: economy and avoid useless repeditions
            aString = aString.Replace("αυγ", "αβγ");
            aString = aString.Replace("αυλ", "αβλ");
            aString = aString.Replace("αύγ", "άβγ");
            aString = aString.Replace("αύλ", "αβλ");
            aString = aString.Replace("ευτ", "εφτ");
            aString = aString.Replace("εύτ", "έφτ");
            aString = aString.Replace("ευκ", "εφκ");
            aString = aString.Replace("εύκ", "έφκ");
            aString = aString.Replace("ευπ", "εφπ");
            aString = aString.Replace("εύπ", "έφπ");
            aString = aString.Replace("ευθ", "εφθ");
            aString = aString.Replace("εύθ", "έφθ");
            aString = aString.Replace("ευφ", "εφ");
            aString = aString.Replace("εύφ", "έφ");
            aString = aString.Replace("ευχ", "εφχ");
            aString = aString.Replace("εύχ", "έφχ");

            aString = aString.Replace("αυτ", "αφτ");
            aString = aString.Replace("αύτ", "άφτ");
            aString = aString.Replace("αυκ", "αφκ");
            aString = aString.Replace("αύκ", "άφκ");
            aString = aString.Replace("αυπ", "αφπ");
            aString = aString.Replace("αύπ", "άφπ");
            aString = aString.Replace("αυθ", "αφθ");
            aString = aString.Replace("αύθ", "άφθ");
            aString = aString.Replace("αυφ", "αφ");
            aString = aString.Replace("αύφ", "άφ");
            aString = aString.Replace("αυχ", "αφχ");
            aString = aString.Replace("αύχ", "άφχ");

            aString = aString.Replace("αυ", "av");
            aString = aString.Replace("αύ", "áv");
            aString = aString.Replace("ευ", "ev");
            aString = aString.Replace("εύ", "év");
            aString = aString.Replace("γγ", "γκ");
            aString = aString.Replace("ντζ", "nτζ");
            aString = aString.Replace("ντσ", "nτσ");
            aString = aString.Replace("ντ", "d");
            aString = aString.Replace("μπ", "b");
            aString = aString.Replace("αι", "ε"); //Αυτή η σειρά επιτρέπει την απαγόρευση της εφαρμογής του κανόνα σε λέξεις όπως αβαείο γιατί διαφορετικά θα είχα αβαίο > αβέο
            aString = aString.Replace("αί", "έ");
            aString = aString.Replace("εί", "ί");
            aString = aString.Replace("ει", "ι");
            aString = aString.Replace("οι", "ι");
            aString = aString.Replace("οί", "ί");
            aString = aString.Replace("ου", "u");
            aString = aString.Replace("ού", "ú");
            aString = aString.Replace("σσ", "σ");
            aString = aString.Replace("θθ", "θ");
            aString = aString.Replace("κκ", "κ");
            aString = aString.Replace("λλ", "λ");
            aString = aString.Replace("μμ", "μ");
            aString = aString.Replace("ππ", "π");
            aString = aString.Replace("ρρ", "ρ");
            aString = aString.Replace("ττ", "τ");
            aString = aString.Replace("φφ", "φ");
            aString = aString.Replace("χχ", "χ");
            aString = aString.Replace("ω", "ο");
            aString = aString.Replace("ώ", "ó");
            aString = aString.Replace("η", "ι");
            aString = aString.Replace("ή", "ί");
            aString = aString.Replace("υ", "ι");
            aString = aString.Replace("ύ", "ί");
            aString = aString.Replace("ξ", "κσ");
            aString = aString.Replace("ψ", "πσ");
            aString = aString.Replace("ϊ", "i");
            aString = aString.Replace("ΐ", "í");
            aString = aString.Replace("ϋ", "i");
            aString = aString.Replace("ΰ", "í");
            

            // end of extra rule

/*            aString = aString.Replace("ποιού", "piú");
            aString = aString.Replace("ποιοί", "pií");
            aString = aString.Replace("ποιώ", "pió");
            aString = aString.Replace("ποιέ", "pié");
            aString = aString.Replace("ποιά", "piá"); */

//========================================================
//5
            aString = aString.Replace("γκιu", "ɉu");
            aString = aString.Replace("γκιú", "ɉú");
            aString = aString.Replace("γκιι", "ɉi");
            aString = aString.Replace("γκιί", "ɉí");
            aString = aString.Replace("γκια", "ɉa");
            aString = aString.Replace("γκιά", "ɉá");
            aString = aString.Replace("γκιε", "ɉe");
            aString = aString.Replace("γκιέ", "ɉé");
            aString = aString.Replace("γκιο", "ɉο");
            aString = aString.Replace("γκιό", "ɉó");
            //=======================================
            aString = aString.Replace("αυχι", "afçi");
            aString = aString.Replace("αυχί", "afçí");
            aString = aString.Replace("αυχε", "afçe");
            aString = aString.Replace("αυχέ", "afçé");
            aString = aString.Replace("ευχι", "efçi");
            aString = aString.Replace("ευχί", "efçí");
            aString = aString.Replace("ευχε", "efçe");
            aString = aString.Replace("ευχέ", "efçé");
            //=======================================
            aString = aString.Replace("κσια", "ksça");
            aString = aString.Replace("κσιά", "ksçá");
            aString = aString.Replace("κσιε", "ksçe");
            aString = aString.Replace("κσιέ", "ksçé");
            aString = aString.Replace("κσιι", "ksçi");
            aString = aString.Replace("κσιί", "ksçí");
            aString = aString.Replace("κσιο", "ksçο");
            aString = aString.Replace("κσιό", "ksçó");
            aString = aString.Replace("κσιu", "ksçu");
            aString = aString.Replace("κσιú", "ksçú");     
            //========================================
            aString = aString.Replace("πσιu", "psçu");
            aString = aString.Replace("πσιú", "psçú");
            aString = aString.Replace("πσιι", "psçi");
            aString = aString.Replace("πσιί", "psçí");
            aString = aString.Replace("πσια", "psça");
            aString = aString.Replace("πσιά", "psçá");
            aString = aString.Replace("πσιε", "psçe");
            aString = aString.Replace("πσιέ", "psçé");
            aString = aString.Replace("πσιο", "psçο");
            aString = aString.Replace("πσιό", "psçó");
            //========================================
            aString = aString.Replace("τσιu", "tsçu");
            aString = aString.Replace("τσιú", "tsçú");
            aString = aString.Replace("τσιι", "tsçi");
            aString = aString.Replace("τσιί", "tsçí");
            aString = aString.Replace("τσια", "tsça");
            aString = aString.Replace("τσιά", "tsçá");
            aString = aString.Replace("τσιε", "tsçe");
            aString = aString.Replace("τσιέ", "tsçé");
            aString = aString.Replace("τσιο", "tsçο");
            aString = aString.Replace("τσιό", "tsçó");
            //======================================
            aString = aString.Replace("θια", "θça");
            aString = aString.Replace("θιά", "θçá");
            aString = aString.Replace("θιε", "θçe");
            aString = aString.Replace("θιέ", "θçé");
            aString = aString.Replace("θιι", "θçi");
            aString = aString.Replace("θιί", "θçí");
            aString = aString.Replace("θιο", "θçο");
            aString = aString.Replace("θιό", "θçó");
            aString = aString.Replace("θιu", "θçu");
            aString = aString.Replace("θιú", "θçú");
            //=====================================
            aString = aString.Replace("φια", "fça");
            aString = aString.Replace("φιά", "fçá");
            aString = aString.Replace("φιε", "fçe");
            aString = aString.Replace("φιέ", "fçé");
            aString = aString.Replace("φιι", "fçi");
            aString = aString.Replace("φιί", "fçí");
            aString = aString.Replace("φιο", "fçο");
            aString = aString.Replace("φιό", "fçó");
            aString = aString.Replace("φιu", "fçu");
            aString = aString.Replace("φιú", "fçú");
            //=====================================
            aString = aString.Replace("κια", "cça");
            aString = aString.Replace("κιά", "cçá");
            aString = aString.Replace("κιε", "cçe");
            aString = aString.Replace("κιέ", "cçé");
            aString = aString.Replace("κιι", "cçi");
            aString = aString.Replace("κιί", "cçí");
            aString = aString.Replace("κιο", "cçο");
            aString = aString.Replace("κιό", "cçó");
            aString = aString.Replace("κιu", "cçu");
            aString = aString.Replace("κιú", "cçú");
            //=====================================
            aString = aString.Replace("πια", "pça");
            aString = aString.Replace("πιά", "pçá");
            aString = aString.Replace("πιε", "pçe");
            aString = aString.Replace("πιέ", "pçé");
            aString = aString.Replace("πιι", "pçi");
            aString = aString.Replace("πιί", "pçí");
            aString = aString.Replace("πιο", "pçο");
            aString = aString.Replace("πιό", "pçó");
            aString = aString.Replace("πιu", "pçu");
            aString = aString.Replace("πιú", "pçú");
            //=====================================
            aString = aString.Replace("σια", "sça");
            aString = aString.Replace("σιά", "sçá");
            aString = aString.Replace("σιε", "sçe");
            aString = aString.Replace("σιέ", "sçé");
            aString = aString.Replace("σιι", "sçi");
            aString = aString.Replace("σιί", "sçí");
            aString = aString.Replace("σιο", "sçο");
            aString = aString.Replace("σιό", "sçó");
            aString = aString.Replace("σιu", "sçu");
            aString = aString.Replace("σιú", "sçú");
            //=====================================
            aString = aString.Replace("τια", "tça");
            aString = aString.Replace("τιά", "tçá");
            aString = aString.Replace("τιε", "tçe");
            aString = aString.Replace("τιέ", "tçé");
            aString = aString.Replace("τιι", "tçi");
            aString = aString.Replace("τιί", "tçí");
            aString = aString.Replace("τιο", "tçο");
            aString = aString.Replace("τιό", "tçó");
            aString = aString.Replace("τιu", "tçu");
            aString = aString.Replace("τιú", "tçú");
            //======================================
            aString = aString.Replace("βια", "vʝa");
            aString = aString.Replace("βιά", "vʝá");
            aString = aString.Replace("βιε", "vʝe");
            aString = aString.Replace("βιέ", "vʝé");
            aString = aString.Replace("βιο", "vʝο");
            aString = aString.Replace("βιό", "vʝó");
            aString = aString.Replace("βιu", "vʝu");
            aString = aString.Replace("βιú", "vʝú");
            //======================================
            aString = aString.Replace("bια", "bʝa");
            aString = aString.Replace("bιά", "bʝá");
            aString = aString.Replace("bιε", "bʝe");
            aString = aString.Replace("bιέ", "bʝé");
            aString = aString.Replace("bιο", "bʝο");
            aString = aString.Replace("bιό", "bʝó");
            aString = aString.Replace("bιu", "bʝu");
            aString = aString.Replace("bιú", "bʝú");
            //======================================
            aString = aString.Replace("δια", "ðʝa");
            aString = aString.Replace("διά", "ðʝá");
            aString = aString.Replace("διε", "ðʝe");
            aString = aString.Replace("διέ", "ðʝé");
            aString = aString.Replace("διο", "ðʝο");
            aString = aString.Replace("διό", "ðʝó");
            aString = aString.Replace("διu", "ðʝu");
            aString = aString.Replace("διú", "ðʝú");
            //======================================
            aString = aString.Replace("dια", "dʝa");
            aString = aString.Replace("dιά", "dʝá");
            aString = aString.Replace("dιε", "dʝe");
            aString = aString.Replace("dιέ", "dʝé");
            aString = aString.Replace("dιο", "dʝο");
            aString = aString.Replace("dιό", "dʝó");
            aString = aString.Replace("dιu", "dʝu");
            aString = aString.Replace("dιú", "dʝú");
            //======================================
            aString = aString.Replace("ζια", "zʝa");
            aString = aString.Replace("ζιά", "zʝá");
            aString = aString.Replace("ζιε", "zʝe");
            aString = aString.Replace("ζιέ", "zʝé");
            aString = aString.Replace("ζιο", "zʝο");
            aString = aString.Replace("ζιό", "zʝó");
            aString = aString.Replace("ζιu", "zʝu");
            aString = aString.Replace("ζιú", "zʝú");
            //======================================
            aString = aString.Replace("ρια", "rʝa"); // Η περίπτωση του ρ ανκαι μοιάζει με το πιο πάνω δεν είναι από ότι φαίνεται το ι διατηρεί την ανεξαρτησία του και δεν συμπροφέρεται.
            aString = aString.Replace("ριά", "rʝá");
            aString = aString.Replace("ριε", "rʝe");
            aString = aString.Replace("ριέ", "rʝé");
            aString = aString.Replace("ριο", "rʝο");
            aString = aString.Replace("ριό", "rʝó");
            aString = aString.Replace("ριu", "rʝu");
            aString = aString.Replace("ριú", "rʝú");
            //======================================
            aString = aString.Replace("μια", "mɲa");
            aString = aString.Replace("μιά", "mɲá");
            aString = aString.Replace("μιε", "mɲe");
            aString = aString.Replace("μιέ", "mɲé");
            aString = aString.Replace("μιο", "mɲο");
            aString = aString.Replace("μιό", "mɲó");
            aString = aString.Replace("μιu", "mɲu");
            aString = aString.Replace("μιú", "mɲú");
            //======================================
            aString = aString.Replace("για", "ʝa");
            aString = aString.Replace("γιά", "ʝá");
            aString = aString.Replace("γιε", "ʝe");
            aString = aString.Replace("γιέ", "ʝé");
            aString = aString.Replace("γιι", "ʝi");
            aString = aString.Replace("γιί", "ʝí");
            aString = aString.Replace("γιο", "ʝο");
            aString = aString.Replace("γιό", "ʝó"); 
            aString = aString.Replace("γιu", "ʝu");
            aString = aString.Replace("γιú", "ʝú");
            //=====================================
            aString = aString.Replace("λιu", "ʎu");
            aString = aString.Replace("λιú", "ʎú");
            aString = aString.Replace("λιι", "ʎi");
            aString = aString.Replace("λιί", "ʎí");
            aString = aString.Replace("λια", "ʎa");
            aString = aString.Replace("λιά", "ʎá");
            aString = aString.Replace("λιε", "ʎe");
            aString = aString.Replace("λιέ", "ʎé");
            aString = aString.Replace("λιο", "ʎο");
            aString = aString.Replace("λιό", "ʎó");
            //=====================================
            aString = aString.Replace("νιu", "ɲu");
            aString = aString.Replace("νιú", "ɲú");
            aString = aString.Replace("νιι", "ɲi");
            aString = aString.Replace("νιί", "ɲí");
            aString = aString.Replace("νια", "ɲa");
            aString = aString.Replace("νιά", "ɲá");
            aString = aString.Replace("νιε", "ɲe");
            aString = aString.Replace("νιέ", "ɲé");
            aString = aString.Replace("νιό", "ɲó");
            aString = aString.Replace("νιο", "ɲο");
            //=====================================
            aString = aString.Replace("χιu", "çu");
            aString = aString.Replace("χιú", "çú");
            aString = aString.Replace("χιι", "çi");
            aString = aString.Replace("χιί", "çí");
            aString = aString.Replace("χια", "ça");
            aString = aString.Replace("χιά", "çá");
            aString = aString.Replace("χιε", "çe");
            aString = aString.Replace("χιέ", "çé");
            aString = aString.Replace("χιο", "çο");
            aString = aString.Replace("χιό", "çó");



            aString = aString.Replace("γχ", "ŋx"); //χρειάζομαι περισσότερους κανόνες τους έβαλα κάτω από ΔΦΑ σε ΔΦΑ


            aString = aString.Replace("γκι", "ɉi");
            aString = aString.Replace("γκί", "ɉí");
            aString = aString.Replace("γκε", "ɉe");
            aString = aString.Replace("γκέ", "ɉé");
            aString = aString.Replace("γκ", "g");
            aString = aString.Replace("νν", "n");
            aString = aString.Replace("γι", "ʝi");
            aString = aString.Replace("γί", "ʝí");
            aString = aString.Replace("γε", "ʝe");
            aString = aString.Replace("γέ", "ʝé");
            aString = aString.Replace("κι", "ci");
            aString = aString.Replace("κί", "cí");
            aString = aString.Replace("κε", "ce");
            aString = aString.Replace("κέ", "cé");
            aString = aString.Replace("τσ", "ts");
            aString = aString.Replace("χι", "çi");
            aString = aString.Replace("χί", "çí");
            aString = aString.Replace("χε", "çe");
            aString = aString.Replace("χέ", "çé");
            
          /*  aString = aString.Replace("ιέ", "jé");
            aString = aString.Replace("ιό", "jó");
            aString = aString.Replace("ιά", "já");
            aString = aString.Replace("ιε", "je");
            aString = aString.Replace("ιο", "jo");
            aString = aString.Replace("ια", "ja");
            aString = aString.Replace("ιu", "ju");
            aString = aString.Replace("ιú", "jú"); */



            aString = aString.Replace("β", "v");
            aString = aString.Replace("γ", "ɣ");
            aString = aString.Replace("δ", "ð");
            aString = aString.Replace("ζ", "z");
            aString = aString.Replace("θ", "θ");
            aString = aString.Replace("κ", "k");
            aString = aString.Replace("λ", "l");
            aString = aString.Replace("μ", "m");
            aString = aString.Replace("ν", "n");
            aString = aString.Replace("ξ", "ks");
            aString = aString.Replace("π", "p");
            aString = aString.Replace("ρ", "r");
            aString = aString.Replace("σ", "s");
            aString = aString.Replace("ς", "s");
            aString = aString.Replace("τ", "t");
            aString = aString.Replace("φ", "f");
            aString = aString.Replace("ψ", "ps");
            aString = aString.Replace("χ", "x");

            aString = aString.Replace("α", "a");
            aString = aString.Replace("ά", "á");
            aString = aString.Replace("ι", "i");
            aString = aString.Replace("ί", "í");
            aString = aString.Replace("ο", "o");
            aString = aString.Replace("ό", "ó");
            aString = aString.Replace("ε", "e");
            aString = aString.Replace("έ", "é");
            aString = aString.Replace("ω", "o");
            aString = aString.Replace("ώ", "ó");


            aString = aString.Replace("np", "b");
            aString = aString.Replace("nd", "d");
            aString = aString.Replace("sv", "zv");
            aString = aString.Replace("sɣ", "zɣ");
            aString = aString.Replace("sð", "zð");
            aString = aString.Replace("sm", "zm");
            aString = aString.Replace("sn", "zn");
            aString = aString.Replace("sd", "zd");
            aString = aString.Replace("sb", "zb");
            aString = aString.Replace("sg", "zg");
            aString = aString.Replace("sr", "zr");

            aString = aString.Replace("nk", "ŋk");
            aString = aString.Replace("ng", "ŋg");

            aString = aString.Replace("nc", "ŋc");
            aString = aString.Replace("nc", "ŋc");

            aString = aString.Replace("mf", "ɱf");
            aString = aString.Replace("mv", "ɱv");

            UseWaitCursor = false;
            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } // Αλγόριθμος μετατροπής σε ΔΦΑ για λέξεις χωρίς συμπροφορά.

       /* private string toGRIPADIC(string aString)
        {
            Regex r1 = new Regex(@"γριά>");
            Regex r2 = new Regex(@"γριάς");
            
            while (!aString.) 
		{
			if (r1.Match(inString).Success)
			{
				Console.WriteLine("Ελίζα: {0}",r1.Replace(inString,"Γιατί είσαι $2?"));
			}
			else if (r2.Match(inString).Success)
			{
				Console.WriteLine("Ελίζα: {0}",r2.Replace(inString,"Γιατί όταν είσαι $1 είναι κακό?"));
			}


        }*/
        public string GreektoUnicode(string ins)
        {
            ins = ins.Replace("*)=|A", "ᾎ");
            ins = ins.Replace("*(=|A", "ᾏ");
            ins = ins.Replace("*)/|A", "ᾌ");
            ins = ins.Replace("*)\\|A", "ᾊ");
            ins = ins.Replace("*(/|A", "ᾍ");
            ins = ins.Replace("*(\\|A", "ᾋ");
            ins = ins.Replace("*)|A", "ᾈ");
            ins = ins.Replace("*(|A", "ᾉ");
            ins = ins.Replace("*)=A", "Ἆ");
            ins = ins.Replace("*(=A", "Ἇ");
            ins = ins.Replace("*)/A", "Ἄ");
            ins = ins.Replace("*)\\A", "Ἂ");
            ins = ins.Replace("*(/A", "Ἅ");
            ins = ins.Replace("*(\\A", "Ἃ");
            ins = ins.Replace("*/A", "Ά");
            ins = ins.Replace("*\\A", "Ὰ");
            ins = ins.Replace("*)A", "Ἀ");
            ins = ins.Replace("*(A", "Ἁ");
            ins = ins.Replace("*|A", "ᾼ");



            ins = ins.Replace("*)/E", "Ἔ");
            ins = ins.Replace("*)\\E", "Ἒ");
            ins = ins.Replace("*(/E", "Ἕ");
            ins = ins.Replace("*(\\E", "Ἓ");
            ins = ins.Replace("*/E", "Έ");
            ins = ins.Replace("*\\E", "Ὲ");
            ins = ins.Replace("*)E", "Ἐ");
            ins = ins.Replace("*(E", "Ἑ");

            ins = ins.Replace("*)=|H", "ᾞ");
            ins = ins.Replace("*(=|H", "ᾟ");
            ins = ins.Replace("*)/|H", "ᾜ");
            ins = ins.Replace("*)\\|H", "ᾚ");
            ins = ins.Replace("*(/|H", "ᾝ");
            ins = ins.Replace("*(\\|H", "ᾛ");
            ins = ins.Replace("*)|H", "ᾘ");
            ins = ins.Replace("*(|H", "ᾙ");
            ins = ins.Replace("*)=H", "Ἦ");
            ins = ins.Replace("*(=H", "Ἧ");
            ins = ins.Replace("*)/H", "Ἤ");
            ins = ins.Replace("*)\\H", "Ἢ");
            ins = ins.Replace("*(/H", "Ἥ");
            ins = ins.Replace("*(\\H", "Ἣ");
            ins = ins.Replace("*/H", "Ή");
            ins = ins.Replace("*\\H", "Ὴ");
            ins = ins.Replace("*)H", "Ἠ");
            ins = ins.Replace("*(H", "Ἡ");
            ins = ins.Replace("*|H", "ῌ");

            ins = ins.Replace("*)=I", "Ἶ");
            ins = ins.Replace("*(=I", "Ἷ");
            ins = ins.Replace("*)/I", "Ἴ");
            ins = ins.Replace("*)\\I", "Ἲ");
            ins = ins.Replace("*(/I", "Ἵ");
            ins = ins.Replace("*(\\I", "Ἳ");
            ins = ins.Replace("*/I", "Ί");
            ins = ins.Replace("*\\I", "Ὶ");
            ins = ins.Replace("*)I", "Ἰ");
            ins = ins.Replace("*(I", "Ἱ");

            ins = ins.Replace("*)/O", "Ὄ");
            ins = ins.Replace("*)\\O", "Ὂ");
            ins = ins.Replace("*(/O", "Ὅ");
            ins = ins.Replace("*(\\O", "Ὃ");
            ins = ins.Replace("*/O", "Ό");
            ins = ins.Replace("*\\O", "Ὸ");
            ins = ins.Replace("*)O", "Ὀ");
            ins = ins.Replace("*(O", "Ὁ");

            ins = ins.Replace("*(=U", "Ὗ");
            ins = ins.Replace("*(/U", "Ὕ");
            ins = ins.Replace("*(\\U", "Ὓ");
            ins = ins.Replace("*/U", "Ύ");
            ins = ins.Replace("*\\U", "Ὺ");
            ins = ins.Replace("*(U", "Ὑ");

            ins = ins.Replace("*)=|W", "ᾮ");
            ins = ins.Replace("*(=|W", "ᾯ");
            ins = ins.Replace("*)/|W", "ᾬ");
            ins = ins.Replace("*)\\|W", "ᾪ");
            ins = ins.Replace("*(/|W", "ᾭ");
            ins = ins.Replace("*(\\|W", "ᾫ");
            ins = ins.Replace("*)|W", "ᾨ");
            ins = ins.Replace("*(|W", "ᾩ");
            ins = ins.Replace("*)=W", "Ὦ");
            ins = ins.Replace("*(=W", "Ὧ");
            ins = ins.Replace("*)/W", "Ὤ");
            ins = ins.Replace("*)\\W", "Ὢ");
            ins = ins.Replace("*(/W", "Ὥ");
            ins = ins.Replace("*(\\W", "Ὣ");
            ins = ins.Replace("*/W", "Ώ");
            ins = ins.Replace("*\\W", "Ὼ");
            ins = ins.Replace("*)W", "Ὠ");
            ins = ins.Replace("*(W", "Ὡ");
            ins = ins.Replace("*|W", "ῼ");

            ins = ins.Replace("*(/W", "Ὥ");

            ins = ins.Replace("A)=|", " ᾆ");
            ins = ins.Replace("A(=|", "ᾇ");
            ins = ins.Replace("A)/|", " ᾄ");
            ins = ins.Replace("A)\\|", " ᾂ");
            ins = ins.Replace("A(/|", "ᾅ");
            ins = ins.Replace("A(\\|", "ᾃ");
            ins = ins.Replace("A)|", " ᾀ");
            ins = ins.Replace("A(|", "ᾁ");
            ins = ins.Replace("A\\|", "ᾲ");
            ins = ins.Replace("A/|", "ᾴ");
            ins = ins.Replace("A=|", "ᾷ");
            ins = ins.Replace("A)=", " ἆ");
            ins = ins.Replace("A(=", "ἇ");
            ins = ins.Replace("A)/", " ἄ");
            ins = ins.Replace("A)\\", " ἂ");
            ins = ins.Replace("A(/", "ἅ");
            ins = ins.Replace("A(\\", "ἃ");
            ins = ins.Replace("A/", "ά");
            ins = ins.Replace("A\\", "ὰ");
            ins = ins.Replace("A)", " ἀ");
            ins = ins.Replace("A(", "ἁ");
            ins = ins.Replace("A|", "ᾳ");
            ins = ins.Replace("A=", "ᾶ");

            ins = ins.Replace("E)/", "ἔ");
            ins = ins.Replace("E)\\", "ἒ");
            ins = ins.Replace("E(/", "ἓ");
            ins = ins.Replace("E(\\", "ἕ");
            ins = ins.Replace("E/", "έ");
            ins = ins.Replace("E\\", "ὲ");
            ins = ins.Replace("E)", "ἐ");
            ins = ins.Replace("E(", "ἑ");

            ins = ins.Replace("H)=|", "ᾖ");
            ins = ins.Replace("H(=|", "ᾗ");
            ins = ins.Replace("H)/|", "ᾔ");
            ins = ins.Replace("H)\\|", "ᾒ");
            ins = ins.Replace("H(/|", "ᾕ");
            ins = ins.Replace("H(\\|", "ᾓ");
            ins = ins.Replace("H)|", "ᾐ");
            ins = ins.Replace("H(|", "ᾑ");
            ins = ins.Replace("H)=", "ἦ");
            ins = ins.Replace("H(=", "ἧ");
            ins = ins.Replace("H)/", "ἤ");
            ins = ins.Replace("H)\\", "ἢ");
            ins = ins.Replace("H(/", "ἥ");
            ins = ins.Replace("H(\\", "ἣ");
            ins = ins.Replace("H\\|", "ῂ");
            ins = ins.Replace("H=|", "ῇ");
            ins = ins.Replace("H/|", "ῄ");
            ins = ins.Replace("H/", "ή");
            ins = ins.Replace("H\\", "ὴ");
            ins = ins.Replace("H)", "ἠ");
            ins = ins.Replace("H(", "ἡ");
            ins = ins.Replace("H|", "ῃ");
            ins = ins.Replace("H=", "ῆ");

            ins = ins.Replace("I)=", "ἶ");
            ins = ins.Replace("I(=", "ἷ");
            ins = ins.Replace("I)/", "ἴ");
            ins = ins.Replace("I)\\", "ἲ");
            ins = ins.Replace("I(/", "ἵ");
            ins = ins.Replace("I(\\", "ἳ");
            ins = ins.Replace("I\\+", "ῒ");
            ins = ins.Replace("I/+", "ΐ");
            ins = ins.Replace("I=+", "ῗ");
            ins = ins.Replace("I/", "ί");
            ins = ins.Replace("I\\", "ὶ");
            ins = ins.Replace("I)", "ἰ");
            ins = ins.Replace("I(", "ἱ");
            ins = ins.Replace("I=", "ῖ");
            ins = ins.Replace("I+", "ϊ");

            ins = ins.Replace("O)/", "ὄ");
            ins = ins.Replace("O)\\", "ὂ");
            ins = ins.Replace("O(/", "ὅ");
            ins = ins.Replace("O(\\", "ὃ");
            ins = ins.Replace("O/", "ό");
            ins = ins.Replace("O\\", "ὸ");
            ins = ins.Replace("O)", "ὀ");
            ins = ins.Replace("O(", "ὁ");

            ins = ins.Replace("U(=", "ὗ");
            ins = ins.Replace("U(/", "ὕ");
            ins = ins.Replace("U(\\", "ὓ");
            ins = ins.Replace("U)\\", "ὒ");
            ins = ins.Replace("U)/", "ὔ");
            ins = ins.Replace("U)=", "ὖ");
            ins = ins.Replace("U\\+", "ῢ");
            ins = ins.Replace("U/+", "ΰ");
            ins = ins.Replace("U=+", "ῧ");
            ins = ins.Replace("U/", "ύ");
            ins = ins.Replace("U\\", "ὺ");
            ins = ins.Replace("U(", "ὑ");
            ins = ins.Replace("U)", "ὐ");
            ins = ins.Replace("U=", "ῦ");
            ins = ins.Replace("U+", "ϋ");

            ins = ins.Replace("W)=|", "ᾦ");
            ins = ins.Replace("W(=|", "ᾧ");
            ins = ins.Replace("W)/|", "ᾢ");
            ins = ins.Replace("W)\\|", "ᾤ");
            ins = ins.Replace("W(/|", "ᾥ");
            ins = ins.Replace("W(\\|", "ᾣ");
            ins = ins.Replace("W)|", "ᾠ");
            ins = ins.Replace("W(|", "ᾡ");
            ins = ins.Replace("W=|", "ῷ");
            ins = ins.Replace("W\\|", "ῲ");
            ins = ins.Replace("W/|", "ῴ");
            ins = ins.Replace("W)=", "ὦ");
            ins = ins.Replace("W(=", "ὧ");
            ins = ins.Replace("W)/", "ὢ");
            ins = ins.Replace("W)\\", "ὤ");
            ins = ins.Replace("W(/", "ὥ");
            ins = ins.Replace("W(\\", "ὣ");
            ins = ins.Replace("W/", "ώ");
            ins = ins.Replace("W\\", "ὼ");
            ins = ins.Replace("W)", "ὠ");
            ins = ins.Replace("W(", "ὡ");
            ins = ins.Replace("W|", "ῳ");
            ins = ins.Replace("W=", "ῶ");


            ins = ins.Replace("*A", "Α");
            ins = ins.Replace("*B", "Β");
            ins = ins.Replace("*G", "Γ");
            ins = ins.Replace("*D", "Δ");
            ins = ins.Replace("*E", "Ε");
            ins = ins.Replace("*Z", "Ζ");
            ins = ins.Replace("*Η", "H");
            ins = ins.Replace("*Q", "Θ");
            ins = ins.Replace("*I", "Ι");
            ins = ins.Replace("*K", "Κ");
            ins = ins.Replace("*L", "Λ");
            ins = ins.Replace("*M", "Μ");
            ins = ins.Replace("*N", "Ν");
            ins = ins.Replace("*C", "Ξ");
            ins = ins.Replace("*O", "Ο");
            ins = ins.Replace("*P", "Π");
            ins = ins.Replace("*R", "Ρ");
            ins = ins.Replace("*S", "Σ");
            ins = ins.Replace("*T", "Τ");
            ins = ins.Replace("*U", "Υ");
            ins = ins.Replace("*F", "Φ");
            ins = ins.Replace("*X", "Χ");
            ins = ins.Replace("*Y", "Ψ");
            ins = ins.Replace("*W", "Ω");

            ins = ins.Replace("*(R", "Ῥ");
            ins = ins.Replace("R)", "ῤ");
            ins = ins.Replace("R(", "ῥ");

            ins = ins.Replace("A", "α");
            ins = ins.Replace("B", "β");
            ins = ins.Replace("G", "γ");
            ins = ins.Replace("D", "δ");
            ins = ins.Replace("E", "ε");
            ins = ins.Replace("Z", "ζ");
            ins = ins.Replace("H", "η");
            ins = ins.Replace("Q", "θ");
            ins = ins.Replace("I", "ι");
            ins = ins.Replace("K", "κ");
            ins = ins.Replace("L", "λ");
            ins = ins.Replace("M", "μ");
            ins = ins.Replace("N", "ν");
            ins = ins.Replace("C", "ξ");
            ins = ins.Replace("O", "ο");
            ins = ins.Replace("P", "π");
            ins = ins.Replace("R", "ρ");
            ins = ins.Replace("S", "σ");
            ins = ins.Replace("T", "τ");
            ins = ins.Replace("U", "υ");
            ins = ins.Replace("F", "φ");
            ins = ins.Replace("X", "χ");
            ins = ins.Replace("Y", "ψ");
            ins = ins.Replace("W", "ω");



            // απομάκρυνση άλλων στοιχείων
            ins = ins.Replace("<20", "");
            ins = ins.Replace(">20", "");
            ins = ins.Replace("€", "");
            ins = ins.Replace("σ ", "ς ");
            ins = ins.Replace("σ.", "ς.");
            ins = ins.Replace("σ,", "ς,");
            ins = ins.Replace("σ;", "ς;");
            ins = ins.Replace("σ:", "ς:");
            ins = ins.Replace("σ!", "ς!");
            ins = ins.Replace("σ)", "ς)");
            ins = ins.Replace("σ-", "ς-");
            ins = ins.Replace("σ]", "ς]");
            ins = ins.Replace("σ'", "ς'");
            ins = ins.Replace(":", "·");
            ins = ins.Replace("@", "\n");
            ins = ins.Replace("#", "");
            ins = ins.Replace("$", "");
            ins = ins.Replace("%", "");
            ins = ins.Replace("^", "");
            ins = ins.Replace("&", "");
            ins = ins.Replace("¶", "");
            ins = ins.Replace("±", "");
            ins = ins.Replace("²", "");
            ins = ins.Replace("‰", "");
            ins = ins.Replace("‡", "");
            ins = ins.Replace("", "");
            ins = ins.Replace("", "");
            ins = ins.Replace("", "");
            ins = ins.Replace("[2", "[");
            ins = ins.Replace("]2", "]");
            ins = ins.Replace("™", "");
            ins = ins.Replace("-", "");
            ins = ins.Replace("{", "");
            ins = ins.Replace("}", "");
            //ins = ins.Replace("","");
            return ins.Trim() + " ";
        }
        private string Normalize(string aString)
        {
            // the a string with the text and then using the replace
            // adds space between the before and after a sympol as:
            /*aString = aString.Replace("\n", " ");
            aString = aString.Replace("\r", " ");
            aString = aString.Replace("\t", " ");*/ 
            aString = aString.Replace("\\", " \\ ");
            aString = aString.Replace("/", " / ");
            aString = aString.Replace("(", "( ");
            aString = aString.Replace(")", " )");
            aString = aString.Replace("{", " { ");
            aString = aString.Replace("}", " {");
            aString = aString.Replace("[", " [ ");
            aString = aString.Replace("]", " ] ");
            aString = aString.Replace("*", " * ");
            aString = aString.Replace("#", " # ");
            aString = aString.Replace("&", " & ");
            aString = aString.Replace("-", "");
            aString = aString.Replace("_", "");
            aString = aString.Replace(",", " , ");
            aString = aString.Replace(":", " : ");
            aString = aString.Replace(";", " ; ");
            aString = aString.Replace("...", "... ");

            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } // Αλγόριθμος Κανονικοποίησης.
        private string toSmallLetters(string aString)
        {
            aString = aString.Replace("Β", "β");
            aString = aString.Replace("Γ", "γ");
            aString = aString.Replace("Δ", "δ");
            aString = aString.Replace("Ζ", "ζ");
            aString = aString.Replace("Θ", "θ");
            aString = aString.Replace("Κ", "κ");
            aString = aString.Replace("Λ", "λ");
            aString = aString.Replace("Μ", "μ");
            aString = aString.Replace("Ν", "ν");
            aString = aString.Replace("Ξ", "ξ");
            aString = aString.Replace("Ο", "ο");
            aString = aString.Replace("Ο", "ό");
            aString = aString.Replace("Π", "π");
            aString = aString.Replace("Ρ", "ρ");
            aString = aString.Replace("Σ ", "ς ");
            aString = aString.Replace("Σ", "σ");
            aString = aString.Replace("Τ", "τ");
            aString = aString.Replace("Φ", "φ");
            aString = aString.Replace("Ψ", "ψ");
            aString = aString.Replace("Χ", "x");

            aString = aString.Replace("Α", "α");
            aString = aString.Replace("Ά", "ά");
            aString = aString.Replace("Η", "η");
            aString = aString.Replace("Ή", "ή");
            aString = aString.Replace("Ι", "ι");
            aString = aString.Replace("Ί", "ί");
            aString = aString.Replace("Υ", "υ");
            aString = aString.Replace("Ύ", "ύ");
            aString = aString.Replace("Ε", "ε");
            aString = aString.Replace("Έ", "έ");
            aString = aString.Replace("Ω", "ω");
            aString = aString.Replace("Ώ", "ώ");

            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } //Μετατροπή σε μικρά γράμματα.
        private string MakeList(string aString)
        {
            // the a string with the text and then using the replace
            // adds space between the before and after a sympol as:
            /* aString = aString.Replace("\\", "");
             aString = aString.Replace("/", "");
             aString = aString.Replace("(", "");
             aString = aString.Replace(")", "");
             aString = aString.Replace("{", "");
             aString = aString.Replace("}", "");
             aString = aString.Replace("[", "");
             aString = aString.Replace("]", "");
             aString = aString.Replace("*", "");
             aString = aString.Replace("#", "");
             aString = aString.Replace("&", "");
             aString = aString.Replace("-", "");
             aString = aString.Replace("_", "");
             aString = aString.Replace(",", "");
             aString = aString.Replace(":", "");
             aString = aString.Replace(";", "");
             aString = aString.Replace("...", "");
             aString = aString.Replace(".", ""); */
            aString = aString.Replace("   ", "  ");
            aString = aString.Replace("  ", " ");
            aString = aString.Replace(" ", "\r");
            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } //Δημιουργία Λίστας.


        private string Clusters(string aString)
        {
            aString = aString.Replace("a", "a\t");
            aString = aString.Replace("b", "b\t");
            aString = aString.Replace("c", "c\t");
            aString = aString.Replace("ç", "ç\t");
            aString = aString.Replace("d", "d\t");
            aString = aString.Replace("ð", "ð\t");
            aString = aString.Replace("e", "e\t");
            aString = aString.Replace("f", "f\t");
            aString = aString.Replace("g", "g\t");
            aString = aString.Replace("ɣ", "ɣ\t");
            aString = aString.Replace("i", "i\t");
            aString = aString.Replace("j", "j\t");
            aString = aString.Replace("Ɉ", "Ɉ\t");
            aString = aString.Replace("ʝ", "ʝ\t");
            aString = aString.Replace("k", "k\t");
            aString = aString.Replace("l", "l\t");
            aString = aString.Replace("m", "m\t");
            aString = aString.Replace("n", "n\t");
            aString = aString.Replace("ɲ", "ɲ\t");
            aString = aString.Replace("ŋ", "ŋ\t");
            aString = aString.Replace("o", "o\t");
            aString = aString.Replace("p", "p\t");
            aString = aString.Replace("r", "r\t");
            aString = aString.Replace("s", "s\t");
            aString = aString.Replace("t", "t\t");
            aString = aString.Replace("u", "u\t");
            aString = aString.Replace("v", "v\t");
            aString = aString.Replace("x", "x\t");
            aString = aString.Replace("ʎ", "ʎ\t");
            aString = aString.Replace("z", "z\t");
            aString = aString.Replace("θ", "θ\t");
            aString = aString.Replace("ú", "ú\t");
            aString = aString.Replace("í", "í\t");
            aString = aString.Replace("é", "é\t");
            aString = aString.Replace("á", "á\t");
            aString = aString.Replace("ó", "ó\t");

            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } 






        private string ContextTras(string aString)
        {
            aString = aString.Replace(",", "");
            aString = aString.Replace(".", " | ");
            aString = aString.Replace("n k", " ɉ");
            aString = aString.Replace("n p", " b");
            aString = aString.Replace("n d", " d");
            aString = aString.Replace("s v", "z v");
            aString = aString.Replace("s ɣ", "z ɣ");
            aString = aString.Replace("s ð", "z ð");
            aString = aString.Replace("s m", "z m");
            aString = aString.Replace("s n", "z n");
            aString = aString.Replace("s d", "z d");
            aString = aString.Replace("s b", "z b");
            aString = aString.Replace("s g", "z g");
            aString = aString.Replace("s ɉ", "z ɉ");


            //aString = aString.Replace("n g", " ɳ");

            while (aString.IndexOf("  ") >= 0)
                aString = aString.Replace("  ", " ");
            return aString.Trim() + " ";
        } //Αλγόριθμος μετατροπής σε ΔΦΑ για συμπροφορά μεταξύ λέξεων.
        /// <summary>
        /// Γεγονότα: 
        /// Κλείσιμο Φόρμας
        /// Δεξί κλικ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        /// <summary>
        /// Ακολουθούν οι οδηγίες σχετικά με τη λειτουργία του μενού.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Checks the value of the text.
            if (rtxText.Text.Length == 0)
            {
                rtxText.Clear();
            }
            if (rtxText.Text.Length != 0)
            {

                // Initializes the variables to pass to the MessageBox.Show method.

                string message = "Θέλετε να αποθηκεύσετε το υπάρχον αρχείο;";
                string caption = "Αποθήκευση τρέχουσας εργασίας";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.No)
                {
                    rtxText.Clear();
                }
                if (result == DialogResult.Yes)
                {
                    Save();
                    // Closes the parent form.
                    rtxText.Clear();
                }
            }
        }
        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            About();
        }
        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Checks the value of the text.
           
            if (rtxText.Text.Length != 0)
            {

                // Initializes the variables to pass to the MessageBox.Show method.

                string message = "Θέλετε να αποθηκεύσετε το αρχείο;";
                string caption = "Αποθήκευση Αρχείου";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.No)
                {
                    rtxText.Clear();
                }
                if (result == DialogResult.Yes)
                {
                    Save();
                    rtxText.Clear();
                    // Closes the parent form.
                    Open();
                }
        }
        if (rtxText.Text.Length == 0)
        {
            Open();
        }
        }
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Save();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            // Checks the value of the text.
            if (rtxText.Text.Length == 0)
            {
                rtxText.Clear();
            }
            if (rtxText.Text.Length != 0)
            {

                // Initializes the variables to pass to the MessageBox.Show method.

                string message = "Θέλετε να αποθηκεύσετε το υπάρχον αρχείο;";
                string caption = "Αποθήκευση τρέχουσας εργασίας";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.No)
                {
                    rtxText.Clear();
                }
                if (result == DialogResult.Yes)
                {
                    Save();
                    // Closes the parent form.
                    rtxText.Clear();
                }
            }
        }
        private void γραμματοσειράToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;

            fontDialog1.Font = rtxText.Font;
            fontDialog1.Color = rtxText.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                rtxText.Font = fontDialog1.Font;
                rtxText.ForeColor = fontDialog1.Color;
            }
        }
        private void cutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {  
            // Ensure that text is currently selected in the text box. 
            if (rtxText.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard. 
                rtxText.Cut();
        }
        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box. 
            if (rtxText.SelectionLength > 0)
                // Copy the selected text to the Clipboard. 
                rtxText.Copy();
        }
        private void pasteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box. 
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                // Determine if any text is selected in the text box. if(rtxText.SelectionLength > 0) 
                {
                    // Ask user if they want to paste over currently selected text. 
                    //if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                    // Move selection to the point after the current selection and paste. 
                    rtxText.SelectionStart = rtxText.SelectionStart + rtxText.SelectionLength;
                }
                // Paste current text in Clipboard into text box. 
                rtxText.Paste();
            }
        }
        private void selectAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (rtxText.CanSelect == true)
                rtxText.SelectAll();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rtxText.CanUndo == true)
                rtxText.Undo();
        }
        private void λεξικήΜετατροπήToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toGRIPA(normText);
            rtxText.Text = ipaText;
            ipaText = "";
        }
        private void redoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (rtxText.CanRedo == true)
                rtxText.Redo();
        }
        private void μετατροπήΚειμένουToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = ContextTras(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
        }
        private void κενούΔιαστήματοςΤέλοςΚειμένουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = TrimEndText(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";  
        }
        private void κενούΔιαστήματοςΤέλοςΛέξεωνToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = DeleteSpace(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";  
        }
        private void δημιουργίαΛίσταςToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = MakeList(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";
        }
        private void ημερομηνίαςΚαιΏραςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pos = rtxText.SelectionStart;
            rtxText.Text = rtxText.Text.Insert(pos, DateTime.Now.ToString());
        }
        private void κεφαλαίαToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToUpper(ipaText);
        }
        private void μικράToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            string holdThis;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toSmallLetters(normText);
            holdThis = ipaText.ToLower();
            rtxText.Text = holdThis;
            ipaText = ""; 
        }
        private void γράμματαΤίτλουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToTitleCase(ipaText);
        }
        private void διαγραφήΌλωνToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display a message box asking users if they
            // want to delete the text.
            if (MessageBox.Show("Πρόκειται να διαγράψεις ολόκληρο το κείμενο. Θέλεις να συνεχίσεις;", " ",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
            {
                rtxText.Text="";
            } 
        }
        private void αντιστροφήToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = Reverser(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";
        }
        private void προστασίαΚειμένουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.ReadOnly = προστασίαΚειμένουToolStripMenuItem.Checked;
        }
        private void υπολογιστικήToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\windows\system32\calc.exe");
        }
        private void ηχογράφησηToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\windows\system32\SoundRecorder.exe");
        }
        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }
        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusBarToolStripMenuItem.Checked)
                statusStrip.Visible = true;
            else
                statusStrip.Visible = false;
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFileDialog1.Filter = "Απλό Κείμενο (*.txt)|*.txt|Μορφή Εμπλουτισμένου Κειμένου (*.rtf)|*.rtf|Όλα τα Αρχεία (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Αποθήκευση ως";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFileDialog1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                if (saveFileDialog1.FilterIndex == 2)
                    rtxText.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                else
                    rtxText.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);

            }
        }
        private void στοίχισηΚειμένουΔεξιάToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (στοίχισηΚειμένουΔεξιάToolStripMenuItem.Checked)
                rtxText.RightToLeft = RightToLeft.Yes;
            else
                rtxText.RightToLeft = RightToLeft.No;
        }
        private void πάνταΟρατόToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = πάνταΟρατόToolStripMenuItem.Checked;
        }
        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.WordWrap = aToolStripMenuItem.Checked;
        }
        /// <summary>
        /// Οδηγίες για την μπάρα εργασιών
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            // Checks the value of the text.

            if (rtxText.Text.Length != 0)
            {

                // Initializes the variables to pass to the MessageBox.Show method.

                string message = "Θέλετε να αποθηκεύσετε το αρχείο;";
                string caption = "Αποθήκευση Αρχείου";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.No)
                {
                    rtxText.Clear();
                }
                if (result == DialogResult.Yes)
                {
                    Save();
                    rtxText.Clear();
                    // Closes the parent form.
                    Open();
                }
            }
            if (rtxText.Text.Length == 0)
            {
                Open();
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }
        private void χρώμαΓραμματοσειράςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.ShowDialog();
            rtxText.ForeColor = colorDialog1.Color;
        }
        private void χρώμαΦόντουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.ShowDialog();
            rtxText.BackColor = colorDialog1.Color;
        }
        private void printSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }
        private void σύμβολοToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\windows\system32\charmap.exe");
        }
        /// <summary>
        /// δεξί κλίκ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void αντιγραφήToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box. 
            if (rtxText.SelectionLength > 0)
                // Copy the selected text to the Clipboard. 
                rtxText.Copy();
        }
        private void αποκοπήToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is currently selected in the text box. 
            if (rtxText.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard. 
                rtxText.Cut();
        }
        private void επικόλλησηToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box. 
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                // Determine if any text is selected in the text box. if(rtxText.SelectionLength > 0) 
                {
                    // Ask user if they want to paste over currently selected text. 
                    //if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                    // Move selection to the point after the current selection and paste. 
                    rtxText.SelectionStart = rtxText.SelectionStart + rtxText.SelectionLength;
                }
                // Paste current text in Clipboard into text box. 
                rtxText.Paste();
            }
        }
        private void λέξειςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toGRIPA(normText);
            rtxText.Text = ipaText;
            ipaText = "";
        }
        private void κανόνεςΣυμπροφοράςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = ContextTras(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
        }
        private void μικράToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            string holdThis;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toSmallLetters(normText);
            holdThis = ipaText.ToLower();
            rtxText.Text = holdThis;
            ipaText = ""; 
        }
        private void κεφαλαίαΓράμματαToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToUpper(ipaText);
        }
        private void γράμματαΤίτλουToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToTitleCase(ipaText);
        }
        private void γραμματοσειράToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;

            fontDialog1.Font = rtxText.Font;
            fontDialog1.Color = rtxText.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                rtxText.Font = fontDialog1.Font;
                rtxText.ForeColor = fontDialog1.Color;
            }
        }
        private void χρώμαΓραμματοσειράςToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.ShowDialog();
            rtxText.ForeColor = colorDialog1.Color;
        }
        private void αποθήκευσηToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void rtxText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                contextMenuStrip1.Show(this, e.Location);

            }
        }
        private void νέαΠραγμάτωσηToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 a = new Form1();
            a.Show();
            a.Text = "Αλλόμορφο";
        }
        /// <summary>
        /// μπάρα εργασιών
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripΑλλόμορφο_Click(object sender, EventArgs e)
        {
            Form1 a = new Form1();
            a.Show();
            a.Text = "Αλλόμορφο";
        }
        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }
        private void ΑποκοπήtoolStripButton1_Click(object sender, EventArgs e)
        {
            // Ensure that text is currently selected in the text box. 
            if (rtxText.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard. 
                rtxText.Cut();
        }
        private void αντιγραφήtoolStripButton2_Click(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box. 
            if (rtxText.SelectionLength > 0)
                // Copy the selected text to the Clipboard. 
                rtxText.Copy();
        }
        private void επικόλλησηtoolStripButton3_Click(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box. 
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                // Determine if any text is selected in the text box. if(rtxText.SelectionLength > 0) 
                {
                    // Ask user if they want to paste over currently selected text. 
                    //if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                    // Move selection to the point after the current selection and paste. 
                    rtxText.SelectionStart = rtxText.SelectionStart + rtxText.SelectionLength;
                }
                // Paste current text in Clipboard into text box. 
                rtxText.Paste();
            }
        }
        private void γραμματοσειρέςtoolStripButton17_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;

            fontDialog1.Font = rtxText.Font;
            fontDialog1.Color = rtxText.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                rtxText.Font = fontDialog1.Font;
                rtxText.ForeColor = fontDialog1.Color;
            }
        }
        private void χρώμαΓtoolStripButton14_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.ShowDialog();
            rtxText.ForeColor = colorDialog1.Color;
        }
        private void χρώμαΦtoolStripButton16_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.ShowDialog();
            rtxText.BackColor = colorDialog1.Color;
        }
        private void ώραtoolStripButton12_Click(object sender, EventArgs e)
        {
            //int pos = rtxText.SelectionStart;
            //rtxText.Text = rtxText.Text.Insert(pos, DateTime.Now.ToString());
            Ημερολόγιο a = new Ημερολόγιο();
            a.ShowDialog();
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            string holdThis;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toSmallLetters(normText);
            holdThis = ipaText.ToLower();
            rtxText.Text = holdThis;
            ipaText = ""; 
        } //μικρά
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToUpper(ipaText);
        } //κεφαλαία
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = rtxText.Text;
            TextInfo myText = new CultureInfo("el", false).TextInfo;
            rtxText.Text = myText.ToTitleCase(ipaText);
        } //γράμματα τίτλου
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string normText;
            string ipaText;
            normText = Normalize(rtxText.Text);
            rtxText.Clear();
            ipaText = toGRIPA(normText);
            rtxText.Text = ipaText;
            ipaText = "";
        } // ΔΦΑ(αθήνα)
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = ContextTras(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
        }// ΔΦΑ (αθήνα)-κείμενο
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = MakeList(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";
        }//λίστα
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = Reverser(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";
        }//αντιστροφή
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\windows\system32\SoundRecorder.exe");
        } //ηχογράφηση
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\windows\system32\charmap.exe");
        }
        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Initializes the variables to pass to the MessageBox.Show method.
            if (rtxText.Text.Length == 0)
            {Application.Exit();}
            else
            {
                string message = "Θέλετε να αποθηκεύσετε το αρχείο;";
                string caption = "Έξοδος από την εφαρμογή";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    Save();
                }
                if (result == DialogResult.No)
                {
                }
            }
        }
        private void στατιστικάToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistica();
        }
        private void αλλαγήςΠαραγράφουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.Text = RemoveParagraphs(rtxText.Text);
        }
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Ημερολόγιο a = new Ημερολόγιο();
            a.ShowDialog();
        }
        private void διεθνέςΦωνητικόΑλφάβητοToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.arts.gla.ac.uk/ipa/ipachart.html");	
            
        }
        private void εύρεσηToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FindDialog findDialog = new FindDialog(rtxText, false);
            findDialog.Show();
        }
        private void εύρεσηΕπομένουToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDialog.FindNext(rtxText);
        }
        private void επιλογήΚαιΕύρεσηToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDialog.CurrentSearchString =  rtxText.SelectedText;
            FindDialog.FindNext(rtxText);
        }
        private void αντικατάστασηToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FindDialog findDialog = new FindDialog(rtxText, true);
            findDialog.Show();
        }
        private void αριστεράToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.SelectionAlignment = HorizontalAlignment.Left;
        }
        private void δεξιάToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.SelectionAlignment = HorizontalAlignment.Right;
        }
        private void κέντροToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void αύξησηΕσδοχήςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.SelectionIndent = 8 + rtxText.SelectionIndent;
            rtxText.SelectionHangingIndent = 3;
            rtxText.SelectionRightIndent = 12;
        }
        private void μείωσηΕσδοχήςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxText.SelectionIndent = rtxText.SelectionIndent - 8;
            rtxText.SelectionHangingIndent = 3;
            rtxText.SelectionRightIndent = 12;
        }
        private void εικόναςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Εισαγωγή αρχείου εικόνας";
            openDialog.DefaultExt = "rtf";
            openDialog.Filter = "Αρχεία Bitmap|*.bmp|Εικόνα JPEG|*.jpg|Εικόνα GIF|*.gif";
            openDialog.FilterIndex = 1;
            openDialog.ShowDialog();
            if (openDialog.FileName == "")
            {
                return;
            }
            try
            {
                string strImagePath = openDialog.FileName;
                Image img;
                img = Image.FromFile(strImagePath);
                Clipboard.SetDataObject(img);
                DataFormats.Format df;
                df = DataFormats.GetFormat(DataFormats.Bitmap);
                if (this.rtxText.CanPaste(df))
                {
                    this.rtxText.Paste(df);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Δεν είναι δυνατό να εισαχθεί η εικόνα", "Επικόλληση"+ex.ToString(), MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void διάβασμαToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpVoice voice = new SpVoice();
            voice.Speak(rtxText.Text, SpeechVoiceSpeakFlags.SVSFDefault);
        }

        private void χαρακτήρεςToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaText;
            ipaText = Clusters(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = ipaText;
            ipaText = "";
        }

        private void greekΣεUnicodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string unicodetxt;
            unicodetxt = GreektoUnicode(rtxText.Text);
            rtxText.Clear();
            rtxText.Text = unicodetxt;
            unicodetxt = "";

        }

        
        }
        }