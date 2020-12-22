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
            
            char[,] playerSprite =
            {
                {'P'}
            };
            GameObject player = new GameObject("Player");
            // Player components ///////////////////////////////////
            KeyReaderComponent pacmanKeyReader = new KeyReaderComponent();
            TransformComponent pacmanTransform = new TransformComponent(Cell.Pacman);
            MoveComponent pacmanMovement = new MoveComponent(2, 2, new PacmanMovementBehaviour());
            MapComponent map = new MapComponent(20, 20);

            player.AddComponent(pacmanKeyReader);
            player.AddComponent(pacmanTransform);
            player.AddComponent(pacmanMovement);
            player.AddComponent(map);



            player.AddComponent(new ConsoleSprite(playerSprite,
                                                  ConsoleColor.DarkRed,
                                                  ConsoleColor.DarkYellow));
            

            // /////////////////////////////////////////////////////
           
            // Add GameObjects to the scene
            scene.AddGameObject(player);
            // ////////////////////////////

            // Add GameObjects to the renderer
            consoleRenderer.AddGameObject(player);
            // ////////////////////////////
            
            // Add renderer to the scene
            scene.AddGameObject(consoleRenderer);
            scene.GameLoop(50);

        }
    }
}
