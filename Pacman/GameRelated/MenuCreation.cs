using System;
using System.Collections.Generic;

namespace Pacman
{
    class MenuCreation
    {
        private const byte XSIZE = 28;
        private const byte YSIZE = 31;

        private readonly ConsolePixel backgroundPixel;
        private readonly ConsoleRenderer consoleRenderer;
        private readonly Scene scene;
        private readonly KeyReaderComponent keyReader;
        private readonly FileWriter fileWriter;

        // Text to render
        private GameObject selector;
        private GameObject startText;
        private GameObject quitText;
        private GameObject controlsText;
        private GameObject movementText;
        private GameObject actionsText;

        private GameObject pacmanLogo;

        public MenuCreation()
        {
            backgroundPixel = new ConsolePixel(' ', ConsoleColor.White,
                                                ConsoleColor.DarkBlue);

            consoleRenderer = new ConsoleRenderer(XSIZE * 3, YSIZE,
                                            backgroundPixel,
                                            "Console Renderer");

            keyReader = new KeyReaderComponent(ConsoleKey.Enter);

            scene = new Scene(XSIZE, YSIZE, keyReader);

            fileWriter = new FileWriter(Path.lives);
            fileWriter.CreateLivesText(4);
        }

        public void Run()
        {
            CreateGameObjects();

            AddGameObjectsToScene();

            AddGameObjectsToRenderer();

            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(100);
        }

        private void CreateGameObjects()
        {

            pacmanLogo = new GameObject("Pacman Logo");
            ConsolePixel logoPixel = new ConsolePixel(
                ' ', ConsoleColor.White, ConsoleColor.DarkYellow);

            Dictionary<Vector2Int, ConsolePixel> logoPixels =
                    new Dictionary<Vector2Int, ConsolePixel>();

            ICollection<Vector2Int[]> testList = new List<Vector2Int[]>();

            CreatePacmanSprite(testList);

            foreach (Vector2Int[] v in testList)
            {
                for (int i = v[0].X; i < v[1].X + 1; i++)
                {
                    for (int j = v[0].Y; j < v[1].Y + 1; j++)
                    {
                        logoPixels[new Vector2Int(i, j)] = logoPixel;
                    }
                }
            }

            pacmanLogo.AddComponent(new TransformComponent(20, 1));
            pacmanLogo.AddComponent(new ConsoleSprite(logoPixels));

            selector = new GameObject("Selector");
            char[,] selectorSprite = { { '-' }, { '-' }, { '>' }, };

            MapComponent map = new MapComponent(28, 31);
            MoveComponent selectorMovement = new MoveComponent();

            selector.AddComponent(keyReader);
            selector.AddComponent(new TransformComponent(2, 25));
            selector.AddComponent(selectorMovement);
            selector.AddComponent(map);
            selector.AddComponent(new ConsoleSprite(selectorSprite,
                                                  ConsoleColor.White,
                                                  ConsoleColor.DarkBlue));

            // Adds a movement behaviour
            selectorMovement.AddMovementBehaviour(
                                    new SelectorMovementBehaviour(
                                    selector));

            ////////////////////////////////////////////////////////////////////

            startText = new GameObject("Start Game");
            startText.AddComponent(new TransformComponent(6, 25));

            RenderableStringComponent renderStartGame
                = new RenderableStringComponent(
                    () => "START GAME",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White, ConsoleColor.DarkBlue);

            startText.AddComponent(renderStartGame);

            ////////////////////////////////////////////////////////////////////

            quitText = new GameObject("Quit");
            quitText.AddComponent(new TransformComponent(6, 27));

            RenderableStringComponent renderQuit
                = new RenderableStringComponent(
                    () => "QUIT",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White, ConsoleColor.DarkBlue);

            quitText.AddComponent(renderQuit);

            ////////////////////////////////////////////////////////////////////

            controlsText = new GameObject("ControlsText");
            controlsText.AddComponent(new TransformComponent(70, 23));

            RenderableStringComponent renderControls
                = new RenderableStringComponent(
                    () => "Controls:",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White, ConsoleColor.DarkBlue);

            controlsText.AddComponent(renderControls);

            ////////////////////////////////////////////////////////////////////

            movementText = new GameObject("MovementText");
            movementText.AddComponent(new TransformComponent(54, 25));

            RenderableStringComponent renderMovement
                = new RenderableStringComponent(
                    () => "Movement Keys: W, A, S, D",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White, ConsoleColor.DarkBlue);

            movementText.AddComponent(renderMovement);

            ////////////////////////////////////////////////////////////////////

            actionsText = new GameObject("ActionsText");
            actionsText.AddComponent(new TransformComponent(50, 27));

            RenderableStringComponent renderActions
                = new RenderableStringComponent(
                    () => "Confirm: Enter , Quit: Escape",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White, ConsoleColor.DarkBlue);

            actionsText.AddComponent(renderActions);
        }

        /// <summary>
        /// Creates pacman sprite
        /// </summary>
        /// <param name="testList"></param>
        private void CreatePacmanSprite(ICollection<Vector2Int[]> testList)
        {
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(14,0),
                                new Vector2Int(26,0)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(10,1),
                                new Vector2Int(30,1)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(8,2),
                                new Vector2Int(32,2)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(6,3),
                                new Vector2Int(34,3)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(4,4),
                                new Vector2Int(36,4)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2,5),
                                new Vector2Int(38,5)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2,6),
                                new Vector2Int(36,6)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,7),
                                new Vector2Int(33,7)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,8),
                                new Vector2Int(30,8)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,9),
                                new Vector2Int(28,9)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,10),
                                new Vector2Int(24,10)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,11),
                                new Vector2Int(28,11)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0,12),
                                new Vector2Int(32,12)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2,13),
                                new Vector2Int(36,13)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2,14),
                                new Vector2Int(38,14)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(4,15),
                                new Vector2Int(36,15)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(6,16),
                                new Vector2Int(34,16)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(8,17),
                                new Vector2Int(32,17)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(10,18),
                                new Vector2Int(30,18)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(12,19),
                                new Vector2Int(26,19)
                            });
        }

        /// <summary>
        /// Adds game objects to scene
        /// </summary>
        private void AddGameObjectsToScene()
        {
            scene.AddGameObject(selector);
        }

        /// <summary>
        /// Adds game objects to renderer
        /// </summary>
        private void AddGameObjectsToRenderer()
        {
            consoleRenderer.AddGameObject(selector);
            consoleRenderer.AddGameObject(startText);
            consoleRenderer.AddGameObject(quitText);
            consoleRenderer.AddGameObject(pacmanLogo);
            consoleRenderer.AddGameObject(controlsText);
            consoleRenderer.AddGameObject(movementText);
            consoleRenderer.AddGameObject(actionsText);
        }
    }
}
