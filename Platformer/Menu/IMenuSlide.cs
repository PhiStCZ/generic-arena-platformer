/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;

namespace GAP.Menu
{
    /// <summary>
    /// An interface for menu slides
    /// </summary>
    interface IMenuSlide
    {
        /// <summary>
        /// Called every frame by MenuHandler to display all visible objects
        /// </summary>
        public void Draw(RenderWindow window);

        /// <summary>
        /// Called when the Up button is pressed
        /// </summary>
        public void OnUp();

        /// <summary>
        /// Called when the Down button is pressed
        /// </summary>
        public void OnDown();

        /// <summary>
        /// Called when the Confirm button is pressed
        /// </summary>
        public void OnConfirm();
    }
}
