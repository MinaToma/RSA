using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace RSA
{
    class Program
    {
        static void Test(string file, int choice)
        {
            FileStream fsQ = new FileStream(file, FileMode.OpenOrCreate);

            StreamReader sr = new StreamReader(fsQ);
            if (File.Exists("Answer" + file))
                File.Delete("Answer" + file);

            int N = int.Parse(sr.ReadLine());
            for (int i = 0; i < N; i++)
            {
                FileStream fsA = new FileStream("Answer" + file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fsA);
                sr.ReadLine();
                string firstNumber = sr.ReadLine();
                string secondNumber = sr.ReadLine();

                BigInteger a = new BigInteger(firstNumber);
                BigInteger b = new BigInteger(secondNumber);

                switch (choice)
                {
                    case 1:
                        sw.WriteLine(a.Add(b));
                        break;
                    case 2:
                        sw.WriteLine(a.Sub(b));
                        break;
                    case 3:
                        sw.WriteLine(a.Mul(b));
                        break;
                }

                sw.WriteLine();
                sw.Close();
                fsA.Close();
            }
            sr.Close();
            fsQ.Close();
        }

        static void Main(string[] args)
        {

            BigInteger m = new BigInteger("2003");
            BigInteger e = new BigInteger("7");
            BigInteger n = new BigInteger("3713");

            RSACore rsa = new RSACore( m, e, n);
            Console.WriteLine(rsa.encrypt());

            Console.WriteLine(rsa.decrypt());

            /*char cont;
            do
            {
                Console.WriteLine("(1) Add");
                Console.WriteLine("(2) Subtract");
                Console.WriteLine("(3) Multiply");

                Console.Write("Enter choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Test("AddTestCases.txt", choice);
                        break;
                    case 2:
                        Test("SubtractTestCases.txt", choice);
                        break;
                    case 3:
                        Test("MultiplyTestCases.txt", choice);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
                Console.Write("Do you want to calculate another? (Y/N): ");
                cont = char.Parse(Console.ReadLine());
            } while (cont == 'y' || cont == 'Y');
         */
        }
    }
}
