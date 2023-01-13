using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.Visitors;

namespace Shapes.Applications
{
    public sealed class Application
    {
        private readonly RenderWindow _window;
        private readonly StreamReader _input;
        private readonly StreamWriter _output;
        private Vector2f _clickMousePosition;

        private static CanvasHandler CanvasHandler => CanvasHandler.Instance;
        private static ToolbarHandler ToolbarHandler => ToolbarHandler.Instance;
        private static readonly Color BackgroundColor = new( 30, 30, 32 );

        public Application( RenderWindow window, StreamReader input, StreamWriter output )
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
            while ( !_input.EndOfStream )
            {
                List<string> shapeParams = _input
                    .ReadLine()
                    .Split( " " )
                    .Where( s => !string.IsNullOrEmpty( s ) )
                    .ToList();
                if ( shapeParams.Count == 0 )
                {
                    continue;
                }

                string shapeType = shapeParams[ 0 ].ToUpper();
                shapeParams.RemoveAt( 0 );

                ShapeDecorator shape = shapeType switch
                {
                    "TRIANGLE" => ShapeDecoratorFactory.GetTriangleDecorator( shapeParams ),
                    "RECTANGLE" => ShapeDecoratorFactory.GetRectangleDecorator( shapeParams ),
                    "CIRCLE" => ShapeDecoratorFactory.GetCircleDecorator( shapeParams ),
                    _ => throw new ArgumentOutOfRangeException( $"Unknown shape type: {shapeType}" ),
                };
                CanvasHandler.AddShape( shape );
            }
        }

        public void PrintShapesInfo()
        {
            ShapeDescriptionVisitor descriptionVisitor = new( _output );
            List<ShapeDecorator> shapes = CanvasHandler.Shapes
                .Concat( CanvasHandler.SelectedShapes )
                .ToList();
            foreach ( ShapeDecorator shape in shapes )
            {
                shape.Accept( descriptionVisitor );
            }
        }

        public void ProcessWindow()
        {
            while ( _window.IsOpen )
            {
                _window.DispatchEvents();
                _window.Clear( BackgroundColor );

                DrawApplication();

                _window.Display();
            }
        }

        private void DrawApplication()
        {
            CanvasHandler.DrawShapes( _window );
            ToolbarHandler.DrawShapes( _window );
        }

        private void WindowClosed( object sender, EventArgs e )
        {
            _window.Close();
        }

        private void WindowMousePressed( object sender, MouseButtonEventArgs e )
        {
            switch ( e.Button )
            {
                case Mouse.Button.Left:
                    _clickMousePosition.X = e.X;
                    _clickMousePosition.Y = e.Y;

                    ShapeDecorator activeShape = CanvasHandler.GetActiveShape( e.X, e.Y );
                    if ( Keyboard.IsKeyPressed( Keyboard.Key.LShift ) )
                    {
                        CanvasHandler.SelectShape( activeShape );
                    }
                    else
                    {
                        if ( !CanvasHandler.SelectedShapes.Contains( activeShape ) )
                        {
                            CanvasHandler.ForceSelectShape( activeShape );
                        }
                    }
                    break;
                default:
                    return;
            }
        }

        private void WindowKeyPressed( object sender, KeyEventArgs e )
        {
            switch ( e.Code )
            {
                case Keyboard.Key.G:
                    if ( Keyboard.IsKeyPressed( Keyboard.Key.LControl ) )
                    {
                        CanvasHandler.GroupShapes();
                    }
                    break;
                case Keyboard.Key.U:
                    if ( Keyboard.IsKeyPressed( Keyboard.Key.LControl ) )
                    {
                        CanvasHandler.UngroupShapes();
                    }
                    break;
                default:
                    return;
            }
        }

        private void WindowMouseMoved( object sender, MouseMoveEventArgs e )
        {
            ShapeDecorator activeShape = CanvasHandler.GetActiveShape( e.X, e.Y );
            if ( Mouse.IsButtonPressed( Mouse.Button.Left ) && CanvasHandler.SelectedShapes.Contains( activeShape ) )
            {
                int moveX = ( int )( e.X - _clickMousePosition.X );
                int moveY = ( int )( e.Y - _clickMousePosition.Y );
                CanvasHandler.MoveShapes( moveX, moveY );

                _clickMousePosition.X += moveX;
                _clickMousePosition.Y += moveY;
            }
        }
    }
}
