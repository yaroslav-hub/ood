using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Shapes.Creators
{
    public sealed class RectangleCreator : ShapeCreator
    {
        private readonly static RectangleCreator _instance = new();

        public static RectangleCreator Instance
        {
            get { return _instance; }
        }

        public override RectangleShape Create(List<string> parameters)
        {
            if (parameters.Count != 4)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            float x0 = float.Parse(parameters[0]);
            float y0 = float.Parse(parameters[1]);
            float x1 = float.Parse(parameters[2]);
            float y1 = float.Parse(parameters[3]);

            Vector2f sizeVector = new(x1 - x0, y1 - y0);
            Vector2f positionVector = new((x0 + x1) / 2, (y0 + y1) / 2);
            RectangleShape rectangle = new();
            rectangle.Size = sizeVector;
            rectangle.Position = positionVector;
            rectangle.FillColor = new Color(255, 140, 105);

            return rectangle;
        }

        private RectangleCreator() { }
    }
}
