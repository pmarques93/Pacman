using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    /// <summary>
    /// Class for renderable strings component. Extendes renderable component.
    /// </summary>
    public class RenderableStringComponent : RenderableComponent
    {
        // Since this is a renderable component, it must implement the Pixels
        // property
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

        // Delegate which returns a string to be rendered
        private Func<string> getStr;

        // Delegate which returns a position for every character in the string
        private Func<int, Vector2Int> getPos;

        // The foreground and background colors of the string to be rendered
        private ConsoleColor fgColor, bgColor;

        /// <summary>
        /// Constructor for RenderableStringComponent
        /// </summary>
        /// <param name="getStr">Delegeate with the string to be rendered</param>
        /// <param name="getPos">Delegate with position for every character
        /// in the string</param>
        /// <param name="fgColor">Foreground Color</param>
        /// <param name="bgColor">Background Color</param>
        public RenderableStringComponent(
            Func<string> getStr, Func<int, Vector2Int> getPos,
            ConsoleColor fgColor, ConsoleColor bgColor)
        {
            this.getStr = getStr;
            this.getPos = getPos;
            this.fgColor = fgColor;
            this.bgColor = bgColor;
        }
    }
}
