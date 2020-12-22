using System;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            // DB test
            // DoubleBuffer<char> df = new DoubleBuffer<char>(10, 10);
            // df[3, 3] = '<';
            // df.Swap();
            // Console.WriteLine(df[3, 3]);

            // Pacman test
            // Scene scene = new Scene(10, 10);
            // scene.SetupScene();

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
            TransformComponent transform = new TransformComponent();
            transform.Position = new Vector2Int(2, 2);
            MoveComponent move = new MoveComponent(2, 2, 19, 19);
            KeyReaderComponent keyReader = new KeyReaderComponent();

            player.AddComponent(transform);
            player.AddComponent(move);
            player.AddComponent(keyReader);
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
            scene.GameLoop(100);

        }
    }
}
