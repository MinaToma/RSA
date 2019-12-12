﻿using System;
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
            string ans = "";
            var msgBigInteger = new BigInteger(msg);
            while (msg != "0")
            {
                var mod = msgBigInteger.Mod(_base);
                ans += (char)int.Parse(mod.ToString());
                msgBigInteger = msgBigInteger.Div(_base);
            }
            return new string(ans.ToCharArray().Reverse().ToArray());
        }

        private static string converAsciiToDecimal(char ch)
        {
            int number = (int)ch;
            return number.ToString();
        }
    }
}
