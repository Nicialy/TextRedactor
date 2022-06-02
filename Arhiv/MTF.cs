using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace Arhiv
{
    
    public class MTF
    {
        public static List<int> MoveToFront(string str, string alfavit)
        {
            List<int> result = new List<int>();
            List<char> listalf = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                int index = alfavit.IndexOf(str[i]);
                if (index != -1)
                {
                    result.Add(index);
                    // Сдвигаем символ в начало
                    char temp = alfavit[index];
                    listalf.Add(temp);
                    for (int j = 0; j < alfavit.Length; j++)
                    {
                        if (j!=index)
                            listalf.Add(alfavit[j]);
                    }
                    alfavit = new string(listalf.ToArray());
                    listalf.Clear();
                    
                }

            }
            return result;
        }
        public static string MoveToFrontDecode(List<int> numb, string alfavit)
        {
            List<string> result = new List<string>();
            List<char> listalf = new List<char>();
            foreach (int i in numb)
            {
                result.Add(alfavit[i].ToString());
                listalf.Add(alfavit[i]);
                for (int j = 0; j < alfavit.Length; j++)
                {
                    if (j != i)
                        listalf.Add(alfavit[j]);
                }
                alfavit = new string(listalf.ToArray());
                listalf.Clear();


            }
            return String.Join("",result.Select(n => n.ToString()).ToArray());
        }

        
    }
}
