using System;
using System.Collections.Generic;
using Pacman.Components;

namespace Pacman.ConsoleRender
{
    /// <summary>
    /// Class for renderable strings. Extends Component.
    /// </summary>
    public class RenderableStringComponent : RenderableComponent
    {
        /// <summary>
        /// Gets pixels to render.
        /// Since this is a renderable component, it must implement the Pixels
        /// property.
        /// </summary>
        public override
        IEnumerable<KeyValuePair<Vector2Int, ConsolePixel>> Pixels
        {
            get
            {
                // Get the string to render
                string strToRender = getStr();

                // Cycle through the string
                for (int i = 0; i < strToRender.Length; i++)
                {
                    // Get position for the current character
                    Vector2Int pos = getPos(i);

                    // Create a console pixel for the current character
                    ConsolePixel pix =
                        new ConsolePixel(strToRender[i], fgColor, bgColor);

                    // Return the position and the pixel to be rendered
                    yield return new
                        KeyValuePair<Vector2Int, ConsolePixel>(pos, pix);
                }
            }
        }

        /// <summary>
        /// Delegate which returns a string to be rendered.
        /// </summary>
        private readonly Func<string> getStr;

        /// <summary>
        /// Delegate which returns a position for every character in the string.
        /// </summary>
        private readonly Func<int, Vector2Int> getPos;

        /// <summary>
        /// The foreground colors of the string to be rendered.
        /// </summary>
        private readonly ConsoleColor fgColor;

        /// <summary>
        /// The background colors of the string to be rendered.
        /// </summary>
        private readonly ConsoleColor bgColor;

        /// <summary>
        /// Constructor for RenderableStringComponent.
        /// </summary>
        /// <param name="getStr">String to print.</param>
        /// <param name="getPos">Position to print.</param>
        /// <param name="fgColor">Foreground Color.</param>
        /// <param name="bgColor">Background Color.</param>
        public RenderableStringComponent(
            Func<string> getStr,
            Func<int, Vector2Int> getPos,
            ConsoleColor fgColor,
            ConsoleColor bgColor)
        {
            this.getStr = getStr;
            this.getPos = getPos;
            this.fgColor = fgColor;
            this.bgColor = bgColor;
        }
    }
}