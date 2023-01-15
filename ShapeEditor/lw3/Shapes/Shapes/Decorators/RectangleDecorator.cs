using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;

namespace Shapes.Decorators
{
    public class RectangleDecorator : ShapeDecorator
    {
        protected RectangleShape Rectangle => (RectangleShape)_shape;

        public Vector2f Size => Rectangle.Size;

        public RectangleDecorator(RectangleShape shape)
            : base(shape) { }

        public void SetSize(Vector2f size)
        {
            Rectangle.Size = size;
        }
        public override int GetArea()
        {
            return (int)(Rectangle.Size.Y * Rectangle.Size.X);
        }

        public override int GetPerimeter()
        {
            return 2 * (int)(Rectangle.Size.Y + Rectangle.Size.X);
        }

        public override void Accept(ShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
