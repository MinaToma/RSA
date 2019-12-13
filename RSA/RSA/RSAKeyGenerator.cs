using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class RSAKeyGenerator
    {
        private BigInteger _zero = new BigInteger("0");
        private BigInteger _one = new BigInteger("1");
        private BigInteger _two = new BigInteger("2");
        private string numericalString = "0123456789";
        private int primeLength = 4;

        public RSAKey GenerateRSAKeys()
        {
            var firstPrime = getPrimeNumber(primeLength);
            var secondPrime = getPrimeNumber(primeLength);

            while (secondPrime.CompareTo(firstPrime) == 0)
            {
                secondPrime = getPrimeNumber(primeLength);
            }

            var mod = getMod(firstPrime, secondPrime);
            var phi = getPhi(firstPrime, secondPrime);
            var publicKey = getPublicKey(phi, primeLength - 1);
            var privateKey = GeneratePrivateKey(publicKey, firstPrime, secondPrime);

            return new RSAKey(privateKey, publicKey, mod);
        }

        private BigInteger getPrimeNumber(int length)
        {
            string number = "";
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    number += numericalString[r.Next(1, 9)];
                }
                else
                {
                    number += numericalString[r.Next(0, 9)];
                }
            }

            if ((number[length - 1] - '0') % 2 == 0)
            {
                char num = number[length - 1];
                number.Remove(length - 1);
                number += (num + 1);
            }

            BigInteger primeNumber = new BigInteger(number);

            while (!primeNumber.IsPrime())
            {
                primeNumber += _two;
            }

            if (primeNumber.value.Count > primeLength)
            {
                return getPrimeNumber(primeLength);
            }

            return primeNumber;
        }

        private BigInteger getPhi(BigInteger firstPrime, BigInteger secondPrime)
        {
            return (firstPrime - _one) * (secondPrime - _one);
        }

        private BigInteger getPublicKey(BigInteger phi, int length)
        {
            BigInteger publicKey = new BigInteger("0");
            Random r = new Random();

            var g = _zero.Clone();
            var _ = _zero.Clone();

            while (g.CompareTo(_one) != 0)
            {
                publicKey = getPrimeNumber(length);
                g = extendedGCD(phi, publicKey, ref _, ref _);
            }

            return publicKey;
        }

        private BigInteger getMod(BigInteger firstPrime, BigInteger secondPrime)
        {
            return firstPrime * secondPrime;
        }

        private BigInteger GeneratePrivateKey(BigInteger publicKey, BigInteger firstPrime, BigInteger secondPrime)
        {
            var x = new BigInteger("0");
            var y = new BigInteger("0");

            var phi = getPhi(firstPrime, secondPrime);

            extendedGCD(publicKey, phi, ref x, ref y);

            while (x.IsNeg())
            {
                x += phi;
            }

            return x;
        }

        private BigInteger extendedGCD(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            if (_zero.CompareTo(b) == 0)
            {
                x = new BigInteger("1");
                y = new BigInteger("0");
                return a;
            }

            var g = extendedGCD(b, a % b, ref y, ref x);
            y -= a / b * x;

            return g;
        }
    }
}
