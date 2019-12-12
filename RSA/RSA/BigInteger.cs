using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class BigInteger
    {
        public List<char> value;
        private List<char> _two = new List<char>("2");
        private List<char> _zero = new List<char>("0");
        private List<char> _one = new List<char>("1");

        public override string ToString()
        {
            return new string(value.ToArray());
        }

        public BigInteger(string val)
        {
            value = val.ToList();
        }

        public BigInteger(List<char> val)
        {
            value = val;
        }

        public BigInteger Add(BigInteger secondNumber)
        {
            if (secondNumber.value[0] == '-')
            {
                return Sub(secondNumber);
            }

            if (value[0] == '-')
            {
                var posValue = Clone();
                posValue.value.RemoveAt(0);

                return secondNumber.Sub(posValue);
            }

            return new BigInteger(AddHelper(Clone(value), Clone(secondNumber.value)));
        }

        public BigInteger Sub(BigInteger secondNumber)
        {
            if (value[0] == '-' && secondNumber.value[0] == '-')
            {
                var posValue = Clone();
                posValue.value.RemoveAt(0);

                var secPosValue = secondNumber.Clone();
                secPosValue.value.RemoveAt(0);

                var res = new BigInteger(AddHelper(posValue.value, secPosValue.value));
                res.value.Insert(0, '-');

                return res;
            }

            if (value[0] == '-')
            {
                var posValue = Clone();
                posValue.value.RemoveAt(0);

                var res = new BigInteger(AddHelper(Clone(secondNumber.value), posValue.value));

                res.value.Insert(0, '-');

                return res;
            }

            if (secondNumber.value[0] == '-')
            {
                var posValue = secondNumber.Clone();
                posValue.value.RemoveAt(0);

                return new BigInteger(AddHelper(Clone(value), posValue.value));
            }

            return new BigInteger(SubHelper(Clone(value), Clone(secondNumber.value)));
        }

        public BigInteger Mul(BigInteger secondNumber)
        {
            bool sign = false;

            if ((secondNumber.value[0] == '-' && value[0] != '-') || (secondNumber.value[0] != '-' && value[0] == '-'))
            {
                sign = true;
            }

            var posValue = Clone();
            var secPosValue = secondNumber.Clone();

            if (posValue.value[0] == '-')
            {
                posValue.value.RemoveAt(0);
            }

            if (secPosValue.value[0] == '-')
            {
                secPosValue.value.RemoveAt(0);
            }

            BigInteger res = new BigInteger(MulHelper(posValue.value, secPosValue.value));

            if (sign)
            {
                res.value.Insert(0, '-');
            }

            return res;
        }

        public BigInteger Div(BigInteger secondNumber)
        {
            bool sign = false;

            if ((secondNumber.value[0] == '-' && value[0] != '-') || (secondNumber.value[0] != '-' && value[0] == '-'))
            {
                sign = true;
            }

            var posValue = Clone();
            var secPosValue = secondNumber.Clone();

            if (posValue.value[0] == '-')
            {
                posValue.value.RemoveAt(0);
            }

            if (secPosValue.value[0] == '-')
            {
                secPosValue.value.RemoveAt(0);
            }

            var res = new BigInteger(divHelper(posValue.value, secPosValue.value).quotient);

            if (sign)
            {
                res.value.Insert(0, '-');
            }

            return res;
        }

        public BigInteger PowerMod(BigInteger power, BigInteger mod)
        {
            return new BigInteger(PowerModHelper(Clone(value), Clone(power.value), Clone(mod.value)));
        }

        public BigInteger Mod(BigInteger mod)
        {
            return new BigInteger(divHelper(Clone(value), Clone(mod.value)).remainder);
        }

        public BigInteger Clone()
        {
            return new BigInteger(Clone(value));
        }

        public bool IsNeg()
        {
            return value[0] == '-';
        }

        public int CompareTo(BigInteger secondNumber)
        {
            return IsSmaller(value, secondNumber.value);
        }

        public bool IsPrime()
        {
            var one = new BigInteger("1");
            var two = new BigInteger("2");
            int k = 4;

            if (this.ToString() == "1" || this.ToString() == "0" || this.ToString() == "4")
                return false;
            if (this.ToString() == "2" || this.ToString() == "3")
                return true;

            var d = this.Sub(one);

            while (d.Mod(two).ToString() == "0")
            {
                d = d.Div(two);
            }

            for (int i = 0; i < k; i++)
            {
                if (MillerTest(d, this) == false)
                    return false;
            }

            return true;
        }

        public static BigInteger operator +(BigInteger firstValue, BigInteger secondValue)
        {
            return firstValue.Add(secondValue);
        }

        public static BigInteger operator -(BigInteger firstValue, BigInteger secondValue)
        {
            return firstValue.Sub(secondValue);
        }

        public static BigInteger operator *(BigInteger firstValue, BigInteger secondValue)
        {
            return firstValue.Mul(secondValue);
        }

        public static BigInteger operator /(BigInteger firstValue, BigInteger secondValue)
        {
            return firstValue.Div(secondValue);
        }

        private List<char> PowerModHelper(List<char> number, List<char> power, List<char> mod)
        {
            var res = Clone(_one);
            number = divHelper(number, mod).remainder;

            while (IsSmaller(Clone(_zero), power) == -1)
            {
                if ((power[power.Count - 1] - '0') % 2 == 1)
                {
                    res = MulHelper(res, number);
                    res = divHelper(res, Clone(mod)).remainder;
                }

                power = divHelper(power, Clone(_two)).quotient;
                number = MulHelper(Clone(number), Clone(number));
                number = divHelper(number, Clone(mod)).remainder;
            }

            return res;
        }

        private DivisionResult divHelper(List<char> firstList, List<char> secondList)
        {
            if (IsSmaller(firstList, secondList) == -1)
            {
                return new DivisionResult(Clone(_zero), new List<char>(firstList.ToArray()));
            }

            var res = divHelper(firstList, MulHelper(secondList, Clone(_two)));
            res.quotient = MulHelper(res.quotient, Clone(_two));

            if (IsSmaller(res.remainder, secondList) != -1)
            {
                res.quotient = AddHelper(res.quotient, Clone(_one));
                res.remainder = SubHelper(res.remainder, secondList);
                return res;
            }

            return res;
        }

        private List<char> AddHelper(List<char> firstList, List<char> secondList)
        {
            Reverse(firstList);
            Reverse(secondList);
            MakeEqualLengthRightAppend(firstList, secondList);

            var carry = 0;
            var sum = 0;
            var len = firstList.Count;
            var res = new List<char>();

            for (int i = 0; i < len; i++)
            {
                sum = (firstList[i] - '0') + (secondList[i] - '0') + carry;
                res.Add((char)((sum % 10) + '0'));
                carry = sum / 10;
            }

            if (carry > 0)
                res.Add((char)(carry + '0'));

            Reverse(res);
            res = RemoveLeadingZeros(res);

            Reverse(firstList);
            Reverse(secondList);

            return res;

        }

        private List<char> MulHelper(List<char> firstList, List<char> secondList)
        {
            MakeEqualLengthLeftAppend(firstList, secondList);

            if (firstList.Count == 0)
            {
                return Clone(_zero);
            }

            if (firstList.Count == 1 || secondList.Count == 1)
            {
                return new List<char>(((firstList[0] - '0') * (secondList[0] - '0')).ToString());
            }

            var lowLen = firstList.Count / 2;
            var highLen = firstList.Count - lowLen;

            var firstNmberL = firstList.GetRange(0, lowLen);
            var firstNumberH = firstList.GetRange(lowLen, highLen);

            var secondNumberL = secondList.GetRange(0, lowLen);
            var secondNumberH = secondList.GetRange(lowLen, highLen);

            var left_left_result = MulHelper(firstNmberL, secondNumberL);
            var right_right_result = MulHelper(firstNumberH, secondNumberH);
            var leftF_rightF__leftS_rightS = MulHelper(AddHelper(firstNmberL, firstNumberH), AddHelper(secondNumberL, secondNumberH));

            leftF_rightF__leftS_rightS = SubHelper(SubHelper(leftF_rightF__leftS_rightS, left_left_result), right_right_result);

            AppendZeros(leftF_rightF__leftS_rightS, highLen);
            leftF_rightF__leftS_rightS = AddHelper(leftF_rightF__leftS_rightS, right_right_result);

            AppendZeros(left_left_result, highLen * 2);

            return AddHelper(left_left_result, leftF_rightF__leftS_rightS);
        }

        private List<char> SubHelper(List<char> firstList, List<char> secondList)
        {
            var NegRes = false;
            if (IsSmaller(firstList, secondList) == -1)
            {
                Swap(ref firstList, ref secondList);
                NegRes = true;
            }

            Reverse(firstList);
            Reverse(secondList);
            MakeEqualLengthRightAppend(firstList, secondList);

            var res = new List<char>();
            var borrow = 0;
            var len = firstList.Count;

            for (int i = 0; i < len; i++)
            {
                int diff = (firstList[i] - '0') - borrow - (secondList[i] - '0');

                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                res.Add((char)(diff + '0'));
            }

            Reverse(res);
            res = RemoveLeadingZeros(res);

            if (NegRes)
                res.Insert(0, '-');

            Reverse(firstList);
            Reverse(secondList);

            return res;
        }

        private int IsSmaller(List<char> firstList, List<char> secondList)
        {
            int len1 = firstList.Count, len2 = secondList.Count;
            if (len1 < len2)
            {
                return -1;
            }

            if (len2 < len1)
            {
                return 1;
            }

            for (int i = 0; i < len1; i++)
            {
                if (firstList[i] < secondList[i])
                {
                    return -1;
                }
                else if (firstList[i] > secondList[i])
                {
                    return 1;
                }
            }

            return 0;
        }

        private List<char> RemoveLeadingZeros(List<char> list)
        {
            var res = list.SkipWhile(c => c == '0').ToList();
            if (res.Count == 0)
            {
                res.Add('0');
            }
            return res;
        }

        private void AppendZeros(List<char> list, int nunberOfZeros)
        {
            for (int i = 0; i < nunberOfZeros; i++)
            {
                list.Add('0');
            }
        }

        private void MakeEqualLengthLeftAppend(List<char> firstList, List<char> secondList)
        {
            Reverse(firstList);
            Reverse(secondList);

            MakeEqualLengthRightAppend(firstList, secondList);

            Reverse(firstList);
            Reverse(secondList);
        }

        private void MakeEqualLengthRightAppend(List<char> firstList, List<char> secondList)
        {
            while (firstList.Count < secondList.Count)
            {
                firstList.Add('0');
            }

            while (firstList.Count > secondList.Count)
            {
                secondList.Add('0');
            }
        }

        private void Reverse(List<char> list)
        {
            list.Reverse();
        }

        private void Swap(ref List<char> secondList, ref List<char> firstList)
        {
            var temp = secondList;
            secondList = firstList;
            firstList = temp;
        }

        private List<char> Clone(List<char> list)
        {
            return list.Select(s => s).ToList();
        }

        private bool MillerTest(BigInteger d, BigInteger n)
        {
            Random r = new Random();

            BigInteger rand = new BigInteger((r.Next()).ToString());
            rand = rand.Mod(n.Sub(new BigInteger("4")));
            rand = rand.Add(new BigInteger("2"));

            var x = rand.PowerMod(d, n);
            var n_1 = n.Sub(new BigInteger("1"));
            if (x.ToString() == "1" || IsSmaller(x.value, n_1.value) == 0)
                return true;

            while (IsSmaller(d.value, n_1.value) != 0)
            {
                x = x.Mul(x);
                x = x.Mod(n);
                d = d.Mul(new BigInteger("2"));

                if (x.ToString() == "1")
                    return false;
                if (IsSmaller(x.value, n_1.value) == 0)
                    return true;
            }
            return false;
        }
    }
}
