using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Bitmap
{
    internal static class Comparer //legacy
    {
        internal static Rgb24 Emission(Rgb24 original, string pixelCode)
        {
            if (pixelCode.Length != 3) return Color.Black; 
            byte[] rgb = { original.R, original.G, original.B };
            for (byte i = 0; i < 3; i++)
            {
                switch (pixelCode[i])
                {
                    case 'a': rgb[i]++; break;
                    case 'c': rgb[i]--; break;
                }
            }
            return new Rgb24(rgb[0], rgb[1], rgb[2]);
        }
        internal static Rgb24 Noise(Rgb24 original, Random q)
        {
            byte[] rgb = { original.R, original.G, original.B };
            for (byte i = 0; i < 3; i++)
            {
                switch (q.Next(0, 2))
                {
                    case 0: rgb[i]++; rgb[i]++; break;
                    case 1: rgb[i]--; rgb[i]--; break;
                }
            }
            return new Rgb24(rgb[0], rgb[1], rgb[2]);
        }
        internal static string Detect(Rgb24 original, Rgb24 comparable)
        {
            byte[] rgb0 = { original.R, original.G, original.B };
            byte[] rgb1 = { comparable.R, comparable.G, comparable.B };
            string pixelCode = "";
            for (byte i = 0; i < 3; i++)
            {
                switch (rgb1[i] - rgb0[i])
                {
                    case 1: pixelCode += "a"; break;
                    case 0: pixelCode += "b"; break;
                    case -1: pixelCode += "c"; break;
                }
            }
            return pixelCode;
        }
    }
}
