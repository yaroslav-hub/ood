using SFML.System;
using SFML.Window;
using Shapes.Applications;
using Shapes.Decorators;
using Shapes.Handlers;

namespace Shapes.State
{
    public sealed class DragAndDropState : IState
    {
        private readonly Application _application;
        private CanvasHandler CanvasHandler => _application.CanvasHandler;

        public DragAndDropState(Application application)
        {
            _application = application;
        }

        public void SimpleClick(Vector2f click)
        {
            ShapeDecorator activeShape = _application.CanvasHandler.GetActivatedShape((int)click.X, (int)click.Y);
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                CanvasHandler.SelectShape(activeShape);
            }
            else
            {
                if (!CanvasHandler.SelectedShapes.Contains(activeShape))
                {
                    CanvasHandler.ForceSelectShape(activeShape);
                }
            }
        }

        public void PressedKeyG()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                _application.CanvasHandler.GroupShapes();
            }
        }

        public void PressedKeyU()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                _application.CanvasHandler.UngroupShapes();
            }
        }

        public void MouseMoved(Vector2f mousePosition)
        {
            ShapeDecorator activeShape = CanvasHandler.GetActivatedShape((int)mousePosition.X, (int)mousePosition.Y);
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && _application.CanvasHandler.SelectedShapes.Contains(activeShape))
            {
                int moveX = (int)(mousePosition.X - _application.ClickMousePosition.X);
                int moveY = (int)(mousePosition.Y - _application.ClickMousePosition.Y);
                CanvasHandler.MoveShapes(moveX, moveY);

                _application.SetClickMousePosition(new Vector2f(
                    _application.ClickMousePosition.X + moveX,
                    _application.ClickMousePosition.Y + moveY));
            }
        }
    }
}
