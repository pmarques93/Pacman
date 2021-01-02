using System;
using System.Collections.Generic;

namespace Pacman
{
    /// <summary>
    /// Class for console sprites. Extends RenderableComponent.
    /// </summary>
    public class ConsoleSprite : RenderableComponent
    {
        // The position-pixel pairs are actually kept here
        private IDictionary<Vector2Int, ConsolePixel> pixels;

        /// <summary>
        /// Gets Pixels.
        /// Since a console sprite is a renderable component, it must implement
        /// this property, which returns an ienumerable of position-pixel pairs
        /// to render.
        /// </summary>
        public override IEnumerable<KeyValuePair<Vector2Int, ConsolePixel>>
            Pixels => pixels;

        /// <summary>
        /// Changes sprite color.
        /// </summary>
        /// <param name="fgColor">Foreground Color.</param>
        /// <param name="bgColor">Background Color.</param>
        public void ChangeColor(ConsoleColor fgColor, ConsoleColor bgColor)
        {
            IDictionary<Vector2Int, ConsolePixel> tempPixels = 
                                    new Dictionary<Vector2Int, ConsolePixel>();

            foreach (Vector2Int v in pixels.Keys)
            {
                char shape = pixels[v].Shape;
                if (!shape.Equals(default))
                {
                    tempPixels[v] = new ConsolePixel(shape, fgColor, bgColor);
                }
            }

            pixels = tempPixels;
        }

        /// <summary>
        /// Constructor for ConsoleSprite.
        /// </summary>
        /// <param name="pixels">IDictionary with pixels to render.</param>
        public ConsoleSprite(IDictionary<Vector2Int, ConsolePixel> pixels)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>(pixels);
        }

        /// <summary>
        /// Constructor for ConsoleSprite.
        /// </summary>
        /// <param name="pixels">Char with pixels to render.</param>
        /// <param name="fgColor">Foreground Color.</param>
        /// <param name="bgColor">Background Color.</param>
        public ConsoleSprite(
            char[,] pixels, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>();
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    char shape = pixels[x, y];
                    if (!shape.Equals(default))
                    {
                        this.pixels[new Vector2Int(x, y)] =
                            new ConsolePixel(shape, fgColor, bgColor);
                    }
                }
            }
        }
    }
}