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
            
            char[,] pacmanSprite =
            {
                {'P'}
            };
            GameObject pacman = new GameObject("Player");
            // Player components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(Cell.Pacman);
            MoveComponent pacmanMovement = new MoveComponent(2, 2);
            MapComponent map = new MapComponent(20, 20);

            pacman.AddComponent(pacmanKeyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMovement);
            pacman.AddComponent(map);

            IMovementBehaviour pacmanMB = new PacmanMovementBehaviour(pacman);
            pacmanMovement.AddMovementBehaviour(pacmanMB);



            pacman.AddComponent(new ConsoleSprite(pacmanSprite,
                                                  ConsoleColor.DarkRed,
                                                  ConsoleColor.DarkYellow));
            

            // /////////////////////////////////////////////////////
           
            // Add GameObjects to the scene
            scene.AddGameObject(pacman);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(pacman);
            // ////////////////////////////
            
            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);

        }
    }
}
