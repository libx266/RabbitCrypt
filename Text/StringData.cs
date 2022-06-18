using RabbitCrypt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Text
{
    internal static class StringData
    {
        /// <summary>
        /// Смещение кодировки для строкового кодирования байтов
        /// </summary>
        const ushort Alphabet = Const.P;

        /// <summary>
        /// Кодирует байт на основе сдвига кодировки
        /// </summary>
        /// <param name="q">байт</param>
        /// <returns>Символ алфавита, порядковый номер которого отвечает значению байта</returns>
        private static char EncodeByte(byte q) =>
            Convert.ToChar(q + Alphabet);

        /// <summary>
        /// Вычисляет байт на основе обратного сдвига кодировки
        /// </summary>
        /// <param name="q">Символ алфавита, порядковый номер которого отвечает значению байта</param>
        /// <returns>байт</returns>
        private static byte DecodeByte(char q) =>
            Convert.ToByte(q - Alphabet);

        /// <summary>
        /// Преобразует данные в систему счисления с основанием 256
        /// </summary>
        /// <param name="data">данные для преобразования</param>
        /// <returns>Поток байтов в строковом представлении по основанию 256</returns>
        internal static string ToBase256(byte[] data) =>
            String.Join("", data.Select(b => EncodeByte(b)));

        /// <summary>
        /// Преобразует строковое представление байтов в исходнй вид
        /// </summary>
        /// <param name="text">Данные для преобразования</param>
        /// <returns>Массив байтов</returns>
        internal static byte[] FromBase256(string text) =>
            text.Select(c => DecodeByte(c)).ToArray();

        internal static string ToBase64(byte[] data) =>
            Convert.ToBase64String(data);

        internal static byte[] FromBase64(string data) =>
            Convert.FromBase64String(data);

        internal static string ToHex(byte[] data)
        {
            var result = new StringBuilder();
            foreach (byte b in data)
            {
                string s = Convert.ToString(b, 16);
                if (s.Length < 2) s = "0" + s;
                result.Append(s);
            }
            return "0x" + result.ToString().ToUpper();
        }
    }
}
