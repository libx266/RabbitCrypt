using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{

    internal static class ArrayOperations //legacy
    {
        internal static string[] Secut(string text, UInt16 parts)
        {
            string[] result = new string[parts];
            var q0 = new StringBuilder();
            UInt16 q1 = 1;
            for (int i = 0; i < text.Length; i++)
            {
                q0.Append(text[i]);
                if (i == text.Length / parts * q1 & q1 < parts)
                { result[q1 - 1] = q0.ToString(); q0.Clear(); q1++; }
            }
            result[parts - 1] = q0.ToString(); return result;
        }
        internal static string Print(long[] array)
        {
            string result = "[";
            for (int i = 0; i < array.Length; i++)
            {
                if (i < array.Length - 1)
                { result += array[i].ToString() + " | "; }
                else { result += array[i].ToString(); }
            }
            result += "]"; return result;
        }
        internal static long[][] Secut(long[] array, UInt16 parts)
        {
            long[][] result = new long[parts][];
            var q0 = new List<long>(); UInt16 q1 = 1;
            for (int i = 0; i < array.Length; i++)
            {
                long item = array[i]; q0.Add(item);
                if (i == array.Length / parts * q1 & q1 < parts)
                { result[q1 - 1] = q0.ToArray(); q0.Clear(); q1++; }
            }
            result[parts - 1] = q0.ToArray(); return result;
        }
        internal static string[][] Secut(string[] array, UInt16 parts)
        {
            string[][] result = new string[parts][];
            List<string> q0 = new List<string>(); ; UInt16 q1 = 1;
            for (int i = 0; i < array.Length; i++)
            {
                string item = array[i]; q0.Add(item);
                if (i == array.Length / parts * q1 & q1 < parts)
                { result[q1 - 1] = q0.ToArray(); q0.Clear(); q1++; }
            }
            result[parts - 1] = q0.ToArray(); return result;
        }
    }
}
