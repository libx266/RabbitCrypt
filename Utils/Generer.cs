using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{
    internal static class Generer //legacy
    {
        /// <summary>
        /// Создает последовательность кластеров указанной длины по указанному ключу
        /// </summary>
        /// <param name="length">Длина последовательности</param>
        /// <param name="key">Ключ последовательности</param>
        /// <returns></returns>
        internal static Int64[] CreateClusters(long length, decimal key)
        {
            decimal value = key;
            long[] result = new Int64[length];

            if (0 == 1)
            {
                for (long i = 0; i < length; i++)
                    result[i] = Convert.ToInt64(dMath.Abs(value = dMath.Random(value)));
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    value = value * dMath.Random(value);
                    if (dMath.Abs(value) < 173M)
                    { value = dMath.Gurple(i + 1, key + dMath.Fract(value)); }
                    if (dMath.Abs(value) < Const.MinValue)
                    {
                        value = dMath.High(value, Const.MinValue) *
                                  (Decimal.One + dMath.Fract(value));
                    }
                    if (dMath.Abs(value) > Const.MaxValue)
                    { value = dMath.Low(value, Const.MaxValue); }
                    result[i] = Convert.ToInt64(dMath.Abs(value));
                }
            }
            
            return result;
        }

        /// <summary>
        /// Генерирует связку ключей
        /// </summary>
        /// <param name="Bytes">данные</param>
        /// <returns></returns>
        private static decimal[] Hash(byte[] Bytes)
        {
            byte[] hash = SHA512.Create().ComputeHash(Bytes);
            string ByteCode = ""; UInt16 i = 0;
            foreach (byte item in hash)
            {
                string HexByte = Convert.ToString(item, 16).ToUpper();
                if (HexByte.Length < 2) HexByte = "0" + HexByte; 
               
                ByteCode += HexByte; i++;
                
                if (i % 8 == 0 & i / 8 < 8) ByteCode += "|"; 
            }
            string[] HexKeys = ByteCode.Split('|');
            decimal[] results = new decimal[8]; i = 0;
            foreach (string HexBytes in HexKeys)
            {
                ulong Key = Convert.ToUInt64(HexBytes, 16);
                results[i] = Convert.ToDecimal(Key / dMath.Pow(10, Convert.ToInt64((Key % 13) + 3)));
                i++;
            }
            return results;
        }


        private static byte[] S2Bytes(string text) => Encoding.UTF8.GetBytes(text);

        /// <summary>
        /// Генерирует связку ключей для графического энкодера на основе парольнорй фразы
        /// </summary>
        /// <param name="text">Парольная фраза</param>
        /// <returns></returns>
        internal static decimal[] GetKeysImg(string text) =>
            Hash(S2Bytes(text));

        private static byte[] Concat(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        internal static int GetSeed(decimal[] keys)
        {
            decimal fraction = Decimal.Zero;
            foreach (decimal key in keys)
            {
                long calko = Convert.ToInt64(key);
                
                if (calko > 0)
                    fraction += Decimal.One - key / calko;
                else fraction += key;
            }
            decimal egregor = dMath.Gurple(7, fraction);
            decimal abs = dMath.Abs(egregor);
            decimal mv = Convert.ToDecimal(Int32.MaxValue);
            if (abs < mv) return Convert.ToInt32(egregor);
            else return Convert.ToInt32(dMath.Low(egregor, mv));
        }

        private static int ToSeed(int value) =>
            value > 0 ? 0 - value : value;

        internal static int GetSeed(string text)
        {
            var Enc = SHA384.Create();
            var bs = Encoding.UTF8.GetBytes(text);
            var bh = Enc.ComputeHash(bs);
            return ToSeed(BitConverter.ToInt32(bh, 44));
        }

        
    }
}
