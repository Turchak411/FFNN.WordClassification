using System;
using System.CodeDom.Compiler;

namespace WeightsGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Generator generator = new Generator();

            Console.Write("Input input-vector length: ");
            int inputVectorLength = Convert.ToInt32(Console.ReadLine());

            int[] netScheme = InputNetScheme();

            generator.GenerateMemory(inputVectorLength, netScheme, "memory.txt");

            Console.WriteLine("Memory generated!");
            Console.ReadKey();
        }

        private static int[] InputNetScheme()
        {
            Console.WriteLine("Input net scheme");
            Console.WriteLine("Example for 3 in input layer, 4 in hidden and 2 in out: \"3 4 2\"");
            string[] netSchemeText = Console.ReadLine().Split(' ');
            int[] netScheme = new Int32[netSchemeText.Length];
            for (int i = 0; i < netSchemeText.Length; i++)
            {
                netScheme[i] = Convert.ToInt32(netSchemeText[i]);
            }

            return netScheme;
        }
    }
}
