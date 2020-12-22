using System;
using System.Collections.Generic;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsolePixel backgroundPixel = new ConsolePixel(
                                                        '.',
                                                        ConsoleColor.White,
                                                        ConsoleColor.DarkBlue);

            ConsoleRenderer consoleRenderer = new ConsoleRenderer(
                                                            28,
                                                            31,
                                                            backgroundPixel,
                                                            "Console Renderer");
            Scene scene = new Scene(28, 31);

            Collision collisions = new Collision(scene, consoleRenderer);

            MapComponent map = new MapComponent(28, 31);


            // PACMAN
            char[,] pacmanSprite =
            {
                {'C'}
            };

            GameObject pacman = new GameObject("Pacman");
            // Components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(new ColliderComponent(Cell.Pacman), 5, 5);
            MoveComponent pacmanMovement = new MoveComponent();


            pacman.AddComponent(pacmanKeyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMovement);
            pacman.AddComponent(map);

            // Adds a movement behaviour
            pacmanMovement.AddMovementBehaviour(new PacmanMovementBehaviour(pacman));



            pacman.AddComponent(new ConsoleSprite(pacmanSprite,
                                                  ConsoleColor.Yellow,
                                                  ConsoleColor.DarkBlue));

            /*
            // GHOST
            char[,] pinkySprite =
            {
                {'P'}
            };
            GameObject pinky = new GameObject("Pinky");
            TransformComponent pinkyTransform = new TransformComponent(new ColliderComponent(Cell.Ghost), 10, 10);
            MoveComponent pinkyMovement = new MoveComponent();

            pinky.AddComponent(pinkyTransform);
            pinky.AddComponent(pinkyMovement);
            pinky.AddComponent(map);

            // Adds a movement behaviour
            pinkyMovement.AddMovementBehaviour(new PinkyMovementBehaviour(pinky));

            pinky.AddComponent(new ConsoleSprite(pinkySprite,
                                                  ConsoleColor.Magenta,
                                                  ConsoleColor.DarkBlue));


            // Fruit
            char[,] fruitSprite =
            {
                {'F'}
            };
            GameObject fruit = new GameObject("Strawberry");
            TransformComponent fruitTransform = new TransformComponent(new ColliderComponent(Cell.Fruit), 2, 2);

            fruit.AddComponent(fruitTransform);

            fruit.AddComponent(new ConsoleSprite(fruitSprite,
                                                  ConsoleColor.DarkYellow,
                                                  ConsoleColor.DarkYellow));
            */

            // Walls
            GameObject walls = new GameObject("Walls");

            ConsolePixel wallPixel = new ConsolePixel(
                ' ', ConsoleColor.White, ConsoleColor.DarkGreen);

            Dictionary<Vector2Int, ConsolePixel> wallPixels =
                new Dictionary<Vector2Int, ConsolePixel>();

            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    if (map.Map[x, y].Collider.Type == Cell.Wall)
                        wallPixels[new Vector2Int(x, y)] = wallPixel;
                }
            }
            TransformComponent wallTransform = new TransformComponent(
                                        new ColliderComponent(Cell.Wall), 0, 0);
            walls.AddComponent(wallTransform);
            walls.AddComponent(new ConsoleSprite(wallPixels));

            // /////////////////////////////////////////////////////

            // Add Gameobjects to collision check
            collisions.AddPacman(pacman);
            //collisions.AddGameObject(pinky);
            //collisions.AddGameObject(fruit);

            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            //scene.AddGameObject(pinky);
            //scene.AddGameObject(fruit);
            scene.AddGameObject(collisions);
            scene.AddGameObject(walls);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(pacman);
            //consoleRenderer.AddGameObject(pinky);
            //consoleRenderer.AddGameObject(fruit);
            consoleRenderer.AddGameObject(walls);
            // ////////////////////////////


            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);
        }
    }
}
