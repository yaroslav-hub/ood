using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Types;

namespace Shapes.Handlers
{
    public sealed class ToolbarHandler : BaseShapeHandler
    {
        private static readonly ToolbarHandler _instance = CreateToolbarHandler();

        public static ToolbarHandler Instance => _instance;

        private ToolbarHandler( ShapeDecoratorGroup shapesGroup, SelectedShapeDecoratorGroup selectedShapesGroup )
            : base( shapesGroup, selectedShapesGroup )
        {
            foreach ( ToolbarButtonType item in Enum.GetValues( typeof( ToolbarButtonType ) ) )
            {
                RectangleDecorator newButton = ButtonsFactory.GetButton( item );
                RectangleDecorator lastButton = (RectangleDecorator)_shapesGroup.Shapes.LastOrDefault();
                if ( lastButton != null )
                {
                    int moveX = (int)(lastButton.Position.X + lastButton.Size.X + 20);
                    newButton.Move(moveX, 0);
                }
                _shapesGroup.Add( newButton );
            }
        }

        private static ToolbarHandler CreateToolbarHandler()
        {
            return new ToolbarHandler(
                ShapeDecoratorFactory.GetShapeDecoratorGroup(),
                ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup() );
        }

        public override void DrawShapes( RenderWindow window )
        {
            RectangleDecorator background = new( new RectangleShape() );
            background.SetPosition( new Vector2f( 0, 0 ) );
            background.SetSize( new Vector2f( window.Size.X, 50 ) );
            background.SetFillColor( Color.White );
            background.Draw( window );

            _shapesGroup.Draw( window );
            _selectedShapesGroup.Draw( window );
        }
    }
}
