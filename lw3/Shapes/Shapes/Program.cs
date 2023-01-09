using SFML.Graphics;
using SFML.Window;
using Shapes.Creators;
using Shapes.Decorators;
using Shapes.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Shapes
{
    class Program
    {
        private const string StandartInputFileName = "input.txt";
        private const string StandartOutputFileName = "output.txt";

        static void Main(string[] args)
        {
            StreamReader inputFileStream = new(StandartInputFileName);
            StreamWriter outputFileStream = new(StandartOutputFileName);

            RenderWindow window = new(new VideoMode(1000, 1000), "Shapes");
            window.Clear(new Color(30, 30, 32));

            ShapeDescriptionVisitor visitor = new(outputFileStream);

            while (!inputFileStream.EndOfStream)
            {
                List<string> shapeParams = inputFileStream
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
                        triangleDecorator.Accept(visitor);
                        break;
                    case "RECTANGLE":
                        RectangleShape rectangle = RectangleCreator.Instance.Create(shapeParams);
                        window.Draw(rectangle);
                        RectangleDecorator rectangleDecorator = new(rectangle);
                        rectangleDecorator.Accept(visitor);
                        break;
                    case "CIRCLE":
                        CircleShape circle = CircleCreator.Instance.Create(shapeParams);
                        window.Draw(circle);
                        CircleDecorator circleDecorator = new(circle);
                        circleDecorator.Accept(visitor);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}");
                }
            }

            window.Display();
            Thread.Sleep(400000);
        }
    }
}
