using System;
using System.Collections.Generic;
using Pacman.Components;
using Pacman.GameRelated;
using System.IO;

namespace Pacman
{
    class MenuCreation
    {
        private const byte XSIZE = 28;
        private const byte YSIZE = 39;

        private readonly ConsolePixel backgroundPixel;
        private readonly ConsoleRenderer consoleRenderer;
        public Scene MenuScene { get; }
        private readonly KeyReaderComponent keyReader;

        // UI
        private GameObject selector;
        private GameObject startText;
        private GameObject quitText;
        private GameObject highScoreText;
        private GameObject highScoreNumberText;
        private GameObject controlsText;
        private GameObject movementText;
        private GameObject actionsText;
        private GameObject sceneChanger;
        private GameObject rules;
        private GameObject rule1;
        private GameObject rule2;
        private GameObject rule3;
        private GameObject icons;
        private GameObject icon1;
        private GameObject icon2;
        private GameObject icon3;
        private GameObject icon4;
        private GameObject icon5;

        private GameObject pacmanLogo;

        // Scene
        private SceneHandler sceneHandler;
        private GameObject newLevelCreator;

        public MenuCreation(KeyReaderComponent keyReader,
                            SceneHandler sceneHandler)
        {
            this.sceneHandler = sceneHandler;

            backgroundPixel = new ConsolePixel(' ', ConsoleColor.White,
                                                ConsoleColor.DarkBlue);

            consoleRenderer = new ConsoleRenderer(XSIZE * 3, YSIZE,
                                            backgroundPixel,
                                            "Console Renderer");

            this.keyReader = keyReader;

            MenuScene = new Scene();
        }

        public void GenerateScene()
        {
            CreateGameObjects();

            AddGameObjectsToScene();

            AddGameObjectsToRenderer();

            MenuScene.AddGameObject(consoleRenderer);
        }

        private void CreateGameObjects()
        {
            keyReader.QuitKeys.Add(ConsoleKey.Enter);
            sceneChanger = new GameObject("Scene Changer");
            SceneChangerComponent sceneChangerComponent =
                                new SceneChangerComponent(sceneHandler);

            sceneChanger.AddComponent(sceneChangerComponent);
            sceneChangerComponent.SceneToLoad = "LevelScene";

            ////////////////////////////////////////////////////////////////////

            newLevelCreator = new GameObject("New Level Creator");

            CreateNewLevelComponent createLevel =
                new CreateNewLevelComponent(keyReader, sceneHandler);

            newLevelCreator.AddComponent(createLevel);

            ////////////////////////////////////////////////////////////////////

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

            ////////////////////////////////////////////////////////////////////

            selector = new GameObject("Selector");
            char[,] selectorSprite = { { '-' }, { '-' }, { '>' }, };

            MapComponent map = new MapComponent(XSIZE, YSIZE);
            MoveComponent selectorMovement = new MoveComponent();

            selector.AddComponent(keyReader);
            selector.AddComponent(new TransformComponent(2, 34));
            selector.AddComponent(selectorMovement);
            selector.AddComponent(map);
            selector.AddComponent(new ConsoleSprite(
                                    selectorSprite,
                                    ConsoleColor.White,
                                    ConsoleColor.DarkBlue));
            SelectorMovementBehaviour selectorMovementBehaviour =
                                 new SelectorMovementBehaviour(
                                        selector,
                                        sceneChanger.
                                        GetComponent<SceneChangerComponent>());

            // Adds a movement behaviour
            selectorMovement.AddMovementBehaviour(selectorMovementBehaviour);

            ////////////////////////////////////////////////////////////////////

            startText = new GameObject("Start Game");
            startText.AddComponent(new TransformComponent(6, 34));

            RenderableStringComponent renderStartGame
                = new RenderableStringComponent(
                    () => "START GAME",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.Yellow,
                    ConsoleColor.DarkBlue);

            startText.AddComponent(renderStartGame);

            ////////////////////////////////////////////////////////////////////

            quitText = new GameObject("Quit");
            quitText.AddComponent(new TransformComponent(6, 36));

            RenderableStringComponent renderQuit
                = new RenderableStringComponent(
                    () => "QUIT",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            quitText.AddComponent(renderQuit);

            ////////////////////////////////////////////////////////////////////

            highScoreText = new GameObject("HighScore Text");

            highScoreText.AddComponent(new TransformComponent(6, 28));

            RenderableStringComponent renderHighScoreText
                = new RenderableStringComponent(
                    () => $"HighScore:",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            highScoreText.AddComponent(renderHighScoreText);

            ////////////////////////////////////////////////////////////////////

            highScoreNumberText = new GameObject("HighScore Number Text");

            HighScoreComponent highScoreComponent =
                new HighScoreComponent();

            RenderableStringComponent renderHighScoreNumberText
                = new RenderableStringComponent(
                    () => $"{highScoreComponent.HighScore}",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            highScoreNumberText.AddComponent(new TransformComponent(6, 30));
            highScoreNumberText.AddComponent(renderHighScoreNumberText);
            highScoreNumberText.AddComponent(highScoreComponent);

            ////////////////////////////////////////////////////////////////////

            controlsText = new GameObject("ControlsText");
            controlsText.AddComponent(new TransformComponent(70, 19));

            RenderableStringComponent renderControls
                = new RenderableStringComponent(
                    () => "Controls:",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            controlsText.AddComponent(renderControls);

            ////////////////////////////////////////////////////////////////////

            movementText = new GameObject("MovementText");
            movementText.AddComponent(new TransformComponent(54, 21));

            RenderableStringComponent renderMovement
                = new RenderableStringComponent(
                    () => "Movement Keys: W, A, S, D",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            movementText.AddComponent(renderMovement);

            ////////////////////////////////////////////////////////////////////

            actionsText = new GameObject("ActionsText");
            actionsText.AddComponent(new TransformComponent(50, 22));

            RenderableStringComponent renderActions
                = new RenderableStringComponent(
                    () => "Confirm: Enter , Quit: Escape",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            actionsText.AddComponent(renderActions);

            ////////////////////////////////////////////////////////////////////

            rules = new GameObject("RulesText");
            rules.AddComponent(new TransformComponent(73, 24));

            RenderableStringComponent renderRules
                = new RenderableStringComponent(
                    () => "Rules:",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            rules.AddComponent(renderRules);

            ////////////////////////////////////////////////////////////////////

            rule1 = new GameObject("Rule1");
            rule1.AddComponent(new TransformComponent(52, 26));

            RenderableStringComponent renderRule1
                = new RenderableStringComponent(
                    () => "Pacman must run from ghosts",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            rule1.AddComponent(renderRule1);

            ////////////////////////////////////////////////////////////////////

            rule2 = new GameObject("Rule2");
            rule2.AddComponent(new TransformComponent(33, 27));

            RenderableStringComponent renderRule2
                = new RenderableStringComponent(
                    () => "Pacman can pick power pills and eat the ghosts",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            rule2.AddComponent(renderRule2);

            ////////////////////////////////////////////////////////////////////

            rule3 = new GameObject("Rule3");
            rule3.AddComponent(new TransformComponent(37, 28));

            RenderableStringComponent renderRule3
                = new RenderableStringComponent(
                    () => "Pacman must eat every food to end the game",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            rule3.AddComponent(renderRule3);

            ////////////////////////////////////////////////////////////////////

            icons = new GameObject("Icons");
            icons.AddComponent(new TransformComponent(73, 30));

            RenderableStringComponent renderIcons
                = new RenderableStringComponent(
                    () => "Icons:",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icons.AddComponent(renderIcons);

            ////////////////////////////////////////////////////////////////////

            icon1 = new GameObject("Icon1");
            icon1.AddComponent(new TransformComponent(68, 32));

            RenderableStringComponent renderIcon1
                = new RenderableStringComponent(
                    () => "P -> Pacman",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icon1.AddComponent(renderIcon1);

            ////////////////////////////////////////////////////////////////////

            icon2 = new GameObject("Icon2");
            icon2.AddComponent(new TransformComponent(54, 33));

            RenderableStringComponent renderIcon2
                = new RenderableStringComponent(
                    () => "Colored Squares -> Ghosts",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icon2.AddComponent(renderIcon2);

            ////////////////////////////////////////////////////////////////////

            icon3 = new GameObject("Icon3");
            icon3.AddComponent(new TransformComponent(68, 34));

            RenderableStringComponent renderIcon3
                = new RenderableStringComponent(
                    () => "F -> Fruits",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icon3.AddComponent(renderIcon3);

            ////////////////////////////////////////////////////////////////////

            icon4 = new GameObject("Icon4");
            icon4.AddComponent(new TransformComponent(62, 35));

            RenderableStringComponent renderIcon4
                = new RenderableStringComponent(
                    () => "PUP -> Power Pill",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icon4.AddComponent(renderIcon4);

            ////////////////////////////////////////////////////////////////////

            icon5 = new GameObject("Icon5");
            icon5.AddComponent(new TransformComponent(70, 36));

            RenderableStringComponent renderIcon5
                = new RenderableStringComponent(
                    () => ". -> Food",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            icon5.AddComponent(renderIcon5);
        }

        /// <summary>
        /// Creates pacman sprite
        /// </summary>
        /// <param name="testList"></param>
        private void CreatePacmanSprite(ICollection<Vector2Int[]> testList)
        {
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(14, 0),
                                new Vector2Int(26, 0)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(10, 1),
                                new Vector2Int(30, 1)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(8, 2),
                                new Vector2Int(32, 2)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(6, 3),
                                new Vector2Int(34, 3)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(4, 4),
                                new Vector2Int(36, 4)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2, 5),
                                new Vector2Int(38, 5)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2, 6),
                                new Vector2Int(36, 6)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 7),
                                new Vector2Int(33, 7)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 8),
                                new Vector2Int(30, 8)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 9),
                                new Vector2Int(28, 9)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 10),
                                new Vector2Int(24, 10)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 11),
                                new Vector2Int(28, 11)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(0, 12),
                                new Vector2Int(32, 12)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2, 13),
                                new Vector2Int(36, 13)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(2, 14),
                                new Vector2Int(38, 14)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(4, 15),
                                new Vector2Int(36, 15)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(6, 16),
                                new Vector2Int(34, 16)
                            });

            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(8, 17),
                                new Vector2Int(32, 17)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(10, 18),
                                new Vector2Int(30, 18)
                            });
            testList.Add(new Vector2Int[]
                            {
                                new Vector2Int(12, 19),
                                new Vector2Int(26, 19)
                            });
        }

        /// <summary>
        /// Adds game objects to MenuScene
        /// </summary>
        private void AddGameObjectsToScene()
        {
            MenuScene.AddGameObject(selector);
            MenuScene.AddGameObject(sceneChanger);
            MenuScene.AddGameObject(newLevelCreator);
            MenuScene.AddGameObject(highScoreNumberText);
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
            consoleRenderer.AddGameObject(highScoreText);
            consoleRenderer.AddGameObject(highScoreNumberText);
            consoleRenderer.AddGameObject(controlsText);
            consoleRenderer.AddGameObject(movementText);
            consoleRenderer.AddGameObject(actionsText);
            consoleRenderer.AddGameObject(rules);
            consoleRenderer.AddGameObject(rule1);
            consoleRenderer.AddGameObject(rule2);
            consoleRenderer.AddGameObject(rule3);
            consoleRenderer.AddGameObject(icons);
            consoleRenderer.AddGameObject(icon1);
            consoleRenderer.AddGameObject(icon2);
            consoleRenderer.AddGameObject(icon3);
            consoleRenderer.AddGameObject(icon4);
            consoleRenderer.AddGameObject(icon5);
        }
    }
}
