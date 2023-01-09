using SFML.Graphics;
using System.Collections.Generic;

namespace Shapes.Creators
{
    public abstract class ShapeCreator
    {
        public abstract Shape Create(List<string> parameters);
    }
}
