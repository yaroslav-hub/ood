using System;
using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using Shapes.Types;

namespace Shapes.Factories
{
    public static class ButtonsFactory
    {
        public const int DefaultButtonHeight = 40;

        public static RectangleDecorator GetButton( ToolbarButtonType buttonType )
        {
            switch ( buttonType )
            {
                case ToolbarButtonType.AddShape:
                    return GetAddShapeButton();
                case ToolbarButtonType.RemoveShape:
                    return GetRemoveShapeButton();
                case ToolbarButtonType.SetShapeFillColor:
                    return GetSetShapeFillColorButton();
                default:
                    throw new ArgumentException( $"Unknown toolbar button type {buttonType}" );
            }
        }

        private static RectangleDecorator GetAddShapeButton()
        {
            return GetDefaultButton();
        }

        private static RectangleDecorator GetRemoveShapeButton()
        {
            RectangleDecorator button = GetDefaultButton();
            button.SetSize( new Vector2f(60, DefaultButtonHeight ) );

            return button;
        }

        private static RectangleDecorator GetSetShapeFillColorButton()
        {
            return GetDefaultButton();
        }

        private static RectangleDecorator GetDefaultButton()
        {
            RectangleDecorator button = new( new RectangleShape() );
            button.SetPosition( new Vector2f( 10, 5 ) );
            button.SetSize( new Vector2f( 40, DefaultButtonHeight ) );
            button.SetFillColor( Color.Blue );

            return button;
        }
    }
}
