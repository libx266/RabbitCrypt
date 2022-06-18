using RabbitCrypt.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Extensions
{
    public static class DataHelper
    {

        public static string ToBase256(this byte[] data) => StringData.ToBase256(data);

        public static byte[] FromBase256(this string data) => StringData.FromBase256(data);

        public static string ToBase64(this byte[] data) => StringData.ToBase64(data);

        public static byte[] FromBase64(this string data) => StringData.FromBase64(data);

        public static string ToHex(this byte[] data) => StringData.ToHex(data);
    }
}
