/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using GAP.Utility;

namespace GAP.Modules
{
    /// <summary>
    /// A component class to be used by the GameFrame class
    /// </summary>
    interface IModule
    {
        /// <summary>
        /// Called once at the start to initialise all components
        /// </summary>
        void Load();

        /// <summary>
        /// Called every frame to update the status of objects
        /// </summary>
        void Update(GameTime time);

        /// <summary>
        /// Called every frame to display all visible objects
        /// </summary>
        void Draw(RenderWindow window);
    }
}
