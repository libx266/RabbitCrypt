using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Text
{
    /// <summary>
    /// Предоставляет набор операций по вычислению хэш-функций
    /// </summary>
    internal static class Hash
    {

        /// <summary>
        /// Вычисляет хэш-функцию на основе текстовых данных
        /// </summary>
        /// <param name="text">Текстовые данные</param>
        /// <param name="Alg">Алгоритм хэш-функции, по умолчанию SHA256</param>
        /// <returns></returns>
        private static byte[] Encode(string text, HashAlgorithm Alg = null)
        {
            HashAlgorithm Enc = Alg ?? SHA256.Create();
            return Enc.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Вычисляет хэш-функцию и возвращает строковое представление даных
        /// </summary>
        /// <param name="text">Поток байтов в строковом представлении по основанию 256</param>
        /// <returns></returns>
        internal static string Base256(string text) => StringData.ToBase256(Encode(text));

        /// <summary>
        /// Набирает данные для ключей алгоритма AES на основе данных вычисленной хэш-функции
        /// </summary>
        /// <param name="password">Данные для хэширования</param>
        /// <returns>Пара ключей</returns>
        internal static byte[][] GetKeysAes(string password)
        {
            byte[] hash = Encode(password, SHA384.Create());
            byte[] key = new byte[32], vector = new byte[16];
            for (int i = 0; i < 48; i++)
            {
                if (i < 32) { key[i] = hash[i]; }
                else { vector[i - 32] = hash[i]; }
            }
            return new byte[][] { key, vector };
        }
    }
}
