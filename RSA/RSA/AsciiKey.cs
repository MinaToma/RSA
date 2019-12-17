using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class AsciiKey
    {
        private static BigInteger _base = new BigInteger("256");
        private static BigInteger _one = new BigInteger("1");

        public static string GetDecimalString(string msg)
        {
            var ans = new BigInteger("0");
            var pow = new BigInteger("1");
            for (int i = msg.Length - 1; i >= 0; i--)
            {
                var convertedNumber = new BigInteger(converAsciiToDecimal(msg[i]));
                var baseMul = convertedNumber.Mul(pow);
                ans.Add(baseMul);
                pow.Mul(_base);
            }
            return ans.ToString();
        }

        public static string GetAsciiString(string msg)
        {
            List<int> ans = new List<int>();
            var msgBigInteger = new BigInteger(msg);
            while (msg.Length>1 || msg != "0")
            {
                var mod = msgBigInteger.Mod(_base);
                ans.AddRange(mod.ToString().Select(s => int.Parse(s.ToString())).ToArray());
                msgBigInteger = msgBigInteger.Div(_base);
            }
            ans.Reverse();
            return new string(ans.Select(s => char.Parse(s.ToString())).ToArray());
        }

        private static string converAsciiToDecimal(char ch)
        {
            int number = (int)ch;
            return number.ToString();
        }
    }
}
