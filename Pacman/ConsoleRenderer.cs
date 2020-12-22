using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman
{
    public class ConsoleRenderer : IGameObject
    {
        // Was the cursor visible before game rendering started?
        // For now we assume it was

        /// <summary>
        /// ConsoleRenderer name
        /// </summary>
        public string Name { get; }
        private bool cursorVisibleBefore = true;

        private ConsolePixel[,] currentFrame, nextFrame;

        private ICollection<IGameObject> gameObjects;
        
        // Scene dimensions
        private int xdim, ydim;

        // Default background pixel
        private ConsolePixel bgPix;

        // Constructor
        public ConsoleRenderer(int xdim, int ydim, ConsolePixel bgPix, string name = "")
        {
            this.xdim = xdim;
            this.ydim = ydim;
            this.bgPix = bgPix;
            Name = name;
            gameObjects = new List<IGameObject>();
            currentFrame = new ConsolePixel[xdim, ydim];
            nextFrame = new ConsolePixel[xdim, ydim];
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    nextFrame[x, y] = bgPix;
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
            ConsolePixel[,] auxFrame;

            // Show frame in screen
            Console.SetCursorPosition(0, 0);
            fgColor = Console.ForegroundColor;
            bgColor = Console.BackgroundColor;
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    // Get current and previous pixels for this position
                    ConsolePixel pix = nextFrame[x, y];
                    ConsolePixel prevPix = currentFrame[x, y];

                    // Clear pixel at previous frame
                    currentFrame[x, y] = bgPix;

                    // If current pixel is not renderable, use background pixel
                    if (!pix.IsRenderable)
                    {
                        pix = bgPix;
                    }

                    // If current pixel is the same as previous pixel, don't
                    // draw it
                    if (pix.Equals(prevPix)) continue;


                    bgColor = pix.backgroundColor;
                    Console.BackgroundColor = bgColor;

                    fgColor = pix.foregroundColor;
                    Console.ForegroundColor = fgColor;

                    // Position cursor
                    Console.SetCursorPosition(x, y);

                    // Render pixel
                    Console.Write(pix.shape);
                }

                // New line
                Console.WriteLine();
            }

            // Swap frame buffers
            auxFrame = currentFrame;
            currentFrame = nextFrame;
            nextFrame = auxFrame;
        }

        public void Update()
        {
            // foreach (Renderable rend in stuffToRender)
            foreach (GameObject gameObj in gameObjects)
            {
                RenderableComponent rendComp = 
                                    gameObj.GetComponent<RenderableComponent>();
                TransformComponent transform = 
                                    gameObj.GetComponent<TransformComponent>();

                // Cycle through all pixels in sprite
                foreach (KeyValuePair<Vector2Int, ConsolePixel> pixel in 
                                                            rendComp.Pixels)
                {
                    // Get absolute position of current pixel
                    int y = (int)(transform.Position.Y + pixel.Key.Y);
                    int x = (int)(transform.Position.X + pixel.Key.X);
                    
                    // Throw exception if any of these is out of bounds
                    if (x < 0 || x >= xdim || y < 0 || y >= ydim)
                        throw new IndexOutOfRangeException(
                            $"Out of bounds pixel at ({x},{y}) in game object"
                            + $" '{gameObj.Name}'");

                    // Put pixel in frame
                    nextFrame[x, y] = pixel.Value;
                }
            }

            // Render the frame
            RenderFrame();
        }

        public void AddGameObject(IGameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }
    }
}