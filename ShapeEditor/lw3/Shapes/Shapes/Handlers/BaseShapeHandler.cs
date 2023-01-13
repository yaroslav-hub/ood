using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using Shapes.Compounds;
using Shapes.Decorators;

namespace Shapes.Handlers
{
    public abstract class BaseShapeHandler
    {
        protected readonly ShapeDecoratorGroup _shapesGroup;
        protected readonly SelectedShapeDecoratorGroup _selectedShapesGroup;

        protected BaseShapeHandler( ShapeDecoratorGroup shapesGroup, SelectedShapeDecoratorGroup selectedShapesGroup )
        {
            _shapesGroup = shapesGroup;
            _selectedShapesGroup = selectedShapesGroup;
        }

        public abstract void DrawShapes( RenderWindow window );

        public void AddShape( ShapeDecorator shape )
        {
            if ( _shapesGroup.Shapes.Contains( shape ) )
            {
                return;
            }

            _shapesGroup.Add( shape );
        }

        public void SelectShape( ShapeDecorator shape )
        {
            if ( !_shapesGroup.Shapes.Contains( shape ) || _selectedShapesGroup.Shapes.Contains( shape ) )
            {
                if ( shape == null )
                {
                    UnselectAll();
                }
                else
                {
                    UnselectShape( shape );
                }
                return;
            }

            _shapesGroup.Remove( shape );
            _selectedShapesGroup.Add( shape );
        }

        public void ForceSelectShape( ShapeDecorator shape )
        {
            UnselectAll();
            SelectShape( shape );
        }

        public void UnselectShape( ShapeDecorator shape )
        {
            if ( _shapesGroup.Shapes.Contains( shape ) || !_selectedShapesGroup.Shapes.Contains( shape ) )
            {
                return;
            }

            _selectedShapesGroup.Remove( shape );
            _shapesGroup.Add( shape );
        }

        public ShapeDecorator GetActiveShape( int mouseX, int mouseY )
        {
            return _selectedShapesGroup.Shapes.LastOrDefault( x => x.GetGlobalBounds().Contains( mouseX, mouseY ) )
                ?? _shapesGroup.Shapes.LastOrDefault( x => x.GetGlobalBounds().Contains( mouseX, mouseY ) );
        }

        protected void UnselectAll()
        {
            List<ShapeDecorator> selectedShapes = _selectedShapesGroup.Shapes.ToList();
            selectedShapes.ForEach( x => UnselectShape( x ) );
        }
    }
}
