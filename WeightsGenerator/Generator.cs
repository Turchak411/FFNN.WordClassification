using System;
using System.IO;
using System.Threading;

namespace WeightsGenerator
{
    public class Generator
    {
        public void GenerateMemory(int inputVectorLength, int[] netScheme, string filePath)
        {
            using (StreamWriter fileWriter = new StreamWriter(filePath))
            {
                for (int i = 0; i < netScheme.Length; i++)
                {
                    for (int k = 0; k < netScheme[i]; k++)
                    {
                        fileWriter.Write("layer_{0} neuron_{1}", i, k);

                        if (i == 0)
                        {
                            GenerateValueRow(fileWriter, inputVectorLength);
                        }
                        else
                        {
                            GenerateValueRow(fileWriter, netScheme[i - 1]);
                        }
                    }
                }
            }
        }

        private void GenerateValueRow(StreamWriter fileWriter, int valuesRowLength)
        {
            for (int i = 0; i < valuesRowLength; i++)
            {
                fileWriter.Write(" " + GenerateValue());
                Thread.Sleep(20);
            }

            fileWriter.WriteLine();
        }

        private double GenerateValue()
        {
            Random rnd = new Random();
            return rnd.NextDouble() - 0.5;
        }
    }
}
