using SFML.Graphics;
using Shapes.Visitors;
using System;

namespace Shapes.Decorators
{
    public sealed class CircleDecorator : ShapeDecorator
    {
        private readonly int _radius;
        public CircleDecorator(CircleShape shape)
            : base(shape)
        {
            _radius = (int)shape.Radius;
        }

        public override void Accept(ShapeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetArea()
        {
            return (int)(_radius * _radius * Math.PI);
        }

        public override int GetPerimeter()
        {
            return (int)(2 * _radius * Math.PI);
        }
    }
}
