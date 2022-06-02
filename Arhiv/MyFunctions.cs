using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arhiv
{
    public class MyFunctions
    {
        public static string MyJoin(List<string> str)
        {
            return String.Join("", str.Select(n => n.ToString()).ToArray());
        }

        // Не пригодилась, не хочу удалять :( , хотя если немного поправить, код с BWT 
        // то он будет нужен, а пока пусть будет пустым
        public static string[] MySplit(string str)
        {
            bool flag = false;
            str = str.Replace("\r", "");
            int count = 0;
            char[] vs = str.ToCharArray();
            List<string> list = new List<string>(10);
            for (int i = 0, j = 0; i < vs.Length; i++, j++)
            {
                string substr = null;
                if (vs[i] == '\n')
                {
                    substr = new string(vs).Substring(i - j, j + 1);
                    j = -1;
                    flag = false;
                }

                if (vs[i] == ' ')
                {
                    count++;
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        if (count != new string(vs).Substring(i - j, j).Length)
                        {
                            substr = new string(vs).Substring(i - j, j);
                            j = 0;
                            count = 0;
                        }

                        flag = false;
                    }

                }
                if (substr != null)
                {
                    list.Add(substr);
                }


            }

            return list.ToArray();
        }
    }
}
