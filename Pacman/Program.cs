namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            /*LevelCreation level = new LevelCreation();
            level.Create();*/

            MenuCreation menu = new MenuCreation();

            menu.Run();
        }
    }
}
