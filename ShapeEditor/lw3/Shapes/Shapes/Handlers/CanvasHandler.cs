using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;

namespace Shapes.Handlers
{
    public sealed class CanvasHandler : BaseShapeHandler
    {
        private static readonly CanvasHandler _instance = CreateShapeHandler();

        public static CanvasHandler Instance => _instance;
        public List<ShapeDecorator> Shapes => _shapesGroup.Shapes;
        public List<ShapeDecorator> SelectedShapes => _selectedShapesGroup.Shapes;

        private CanvasHandler( ShapeDecoratorGroup shapesGroup, SelectedShapeDecoratorGroup selectedShapesGroup )
            : base( shapesGroup, selectedShapesGroup ) { }

        private static CanvasHandler CreateShapeHandler()
        {
            return new CanvasHandler(
                ShapeDecoratorFactory.GetShapeDecoratorGroup(),
                ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup() );
        }

        public void GroupShapes()
        {
            if ( _selectedShapesGroup.Shapes.Count < 2 )
            {
                return;
            }

            ShapeDecoratorGroup group = ShapeDecoratorFactory.GetShapeDecoratorGroup();
            _selectedShapesGroup.Shapes.ForEach( x => group.Add( x ) );

            _selectedShapesGroup.RemoveAll();
            _selectedShapesGroup.Add( group );
        }

        public void UngroupShapes()
        {
            List<ShapeDecorator> groups = _selectedShapesGroup.Shapes.ToList();
            foreach ( ShapeDecorator group in groups )
            {
                if ( group.GetType() == typeof( ShapeDecoratorGroup ) )
                {
                    foreach ( ShapeDecorator shape in ( ( ShapeDecoratorGroup )group ).Shapes )
                    {
                        _selectedShapesGroup.Add( shape );
                    }
                    _selectedShapesGroup.Remove( group );
                }
            }
        }

        public void MoveShapes( int moveX, int moveY )
        {
            _selectedShapesGroup.Move( moveX, moveY );
        }

        public override void DrawShapes( RenderWindow window )
        {
            _shapesGroup.Draw( window );
            _selectedShapesGroup.Draw( window );
        }
    }
}
