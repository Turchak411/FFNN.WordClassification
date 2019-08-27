using NeuralNetwork.ServicesManager.Vectors;
using NeuralNetwork.ServicesManager.Visualize;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetwork.ServicesManager.Visualize
{
    public class TrainVisualizator
    {
        private List<VisualizeObject> _objects;

        public void StartVisualize(List<Coeficent> vectors)
        {
            _objects = new List<VisualizeObject>();

            for(int i = 0; i < vectors.Count; i++)
            {
                _objects.Add(new VisualizeObject(vectors[i]._word));
            }
        }

        private void CreatePicture(VisualizeObject vObject, int width = 1000, int height = 500)
        {
            Image bmp = new Bitmap(width, height);

            // Calculating params:
            if (width < 100 && height < 500)
                return;

            int leftX = 100;
            int leftTopY = 130;
            int leftBotY = height - 50;
            int rightX = width - 50;
            int rightTopY = leftTopY;
            int rightBotY = leftBotY;

            float startX = leftX;
            float startY = (float)vObject._points[0] * (leftBotY - leftTopY) + 130;

            int xStep = 5;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Pen penBlack = new Pen(Color.Black, 1);
                Pen penGreen = new Pen(Color.Green, 2);
                Pen penRed = new Pen(Color.Red, 2);

                // Draw frame lines:
                g.DrawLine(penRed, leftX, leftTopY, rightX, rightTopY);
                g.DrawLine(penGreen, leftX, leftBotY, rightX, rightBotY);
                g.DrawLine(penGreen, leftX, leftBotY, leftX, leftBotY);
                g.DrawLine(penGreen, rightX, rightBotY, rightX, rightBotY);

                // Draw content:
                for (int i = 1; i < vObject._points.Count; i++)
                {
                    float newY = (float)vObject._points[i] * (leftBotY - leftTopY) + 130;

                    g.DrawLine(penBlack, startX, startY, startX + xStep, newY);

                    startX += xStep;
                    startY = newY;
                }
            }

            bmp.Save(vObject._word + "- train result.png");
        }

        public void AddPoint(Coeficent vector, double netResult)
        {
            int objectIndex = _objects.FindIndex(x => x._word == vector._word);

            _objects[objectIndex]._points.Add(netResult);
        }

        public void SaveGraphics()
        {
            foreach(VisualizeObject vObject in _objects)
            {
                CreatePicture(vObject);
            }
        }

    }
}
