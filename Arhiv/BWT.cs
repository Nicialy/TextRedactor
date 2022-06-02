using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Threading.Tasks;

namespace Arhiv
{
    public  class BWT
    {
        
        public const char STX = (char)0x02;
        public const char ETX = (char)0x03;
        
        
        private string text;
        private string dec;
        public BWT(TextRange text)
        {


            this.text = text.Text; //MTF.MySplit(text.Text);
       
        }
        public BWT(string dec)
        {
            this.dec = dec;
        }
       
        public  string Shifr()
        {
            List<string> rez = new List<string>();
           //foreach (string s in text)
          // {
                var temp = Bwtencode(text);
                if (temp != null)
                    rez.Add(temp);
          // }
            return temp; // MTF.MyJoin(rez); 
        }
        private static void Rotate(ref char[] a)
        {
            char t = a.Last();
            for (int i = a.Length - 1; i > 0; --i)
            {
                a[i] = a[i - 1];
            }
            a[0] = t;
        }
        
        public static int Compare(string s1, string s2)
        {
            for (int i = 0; i < s1.Length && i < s2.Length; ++i)
            {

                if (s1[i] < s2[i])
                {
                    return -1;
                }
                if (s2[i] < s1[i])
                {
                    return 1;
                }
            }
            if (s1.Length < s2.Length)
            {
                return -1;
            }
            if (s2.Length < s1.Length)
            {
                return 1;
            }
            return 0;
        }
        public static string Bwtencode(string s)
        {
            if (s.Any(a => a == STX || a == ETX))
            {
                throw new ArgumentException("Входные данные не могут содержать STX или ETX");
            }
            char[] ss = (STX + s + ETX).ToCharArray();
            List<string> table = new List<string>();
            for (int i = 0; i < ss.Length; ++i)
            {
                table.Add(new string(ss));
                Rotate(ref ss);
            }
            table.Sort(Compare);
            return new string(table.Select(a => a.Last()).ToArray());
        }
        public string Decode()
        {
            List<string> rez = new List<string>();
           // foreach (string s in dec)
           // {
                var temp = Bwtdecodes(dec);
                if (temp != null)
                    rez.Add(temp);

           // }
            return String.Join("", rez.ToArray());

        }
        private string Bwtdecodes(string r)
        {
            int len = r.Length;
            List<string> table = new List<string>(new string[len]);
            for (int i = 0; i < len; ++i)
            {
                for (int j = 0; j < len; ++j)
                {
                    table[j] = r[j] + table[j];
                }
                table.Sort(Compare);
            }
            foreach (string row in table)
            {
                if (row.Last() == ETX)
                {

                    return row.Substring(1, len - 2);
                }
            }
            return "";
        }
    }
   
}
