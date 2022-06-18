using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{
    internal static class dMath
    {
        internal static Decimal Fact(long value)
        {
            decimal result = Decimal.One;

            for (long i = 0; i < value; i++)
                result *= (i + 1);

            return result;
        }


        internal static Decimal Pow(decimal value, long pow)
        {
            decimal _pow = value;

            var oper = new Action[]
            {
                () => value = value * _pow,
                () => value = value / _pow,
            }
            [Convert.ToInt32(pow < 0)];

            for (long n = 0; n < Math.Abs(pow); n++) oper();
            
            return value;
        }

        internal static Decimal Sin(decimal value, long accuracy = 16)
        {
            value *= (Const.PI / 180m);
            decimal result = value;
            
            for (long n = 0; n < accuracy; n++)
                result += ((Pow(-1, n) * Pow(value, 2 * n + 1))
                    / Fact(2 * n + 1));

            return result;

        }

        internal static Decimal Cos(decimal value, long accuracy = 16)
        {
            value *= (Const.PI / 180m);
            decimal result = Decimal.One;

            for (long n = 0; n < accuracy; n++)
                result += Pow(-1, n) * (Pow(value, 2 * n) / Fact(2 * n));

            return result;
        }

        internal static Decimal Requantize(decimal seed)
        {
            decimal a = seed % 53M;
            decimal b = seed % 157M;
            decimal c = seed % 173M;
            decimal d = seed % 211M;
            decimal e = seed % 257M;
            decimal f = seed % 263M;
            decimal g = seed % 373M;
            decimal h = seed % 563M;

            decimal r = Abs(Sin(a + b) / Cos(c + d) * e * f * g * h);
            long s = Convert.ToInt64(r); long i = 0;

            while (s > 0)
            {
                s /= 27;
                i++;
            }

            return r * Pow(27, 7 - i);
        }

        internal static Decimal Random(decimal seed)
        {
            decimal a = seed % 53M;
            decimal b = seed % 157M;
            decimal c = seed % 173M;
            decimal d = seed % 211M;
            decimal e = seed % 257M;
            decimal f = seed % 263M;
            decimal g = seed % 373M;
            decimal h = seed % 563M;
            decimal r = Const.DBZeF;
            return ((((a * b) / (d * c + r)) + ((e * f) / (g * h + r))) / (((a / (b + r)) * (g / (h + r))) - ((d / (c + r)) * (e / (f + r)) + r)));
        }

        internal static Decimal Gurple(int value, decimal key) //legacy
        {
            decimal result = (key % value) + (Decimal.One - (key / (Convert.ToUInt64(key) + Const.DBZeF))) * (value % 71);
            return result * (43 - result % 17);
        }

        internal static Decimal Low(decimal value, decimal max) //legacy
        {
            decimal abs = dMath.Abs(value);
            decimal mode = dMath.Round(abs / max + 1);
            value /= (mode + mode % 7); return value;
        }
        internal static Decimal High(decimal value, decimal min) //legacy
        {
            decimal mode = dMath.Round(min / value);
            value *= (mode + mode % 7); return value;
        }


        internal static Decimal Round(decimal value) => value - dMath.Fract(value);
        
        internal static Decimal Fract(decimal value) => value % 1;
        
        internal static Decimal Abs(decimal value) =>
            value >= 0 ? value : (Decimal.Zero - Decimal.One) * value;
    }
}
