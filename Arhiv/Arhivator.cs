using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
namespace Arhiv
{
    public class Arhivator : IArhivator
    {
        
        
        private TextRange _range;
        private BWT bwt;
        private ArifmCode arifmCode;
        private string alfavit;
        private string bwttext;
       
        //save file
        public Arhivator(RichTextBox rtb, string logFileName)
        {
            _range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            bwt = new BWT(_range);
            bwttext = bwt.Shifr();
            alfavit = Alfavit(bwttext);
            if (alfavit.Length >= _range.Text.Length)
                throw new ArhivException("Архивирование не имеет смысла");
            List<int> potok = MTF.MoveToFront(bwttext, alfavit);
            arifmCode = new ArifmCode(potok, logFileName,alfavit);
        }
        //Open fILE
        public Arhivator(string logFileName ,RichTextBox rtb)
        {
            _range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            arifmCode = new ArifmCode(logFileName);
        }
        public void Save()
        {
            arifmCode.WriteArhive();
            arifmCode = null;
        }
        public void Load()
        {


            List<int> l = arifmCode.LoadArhive(ref alfavit);
            string mtftext = MTF.MoveToFrontDecode(l,alfavit);
            bwt = new BWT(mtftext);
            string text = bwt.Decode();
            _range.Text = text;

        }
        private string Alfavit(string s)
        {
            
			var rich = s.Distinct().ToArray();
            List<string> str = new List<string>(20);
            for (int i = 0; i < rich.Length; i++)
                str.Add(rich[i].ToString());
			str.Sort(BWT.Compare);
            return new String(str.Select(a => a.Last()).ToArray());

        }


        
    }
}
