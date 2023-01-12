using SFML.Graphics;
using Shapes.Visitors;
using System;

namespace Shapes.Decorators
{
    public sealed class CircleDecorator : ShapeDecorator
    {
        private CircleShape Circle => (CircleShape)_shape;

        public CircleDecorator(CircleShape shape)
            : base(shape) { }

        public void SetRadius(uint radius)
        {
            Circle.Radius = radius;
        }

        public override int GetArea()
        {
            return (int)(Circle.Radius * Circle.Radius * Math.PI);
        }

        public override int GetPerimeter()
        {
            return (int)(2 * Circle.Radius * Math.PI);
        }

        public override void Visit(ShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
