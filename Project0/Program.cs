using System;

namespace CollisionExample
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CollisionExampleGame())
                game.Run();
        }
    }
}
