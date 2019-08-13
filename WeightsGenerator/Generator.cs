using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace WeightsGenerator
{
    public class Generator
    {
        public void GenerateMemory(int inputVectorLength, int[] netScheme, string filePath)
        {
            using (var progress = new ProgressBar())
            {
                int iteration = 0;
                int iterationsTotal = CalcTotalIterations(netScheme);

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

                            iteration++;
                            progress.Report((double)iteration / iterationsTotal);
                        }
                    }
                }
            }
        }

        private int CalcTotalIterations(int[] netScheme)
        {
            int iterationsTotal = 0;

            foreach (var layer in netScheme)
            {
                iterationsTotal += layer;
            }

            return iterationsTotal;
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
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.NextDouble() - 0.5;
        }
    }
}
