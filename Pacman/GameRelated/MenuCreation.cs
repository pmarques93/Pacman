using System;

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

        // Text to render
        private GameObject selector;
        private GameObject startText;
        private GameObject quitText;

        public MenuCreation()
        {
            backgroundPixel = new ConsolePixel(' ', ConsoleColor.White,
                                                ConsoleColor.DarkBlue);

            consoleRenderer = new ConsoleRenderer(XSIZE * 3, YSIZE,
                                            backgroundPixel,
                                            "Console Renderer");

            keyReader = new KeyReaderComponent(ConsoleKey.Enter);

            scene = new Scene(XSIZE, YSIZE, keyReader);
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
        }

        /// <summary>
        /// Adds game objects to scene
        /// </summary>
        private void AddGameObjectsToScene()
        {
            scene.AddGameObject(selector);
            scene.AddGameObject(startText);
            scene.AddGameObject(quitText);
        }

        /// <summary>
        /// Adds game objects to renderer
        /// </summary>
        private void AddGameObjectsToRenderer()
        {
            consoleRenderer.AddGameObject(selector);
            consoleRenderer.AddGameObject(startText);
            consoleRenderer.AddGameObject(quitText);
        }

        private void CreateLevel()
        {
            LevelCreation levelCreation = new LevelCreation();
            levelCreation.Create();
        }
    }
}
