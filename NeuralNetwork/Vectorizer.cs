using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace NeuralNetwork
{
    public class Vectorizer
    {
        private List<Coeficent> _coefCollection = new List<Coeficent>();

        public void Vectorizing(string trainFileName, string outputDataFolder)
        {
            string[] dirFiles = Directory.GetFiles(trainFileName);

            for (int i = 0; i < dirFiles.Length; i++)
            {
                Console.WriteLine("====================================");
                Console.WriteLine("START CONVERTING WORDS IN FILE > {0} <", dirFiles[i]);
                // Загрузка слов в массив:
                string[] sourceData = ReadDataFile(dirFiles[i]);

                // Удаление пустых элементов из массива:
                sourceData = DeleteEmptyElements(sourceData);

                // Удаление повторяющихся элементов:
                sourceData = sourceData.Distinct().ToArray();

                // Векторизация всех слов:
                int iter = 0;
                int itemConvertedCount = 0;
                Console.WriteLine();
                for(int k = 0; k < sourceData.Length; k++)
                {
                    _coefCollection.Add(new Coeficent(sourceData[k].ToLower(), GetWordVector(sourceData[k].ToLower())));

                    itemConvertedCount++;
                    iter++;

                    if (iter == 10000)
                    {
                        Console.WriteLine("{0} / {1} words already converted!", itemConvertedCount, sourceData.Length);
                        iter = 0;
                    }
                };

                SaveResult(outputDataFolder + "//" + dirFiles[i].Split('\\')[1]);
                _coefCollection.Clear();
            }
        }

        private static string[] DeleteEmptyElements(string[] sourceData)
        {
            List<string> sourceDataList = sourceData.ToList();

            for (int i = 0; i < sourceDataList.Count; i++)
            {
                if (sourceDataList[i] == "")
                {
                    sourceDataList.RemoveAt(i);
                    i--;
                }
            }

            return sourceDataList.ToArray();
        }

        static string[] ReadDataFile(string path)
        {
            char[] delimiterChars =
            {
                ' ', '\t', '"', ',', '.', ':', ';', '!', '?', '«',
                '»', '<', '>', '„', '“', '—', '(', ')', '_', '/', '=',
                '\\', '`'
            };

            string data;
            using (StreamReader streamReader = new StreamReader(path))
            {
                data = streamReader.ReadToEnd();
            }

            return data.Split(delimiterChars);
        }

        public void SaveResult(string saveFileName)
        {
            using (StreamWriter fw = new StreamWriter(saveFileName))
            {
                _coefCollection.ForEach(coeficent =>
                {
                    fw.Write(coeficent._word + " ");
                    foreach (var cof in coeficent._listFloat)
                    {
                        fw.Write(cof + " ");
                    }

                    fw.WriteLine();
                });
            }
        }

        private static double[] GetWordVector(string wordText)
        {
            double[] alphabetEN = new Double[26];
            //int[] alphabetRU = new Int32[32];
            //int[] alphabet = new Int32[alphabetEN.Length + alphabetRU.Length];

            for (int i = 0; i < wordText.Length; i++)
            {
                alphabetEN = GetCharVector(wordText[i], alphabetEN);
            }

            return alphabetEN;
        }

        private static double[] GetCharVector(char textChar, double[] charVector)
        {
            switch (textChar)
            {
                case 'a':
                    charVector[0]++;
                    break;
                case 'b':
                    charVector[1]++;
                    break;
                case 'c':
                    charVector[2]++;
                    break;
                case 'd':
                    charVector[3]++;
                    break;
                case 'e':
                    charVector[4]++;
                    break;
                case 'f':
                    charVector[5]++;
                    break;
                case 'g':
                    charVector[6]++;
                    break;
                case 'h':
                    charVector[7]++;
                    break;
                case 'i':
                    charVector[8]++;
                    break;
                case 'j':
                    charVector[9]++;
                    break;
                case 'k':
                    charVector[10]++;
                    break;
                case 'l':
                    charVector[11]++;
                    break;
                case 'm':
                    charVector[12]++;
                    break;
                case 'n':
                    charVector[13]++;
                    break;
                case 'o':
                    charVector[14]++;
                    break;
                case 'p':
                    charVector[15]++;
                    break;
                case 'q':
                    charVector[16]++;
                    break;
                case 'r':
                    charVector[17]++;
                    break;
                case 's':
                    charVector[18]++;
                    break;
                case 't':
                    charVector[19]++;
                    break;
                case 'u':
                    charVector[20]++;
                    break;
                case 'v':
                    charVector[21]++;
                    break;
                case 'w':
                    charVector[22]++;
                    break;
                case 'x':
                    charVector[23]++;
                    break;
                case 'y':
                    charVector[24]++;
                    break;
                case 'z':
                    charVector[25]++;
                    break;
                default:
                    break;
            }

            return charVector;
        }
    }
}
