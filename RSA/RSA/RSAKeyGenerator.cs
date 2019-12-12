using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class RSAKeyGenerator
    {
        private static BigInteger _zero = new BigInteger("0");
        private static BigInteger _one = new BigInteger("1");
        private static BigInteger _two = new BigInteger("2");
        private static string numericalString = "0123456789";

        public BigInteger GetPrimeNumber(int length)
        {
            string number = "";
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                    number += numericalString[r.Next(1, 9)];
                else
                    number += numericalString[r.Next(0, 9)];
            }

            if ((number[length - 1] - '0') % 2 == 0)
            {
                char num = number[length - 1];
                number.Remove(length - 1);
                number += (num + 1);
            }

            BigInteger primeNumber = new BigInteger(number);

            while (!primeNumber.IsPrime())
                primeNumber += _two;

            return primeNumber;
        }

        public BigInteger GetPhi(BigInteger firstPrime, BigInteger secondPrime)
        {
            firstPrime -= _one;
            return firstPrime * (secondPrime - _one);
        }

        public BigInteger GetE(BigInteger phi, int length)
        {
            BigInteger e = new BigInteger("0");
            e = GetPrimeNumber(1);
            Random r = new Random();

            while (phi.Mod(e).CompareTo(_zero) == 1)
                e = GetPrimeNumber(r.Next(1, length - 1));

            return e;
        }

        public BigInteger GetN(BigInteger firstPrime, BigInteger secondPrime)
        {
            return firstPrime * secondPrime;
        }

        public BigInteger GetD(BigInteger phi, BigInteger e)
        {
            return e.PowerMod(phi - _two, phi);
        }

        public static BigInteger GeneratePrivateKey(BigInteger e, BigInteger mod)
        {
            var x = new BigInteger("0");
            var y = new BigInteger("0");

            var factors = GetPrimeFactors(mod);
            var phi = GetPhi(factors);

            ExtendedGCD(e, phi, ref x, ref y);
            while (x.IsNeg())
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
            y -= a / b * x;

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

                cur += _one;
            }

            return factors;
        }

        private static BigInteger GetPhi(List<BigInteger> factors)
        {
            var phi = new BigInteger("1");

            foreach (var bigInteger in factors)
            {
                phi *= (bigInteger - _one);
            }

            return phi;
        }
    }
}
