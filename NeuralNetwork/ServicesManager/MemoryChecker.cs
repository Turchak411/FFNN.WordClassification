using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.ServicesManager
{
    public class MemoryChecker
    {
        public bool IsValid(string memoryPath)
        {
            using (StreamReader fileReader = new StreamReader(memoryPath))
            {
                while(!fileReader.EndOfStream)
                {
                    string[] readedLine = fileReader.ReadLine().Split(' ');

                    if(readedLine.Length < 3)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
