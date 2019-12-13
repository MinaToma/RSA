using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class RSACore
    {
        private BigInteger _number;
        private BigInteger _power;
        private BigInteger _mod;

        public RSACore(string number, string power, string mod)
        {
            _number = new BigInteger(number);
            _power = new BigInteger(power);
            _mod = new BigInteger(mod);
        }

        public RSACore(BigInteger number, BigInteger power, BigInteger mod)
        {
            _number = number;
            _power = power;
            _mod = mod;
        }

        public BigInteger Encrypt()
        {
            return _number.PowerMod(_power, _mod);
        }
    }
}
