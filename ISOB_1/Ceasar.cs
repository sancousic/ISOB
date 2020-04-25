using System;
using System.Collections.Generic;
using System.Text;

namespace ISOB_1
{
    public static class Ceasar
    {
        public static string Encrypt(string str, int k) 
        {
            StringBuilder res = new StringBuilder();

            foreach(var c in str)
            {
                if (char.IsUpper(c))
                {
                    res.Append((char)((c - 'A' + k) % 26 + 'A'));
                }
                else if (char.IsLower(c))
                {
                    res.Append((char)(((c - 'a' + k) % 26) + 'a'));
                }
                else
                    res.Append(c);
            }

            return res.ToString();
        }
        public static string Decrypt(string str, int k)
        {
            StringBuilder res = new StringBuilder();

            foreach(var c in str)
            {
                if (char.IsUpper(c))
                {
                    res.Append((char)((c - 'A' - k + 26) % 26 + 'A'));
                }
                else if (char.IsLower(c))
                {
                    res.Append((char)(((c - 'a' - k + 26) % 26) + 'a'));
                }
                else 
                    res.Append(c);
            }

            return res.ToString();
        }

    }
}
