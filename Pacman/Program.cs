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
                                                            56,
                                                            31,
                                                            backgroundPixel,
                                                            "Console Renderer");
            Scene scene = new Scene(28, 31);

            Collision collisions = new Collision(scene, consoleRenderer);

            MapComponent map = new MapComponent(28, 31);


            // PACMAN
            char[,] pacmanSprite =
            {
                {'C'},
                {'C'},
            };

            GameObject pacman = new GameObject("Pacman");
            // Components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(10, 5);
            MoveComponent pacmanMovement = new MoveComponent();
            ColliderComponent pacmanCollider = new ColliderComponent(Cell.Pacman);

            pacman.AddComponent(pacmanKeyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMovement);
            pacman.AddComponent(pacmanCollider);
            pacman.AddComponent(map);

            // Adds a movement behaviour
            pacmanMovement.AddMovementBehaviour(new PacmanMovementBehaviour(pacman));



            pacman.AddComponent(new ConsoleSprite(pacmanSprite,
                                                  ConsoleColor.White,
                                                  ConsoleColor.Red));

          
            // GHOST
            // char[,] pinkySprite =
            // {
            //     {'P'}
            // };
            // GameObject pinky = new GameObject("Pinky");
            // TransformComponent pinkyTransform = new TransformComponent(10, 10);
            // MoveComponent pinkyMovement = new MoveComponent();
            // ColliderComponent pinkyCollider = new ColliderComponent(Cell.Ghost);

            // pinky.AddComponent(pinkyTransform);
            // pinky.AddComponent(pinkyMovement);
            // pinky.AddComponent(pinkyCollider);
            // pinky.AddComponent(map);

            // // Adds a movement behaviour
            // pinkyMovement.AddMovementBehaviour(new PinkyMovementBehaviour(pinky));

            // pinky.AddComponent(new ConsoleSprite(pinkySprite,
            //                                       ConsoleColor.Magenta,
            //                                       ConsoleColor.DarkBlue));


            // Fruit
            char[,] fruitSprite =
            {
                {'F'},
                {'F'},
            };
            GameObject fruit = new GameObject("Strawberry");
            TransformComponent fruitTransform = new TransformComponent(5, 1);
            ColliderComponent fruitCollider = new ColliderComponent(Cell.Fruit);

            fruit.AddComponent(fruitTransform);
            fruit.AddComponent(fruitCollider);

            fruit.AddComponent(new ConsoleSprite(fruitSprite,
                                                  ConsoleColor.DarkYellow,
                                                  ConsoleColor.DarkYellow));
            

            // Walls
            GameObject walls = new GameObject("Walls");

            ConsolePixel wallPixel = new ConsolePixel(
                ' ', ConsoleColor.White, ConsoleColor.DarkGreen);

            Dictionary<Vector2Int, ConsolePixel> wallPixels =
                new Dictionary<Vector2Int, ConsolePixel>();

            // for (int x = 0; x < map.Map.GetLength(0); x++)
            // {
            //     for (int y = 0; y < map.Map.GetLength(1); y++)
            //     {
            //         if (map.Map[x, y].Collider.Type == Cell.Wall)
            //         {
            //             wallPixels[new Vector2Int(x, y)] = wallPixel;
            //         }
            //     }
            // }

            // UNCOMMENT THIS AND COMMENT THE PREVIOUS LOOP TO SEE SOMETHING COOL

            int iTest = 0;
            int jTest = 1;
            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    if (map.Map[x, y].Collider.Type == Cell.Wall)
                    {
                        wallPixels[new Vector2Int(iTest, y)] = wallPixel;
                        wallPixels[new Vector2Int(jTest, y)] = wallPixel;
                        // wallPixels[new Vector2Int(x, y)] = wallPixel;
                    }
                }
                iTest += 2;
                jTest += 2;
            }

            TransformComponent wallTransform = new TransformComponent(0, 0);
            walls.AddComponent(wallTransform);
            walls.AddComponent(new ConsoleSprite(wallPixels));

            // /////////////////////////////////////////////////////

            // Add Gameobjects to collision check
            collisions.AddPacman(pacman);
            //collisions.AddGameObject(pinky);
            collisions.AddGameObject(fruit);

            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            //scene.AddGameObject(pinky);
            scene.AddGameObject(fruit);
            scene.AddGameObject(collisions);
            scene.AddGameObject(walls);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(walls);
            consoleRenderer.AddGameObject(pacman);
            //consoleRenderer.AddGameObject(pinky);
            consoleRenderer.AddGameObject(fruit);
            // ////////////////////////////


            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);
        }
    }
}
