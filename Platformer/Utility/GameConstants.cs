/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;

namespace GAP.Utility
{
    /// <summary>
    /// A small helper class to keep track of constants
    /// </summary>
    class GameConstants
    {
        public readonly Vector2f ScreenDimensions;
        public readonly int TargetFPS;
        public readonly float TargetFrameTime;

        public GameConstants(int fps, uint width, uint height)
        {
            TargetFPS = fps;
            TargetFrameTime = 1f / fps;
            ScreenDimensions = new Vector2f(width, height);
        }

        /// <summary>
        /// Puts the center of a text in its origin (so it basically centers it)
        /// </summary>
        public static void CenterText(Text text)
        {
            text.Position -= text.FindCharacterPos((uint)((text.DisplayedString.Length / 2) + 1));
        }
    }
}
