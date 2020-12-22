using System;

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
                                                            20, 
                                                            20, 
                                                            backgroundPixel, 
                                                            "Console Renderer");
            Scene scene = new Scene(20, 20);

            Collision collisions = new Collision();

            MapComponent map = new MapComponent(20, 20, collisions);

           
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
            TransformComponent fruitTransform = new TransformComponent(new ColliderComponent(Cell.Fruit), 0, 0);

            fruit.AddComponent(fruitTransform);

            fruit.AddComponent(new ConsoleSprite(fruitSprite,
                                                  ConsoleColor.DarkYellow,
                                                  ConsoleColor.DarkYellow));

            // /////////////////////////////////////////////////////

            // Add Gameobjects to collision check
            collisions.AddPacman(pacman);
            collisions.AddGameObject(pinky);
            collisions.AddGameObject(fruit);


            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            scene.AddGameObject(pinky);
            scene.AddGameObject(fruit);
            scene.AddGameObject(collisions);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(pacman);
            consoleRenderer.AddGameObject(pinky);
            consoleRenderer.AddGameObject(fruit);
            // ////////////////////////////

            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);

        }
    }
}
