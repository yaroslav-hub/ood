using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes.Decorators
{
    class TriangleDecorator : ShapeDecorator
    {
        private readonly Vector2f _firstPoint;
        private readonly Vector2f _secondPoint;
        private readonly Vector2f _thirdPoint;
        public TriangleDecorator(ConvexShape shape)
            : base(shape)
        {
            _firstPoint = shape.GetPoint(0);
            _secondPoint = shape.GetPoint(1);
            _thirdPoint = shape.GetPoint(2);
        }

        public override int GetArea()
        {
            return (int)Math.Abs(
                (_secondPoint.X - _firstPoint.X) * (_thirdPoint.Y - _firstPoint.Y) 
                - (_thirdPoint.X - _firstPoint.X) * (_secondPoint.Y - _firstPoint.Y) / 2);
        }

        public override int GetPerimeter()
        {
            return (int)(
                GetSideLength(_firstPoint, _secondPoint)
                + GetSideLength(_secondPoint, _thirdPoint)
                + GetSideLength(_firstPoint, _thirdPoint));
        }

        private double GetSideLength(Vector2f firstPoint, Vector2f secondPoint)
        {
            return Math.Sqrt(
                Math.Pow(firstPoint.X - secondPoint.X, 2) 
                + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }
    }
}
