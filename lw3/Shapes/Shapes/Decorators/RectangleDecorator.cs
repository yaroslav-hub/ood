﻿using SFML.Graphics;
using Shapes.Visitors;

namespace Shapes.Decorators
{
    public sealed class RectangleDecorator : ShapeDecorator
    {
        private readonly int _height;
        private readonly int _width;

        public RectangleDecorator(RectangleShape shape)
            : base(shape)
        {
            _height = (int)shape.Size.Y;
            _width = (int)shape.Size.X;
        }

        public override void Accept(ShapeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetArea()
        {
            return _height * _width;
        }

        public override int GetPerimeter()
        {
            return 2 * (_height + _width);
        }
    }
}
