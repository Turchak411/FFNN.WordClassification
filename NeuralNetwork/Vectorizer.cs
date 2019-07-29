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
                '\\', '`', '\n'
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
            double[] alphabet = new Double[70]; // EN + RU + numbers (может еще добавить знаки '-' и т.д.)

            for (int i = 0; i < wordText.Length; i++)
            {
                alphabet = GetCharVector(wordText[i], alphabet);
            }

            return alphabet;
        }

        private static double[] GetCharVector(char textChar, double[] charVector)
        {
            char[] charsArray = new char[70]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z',

                'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м',
                'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я',

                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',

                '-'
            };

            // Увеличение значения у соответствующего символу элемента вектора:
            charVector[charsArray.ToList().FindIndex(x => x == textChar)]++;

            return charVector;
        }
    }
}
