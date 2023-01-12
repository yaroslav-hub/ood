using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using Shapes.Visitors;
using System;
using System.Collections.Generic;

namespace Shapes.Compounds
{
    public class ShapeDecoratorGroup : ShapeDecorator
    {
        private readonly List<ShapeDecorator> _shapes;

        public List<ShapeDecorator> Shapes => _shapes;

        public ShapeDecoratorGroup()
            : base() 
        {
            _shapes = new();
        }

        public void Add(ShapeDecorator shape)
        {
            if (_shapes.Contains(shape))
            {
                return;
            }

            _shapes.Add(shape);
        }

        public void Remove(ShapeDecorator shape)
        {
            _shapes.Remove(shape);
        }

        public void RemoveAll()
        {
            _shapes.Clear();
        }

        public override int GetArea()
        {
            throw new NotImplementedException();
        }

        public override int GetPerimeter()
        {
            throw new NotImplementedException();
        }

        public override void Visit(ShapeVisitor visitor)
        {
            _shapes.ForEach(x => visitor.Visit(x));
        }

        public override void Move(int moveX, int moveY)
        {
            _shapes.ForEach(x => x.Move(moveX, moveY));
        }

        public override void Draw(RenderWindow window)
        {
            _shapes.ForEach(x => x.Draw(window));
        }

        public override FloatRect GetGlobalBounds()
        {
            FloatRect frame = _shapes[0].GetGlobalBounds();
            Vector2f leftTopPoint;
            leftTopPoint.X = frame.Left;
            leftTopPoint.Y = frame.Top;

            Vector2f rightBottomPoint;
            rightBottomPoint.X = frame.Left + frame.Width;
            rightBottomPoint.Y = frame.Top + frame.Height;

            foreach (ShapeDecorator shape in _shapes)
            {
                FloatRect currentFrame = shape.GetGlobalBounds();

                if (currentFrame.Left < leftTopPoint.X)
                {
                    leftTopPoint.X = currentFrame.Left;
                }
                if (currentFrame.Top < leftTopPoint.Y)
                {
                    leftTopPoint.Y = currentFrame.Top;
                }
                if (currentFrame.Left + currentFrame.Width > rightBottomPoint.X)
                {
                    rightBottomPoint.X = currentFrame.Left + currentFrame.Width;
                }
                if (currentFrame.Top + currentFrame.Height > rightBottomPoint.Y)
                {
                    rightBottomPoint.Y = currentFrame.Top + currentFrame.Height;
                }
            }

            return new FloatRect(
                leftTopPoint.X, 
                leftTopPoint.Y, 
                rightBottomPoint.X - leftTopPoint.X, 
                rightBottomPoint.Y - leftTopPoint.Y);
        }
    }
}
