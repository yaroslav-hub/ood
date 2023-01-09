using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Shapes.Creators
{
    public sealed class TriangleCreator : ShapeCreator
    {
        private static readonly TriangleCreator _instance = new();

        public static TriangleCreator Instance
        {
            get { return _instance; }
        }

        public override ConvexShape Create(List<string> parameters)
        {
            if (parameters.Count != 6)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            Vector2f firstVector = new(float.Parse(parameters[0]), float.Parse(parameters[1]));
            Vector2f secondVector = new(float.Parse(parameters[2]), float.Parse(parameters[3]));
            Vector2f thirdVector = new(float.Parse(parameters[4]), float.Parse(parameters[5]));
            ConvexShape triangle = new();
            triangle.Position = new Vector2f(50, 50);
            triangle.SetPointCount(3);
            triangle.SetPoint(0, firstVector);
            triangle.SetPoint(1, secondVector);
            triangle.SetPoint(2, thirdVector);
            triangle.FillColor = new Color(161, 133, 148);

            return triangle;
        }

        private TriangleCreator() { }
    }
}
