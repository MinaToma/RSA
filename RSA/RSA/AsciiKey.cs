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
                ans = ans.Add(baseMul);
                pow = pow.Mul(_base);
            }
            return ans.ToString();
        }

        public static string GetAsciiString(string msg)
        {
            List<char> ans = new List<char>();
            var msgBigInteger = new BigInteger(msg);
            while (msgBigInteger.value.Count > 1 || msgBigInteger.ToString() != "0")
            {
                var mod = msgBigInteger.Mod(_base);
                ans.Add((char) int.Parse(mod.ToString()));
                msgBigInteger = msgBigInteger.Div(_base);
            }
            ans.Reverse();
            return new string (ans.ToArray());
        }

        private static string converAsciiToDecimal(char ch)
        {
            int number = (int)ch;
            return number.ToString();
        }
        private static List<int> convertStringToListInt(String str)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                list.Add((int)(str[i] - '0'));
            }
            return list;
        }

        private static string convertListIntToString(List<int> list)
        {
            return new string(list.Select(s => (char)(s + '0')).ToArray());
        }
    }
}
