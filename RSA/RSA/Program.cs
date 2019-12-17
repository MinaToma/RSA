using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace RSA
{
    class Program
    {
        static void Milestone1()
        {
            char cont;
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
                        TestMilestone1("AddTestCases.txt", choice);
                        break;
                    case 2:
                        TestMilestone1("SubtractTestCases.txt", choice);
                        break;
                    case 3:
                        TestMilestone1("MultiplyTestCases.txt", choice);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
                Console.Write("Do you want to calculate another? (Y/N): ");
                cont = char.Parse(Console.ReadLine());
            } while (cont == 'y' || cont == 'Y');
        }
        static void TestMilestone1(string file, int choice)
        {
            FileStream fsQ = new FileStream("SampleRSA_I/" + file, FileMode.OpenOrCreate);

            StreamReader sr = new StreamReader(fsQ);
            if (File.Exists("SampleRSA_I/Answer" + file))
                File.Delete("SampleRSA_I/Answer" + file);

            int N = int.Parse(sr.ReadLine());
            int nStart = Environment.TickCount;
            for (int i = 0; i < N; i++)
            {
                FileStream fsA = new FileStream("SampleRSA_I/Answer" + file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fsA);
                sr.ReadLine();
                string firstNumber = sr.ReadLine();
                string secondNumber = sr.ReadLine();

                BigInteger a = new BigInteger(firstNumber);
                BigInteger b = new BigInteger(secondNumber);

                switch (choice)
                {
                    case 1:
                        sw.WriteLine(a + b);
                        break;
                    case 2:
                        sw.WriteLine(a - b);
                        break;
                    case 3:
                        sw.WriteLine(a * b);
                        break;
                }

                if (i < N - 1)
                    sw.WriteLine();
                sw.Close();
                fsA.Close();
            }
            int nEnd = Environment.TickCount;
            Console.WriteLine("Test " + file.Substring(0,3).PadRight(2) + " passed.   Time " + (nEnd - nStart).ToString().PadRight(5) + " ms");
            sr.Close();
            fsQ.Close();
        }

        static void Milestone2()
        {
            char cont;
            do
            {
                Console.WriteLine("(1) Sample Tests");
                Console.WriteLine("(2) Complete Tests");

                Console.Write("Enter choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        TestMilestone2("SampleRSA_II/SampleRSA.txt", choice);
                        break;
                    case 2:
                        TestMilestone2("Complete Test/TestRSA.txt", choice);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
                Console.Write("Do you want to calculate another? (Y/N): ");
                cont = char.Parse(Console.ReadLine());
            } while (cont == 'y' || cont == 'Y');
        }

        static void TestMilestone2(string file, int choice)
        {
            FileStream fsQ = new FileStream(file, FileMode.OpenOrCreate);

            StreamReader sr = new StreamReader(fsQ);
            switch (choice)
            {
                case 1:
                    if (File.Exists("SampleRSA_II/AnswerSample.txt"))
                        File.Delete("SampleRSA_II/AnswerSample.txt");
                    break;
                case 2:
                    if (File.Exists("Complete Test/AnswerTestRSA.txt"))
                        File.Delete("Complete Test/AnswerTestRSA.txt");
                    break;
            }
            int start = Environment.TickCount;
            int N = int.Parse(sr.ReadLine());
            for (int i = 0; i < N; i++)
            {
                FileStream fsA = new FileStream(choice == 1 ? "SampleRSA_II/AnswerSample.txt" : "Complete Test/AnswerTestRSA.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fsA);

                string _mod = sr.ReadLine();
                string _power = sr.ReadLine();
                string _number = sr.ReadLine();
                int c = int.Parse(sr.ReadLine());
                RSACore rsa = new RSACore(_number, _power, _mod);

                int nStart = Environment.TickCount;
                switch (c)
                {
                    case 0:
                        sw.WriteLine(rsa.Encrypt());
                        break;
                    case 1:
                        sw.WriteLine(rsa.Encrypt()); //decryption doesn't apply here since it's the same function.
                        break;
                }
                int nEnd = Environment.TickCount;
                Console.WriteLine("Test #" + (i + 1).ToString().PadRight(2) + " passed.   Time " + (nEnd - nStart).ToString().PadRight(5) + " ms");
                sw.Close();
                fsA.Close();
            }
            sr.Close();
            fsQ.Close();
            int end = Environment.TickCount;
            Console.WriteLine("TOTAL   TIME = " + (end - start).ToString().PadRight(6) + " ms");
            Console.WriteLine("AVERAGE TIME = " + (((double)(end - start)) / N).ToString().PadRight(6) + " ms");
        }

        static void Main(string[] args)
        {
            var a = new BigInteger("111");
            var b = new BigInteger("2");
            
            var x = new BigInteger("62");
            var y = new BigInteger("65");
            var z = new BigInteger("133");
            Console.WriteLine(x.PowerMod(y, z));

            char cont;
            do
            {
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("(1) Milestone 1");
                Console.WriteLine("(2) Milestone 2");
                Console.WriteLine("-----------------------------------------------------------");
                Console.Write("Enter choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Milestone1();
                        break;
                    case 2:
                        Milestone2();
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
                Console.Write("Another Milestone? (Y/N): ");
                cont = char.Parse(Console.ReadLine());
            } while (cont == 'y' || cont == 'Y');
        }
    }
}
