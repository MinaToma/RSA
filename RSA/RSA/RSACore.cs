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
        private BigInteger _powerInverse;

        public RSACore(string number, string power, string mod)
        {
            _number = new BigInteger(number);
            _power = new BigInteger(power);
            _mod = new BigInteger(mod);

            _powerInverse = RSAKeyGenerator.GeneratePrivateKey(_power, _mod);
        }

        public RSACore(BigInteger number, BigInteger power, BigInteger mod)
        {
            _number = number;
            _power = power;
            _mod = mod;

            _powerInverse = RSAKeyGenerator.GeneratePrivateKey(_power, _mod);
        }

        public BigInteger encrypt()
        {
            return _number.PowerMod(_power, _mod);
        }

        public BigInteger decrypt()
        {
            return _number.PowerMod(_power, _mod).PowerMod(_powerInverse, _mod);
        }
    }
}
