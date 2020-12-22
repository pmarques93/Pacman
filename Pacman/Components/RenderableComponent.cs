using System.Collections.Generic;

namespace Pacman
{
    public abstract class RenderableComponent : Component
    {
        public abstract
        IEnumerable<KeyValuePair<Vector2Int, ConsolePixel>> Pixels { get; }
    }
}