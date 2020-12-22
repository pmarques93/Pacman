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
            TransformComponent transform = new TransformComponent(Cell.Pacman);
            MoveComponent move = new MoveComponent(2, 2);
            KeyReaderComponent keyReader = new KeyReaderComponent();
            MapComponent map = new MapComponent(20, 20);

            player.AddComponent(transform);
            player.AddComponent(move);
            player.AddComponent(keyReader);
            player.AddComponent(new ConsoleSprite(playerSprite,
                                                  ConsoleColor.DarkRed,
                                                  ConsoleColor.DarkYellow));
            player.AddComponent(map);
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
