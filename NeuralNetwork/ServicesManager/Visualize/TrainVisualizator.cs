using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.ServicesManager.Visualize
{
    public class TrainVisualizator
    {
        private List<VisualizeObject> _prevDataList = new List<VisualizeObject>();

        public TrainVisualizator()
        {
            // Load prev data:
            _prevDataList = LoadData();
        }

        private List<VisualizeObject> LoadData(string path = "trainVisualizationData.txt")
        {
            List<VisualizeObject> data = new List<VisualizeObject>();

            using (StreamReader fileReader = new StreamReader(path))
            {
                while(!fileReader.EndOfStream)
                {
                    string[] readedData = fileReader.ReadLine().Split(' ');

                    List<double> points = new List<double>();

                    for(int i = 1; i < readedData.Length; i++)
                    {
                        points.Add(double.Parse(readedData[i]));
                    }

                    data.Add(new VisualizeObject(readedData[0], points));
                }
            }

            return data;
        }

        public void DrawTestVectorsGraphics(int width = 1000, int height = 500)
        {
            // Load current data from file:
            List<VisualizeObject> currentData = LoadData();

            //Draw graphic:
            for(int i = 0; i < currentData.Count; i++)
            {
                DrawWordGraphic(currentData[i], _prevDataList[i], width, height);
            }       
        }

        private void DrawWordGraphic(VisualizeObject vObject, VisualizeObject vObjectPrev, int width, int height)
        {
            Image bmp = new Bitmap(width, height);

            // Calculating params:
            if (width < 100 && height < 500)
                return;

            int leftX = 200;
            int leftTopY = 130;
            int leftBotY = height - 50;

            int xStep = 15;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Pen penBlack = new Pen(Color.Black, 10);
                Pen penGreen = new Pen(Color.Green, 10);
                Pen penRed = new Pen(Color.Red, 10);

                // Draw frame lines:
                g.DrawRectangle(new Pen(Color.Black, 2), new Rectangle(0, 0, width, height));

                // Draw content:
                float startX = leftX;
                float startY = leftBotY - 30;

                float scaling = 250;

                for (int i = 1; i < vObject._points.Count; i++)
                {
                    float secondY = startY - (float)vObject._points[i] * scaling;

                    g.DrawLine(penBlack, startX, startY, startX, secondY);

                    startX += xStep * 4;
                }

                startX = leftX;
                startY = leftBotY - 30;

                startX += xStep;

                for (int i = 1; i < vObjectPrev._points.Count; i++)
                {
                    float secondY = startY - (float)vObjectPrev._points[i] * scaling;

                    if ((float)vObject._points[i] > (float)vObjectPrev._points[i])
                    {
                        g.DrawLine(penRed, startX, startY, startX, secondY);
                    }
                    else
                    {
                        if ((float) vObject._points[i] < (float) vObjectPrev._points[i])
                        {
                            g.DrawLine(penGreen, startX, startY, startX, secondY);
                        }
                        else
                        {
                            g.DrawLine(penBlack, startX, startY, startX, secondY);
                        }
                    }

                    startX += xStep * 4;
                }
            }

            if (!Directory.Exists("graphics_train"))
            {
                Directory.CreateDirectory("graphics_train");
            }

            bmp.Save("graphics_train/" + vObject._word + "- train result.png");
        }
    }
}
