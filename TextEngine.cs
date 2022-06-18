using RabbitCrypt.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt
{
    public static class TextEngine
    {
        /// <summary>
        /// текущий шифратор
        /// </summary>
        private readonly static Aes AES;

        /// <summary>
        /// Производит инициализацию шифратора
        /// </summary>
        static TextEngine()
        {
            AES = Aes.Create();
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CBC;
            AES.KeySize = 256;
            AES.BlockSize = 128;
        }

        private static string Encode(string text, string password)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[][] egregor = Hash.GetKeysAes(password);
                byte[] key = egregor[0], vector = egregor[1];

                var kurwa = new Random();
                var result = new StringBuilder();

                var MS = new MemoryStream();
                var CS = new CryptoStream(MS, AES.CreateEncryptor(key, vector), CryptoStreamMode.Write);

                CS.Write(data, 0, data.Length);
                CS.FlushFinalBlock(); CS.Close();
                byte[] enc = MS.ToArray(); MS.Close();

                for (int j = 0; j < enc.Length; j++)
                {
                    result.Append(Convert.ToString(enc[j], 8));
                    switch (kurwa.Next(0, 2))
                    {
                        case 0: result.Append("8"); break;
                        default: result.Append("9"); break;
                    }
                }
                string imp = "";

                switch (kurwa.Next(0, 2))
                {
                    case 0: imp = "8"; break;
                    default: imp = "9"; break;
                }

                return Convert.ToString(text.Length, 8) + imp + result.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }

        private static string Decode(string text, string password)
        {
            try
            {
                int length = 0;
                var data0 = new List<byte>();
                var item = ""; int x = 0;

                foreach (char q in text)
                {
                    if (q == '8' | q == '9')
                    {
                        if (x == 0) 
                        { 
                            length = Convert.ToInt32(item, 8);
                            data0 = new List<byte>(length); 
                            item = "";
                        }
                        else 
                        { 
                            data0.Add(Convert.ToByte(item, 8)); 
                            item = ""; 
                        }
                        x++;
                    }
                    else { item += q; }
                }
                var data1 = data0.ToArray();

                byte[][] egregor = Hash.GetKeysAes(password);
                byte[] key = egregor[0], vector = egregor[1];

                var MS = new MemoryStream(data1);
                var CS = new CryptoStream(MS, AES.CreateDecryptor(key, vector), CryptoStreamMode.Read);
                CS.Read(data1, 0, data1.Length); MS.Close(); CS.Close();

                return Encoding.UTF8.GetString(data1).Substring(0, length);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }


        /// <summary>
        /// Осуществляет шифрование информации
        /// </summary>
        /// <param name="text">Текст для шифрации</param>
        /// <param name="password">Парольная фраза</param>
        /// <returns>Поток байтов, передаваемых строкой в восьмеричной системе счисления</returns>
        public static async Task<string> EncodeAsync(string text, string password) =>
            await Task.Run(() => Encode(text, password));


        /// <summary>
        /// Осущесвтляет дешифрацию информации
        /// </summary>
        /// <param name="text">Поток байтов, передаваемых строкой в восьмеричной системе счисления</param>
        /// <param name="password">Парольная фраза</param>
        /// <returns>Исходные текстовые данные</returns>
        public static async Task<string> DecodeAsync(string text, string password) =>
            await Task.Run(() => Decode(text, password));
    }
}
