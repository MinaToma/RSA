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

            return new BigInteger(addHelper(clone(value), clone(secondNumber.value)));
        }

        public BigInteger Sub(BigInteger secondNumber)
        {
            if (value[0] == '-' && secondNumber.value[0] == '-')
            {
                var posValue = Clone();
                posValue.value.RemoveAt(0);

                var secPosValue = secondNumber.Clone();
                secPosValue.value.RemoveAt(0);

                var res = new BigInteger(addHelper(posValue.value, secPosValue.value));
                res.value.Insert(0, '-');

                return res;
            }

            if (value[0] == '-')
            {
                var posValue = Clone();
                posValue.value.RemoveAt(0);

                var res = new BigInteger(addHelper(clone(secondNumber.value), posValue.value));

                res.value.Insert(0, '-');

                return res;
            }

            if (secondNumber.value[0] == '-')
            {
                var posValue = secondNumber.Clone();
                posValue.value.RemoveAt(0);

                return new BigInteger(addHelper(clone(value), posValue.value));
            }

            return new BigInteger(subHelper(clone(value), clone(secondNumber.value)));
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

            BigInteger res = new BigInteger(mulHelper(posValue.value, secPosValue.value));

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
            return new BigInteger(powerModHelper(clone(value), clone(power.value), clone(mod.value)));
        }

        public BigInteger Mod(BigInteger mod)
        {
            return new BigInteger(divHelper(clone(value), clone(mod.value)).remainder);
        }

        public BigInteger Clone()
        {
            return new BigInteger(clone(value));
        }

        public bool IsNeg()
        {
            return value[0] == '-';
        }

        public int CompareTo(BigInteger secondNumber)
        {
            return isSmaller(value, secondNumber.value);
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
                if (millerTest(d, this) == false)
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

        public static BigInteger operator %(BigInteger number, BigInteger mod)
        {
            return number.Mod(mod);
        }

        private List<char> powerModHelper(List<char> number, List<char> power, List<char> mod)
        {
            var res = clone(_one);
            number = divHelper(number, clone(mod)).remainder;

            while (isSmaller(clone(_zero), power) == -1)
            {
                if ((power[power.Count - 1] - '0') % 2 == 1)
                {
                    res = mulHelper(res, number);
                    res = divHelper(res, clone(mod)).remainder;
                    power[power.Count - 1]--;
                }

                power = divideByTwo(power);
                number = mulHelper(clone(number), clone(number));
                number = divHelper(number, clone(mod)).remainder;
            }

            return res;
        }

        public List<char> divideByTwo(List<char> number)
        {
            int idx = 0;
            List<char> res = new List<char>();
            int curNum = 0;

            while (idx < number.Count)
            {
                curNum *= 10;
                curNum += (int)(number[idx++] - '0');
                if (curNum >= 2)
                {
                    res.AddRange((curNum / 2).ToString());
                    curNum %= 2;
                }
                else if (curNum == 0)
                {
                    res.Add('0');
                }
                else if (res.Count > 0 && curNum < 2)
                {
                    res.Add('0');
                }
            }

            if (res.Count == 0)
            {
                res.Add('0');
            }

            return res;
        }

        private DivisionResult divHelper(List<char> firstList, List<char> secondList)
        {
            if (isSmaller(firstList, secondList) == -1)
            {
                return new DivisionResult(clone(_zero), new List<char>(firstList.ToArray()));
            }

            var res = divHelper(firstList, addHelper(clone(secondList), clone(secondList)));
            res.quotient = addHelper(clone(res.quotient), clone(res.quotient));

            if (isSmaller(res.remainder, secondList) != -1)
            {
                res.quotient = addHelper(res.quotient, clone(_one));
                res.remainder = subHelper(res.remainder, secondList);
                return res;
            }

            return res;
        }

        private List<char> addHelper(List<char> firstList, List<char> secondList)
        {
            reverse(firstList);
            reverse(secondList);
            makeEqualLengthRightAppend(firstList, secondList);

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
            {
                res.Add((char)(carry + '0'));
            }

            reverse(res);
            res = removeLeadingZeros(res);

            reverse(firstList);
            reverse(secondList);

            return res;

        }

        private List<char> mulHelper(List<char> firstList, List<char> secondList)
        {
            makeEqualLengthLeftAppend(firstList, secondList);

            if (firstList.Count == 0)
            {
                return clone(_zero);
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

            var left_left_result = mulHelper(firstNmberL, secondNumberL);
            var right_right_result = mulHelper(firstNumberH, secondNumberH);
            var leftF_rightF__leftS_rightS = mulHelper(addHelper(firstNmberL, firstNumberH), addHelper(secondNumberL, secondNumberH));

            leftF_rightF__leftS_rightS = subHelper(subHelper(leftF_rightF__leftS_rightS, left_left_result), right_right_result);

            appendZeros(leftF_rightF__leftS_rightS, highLen);
            leftF_rightF__leftS_rightS = addHelper(leftF_rightF__leftS_rightS, right_right_result);

            appendZeros(left_left_result, highLen * 2);

            return addHelper(left_left_result, leftF_rightF__leftS_rightS);
        }

        private List<char> subHelper(List<char> firstList, List<char> secondList)
        {
            var NegRes = false;
            if (isSmaller(firstList, secondList) == -1)
            {
                swap(ref firstList, ref secondList);
                NegRes = true;
            }

            reverse(firstList);
            reverse(secondList);
            makeEqualLengthRightAppend(firstList, secondList);

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

            reverse(res);
            res = removeLeadingZeros(res);

            if (NegRes)
                res.Insert(0, '-');

            reverse(firstList);
            reverse(secondList);

            return res;
        }

        private int isSmaller(List<char> firstList, List<char> secondList)
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

        private List<char> removeLeadingZeros(List<char> list)
        {
            var res = list.SkipWhile(c => c == '0').ToList();
            if (res.Count == 0)
            {
                res.Add('0');
            }
            return res;
        }

        private void appendZeros(List<char> list, int nunberOfZeros)
        {
            for (int i = 0; i < nunberOfZeros; i++)
            {
                list.Add('0');
            }
        }

        private void makeEqualLengthLeftAppend(List<char> firstList, List<char> secondList)
        {
            reverse(firstList);
            reverse(secondList);

            makeEqualLengthRightAppend(firstList, secondList);

            reverse(firstList);
            reverse(secondList);
        }

        private void makeEqualLengthRightAppend(List<char> firstList, List<char> secondList)
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

        private void reverse(List<char> list)
        {
            list.Reverse();
        }

        private void swap(ref List<char> secondList, ref List<char> firstList)
        {
            var temp = secondList;
            secondList = firstList;
            firstList = temp;
        }

        private List<char> clone(List<char> list)
        {
            return list.Select(s => s).ToList();
        }

        private bool millerTest(BigInteger d, BigInteger n)
        {
            Random r = new Random();

            BigInteger rand = new BigInteger((r.Next()).ToString());
            rand = rand.Mod(n.Sub(new BigInteger("4")));
            rand = rand.Add(new BigInteger("2"));

            var x = rand.PowerMod(d, n);
            var n_1 = n.Sub(new BigInteger("1"));
            if (x.ToString() == "1" || isSmaller(x.value, n_1.value) == 0)
                return true;

            while (isSmaller(d.value, n_1.value) != 0)
            {
                x = x.Mul(x);
                x = x.Mod(n);
                d = d.Mul(new BigInteger("2"));

                if (x.ToString() == "1")
                    return false;
                if (isSmaller(x.value, n_1.value) == 0)
                    return true;
            }
            return false;
        }
    }
}
