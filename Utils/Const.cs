using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{
    internal static class Const
    {
        internal const int C = 5120; //U+1400 Compression alphabet range
        internal const int MSL0 = 33264; //Max String Length (default resolution)
        internal const int MSL1 = 544320; //Max String Length (high resolution)
        internal const int P = 40960; //U+A000 bytecode alphabet
        internal const decimal MinValue = 387420489M; //27^6
        internal const decimal MaxValue = 10460353203M - 114111M; //27^7 - U+10FFFF
        internal const decimal DBZeF = 0.0000000007876M; //DivisionByZero exception Fixer
        internal const string N27 = "aaa,aab,aac,aba,abb,abc,aca,acb,acc,baa,bab,bac,bba,bbb,bbc,bca,bcb,bcc,caa,cab,cac,cba,cbb,cbc,cca,ccb,ccc";
        internal const decimal PI = 3.14159265358979323846264338327950288m;
    }
}
