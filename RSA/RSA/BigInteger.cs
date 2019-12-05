using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    public class BigInteger
    {
        private static List<char> _two = new List<char>("2");
        private static List<char> _zero = new List<char>("0");

        public static string Add(string firstNumber, string secondNumber)
        {
            var firstList = firstNumber.ToList();
            var secondList = secondNumber.ToList();

            return new string(addHelper(firstList, secondList).ToArray());
        }

        public static string Sub(string firstNumber, string secondNumber)
        {
            var firstList = firstNumber.ToList();
            var secondList = secondNumber.ToList();

            return new string(subHelper(firstList, secondList).ToArray());
        }

        public static string Mul(string firstNumber, string secondNumber)
        {
            var firstList = firstNumber.ToList();
            var secondList = secondNumber.ToList();

            return new string(mulHelper(firstList, secondList).ToArray());
        }

        public static DivisionResult Div(string firstNumber, string secondNumber)
        {
            var firstList = firstNumber.ToList();
            var secondList = secondNumber.ToList();

            return divHelper(firstList, secondList);
        }

        public static List<char> PowerMod(List<char> number, List<char> power, List<char> mod)
        {
            var res = new List<char>("1");
            number = divHelper(number, mod).remainder;

            while (isSmaller(clone(_zero), power))
            {
                if ((power[power.Count - 1] - '0') % 2 == 1)
                {
                    res = mulHelper(res, number);
                    res = divHelper(res, clone(mod)).remainder;
                }

                power = divHelper(power, clone(_two)).quotient;
                number = mulHelper(clone(number), clone(number));
                number = divHelper(number, clone(mod)).remainder;
            }

            return res;
        }

        private static DivisionResult divHelper(List<char> firstList, List<char> secondList)
        {
            if (isSmaller(firstList, secondList))
            {
                return new DivisionResult(clone(_zero), new List<char>(firstList.ToArray()));
            }

            var res = divHelper(firstList, mulHelper(secondList, clone(_two)));
            res.quotient = mulHelper(res.quotient, clone(_two));

            if (!isSmaller(res.remainder, secondList))
            {
                res.quotient = addHelper(res.quotient, new List<char>("1"));
                res.remainder = subHelper(res.remainder, secondList);
                return res;
            }

            return res;
        }

        private static List<char> addHelper(List<char> firstList, List<char> secondList)
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
                res.Add((char)(carry + '0'));

            reverse(res);
            res = removeLeadingZeros(res);

            reverse(firstList);
            reverse(secondList);

            return res;

        }

        private static List<char> mulHelper(List<char> firstList, List<char> secondList)
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

        private static List<char> subHelper(List<char> firstList, List<char> secondList)
        {
            var NegRes = false;
            if (isSmaller(firstList, secondList))
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

        private static bool isSmaller(List<char> firstList, List<char> secondList)
        {
            int len1 = firstList.Count, len2 = secondList.Count;
            if (len1 < len2)
            {
                return true;
            }

            if (len2 < len1)
            {
                return false;
            }

            for (int i = 0; i < len1; i++)
            {
                if (firstList[i] < secondList[i])
                {
                    return true;
                }
                else if (firstList[i] > secondList[i])
                {
                    return false;
                }
            }

            return false;
        }

        private static List<char> removeLeadingZeros(List<char> list)
        {
            List<char> res = list.SkipWhile(c => c == '0').ToList();
            if (res.Count == 0)
            {
                res.Add('0');
            }
            return res;
        }

        private static void appendZeros(List<char> list, int nunberOfZeros)
        {
            for (int i = 0; i < nunberOfZeros; i++)
            {
                list.Add('0');
            }
        }

        private static void makeEqualLengthLeftAppend(List<char> firstList, List<char> secondList)
        {
            reverse(firstList);
            reverse(secondList);

            makeEqualLengthRightAppend(firstList, secondList);

            reverse(firstList);
            reverse(secondList);
        }

        private static void makeEqualLengthRightAppend(List<char> firstList, List<char> secondList)
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

        private static void reverse(List<char> list)
        {
            list.Reverse();
        }

        private static void swap(ref List<char> secondList, ref List<char> firstList)
        {
            var temp = secondList;
            secondList = firstList;
            firstList = temp;
        }

        private static List<char> clone(List<char> list)
        {
            return list.Select(s => s).ToList();
        }
    }
}
