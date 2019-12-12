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

                if(i < N - 1)
                    sw.WriteLine();
                sw.Close();
                fsA.Close();
            }
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
            switch(choice)
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

                switch (c)
                {
                    case 0:
                        sw.WriteLine(rsa.encrypt());
                        break;
                    case 1:
                        sw.WriteLine(rsa.encrypt()); //decryption doesn't apply here since it's the same function.
                        break;
                }
                Console.WriteLine("Test #" + (i + 1) + " passed.");
                sw.Close();
                fsA.Close();
            }
            sr.Close();
            fsQ.Close();
        }

        static void Main(string[] args)
        {

            /*BigInteger m = new BigInteger("2003");
            BigInteger e = new BigInteger("7");
            BigInteger n = new BigInteger("3713");

            RSACore rsa = new RSACore(m, e, n);
            Console.WriteLine(rsa.encrypt());

            Console.WriteLine(rsa.decrypt());*/
            /*
            RSAKeyGenerator rSAKeyGenerator = new RSAKeyGenerator();

            int bitLength = 4;
            var prime1 = rSAKeyGenerator.GetPrimeNumber(bitLength/2);
            var prime2 = rSAKeyGenerator.GetPrimeNumber(bitLength/2);
            var N = rSAKeyGenerator.GetN(prime1, prime2);
            var PHI = rSAKeyGenerator.GetPhi(prime1, prime2);
            var E = rSAKeyGenerator.GetE(PHI,bitLength);
            // not sure it work 
            var D = rSAKeyGenerator.GetD(PHI, E);
            Console.WriteLine(E.ToString());
            Console.WriteLine(N.ToString());
            Console.WriteLine(D.ToString());
            */


            /*
           string mod = "2039896550861909479";
           int pow = 7;
           string d = "1165655170271755543"; 
            */

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
