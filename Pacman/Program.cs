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

            MapComponent map = new MapComponent(20, 20);

            // PACMAN
            char[,] pacmanSprite =
            {
                {'C'}
            };

            GameObject pacman = new GameObject("Pacman");
            // Components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(Cell.Pacman, 5, 5);
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
            TransformComponent pinkyTransform = new TransformComponent(Cell.Ghost, 10, 10);
            MoveComponent pinkyMovement = new MoveComponent();

            pinky.AddComponent(pinkyTransform);
            pinky.AddComponent(pinkyMovement);
            pinky.AddComponent(map);

            // Adds a movement behaviour
            pinkyMovement.AddMovementBehaviour(new PinkyMovementBehaviour(pinky));

            pinky.AddComponent(new ConsoleSprite(pinkySprite,
                                                  ConsoleColor.Magenta,
                                                  ConsoleColor.DarkBlue));


            // /////////////////////////////////////////////////////

            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            scene.AddGameObject(pinky);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(pacman);
            consoleRenderer.AddGameObject(pinky);
            // ////////////////////////////

            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(100);

        }
    }
}
