using System;
using System.IO;

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

                switch (choice)
                {
                    case 1:
                        Console.WriteLine(BigInteger.Add(firstNumber, secondNumber));
                        sw.WriteLine(BigInteger.Add(firstNumber, secondNumber));
                        break;
                    case 2:
                        sw.WriteLine(BigInteger.Sub(firstNumber, secondNumber));
                        break;
                    case 3:
                        sw.WriteLine(BigInteger.Mul(firstNumber, secondNumber));
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
            
        }
    }
}
