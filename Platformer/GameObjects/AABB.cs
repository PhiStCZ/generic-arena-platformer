/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.System;

namespace GAP.GameObjects
{
    /// <summary>
    /// the basic structure for the platformer object model
    /// </summary>
    struct AABB
    {
        public Vector2f Center;
        public Vector2f HalfSize;

        public AABB(Vector2f center, Vector2f halfSize)
        {
            this.Center = center;
            this.HalfSize = halfSize;
        }
        public AABB(Vector2f center)
        {
            this.Center = center;
            this.HalfSize = Utility.GameEnvironment.DefaultHalfsize;
        }
    }
}
