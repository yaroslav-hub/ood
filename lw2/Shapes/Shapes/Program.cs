using SFML.Graphics;
using SFML.Window;
using Shapes.Creators;
using Shapes.Decorators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Shapes
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = Console.ReadLine();
            StreamReader fileStream = new(file);

            RenderWindow window = new(new VideoMode(1000, 1000), "Shapes");
            window.Clear(new Color(30, 30, 32));

            while (!fileStream.EndOfStream)
            {
                List<string> shapeParams = fileStream
                    .ReadLine()
                    .Split(" ")
                    .Where(s => !String.IsNullOrEmpty(s))
                    .ToList();
                if (shapeParams.Count == 0)
                {
                    continue;
                }

                string shapeType = shapeParams[0].ToUpper();
                shapeParams.Remove(shapeType);
                switch (shapeType)
                {
                    case "TRIANGLE":
                        ConvexShape triangle = TriangleCreator.Instance.Create(shapeParams);
                        window.Draw(triangle);
                        TriangleDecorator triangleDecorator = new(triangle);
                        DisplayShapeDescription(shapeType, triangleDecorator.GetArea(), triangleDecorator.GetPerimeter());
                        break;
                    case "RECTANGLE":
                        RectangleShape rectangle = RectangleCreator.Instance.Create(shapeParams);
                        window.Draw(rectangle);
                        RectangleDecorator rectangleDecorator = new(rectangle);
                        DisplayShapeDescription(shapeType, rectangleDecorator.GetArea(), rectangleDecorator.GetPerimeter());
                        break;
                    case "CIRCLE":
                        CircleShape circle = CircleCreator.Instance.Create(shapeParams);
                        window.Draw(circle);
                        CircleDecorator circleDecorator = new(circle);
                        DisplayShapeDescription(shapeType, circleDecorator.GetArea(), circleDecorator.GetPerimeter());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}");
                }
            }

            window.Display();
            Thread.Sleep(400000);
        }

        private static void DisplayShapeDescription(string type, int area, int perimeter)
        {
            Console.WriteLine($"{type}: P={perimeter}; S={area}");
        }
    }
}
