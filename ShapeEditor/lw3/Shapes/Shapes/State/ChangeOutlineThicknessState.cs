using SFML.System;
using Shapes.Applications;
using Shapes.Decorators;
using Shapes.Handlers;

namespace Shapes.State
{
    public sealed class ChangeOutlineThicknessState : IState
    {
        private readonly Application _application;
        private ToolbarHandler ToolbarHandler => _application.ToolbarHandler;
        private CanvasHandler CanvasHandler => _application.CanvasHandler;

        public ChangeOutlineThicknessState(Application application)
        {
            _application = application;
        }

        public void SimpleClick(Vector2f click)
        {
            ShapeDecorator activeShape = CanvasHandler.GetActivatedShape((int)click.X, (int)click.Y);
            if (activeShape != null)
            {
                activeShape.SetOutlineThickness(ToolbarHandler.CurrentOutlineThickness);
            }
        }

        public void PressedKeyG()
        {
        }

        public void PressedKeyU()
        {
        }

        public void MouseMoved(Vector2f mousePosition)
        {
        }
    }
}
