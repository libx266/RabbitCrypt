using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Bitmap
{
    internal static class Detecter //legacy
    {
        internal static string GetPixelCode(Image<Rgb24> orig, Image<Rgb24> compare, int thread)
        {
            int mode = orig.Height / 16;
            var result = new StringBuilder();
            int z = 1; bool b0 = true; string pc = "";
            for (int y = mode * thread; y < mode * (thread + 1); y++)
            {
                for (int x = 0; x < orig.Width; x++)
                {
                    Rgb24 o = orig[x, y];
                    Rgb24 c = compare[x, y];
                    string q = Comparer.Detect(o, c);
                    if (q != "")
                    {
                        pc += q;
                        if (z % 8 == 0)
                        {
                            pc += "|";
                            b0 = false;
                        }
                        if (z % 9 == 0)
                        {
                            pc += ";";
                            result.Append(pc);
                            pc = ""; z = 0;
                            b0 = false;
                        }
                        if (b0) pc += ",";
                    }
                    z++; b0 = true;
                }
            }
            return result.ToString();
        }
    }
}
