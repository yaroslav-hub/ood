using SFML.Graphics;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Handlers
{
    public sealed class ShapeHandler
    {
        private static readonly ShapeHandler _instance = new();
        private readonly ShapeDecoratorGroup _shapesGroup;
        private readonly SelectedShapeDecoratorGroup _selectedShapesGroup;

        public static ShapeHandler Instance => _instance;
        public List<ShapeDecorator> Shapes => _shapesGroup.Shapes;
        public List<ShapeDecorator> SelectedShapes => _selectedShapesGroup.Shapes;

        private ShapeHandler()
        {
            _shapesGroup = ShapeDecoratorFactory.GetShapeDecoratorGroup();
            _selectedShapesGroup = ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup();
        }

        public void AddShape(ShapeDecorator shape)
        {
            if (_shapesGroup.Shapes.Contains(shape))
            {
                return;
            }

            _shapesGroup.Add(shape);
        }

        public void SelectShape(ShapeDecorator shape)
        {
            if (!_shapesGroup.Shapes.Contains(shape) || _selectedShapesGroup.Shapes.Contains(shape))
            {
                if (shape == null)
                {
                    UnselectAll();
                }
                else
                {
                    UnselectShape(shape);
                }
                return;
            }

            _shapesGroup.Remove(shape);
            _selectedShapesGroup.Add(shape);
        }

        public void ForceSelectShape(ShapeDecorator shape)
        {
            UnselectAll();
            SelectShape(shape);
        }

        public void UnselectShape(ShapeDecorator shape)
        {
            if (_shapesGroup.Shapes.Contains(shape) || !_selectedShapesGroup.Shapes.Contains(shape))
            {
                return;
            }

            _selectedShapesGroup.Remove(shape);
            _shapesGroup.Add(shape);
        }

        public void GroupShapes()
        {
            if (_selectedShapesGroup.Shapes.Count < 2)
            {
                return;
            }

            ShapeDecoratorGroup group = ShapeDecoratorFactory.GetShapeDecoratorGroup();
            _selectedShapesGroup.Shapes.ForEach(x => group.Add(x));

            _selectedShapesGroup.RemoveAll();
            _selectedShapesGroup.Add(group);
        }

        public void UngroupShapes()
        {
            List<ShapeDecorator> groups = _selectedShapesGroup.Shapes.ToList();
            foreach (ShapeDecorator group in groups)
            {
                if (group.GetType() == typeof(ShapeDecoratorGroup))
                {
                    foreach (ShapeDecorator shape in ((ShapeDecoratorGroup)group).Shapes)
                    {
                        _selectedShapesGroup.Add(shape);
                    }
                    _selectedShapesGroup.Remove(group);
                }
            }
        }

        public void MoveShapes(int moveX, int moveY)
        {
            _selectedShapesGroup.Move(moveX, moveY);
        }

        public void DrawShapes(RenderWindow window)
        {
            _shapesGroup.Draw(window);
            _selectedShapesGroup.Draw(window);
        }

        private void UnselectAll()
        {
            List<ShapeDecorator> selectedShapes = _selectedShapesGroup.Shapes.ToList();
            selectedShapes.ForEach(x => UnselectShape(x));
        }
    }
}
