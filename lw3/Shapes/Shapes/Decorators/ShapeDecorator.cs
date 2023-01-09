using SFML.Graphics;
using Shapes.Visitors;

namespace Shapes.Decorators
{
    public abstract class ShapeDecorator
    {
        private readonly Shape _shape;

        public ShapeDecorator(Shape shape)
        {
            _shape = shape;
        }

        public abstract int GetArea();
        public abstract int GetPerimeter();
        public abstract void Accept(ShapeVisitor visitor);
    }
}
