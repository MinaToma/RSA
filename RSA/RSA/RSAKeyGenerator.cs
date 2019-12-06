using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class RSAKeyGenerator
    {
        private static BigInteger _zero = new BigInteger("0");
        private static BigInteger _one = new BigInteger("1");

        public static BigInteger GeneratePrivateKey(BigInteger e, BigInteger mod)
        {
            var x = new BigInteger("0");
            var y = new BigInteger("0");

            var factors = GetPrimeFactors(mod);
            var phi = GetPhi(factors);

            ExtendedGCD(e, phi, ref x, ref y);

            while (x.isNeg())
            {
                x = x.Add(phi);
            }

            return x;
        }

        private static BigInteger ExtendedGCD(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            if (_zero.CompareTo(b) == 0)
            {
                x = new BigInteger("1");
                y = new BigInteger("0");
                return a;
            }

            var g = ExtendedGCD(b, a.Mod(b), ref y, ref x);
            y = y.Sub(a.Div(b).Mul(x));

            return g;
        }

        private static List<BigInteger> GetPrimeFactors(BigInteger number)
        {
            var factors = new List<BigInteger>();
            var cur = new BigInteger("2");

            while (cur.CompareTo(number) == -1)
            {
                if (_zero.CompareTo(number.Mod(cur)) == 0)
                {
                    factors.Add(cur);
                }

                cur = cur.Add(_one);
            }

            return factors;
        }

        private static BigInteger GetPhi(List<BigInteger> factors)
        {
            var phi = new BigInteger("1");

            foreach (var bigInteger in factors)
            {
                phi = phi.Mul(bigInteger.Sub(_one));
            }

            return phi;
        }
    }
}
