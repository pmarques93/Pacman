﻿using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using Pacman.Components;
using Pacman.MovementBehaviours.ChaseBehaviour;
using Pacman.Collisions;
using Pacman.ConsoleRender;
using Pacman.MovementBehaviours;
using Pacman.FileWR;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Class responsible for creating pacman level.
    /// </summary>
    public class LevelCreation
    {
        private const byte XSIZE = 28;
        private const byte YSIZE = 31;

        /// <summary>
        /// Gets levelScene.
        /// </summary>
        public Scene LevelScene { get; }

        // Game related components and variables -- READONLY
        private readonly SceneHandler sceneHandler;
        private readonly Random random;
        private readonly ConsoleRenderer consoleRenderer;
        private readonly Collision collisions;
        private readonly MapComponent map;

        // Pacman -- READONLY
        private readonly KeyReaderComponent pacmanKeyReader;

        // Foods -- READONLY
        private readonly GameObject[] allFoods;

        // Powerpills -- READONLY
        private readonly GameObject[] allPowerPills;

        // UI -- READONLY
        private readonly Score score;
        private readonly LivesComponent lives;

        // Game related components and variables
        private GhostBehaviourHandler gameState;
        private GameObject gameOverCheck;
        private GameOverCheckComponent gameOverCheckComponent;

        // Pacman
        private GameObject pacman;
        private MapTransformComponent pacmanMapTransform;
        private PacmanMovementBehaviour pacmanMovementBehaviour;

        // Ghosts
        private GameObject spawner;
        private GameObject pinky;
        private GameObject blinky;
        private GameObject inky;
        private GameObject clyde;

        // Fruits
        private GameObject fruitSpawner;
        private FruitSpawnerComponent fruitSpawnerComponent;
        private GameObject[] allFruits;
        private uint fruitName;
        private uint fruitSlot;

        // Walls
        private GameObject walls;

        // UI
        private GameObject scoreText;
        private GameObject highScoreText;
        private GameObject livesText;
        private GameObject sceneChanger;

        /// <summary>
        /// Constructor for LevelCreation.
        /// </summary>
        /// <param name="keyReader">Reference to keyReader.</param>
        /// <param name="sceneHandler">Reference to sceneHandler.</param>
        /// <param name="random">Reference to Random.</param>
        public LevelCreation(KeyReaderComponent keyReader, SceneHandler sceneHandler, Random random)
        {
            this.sceneHandler = sceneHandler;

            ConsolePixel backgroundPixel = new ConsolePixel(
                                ' ',
                                ConsoleColor.White,
                                ConsoleColor.DarkBlue);

            map = new MapComponent(XSIZE, YSIZE);

            byte numberOfLives = 3;
            lives = new LivesComponent(numberOfLives);

            collisions = new Collision(map);

            pacmanKeyReader = keyReader;

            consoleRenderer = new ConsoleRenderer(
                                    XSIZE * 3,
                                    YSIZE + 3,
                                    backgroundPixel,
                                    collisions,
                                    "Console Renderer");

            LevelScene = new Scene();

            score = new Score(collisions);

            allFoods = new GameObject[246];

            allPowerPills = new GameObject[4];

            allFruits = new GameObject[1];

            this.random = random;
        }

        /// <summary>
        /// Method responsible for creating pacman level.
        /// </summary>
        public void GenerateScene()
        {
            sceneChanger = new GameObject("SceneChanger");
            SceneChangerComponent sceneChangerComponent =
                     new SceneChangerComponent(sceneHandler);
            sceneChanger.AddComponent(sceneChangerComponent);
            pacmanKeyReader.EscapePressed += GameOver;

            spawner = new GameObject("Spawner");
            SpawnerComponent spawnerComponent = new SpawnerComponent();
            spawner.AddComponent(spawnerComponent);

            // PACMAN
            PacmanCreation(map);

            // GHOST
            GhostCreation(map);

            gameState = new GhostBehaviourHandler(
                collisions,
                pacman,
                new List<GameObject>()
                { blinky, pinky, inky, clyde },
                map,
                random,
                pacmanMovementBehaviour);

            // WALLS
            WallCreation(map);

            for (int i = 10; i < 18; i++)
            {
                for (int j = 12; j < 16; j++)
                {
                    map.Map[i, j].Collider.Type |= Cell.GhostHouse;
                }
            }

            map.Map[13, 11].Collider.Type |= Cell.GhostHouseExit;
            map.Map[14, 11].Collider.Type |= Cell.GhostHouseExit;

            // FOOD
            FoodCreation();

            // POWERPILLS
            PowerPillsCreation();

            // FRUITS
            FruitSpawnerCreation();

            // UI
            UICreation();

            // GAME OVER CHECK
            GameOverCheckCreation();

            // Add Gameobjects to collision check
            AddGameObjectsToCollisionCheck();

            // Add GameObjects to the LevelScene
            AddGameObjectsToScene();

            // Add GameObjects to the renderer
            AddGameObjectsToRender();

            gameState.GhostChaseCollision += ResetPositions;

            // Add renderer to the LevelScene
            LevelScene.AddGameObject(consoleRenderer);
        }

        /// <summary>
        /// Creates GameOverCheck. Used to check if there are foods left.
        /// </summary>
        private void GameOverCheckCreation()
        {
            gameOverCheck = gameOverCheck = new GameObject("Game Over Check");

            gameOverCheckComponent =
                new GameOverCheckComponent(240, collisions);

            gameOverCheck.AddComponent(gameOverCheckComponent);

            gameOverCheckComponent.NoFoodsLeft += GameOver;
        }

        /// <summary>
        /// Happens when the game is over.
        /// Writes high score and terminates current scene.
        /// </summary>
        private void GameOver()
        {
            // Writes a new high score
            if (File.Exists(FilePath.Highscore))
            {
                FileReader fileReader = new FileReader(FilePath.Highscore);
                uint highScore = fileReader.ReadHighScore();

                uint.TryParse(score.GetScore, out uint tempScore);
                if (tempScore > highScore)
                {
                    FileWriter fileWriter = new FileWriter(FilePath.Highscore);
                    fileWriter.CreateHighScoreTXT(tempScore);
                }
            }
            else
            {
                FileWriter fileWriter = new FileWriter(FilePath.Highscore);
                uint.TryParse(score.GetScore, out uint tempScore);
                fileWriter.CreateHighScoreTXT(tempScore);
            }

            pacmanKeyReader.QuitKeys.Clear();
            pacmanKeyReader.QuitKeys.Add(ConsoleKey.Enter);

            // Loads menu scene
            SceneChangerComponent sceneChangerComponent =
                sceneChanger.GetComponent<SceneChangerComponent>();
            sceneChangerComponent.SceneToLoad = "MenuScene";
            sceneChangerComponent.SceneHandler.CurrentScene.Unload = true;
            sceneChangerComponent.ChangeScene();
            sceneChangerComponent.SceneHandler.CurrentScene.Unload = false;

            // Removes level scene (deletes the scene)
            sceneHandler.RemoveScene("LevelScene");
        }

        /// <summary>
        /// Resets pacman position.
        /// </summary>
        private void ResetPositions()
        {
            lives.Lives--;

            if (lives.Lives == 0)
                GameOver();

            map.Map[
                pacmanMapTransform.Position.X, pacmanMapTransform.Position.Y].
                Collider.Type &= ~Cell.Pacman;
            pacman.GetComponent<TransformComponent>().Position =
                new Vector2Int(42, 23);
            pacmanMapTransform.Position = new Vector2Int(14, 23);

            MapTransformComponent pinkyMapTransform =
                pinky.GetComponent<MapTransformComponent>();
            map.Map[
                pinkyMapTransform.Position.X, pinkyMapTransform.Position.Y].
                Collider.Type &= ~Cell.Ghost;
            pinky.GetComponent<TransformComponent>().Position =
                new Vector2Int(36, 14);
            pinky.GetComponent<MoveComponent>().MovementState =
                MovementState.OnGhostHouse;
            pinkyMapTransform.Position = new Vector2Int(12, 14);
            pinkyMapTransform.Direction = Direction.Up;

            MapTransformComponent blinkyMapTransform =
                blinky.GetComponent<MapTransformComponent>();
            map.Map[
                blinkyMapTransform.Position.X, blinkyMapTransform.Position.Y].
                Collider.Type &= ~Cell.Ghost;
            blinky.GetComponent<TransformComponent>().Position =
                new Vector2Int(33, 13);
            blinky.GetComponent<MoveComponent>().MovementState =
                MovementState.OnGhostHouse;
            blinkyMapTransform.Position = new Vector2Int(11, 13);
            blinkyMapTransform.Direction = Direction.Up;

            MapTransformComponent clydeMapTransform =
                clyde.GetComponent<MapTransformComponent>();
            map.Map[
                clydeMapTransform.Position.X, clydeMapTransform.Position.Y].
                Collider.Type &= ~Cell.Ghost;
            clyde.GetComponent<TransformComponent>().Position =
                new Vector2Int(42, 14);
            clyde.GetComponent<MoveComponent>().MovementState =
                MovementState.OnGhostHouse;
            clydeMapTransform.Position = new Vector2Int(14, 14);
            clydeMapTransform.Direction = Direction.Up;

            MapTransformComponent inkyMapTransform =
                inky.GetComponent<MapTransformComponent>();
            map.Map[
                inkyMapTransform.Position.X, inkyMapTransform.Position.Y].
                Collider.Type &= ~Cell.Ghost;
            inky.GetComponent<TransformComponent>().Position =
                new Vector2Int(45, 14);
            inky.GetComponent<MoveComponent>().MovementState =
                MovementState.OnGhostHouse;
            inkyMapTransform.Position = new Vector2Int(15, 14);
            inkyMapTransform.Direction = Direction.Up;
        }

        /// <summary>
        /// Creates pacman.
        /// </summary>
        /// <param name="map">Map reference to the game map.</param>
        private void PacmanCreation(MapComponent map)
        {
            char[,] pacmanSprite = { { ' ' }, { 'P' }, { ' ' }, };
            pacman = new GameObject("Pacman");

            // Components ///////////////////////////////////
            TransformComponent pacmanTransform = new TransformComponent(42, 23);
            pacmanMapTransform = new MapTransformComponent(14, 23);
            map.Map[
                pacmanMapTransform.Position.X, pacmanMapTransform.Position.Y].
                Collider.Type |= Cell.Pacman;
            MoveComponent pacmanMovement = new MoveComponent();
            ColliderComponent pacmanCollider =
                new ColliderComponent(Cell.Pacman);

            pacman.AddComponent(pacmanKeyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMapTransform);
            pacman.AddComponent(pacmanMovement);
            pacman.AddComponent(pacmanCollider);
            pacman.AddComponent(map);

            // Adds a movement behaviour
            pacmanMovementBehaviour = new PacmanMovementBehaviour(
                                        pacman, pacmanMapTransform, 3);

            pacmanMovement.AddMovementBehaviour(pacmanMovementBehaviour);

            pacman.AddComponent(new ConsoleSprite(
                pacmanSprite,
                ConsoleColor.Yellow,
                ConsoleColor.DarkBlue));

            spawner.GetComponent<SpawnerComponent>().
                        AddGameObject(new SpawnStruct(
                                        pacmanTransform.Position,
                                        pacmanMapTransform.Position,
                                        pacman));
        }

        /// <summary>
        /// Creates ghosts.
        /// </summary>
        /// <param name="map">Map reference to the game map.</param>
        private void GhostCreation(MapComponent map)
        {
            char[,] pinkySprite =
            {
                { ' ' },
                { 'P' },
                { ' ' },
            };
            pinky = new GameObject("pinky");
            TransformComponent pinkyTransform = new TransformComponent(36, 14);
            MapTransformComponent pinkyMapTransform =
                new MapTransformComponent(12, 14);
            MoveComponent pinkyMovement = new MoveComponent();
            pinkyMovement.MovementState = MovementState.OnGhostHouse;
            ColliderComponent pinkyCollider = new ColliderComponent(Cell.Ghost);

            pinky.AddComponent(pinkyTransform);
            pinky.AddComponent(pinkyMapTransform);
            pinky.AddComponent(pinkyMovement);
            pinky.AddComponent(pinkyCollider);
            pinky.AddComponent(map);

            // Adds a movement behaviour
            pinkyMovement.AddMovementBehaviour(new PinkyChaseBehaviour(
                                                pacmanMovementBehaviour,
                                                pinky,
                                                map,
                                                pacmanMapTransform,
                                                pinkyMapTransform,
                                                3));

            pinky.AddComponent(new ConsoleSprite(
                pinkySprite,
                ConsoleColor.White,
                ConsoleColor.Magenta));

            ////////////////////////////////////////////////////////////////////

            char[,] blinkySprite =
            {
                { ' ' },
                { 'B' },
                { ' ' },
            };
            blinky = new GameObject("blinky");
            TransformComponent blinkyTransform =
                                            new TransformComponent(33, 13);
            MapTransformComponent blinkyMapTransform =
                                            new MapTransformComponent(11, 13);
            MoveComponent blinkyMovement = new MoveComponent();
            ColliderComponent blinkyCollider =
                                            new ColliderComponent(Cell.Ghost);

            blinky.AddComponent(blinkyTransform);
            blinky.AddComponent(blinkyMapTransform);
            blinky.AddComponent(blinkyMovement);
            blinky.AddComponent(blinkyCollider);
            blinky.AddComponent(map);

            // Adds a movement behaviour
            blinkyMovement.AddMovementBehaviour(new BlinkyChaseBehaviour(
                                                blinky,
                                                map,
                                                pacmanMapTransform,
                                                blinkyMapTransform,
                                                3));

            blinky.AddComponent(new ConsoleSprite(
                blinkySprite,
                ConsoleColor.White,
                ConsoleColor.Red));

            blinkyMovement.MovementState = MovementState.OnGhostHouse;

            ////////////////////////////////////////////////////////////////////

            char[,] inkySprite =
            {
                { ' ' },
                { 'I' },
                { ' ' },
            };
            inky = new GameObject("inky");
            TransformComponent inkyTransform = new TransformComponent(45, 14);
            MapTransformComponent inkyMapTransform =
                                            new MapTransformComponent(15, 14);
            MoveComponent inkyMovement = new MoveComponent();
            ColliderComponent inkyCollider = new ColliderComponent(Cell.Ghost);
            inkyMovement.MovementState = MovementState.OnGhostHouse;
            inky.AddComponent(inkyTransform);
            inky.AddComponent(inkyMapTransform);
            inky.AddComponent(inkyMovement);
            inky.AddComponent(inkyCollider);
            inky.AddComponent(map);

            // Adds a movement behaviour
            inkyMovement.AddMovementBehaviour(
                            new InkyChaseBehaviour(
                                    pacmanMovementBehaviour,
                                    map,
                                    pacmanMapTransform,
                                    blinkyMapTransform,
                                    inky,
                                    inkyMapTransform,
                                    3));

            inky.AddComponent(new ConsoleSprite(
                                inkySprite,
                                ConsoleColor.White,
                                ConsoleColor.Blue));

            ////////////////////////////////////////////////////////////////////

            char[,] clydeSprite =
            {
                { ' ' },
                { 'C' },
                { ' ' },
            };
            clyde = new GameObject("clyde");
            TransformComponent clydeTransform = new TransformComponent(42, 14);
            MapTransformComponent clydeMapTransform =
                                            new MapTransformComponent(14, 14);
            MoveComponent clydeMovement = new MoveComponent();
            ColliderComponent clydeCollider =
                                            new ColliderComponent(Cell.Ghost);

            clyde.AddComponent(clydeTransform);
            clyde.AddComponent(clydeMapTransform);
            clyde.AddComponent(clydeMovement);
            clyde.AddComponent(clydeCollider);
            clyde.AddComponent(map);

            // Adds a movement behaviour
            clydeMovement.AddMovementBehaviour(new ClydeChaseBehaviour(
                                                pacmanMovementBehaviour,
                                                clyde,
                                                map,
                                                pacmanMapTransform,
                                                clydeMapTransform,
                                                3));

            clyde.AddComponent(new ConsoleSprite(
                                clydeSprite,
                                ConsoleColor.DarkBlue,
                                ConsoleColor.DarkYellow));
        }

        /// <summary>
        /// Creates foods.
        /// </summary>
        private void FoodCreation()
        {
            allFoods[0] = new GameObject("Food0");
            char[,] food0Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food0Transform = new TransformComponent(3, 1);
            MapTransformComponent food0MapTransform =
                new MapTransformComponent(1, 1);
            map.Map[
                food0MapTransform.Position.X, food0MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food0Collider = new ColliderComponent(Cell.Food);
            allFoods[0].AddComponent(food0Transform);
            allFoods[0].AddComponent(food0MapTransform);
            allFoods[0].AddComponent(food0Collider);
            allFoods[0].AddComponent(new ConsoleSprite(
                food0Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[1] = new GameObject("Food1");
            char[,] food1Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food1Transform = new TransformComponent(6, 1);
            MapTransformComponent food1MapTransform =
                new MapTransformComponent(2, 1);
            map.Map[
                food1MapTransform.Position.X, food1MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food1Collider = new ColliderComponent(Cell.Food);
            allFoods[1].AddComponent(food1Transform);
            allFoods[1].AddComponent(food1MapTransform);
            allFoods[1].AddComponent(food1Collider);
            allFoods[1].AddComponent(new ConsoleSprite(
                food1Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[2] = new GameObject("Food2");
            char[,] food2Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food2Transform = new TransformComponent(9, 1);
            MapTransformComponent food2MapTransform =
                new MapTransformComponent(3, 1);
            map.Map[
                food2MapTransform.Position.X, food2MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food2Collider = new ColliderComponent(Cell.Food);
            allFoods[2].AddComponent(food2Transform);
            allFoods[2].AddComponent(food2MapTransform);
            allFoods[2].AddComponent(food2Collider);
            allFoods[2].AddComponent(new ConsoleSprite(
                food2Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[3] = new GameObject("Food3");
            char[,] food3Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food3Transform = new TransformComponent(12, 1);
            MapTransformComponent food3MapTransform =
                new MapTransformComponent(4, 1);
            map.Map[
                food3MapTransform.Position.X, food3MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food3Collider = new ColliderComponent(Cell.Food);
            allFoods[3].AddComponent(food3Transform);
            allFoods[3].AddComponent(food3MapTransform);
            allFoods[3].AddComponent(food3Collider);
            allFoods[3].AddComponent(new ConsoleSprite(
                food3Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[4] = new GameObject("Food4");
            char[,] food4Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food4Transform = new TransformComponent(15, 1);
            MapTransformComponent food4MapTransform =
                new MapTransformComponent(5, 1);
            map.Map[
                food4MapTransform.Position.X, food4MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food4Collider = new ColliderComponent(Cell.Food);
            allFoods[4].AddComponent(food4Transform);
            allFoods[4].AddComponent(food4MapTransform);
            allFoods[4].AddComponent(food4Collider);
            allFoods[4].AddComponent(new ConsoleSprite(
                food4Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[5] = new GameObject("Food5");
            char[,] food5Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food5Transform = new TransformComponent(18, 1);
            MapTransformComponent food5MapTransform =
                new MapTransformComponent(6, 1);
            map.Map[
                food5MapTransform.Position.X, food5MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food5Collider = new ColliderComponent(Cell.Food);
            allFoods[5].AddComponent(food5Transform);
            allFoods[5].AddComponent(food5MapTransform);
            allFoods[5].AddComponent(food5Collider);
            allFoods[5].AddComponent(new ConsoleSprite(
                food5Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[6] = new GameObject("Food6");
            char[,] food6Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food6Transform = new TransformComponent(21, 1);
            MapTransformComponent food6MapTransform =
                new MapTransformComponent(7, 1);
            map.Map[
                food6MapTransform.Position.X, food6MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food6Collider = new ColliderComponent(Cell.Food);
            allFoods[6].AddComponent(food6Transform);
            allFoods[6].AddComponent(food6MapTransform);
            allFoods[6].AddComponent(food6Collider);
            allFoods[6].AddComponent(new ConsoleSprite(
                food6Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[7] = new GameObject("Food7");
            char[,] food7Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food7Transform = new TransformComponent(24, 1);
            MapTransformComponent food7MapTransform =
                new MapTransformComponent(8, 1);
            map.Map[
                food7MapTransform.Position.X, food7MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food7Collider = new ColliderComponent(Cell.Food);
            allFoods[7].AddComponent(food7Transform);
            allFoods[7].AddComponent(food7MapTransform);
            allFoods[7].AddComponent(food7Collider);
            allFoods[7].AddComponent(new ConsoleSprite(
                food7Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[8] = new GameObject("Food8");
            char[,] food8Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food8Transform = new TransformComponent(27, 1);
            MapTransformComponent food8MapTransform =
                new MapTransformComponent(9, 1);
            map.Map[
                food8MapTransform.Position.X, food8MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food8Collider = new ColliderComponent(Cell.Food);
            allFoods[8].AddComponent(food8Transform);
            allFoods[8].AddComponent(food8MapTransform);
            allFoods[8].AddComponent(food8Collider);
            allFoods[8].AddComponent(new ConsoleSprite(
                food8Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[9] = new GameObject("Food9");
            char[,] food9Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food9Transform = new TransformComponent(30, 1);
            MapTransformComponent food9MapTransform =
                new MapTransformComponent(10, 1);
            map.Map[
                food9MapTransform.Position.X, food9MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food9Collider = new ColliderComponent(Cell.Food);
            allFoods[9].AddComponent(food9Transform);
            allFoods[9].AddComponent(food9MapTransform);
            allFoods[9].AddComponent(food9Collider);
            allFoods[9].AddComponent(new ConsoleSprite(
                food9Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[10] = new GameObject("Food10");
            char[,] food10Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food10Transform = new TransformComponent(33, 1);
            MapTransformComponent food10MapTransform =
                new MapTransformComponent(11, 1);
            map.Map[
                food10MapTransform.Position.X, food10MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food10Collider = new ColliderComponent(Cell.Food);
            allFoods[10].AddComponent(food10Transform);
            allFoods[10].AddComponent(food10MapTransform);
            allFoods[10].AddComponent(food10Collider);
            allFoods[10].AddComponent(new ConsoleSprite(
                food10Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[11] = new GameObject("Food11");
            char[,] food11Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food11Transform = new TransformComponent(36, 1);
            MapTransformComponent food11MapTransform =
                new MapTransformComponent(12, 1);
            map.Map[
                food11MapTransform.Position.X, food11MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food11Collider = new ColliderComponent(Cell.Food);
            allFoods[11].AddComponent(food11Transform);
            allFoods[11].AddComponent(food11MapTransform);
            allFoods[11].AddComponent(food11Collider);
            allFoods[11].AddComponent(new ConsoleSprite(
                food11Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[12] = new GameObject("Food12");
            char[,] food12Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food12Transform = new TransformComponent(45, 1);
            MapTransformComponent food12MapTransform =
                new MapTransformComponent(15, 1);
            map.Map[
                food12MapTransform.Position.X, food12MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food12Collider = new ColliderComponent(Cell.Food);
            allFoods[12].AddComponent(food12Transform);
            allFoods[12].AddComponent(food12MapTransform);
            allFoods[12].AddComponent(food12Collider);
            allFoods[12].AddComponent(new ConsoleSprite(
                food12Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[13] = new GameObject("Food13");
            char[,] food13Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food13Transform = new TransformComponent(48, 1);
            MapTransformComponent food13MapTransform =
                new MapTransformComponent(16, 1);
            map.Map[
                food13MapTransform.Position.X, food13MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food13Collider = new ColliderComponent(Cell.Food);
            allFoods[13].AddComponent(food13Transform);
            allFoods[13].AddComponent(food13MapTransform);
            allFoods[13].AddComponent(food13Collider);
            allFoods[13].AddComponent(new ConsoleSprite(
                food13Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[14] = new GameObject("Food14");
            char[,] food14Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food14Transform = new TransformComponent(51, 1);
            MapTransformComponent food14MapTransform =
                new MapTransformComponent(17, 1);
            map.Map[
                food14MapTransform.Position.X, food14MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food14Collider = new ColliderComponent(Cell.Food);
            allFoods[14].AddComponent(food14Transform);
            allFoods[14].AddComponent(food14MapTransform);
            allFoods[14].AddComponent(food14Collider);
            allFoods[14].AddComponent(new ConsoleSprite(
                food14Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[15] = new GameObject("Food15");
            char[,] food15Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food15Transform = new TransformComponent(54, 1);
            MapTransformComponent food15MapTransform =
                new MapTransformComponent(18, 1);
            map.Map[
                food15MapTransform.Position.X, food15MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food15Collider = new ColliderComponent(Cell.Food);
            allFoods[15].AddComponent(food15Transform);
            allFoods[15].AddComponent(food15MapTransform);
            allFoods[15].AddComponent(food15Collider);
            allFoods[15].AddComponent(new ConsoleSprite(
                food15Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[16] = new GameObject("Food16");
            char[,] food16Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food16Transform = new TransformComponent(57, 1);
            MapTransformComponent food16MapTransform =
                new MapTransformComponent(19, 1);
            map.Map[
                food16MapTransform.Position.X, food16MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food16Collider = new ColliderComponent(Cell.Food);
            allFoods[16].AddComponent(food16Transform);
            allFoods[16].AddComponent(food16MapTransform);
            allFoods[16].AddComponent(food16Collider);
            allFoods[16].AddComponent(new ConsoleSprite(
                food16Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[17] = new GameObject("Food17");
            char[,] food17Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food17Transform = new TransformComponent(60, 1);
            MapTransformComponent food17MapTransform =
                new MapTransformComponent(20, 1);
            map.Map[
                food17MapTransform.Position.X, food17MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food17Collider = new ColliderComponent(Cell.Food);
            allFoods[17].AddComponent(food17Transform);
            allFoods[17].AddComponent(food17MapTransform);
            allFoods[17].AddComponent(food17Collider);
            allFoods[17].AddComponent(new ConsoleSprite(
                food17Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[18] = new GameObject("Food18");
            char[,] food18Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food18Transform = new TransformComponent(63, 1);
            MapTransformComponent food18MapTransform =
                new MapTransformComponent(21, 1);
            map.Map[
                food18MapTransform.Position.X, food18MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food18Collider = new ColliderComponent(Cell.Food);
            allFoods[18].AddComponent(food18Transform);
            allFoods[18].AddComponent(food18MapTransform);
            allFoods[18].AddComponent(food18Collider);
            allFoods[18].AddComponent(new ConsoleSprite(
                food18Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[19] = new GameObject("Food19");
            char[,] food19Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food19Transform = new TransformComponent(66, 1);
            MapTransformComponent food19MapTransform =
                new MapTransformComponent(22, 1);
            map.Map[
                food19MapTransform.Position.X, food19MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food19Collider = new ColliderComponent(Cell.Food);
            allFoods[19].AddComponent(food19Transform);
            allFoods[19].AddComponent(food19MapTransform);
            allFoods[19].AddComponent(food19Collider);
            allFoods[19].AddComponent(new ConsoleSprite(
                food19Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[20] = new GameObject("Food20");
            char[,] food20Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food20Transform = new TransformComponent(69, 1);
            MapTransformComponent food20MapTransform =
                new MapTransformComponent(23, 1);
            map.Map[
                food20MapTransform.Position.X, food20MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food20Collider = new ColliderComponent(Cell.Food);
            allFoods[20].AddComponent(food20Transform);
            allFoods[20].AddComponent(food20MapTransform);
            allFoods[20].AddComponent(food20Collider);
            allFoods[20].AddComponent(new ConsoleSprite(
                food20Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[21] = new GameObject("Food21");
            char[,] food21Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food21Transform = new TransformComponent(72, 1);
            MapTransformComponent food21MapTransform =
                new MapTransformComponent(24, 1);
            map.Map[
                food21MapTransform.Position.X, food21MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food21Collider = new ColliderComponent(Cell.Food);
            allFoods[21].AddComponent(food21Transform);
            allFoods[21].AddComponent(food21MapTransform);
            allFoods[21].AddComponent(food21Collider);
            allFoods[21].AddComponent(new ConsoleSprite(
                food21Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[22] = new GameObject("Food22");
            char[,] food22Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food22Transform = new TransformComponent(75, 1);
            MapTransformComponent food22MapTransform =
                new MapTransformComponent(25, 1);
            map.Map[
                food22MapTransform.Position.X, food22MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food22Collider = new ColliderComponent(Cell.Food);
            allFoods[22].AddComponent(food22Transform);
            allFoods[22].AddComponent(food22MapTransform);
            allFoods[22].AddComponent(food22Collider);
            allFoods[22].AddComponent(new ConsoleSprite(
                food22Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[23] = new GameObject("Food23");
            char[,] food23Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food23Transform = new TransformComponent(78, 1);
            MapTransformComponent food23MapTransform =
                new MapTransformComponent(26, 1);
            map.Map[
                food23MapTransform.Position.X, food23MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food23Collider = new ColliderComponent(Cell.Food);
            allFoods[23].AddComponent(food23Transform);
            allFoods[23].AddComponent(food23MapTransform);
            allFoods[23].AddComponent(food23Collider);
            allFoods[23].AddComponent(new ConsoleSprite(
                food23Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[24] = new GameObject("Food24");
            char[,] food24Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food24Transform = new TransformComponent(3, 2);
            MapTransformComponent food24MapTransform =
                new MapTransformComponent(1, 2);
            map.Map[
                food24MapTransform.Position.X, food24MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food24Collider = new ColliderComponent(Cell.Food);
            allFoods[24].AddComponent(food24Transform);
            allFoods[24].AddComponent(food24MapTransform);
            allFoods[24].AddComponent(food24Collider);
            allFoods[24].AddComponent(new ConsoleSprite(
                food24Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[25] = new GameObject("Food25");
            char[,] food25Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food25Transform = new TransformComponent(18, 2);
            MapTransformComponent food25MapTransform =
                new MapTransformComponent(6, 2);
            map.Map[
                food25MapTransform.Position.X, food24MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food25Collider = new ColliderComponent(Cell.Food);
            allFoods[25].AddComponent(food25Transform);
            allFoods[25].AddComponent(food25MapTransform);
            allFoods[25].AddComponent(food25Collider);
            allFoods[25].AddComponent(new ConsoleSprite(
                food25Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[26] = new GameObject("Food26");
            char[,] food26Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food26Transform = new TransformComponent(36, 2);
            MapTransformComponent food26MapTransform =
                new MapTransformComponent(12, 2);
            map.Map[
                food26MapTransform.Position.X, food26MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food26Collider = new ColliderComponent(Cell.Food);
            allFoods[26].AddComponent(food26Transform);
            allFoods[26].AddComponent(food26MapTransform);
            allFoods[26].AddComponent(food26Collider);
            allFoods[26].AddComponent(new ConsoleSprite(
                food26Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[27] = new GameObject("Food27");
            char[,] food27Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food27Transform = new TransformComponent(45, 2);
            MapTransformComponent food27MapTransform =
                new MapTransformComponent(15, 2);
            map.Map[
                food27MapTransform.Position.X, food27MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food27Collider = new ColliderComponent(Cell.Food);
            allFoods[27].AddComponent(food27Transform);
            allFoods[27].AddComponent(food27MapTransform);
            allFoods[27].AddComponent(food27Collider);
            allFoods[27].AddComponent(new ConsoleSprite(
                food27Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[28] = new GameObject("Food28");
            char[,] food28Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food28Transform = new TransformComponent(63, 2);
            MapTransformComponent food28MapTransform =
                new MapTransformComponent(21, 2);
            map.Map[
                food28MapTransform.Position.X, food28MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food28Collider = new ColliderComponent(Cell.Food);
            allFoods[28].AddComponent(food28Transform);
            allFoods[28].AddComponent(food28MapTransform);
            allFoods[28].AddComponent(food28Collider);
            allFoods[28].AddComponent(new ConsoleSprite(
                food28Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[29] = new GameObject("Food29");
            char[,] food29Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food29Transform = new TransformComponent(78, 2);
            MapTransformComponent food29MapTransform =
                new MapTransformComponent(26, 2);
            map.Map[
                food29MapTransform.Position.X, food29MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food29Collider = new ColliderComponent(Cell.Food);
            allFoods[29].AddComponent(food29Transform);
            allFoods[29].AddComponent(food29MapTransform);
            allFoods[29].AddComponent(food29Collider);
            allFoods[29].AddComponent(new ConsoleSprite(
                food29Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            // POWER PILL
            allFoods[31] = new GameObject("Food31");
            char[,] food31Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food31Transform = new TransformComponent(18, 3);
            MapTransformComponent food31MapTransform =
                new MapTransformComponent(6, 3);
            map.Map[
                food31MapTransform.Position.X, food31MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food31Collider = new ColliderComponent(Cell.Food);
            allFoods[31].AddComponent(food31Transform);
            allFoods[31].AddComponent(food31MapTransform);
            allFoods[31].AddComponent(food31Collider);
            allFoods[31].AddComponent(new ConsoleSprite(
                food31Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[32] = new GameObject("Food32");
            char[,] food32Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food32Transform = new TransformComponent(36, 3);
            MapTransformComponent food32MapTransform =
                new MapTransformComponent(12, 3);
            map.Map[
                food32MapTransform.Position.X, food32MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food32Collider = new ColliderComponent(Cell.Food);
            allFoods[32].AddComponent(food32Transform);
            allFoods[32].AddComponent(food32MapTransform);
            allFoods[32].AddComponent(food32Collider);
            allFoods[32].AddComponent(new ConsoleSprite(
                food32Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[33] = new GameObject("Food33");
            char[,] food33Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food33Transform = new TransformComponent(45, 3);
            MapTransformComponent food33MapTransform =
                new MapTransformComponent(15, 3);
            map.Map[
                food33MapTransform.Position.X, food33MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food33Collider = new ColliderComponent(Cell.Food);
            allFoods[33].AddComponent(food33Transform);
            allFoods[33].AddComponent(food33MapTransform);
            allFoods[33].AddComponent(food33Collider);
            allFoods[33].AddComponent(new ConsoleSprite(
                food33Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[34] = new GameObject("Food34");
            char[,] food34Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food34Transform = new TransformComponent(63, 3);
            MapTransformComponent food34MapTransform =
                new MapTransformComponent(21, 3);
            map.Map[
                food34MapTransform.Position.X, food34MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food34Collider = new ColliderComponent(Cell.Food);
            allFoods[34].AddComponent(food34Transform);
            allFoods[34].AddComponent(food34MapTransform);
            allFoods[34].AddComponent(food34Collider);
            allFoods[34].AddComponent(new ConsoleSprite(
                food34Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            // POWER PILL
            allFoods[36] = new GameObject("Food36");
            char[,] food36Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food36Transform = new TransformComponent(3, 4);
            MapTransformComponent food36MapTransform =
                new MapTransformComponent(1, 4);
            map.Map[
                food36MapTransform.Position.X, food36MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food36Collider = new ColliderComponent(Cell.Food);
            allFoods[36].AddComponent(food36Transform);
            allFoods[36].AddComponent(food36MapTransform);
            allFoods[36].AddComponent(food36Collider);
            allFoods[36].AddComponent(new ConsoleSprite(
                food36Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[37] = new GameObject("Food37");
            char[,] food37Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food37Transform = new TransformComponent(18, 4);
            MapTransformComponent food37MapTransform =
                new MapTransformComponent(6, 4);
            map.Map[
                food37MapTransform.Position.X, food37MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food37Collider = new ColliderComponent(Cell.Food);
            allFoods[37].AddComponent(food37Transform);
            allFoods[37].AddComponent(food37MapTransform);
            allFoods[37].AddComponent(food37Collider);
            allFoods[37].AddComponent(new ConsoleSprite(
                food37Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[38] = new GameObject("Food38");
            char[,] food38Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food38Transform = new TransformComponent(36, 4);
            MapTransformComponent food38MapTransform =
                new MapTransformComponent(12, 4);
            map.Map[
                food38MapTransform.Position.X, food38MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food38Collider = new ColliderComponent(Cell.Food);
            allFoods[38].AddComponent(food38Transform);
            allFoods[38].AddComponent(food38MapTransform);
            allFoods[38].AddComponent(food38Collider);
            allFoods[38].AddComponent(new ConsoleSprite(
                food38Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[39] = new GameObject("Food39");
            char[,] food39Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food39Transform = new TransformComponent(45, 4);
            MapTransformComponent food39MapTransform =
                new MapTransformComponent(15, 4);
            map.Map[
                food39MapTransform.Position.X, food39MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food39Collider = new ColliderComponent(Cell.Food);
            allFoods[39].AddComponent(food39Transform);
            allFoods[39].AddComponent(food39MapTransform);
            allFoods[39].AddComponent(food39Collider);
            allFoods[39].AddComponent(new ConsoleSprite(
                food39Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[40] = new GameObject("Food40");
            char[,] food40Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food40Transform = new TransformComponent(63, 4);
            MapTransformComponent food40MapTransform =
                new MapTransformComponent(21, 4);
            map.Map[
                food40MapTransform.Position.X, food40MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food40Collider = new ColliderComponent(Cell.Food);
            allFoods[40].AddComponent(food40Transform);
            allFoods[40].AddComponent(food40MapTransform);
            allFoods[40].AddComponent(food40Collider);
            allFoods[40].AddComponent(new ConsoleSprite(
                food40Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[41] = new GameObject("Food41");
            char[,] food41Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food41Transform = new TransformComponent(78, 4);
            MapTransformComponent food41MapTransform =
                new MapTransformComponent(26, 4);
            map.Map[
                food41MapTransform.Position.X, food41MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food41Collider = new ColliderComponent(Cell.Food);
            allFoods[41].AddComponent(food41Transform);
            allFoods[41].AddComponent(food41MapTransform);
            allFoods[41].AddComponent(food41Collider);
            allFoods[41].AddComponent(new ConsoleSprite(
                food41Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[42] = new GameObject("Food42");
            char[,] food42Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food42Transform = new TransformComponent(3, 5);
            MapTransformComponent food42MapTransform =
                new MapTransformComponent(1, 5);
            map.Map[
                food42MapTransform.Position.X, food42MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food42Collider = new ColliderComponent(Cell.Food);
            allFoods[42].AddComponent(food42Transform);
            allFoods[42].AddComponent(food42MapTransform);
            allFoods[42].AddComponent(food42Collider);
            allFoods[42].AddComponent(new ConsoleSprite(
                food42Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[43] = new GameObject("Food43");
            char[,] food43Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food43Transform = new TransformComponent(6, 5);
            MapTransformComponent food43MapTransform =
                new MapTransformComponent(2, 5);
            map.Map[
                food43MapTransform.Position.X, food43MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food43Collider = new ColliderComponent(Cell.Food);
            allFoods[43].AddComponent(food43Transform);
            allFoods[43].AddComponent(food43MapTransform);
            allFoods[43].AddComponent(food43Collider);
            allFoods[43].AddComponent(new ConsoleSprite(
                food43Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[44] = new GameObject("Food44");
            char[,] food44Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food44Transform = new TransformComponent(9, 5);
            MapTransformComponent food44MapTransform =
                new MapTransformComponent(3, 5);
            map.Map[
                food44MapTransform.Position.X, food44MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food44Collider = new ColliderComponent(Cell.Food);
            allFoods[44].AddComponent(food44Transform);
            allFoods[44].AddComponent(food44MapTransform);
            allFoods[44].AddComponent(food44Collider);
            allFoods[44].AddComponent(new ConsoleSprite(
                food44Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[45] = new GameObject("Food45");
            char[,] food45Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food45Transform = new TransformComponent(12, 5);
            MapTransformComponent food45MapTransform =
                new MapTransformComponent(4, 5);
            map.Map[
                food45MapTransform.Position.X, food45MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food45Collider = new ColliderComponent(Cell.Food);
            allFoods[45].AddComponent(food45Transform);
            allFoods[45].AddComponent(food45MapTransform);
            allFoods[45].AddComponent(food45Collider);
            allFoods[45].AddComponent(new ConsoleSprite(
                food45Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[46] = new GameObject("Food46");
            char[,] food46Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food46Transform = new TransformComponent(15, 5);
            MapTransformComponent food46MapTransform =
                new MapTransformComponent(5, 5);
            map.Map[
                food46MapTransform.Position.X, food46MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food46Collider = new ColliderComponent(Cell.Food);
            allFoods[46].AddComponent(food46Transform);
            allFoods[46].AddComponent(food46MapTransform);
            allFoods[46].AddComponent(food46Collider);
            allFoods[46].AddComponent(new ConsoleSprite(
                food46Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[47] = new GameObject("Food47");
            char[,] food47Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food47Transform = new TransformComponent(18, 5);
            MapTransformComponent food47MapTransform =
                new MapTransformComponent(6, 5);
            map.Map[
                food47MapTransform.Position.X, food47MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food47Collider = new ColliderComponent(Cell.Food);
            allFoods[47].AddComponent(food47Transform);
            allFoods[47].AddComponent(food47MapTransform);
            allFoods[47].AddComponent(food47Collider);
            allFoods[47].AddComponent(new ConsoleSprite(
                food47Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[48] = new GameObject("Food48");
            char[,] food48Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food48Transform = new TransformComponent(21, 5);
            MapTransformComponent food48MapTransform =
                new MapTransformComponent(7, 5);
            map.Map[
                food48MapTransform.Position.X, food48MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food48Collider = new ColliderComponent(Cell.Food);
            allFoods[48].AddComponent(food48Transform);
            allFoods[48].AddComponent(food48MapTransform);
            allFoods[48].AddComponent(food48Collider);
            allFoods[48].AddComponent(new ConsoleSprite(
                food48Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[49] = new GameObject("Food49");
            char[,] food49Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food49Transform = new TransformComponent(24, 5);
            MapTransformComponent food49MapTransform =
                new MapTransformComponent(8, 5);
            map.Map[
                food49MapTransform.Position.X, food49MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food49Collider = new ColliderComponent(Cell.Food);
            allFoods[49].AddComponent(food49Transform);
            allFoods[49].AddComponent(food49MapTransform);
            allFoods[49].AddComponent(food49Collider);
            allFoods[49].AddComponent(new ConsoleSprite(
                food49Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[50] = new GameObject("Food50");
            char[,] food50Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food50Transform = new TransformComponent(27, 5);
            MapTransformComponent food50MapTransform =
                new MapTransformComponent(9, 5);
            map.Map[
                food50MapTransform.Position.X, food50MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food50Collider = new ColliderComponent(Cell.Food);
            allFoods[50].AddComponent(food50Transform);
            allFoods[50].AddComponent(food50MapTransform);
            allFoods[50].AddComponent(food50Collider);
            allFoods[50].AddComponent(new ConsoleSprite(
                food50Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[51] = new GameObject("Food51");
            char[,] food51Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food51Transform = new TransformComponent(30, 5);
            MapTransformComponent food51MapTransform =
                new MapTransformComponent(10, 5);
            map.Map[
                food51MapTransform.Position.X, food51MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food51Collider = new ColliderComponent(Cell.Food);
            allFoods[51].AddComponent(food51Transform);
            allFoods[51].AddComponent(food51MapTransform);
            allFoods[51].AddComponent(food51Collider);
            allFoods[51].AddComponent(new ConsoleSprite(
                food51Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[52] = new GameObject("Food52");
            char[,] food52Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food52Transform = new TransformComponent(33, 5);
            MapTransformComponent food52MapTransform =
                new MapTransformComponent(11, 5);
            map.Map[
                food52MapTransform.Position.X, food52MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food52Collider = new ColliderComponent(Cell.Food);
            allFoods[52].AddComponent(food52Transform);
            allFoods[52].AddComponent(food52MapTransform);
            allFoods[52].AddComponent(food52Collider);
            allFoods[52].AddComponent(new ConsoleSprite(
                food52Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[53] = new GameObject("Food53");
            char[,] food53Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food53Transform = new TransformComponent(36, 5);
            MapTransformComponent food53MapTransform =
                new MapTransformComponent(12, 5);
            map.Map[
                food53MapTransform.Position.X, food53MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food53Collider = new ColliderComponent(Cell.Food);
            allFoods[53].AddComponent(food53Transform);
            allFoods[53].AddComponent(food53MapTransform);
            allFoods[53].AddComponent(food53Collider);
            allFoods[53].AddComponent(new ConsoleSprite(
                food53Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[54] = new GameObject("Food54");
            char[,] food54Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food54Transform = new TransformComponent(39, 5);
            MapTransformComponent food54MapTransform =
                new MapTransformComponent(13, 5);
            map.Map[
                food54MapTransform.Position.X, food54MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food54Collider = new ColliderComponent(Cell.Food);
            allFoods[54].AddComponent(food54Transform);
            allFoods[54].AddComponent(food54MapTransform);
            allFoods[54].AddComponent(food54Collider);
            allFoods[54].AddComponent(new ConsoleSprite(
                food54Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[55] = new GameObject("Food55");
            char[,] food55Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food55Transform = new TransformComponent(42, 5);
            MapTransformComponent food55MapTransform =
                new MapTransformComponent(14, 5);
            map.Map[
                food55MapTransform.Position.X, food55MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food55Collider = new ColliderComponent(Cell.Food);
            allFoods[55].AddComponent(food55Transform);
            allFoods[55].AddComponent(food55MapTransform);
            allFoods[55].AddComponent(food55Collider);
            allFoods[55].AddComponent(new ConsoleSprite(
                food55Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[56] = new GameObject("Food56");
            char[,] food56Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food56Transform = new TransformComponent(45, 5);
            MapTransformComponent food56MapTransform =
                new MapTransformComponent(15, 5);
            map.Map[
                food56MapTransform.Position.X, food56MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food56Collider = new ColliderComponent(Cell.Food);
            allFoods[56].AddComponent(food56Transform);
            allFoods[56].AddComponent(food56MapTransform);
            allFoods[56].AddComponent(food56Collider);
            allFoods[56].AddComponent(new ConsoleSprite(
                food56Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[57] = new GameObject("Food57");
            char[,] food57Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food57Transform = new TransformComponent(48, 5);
            MapTransformComponent food57MapTransform =
                new MapTransformComponent(16, 5);
            map.Map[
                food57MapTransform.Position.X, food57MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food57Collider = new ColliderComponent(Cell.Food);
            allFoods[57].AddComponent(food57Transform);
            allFoods[57].AddComponent(food57MapTransform);
            allFoods[57].AddComponent(food57Collider);
            allFoods[57].AddComponent(new ConsoleSprite(
                food57Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[58] = new GameObject("Food58");
            char[,] food58Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food58Transform = new TransformComponent(51, 5);
            MapTransformComponent food58MapTransform =
                new MapTransformComponent(17, 5);
            map.Map[
                food58MapTransform.Position.X, food58MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food58Collider = new ColliderComponent(Cell.Food);
            allFoods[58].AddComponent(food58Transform);
            allFoods[58].AddComponent(food58MapTransform);
            allFoods[58].AddComponent(food58Collider);
            allFoods[58].AddComponent(new ConsoleSprite(
                food58Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[59] = new GameObject("Food59");
            char[,] food59Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food59Transform = new TransformComponent(54, 5);
            MapTransformComponent food59MapTransform =
                new MapTransformComponent(18, 5);
            map.Map[
                food59MapTransform.Position.X, food59MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food59Collider = new ColliderComponent(Cell.Food);
            allFoods[59].AddComponent(food59Transform);
            allFoods[59].AddComponent(food59MapTransform);
            allFoods[59].AddComponent(food59Collider);
            allFoods[59].AddComponent(new ConsoleSprite(
                food59Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[60] = new GameObject("Food60");
            char[,] food60Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food60Transform = new TransformComponent(57, 5);
            MapTransformComponent food60MapTransform =
                new MapTransformComponent(19, 5);
            map.Map[
                food60MapTransform.Position.X, food60MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food60Collider = new ColliderComponent(Cell.Food);
            allFoods[60].AddComponent(food60Transform);
            allFoods[60].AddComponent(food60MapTransform);
            allFoods[60].AddComponent(food60Collider);
            allFoods[60].AddComponent(new ConsoleSprite(
                food60Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[61] = new GameObject("Food61");
            char[,] food61Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food61Transform = new TransformComponent(60, 5);
            MapTransformComponent food61MapTransform =
                new MapTransformComponent(20, 5);
            map.Map[
                food61MapTransform.Position.X, food61MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food61Collider = new ColliderComponent(Cell.Food);
            allFoods[61].AddComponent(food61Transform);
            allFoods[61].AddComponent(food61MapTransform);
            allFoods[61].AddComponent(food61Collider);
            allFoods[61].AddComponent(new ConsoleSprite(
                food61Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[62] = new GameObject("Food62");
            char[,] food62Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food62Transform = new TransformComponent(63, 5);
            MapTransformComponent food62MapTransform =
                new MapTransformComponent(21, 5);
            map.Map[
                food62MapTransform.Position.X, food62MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food62Collider = new ColliderComponent(Cell.Food);
            allFoods[62].AddComponent(food62Transform);
            allFoods[62].AddComponent(food62MapTransform);
            allFoods[62].AddComponent(food62Collider);
            allFoods[62].AddComponent(new ConsoleSprite(
                food62Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[63] = new GameObject("Food63");
            char[,] food63Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food63Transform = new TransformComponent(66, 5);
            MapTransformComponent food63MapTransform =
                new MapTransformComponent(22, 5);
            map.Map[
                food63MapTransform.Position.X, food63MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food63Collider = new ColliderComponent(Cell.Food);
            allFoods[63].AddComponent(food63Transform);
            allFoods[63].AddComponent(food63MapTransform);
            allFoods[63].AddComponent(food63Collider);
            allFoods[63].AddComponent(new ConsoleSprite(
                food63Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[64] = new GameObject("Food64");
            char[,] food64Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food64Transform = new TransformComponent(69, 5);
            MapTransformComponent food64MapTransform =
                new MapTransformComponent(23, 5);
            map.Map[
                food64MapTransform.Position.X, food64MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food64Collider = new ColliderComponent(Cell.Food);
            allFoods[64].AddComponent(food64Transform);
            allFoods[64].AddComponent(food64MapTransform);
            allFoods[64].AddComponent(food64Collider);
            allFoods[64].AddComponent(new ConsoleSprite(
                food64Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[65] = new GameObject("Food65");
            char[,] food65Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food65Transform = new TransformComponent(72, 5);
            MapTransformComponent food65MapTransform =
                new MapTransformComponent(24, 5);
            map.Map[
                food65MapTransform.Position.X, food65MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food65Collider = new ColliderComponent(Cell.Food);
            allFoods[65].AddComponent(food65Transform);
            allFoods[65].AddComponent(food65MapTransform);
            allFoods[65].AddComponent(food65Collider);
            allFoods[65].AddComponent(new ConsoleSprite(
                food65Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[66] = new GameObject("Food66");
            char[,] food66Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food66Transform = new TransformComponent(75, 5);
            MapTransformComponent food66MapTransform =
                new MapTransformComponent(25, 5);
            map.Map[
                food66MapTransform.Position.X, food66MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food66Collider = new ColliderComponent(Cell.Food);
            allFoods[66].AddComponent(food66Transform);
            allFoods[66].AddComponent(food66MapTransform);
            allFoods[66].AddComponent(food66Collider);
            allFoods[66].AddComponent(new ConsoleSprite(
                food66Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[67] = new GameObject("Food67");
            char[,] food67Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food67Transform = new TransformComponent(78, 5);
            MapTransformComponent food67MapTransform =
                new MapTransformComponent(26, 5);
            map.Map[
                food67MapTransform.Position.X, food67MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food67Collider = new ColliderComponent(Cell.Food);
            allFoods[67].AddComponent(food67Transform);
            allFoods[67].AddComponent(food67MapTransform);
            allFoods[67].AddComponent(food67Collider);
            allFoods[67].AddComponent(new ConsoleSprite(
                food67Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[68] = new GameObject("Food68");
            char[,] food68Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food68Transform = new TransformComponent(3, 6);
            MapTransformComponent food68MapTransform =
                new MapTransformComponent(1, 6);
            map.Map[
                food68MapTransform.Position.X, food68MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food68Collider = new ColliderComponent(Cell.Food);
            allFoods[68].AddComponent(food68Transform);
            allFoods[68].AddComponent(food68MapTransform);
            allFoods[68].AddComponent(food68Collider);
            allFoods[68].AddComponent(new ConsoleSprite(
                food68Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[69] = new GameObject("Food69");
            char[,] food69Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food69Transform = new TransformComponent(18, 6);
            MapTransformComponent food69MapTransform =
                new MapTransformComponent(6, 6);
            map.Map[
                food69MapTransform.Position.X, food69MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food69Collider = new ColliderComponent(Cell.Food);
            allFoods[69].AddComponent(food69Transform);
            allFoods[69].AddComponent(food69MapTransform);
            allFoods[69].AddComponent(food69Collider);
            allFoods[69].AddComponent(new ConsoleSprite(
                food69Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[70] = new GameObject("Food70");
            char[,] food70Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food70Transform = new TransformComponent(27, 6);
            MapTransformComponent food70MapTransform =
                new MapTransformComponent(9, 6);
            map.Map[
                food70MapTransform.Position.X, food70MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food70Collider = new ColliderComponent(Cell.Food);
            allFoods[70].AddComponent(food70Transform);
            allFoods[70].AddComponent(food70MapTransform);
            allFoods[70].AddComponent(food70Collider);
            allFoods[70].AddComponent(new ConsoleSprite(
                food70Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[71] = new GameObject("Food71");
            char[,] food71Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food71Transform = new TransformComponent(54, 6);
            MapTransformComponent food71MapTransform =
                new MapTransformComponent(18, 6);
            map.Map[
                food71MapTransform.Position.X, food71MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food71Collider = new ColliderComponent(Cell.Food);
            allFoods[71].AddComponent(food71Transform);
            allFoods[71].AddComponent(food71MapTransform);
            allFoods[71].AddComponent(food71Collider);
            allFoods[71].AddComponent(new ConsoleSprite(
                food71Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[72] = new GameObject("Food72");
            char[,] food72Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food72Transform = new TransformComponent(63, 6);
            MapTransformComponent food72MapTransform =
                new MapTransformComponent(21, 6);
            map.Map[
                food72MapTransform.Position.X, food72MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food72Collider = new ColliderComponent(Cell.Food);
            allFoods[72].AddComponent(food72Transform);
            allFoods[72].AddComponent(food72MapTransform);
            allFoods[72].AddComponent(food72Collider);
            allFoods[72].AddComponent(new ConsoleSprite(
                food72Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[73] = new GameObject("Food73");
            char[,] food73Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food73Transform = new TransformComponent(78, 6);
            MapTransformComponent food73MapTransform =
                new MapTransformComponent(26, 6);
            map.Map[
                food73MapTransform.Position.X, food73MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food73Collider = new ColliderComponent(Cell.Food);
            allFoods[73].AddComponent(food73Transform);
            allFoods[73].AddComponent(food73MapTransform);
            allFoods[73].AddComponent(food73Collider);
            allFoods[73].AddComponent(new ConsoleSprite(
                food73Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[74] = new GameObject("Food74");
            char[,] food74Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food74Transform = new TransformComponent(3, 7);
            MapTransformComponent food74MapTransform =
                new MapTransformComponent(1, 7);
            map.Map[
                food74MapTransform.Position.X, food74MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food74Collider = new ColliderComponent(Cell.Food);
            allFoods[74].AddComponent(food74Transform);
            allFoods[74].AddComponent(food74MapTransform);
            allFoods[74].AddComponent(food74Collider);
            allFoods[74].AddComponent(new ConsoleSprite(
                food74Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[75] = new GameObject("Food75");
            char[,] food75Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food75Transform = new TransformComponent(18, 7);
            MapTransformComponent food75MapTransform =
                new MapTransformComponent(6, 7);
            map.Map[
                food75MapTransform.Position.X, food75MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food75Collider = new ColliderComponent(Cell.Food);
            allFoods[75].AddComponent(food75Transform);
            allFoods[75].AddComponent(food75MapTransform);
            allFoods[75].AddComponent(food75Collider);
            allFoods[75].AddComponent(new ConsoleSprite(
                food75Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[76] = new GameObject("Food76");
            char[,] food76Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food76Transform = new TransformComponent(27, 7);
            MapTransformComponent food76MapTransform =
                new MapTransformComponent(9, 7);
            map.Map[
                food76MapTransform.Position.X, food76MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food76Collider = new ColliderComponent(Cell.Food);
            allFoods[76].AddComponent(food76Transform);
            allFoods[76].AddComponent(food76MapTransform);
            allFoods[76].AddComponent(food76Collider);
            allFoods[76].AddComponent(new ConsoleSprite(
                food76Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[77] = new GameObject("Food77");
            char[,] food77Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food77Transform = new TransformComponent(54, 7);
            MapTransformComponent food77MapTransform =
                new MapTransformComponent(18, 7);
            map.Map[
                food77MapTransform.Position.X, food77MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food77Collider = new ColliderComponent(Cell.Food);
            allFoods[77].AddComponent(food77Transform);
            allFoods[77].AddComponent(food77MapTransform);
            allFoods[77].AddComponent(food77Collider);
            allFoods[77].AddComponent(new ConsoleSprite(
                food77Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[78] = new GameObject("Food78");
            char[,] food78Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food78Transform = new TransformComponent(63, 7);
            MapTransformComponent food78MapTransform =
                new MapTransformComponent(21, 7);
            map.Map[
                food78MapTransform.Position.X, food78MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food78Collider = new ColliderComponent(Cell.Food);
            allFoods[78].AddComponent(food78Transform);
            allFoods[78].AddComponent(food78MapTransform);
            allFoods[78].AddComponent(food78Collider);
            allFoods[78].AddComponent(new ConsoleSprite(
                food78Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[79] = new GameObject("Food79");
            char[,] food79Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food79Transform = new TransformComponent(78, 7);
            MapTransformComponent food79MapTransform =
                new MapTransformComponent(26, 7);
            map.Map[
                food79MapTransform.Position.X, food79MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food79Collider = new ColliderComponent(Cell.Food);
            allFoods[79].AddComponent(food79Transform);
            allFoods[79].AddComponent(food79MapTransform);
            allFoods[79].AddComponent(food79Collider);
            allFoods[79].AddComponent(new ConsoleSprite(
                food79Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[80] = new GameObject("Food80");
            char[,] food80Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food80Transform = new TransformComponent(3, 8);
            MapTransformComponent food80MapTransform =
                new MapTransformComponent(1, 8);
            map.Map[
                food80MapTransform.Position.X, food80MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food80Collider = new ColliderComponent(Cell.Food);
            allFoods[80].AddComponent(food80Transform);
            allFoods[80].AddComponent(food80MapTransform);
            allFoods[80].AddComponent(food80Collider);
            allFoods[80].AddComponent(new ConsoleSprite(
                food80Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[81] = new GameObject("Food81");
            char[,] food81Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food81Transform = new TransformComponent(6, 8);
            MapTransformComponent food81MapTransform =
                new MapTransformComponent(2, 8);
            map.Map[
                food81MapTransform.Position.X, food81MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food81Collider = new ColliderComponent(Cell.Food);
            allFoods[81].AddComponent(food81Transform);
            allFoods[81].AddComponent(food81MapTransform);
            allFoods[81].AddComponent(food81Collider);
            allFoods[81].AddComponent(new ConsoleSprite(
                food81Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[82] = new GameObject("Food82");
            char[,] food82Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food82Transform = new TransformComponent(9, 8);
            MapTransformComponent food82MapTransform =
                new MapTransformComponent(3, 8);
            map.Map[
                food82MapTransform.Position.X, food82MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food82Collider = new ColliderComponent(Cell.Food);
            allFoods[82].AddComponent(food82Transform);
            allFoods[82].AddComponent(food82MapTransform);
            allFoods[82].AddComponent(food82Collider);
            allFoods[82].AddComponent(new ConsoleSprite(
                food82Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[83] = new GameObject("Food83");
            char[,] food83Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food83Transform = new TransformComponent(12, 8);
            MapTransformComponent food83MapTransform =
                new MapTransformComponent(4, 8);
            map.Map[
                food83MapTransform.Position.X, food83MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food83Collider = new ColliderComponent(Cell.Food);
            allFoods[83].AddComponent(food83Transform);
            allFoods[83].AddComponent(food83MapTransform);
            allFoods[83].AddComponent(food83Collider);
            allFoods[83].AddComponent(new ConsoleSprite(
                food83Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[84] = new GameObject("Food84");
            char[,] food84Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food84Transform = new TransformComponent(15, 8);
            MapTransformComponent food84MapTransform =
                new MapTransformComponent(5, 8);
            map.Map[
                food84MapTransform.Position.X, food84MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food84Collider = new ColliderComponent(Cell.Food);
            allFoods[84].AddComponent(food84Transform);
            allFoods[84].AddComponent(food84MapTransform);
            allFoods[84].AddComponent(food84Collider);
            allFoods[84].AddComponent(new ConsoleSprite(
                food84Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[85] = new GameObject("Food85");
            char[,] food85Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food85Transform = new TransformComponent(18, 8);
            MapTransformComponent food85MapTransform =
                new MapTransformComponent(6, 8);
            map.Map[
                food85MapTransform.Position.X, food85MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food85Collider = new ColliderComponent(Cell.Food);
            allFoods[85].AddComponent(food85Transform);
            allFoods[85].AddComponent(food85MapTransform);
            allFoods[85].AddComponent(food85Collider);
            allFoods[85].AddComponent(new ConsoleSprite(
                food85Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[86] = new GameObject("Food86");
            char[,] food86Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food86Transform = new TransformComponent(27, 8);
            MapTransformComponent food86MapTransform =
                new MapTransformComponent(9, 8);
            map.Map[
                food86MapTransform.Position.X, food86MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food86Collider = new ColliderComponent(Cell.Food);
            allFoods[86].AddComponent(food86Transform);
            allFoods[86].AddComponent(food86MapTransform);
            allFoods[86].AddComponent(food86Collider);
            allFoods[86].AddComponent(new ConsoleSprite(
                food86Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[87] = new GameObject("Food87");
            char[,] food87Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food87Transform = new TransformComponent(30, 8);
            MapTransformComponent food87MapTransform =
                new MapTransformComponent(10, 8);
            map.Map[
                food87MapTransform.Position.X, food87MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food87Collider = new ColliderComponent(Cell.Food);
            allFoods[87].AddComponent(food87Transform);
            allFoods[87].AddComponent(food87MapTransform);
            allFoods[87].AddComponent(food87Collider);
            allFoods[87].AddComponent(new ConsoleSprite(
                food87Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[88] = new GameObject("Food88");
            char[,] food88Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food88Transform = new TransformComponent(33, 8);
            MapTransformComponent food88MapTransform =
                new MapTransformComponent(11, 8);
            map.Map[
                food88MapTransform.Position.X, food88MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food88Collider = new ColliderComponent(Cell.Food);
            allFoods[88].AddComponent(food88Transform);
            allFoods[88].AddComponent(food88MapTransform);
            allFoods[88].AddComponent(food88Collider);
            allFoods[88].AddComponent(new ConsoleSprite(
                food88Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[89] = new GameObject("Food89");
            char[,] food89Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food89Transform = new TransformComponent(36, 8);
            MapTransformComponent food89MapTransform =
                new MapTransformComponent(12, 8);
            map.Map[
                food89MapTransform.Position.X, food89MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food89Collider = new ColliderComponent(Cell.Food);
            allFoods[89].AddComponent(food89Transform);
            allFoods[89].AddComponent(food89MapTransform);
            allFoods[89].AddComponent(food89Collider);
            allFoods[89].AddComponent(new ConsoleSprite(
                food89Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[90] = new GameObject("Food90");
            char[,] food90Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food90Transform = new TransformComponent(45, 8);
            MapTransformComponent food90MapTransform =
                new MapTransformComponent(15, 8);
            map.Map[
                food90MapTransform.Position.X, food90MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food90Collider = new ColliderComponent(Cell.Food);
            allFoods[90].AddComponent(food90Transform);
            allFoods[90].AddComponent(food90MapTransform);
            allFoods[90].AddComponent(food90Collider);
            allFoods[90].AddComponent(new ConsoleSprite(
                food90Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[91] = new GameObject("Food91");
            char[,] food91Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food91Transform = new TransformComponent(48, 8);
            MapTransformComponent food91MapTransform =
                new MapTransformComponent(16, 8);
            map.Map[
                food91MapTransform.Position.X, food91MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food91Collider = new ColliderComponent(Cell.Food);
            allFoods[91].AddComponent(food91Transform);
            allFoods[91].AddComponent(food91MapTransform);
            allFoods[91].AddComponent(food91Collider);
            allFoods[91].AddComponent(new ConsoleSprite(
                food91Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[92] = new GameObject("Food92");
            char[,] food92Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food92Transform = new TransformComponent(51, 8);
            MapTransformComponent food92MapTransform =
                new MapTransformComponent(17, 8);
            map.Map[
                food92MapTransform.Position.X, food92MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food92Collider = new ColliderComponent(Cell.Food);
            allFoods[92].AddComponent(food92Transform);
            allFoods[92].AddComponent(food92MapTransform);
            allFoods[92].AddComponent(food92Collider);
            allFoods[92].AddComponent(new ConsoleSprite(
                food92Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[93] = new GameObject("Food93");
            char[,] food93Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food93Transform = new TransformComponent(54, 8);
            MapTransformComponent food93MapTransform =
                new MapTransformComponent(18, 8);
            map.Map[
                food93MapTransform.Position.X, food93MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food93Collider = new ColliderComponent(Cell.Food);
            allFoods[93].AddComponent(food93Transform);
            allFoods[93].AddComponent(food93MapTransform);
            allFoods[93].AddComponent(food93Collider);
            allFoods[93].AddComponent(new ConsoleSprite(
                food93Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[94] = new GameObject("Food94");
            char[,] food94Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food94Transform = new TransformComponent(63, 8);
            MapTransformComponent food94MapTransform =
                new MapTransformComponent(21, 8);
            map.Map[
                food94MapTransform.Position.X, food94MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food94Collider = new ColliderComponent(Cell.Food);
            allFoods[94].AddComponent(food94Transform);
            allFoods[94].AddComponent(food94MapTransform);
            allFoods[94].AddComponent(food94Collider);
            allFoods[94].AddComponent(new ConsoleSprite(
                food94Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[95] = new GameObject("Food95");
            char[,] food95Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food95Transform = new TransformComponent(66, 8);
            MapTransformComponent food95MapTransform =
                new MapTransformComponent(22, 8);
            map.Map[
                food95MapTransform.Position.X, food95MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food95Collider = new ColliderComponent(Cell.Food);
            allFoods[95].AddComponent(food95Transform);
            allFoods[95].AddComponent(food95MapTransform);
            allFoods[95].AddComponent(food95Collider);
            allFoods[95].AddComponent(new ConsoleSprite(
                food95Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[96] = new GameObject("Food96");
            char[,] food96Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food96Transform = new TransformComponent(69, 8);
            MapTransformComponent food96MapTransform =
                new MapTransformComponent(23, 8);
            map.Map[
                food96MapTransform.Position.X, food96MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food96Collider = new ColliderComponent(Cell.Food);
            allFoods[96].AddComponent(food96Transform);
            allFoods[96].AddComponent(food96MapTransform);
            allFoods[96].AddComponent(food96Collider);
            allFoods[96].AddComponent(new ConsoleSprite(
                food96Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[97] = new GameObject("Food97");
            char[,] food97Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food97Transform = new TransformComponent(72, 8);
            MapTransformComponent food97MapTransform =
                new MapTransformComponent(24, 8);
            map.Map[
                food97MapTransform.Position.X, food97MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food97Collider = new ColliderComponent(Cell.Food);
            allFoods[97].AddComponent(food97Transform);
            allFoods[97].AddComponent(food97MapTransform);
            allFoods[97].AddComponent(food97Collider);
            allFoods[97].AddComponent(new ConsoleSprite(
                food97Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[98] = new GameObject("Food98");
            char[,] food98Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food98Transform = new TransformComponent(75, 8);
            MapTransformComponent food98MapTransform =
                new MapTransformComponent(25, 8);
            map.Map[
                food98MapTransform.Position.X, food98MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food98Collider = new ColliderComponent(Cell.Food);
            allFoods[98].AddComponent(food98Transform);
            allFoods[98].AddComponent(food98MapTransform);
            allFoods[98].AddComponent(food98Collider);
            allFoods[98].AddComponent(new ConsoleSprite(
                food98Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[99] = new GameObject("Food99");
            char[,] food99Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food99Transform = new TransformComponent(78, 8);
            MapTransformComponent food99MapTransform =
                new MapTransformComponent(26, 8);
            map.Map[
                food99MapTransform.Position.X, food99MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food99Collider = new ColliderComponent(Cell.Food);
            allFoods[99].AddComponent(food99Transform);
            allFoods[99].AddComponent(food99MapTransform);
            allFoods[99].AddComponent(food99Collider);
            allFoods[99].AddComponent(new ConsoleSprite(
                food99Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[100] = new GameObject("Food100");
            char[,] food100Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food100Transform = new TransformComponent(18, 9);
            MapTransformComponent food100MapTransform =
                new MapTransformComponent(6, 9);
            map.Map[
                food100MapTransform.Position.X, food100MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food100Collider = new ColliderComponent(Cell.Food);
            allFoods[100].AddComponent(food100Transform);
            allFoods[100].AddComponent(food100MapTransform);
            allFoods[100].AddComponent(food100Collider);
            allFoods[100].AddComponent(new ConsoleSprite(
                food100Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[101] = new GameObject("Food101");
            char[,] food101Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food101Transform = new TransformComponent(63, 9);
            MapTransformComponent food101MapTransform =
                new MapTransformComponent(21, 9);
            map.Map[
                food101MapTransform.Position.X, food101MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food101Collider = new ColliderComponent(Cell.Food);
            allFoods[101].AddComponent(food101Transform);
            allFoods[101].AddComponent(food101MapTransform);
            allFoods[101].AddComponent(food101Collider);
            allFoods[101].AddComponent(new ConsoleSprite(
                food101Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[102] = new GameObject("Food102");
            char[,] food102Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food102Transform = new TransformComponent(18, 10);
            MapTransformComponent food102MapTransform =
                new MapTransformComponent(6, 10);
            map.Map[
                food102MapTransform.Position.X, food102MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food102Collider = new ColliderComponent(Cell.Food);
            allFoods[102].AddComponent(food102Transform);
            allFoods[102].AddComponent(food102MapTransform);
            allFoods[102].AddComponent(food102Collider);
            allFoods[102].AddComponent(new ConsoleSprite(
                food102Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[103] = new GameObject("Food103");
            char[,] food103Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food103Transform = new TransformComponent(63, 10);
            MapTransformComponent food103MapTransform =
                new MapTransformComponent(21, 10);
            map.Map[
                food103MapTransform.Position.X, food103MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food103Collider = new ColliderComponent(Cell.Food);
            allFoods[103].AddComponent(food103Transform);
            allFoods[103].AddComponent(food103MapTransform);
            allFoods[103].AddComponent(food103Collider);
            allFoods[103].AddComponent(new ConsoleSprite(
                food103Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[104] = new GameObject("Food104");
            char[,] food104Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food104Transform = new TransformComponent(18, 11);
            MapTransformComponent food104MapTransform =
                new MapTransformComponent(6, 11);
            map.Map[
                food104MapTransform.Position.X, food104MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food104Collider = new ColliderComponent(Cell.Food);
            allFoods[104].AddComponent(food104Transform);
            allFoods[104].AddComponent(food104MapTransform);
            allFoods[104].AddComponent(food104Collider);
            allFoods[104].AddComponent(new ConsoleSprite(
                food104Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[105] = new GameObject("Food105");
            char[,] food105Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food105Transform = new TransformComponent(63, 11);
            MapTransformComponent food105MapTransform =
                new MapTransformComponent(21, 11);
            map.Map[
                food105MapTransform.Position.X, food105MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food105Collider = new ColliderComponent(Cell.Food);
            allFoods[105].AddComponent(food105Transform);
            allFoods[105].AddComponent(food105MapTransform);
            allFoods[105].AddComponent(food105Collider);
            allFoods[105].AddComponent(new ConsoleSprite(
                food105Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[106] = new GameObject("Food106");
            char[,] food106Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food106Transform = new TransformComponent(18, 12);
            MapTransformComponent food106MapTransform =
                new MapTransformComponent(6, 12);
            map.Map[
                food106MapTransform.Position.X, food106MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food106Collider = new ColliderComponent(Cell.Food);
            allFoods[106].AddComponent(food106Transform);
            allFoods[106].AddComponent(food106MapTransform);
            allFoods[106].AddComponent(food106Collider);
            allFoods[106].AddComponent(new ConsoleSprite(
                food106Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[107] = new GameObject("Food107");
            char[,] food107Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food107Transform = new TransformComponent(63, 12);
            MapTransformComponent food107MapTransform =
                new MapTransformComponent(21, 12);
            map.Map[
                food107MapTransform.Position.X, food107MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food107Collider = new ColliderComponent(Cell.Food);
            allFoods[107].AddComponent(food107Transform);
            allFoods[107].AddComponent(food107MapTransform);
            allFoods[107].AddComponent(food107Collider);
            allFoods[107].AddComponent(new ConsoleSprite(
                food107Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[108] = new GameObject("Food108");
            char[,] food108Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food108Transform = new TransformComponent(18, 13);
            MapTransformComponent food108MapTransform =
                new MapTransformComponent(6, 13);
            map.Map[
                food108MapTransform.Position.X, food108MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food108Collider = new ColliderComponent(Cell.Food);
            allFoods[108].AddComponent(food108Transform);
            allFoods[108].AddComponent(food108MapTransform);
            allFoods[108].AddComponent(food108Collider);
            allFoods[108].AddComponent(new ConsoleSprite(
                food108Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[109] = new GameObject("Food109");
            char[,] food109Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food109Transform = new TransformComponent(63, 13);
            MapTransformComponent food109MapTransform =
                new MapTransformComponent(21, 13);
            map.Map[
                food109MapTransform.Position.X, food109MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food109Collider = new ColliderComponent(Cell.Food);
            allFoods[109].AddComponent(food109Transform);
            allFoods[109].AddComponent(food109MapTransform);
            allFoods[109].AddComponent(food109Collider);
            allFoods[109].AddComponent(new ConsoleSprite(
                food109Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[110] = new GameObject("Food110");
            char[,] food110Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food110Transform = new TransformComponent(18, 14);
            MapTransformComponent food110MapTransform =
                new MapTransformComponent(6, 14);
            map.Map[
                food110MapTransform.Position.X, food110MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food110Collider = new ColliderComponent(Cell.Food);
            allFoods[110].AddComponent(food110Transform);
            allFoods[110].AddComponent(food110MapTransform);
            allFoods[110].AddComponent(food110Collider);
            allFoods[110].AddComponent(new ConsoleSprite(
                food110Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[111] = new GameObject("Food111");
            char[,] food111Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food111Transform = new TransformComponent(63, 14);
            MapTransformComponent food111MapTransform =
                new MapTransformComponent(21, 14);
            map.Map[
                food111MapTransform.Position.X, food111MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food111Collider = new ColliderComponent(Cell.Food);
            allFoods[111].AddComponent(food111Transform);
            allFoods[111].AddComponent(food111MapTransform);
            allFoods[111].AddComponent(food111Collider);
            allFoods[111].AddComponent(new ConsoleSprite(
                food111Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[112] = new GameObject("Food112");
            char[,] food112Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food112Transform = new TransformComponent(18, 15);
            MapTransformComponent food112MapTransform =
                new MapTransformComponent(6, 15);
            map.Map[
                food112MapTransform.Position.X, food112MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food112Collider = new ColliderComponent(Cell.Food);
            allFoods[112].AddComponent(food112Transform);
            allFoods[112].AddComponent(food112MapTransform);
            allFoods[112].AddComponent(food112Collider);
            allFoods[112].AddComponent(new ConsoleSprite(
                food112Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[113] = new GameObject("Food113");
            char[,] food113Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food113Transform = new TransformComponent(63, 15);
            MapTransformComponent food113MapTransform =
                new MapTransformComponent(21, 15);
            map.Map[
                food113MapTransform.Position.X, food113MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food113Collider = new ColliderComponent(Cell.Food);
            allFoods[113].AddComponent(food113Transform);
            allFoods[113].AddComponent(food113MapTransform);
            allFoods[113].AddComponent(food113Collider);
            allFoods[113].AddComponent(new ConsoleSprite(
                food113Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[114] = new GameObject("Food114");
            char[,] food114Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food114Transform = new TransformComponent(18, 16);
            MapTransformComponent food114MapTransform =
                new MapTransformComponent(6, 16);
            map.Map[
                food114MapTransform.Position.X, food114MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food114Collider = new ColliderComponent(Cell.Food);
            allFoods[114].AddComponent(food114Transform);
            allFoods[114].AddComponent(food114MapTransform);
            allFoods[114].AddComponent(food114Collider);
            allFoods[114].AddComponent(new ConsoleSprite(
                food114Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[115] = new GameObject("Food115");
            char[,] food115Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food115Transform = new TransformComponent(63, 16);
            MapTransformComponent food115MapTransform =
                new MapTransformComponent(21, 16);
            map.Map[
                food115MapTransform.Position.X, food115MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food115Collider = new ColliderComponent(Cell.Food);
            allFoods[115].AddComponent(food115Transform);
            allFoods[115].AddComponent(food115MapTransform);
            allFoods[115].AddComponent(food115Collider);
            allFoods[115].AddComponent(new ConsoleSprite(
                food115Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[116] = new GameObject("Food116");
            char[,] food116Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food116Transform = new TransformComponent(18, 17);
            MapTransformComponent food116MapTransform =
                new MapTransformComponent(6, 17);
            map.Map[
                food116MapTransform.Position.X, food116MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food116Collider = new ColliderComponent(Cell.Food);
            allFoods[116].AddComponent(food116Transform);
            allFoods[116].AddComponent(food116MapTransform);
            allFoods[116].AddComponent(food116Collider);
            allFoods[116].AddComponent(new ConsoleSprite(
                food116Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[117] = new GameObject("Food117");
            char[,] food117Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food117Transform = new TransformComponent(63, 17);
            MapTransformComponent food117MapTransform =
                new MapTransformComponent(21, 17);
            map.Map[
                food117MapTransform.Position.X, food117MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food117Collider = new ColliderComponent(Cell.Food);
            allFoods[117].AddComponent(food117Transform);
            allFoods[117].AddComponent(food117MapTransform);
            allFoods[117].AddComponent(food117Collider);
            allFoods[117].AddComponent(new ConsoleSprite(
                food117Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[118] = new GameObject("Food118");
            char[,] food118Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food118Transform = new TransformComponent(18, 18);
            MapTransformComponent food118MapTransform =
                new MapTransformComponent(6, 18);
            map.Map[
                food118MapTransform.Position.X, food118MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food118Collider = new ColliderComponent(Cell.Food);
            allFoods[118].AddComponent(food118Transform);
            allFoods[118].AddComponent(food118MapTransform);
            allFoods[118].AddComponent(food118Collider);
            allFoods[118].AddComponent(new ConsoleSprite(
                food118Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[119] = new GameObject("Food119");
            char[,] food119Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food119Transform = new TransformComponent(63, 18);
            MapTransformComponent food119MapTransform =
                new MapTransformComponent(21, 18);
            map.Map[
                food119MapTransform.Position.X, food119MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food119Collider = new ColliderComponent(Cell.Food);
            allFoods[119].AddComponent(food119Transform);
            allFoods[119].AddComponent(food119MapTransform);
            allFoods[119].AddComponent(food119Collider);
            allFoods[119].AddComponent(new ConsoleSprite(
                food119Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[120] = new GameObject("Food120");
            char[,] food120Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food120Transform = new TransformComponent(18, 19);
            MapTransformComponent food120MapTransform =
                new MapTransformComponent(6, 19);
            map.Map[
                food120MapTransform.Position.X, food120MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food120Collider = new ColliderComponent(Cell.Food);
            allFoods[120].AddComponent(food120Transform);
            allFoods[120].AddComponent(food120MapTransform);
            allFoods[120].AddComponent(food120Collider);
            allFoods[120].AddComponent(new ConsoleSprite(
                food120Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[121] = new GameObject("Food121");
            char[,] food121Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food121Transform = new TransformComponent(63, 19);
            MapTransformComponent food121MapTransform =
                new MapTransformComponent(21, 19);
            map.Map[
                food121MapTransform.Position.X, food121MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food121Collider = new ColliderComponent(Cell.Food);
            allFoods[121].AddComponent(food121Transform);
            allFoods[121].AddComponent(food121MapTransform);
            allFoods[121].AddComponent(food121Collider);
            allFoods[121].AddComponent(new ConsoleSprite(
                food121Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[122] = new GameObject("Food122");
            char[,] food122Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food122Transform = new TransformComponent(3, 20);
            MapTransformComponent food122MapTransform =
                new MapTransformComponent(1, 20);
            map.Map[
                food122MapTransform.Position.X, food122MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food122Collider = new ColliderComponent(Cell.Food);
            allFoods[122].AddComponent(food122Transform);
            allFoods[122].AddComponent(food122MapTransform);
            allFoods[122].AddComponent(food122Collider);
            allFoods[122].AddComponent(new ConsoleSprite(
                food122Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[123] = new GameObject("Food123");
            char[,] food123Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food123Transform = new TransformComponent(6, 20);
            MapTransformComponent food123MapTransform =
                new MapTransformComponent(2, 20);
            map.Map[
                food123MapTransform.Position.X, food123MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food123Collider = new ColliderComponent(Cell.Food);
            allFoods[123].AddComponent(food123Transform);
            allFoods[123].AddComponent(food123MapTransform);
            allFoods[123].AddComponent(food123Collider);
            allFoods[123].AddComponent(new ConsoleSprite(
                food123Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[124] = new GameObject("Food124");
            char[,] food124Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food124Transform = new TransformComponent(9, 20);
            MapTransformComponent food124MapTransform =
                new MapTransformComponent(3, 20);
            map.Map[
                food124MapTransform.Position.X, food124MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food124Collider = new ColliderComponent(Cell.Food);
            allFoods[124].AddComponent(food124Transform);
            allFoods[124].AddComponent(food124MapTransform);
            allFoods[124].AddComponent(food124Collider);
            allFoods[124].AddComponent(new ConsoleSprite(
                food124Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[125] = new GameObject("Food125");
            char[,] food125Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food125Transform = new TransformComponent(12, 20);
            MapTransformComponent food125MapTransform =
                new MapTransformComponent(4, 20);
            map.Map[
                food125MapTransform.Position.X, food125MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food125Collider = new ColliderComponent(Cell.Food);
            allFoods[125].AddComponent(food125Transform);
            allFoods[125].AddComponent(food125MapTransform);
            allFoods[125].AddComponent(food125Collider);
            allFoods[125].AddComponent(new ConsoleSprite(
                food125Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[126] = new GameObject("Food126");
            char[,] food126Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food126Transform = new TransformComponent(15, 20);
            MapTransformComponent food126MapTransform =
                new MapTransformComponent(5, 20);
            map.Map[
                food126MapTransform.Position.X, food126MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food126Collider = new ColliderComponent(Cell.Food);
            allFoods[126].AddComponent(food126Transform);
            allFoods[126].AddComponent(food126MapTransform);
            allFoods[126].AddComponent(food126Collider);
            allFoods[126].AddComponent(new ConsoleSprite(
                food126Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[127] = new GameObject("Food127");
            char[,] food127Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food127Transform = new TransformComponent(18, 20);
            MapTransformComponent food127MapTransform =
                new MapTransformComponent(6, 20);
            map.Map[
                food127MapTransform.Position.X, food127MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food127Collider = new ColliderComponent(Cell.Food);
            allFoods[127].AddComponent(food127Transform);
            allFoods[127].AddComponent(food127MapTransform);
            allFoods[127].AddComponent(food127Collider);
            allFoods[127].AddComponent(new ConsoleSprite(
                food127Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[128] = new GameObject("Food128");
            char[,] food128Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food128Transform = new TransformComponent(21, 20);
            MapTransformComponent food128MapTransform =
                new MapTransformComponent(7, 20);
            map.Map[
                food128MapTransform.Position.X, food128MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food128Collider = new ColliderComponent(Cell.Food);
            allFoods[128].AddComponent(food128Transform);
            allFoods[128].AddComponent(food128MapTransform);
            allFoods[128].AddComponent(food128Collider);
            allFoods[128].AddComponent(new ConsoleSprite(
                food128Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[129] = new GameObject("Food129");
            char[,] food129Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food129Transform = new TransformComponent(24, 20);
            MapTransformComponent food129MapTransform =
                new MapTransformComponent(8, 20);
            map.Map[
                food129MapTransform.Position.X, food129MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food129Collider = new ColliderComponent(Cell.Food);
            allFoods[129].AddComponent(food129Transform);
            allFoods[129].AddComponent(food129MapTransform);
            allFoods[129].AddComponent(food129Collider);
            allFoods[129].AddComponent(new ConsoleSprite(
                food129Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[130] = new GameObject("Food130");
            char[,] food130Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food130Transform = new TransformComponent(27, 20);
            MapTransformComponent food130MapTransform =
                new MapTransformComponent(9, 20);
            map.Map[
                food130MapTransform.Position.X, food130MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food130Collider = new ColliderComponent(Cell.Food);
            allFoods[130].AddComponent(food130Transform);
            allFoods[130].AddComponent(food130MapTransform);
            allFoods[130].AddComponent(food130Collider);
            allFoods[130].AddComponent(new ConsoleSprite(
                food130Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[131] = new GameObject("Food131");
            char[,] food131Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food131Transform = new TransformComponent(30, 20);
            MapTransformComponent food131MapTransform =
                new MapTransformComponent(10, 20);
            map.Map[
                food131MapTransform.Position.X, food131MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food131Collider = new ColliderComponent(Cell.Food);
            allFoods[131].AddComponent(food131Transform);
            allFoods[131].AddComponent(food131MapTransform);
            allFoods[131].AddComponent(food131Collider);
            allFoods[131].AddComponent(new ConsoleSprite(
                food131Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[132] = new GameObject("Food132");
            char[,] food132Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food132Transform = new TransformComponent(33, 20);
            MapTransformComponent food132MapTransform =
                new MapTransformComponent(11, 20);
            map.Map[
                food132MapTransform.Position.X, food132MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food132Collider = new ColliderComponent(Cell.Food);
            allFoods[132].AddComponent(food132Transform);
            allFoods[132].AddComponent(food132MapTransform);
            allFoods[132].AddComponent(food132Collider);
            allFoods[132].AddComponent(new ConsoleSprite(
                food132Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[133] = new GameObject("Food133");
            char[,] food133Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food133Transform = new TransformComponent(36, 20);
            MapTransformComponent food133MapTransform =
                new MapTransformComponent(12, 20);
            map.Map[
                food133MapTransform.Position.X, food133MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food133Collider = new ColliderComponent(Cell.Food);
            allFoods[133].AddComponent(food133Transform);
            allFoods[133].AddComponent(food133MapTransform);
            allFoods[133].AddComponent(food133Collider);
            allFoods[133].AddComponent(new ConsoleSprite(
                food133Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[134] = new GameObject("Food134");
            char[,] food134Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food134Transform = new TransformComponent(45, 20);
            MapTransformComponent food134MapTransform =
                new MapTransformComponent(15, 20);
            map.Map[
                food134MapTransform.Position.X, food134MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food134Collider = new ColliderComponent(Cell.Food);
            allFoods[134].AddComponent(food134Transform);
            allFoods[134].AddComponent(food134MapTransform);
            allFoods[134].AddComponent(food134Collider);
            allFoods[134].AddComponent(new ConsoleSprite(
                food134Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[135] = new GameObject("Food135");
            char[,] food135Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food135Transform = new TransformComponent(48, 20);
            MapTransformComponent food135MapTransform =
                new MapTransformComponent(16, 20);
            map.Map[
                food135MapTransform.Position.X, food135MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food135Collider = new ColliderComponent(Cell.Food);
            allFoods[135].AddComponent(food135Transform);
            allFoods[135].AddComponent(food135MapTransform);
            allFoods[135].AddComponent(food135Collider);
            allFoods[135].AddComponent(new ConsoleSprite(
                food135Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[136] = new GameObject("Food136");
            char[,] food136Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food136Transform = new TransformComponent(51, 20);
            MapTransformComponent food136MapTransform =
                new MapTransformComponent(17, 20);
            map.Map[
                food136MapTransform.Position.X, food136MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food136Collider = new ColliderComponent(Cell.Food);
            allFoods[136].AddComponent(food136Transform);
            allFoods[136].AddComponent(food136MapTransform);
            allFoods[136].AddComponent(food136Collider);
            allFoods[136].AddComponent(new ConsoleSprite(
                food136Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[137] = new GameObject("Food137");
            char[,] food137Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food137Transform = new TransformComponent(54, 20);
            MapTransformComponent food137MapTransform =
                new MapTransformComponent(18, 20);
            map.Map[
                food137MapTransform.Position.X, food137MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food137Collider = new ColliderComponent(Cell.Food);
            allFoods[137].AddComponent(food137Transform);
            allFoods[137].AddComponent(food137MapTransform);
            allFoods[137].AddComponent(food137Collider);
            allFoods[137].AddComponent(new ConsoleSprite(
                food137Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[138] = new GameObject("Food138");
            char[,] food138Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food138Transform = new TransformComponent(57, 20);
            MapTransformComponent food138MapTransform =
                new MapTransformComponent(19, 20);
            map.Map[
                food138MapTransform.Position.X, food138MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food138Collider = new ColliderComponent(Cell.Food);
            allFoods[138].AddComponent(food138Transform);
            allFoods[138].AddComponent(food138MapTransform);
            allFoods[138].AddComponent(food138Collider);
            allFoods[138].AddComponent(new ConsoleSprite(
                food138Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[139] = new GameObject("Food139");
            char[,] food139Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food139Transform = new TransformComponent(60, 20);
            MapTransformComponent food139MapTransform =
                new MapTransformComponent(20, 20);
            map.Map[
                food139MapTransform.Position.X, food139MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food139Collider = new ColliderComponent(Cell.Food);
            allFoods[139].AddComponent(food139Transform);
            allFoods[139].AddComponent(food139MapTransform);
            allFoods[139].AddComponent(food139Collider);
            allFoods[139].AddComponent(new ConsoleSprite(
                food139Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[140] = new GameObject("Food140");
            char[,] food140Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food140Transform = new TransformComponent(63, 20);
            MapTransformComponent food140MapTransform =
                new MapTransformComponent(21, 20);
            map.Map[
                food140MapTransform.Position.X, food140MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food140Collider = new ColliderComponent(Cell.Food);
            allFoods[140].AddComponent(food140Transform);
            allFoods[140].AddComponent(food140MapTransform);
            allFoods[140].AddComponent(food140Collider);
            allFoods[140].AddComponent(new ConsoleSprite(
                food140Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[141] = new GameObject("Food141");
            char[,] food141Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food141Transform = new TransformComponent(66, 20);
            MapTransformComponent food141MapTransform =
                new MapTransformComponent(22, 20);
            map.Map[
                food141MapTransform.Position.X, food141MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food141Collider = new ColliderComponent(Cell.Food);
            allFoods[141].AddComponent(food141Transform);
            allFoods[141].AddComponent(food141MapTransform);
            allFoods[141].AddComponent(food141Collider);
            allFoods[141].AddComponent(new ConsoleSprite(
                food141Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[142] = new GameObject("Food142");
            char[,] food142Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food142Transform = new TransformComponent(69, 20);
            MapTransformComponent food142MapTransform =
                new MapTransformComponent(23, 20);
            map.Map[
                food142MapTransform.Position.X, food142MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food142Collider = new ColliderComponent(Cell.Food);
            allFoods[142].AddComponent(food142Transform);
            allFoods[142].AddComponent(food142MapTransform);
            allFoods[142].AddComponent(food142Collider);
            allFoods[142].AddComponent(new ConsoleSprite(
                food142Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[143] = new GameObject("Food143");
            char[,] food143Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food143Transform = new TransformComponent(72, 20);
            MapTransformComponent food143MapTransform =
                new MapTransformComponent(24, 20);
            map.Map[
                food143MapTransform.Position.X, food143MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food143Collider = new ColliderComponent(Cell.Food);
            allFoods[143].AddComponent(food143Transform);
            allFoods[143].AddComponent(food143MapTransform);
            allFoods[143].AddComponent(food143Collider);
            allFoods[143].AddComponent(new ConsoleSprite(
                food143Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[144] = new GameObject("Food144");
            char[,] food144Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food144Transform = new TransformComponent(75, 20);
            MapTransformComponent food144MapTransform =
                new MapTransformComponent(25, 20);
            map.Map[
                food144MapTransform.Position.X, food144MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food144Collider = new ColliderComponent(Cell.Food);
            allFoods[144].AddComponent(food144Transform);
            allFoods[144].AddComponent(food144MapTransform);
            allFoods[144].AddComponent(food144Collider);
            allFoods[144].AddComponent(new ConsoleSprite(
                food144Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[145] = new GameObject("Food145");
            char[,] food145Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food145Transform = new TransformComponent(78, 20);
            MapTransformComponent food145MapTransform =
                new MapTransformComponent(26, 20);
            map.Map[
                food145MapTransform.Position.X, food145MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food145Collider = new ColliderComponent(Cell.Food);
            allFoods[145].AddComponent(food145Transform);
            allFoods[145].AddComponent(food145MapTransform);
            allFoods[145].AddComponent(food145Collider);
            allFoods[145].AddComponent(new ConsoleSprite(
                food145Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[146] = new GameObject("Food146");
            char[,] food146Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food146Transform = new TransformComponent(3, 21);
            MapTransformComponent food146MapTransform =
                new MapTransformComponent(1, 21);
            map.Map[
                food146MapTransform.Position.X, food146MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food146Collider = new ColliderComponent(Cell.Food);
            allFoods[146].AddComponent(food146Transform);
            allFoods[146].AddComponent(food146MapTransform);
            allFoods[146].AddComponent(food146Collider);
            allFoods[146].AddComponent(new ConsoleSprite(
                food146Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[147] = new GameObject("Food147");
            char[,] food147Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food147Transform = new TransformComponent(18, 21);
            MapTransformComponent food147MapTransform =
                new MapTransformComponent(6, 21);
            map.Map[
                food147MapTransform.Position.X, food147MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food147Collider = new ColliderComponent(Cell.Food);
            allFoods[147].AddComponent(food147Transform);
            allFoods[147].AddComponent(food147MapTransform);
            allFoods[147].AddComponent(food147Collider);
            allFoods[147].AddComponent(new ConsoleSprite(
                food147Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[148] = new GameObject("Food148");
            char[,] food148Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food148Transform = new TransformComponent(36, 21);
            MapTransformComponent food148MapTransform =
                new MapTransformComponent(12, 21);
            map.Map[
                food148MapTransform.Position.X, food148MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food148Collider = new ColliderComponent(Cell.Food);
            allFoods[148].AddComponent(food148Transform);
            allFoods[148].AddComponent(food148MapTransform);
            allFoods[148].AddComponent(food148Collider);
            allFoods[148].AddComponent(new ConsoleSprite(
                food148Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[149] = new GameObject("Food149");
            char[,] food149Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food149Transform = new TransformComponent(45, 21);
            MapTransformComponent food149MapTransform =
                new MapTransformComponent(15, 21);
            map.Map[
                food149MapTransform.Position.X, food149MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food149Collider = new ColliderComponent(Cell.Food);
            allFoods[149].AddComponent(food149Transform);
            allFoods[149].AddComponent(food149MapTransform);
            allFoods[149].AddComponent(food149Collider);
            allFoods[149].AddComponent(new ConsoleSprite(
                food149Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[150] = new GameObject("Food150");
            char[,] food150Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food150Transform = new TransformComponent(54, 21);
            MapTransformComponent food150MapTransform =
                new MapTransformComponent(18, 21);
            map.Map[
                food150MapTransform.Position.X, food150MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food150Collider = new ColliderComponent(Cell.Food);
            allFoods[150].AddComponent(food150Transform);
            allFoods[150].AddComponent(food150MapTransform);
            allFoods[150].AddComponent(food150Collider);
            allFoods[150].AddComponent(new ConsoleSprite(
                food150Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[151] = new GameObject("Food151");
            char[,] food151Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food151Transform = new TransformComponent(63, 21);
            MapTransformComponent food151MapTransform =
                new MapTransformComponent(21, 21);
            map.Map[
                food151MapTransform.Position.X, food151MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food151Collider = new ColliderComponent(Cell.Food);
            allFoods[151].AddComponent(food151Transform);
            allFoods[151].AddComponent(food151MapTransform);
            allFoods[151].AddComponent(food151Collider);
            allFoods[151].AddComponent(new ConsoleSprite(
                food151Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[152] = new GameObject("Food152");
            char[,] food152Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food152Transform = new TransformComponent(78, 21);
            MapTransformComponent food152MapTransform =
                new MapTransformComponent(26, 21);
            map.Map[
                food152MapTransform.Position.X, food152MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food152Collider = new ColliderComponent(Cell.Food);
            allFoods[152].AddComponent(food152Transform);
            allFoods[152].AddComponent(food152MapTransform);
            allFoods[152].AddComponent(food152Collider);
            allFoods[152].AddComponent(new ConsoleSprite(
                food152Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[153] = new GameObject("Food153");
            char[,] food153Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food153Transform = new TransformComponent(3, 22);
            MapTransformComponent food153MapTransform =
                new MapTransformComponent(1, 22);
            map.Map[
                food153MapTransform.Position.X, food153MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food153Collider = new ColliderComponent(Cell.Food);
            allFoods[153].AddComponent(food153Transform);
            allFoods[153].AddComponent(food153MapTransform);
            allFoods[153].AddComponent(food153Collider);
            allFoods[153].AddComponent(new ConsoleSprite(
                food153Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[154] = new GameObject("Food154");
            char[,] food154Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food154Transform = new TransformComponent(18, 22);
            MapTransformComponent food154MapTransform =
                new MapTransformComponent(6, 22);
            map.Map[
                food154MapTransform.Position.X, food154MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food154Collider = new ColliderComponent(Cell.Food);
            allFoods[154].AddComponent(food154Transform);
            allFoods[154].AddComponent(food154MapTransform);
            allFoods[154].AddComponent(food154Collider);
            allFoods[154].AddComponent(new ConsoleSprite(
                food154Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[155] = new GameObject("Food155");
            char[,] food155Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food155Transform = new TransformComponent(36, 22);
            MapTransformComponent food155MapTransform =
                new MapTransformComponent(12, 22);
            map.Map[
                food155MapTransform.Position.X, food155MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food155Collider = new ColliderComponent(Cell.Food);
            allFoods[155].AddComponent(food155Transform);
            allFoods[155].AddComponent(food155MapTransform);
            allFoods[155].AddComponent(food155Collider);
            allFoods[155].AddComponent(new ConsoleSprite(
                food155Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[156] = new GameObject("Food156");
            char[,] food156Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food156Transform = new TransformComponent(45, 22);
            MapTransformComponent food156MapTransform =
                new MapTransformComponent(15, 22);
            map.Map[
                food156MapTransform.Position.X, food156MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food156Collider = new ColliderComponent(Cell.Food);
            allFoods[156].AddComponent(food156Transform);
            allFoods[156].AddComponent(food156MapTransform);
            allFoods[156].AddComponent(food156Collider);
            allFoods[156].AddComponent(new ConsoleSprite(
                food156Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[157] = new GameObject("Food157");
            char[,] food157Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food157Transform = new TransformComponent(54, 22);
            MapTransformComponent food157MapTransform =
                new MapTransformComponent(18, 22);
            map.Map[
                food157MapTransform.Position.X, food157MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food157Collider = new ColliderComponent(Cell.Food);
            allFoods[157].AddComponent(food157Transform);
            allFoods[157].AddComponent(food157MapTransform);
            allFoods[157].AddComponent(food157Collider);
            allFoods[157].AddComponent(new ConsoleSprite(
                food157Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[158] = new GameObject("Food158");
            char[,] food158Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food158Transform = new TransformComponent(63, 22);
            MapTransformComponent food158MapTransform =
                new MapTransformComponent(21, 22);
            map.Map[
                food158MapTransform.Position.X, food158MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food158Collider = new ColliderComponent(Cell.Food);
            allFoods[158].AddComponent(food158Transform);
            allFoods[158].AddComponent(food158MapTransform);
            allFoods[158].AddComponent(food158Collider);
            allFoods[158].AddComponent(new ConsoleSprite(
                food158Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[159] = new GameObject("Food159");
            char[,] food159Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food159Transform = new TransformComponent(78, 22);
            MapTransformComponent food159MapTransform =
                new MapTransformComponent(26, 22);
            map.Map[
                food159MapTransform.Position.X, food159MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food159Collider = new ColliderComponent(Cell.Food);
            allFoods[159].AddComponent(food159Transform);
            allFoods[159].AddComponent(food159MapTransform);
            allFoods[159].AddComponent(food159Collider);
            allFoods[159].AddComponent(new ConsoleSprite(
                food159Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            // POWER PILL
            allFoods[161] = new GameObject("Food161");
            char[,] food161Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food161Transform = new TransformComponent(6, 23);
            MapTransformComponent food161MapTransform =
                new MapTransformComponent(2, 23);
            map.Map[
                food161MapTransform.Position.X, food161MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food161Collider = new ColliderComponent(Cell.Food);
            allFoods[161].AddComponent(food161Transform);
            allFoods[161].AddComponent(food161MapTransform);
            allFoods[161].AddComponent(food161Collider);
            allFoods[161].AddComponent(new ConsoleSprite(
                food161Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[162] = new GameObject("Food162");
            char[,] food162Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food162Transform = new TransformComponent(9, 23);
            MapTransformComponent food162MapTransform =
                new MapTransformComponent(3, 23);
            map.Map[
                food162MapTransform.Position.X, food162MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food162Collider = new ColliderComponent(Cell.Food);
            allFoods[162].AddComponent(food162Transform);
            allFoods[162].AddComponent(food162MapTransform);
            allFoods[162].AddComponent(food162Collider);
            allFoods[162].AddComponent(new ConsoleSprite(
                food162Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[163] = new GameObject("Food163");
            char[,] food163Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food163Transform = new TransformComponent(18, 23);
            MapTransformComponent food163MapTransform =
                new MapTransformComponent(6, 23);
            map.Map[
                food163MapTransform.Position.X, food163MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food163Collider = new ColliderComponent(Cell.Food);
            allFoods[163].AddComponent(food163Transform);
            allFoods[163].AddComponent(food163MapTransform);
            allFoods[163].AddComponent(food163Collider);
            allFoods[163].AddComponent(new ConsoleSprite(
                food163Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[164] = new GameObject("Food164");
            char[,] food164Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food164Transform = new TransformComponent(21, 23);
            MapTransformComponent food164MapTransform =
                new MapTransformComponent(7, 23);
            map.Map[
                food164MapTransform.Position.X, food164MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food164Collider = new ColliderComponent(Cell.Food);
            allFoods[164].AddComponent(food164Transform);
            allFoods[164].AddComponent(food164MapTransform);
            allFoods[164].AddComponent(food164Collider);
            allFoods[164].AddComponent(new ConsoleSprite(
                food164Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[165] = new GameObject("Food165");
            char[,] food165Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food165Transform = new TransformComponent(24, 23);
            MapTransformComponent food165MapTransform =
                new MapTransformComponent(8, 23);
            map.Map[
                food165MapTransform.Position.X, food165MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food165Collider = new ColliderComponent(Cell.Food);
            allFoods[165].AddComponent(food165Transform);
            allFoods[165].AddComponent(food165MapTransform);
            allFoods[165].AddComponent(food165Collider);
            allFoods[165].AddComponent(new ConsoleSprite(
                food165Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[166] = new GameObject("Food166");
            char[,] food166Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food166Transform = new TransformComponent(27, 23);
            MapTransformComponent food166MapTransform =
                new MapTransformComponent(9, 23);
            map.Map[
                food166MapTransform.Position.X, food166MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food166Collider = new ColliderComponent(Cell.Food);
            allFoods[166].AddComponent(food166Transform);
            allFoods[166].AddComponent(food166MapTransform);
            allFoods[166].AddComponent(food166Collider);
            allFoods[166].AddComponent(new ConsoleSprite(
                food166Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[167] = new GameObject("Food167");
            char[,] food167Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food167Transform = new TransformComponent(30, 23);
            MapTransformComponent food167MapTransform =
                new MapTransformComponent(10, 23);
            map.Map[
                food167MapTransform.Position.X, food167MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food167Collider = new ColliderComponent(Cell.Food);
            allFoods[167].AddComponent(food167Transform);
            allFoods[167].AddComponent(food167MapTransform);
            allFoods[167].AddComponent(food167Collider);
            allFoods[167].AddComponent(new ConsoleSprite(
                food167Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[168] = new GameObject("Food168");
            char[,] food168Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food168Transform = new TransformComponent(33, 23);
            MapTransformComponent food168MapTransform =
                new MapTransformComponent(11, 23);
            map.Map[
                food168MapTransform.Position.X, food168MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food168Collider = new ColliderComponent(Cell.Food);
            allFoods[168].AddComponent(food168Transform);
            allFoods[168].AddComponent(food168MapTransform);
            allFoods[168].AddComponent(food168Collider);
            allFoods[168].AddComponent(new ConsoleSprite(
                food168Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[169] = new GameObject("Food169");
            char[,] food169Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food169Transform = new TransformComponent(36, 23);
            MapTransformComponent food169MapTransform =
                new MapTransformComponent(12, 23);
            map.Map[
                food169MapTransform.Position.X, food169MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food169Collider = new ColliderComponent(Cell.Food);
            allFoods[169].AddComponent(food169Transform);
            allFoods[169].AddComponent(food169MapTransform);
            allFoods[169].AddComponent(food169Collider);
            allFoods[169].AddComponent(new ConsoleSprite(
                food169Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[170] = new GameObject("Food170");
            char[,] food170Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food170Transform = new TransformComponent(45, 23);
            MapTransformComponent food170MapTransform =
                new MapTransformComponent(15, 23);
            map.Map[
                food170MapTransform.Position.X, food170MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food170Collider = new ColliderComponent(Cell.Food);
            allFoods[170].AddComponent(food170Transform);
            allFoods[170].AddComponent(food170MapTransform);
            allFoods[170].AddComponent(food170Collider);
            allFoods[170].AddComponent(new ConsoleSprite(
                food170Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[171] = new GameObject("Food171");
            char[,] food171Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food171Transform = new TransformComponent(48, 23);
            MapTransformComponent food171MapTransform =
                new MapTransformComponent(16, 23);
            map.Map[
                food171MapTransform.Position.X, food171MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food171Collider = new ColliderComponent(Cell.Food);
            allFoods[171].AddComponent(food171Transform);
            allFoods[171].AddComponent(food171MapTransform);
            allFoods[171].AddComponent(food171Collider);
            allFoods[171].AddComponent(new ConsoleSprite(
                food171Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[172] = new GameObject("Food172");
            char[,] food172Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food172Transform = new TransformComponent(51, 23);
            MapTransformComponent food172MapTransform =
                new MapTransformComponent(17, 23);
            map.Map[
                food172MapTransform.Position.X, food172MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food172Collider = new ColliderComponent(Cell.Food);
            allFoods[172].AddComponent(food172Transform);
            allFoods[172].AddComponent(food172MapTransform);
            allFoods[172].AddComponent(food172Collider);
            allFoods[172].AddComponent(new ConsoleSprite(
                food172Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[173] = new GameObject("Food173");
            char[,] food173Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food173Transform = new TransformComponent(54, 23);
            MapTransformComponent food173MapTransform =
                new MapTransformComponent(18, 23);
            map.Map[
                food173MapTransform.Position.X, food173MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food173Collider = new ColliderComponent(Cell.Food);
            allFoods[173].AddComponent(food173Transform);
            allFoods[173].AddComponent(food173MapTransform);
            allFoods[173].AddComponent(food173Collider);
            allFoods[173].AddComponent(new ConsoleSprite(
                food173Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[174] = new GameObject("Food174");
            char[,] food174Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food174Transform = new TransformComponent(57, 23);
            MapTransformComponent food174MapTransform =
                new MapTransformComponent(19, 23);
            map.Map[
                food174MapTransform.Position.X, food174MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food174Collider = new ColliderComponent(Cell.Food);
            allFoods[174].AddComponent(food174Transform);
            allFoods[174].AddComponent(food174MapTransform);
            allFoods[174].AddComponent(food174Collider);
            allFoods[174].AddComponent(new ConsoleSprite(
                food174Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[175] = new GameObject("Food175");
            char[,] food175Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food175Transform = new TransformComponent(60, 23);
            MapTransformComponent food175MapTransform =
                new MapTransformComponent(20, 23);
            map.Map[
                food175MapTransform.Position.X, food175MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food175Collider = new ColliderComponent(Cell.Food);
            allFoods[175].AddComponent(food175Transform);
            allFoods[175].AddComponent(food175MapTransform);
            allFoods[175].AddComponent(food175Collider);
            allFoods[175].AddComponent(new ConsoleSprite(
                food175Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[176] = new GameObject("Food176");
            char[,] food176Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food176Transform = new TransformComponent(63, 23);
            MapTransformComponent food176MapTransform =
                new MapTransformComponent(21, 23);
            map.Map[
                food176MapTransform.Position.X, food176MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food176Collider = new ColliderComponent(Cell.Food);
            allFoods[176].AddComponent(food176Transform);
            allFoods[176].AddComponent(food176MapTransform);
            allFoods[176].AddComponent(food176Collider);
            allFoods[176].AddComponent(new ConsoleSprite(
                food176Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[177] = new GameObject("Food177");
            char[,] food177Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food177Transform = new TransformComponent(72, 23);
            MapTransformComponent food177MapTransform =
                new MapTransformComponent(24, 23);
            map.Map[
                food177MapTransform.Position.X, food177MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food177Collider = new ColliderComponent(Cell.Food);
            allFoods[177].AddComponent(food177Transform);
            allFoods[177].AddComponent(food177MapTransform);
            allFoods[177].AddComponent(food177Collider);
            allFoods[177].AddComponent(new ConsoleSprite(
                food177Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[178] = new GameObject("Food178");
            char[,] food178Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food178Transform = new TransformComponent(75, 23);
            MapTransformComponent food178MapTransform =
                new MapTransformComponent(25, 23);
            map.Map[
                food178MapTransform.Position.X, food178MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food178Collider = new ColliderComponent(Cell.Food);
            allFoods[178].AddComponent(food178Transform);
            allFoods[178].AddComponent(food178MapTransform);
            allFoods[178].AddComponent(food178Collider);
            allFoods[178].AddComponent(new ConsoleSprite(
                food178Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            // POWER PILL
            allFoods[180] = new GameObject("Food180");
            char[,] food180Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food180Transform = new TransformComponent(9, 24);
            MapTransformComponent food180MapTransform =
                new MapTransformComponent(3, 24);
            map.Map[
                food180MapTransform.Position.X, food180MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food180Collider = new ColliderComponent(Cell.Food);
            allFoods[180].AddComponent(food180Transform);
            allFoods[180].AddComponent(food180MapTransform);
            allFoods[180].AddComponent(food180Collider);
            allFoods[180].AddComponent(new ConsoleSprite(
                food180Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[181] = new GameObject("Food181");
            char[,] food181Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food181Transform = new TransformComponent(18, 24);
            MapTransformComponent food181MapTransform =
                new MapTransformComponent(6, 24);
            map.Map[
                food181MapTransform.Position.X, food181MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food181Collider = new ColliderComponent(Cell.Food);
            allFoods[181].AddComponent(food181Transform);
            allFoods[181].AddComponent(food181MapTransform);
            allFoods[181].AddComponent(food181Collider);
            allFoods[181].AddComponent(new ConsoleSprite(
                food181Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[182] = new GameObject("Food182");
            char[,] food182Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food182Transform = new TransformComponent(27, 24);
            MapTransformComponent food182MapTransform =
                new MapTransformComponent(9, 24);
            map.Map[
                food182MapTransform.Position.X, food182MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food182Collider = new ColliderComponent(Cell.Food);
            allFoods[182].AddComponent(food182Transform);
            allFoods[182].AddComponent(food182MapTransform);
            allFoods[182].AddComponent(food182Collider);
            allFoods[182].AddComponent(new ConsoleSprite(
                food182Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[183] = new GameObject("Food183");
            char[,] food183Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food183Transform = new TransformComponent(54, 24);
            MapTransformComponent food183MapTransform =
                new MapTransformComponent(18, 24);
            map.Map[
                food183MapTransform.Position.X, food183MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food183Collider = new ColliderComponent(Cell.Food);
            allFoods[183].AddComponent(food183Transform);
            allFoods[183].AddComponent(food183MapTransform);
            allFoods[183].AddComponent(food183Collider);
            allFoods[183].AddComponent(new ConsoleSprite(
                food183Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[184] = new GameObject("Food184");
            char[,] food184Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food184Transform = new TransformComponent(63, 24);
            MapTransformComponent food184MapTransform =
                new MapTransformComponent(21, 24);
            map.Map[
                food184MapTransform.Position.X, food184MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food184Collider = new ColliderComponent(Cell.Food);
            allFoods[184].AddComponent(food184Transform);
            allFoods[184].AddComponent(food184MapTransform);
            allFoods[184].AddComponent(food184Collider);
            allFoods[184].AddComponent(new ConsoleSprite(
                food184Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[185] = new GameObject("Food185");
            char[,] food185Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food185Transform = new TransformComponent(72, 24);
            MapTransformComponent food185MapTransform =
                new MapTransformComponent(24, 24);
            map.Map[
                food185MapTransform.Position.X, food185MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food185Collider = new ColliderComponent(Cell.Food);
            allFoods[185].AddComponent(food185Transform);
            allFoods[185].AddComponent(food185MapTransform);
            allFoods[185].AddComponent(food185Collider);
            allFoods[185].AddComponent(new ConsoleSprite(
                food185Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[186] = new GameObject("Food186");
            char[,] food186Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food186Transform = new TransformComponent(9, 25);
            MapTransformComponent food186MapTransform =
                new MapTransformComponent(3, 25);
            map.Map[
                food186MapTransform.Position.X, food186MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food186Collider = new ColliderComponent(Cell.Food);
            allFoods[186].AddComponent(food186Transform);
            allFoods[186].AddComponent(food186MapTransform);
            allFoods[186].AddComponent(food186Collider);
            allFoods[186].AddComponent(new ConsoleSprite(
                food186Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[187] = new GameObject("Food187");
            char[,] food187Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food187Transform = new TransformComponent(18, 25);
            MapTransformComponent food187MapTransform =
                new MapTransformComponent(6, 25);
            map.Map[
                food187MapTransform.Position.X, food187MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food187Collider = new ColliderComponent(Cell.Food);
            allFoods[187].AddComponent(food187Transform);
            allFoods[187].AddComponent(food187MapTransform);
            allFoods[187].AddComponent(food187Collider);
            allFoods[187].AddComponent(new ConsoleSprite(
                food187Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[188] = new GameObject("Food188");
            char[,] food188Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food188Transform = new TransformComponent(27, 25);
            MapTransformComponent food188MapTransform =
                new MapTransformComponent(9, 25);
            map.Map[
                food188MapTransform.Position.X, food188MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food188Collider = new ColliderComponent(Cell.Food);
            allFoods[188].AddComponent(food188Transform);
            allFoods[188].AddComponent(food188MapTransform);
            allFoods[188].AddComponent(food188Collider);
            allFoods[188].AddComponent(new ConsoleSprite(
                food188Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[189] = new GameObject("Food189");
            char[,] food189Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food189Transform = new TransformComponent(54, 25);
            MapTransformComponent food189MapTransform =
                new MapTransformComponent(18, 25);
            map.Map[
                food189MapTransform.Position.X, food189MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food189Collider = new ColliderComponent(Cell.Food);
            allFoods[189].AddComponent(food189Transform);
            allFoods[189].AddComponent(food189MapTransform);
            allFoods[189].AddComponent(food189Collider);
            allFoods[189].AddComponent(new ConsoleSprite(
                food189Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[190] = new GameObject("Food190");
            char[,] food190Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food190Transform = new TransformComponent(63, 25);
            MapTransformComponent food190MapTransform =
                new MapTransformComponent(21, 25);
            map.Map[
                food190MapTransform.Position.X, food190MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food190Collider = new ColliderComponent(Cell.Food);
            allFoods[190].AddComponent(food190Transform);
            allFoods[190].AddComponent(food190MapTransform);
            allFoods[190].AddComponent(food190Collider);
            allFoods[190].AddComponent(new ConsoleSprite(
                food190Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[191] = new GameObject("Food191");
            char[,] food191Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food191Transform = new TransformComponent(72, 25);
            MapTransformComponent food191MapTransform =
                new MapTransformComponent(24, 25);
            map.Map[
                food191MapTransform.Position.X, food191MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food191Collider = new ColliderComponent(Cell.Food);
            allFoods[191].AddComponent(food191Transform);
            allFoods[191].AddComponent(food191MapTransform);
            allFoods[191].AddComponent(food191Collider);
            allFoods[191].AddComponent(new ConsoleSprite(
                food191Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[192] = new GameObject("Food192");
            char[,] food192Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food192Transform = new TransformComponent(3, 26);
            MapTransformComponent food192MapTransform =
                new MapTransformComponent(1, 26);
            map.Map[
                food192MapTransform.Position.X, food192MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food192Collider = new ColliderComponent(Cell.Food);
            allFoods[192].AddComponent(food192Transform);
            allFoods[192].AddComponent(food192MapTransform);
            allFoods[192].AddComponent(food192Collider);
            allFoods[192].AddComponent(new ConsoleSprite(
                food192Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[193] = new GameObject("Food193");
            char[,] food193Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food193Transform = new TransformComponent(6, 26);
            MapTransformComponent food193MapTransform =
                new MapTransformComponent(2, 26);
            map.Map[
                food193MapTransform.Position.X, food193MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food193Collider = new ColliderComponent(Cell.Food);
            allFoods[193].AddComponent(food193Transform);
            allFoods[193].AddComponent(food193MapTransform);
            allFoods[193].AddComponent(food193Collider);
            allFoods[193].AddComponent(new ConsoleSprite(
                food193Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[194] = new GameObject("Food194");
            char[,] food194Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food194Transform = new TransformComponent(9, 26);
            MapTransformComponent food194MapTransform =
                new MapTransformComponent(3, 26);
            map.Map[
                food194MapTransform.Position.X, food194MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food194Collider = new ColliderComponent(Cell.Food);
            allFoods[194].AddComponent(food194Transform);
            allFoods[194].AddComponent(food194MapTransform);
            allFoods[194].AddComponent(food194Collider);
            allFoods[194].AddComponent(new ConsoleSprite(
                food194Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[195] = new GameObject("Food195");
            char[,] food195Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food195Transform = new TransformComponent(12, 26);
            MapTransformComponent food195MapTransform =
                new MapTransformComponent(4, 26);
            map.Map[
                food195MapTransform.Position.X, food195MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food195Collider = new ColliderComponent(Cell.Food);
            allFoods[195].AddComponent(food195Transform);
            allFoods[195].AddComponent(food195MapTransform);
            allFoods[195].AddComponent(food195Collider);
            allFoods[195].AddComponent(new ConsoleSprite(
                food195Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[196] = new GameObject("Food196");
            char[,] food196Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food196Transform = new TransformComponent(15, 26);
            MapTransformComponent food196MapTransform =
                new MapTransformComponent(5, 26);
            map.Map[
                food196MapTransform.Position.X, food196MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food196Collider = new ColliderComponent(Cell.Food);
            allFoods[196].AddComponent(food196Transform);
            allFoods[196].AddComponent(food196MapTransform);
            allFoods[196].AddComponent(food196Collider);
            allFoods[196].AddComponent(new ConsoleSprite(
                food196Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[197] = new GameObject("Food197");
            char[,] food197Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food197Transform = new TransformComponent(18, 26);
            MapTransformComponent food197MapTransform =
                new MapTransformComponent(6, 26);
            map.Map[
                food197MapTransform.Position.X, food197MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food197Collider = new ColliderComponent(Cell.Food);
            allFoods[197].AddComponent(food197Transform);
            allFoods[197].AddComponent(food197MapTransform);
            allFoods[197].AddComponent(food197Collider);
            allFoods[197].AddComponent(new ConsoleSprite(
                food197Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[198] = new GameObject("Food198");
            char[,] food198Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food198Transform = new TransformComponent(27, 26);
            MapTransformComponent food198MapTransform =
                new MapTransformComponent(9, 26);
            map.Map[
                food198MapTransform.Position.X, food198MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food198Collider = new ColliderComponent(Cell.Food);
            allFoods[198].AddComponent(food198Transform);
            allFoods[198].AddComponent(food198MapTransform);
            allFoods[198].AddComponent(food198Collider);
            allFoods[198].AddComponent(new ConsoleSprite(
                food198Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[199] = new GameObject("Food199");
            char[,] food199Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food199Transform = new TransformComponent(30, 26);
            MapTransformComponent food199MapTransform =
                new MapTransformComponent(10, 26);
            map.Map[
                food199MapTransform.Position.X, food199MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food199Collider = new ColliderComponent(Cell.Food);
            allFoods[199].AddComponent(food199Transform);
            allFoods[199].AddComponent(food199MapTransform);
            allFoods[199].AddComponent(food199Collider);
            allFoods[199].AddComponent(new ConsoleSprite(
                food199Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[200] = new GameObject("Food200");
            char[,] food200Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food200Transform = new TransformComponent(33, 26);
            MapTransformComponent food200MapTransform =
                new MapTransformComponent(11, 26);
            map.Map[
                food200MapTransform.Position.X, food200MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food200Collider = new ColliderComponent(Cell.Food);
            allFoods[200].AddComponent(food200Transform);
            allFoods[200].AddComponent(food200MapTransform);
            allFoods[200].AddComponent(food200Collider);
            allFoods[200].AddComponent(new ConsoleSprite(
                food200Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[201] = new GameObject("Food201");
            char[,] food201Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food201Transform = new TransformComponent(36, 26);
            MapTransformComponent food201MapTransform =
                new MapTransformComponent(12, 26);
            map.Map[
                food201MapTransform.Position.X, food201MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food201Collider = new ColliderComponent(Cell.Food);
            allFoods[201].AddComponent(food201Transform);
            allFoods[201].AddComponent(food201MapTransform);
            allFoods[201].AddComponent(food201Collider);
            allFoods[201].AddComponent(new ConsoleSprite(
                food201Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[202] = new GameObject("Food202");
            char[,] food202Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food202Transform = new TransformComponent(45, 26);
            MapTransformComponent food202MapTransform =
                new MapTransformComponent(15, 26);
            map.Map[
                food202MapTransform.Position.X, food202MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food202Collider = new ColliderComponent(Cell.Food);
            allFoods[202].AddComponent(food202Transform);
            allFoods[202].AddComponent(food202MapTransform);
            allFoods[202].AddComponent(food202Collider);
            allFoods[202].AddComponent(new ConsoleSprite(
                food202Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[203] = new GameObject("Food203");
            char[,] food203Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food203Transform = new TransformComponent(48, 26);
            MapTransformComponent food203MapTransform =
                new MapTransformComponent(16, 26);
            map.Map[
                food203MapTransform.Position.X, food203MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food203Collider = new ColliderComponent(Cell.Food);
            allFoods[203].AddComponent(food203Transform);
            allFoods[203].AddComponent(food203MapTransform);
            allFoods[203].AddComponent(food203Collider);
            allFoods[203].AddComponent(new ConsoleSprite(
                food203Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[204] = new GameObject("Food204");
            char[,] food204Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food204Transform = new TransformComponent(51, 26);
            MapTransformComponent food204MapTransform =
                new MapTransformComponent(17, 26);
            map.Map[
                food204MapTransform.Position.X, food204MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food204Collider = new ColliderComponent(Cell.Food);
            allFoods[204].AddComponent(food204Transform);
            allFoods[204].AddComponent(food204MapTransform);
            allFoods[204].AddComponent(food204Collider);
            allFoods[204].AddComponent(new ConsoleSprite(
                food204Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[205] = new GameObject("Food205");
            char[,] food205Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food205Transform = new TransformComponent(54, 26);
            MapTransformComponent food205MapTransform =
                new MapTransformComponent(18, 26);
            map.Map[
                food205MapTransform.Position.X, food205MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food205Collider = new ColliderComponent(Cell.Food);
            allFoods[205].AddComponent(food205Transform);
            allFoods[205].AddComponent(food205MapTransform);
            allFoods[205].AddComponent(food205Collider);
            allFoods[205].AddComponent(new ConsoleSprite(
                food205Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[206] = new GameObject("Food206");
            char[,] food206Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food206Transform = new TransformComponent(63, 26);
            MapTransformComponent food206MapTransform =
                new MapTransformComponent(21, 26);
            map.Map[
                food206MapTransform.Position.X, food206MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food206Collider = new ColliderComponent(Cell.Food);
            allFoods[206].AddComponent(food206Transform);
            allFoods[206].AddComponent(food206MapTransform);
            allFoods[206].AddComponent(food206Collider);
            allFoods[206].AddComponent(new ConsoleSprite(
                food206Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[207] = new GameObject("Food207");
            char[,] food207Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food207Transform = new TransformComponent(66, 26);
            MapTransformComponent food207MapTransform =
                new MapTransformComponent(22, 26);
            map.Map[
                food207MapTransform.Position.X, food207MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food207Collider = new ColliderComponent(Cell.Food);
            allFoods[207].AddComponent(food207Transform);
            allFoods[207].AddComponent(food207MapTransform);
            allFoods[207].AddComponent(food207Collider);
            allFoods[207].AddComponent(new ConsoleSprite(
                food207Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[208] = new GameObject("Food208");
            char[,] food208Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food208Transform = new TransformComponent(69, 26);
            MapTransformComponent food208MapTransform =
                new MapTransformComponent(23, 26);
            map.Map[
                food208MapTransform.Position.X, food208MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food208Collider = new ColliderComponent(Cell.Food);
            allFoods[208].AddComponent(food208Transform);
            allFoods[208].AddComponent(food208MapTransform);
            allFoods[208].AddComponent(food208Collider);
            allFoods[208].AddComponent(new ConsoleSprite(
                food208Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[209] = new GameObject("Food209");
            char[,] food209Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food209Transform = new TransformComponent(72, 26);
            MapTransformComponent food209MapTransform =
                new MapTransformComponent(24, 26);
            map.Map[
                food209MapTransform.Position.X, food209MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food209Collider = new ColliderComponent(Cell.Food);
            allFoods[209].AddComponent(food209Transform);
            allFoods[209].AddComponent(food209MapTransform);
            allFoods[209].AddComponent(food209Collider);
            allFoods[209].AddComponent(new ConsoleSprite(
                food209Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[210] = new GameObject("Food210");
            char[,] food210Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food210Transform = new TransformComponent(75, 26);
            MapTransformComponent food210MapTransform =
                new MapTransformComponent(25, 26);
            map.Map[
                food210MapTransform.Position.X, food210MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food210Collider = new ColliderComponent(Cell.Food);
            allFoods[210].AddComponent(food210Transform);
            allFoods[210].AddComponent(food210MapTransform);
            allFoods[210].AddComponent(food210Collider);
            allFoods[210].AddComponent(new ConsoleSprite(
                food210Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[211] = new GameObject("Food211");
            char[,] food211Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food211Transform = new TransformComponent(78, 26);
            MapTransformComponent food211MapTransform =
                new MapTransformComponent(26, 26);
            map.Map[
                food211MapTransform.Position.X, food211MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food211Collider = new ColliderComponent(Cell.Food);
            allFoods[211].AddComponent(food211Transform);
            allFoods[211].AddComponent(food211MapTransform);
            allFoods[211].AddComponent(food211Collider);
            allFoods[211].AddComponent(new ConsoleSprite(
                food211Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[212] = new GameObject("Food212");
            char[,] food212Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food212Transform = new TransformComponent(3, 27);
            MapTransformComponent food212MapTransform =
                new MapTransformComponent(1, 27);
            map.Map[
                food212MapTransform.Position.X, food212MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food212Collider = new ColliderComponent(Cell.Food);
            allFoods[212].AddComponent(food212Transform);
            allFoods[212].AddComponent(food212MapTransform);
            allFoods[212].AddComponent(food212Collider);
            allFoods[212].AddComponent(new ConsoleSprite(
                food212Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[213] = new GameObject("Food213");
            char[,] food213Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food213Transform = new TransformComponent(36, 27);
            MapTransformComponent food213MapTransform =
                new MapTransformComponent(12, 27);
            map.Map[
                food213MapTransform.Position.X, food213MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food213Collider = new ColliderComponent(Cell.Food);
            allFoods[213].AddComponent(food213Transform);
            allFoods[213].AddComponent(food213MapTransform);
            allFoods[213].AddComponent(food213Collider);
            allFoods[213].AddComponent(new ConsoleSprite(
                food213Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[214] = new GameObject("Food214");
            char[,] food214Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food214Transform = new TransformComponent(45, 27);
            MapTransformComponent food214MapTransform =
                new MapTransformComponent(15, 27);
            map.Map[
                food214MapTransform.Position.X, food214MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food214Collider = new ColliderComponent(Cell.Food);
            allFoods[214].AddComponent(food214Transform);
            allFoods[214].AddComponent(food214MapTransform);
            allFoods[214].AddComponent(food214Collider);
            allFoods[214].AddComponent(new ConsoleSprite(
                food214Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[215] = new GameObject("Food215");
            char[,] food215Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food215Transform = new TransformComponent(78, 27);
            MapTransformComponent food215MapTransform =
                new MapTransformComponent(26, 27);
            map.Map[
                food215MapTransform.Position.X, food215MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food215Collider = new ColliderComponent(Cell.Food);
            allFoods[215].AddComponent(food215Transform);
            allFoods[215].AddComponent(food215MapTransform);
            allFoods[215].AddComponent(food215Collider);
            allFoods[215].AddComponent(new ConsoleSprite(
                food215Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[216] = new GameObject("Food216");
            char[,] food216Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food216Transform = new TransformComponent(3, 28);
            MapTransformComponent food216MapTransform =
                new MapTransformComponent(1, 28);
            map.Map[
                food216MapTransform.Position.X, food216MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food216Collider = new ColliderComponent(Cell.Food);
            allFoods[216].AddComponent(food216Transform);
            allFoods[216].AddComponent(food216MapTransform);
            allFoods[216].AddComponent(food216Collider);
            allFoods[216].AddComponent(new ConsoleSprite(
                food216Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[217] = new GameObject("Food217");
            char[,] food217Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food217Transform = new TransformComponent(36, 28);
            MapTransformComponent food217MapTransform =
                new MapTransformComponent(12, 28);
            map.Map[
                food217MapTransform.Position.X, food217MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food217Collider = new ColliderComponent(Cell.Food);
            allFoods[217].AddComponent(food217Transform);
            allFoods[217].AddComponent(food217MapTransform);
            allFoods[217].AddComponent(food217Collider);
            allFoods[217].AddComponent(new ConsoleSprite(
                food217Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[218] = new GameObject("Food218");
            char[,] food218Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food218Transform = new TransformComponent(45, 28);
            MapTransformComponent food218MapTransform =
                new MapTransformComponent(15, 28);
            map.Map[
                food218MapTransform.Position.X, food218MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food218Collider = new ColliderComponent(Cell.Food);
            allFoods[218].AddComponent(food218Transform);
            allFoods[218].AddComponent(food218MapTransform);
            allFoods[218].AddComponent(food218Collider);
            allFoods[218].AddComponent(new ConsoleSprite(
                food218Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[219] = new GameObject("Food219");
            char[,] food219Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food219Transform = new TransformComponent(78, 28);
            MapTransformComponent food219MapTransform =
                new MapTransformComponent(26, 28);
            map.Map[
                food219MapTransform.Position.X, food219MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food219Collider = new ColliderComponent(Cell.Food);
            allFoods[219].AddComponent(food219Transform);
            allFoods[219].AddComponent(food219MapTransform);
            allFoods[219].AddComponent(food219Collider);
            allFoods[219].AddComponent(new ConsoleSprite(
                food219Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[220] = new GameObject("Food220");
            char[,] food220Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food220Transform = new TransformComponent(3, 29);
            MapTransformComponent food220MapTransform =
                new MapTransformComponent(1, 29);
            map.Map[
                food220MapTransform.Position.X, food220MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food220Collider = new ColliderComponent(Cell.Food);
            allFoods[220].AddComponent(food220Transform);
            allFoods[220].AddComponent(food220MapTransform);
            allFoods[220].AddComponent(food220Collider);
            allFoods[220].AddComponent(new ConsoleSprite(
                food220Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[221] = new GameObject("Food221");
            char[,] food221Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food221Transform = new TransformComponent(6, 29);
            MapTransformComponent food221MapTransform =
                new MapTransformComponent(2, 29);
            map.Map[
                food221MapTransform.Position.X, food221MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food221Collider = new ColliderComponent(Cell.Food);
            allFoods[221].AddComponent(food221Transform);
            allFoods[221].AddComponent(food221MapTransform);
            allFoods[221].AddComponent(food221Collider);
            allFoods[221].AddComponent(new ConsoleSprite(
                food221Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[222] = new GameObject("Food222");
            char[,] food222Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food222Transform = new TransformComponent(9, 29);
            MapTransformComponent food222MapTransform =
                new MapTransformComponent(3, 29);
            map.Map[
                food222MapTransform.Position.X, food222MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food222Collider = new ColliderComponent(Cell.Food);
            allFoods[222].AddComponent(food222Transform);
            allFoods[222].AddComponent(food222MapTransform);
            allFoods[222].AddComponent(food222Collider);
            allFoods[222].AddComponent(new ConsoleSprite(
                food222Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[223] = new GameObject("Food223");
            char[,] food223Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food223Transform = new TransformComponent(12, 29);
            MapTransformComponent food223MapTransform =
                new MapTransformComponent(4, 29);
            map.Map[
                food223MapTransform.Position.X, food223MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food223Collider = new ColliderComponent(Cell.Food);
            allFoods[223].AddComponent(food223Transform);
            allFoods[223].AddComponent(food223MapTransform);
            allFoods[223].AddComponent(food223Collider);
            allFoods[223].AddComponent(new ConsoleSprite(
                food223Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[224] = new GameObject("Food224");
            char[,] food224Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food224Transform = new TransformComponent(15, 29);
            MapTransformComponent food224MapTransform =
                new MapTransformComponent(5, 29);
            map.Map[
                food224MapTransform.Position.X, food224MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food224Collider = new ColliderComponent(Cell.Food);
            allFoods[224].AddComponent(food224Transform);
            allFoods[224].AddComponent(food224MapTransform);
            allFoods[224].AddComponent(food224Collider);
            allFoods[224].AddComponent(new ConsoleSprite(
                food224Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[225] = new GameObject("Food225");
            char[,] food225Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food225Transform = new TransformComponent(18, 29);
            MapTransformComponent food225MapTransform =
                new MapTransformComponent(6, 29);
            map.Map[
                food225MapTransform.Position.X, food225MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food225Collider = new ColliderComponent(Cell.Food);
            allFoods[225].AddComponent(food225Transform);
            allFoods[225].AddComponent(food225MapTransform);
            allFoods[225].AddComponent(food225Collider);
            allFoods[225].AddComponent(new ConsoleSprite(
                food225Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[226] = new GameObject("Food226");
            char[,] food226Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food226Transform = new TransformComponent(21, 29);
            MapTransformComponent food226MapTransform =
                new MapTransformComponent(7, 29);
            map.Map[
                food226MapTransform.Position.X, food226MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food226Collider = new ColliderComponent(Cell.Food);
            allFoods[226].AddComponent(food226Transform);
            allFoods[226].AddComponent(food226MapTransform);
            allFoods[226].AddComponent(food226Collider);
            allFoods[226].AddComponent(new ConsoleSprite(
                food226Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[227] = new GameObject("Food227");
            char[,] food227Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food227Transform = new TransformComponent(24, 29);
            MapTransformComponent food227MapTransform =
                new MapTransformComponent(8, 29);
            map.Map[
                food227MapTransform.Position.X, food227MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food227Collider = new ColliderComponent(Cell.Food);
            allFoods[227].AddComponent(food227Transform);
            allFoods[227].AddComponent(food227MapTransform);
            allFoods[227].AddComponent(food227Collider);
            allFoods[227].AddComponent(new ConsoleSprite(
                food227Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[228] = new GameObject("Food228");
            char[,] food228Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food228Transform = new TransformComponent(27, 29);
            MapTransformComponent food228MapTransform =
                new MapTransformComponent(9, 29);
            map.Map[
                food228MapTransform.Position.X, food228MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food228Collider = new ColliderComponent(Cell.Food);
            allFoods[228].AddComponent(food228Transform);
            allFoods[228].AddComponent(food228MapTransform);
            allFoods[228].AddComponent(food228Collider);
            allFoods[228].AddComponent(new ConsoleSprite(
                food228Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[229] = new GameObject("Food229");
            char[,] food229Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food229Transform = new TransformComponent(30, 29);
            MapTransformComponent food229MapTransform =
                new MapTransformComponent(10, 29);
            map.Map[
                food229MapTransform.Position.X, food229MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food229Collider = new ColliderComponent(Cell.Food);
            allFoods[229].AddComponent(food229Transform);
            allFoods[229].AddComponent(food229MapTransform);
            allFoods[229].AddComponent(food229Collider);
            allFoods[229].AddComponent(new ConsoleSprite(
                food229Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[230] = new GameObject("Food230");
            char[,] food230Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food230Transform = new TransformComponent(33, 29);
            MapTransformComponent food230MapTransform =
                new MapTransformComponent(11, 29);
            map.Map[
                food230MapTransform.Position.X, food230MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food230Collider = new ColliderComponent(Cell.Food);
            allFoods[230].AddComponent(food230Transform);
            allFoods[230].AddComponent(food230MapTransform);
            allFoods[230].AddComponent(food230Collider);
            allFoods[230].AddComponent(new ConsoleSprite(
                food230Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[231] = new GameObject("Food231");
            char[,] food231Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food231Transform = new TransformComponent(36, 29);
            MapTransformComponent food231MapTransform =
                new MapTransformComponent(12, 29);
            map.Map[
                food231MapTransform.Position.X, food231MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food231Collider = new ColliderComponent(Cell.Food);
            allFoods[231].AddComponent(food231Transform);
            allFoods[231].AddComponent(food231MapTransform);
            allFoods[231].AddComponent(food231Collider);
            allFoods[231].AddComponent(new ConsoleSprite(
                food231Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[232] = new GameObject("Food232");
            char[,] food232Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food232Transform = new TransformComponent(39, 29);
            MapTransformComponent food232MapTransform =
                new MapTransformComponent(13, 29);
            map.Map[
                food232MapTransform.Position.X, food232MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food232Collider = new ColliderComponent(Cell.Food);
            allFoods[232].AddComponent(food232Transform);
            allFoods[232].AddComponent(food232MapTransform);
            allFoods[232].AddComponent(food232Collider);
            allFoods[232].AddComponent(new ConsoleSprite(
                food232Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[233] = new GameObject("Food233");
            char[,] food233Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food233Transform = new TransformComponent(42, 29);
            MapTransformComponent food233MapTransform =
                new MapTransformComponent(14, 29);
            map.Map[
                food233MapTransform.Position.X, food233MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food233Collider = new ColliderComponent(Cell.Food);
            allFoods[233].AddComponent(food233Transform);
            allFoods[233].AddComponent(food233MapTransform);
            allFoods[233].AddComponent(food233Collider);
            allFoods[233].AddComponent(new ConsoleSprite(
                food233Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[234] = new GameObject("Food234");
            char[,] food234Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food234Transform = new TransformComponent(45, 29);
            MapTransformComponent food234MapTransform =
                new MapTransformComponent(15, 29);
            map.Map[
                food234MapTransform.Position.X, food234MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food234Collider = new ColliderComponent(Cell.Food);
            allFoods[234].AddComponent(food234Transform);
            allFoods[234].AddComponent(food234MapTransform);
            allFoods[234].AddComponent(food234Collider);
            allFoods[234].AddComponent(new ConsoleSprite(
                food234Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[235] = new GameObject("Food235");
            char[,] food235Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food235Transform = new TransformComponent(48, 29);
            MapTransformComponent food235MapTransform =
                new MapTransformComponent(16, 29);
            map.Map[
                food235MapTransform.Position.X, food235MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food235Collider = new ColliderComponent(Cell.Food);
            allFoods[235].AddComponent(food235Transform);
            allFoods[235].AddComponent(food235MapTransform);
            allFoods[235].AddComponent(food235Collider);
            allFoods[235].AddComponent(new ConsoleSprite(
                food235Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[236] = new GameObject("Food236");
            char[,] food236Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food236Transform = new TransformComponent(51, 29);
            MapTransformComponent food236MapTransform =
                new MapTransformComponent(17, 29);
            map.Map[
                food236MapTransform.Position.X, food236MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food236Collider = new ColliderComponent(Cell.Food);
            allFoods[236].AddComponent(food236Transform);
            allFoods[236].AddComponent(food236MapTransform);
            allFoods[236].AddComponent(food236Collider);
            allFoods[236].AddComponent(new ConsoleSprite(
                food236Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[237] = new GameObject("Food237");
            char[,] food237Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food237Transform = new TransformComponent(54, 29);
            MapTransformComponent food237MapTransform =
                new MapTransformComponent(18, 29);
            map.Map[
                food237MapTransform.Position.X, food237MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food237Collider = new ColliderComponent(Cell.Food);
            allFoods[237].AddComponent(food237Transform);
            allFoods[237].AddComponent(food237MapTransform);
            allFoods[237].AddComponent(food237Collider);
            allFoods[237].AddComponent(new ConsoleSprite(
                food237Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[238] = new GameObject("Food238");
            char[,] food238Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food238Transform = new TransformComponent(57, 29);
            MapTransformComponent food238MapTransform =
                new MapTransformComponent(19, 29);
            map.Map[
                food238MapTransform.Position.X, food238MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food238Collider = new ColliderComponent(Cell.Food);
            allFoods[238].AddComponent(food238Transform);
            allFoods[238].AddComponent(food238MapTransform);
            allFoods[238].AddComponent(food238Collider);
            allFoods[238].AddComponent(new ConsoleSprite(
                food238Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[239] = new GameObject("Food239");
            char[,] food239Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food239Transform = new TransformComponent(60, 29);
            MapTransformComponent food239MapTransform =
                new MapTransformComponent(20, 29);
            map.Map[
                food239MapTransform.Position.X, food239MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food239Collider = new ColliderComponent(Cell.Food);
            allFoods[239].AddComponent(food239Transform);
            allFoods[239].AddComponent(food239MapTransform);
            allFoods[239].AddComponent(food239Collider);
            allFoods[239].AddComponent(new ConsoleSprite(
                food239Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[240] = new GameObject("Food240");
            char[,] food240Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food240Transform = new TransformComponent(63, 29);
            MapTransformComponent food240MapTransform =
                new MapTransformComponent(21, 29);
            map.Map[
                food240MapTransform.Position.X, food240MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food240Collider = new ColliderComponent(Cell.Food);
            allFoods[240].AddComponent(food240Transform);
            allFoods[240].AddComponent(food240MapTransform);
            allFoods[240].AddComponent(food240Collider);
            allFoods[240].AddComponent(new ConsoleSprite(
                food240Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[241] = new GameObject("Food241");
            char[,] food241Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food241Transform = new TransformComponent(66, 29);
            MapTransformComponent food241MapTransform =
                new MapTransformComponent(22, 29);
            map.Map[
                food241MapTransform.Position.X, food241MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food241Collider = new ColliderComponent(Cell.Food);
            allFoods[241].AddComponent(food241Transform);
            allFoods[241].AddComponent(food241MapTransform);
            allFoods[241].AddComponent(food241Collider);
            allFoods[241].AddComponent(new ConsoleSprite(
                food241Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[242] = new GameObject("Food242");
            char[,] food242Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food242Transform = new TransformComponent(69, 29);
            MapTransformComponent food242MapTransform =
                new MapTransformComponent(23, 29);
            map.Map[
                food242MapTransform.Position.X, food242MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food242Collider = new ColliderComponent(Cell.Food);
            allFoods[242].AddComponent(food242Transform);
            allFoods[242].AddComponent(food242MapTransform);
            allFoods[242].AddComponent(food242Collider);
            allFoods[242].AddComponent(new ConsoleSprite(
                food242Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[243] = new GameObject("Food243");
            char[,] food243Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food243Transform = new TransformComponent(72, 29);
            MapTransformComponent food243MapTransform =
                new MapTransformComponent(24, 29);
            map.Map[
                food243MapTransform.Position.X, food243MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food243Collider = new ColliderComponent(Cell.Food);
            allFoods[243].AddComponent(food243Transform);
            allFoods[243].AddComponent(food243MapTransform);
            allFoods[243].AddComponent(food243Collider);
            allFoods[243].AddComponent(new ConsoleSprite(
                food243Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[244] = new GameObject("Food244");
            char[,] food244Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food244Transform = new TransformComponent(75, 29);
            MapTransformComponent food244MapTransform =
                new MapTransformComponent(25, 29);
            map.Map[
                food244MapTransform.Position.X, food244MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food244Collider = new ColliderComponent(Cell.Food);
            allFoods[244].AddComponent(food244Transform);
            allFoods[244].AddComponent(food244MapTransform);
            allFoods[244].AddComponent(food244Collider);
            allFoods[244].AddComponent(new ConsoleSprite(
                food244Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[245] = new GameObject("Food245");
            char[,] food245Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food245Transform = new TransformComponent(78, 29);
            MapTransformComponent food245MapTransform =
                new MapTransformComponent(26, 29);
            map.Map[
                food245MapTransform.Position.X, food245MapTransform.Position.Y].
                Collider.Type |= Cell.Food;
            ColliderComponent food245Collider = new ColliderComponent(Cell.Food);
            allFoods[245].AddComponent(food245Transform);
            allFoods[245].AddComponent(food245MapTransform);
            allFoods[245].AddComponent(food245Collider);
            allFoods[245].AddComponent(new ConsoleSprite(
                food245Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));
        }

        /// <summary>
        /// Creates fruit spawner and adds fruit spawner component.
        /// </summary>
        private void FruitSpawnerCreation()
        {
            fruitSpawner = new GameObject("Fruit Spwaner");

            fruitSpawnerComponent = new FruitSpawnerComponent(15000);

            fruitSpawner.AddComponent(fruitSpawnerComponent);

            // After fruitSpawnerComponent calls Start() method, this
            // class subscribes to FruitTimer event
            fruitSpawnerComponent.RegisterToTimerEvent +=
                RegisterToFruitSpawnerTimedEvent;
        }

        /// <summary>
        /// Method used to register fruit spawner event for fruitCreation.
        /// </summary>
        private void RegisterToFruitSpawnerTimedEvent() =>
            fruitSpawnerComponent.FruitTimer.Elapsed += FruitCreation;

        /// <summary>
        /// Creates a fruit in a random position.
        /// This method is called with a timed event created on
        /// FruitSpawnerComponent script.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="e">Elapsed event arguments.</param>
        private void FruitCreation(object source, ElapsedEventArgs e)
        {
            ushort foodsEaten =
                gameOverCheck.GetComponent<GameOverCheckComponent>().FoodsEaten;

            // Only happens if the player ate more foods than fruits on the map
            if (foodsEaten > allFruits.Length)
            {
                // Creates a random position
                int randX = random.Next(1, XSIZE - 1);
                int randY = random.Next(1, YSIZE - 1);

                // If the position is free to spawn a fruit
                if (!map.Map[randX, randY].Collider.Type.HasFlag(Cell.Ghost) &&
                    !map.Map[randX, randY].Collider.Type.HasFlag(Cell.Pacman) &&
                    !map.Map[randX, randY].Collider.Type.HasFlag(Cell.Fruit) &&
                    !map.Map[randX, randY].Collider.Type.HasFlag(Cell.Food) &&
                    !map.Map[randX, randY].Collider.Type.
                        HasFlag(Cell.PowerPill) &&
                    !map.Map[randX, randY].Collider.Type.HasFlag(Cell.Wall) &&
                    !map.Map[randX, randY].Collider.Type.
                        HasFlag(Cell.GhostHouse))
                {
                    allFruits[fruitSlot] = new GameObject($"Fruit{fruitName}");
                    char[,] fruitSprite = { { ' ' }, { 'F' }, { ' ' }, };
                    TransformComponent fruitTransform =
                        new TransformComponent(randX * 3, randY);
                    MapTransformComponent fruitMapTransform =
                        new MapTransformComponent(randX, randY);
                    map.Map[
                        fruitMapTransform.Position.X,
                        fruitMapTransform.Position.Y].
                        Collider.Type |= Cell.Fruit;

                    ColliderComponent fruitCollider =
                        new ColliderComponent(Cell.Fruit);
                    allFruits[fruitSlot].AddComponent(fruitTransform);
                    allFruits[fruitSlot].AddComponent(fruitMapTransform);
                    allFruits[fruitSlot].AddComponent(fruitCollider);
                    allFruits[fruitSlot].AddComponent(
                        new ConsoleSprite(
                            fruitSprite,
                            ConsoleColor.White,
                            ConsoleColor.DarkBlue));

                    // Adds fruit
                    collisions.AddGameObject(allFruits[fruitSlot]);
                    LevelScene.AddGameObject(allFruits[fruitSlot]);
                    consoleRenderer.AddGameObject(allFruits[fruitSlot]);

                    fruitName++;
                    fruitSlot++;

                    allFruits = new GameObject[fruitSlot + 1];
                }
                else
                {
                    // Finds a new position
                    FruitCreation(source, e);
                }
            }
        }

        /// <summary>
        /// Creates power pills.
        /// </summary>
        private void PowerPillsCreation()
        {
            allPowerPills[0] = new GameObject("PowerPill1");
            char[,] powerPill0Sprite = { { 'P' }, { 'U' }, { 'P' }, };
            TransformComponent powerPill0Transform =
                new TransformComponent(3, 3);
            MapTransformComponent powerPill0MapTransform =
                new MapTransformComponent(1, 3);
            map.Map[
                powerPill0MapTransform.Position.X,
                powerPill0MapTransform.Position.Y].
                Collider.Type = Cell.PowerPill;

            ColliderComponent powerPill0Collider =
                new ColliderComponent(Cell.PowerPill);

            allPowerPills[0].AddComponent(powerPill0MapTransform);
            allPowerPills[0].AddComponent(powerPill0Transform);
            allPowerPills[0].AddComponent(powerPill0Collider);
            allPowerPills[0].AddComponent(new ConsoleSprite(
                powerPill0Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            /////////////////
            allPowerPills[1] = new GameObject("PowerPill2");
            char[,] powerPill1Sprite = { { 'P' }, { 'U' }, { 'P' }, };
            TransformComponent powerPill1Transform =
                new TransformComponent(78, 3);
            MapTransformComponent powerPill1MapTransform =
                new MapTransformComponent(26, 3);
            map.Map[
                powerPill1MapTransform.Position.X,
                powerPill1MapTransform.Position.Y].
                Collider.Type = Cell.PowerPill;

            ColliderComponent powerPill1Collider =
                new ColliderComponent(Cell.PowerPill);

            allPowerPills[1].AddComponent(powerPill1Transform);
            allPowerPills[1].AddComponent(powerPill1MapTransform);
            allPowerPills[1].AddComponent(powerPill1Collider);
            allPowerPills[1].AddComponent(new ConsoleSprite(
                powerPill1Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            /////////////////
            allPowerPills[2] = new GameObject("PowerPill3");
            char[,] powerPill2Sprite = { { 'P' }, { 'U' }, { 'P' }, };
            TransformComponent powerPill2Transform =
                new TransformComponent(3, 23);

            MapTransformComponent powerPill2MapTransform =
                new MapTransformComponent(1, 23);
            map.Map[
                powerPill2MapTransform.Position.X,
                powerPill2MapTransform.Position.Y].
                Collider.Type = Cell.PowerPill;

            ColliderComponent powerPill2Collider =
                new ColliderComponent(Cell.PowerPill);

            allPowerPills[2].AddComponent(powerPill2Transform);
            allPowerPills[2].AddComponent(powerPill2MapTransform);
            allPowerPills[2].AddComponent(powerPill2Collider);
            allPowerPills[2].AddComponent(new ConsoleSprite(
                powerPill2Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            /////////////////
            allPowerPills[3] = new GameObject("PowerPill4");
            char[,] powerPill3Sprite = { { 'P' }, { 'U' }, { 'P' }, };
            TransformComponent powerPill3Transform =
                new TransformComponent(78, 23);

            MapTransformComponent powerPill3MapTransform =
                new MapTransformComponent(26, 23);
            map.Map[
                powerPill3MapTransform.Position.X,
                powerPill3MapTransform.Position.Y].
                Collider.Type = Cell.PowerPill;

            ColliderComponent powerPill3Collider =
                new ColliderComponent(Cell.PowerPill);

            allPowerPills[3].AddComponent(powerPill3Transform);
            allPowerPills[3].AddComponent(powerPill3MapTransform);
            allPowerPills[3].AddComponent(powerPill3Collider);
            allPowerPills[3].AddComponent(new ConsoleSprite(
                powerPill3Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));
        }

        /// <summary>
        /// Creates walls.
        /// </summary>
        /// <param name="map">Map reference to the game map.</param>
        private void WallCreation(MapComponent map)
        {
            walls = new GameObject("Walls");

            ConsolePixel wallPixel = new ConsolePixel(
                ' ', ConsoleColor.White, ConsoleColor.DarkGreen);

            Dictionary<Vector2Int, ConsolePixel> wallPixels =
                new Dictionary<Vector2Int, ConsolePixel>();

            int iTest = 0;
            int jTest = 1;
            int kTest = 2;
            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    if (map.Map[x, y].Collider.Type == Cell.Wall)
                    {
                        wallPixels[new Vector2Int(iTest, y)] = wallPixel;
                        wallPixels[new Vector2Int(jTest, y)] = wallPixel;
                        wallPixels[new Vector2Int(kTest, y)] = wallPixel;
                    }
                }

                iTest += 3;
                jTest += 3;
                kTest += 3;
            }

            TransformComponent wallTransform = new TransformComponent(0, 0);
            walls.AddComponent(wallTransform);
            walls.AddComponent(new ConsoleSprite(wallPixels));
        }

        /// <summary>
        /// Creates score related variables.
        /// </summary>
        private void UICreation()
        {
            scoreText = new GameObject("Score Text");

            scoreText.AddComponent(new TransformComponent(0, YSIZE));

            RenderableStringComponent renderScoreText
                = new RenderableStringComponent(
                    () => $"Score: {score.GetScore}",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            scoreText.AddComponent(renderScoreText);

            ////////////////////////////////////////////////////////////////////

            highScoreText = new GameObject("HighScore Text");

            HighScoreComponent highScoreComponent =
                new HighScoreComponent();

            RenderableStringComponent renderHighScoreText
                = new RenderableStringComponent(
                    () => $"HighScore: {highScoreComponent.HighScore}",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            highScoreText.AddComponent(new TransformComponent(0, YSIZE + 1));
            highScoreText.AddComponent(renderHighScoreText);
            highScoreText.AddComponent(highScoreComponent);

            ////////////////////////////////////////////////////////////////////

            livesText = new GameObject("Lives Text");

            livesText.AddComponent(new TransformComponent(0, YSIZE + 2));

            RenderableStringComponent renderLivesText
                = new RenderableStringComponent(
                    () => $"Lives: {lives.Lives}",
                    i => new Vector2Int(i, 0),
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);

            livesText.AddComponent(renderLivesText);
        }

        /// <summary>
        /// Adds gameobjects to collision check.
        /// </summary>
        private void AddGameObjectsToCollisionCheck()
        {
            collisions.AddGameObject(pinky);
            collisions.AddGameObject(blinky);
            collisions.AddGameObject(inky);
            collisions.AddGameObject(clyde);

            foreach (GameObject food in allFoods)
            {
                if (food != null)
                {
                    collisions.AddGameObject(food);
                }
            }

            foreach (GameObject powerPill in allPowerPills)
            {
                if (powerPill != null)
                {
                    collisions.AddGameObject(powerPill);
                }
            }
        }

        /// <summary>
        /// Adds gameobjects to LevelScene.
        /// </summary>
        private void AddGameObjectsToScene()
        {
            LevelScene.AddGameObject(spawner);
            LevelScene.AddGameObject(pacman);
            LevelScene.AddGameObject(collisions);

            foreach (GameObject food in allFoods)
            {
                if (food != null)
                {
                    LevelScene.AddGameObject(food);
                }
            }

            foreach (GameObject powerPill in allPowerPills)
            {
                if (powerPill != null)
                {
                    LevelScene.AddGameObject(powerPill);
                }
            }

            LevelScene.AddGameObject(pinky);
            LevelScene.AddGameObject(blinky);
            LevelScene.AddGameObject(inky);
            LevelScene.AddGameObject(clyde);

            LevelScene.AddGameObject(walls);
            LevelScene.AddGameObject(score);
            LevelScene.AddGameObject(scoreText);
            LevelScene.AddGameObject(highScoreText);
            LevelScene.AddGameObject(gameState);
            LevelScene.AddGameObject(fruitSpawner);
            LevelScene.AddGameObject(gameOverCheck);
        }

        /// <summary>
        /// Adds gameobjects to render.
        /// </summary>
        private void AddGameObjectsToRender()
        {
            foreach (GameObject food in allFoods)
            {
                if (food != null)
                {
                    consoleRenderer.AddGameObject(food);
                }
            }

            foreach (GameObject powerPill in allPowerPills)
            {
                if (powerPill != null)
                {
                    consoleRenderer.AddGameObject(powerPill);
                }
            }

            consoleRenderer.AddGameObject(pacman);
            consoleRenderer.AddGameObject(pinky);
            consoleRenderer.AddGameObject(blinky);
            consoleRenderer.AddGameObject(inky);
            consoleRenderer.AddGameObject(clyde);
            consoleRenderer.AddGameObject(walls);
            consoleRenderer.AddGameObject(scoreText);
            consoleRenderer.AddGameObject(highScoreText);
            consoleRenderer.AddGameObject(livesText);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LevelCreation"/> class.
        /// </summary>
        ~LevelCreation()
        {
            gameOverCheckComponent.NoFoodsLeft -= GameOver;

            fruitSpawnerComponent.RegisterToTimerEvent -=
                RegisterToFruitSpawnerTimedEvent;

            fruitSpawnerComponent.FruitTimer.Elapsed -= FruitCreation;

            gameState.GhostChaseCollision -= ResetPositions;
        }
    }
}
