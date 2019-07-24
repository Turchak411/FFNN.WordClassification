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
            double[] alphabet = new Double[69]; // EN + RU + numbers (может еще добавить знаки '-' и т.д.)

            for (int i = 0; i < wordText.Length; i++)
            {
                alphabet = GetCharVector(wordText[i], alphabet);
            }

            return alphabet;
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
                case 'а':
                    charVector[26]++;
                    break;
                case 'б':
                    charVector[27]++;
                    break;
                case 'в':
                    charVector[28]++;
                    break;
                case 'г':
                    charVector[29]++;
                    break;
                case 'д':
                    charVector[30]++;
                    break;
                case 'е':
                    charVector[31]++;
                    break;
                case 'ё':
                    charVector[32]++;
                    break;
                case 'ж':
                    charVector[33]++;
                    break;
                case 'з':
                    charVector[34]++;
                    break;
                case 'и':
                    charVector[35]++;
                    break;
                case 'й':
                    charVector[36]++;
                    break;
                case 'к':
                    charVector[37]++;
                    break;
                case 'л':
                    charVector[38]++;
                    break;
                case 'м':
                    charVector[39]++;
                    break;
                case 'н':
                    charVector[40]++;
                    break;
                case 'о':
                    charVector[41]++;
                    break;
                case 'п':
                    charVector[42]++;
                    break;
                case 'р':
                    charVector[43]++;
                    break;
                case 'с':
                    charVector[44]++;
                    break;
                case 'т':
                    charVector[45]++;
                    break;
                case 'у':
                    charVector[46]++;
                    break;
                case 'ф':
                    charVector[47]++;
                    break;
                case 'х':
                    charVector[48]++;
                    break;
                case 'ц':
                    charVector[49]++;
                    break;
                case 'ч':
                    charVector[50]++;
                    break;
                case 'ш':
                    charVector[51]++;
                    break;
                case 'щ':
                    charVector[52]++;
                    break;
                case 'ъ':
                    charVector[53]++;
                    break;
                case 'ы':
                    charVector[54]++;
                    break;
                case 'ь':
                    charVector[55]++;
                    break;
                case 'э':
                    charVector[56]++;
                    break;
                case 'ю':
                    charVector[57]++;
                    break;
                case 'я':
                    charVector[58]++;
                    break;
                case '0':
                    charVector[59]++;
                    break;
                case '1':
                    charVector[60]++;
                    break;
                case '2':
                    charVector[61]++;
                    break;
                case '3':
                    charVector[62]++;
                    break;
                case '4':
                    charVector[63]++;
                    break;
                case '5':
                    charVector[64]++;
                    break;
                case '6':
                    charVector[65]++;
                    break;
                case '7':
                    charVector[66]++;
                    break;
                case '8':
                    charVector[67]++;
                    break;
                case '9':
                    charVector[68]++;
                    break;
                default:
                    break;
            }

            return charVector;
        }
    }
}
