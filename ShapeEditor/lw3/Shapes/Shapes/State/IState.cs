using SFML.System;

namespace Shapes.State
{
    public interface IState
    {
        void SimpleClick(Vector2f click);
        void PressedKeyG();
        void PressedKeyU();
        void MouseMoved(Vector2f mousePosition);
    }
}