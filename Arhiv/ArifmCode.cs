using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.IO; 
using System.Threading.Tasks;
using RT.ArithmeticCoding;

namespace Arhiv
{
    public class ArifmCode
    {
        private uint[] freq;
        private ArithmeticSymbolArrayContext context;
        private ArithmeticCodingWriter encoder;
        private string FileName;
        private string alfavit;
        private List<int> potok;
        private struct Pair {
            public uint chastota;
            public int numb;
        }
        public ArifmCode(List<int> potok, string FileName, string alfavit)
        {
            this.potok = potok;
            this.alfavit = alfavit;
            freq = PodschetChastota();
            this.FileName = FileName;
            context = new ArithmeticSymbolArrayContext(freq);

        }
        public ArifmCode(string FileName)
        {
            this.FileName = FileName;
        }
        private uint[] PodschetChastota()
        {

            List<int> distinct = new List<int>(alfavit.Length);
            for (int i = 0; i < alfavit.Length; i++)
            {
                distinct.Add(i);
            }
            List<Pair> para = new List<Pair>();
            foreach (int i in distinct)
            {
                Pair st = new Pair(){numb = i ,chastota = Podchet(i) };
                para.Add(st);
            }
            return para.Select(n => n.chastota).ToArray();
        }

        public void WriteArhive()
        {

            using (FileStream Fstream = File.Create(FileName))
            {
                // Открываем поток в файл и записываем MTF и алфавит
                //Alfavit to file
                ChangeForStreamStr();
                byte[] arr = Encoding.Default.GetBytes(alfavit + (char)0x03);
                List<byte[]> listchas = new List<byte[]>(freq.Length);
                foreach (uint fre in freq) {

                    listchas.Add(Encoding.Default.GetBytes(fre.ToString() + BWT.ETX));
                }
                Fstream.Write(arr, 0, arr.Length);
                foreach (byte[] ch in listchas)
                {
                    Fstream.Write(ch, 0, ch.Length);
                }
                // MTF to File
                encoder = new ArithmeticCodingWriter(Fstream, context);
                foreach (int i in potok)
                {
                    encoder.WriteSymbol(i);
                }


                encoder.Finalize();
                Fstream.Flush();
                Fstream.Close();
            }
        }

        public List<int> LoadArhive(ref string  _alfavit)
        {
            List<int> filetext;
            using (FileStream Fstream = File.OpenRead(FileName))
            {

                _alfavit = ReadAlfavit(Fstream);
                var freq = ChastotasFile(Fstream, _alfavit.Length);
                var _context = new ArithmeticSymbolArrayContext(freq);
                
                    
                var decoder = new ArithmeticCodingReader(Fstream, _context);
                int pospotok = Sum(freq);
                filetext = new List<int>(pospotok);
                for (int i = 0; i < pospotok; i++)
                {
                    filetext.Add(decoder.ReadSymbol());
                }
                decoder.Finalize();
                Fstream.Close();

            }
            return filetext;
            //decoder.Finalize();
        }
        private uint[] ChastotasFile(FileStream fileStream,int length)
        {
            List<int> list = new List<int>(length);
            for (int i = 0; i < length; i++)
            {
                List<string> fint = new List<string>(3);
                while (true)
                {
                    byte[] buffer = new byte[1];

                    fileStream.Read(buffer, 0, 1);
                    if (buffer[0] == 0x03)
                        break;
                    fint.Add(Encoding.Default.GetString(buffer));
                    buffer = null;
                }
                
                bool isNum = int.TryParse(MyFunctions.MyJoin(fint), out int Num);
                if (isNum)
                    list.Add(Num);
                else
                    throw new ArhivException("Файл не возможно декодировать");
                fint.Clear();
            }
            return list.Select(n => (uint)n).ToArray();

        }
        private string ReadAlfavit(FileStream fileStream)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            List<string> alf = new List<string>(3);
            while (true)
            {

                byte[] buffer = new byte[1];

                fileStream.Read(buffer, 0, 1);
                if (buffer[0] == 0x03)
                    break;
                alf.Add(Encoding.Default.GetString(buffer));
           

            }
            return CreateAlfavit(alf);
        }
       
        private int Sum(uint[] s)
        {
            int sum = 0;
            foreach (int i in s)
            {
                sum += i;
            }
            return sum;
        }
        private void ChangeForStreamStr()
        {
            List<string> alf = new List<string>(alfavit.Length - 2);
            char[] newalf = alfavit.ToCharArray();
            foreach (char c in newalf)
            {
                if (c!=BWT.ETX && c !=BWT.STX)
                    alf.Add(c.ToString());
            }    
            alfavit = MyFunctions.MyJoin(alf);

        }

        private string CreateAlfavit(List<string> alf)
        {
            alf.Add(BWT.ETX.ToString());
            alf.Add(BWT.STX.ToString());
            alf.Sort(BWT.Compare);

            return MyFunctions.MyJoin(alf);
        }
        private uint Podchet(int seek)
            {
                uint count = 0;
                foreach(int i in potok)
                {
                    if (i == seek)
                        count++;
                }
            return count;
        }
        
    }
}
