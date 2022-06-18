using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{
    internal sealed class Numeral
    {
        private string[] Dictionary;
        private ushort Base;
        internal Numeral(string dt)
        {
            this.Dictionary = dt.Split(',');
            this.Base = Convert.ToUInt16(Dictionary.Length);
        }
        internal string Encode(long value)
        {
            var CharIndex = new UInt16[8];
            var result = new StringBuilder();
            UInt16 i = 0;
            while (value > 0)
            {
                CharIndex[i] = Convert.ToUInt16(value % this.Base);
                value /= this.Base; i++;
            }
            for (UInt16 j = i; j < 8; j++) CharIndex[j] = 0; 
            
            Array.Reverse(CharIndex);
            
            for (UInt16 q = 0; q < 7; q++)
                result.Append(this.Dictionary[CharIndex[q]] + ","); 
            
            return result.Append(this.Dictionary[CharIndex[7]]).ToString();
        }
        internal long Decode(string text)
        {
            string[] numbers = text.Split(',');
            Array.Reverse(numbers);
            UInt16 Index = 0; Int64 result = 0;
            UInt16 ValueIndex = 0; UInt16 RankIndex = 0;
            foreach (string CurrentNumber in numbers)
            {
                foreach (string CurrentItem in this.Dictionary)
                {
                    if (CurrentNumber == CurrentItem)
                    { ValueIndex = Index; break; }
                    Index++;
                }
                result += Convert.ToInt64(ValueIndex * dMath.Pow(this.Base, RankIndex));
                RankIndex++; Index = 0; ValueIndex = 0;
            }
            return result / this.Base;
        }
        internal string Enumerate(long value) =>
            this.Dictionary[value];
        internal long Denumerate(string value)
        {
            long result = 0;
            for (int i = 0; i < this.Dictionary.Length; i++)
            {
                if (value == this.Dictionary[i])
                { result = Convert.ToInt64(i); break; }
            }
            return result;
        }
    }
}
