using System;
using System.Collections.Generic;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsolePixel backgroundPixel = new ConsolePixel(
                                                        ' ',
                                                        ConsoleColor.White,
                                                        ConsoleColor.DarkBlue);

            ConsoleRenderer consoleRenderer = new ConsoleRenderer(
                                                            84,
                                                            31,
                                                            backgroundPixel,
                                                            "Console Renderer");
            Scene scene = new Scene(28, 31);

            Collision collisions = new Collision(scene, consoleRenderer);

            MapComponent map = new MapComponent(28, 31);

            ConsoleScore score = new ConsoleScore(collisions);


            // PACMAN
            char[,] pacmanSprite =
            {
                {' '},
                {'C'},
                {' '},
            };

            GameObject pacman = new GameObject("Pacman");
            // Components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(3, 5);
            MoveComponent pacmanMovement = new MoveComponent();
            ColliderComponent pacmanCollider = new ColliderComponent(Cell.Pacman);

            pacman.AddComponent(pacmanKeyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMovement);
            pacman.AddComponent(pacmanCollider);
            pacman.AddComponent(map);

            // Adds a movement behaviour
            pacmanMovement.AddMovementBehaviour(
                                    new PacmanMovementBehaviour(
                                    pacman, new TransformComponent(1,5), 3));



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


            // Food
            GameObject[] allFoods = new GameObject[50];

            #region every food
            allFoods[0] = new GameObject("Food0");
            char[,] food0Sprite ={{' '},{'.'},{' '},};
            TransformComponent food0Transform = new TransformComponent(3, 1);
            ColliderComponent food0Collider = new ColliderComponent(Cell.Food);
            allFoods[0].AddComponent(food0Transform);
            allFoods[0].AddComponent(food0Collider);
            allFoods[0].AddComponent(new ConsoleSprite(
                food0Sprite,ConsoleColor.White,ConsoleColor.DarkBlue));

            allFoods[1] = new GameObject("Food1");
            char[,] food1Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food1Transform = new TransformComponent(6, 1);
            ColliderComponent food1Collider = new ColliderComponent(Cell.Food);
            allFoods[1].AddComponent(food1Transform);
            allFoods[1].AddComponent(food1Collider);
            allFoods[1].AddComponent(new ConsoleSprite(
                food1Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[2] = new GameObject("Food2");
            char[,] food2Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food2Transform = new TransformComponent(9, 1);
            ColliderComponent food2Collider = new ColliderComponent(Cell.Food);
            allFoods[2].AddComponent(food2Transform);
            allFoods[2].AddComponent(food2Collider);
            allFoods[2].AddComponent(new ConsoleSprite(
                food2Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[3] = new GameObject("Food3");
            char[,] food3Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food3Transform = new TransformComponent(12, 1);
            ColliderComponent food3Collider = new ColliderComponent(Cell.Food);
            allFoods[3].AddComponent(food3Transform);
            allFoods[3].AddComponent(food3Collider);
            allFoods[3].AddComponent(new ConsoleSprite(
                food3Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[4] = new GameObject("Food4");
            char[,] food4Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food4Transform = new TransformComponent(15, 1);
            ColliderComponent food4Collider = new ColliderComponent(Cell.Food);
            allFoods[4].AddComponent(food4Transform);
            allFoods[4].AddComponent(food4Collider);
            allFoods[4].AddComponent(new ConsoleSprite(
                food4Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[5] = new GameObject("Food5");
            char[,] food5Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food5Transform = new TransformComponent(18, 1);
            ColliderComponent food5Collider = new ColliderComponent(Cell.Food);
            allFoods[5].AddComponent(food5Transform);
            allFoods[5].AddComponent(food5Collider);
            allFoods[5].AddComponent(new ConsoleSprite(
                food5Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[6] = new GameObject("Food6");
            char[,] food6Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food6Transform = new TransformComponent(21, 1);
            ColliderComponent food6Collider = new ColliderComponent(Cell.Food);
            allFoods[6].AddComponent(food6Transform);
            allFoods[6].AddComponent(food6Collider);
            allFoods[6].AddComponent(new ConsoleSprite(
                food6Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[7] = new GameObject("Food7");
            char[,] food7Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food7Transform = new TransformComponent(24, 1);
            ColliderComponent food7Collider = new ColliderComponent(Cell.Food);
            allFoods[7].AddComponent(food7Transform);
            allFoods[7].AddComponent(food7Collider);
            allFoods[7].AddComponent(new ConsoleSprite(
                food7Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[8] = new GameObject("Food8");
            char[,] food8Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food8Transform = new TransformComponent(27, 1);
            ColliderComponent food8Collider = new ColliderComponent(Cell.Food);
            allFoods[8].AddComponent(food8Transform);
            allFoods[8].AddComponent(food8Collider);
            allFoods[8].AddComponent(new ConsoleSprite(
                food8Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[9] = new GameObject("Food9");
            char[,] food9Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food9Transform = new TransformComponent(30, 1);
            ColliderComponent food9Collider = new ColliderComponent(Cell.Food);
            allFoods[9].AddComponent(food9Transform);
            allFoods[9].AddComponent(food9Collider);
            allFoods[9].AddComponent(new ConsoleSprite(
                food2Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[10] = new GameObject("Food10");
            char[,] food10Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food10Transform = new TransformComponent(33, 1);
            ColliderComponent food10Collider = new ColliderComponent(Cell.Food);
            allFoods[10].AddComponent(food10Transform);
            allFoods[10].AddComponent(food10Collider);
            allFoods[10].AddComponent(new ConsoleSprite(
                food10Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[11] = new GameObject("Food11");
            char[,] food11Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food11Transform = new TransformComponent(36, 1);
            ColliderComponent food11Collider = new ColliderComponent(Cell.Food);
            allFoods[11].AddComponent(food11Transform);
            allFoods[11].AddComponent(food11Collider);
            allFoods[11].AddComponent(new ConsoleSprite(
                food11Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[12] = new GameObject("Food12");
            char[,] food12Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food12Transform = new TransformComponent(45, 1);
            ColliderComponent food12Collider = new ColliderComponent(Cell.Food);
            allFoods[12].AddComponent(food12Transform);
            allFoods[12].AddComponent(food12Collider);
            allFoods[12].AddComponent(new ConsoleSprite(
                food12Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[13] = new GameObject("Food13");
            char[,] food13Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food13Transform = new TransformComponent(48, 1);
            ColliderComponent food13Collider = new ColliderComponent(Cell.Food);
            allFoods[13].AddComponent(food13Transform);
            allFoods[13].AddComponent(food13Collider);
            allFoods[13].AddComponent(new ConsoleSprite(
                food13Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[14] = new GameObject("Food14");
            char[,] food14Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food14Transform = new TransformComponent(51, 1);
            ColliderComponent food14Collider = new ColliderComponent(Cell.Food);
            allFoods[14].AddComponent(food14Transform);
            allFoods[14].AddComponent(food14Collider);
            allFoods[14].AddComponent(new ConsoleSprite(
                food14Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[15] = new GameObject("Food15");
            char[,] food15Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food15Transform = new TransformComponent(54, 1);
            ColliderComponent food15Collider = new ColliderComponent(Cell.Food);
            allFoods[15].AddComponent(food15Transform);
            allFoods[15].AddComponent(food15Collider);
            allFoods[15].AddComponent(new ConsoleSprite(
                food15Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[16] = new GameObject("Food16");
            char[,] food16Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food16Transform = new TransformComponent(57, 1);
            ColliderComponent food16Collider = new ColliderComponent(Cell.Food);
            allFoods[16].AddComponent(food16Transform);
            allFoods[16].AddComponent(food16Collider);
            allFoods[16].AddComponent(new ConsoleSprite(
                food16Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[17] = new GameObject("Food17");
            char[,] food17Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food17Transform = new TransformComponent(60, 1);
            ColliderComponent food17Collider = new ColliderComponent(Cell.Food);
            allFoods[17].AddComponent(food17Transform);
            allFoods[17].AddComponent(food17Collider);
            allFoods[17].AddComponent(new ConsoleSprite(
                food17Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[18] = new GameObject("Food18");
            char[,] food18Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food18Transform = new TransformComponent(63, 1);
            ColliderComponent food18Collider = new ColliderComponent(Cell.Food);
            allFoods[18].AddComponent(food18Transform);
            allFoods[18].AddComponent(food18Collider);
            allFoods[18].AddComponent(new ConsoleSprite(
                food18Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[19] = new GameObject("Food19");
            char[,] food19Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food19Transform = new TransformComponent(66, 1);
            ColliderComponent food19Collider = new ColliderComponent(Cell.Food);
            allFoods[19].AddComponent(food19Transform);
            allFoods[19].AddComponent(food19Collider);
            allFoods[19].AddComponent(new ConsoleSprite(
                food19Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[20] = new GameObject("Food20");
            char[,] food20Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food20Transform = new TransformComponent(69, 1);
            ColliderComponent food20Collider = new ColliderComponent(Cell.Food);
            allFoods[20].AddComponent(food20Transform);
            allFoods[20].AddComponent(food20Collider);
            allFoods[20].AddComponent(new ConsoleSprite(
                food20Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[21] = new GameObject("Food21");
            char[,] food21Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food21Transform = new TransformComponent(72, 1);
            ColliderComponent food21Collider = new ColliderComponent(Cell.Food);
            allFoods[21].AddComponent(food21Transform);
            allFoods[21].AddComponent(food21Collider);
            allFoods[21].AddComponent(new ConsoleSprite(
                food21Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[22] = new GameObject("Food22");
            char[,] food22Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food22Transform = new TransformComponent(75, 1);
            ColliderComponent food22Collider = new ColliderComponent(Cell.Food);
            allFoods[22].AddComponent(food22Transform);
            allFoods[22].AddComponent(food22Collider);
            allFoods[22].AddComponent(new ConsoleSprite(
                food22Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));

            allFoods[23] = new GameObject("Food23");
            char[,] food23Sprite = { { ' ' }, { '.' }, { ' ' }, };
            TransformComponent food23Transform = new TransformComponent(78, 1);
            ColliderComponent food23Collider = new ColliderComponent(Cell.Food);
            allFoods[23].AddComponent(food23Transform);
            allFoods[23].AddComponent(food23Collider);
            allFoods[23].AddComponent(new ConsoleSprite(
                food23Sprite, ConsoleColor.White, ConsoleColor.DarkBlue));
            #endregion

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

            // /////////////////////////////////////////////////////

            // Add Gameobjects to collision check
            collisions.AddPacman(pacman);
            //collisions.AddGameObject(pinky);
            foreach(GameObject food in allFoods)
                if (food != null) collisions.AddGameObject(food);

            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            //scene.AddGameObject(pinky);

            foreach (GameObject food in allFoods)
                if (food != null) scene.AddGameObject(food);

   

            scene.AddGameObject(walls);
            scene.AddGameObject(collisions);
            scene.AddGameObject(score);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(pacman);
            //consoleRenderer.AddGameObject(pinky);

            foreach (GameObject food in allFoods)
                if (food != null) consoleRenderer.AddGameObject(food);

            consoleRenderer.AddGameObject(walls);
            // ////////////////////////////

            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);
        }
    }
}
