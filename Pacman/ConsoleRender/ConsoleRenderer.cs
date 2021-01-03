using System;
using System.Collections.Generic;
using Pacman.Collisions;
using Pacman.Components;

namespace Pacman.ConsoleRender
{
    /// <summary>
    /// Class responsible for realizing the render of the scene and its objects.
    /// Implements IGameObject.
    /// </summary>
    public class ConsoleRenderer : IGameObject
    {
        /// <summary>
        /// Gets consoleRenderer name.
        /// </summary>
        public string Name { get; }

        private readonly bool cursorVisibleBefore;

        private readonly IList<GameObject> gameObjects;

        // Scene dimensions
        private readonly int xdim;
        private readonly int ydim;

        private readonly bool inGame;

        private readonly Collision collisions;

        // Default background pixel
        private readonly ConsolePixel bgPix;

        private ConsolePixel[,] currentFrame;
        private ConsolePixel[,] nextFrame;

        /// <summary>
        /// Constructor, that creates a new instance of ConsoleRenderer and
        /// initializes its members. Constructor for game.
        /// </summary>
        /// <param name="xdim">Horizontal dimension of the scene.</param>
        /// <param name="ydim">Vertical dimension of the scene.</param>
        /// <param name="bgPix">Default ConsolePixel for the background.</param>
        /// <param name="collision">Reference to a collision class.</param>
        /// <param name="name">Name of the ConsoleRenderer object.</param>
        public ConsoleRenderer(
            int xdim,
            int ydim,
            ConsolePixel bgPix,
            Collision collision,
            string name = "")
        {
            cursorVisibleBefore = true;
            this.xdim = xdim;
            this.ydim = ydim;
            this.bgPix = bgPix;
            Name = name;
            gameObjects = new List<GameObject>();
            currentFrame = new ConsolePixel[xdim, ydim];
            nextFrame = new ConsolePixel[xdim, ydim];
            collisions = collision;
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    nextFrame[x, y] = bgPix;
                }
            }

            inGame = true;
        }

        /// <summary>
        /// Constructor, that creates a new instance of ConsoleRenderer and
        /// initializes its members. Constructor for menu.
        /// </summary>
        /// <param name="xdim">Horizontal dimension of the scene.</param>
        /// <param name="ydim">Vertical dimension of the scene.</param>
        /// <param name="bgPix">Default ConsolePixel for the background.</param>
        /// <param name="name">Name of the ConsoleRenderer object.</param>
        public ConsoleRenderer(
            int xdim,
            int ydim,
            ConsolePixel bgPix,
            string name = "")
        {
            cursorVisibleBefore = true;
            this.xdim = xdim;
            this.ydim = ydim;
            this.bgPix = bgPix;
            Name = name;
            gameObjects = new List<GameObject>();
            currentFrame = new ConsolePixel[xdim, ydim];
            nextFrame = new ConsolePixel[xdim, ydim];
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    nextFrame[x, y] = bgPix;
                }
            }

            inGame = false;
        }

        /// <summary>
        /// Runs once at the start and realizes a pre-rendering setup.
        /// </summary>
        public void Start()
        {
            // Clean console
            Console.Clear();

            // Hide cursor
            Console.CursorVisible = false;

            // Render the first frame
            RenderFrame();

            if (inGame)
                collisions.FoodCollision += RemoveGameObject;
        }

        /// <summary>
        /// Runs once at the finish and tears down the rendering.
        /// </summary>
        public void Finish()
        {
            Console.CursorVisible = cursorVisibleBefore;

            if (inGame)
                collisions.FoodCollision -= RemoveGameObject;
        }

        /// <summary>
        /// Realizes the rendering on each update.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                RenderableComponent rendComp =
                                    gameObjects[i].
                                    GetComponent<RenderableComponent>();
                TransformComponent transform =
                                    gameObjects[i].
                                    GetComponent<TransformComponent>();

                // Cycle through all pixels in sprite
                foreach (KeyValuePair<Vector2Int, ConsolePixel> pixel in
                                                            rendComp.Pixels)
                {
                    // Get absolute position of current pixel
                    int y = transform.Position.Y + pixel.Key.Y;
                    int x = transform.Position.X + pixel.Key.X;

                    // Put pixel in frame
                    nextFrame[x, y] = pixel.Value;
                }
            }

            // Renders the frame
            RenderFrame();
        }

        /// <summary>
        /// Renders the actual frame.
        /// </summary>
        private void RenderFrame()
        {
            // Background and foreground colors of each pixel
            ConsoleColor fgColor, bgColor;

            // Auxiliary frame variable for swapping buffers in the end
            ConsolePixel[,] auxFrame;

            // Show frame in screen
            Console.SetCursorPosition(0, 0);

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
                    if (pix.Equals(prevPix) && !pix.Equals(bgPix))
                        continue;

                    bgColor = pix.BackgroundColor;
                    Console.BackgroundColor = bgColor;

                    fgColor = pix.ForegroundColor;
                    Console.ForegroundColor = fgColor;

                    // Position cursor
                    Console.SetCursorPosition(x, y);

                    // Render pixel
                    Console.Write(pix.Shape);
                }

                // New line
                Console.WriteLine();
            }

            // Swap frame buffers
            auxFrame = currentFrame;
            currentFrame = nextFrame;
            nextFrame = auxFrame;
        }

        /// <summary>
        /// Adds an object to be rendered.
        /// </summary>
        /// <param name="gameObject">Object to be rendered.</param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes an object from being rendered.
        /// </summary>
        /// <param name="gameObject">Object to remove.</param>
        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
    }
}