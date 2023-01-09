using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Shapes.Creators
{
    public sealed class CircleCreator : ShapeCreator
    {
        private readonly static CircleCreator _instance = new();

        public static CircleCreator Instance
        {
            get { return _instance; }
        }

        public override CircleShape Create(List<string> parameters)
        {
            if (parameters.Count != 3)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            Vector2f positionVector = new(float.Parse(parameters[0]), float.Parse(parameters[1]));
            CircleShape circle = new(float.Parse(parameters[2]));
            circle.Position = positionVector;
            circle.FillColor = new Color(168, 228, 160);

            return circle;
        }

        private CircleCreator() { }
    }
}
