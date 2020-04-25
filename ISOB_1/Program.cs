using System;
using System.IO;

namespace ISOB_1
{
    class Program
    {
        static string input_path = "input.txt";
        static string encrypt_ceasar = "encrypt_ceasar.txt";
        static string decrypt_ceasar = "decrypt_ceasar.txt";
        static string encrypt_vigenere = "encrypt_vigenere.txt";
        static string decrypt_vigenere = "decrypt_vigenere.txt";
        static string key = "LEMON";

        static void Main(string[] args)
        {
            try
            {
                string str;
                using (StreamReader sr = new StreamReader(input_path))
                {
                    str = sr.ReadToEnd();
                    using (StreamWriter sw = new StreamWriter(encrypt_ceasar))
                    {
                        sw.Write(Ceasar.Encrypt(str, 1));
                    }
                    using (StreamWriter sw = new StreamWriter(encrypt_vigenere))
                    {
                        sw.Write(Vigenere.Encrypt(str, key));
                    }
                }
                using (StreamReader sr = new StreamReader(encrypt_ceasar))
                {
                    using StreamWriter sw = new StreamWriter(decrypt_ceasar);
                    sw.Write(Ceasar.Decrypt(sr.ReadToEnd(), 1));
                }
                using (StreamReader sr = new StreamReader(encrypt_vigenere))
                {
                    using StreamWriter sw = new StreamWriter(decrypt_vigenere);
                    sw.Write(Vigenere.Decrypt(sr.ReadToEnd(), key));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
