using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class AsciiKey
    {
        private static string _base = "256";
        private static string _one = "1";

        public static string GetDecimalString(string msg)
        {
            var ans = "0";
            var pow = "1";

            for (int i = msg.Length - 1; i >= 0; i--)
            {
                string num = converAsciiToDecimal(msg[i]);
                string mul = BigInteger.Mul(num, pow);
                ans = BigInteger.Add(ans, mul);
                pow = BigInteger.Mul(pow, _base);
                
            }
             
            return ans;

        }
        public static string GetAsciiString(string msg)
        {
            string ans = "";
            while(msg!="0")
            {
                var mod = BigInteger.PowerMod(msg.ToList(), _one.ToList(), _base.ToList());
                ans += (char)int.Parse(getStringFromList(mod));
                var newMsg = BigInteger.Div(msg, _base);
                msg = getStringFromList( newMsg.quotient);
            }

            string revAns = "";
            for(int j=ans.Length-1; j>=0;  j--)
            {
                revAns += ans[j];
            }
            return revAns;
        }

        private static string converAsciiToDecimal(char ch)
        {
           
            int number = (int)ch;
            return number.ToString();
        }
        private static string getStringFromList(List<char> list)
        {
            string ans = "";
            for (int i = 0; i < list.Count; i++)
                ans += list[i];
            return ans;
        }

        /*
            string mod = "2039896550861909479";
            int pow = 7;
            string d = "1165655170271755543"; 
         */
    }
}
