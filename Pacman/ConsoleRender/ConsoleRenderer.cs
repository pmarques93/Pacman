using System;
using System.Collections.Generic;

namespace Pacman
{
    /// <summary>
    /// Responsible for realizing the render of the scene and its objects.
    /// </summary>
    public class ConsoleRenderer : IGameObject
    {
        // Was the cursor visible before game rendering started?
        // For now we assume it was

        /// <summary>
        /// ConsoleRenderer name
        /// </summary>
        public string Name { get; }
        private bool cursorVisibleBefore = true;
        private bool firstFrame;

        private ConsolePixel[,] currentFrame, nextFrame;

        private ICollection<IGameObject> gameObjects;

        // Scene dimensions
        private int xdim, ydim;

        // Default background pixel
        private ConsolePixel bgPix;

        private readonly bool inGame;

        // Component
        private readonly Collision collisions;

        /// <summary>
        /// Constructor, that creates a new instance of ConsoleRenderer and
        /// initializes its members. Constructor for game
        /// </summary>
        /// <param name="xdim">Horizontal dimension of the scene.</param>
        /// <param name="ydim">Vertical dimension of the scene.</param>
        /// <param name="bgPix">Default ConsolePixel for the background.</param>
        /// <param name="collision">Reference to a collision class.</param>
        /// <param name="name">Name of the ConsoleRenderer object.</param>
        public ConsoleRenderer(int xdim,
                               int ydim,
                               ConsolePixel bgPix,
                               Collision collision,
                               string name = "")
        {
            this.xdim = xdim;
            this.ydim = ydim;
            this.bgPix = bgPix;
            Name = name;
            gameObjects = new List<IGameObject>();
            currentFrame = new ConsolePixel[xdim, ydim];
            nextFrame = new ConsolePixel[xdim, ydim];
            this.collisions = collision;
            for (int y = 0; y < ydim; y++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    nextFrame[x, y] = bgPix;
                }
            }
            inGame = true;
            firstFrame = true;
        }

        /// <summary>
        /// Constructor, that creates a new instance of ConsoleRenderer and
        /// initializes its members. Constructor for menu
        /// </summary>
        /// <param name="xdim">Horizontal dimension of the scene.</param>
        /// <param name="ydim">Vertical dimension of the scene.</param>
        /// <param name="bgPix">Default ConsolePixel for the background.</param>
        /// <param name="name">Name of the ConsoleRenderer object.</param>
        public ConsoleRenderer(int xdim,
                               int ydim,
                               ConsolePixel bgPix,
                               string name = "")
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
            inGame = false;
            firstFrame = true;
        }

        /// <summary>
        /// Runs once at the start and realizes a pre-rendering setup
        /// </summary>
        public void Start()
        {
            // Clean console
            Console.Clear();

            // Hide cursor
            Console.CursorVisible = false;

            // Render the first frame
            RenderFrame();
            firstFrame = false;

            if (inGame) collisions.FoodCollision += RemoveGameObject;
        }

        /// <summary>
        /// Runs once at the finish and tears down the rendering.
        /// </summary>
        public void Finish()
        {
            Console.CursorVisible = cursorVisibleBefore;

            if (inGame) collisions.FoodCollision -= RemoveGameObject;
        }

        /// <summary>
        /// Realizes the rendering on each update.
        /// </summary>
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
                    if (pix.Equals(prevPix) && !pix.Equals(bgPix))
                        continue;


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

        /// <summary>
        /// Adds an object to be rendered.
        /// </summary>
        /// <param name="gameObject">Object to be rendered.</param>
        public void AddGameObject(IGameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes an object from being rendered.
        /// </summary>
        /// <param name="gameObject">Object to remove</param>
        public void RemoveGameObject(IGameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
    }
}