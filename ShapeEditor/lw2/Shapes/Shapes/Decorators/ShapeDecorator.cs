using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;

namespace Shapes.Decorators
{
    public abstract class ShapeDecorator : Shape
    {
        protected readonly Shape _shape;

        public ShapeDecorator(Shape shape)
        {
            _shape = shape;
        }

        protected ShapeDecorator() { }

        public abstract int GetArea();
        public abstract int GetPerimeter();
        public abstract void Visit(ShapeVisitor visitor);
        
        public override uint GetPointCount()
        {
            return _shape.GetPointCount();
        }

        public override Vector2f GetPoint(uint index)
        {
            return _shape.GetPoint(index);
        }

        public virtual void SetPosition(Vector2f position)
        {
            _shape.Position = position;
        }

        public virtual void SetFillColor(Color color)
        {
            _shape.FillColor = color;
        }

        public virtual void SetOutlineThickness(int thickness)
        {
            _shape.OutlineThickness = thickness;
        }

        public virtual void SetOutlineColor(Color color)
        {
            _shape.OutlineColor = color;
        }

        public virtual void Move(int moveX, int moveY)
        {
            Vector2f currentPosition = _shape.Position;
            _shape.Position = new Vector2f(currentPosition.X + moveX, currentPosition.Y + moveY);
        }

        public virtual void Draw(RenderWindow window)
        {
            window.Draw(_shape);
        }

        public new virtual FloatRect GetGlobalBounds()
        {
            return _shape.GetGlobalBounds();
        }
    }
}
