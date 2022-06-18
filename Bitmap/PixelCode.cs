using RabbitCrypt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Bitmap
{
    internal static class PixelCode
    {
        internal static string Emission(long[] clusters, string text, Random q)
        {
            var result = new StringBuilder();
            var Engine = new Numeral(Const.N27);
            for (int i = 0; i < clusters.Length; i++)
            {
                long impurity = Convert.ToInt64(q.Next(1, 27));
                long cluster = (clusters[i] + (long)text[i]) * impurity;
                result.Append(Engine.Encode(cluster) + "|" + Engine.Enumerate(impurity));
                if (i < clusters.Length - 1)
                { result.Append(";"); }
            }
            return result.ToString();
        }

        internal static string Detect(long[] clusters, string[] text)
        {
            var result = new StringBuilder();
            var Engine = new Numeral(Const.N27);
            for (int i = 0; i < clusters.Length; i++)
            {
                try
                {
                    string[] record = text[i].Split('|'); char symbol;
                    long impurity = Engine.Denumerate(record[1]);
                    long q = clusters[i];
                    long cluster = Engine.Decode(record[0]) / impurity - clusters[i];

                    if (cluster <= UInt16.MaxValue)
                        symbol = (char)cluster;
                    else symbol = '#';

                    result.Append(symbol);
                }
                catch { result.Append('#'); }
            }
            return result.ToString();
        }
    }
}
