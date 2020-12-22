using System;
using System.Collections.Generic;

namespace Pacman
{
    public class ConsoleSprite : RenderableComponent
    {
        // Since a console sprite is a renderable component, it must implement
        // this property, which returns an ienumerable of position-pixel pairs
        // to render
        public override
        IEnumerable<KeyValuePair<Vector2Int, ConsolePixel>> Pixels => pixels;

        // The position-pixel pairs are actually kept here
        private IDictionary<Vector2Int, ConsolePixel> pixels;

        // Below there are several constructors for this class

        public ConsoleSprite(IDictionary<Vector2Int, ConsolePixel> pixels)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>(pixels);
        }

        public ConsoleSprite(ConsolePixel[,] pixels)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>();
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    ConsolePixel cpixel = pixels[x, y];
                    if (cpixel.IsRenderable)
                    {
                        this.pixels[new Vector2Int(x, y)] = cpixel;
                    }
                }
            }
        }

        public ConsoleSprite(char[,] pixels)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>();
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    char shape = pixels[x, y];
                    if (!shape.Equals(default(char)))
                    {
                        this.pixels[new Vector2Int(x, y)] =
                            new ConsolePixel(shape);
                    }
                }
            }
        }

        public ConsoleSprite(
            char[,] pixels, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            this.pixels = new Dictionary<Vector2Int, ConsolePixel>();
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    char shape = pixels[x, y];
                    if (!shape.Equals(default(char)))
                    {
                        this.pixels[new Vector2Int(x, y)] =
                            new ConsolePixel(shape, fgColor, bgColor);
                    }
                }
            }
        }
    }
}