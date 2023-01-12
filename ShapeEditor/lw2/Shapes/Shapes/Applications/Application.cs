using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shapes.Applications
{
    public sealed class Application
    {
        private readonly RenderWindow _window;
        private readonly StreamReader _input;
        private readonly StreamWriter _output;
        private Vector2f _clickMousePosition;

        private static ShapeHandler ShapeHandler => ShapeHandler.Instance;
        private static readonly Color BackgroundColor = new(30, 30, 32);

        public Application(RenderWindow window, StreamReader input, StreamWriter output)
        {
            _window = window;
            _window.Closed += WindowClosed;
            _window.MouseButtonPressed += WindowMousePressed;
            _window.KeyPressed += WindowKeyPressed;
            _window.MouseMoved += WindowMouseMoved;

            _input = input;
            _output = output;

            _clickMousePosition = new Vector2f();
        }

        public void ReadDefaultShapes()
        {
            while (!_input.EndOfStream)
            {
                List<string> shapeParams = _input
                    .ReadLine()
                    .Split(" ")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();
                if (shapeParams.Count == 0)
                {
                    continue;
                }

                string shapeType = shapeParams[0].ToUpper();
                shapeParams.RemoveAt(0);

                ShapeDecorator shape = shapeType switch
                {
                    "TRIANGLE" => ShapeDecoratorFactory.GetTriangleDecorator(shapeParams),
                    "RECTANGLE" => ShapeDecoratorFactory.GetRectangleDecorator(shapeParams),
                    "CIRCLE" => ShapeDecoratorFactory.GetCircleDecorator(shapeParams),
                    _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}"),
                };
                ShapeHandler.AddShape(shape);
            }
        }

        public void PrintShapesInfo()
        {
            ShapeDescriptionVisitor descriptionVisitor = new(_output);
            List<ShapeDecorator> shapes = ShapeHandler.Shapes
                .Concat(ShapeHandler.SelectedShapes)
                .ToList();
            foreach (ShapeDecorator shape in shapes)
            {
                shape.Visit(descriptionVisitor);
            }
        }

        public void ProcessWindow()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear(BackgroundColor);

                DrawApplication();

                _window.Display();
            }
        }

        private void DrawApplication()
        {
            ShapeHandler.DrawShapes(_window);
        }

        private ShapeDecorator GetActiveShape(int mouseX, int mouseY)
        {
            return ShapeHandler.SelectedShapes.LastOrDefault(x => x.GetGlobalBounds().Contains(mouseX, mouseY)) 
                ?? ShapeHandler.Shapes.LastOrDefault(x => x.GetGlobalBounds().Contains(mouseX, mouseY));
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _window.Close();
        }

        private void WindowMousePressed(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    _clickMousePosition.X = e.X;
                    _clickMousePosition.Y = e.Y;

                    ShapeDecorator activeShape = GetActiveShape(e.X, e.Y);
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                    {
                        ShapeHandler.SelectShape(GetActiveShape(e.X, e.Y));
                    }
                    else
                    {
                        if (!ShapeHandler.SelectedShapes.Contains(activeShape))
                        {
                            ShapeHandler.ForceSelectShape(activeShape);
                        }
                    }
                    break;
                default:
                    return;
            }
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.G:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    {
                        ShapeHandler.GroupShapes();
                    }
                    break;
                case Keyboard.Key.U:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    {
                        ShapeHandler.UngroupShapes();
                    }
                    break;
                default:
                    return;
            }
        }

        private void WindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && ShapeHandler.SelectedShapes.Count != 0)
            {
                int moveX = (int)(e.X - _clickMousePosition.X);
                int moveY = (int)(e.Y - _clickMousePosition.Y);
                ShapeHandler.MoveShapes(moveX, moveY);
                
                _clickMousePosition.X += moveX;
                _clickMousePosition.Y += moveY;
            }
        }
    }
}
