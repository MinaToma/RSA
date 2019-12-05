using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class RSACore
    {
        private int _startValue = 10;
        private List<char> _number;
        private List<char> _power;
        private List<char> _mod;

        public RSACore(string number, string power, string mod)
        {
            _number = number.ToList();
            _power = power.ToList();
            _mod = mod.ToList();
        }

        public string encrypt()
        {
            return new string(BigInteger.PowerMod(_number, _power, _mod).ToArray());
        }

        public string decrypt(string E)
        {
            var e = E.ToList();
            return new string(BigInteger.PowerMod(_number, e, _mod).ToArray());
        }

        public string encryptString(string str, string E, string N)
        {
            var stringToNumber = convertStringToNumber(str);
            var e = E.ToList();
            var n = N.ToList();

            return new string(BigInteger.PowerMod(stringToNumber, e, n).ToArray());
        }

        public string decryptString(string M, string E, string N)
        {
            return convertNumberToString(encryptString(M, E, N));
        }


        private List<char> convertStringToNumber(string str)
        {
            List<char> number = new List<char>();
            number.AddRange(new string(str.SelectMany(c => (c - 'a' + _startValue).ToString()).ToArray()));
            return number;
        }

        private string convertNumberToString(string number)
        {
            return new string(Enumerable.Range(0, number.Length / 2)
                .Select(i => number.Substring(i * 2, 2))
                .Select(str => (char)(((str[0] - '0') * 10 + (str[1] - '0')) + 'a' - _startValue))
                .ToArray());
        }
    }
}
