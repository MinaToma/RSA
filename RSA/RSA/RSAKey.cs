using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class RSAKey
    {
        public BigInteger privateKey { get; }
        public BigInteger publicKey { get; }
        public BigInteger mod { get; }

        public RSAKey(BigInteger _privateKey, BigInteger _publicKey, BigInteger _mod)
        {
            mod = _mod;
            privateKey = _privateKey;
            publicKey = _publicKey;
        }

        override
        public string ToString()
        {
            return privateKey.ToString() + " " + publicKey.ToString() + " " + mod.ToString();
        }
    }
}
