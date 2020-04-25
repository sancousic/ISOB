using System;
using System.Collections.Generic;
using System.Text;

namespace ISOB_1
{
    static class Vigenere
    {
        private static string ExtendKey(string key, int div)
        {
            StringBuilder extend_key = new StringBuilder(key);
            
            for (int i = 0; i < div; i++)
            {
                extend_key.Append(key[i % key.Length]);
            }
            
            return extend_key.ToString();
        }
        public static string Encrypt(string str, string key) 
        {
            StringBuilder res = new StringBuilder();

            int div = str.Length - key.Length;
            key = ExtendKey(key, div);
            
            
            for(int i = 0; i < str.Length; i++)
            {
                int a = 0;
                if (char.IsUpper(key[i]))
                    a = (int)('A');
                else if (char.IsLower(key[i]))
                    a = (int)('a');

                char c = str[i];
                if (char.IsUpper(c))
                    res.Append((char)((key[i] - a + c - 'A') % 26 + 'A'));
                else if (char.IsLower(c))
                    res.Append((char)((key[i] - a + c - 'a') % 26 + 'a'));
                else
                    res.Append(c);
            }            

            return res.ToString();
        }
        public static string Decrypt(string str, string key)
        {
            StringBuilder res = new StringBuilder();
            
            int div = str.Length - key.Length;
            key = ExtendKey(key, div);

            for (int i = 0; i < str.Length; i++)
            {
                int a = 0;
                if (char.IsUpper(key[i]))
                    a = (int)('A');
                else if (char.IsLower(key[i]))
                    a = (int)('a');

                char c = str[i];
                if (char.IsUpper(c))
                    res.Append((char)((-key[i] + a + str[i] - 'A' + 26) % 26 + 'A'));
                else if (char.IsLower(c))
                    res.Append((char)((-key[i] + a + str[i] - 'a' + 26) % 26 + 'a'));
                else
                    res.Append(c);
            }

            return res.ToString();
        }
    }
}
