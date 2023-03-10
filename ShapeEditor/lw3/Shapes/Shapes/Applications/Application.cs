using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.State;
using Shapes.Types;
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
        private readonly ToolbarHandler _toolbarHandler;
        private readonly CanvasHandler _canvasHandler;

        private readonly DragAndDropState _dragAndDropState;
        private readonly AddShapeState _addShapeState;
        private readonly ChangeFillColorState _changeFillColorState;
        private readonly ChangeOutlineColorState _changeOutlineColorState;
        private readonly ChangeOutlineThicknessState _changeOutlineThicknessState;
        private Vector2f _clickMousePosition;
        private IState _state;

        public CanvasHandler CanvasHandler => _canvasHandler;
        public ToolbarHandler ToolbarHandler => _toolbarHandler;
        public Vector2f ClickMousePosition => _clickMousePosition;

        public Application(RenderWindow window, StreamReader input, StreamWriter output)
        {

            _input = input;
            _output = output;

            _dragAndDropState = new DragAndDropState(this);
            _addShapeState = new AddShapeState(this);
            _changeFillColorState = new ChangeFillColorState(this);
            _changeOutlineColorState = new ChangeOutlineColorState(this);
            _changeOutlineThicknessState = new ChangeOutlineThicknessState(this);
            
            _toolbarHandler = new ToolbarHandler(
                SetDragAndDropState,
                SetAddShapeState,
                SetChangeFillColorState,
                SetChangeOutlineColorState,
                SetChangeOutlineThicknessState);
            _canvasHandler = new CanvasHandler();

            _window = window;
            _window.Closed += WindowClosed;
            _window.MouseButtonPressed += WindowMousePressed;
            _window.KeyPressed += WindowKeyPressed;
            _window.MouseMoved += WindowMouseMoved;

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
                if (shapeParams.Count == 0 || !Enum.TryParse(shapeParams[0], true, out ShapeType shapeType))
                {
                    continue;
                }

                shapeParams.RemoveAt(0);
                ShapeDecorator shape = shapeType switch
                {
                    ShapeType.Triangle => ShapeDecoratorFactory.GetTriangleDecorator(shapeParams),
                    ShapeType.Rectangle => ShapeDecoratorFactory.GetRectangleDecorator(shapeParams),
                    ShapeType.Circle => ShapeDecoratorFactory.GetCircleDecorator(shapeParams),
                    _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}")
                };
                CanvasHandler.AddShape(shape);
            }
        }

        public void PrintShapesInfo()
        {
            ShapeDescriptionVisitor descriptionVisitor = new(_output);
            List<ShapeDecorator> shapes = CanvasHandler.Shapes
                .Concat(CanvasHandler.SelectedShapes)
                .ToList();
            foreach (ShapeDecorator shape in shapes)
            {
                shape.Accept(descriptionVisitor);
            }
        }

        public void ProcessWindow()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear(DefaultColors.BlueDark);

                DrawApplication();

                _window.Display();
            }
        }

        private void DrawApplication()
        {
            CanvasHandler.Draw(_window);
            ToolbarHandler.Draw(_window);
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
                    int mouseX = e.X;
                    int mouseY = e.Y;
                    _clickMousePosition.X = mouseX;
                    _clickMousePosition.Y = mouseY;

                    if (ToolbarHandler.HandleClick(mouseX, mouseY))
                    {
                        return;
                    }

                    _state.SimpleClick(new Vector2f(e.X, e.Y));
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
                    _state.PressedKeyG();
                    break;
                case Keyboard.Key.U:
                    _state.PressedKeyU();
                    break;
                default:
                    return;
            }
        }

        private void WindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            _state.MouseMoved(new Vector2f(e.X, e.Y));
        }

        private void SetDragAndDropState() => _state = _dragAndDropState;
        private void SetAddShapeState()
        {
            _state = _addShapeState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeOutlineColorState()
        {
            _state = _changeOutlineColorState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeFillColorState()
        {
            _state = _changeFillColorState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeOutlineThicknessState()
        {
            _state = _changeOutlineThicknessState;
            CanvasHandler.UnselectAll();
        }

        public void SetClickMousePosition(Vector2f position) => _clickMousePosition = position;
    }
}
