using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman
{
    public class ConsoleRenderer
    {
        // Was the cursor visible before game rendering started?
        // For now we assume it was
        private bool cursorVisibleBefore = true;

        private ConsolePixel[,] framePrev, frameNext;

        // This struct is used internally for managing renderable components
        // private struct Renderable
        // {
        //     public string Name { get; }
        //     public Vector3 Pos { get; }
        //     public RenderableComponent Sprite { get; }

        //     public Renderable(
        //         string name, Vector3 pos, RenderableComponent sprite)
        //     {
        //         Name = name;
        //         Pos = pos;
        //         Sprite = sprite;
        //     }
        // }

        // Scene dimensions
        private int xdim, ydim;

        // Default background pixel
        private ConsolePixel bgPix;

        // Constructor
        public ConsoleRenderer(int xdim, int ydim, ConsolePixel bgPix)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            this.bgPix = bgPix;
            framePrev = new ConsolePixel[xdim, ydim];
            frameNext = new ConsolePixel[xdim, ydim];
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    frameNext[x, y] = bgPix;
                }
            }
        }

        // Pre-rendering setup
        public void Start()
        {
            // Clean console
            Console.Clear();

            // Hide cursor
            Console.CursorVisible = false;

            // Render the first frame
            RenderFrame();
        }

        // Post-rendering teardown
        public void Finish()
        {
            Console.CursorVisible = cursorVisibleBefore;
        }

        // Renders the actual frame
        private void RenderFrame()
        {
            // Background and foreground colors of each pixel
            ConsoleColor fgColor, bgColor;

            // Auxiliary frame variable for swapping buffers in the end
            ConsolePixel[,] frameAux;

            // Show frame in screen
            Console.SetCursorPosition(0, 0);
            fgColor = Console.ForegroundColor;
            bgColor = Console.BackgroundColor;
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    // Get current and previous pixels for this position
                    ConsolePixel pix = frameNext[x, y];
                    ConsolePixel prevPix = framePrev[x, y];

                    // Clear pixel at previous frame
                    framePrev[x, y] = bgPix;

                    // If current pixel is not renderable, use background pixel
                    if (!pix.IsRenderable)
                    {
                        pix = bgPix;
                    }

                    // If current pixel is the same as previous pixel, don't
                    // draw it
                    if (pix.Equals(prevPix)) continue;

                    // Do we have to change the background and foreground
                    // colors for this pixel?
                    if (!pix.backgroundColor.Equals(bgColor))
                    {
                        bgColor = pix.backgroundColor;
                        Console.BackgroundColor = bgColor;
                    }
                    if (!pix.foregroundColor.Equals(fgColor))
                    {
                        fgColor = pix.foregroundColor;
                        Console.ForegroundColor = fgColor;
                    }

                    // Position cursor
                    Console.SetCursorPosition(x, y);

                    // Render pixel
                    Console.Write(pix.shape);
                }

                // New line
                Console.WriteLine();
            }

            // Swap frame buffers
            frameAux = frameNext;
            frameNext = framePrev;
            framePrev = frameAux;
        }
    }
}