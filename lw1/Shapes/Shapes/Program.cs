using SFML.Graphics;
using SFML.System;
using SFML.Window;
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
                        ConvexShape triangle = GetTriangle(shapeParams);
                        window.Draw(triangle);
                        TriangleDecorator triangleDecorator = new(triangle);
                        DisplayShapeDescription(shapeType, triangleDecorator.GetArea(), triangleDecorator.GetPerimeter());
                        break;
                    case "RECTANGLE":
                        RectangleShape rectangle = GetRectangle(shapeParams);
                        window.Draw(rectangle);
                        RectangleDecorator rectangleDecorator = new(rectangle);
                        DisplayShapeDescription(shapeType, rectangleDecorator.GetArea(), rectangleDecorator.GetPerimeter());
                        break;
                    case "CIRCLE":
                        CircleShape circle = GetCircle(shapeParams);
                        window.Draw(circle);
                        CircleDecorator circleDecorator = new(circle);
                        DisplayShapeDescription(shapeType,circleDecorator.GetArea(), circleDecorator.GetPerimeter());
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

        private static ConvexShape GetTriangle(List<string> coordinates)
        {
            if (coordinates.Count != 6)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            Vector2f firstVector = new(float.Parse(coordinates[0]), float.Parse(coordinates[1]));
            Vector2f secondVector = new(float.Parse(coordinates[2]), float.Parse(coordinates[3])); 
            Vector2f thirdVector = new(float.Parse(coordinates[4]), float.Parse(coordinates[5]));
            ConvexShape triangle = new();
            triangle.Position = new Vector2f(50, 50);
            triangle.SetPointCount(3);
            triangle.SetPoint(0, firstVector);
            triangle.SetPoint(1, secondVector);
            triangle.SetPoint(2, thirdVector);
            triangle.FillColor = new Color(161, 133, 148);

            return triangle;
        }

        private static RectangleShape GetRectangle(List<string> coordinates)
        {
            if (coordinates.Count != 4)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            float x0 = float.Parse(coordinates[0]);
            float y0 = float.Parse(coordinates[1]);
            float x1 = float.Parse(coordinates[2]);
            float y1 = float.Parse(coordinates[3]);

            Vector2f sizeVector = new(x1 - x0, y1 - y0);
            Vector2f positionVector = new((x0 + x1) /2, (y0 + y1) / 2);
            RectangleShape rectangle = new();
            rectangle.Size = sizeVector;
            rectangle.Position = positionVector;
            rectangle.FillColor = new Color(255, 140, 105);

            return rectangle;
        }

        private static CircleShape GetCircle(List<string> coordinates)
        {
            if (coordinates.Count != 3)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            Vector2f positionVector = new(float.Parse(coordinates[0]), float.Parse(coordinates[1]));
            CircleShape circle = new(float.Parse(coordinates[2]));
            circle.Position = positionVector;
            circle.FillColor = new Color(168, 228, 160);

            return circle;
        }
    }
}
