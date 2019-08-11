using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Hash2Vec.Core;
using Hash2Vec.ServiceManager.DistanceVectors;
using Hash2Vec.ServiceManager.ReadingVectors;

namespace UniqueVectors.ServicesManager
{
    public class Hash2VecTextReader : IHash2VecReader
    {
        public Vocabulary Read(StreamReader inputStream)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));

            int vectorSize = -1;
            bool isFirstLine = true;
            var vectors = new List<Representation>();
            var enUsCulture = CultureInfo.GetCultureInfo("en-US");

            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine()?.Split(' ');
                if (isFirstLine)
                {
                    if (line != null && line.Length == 2)
                    {
                        var vocabularySize = 0;
                        try
                        {
                            //header
                            vocabularySize = int.Parse(line[0]);
                            vectorSize = int.Parse(line[1]);
                            vectors = new List<Representation>(vocabularySize);
                            continue;
                        }
                        catch
                        {
                            vocabularySize = 0;
                            vectorSize = -1;
                        }
                    }

                    if (vectorSize == -1)
                        vectorSize = float.TryParse(line.First(), out _) ? line.Length - 1 : line.Length - 2;

                    isFirstLine = false;
                }

                var vecs = line != null && float.TryParse(line.First(), out _) ? line.Take(vectorSize).ToArray()
                    : (line ?? throw new InvalidOperationException()).Skip(1).Take(vectorSize).ToArray();

                if (vecs.Length != vectorSize)
                    throw new FormatException("word \"" + line.First() + "\" has wrong vector size of " + vecs.Length);

                vectors.Add(new Representation(line.First(), vecs.Select(float.Parse).ToArray()));
            }
            return new Vocabulary(vectors, vectorSize);
        }

        public List<Vector> LoadVectors(string path)
        {
            var vectors = new List<Vector>();

            using (var fileReader = new StreamReader(path))
            {
                while (!fileReader.EndOfStream)
                {
                    var readLine = fileReader.ReadLine()?.Split(' ');
                    if (readLine != null)
                    {
                        var value = new float[readLine.Length - 1];

                        for (int i = 0; i < readLine.Length - 1; i++)
                        {
                            value[i] = float.Parse(readLine[i]);
                        }

                        vectors.Add(new Vector(readLine[0], value));
                    }
                }
            }

            return vectors;
        }




        public Vocabulary Read(Stream inputStream)
        {
            using (var strStream = new StreamReader(inputStream))
            {
                return Read(strStream);
            }
        }

        public Vocabulary Read(string path)
        {
            using (var fileSteram = new FileStream(path, FileMode.Open))
            {
                return Read(fileSteram);
            }
        }
    }
}
