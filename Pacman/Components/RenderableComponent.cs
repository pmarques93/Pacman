using System.Collections.Generic;

namespace Pacman.Components
{
    /// <summary>
    /// Class for every renderable component. Extends Component.
    /// </summary>
    public abstract class RenderableComponent : Component
    {
        /// <summary>
        /// Gets pixels.
        /// </summary>
        public abstract
        IEnumerable<KeyValuePair<Vector2Int, ConsolePixel>> Pixels
        { get; }
    }
}