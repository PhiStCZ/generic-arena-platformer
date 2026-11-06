/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

namespace GAP
{
    class Program
    {
        /// <summary>
        /// Entry point of the program, launches the game
        /// </summary>
        static void Main()
        {
            Model.PlatformerFrame game = new Model.PlatformerFrame();
            game.Run();
        }
    }
}
