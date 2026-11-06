/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.Graphics;
using SFML.System;

namespace GAP.GameObjects
{
    /// <summary>
    /// Base class for all objects in the object model
    /// </summary>
    abstract class GameObject
    {
        public AABB HitBox;
        public Color Color;
        public Texture Texture;

        public bool UsesVisualBox = false;
        public AABB VisualBox; // its center is relative to the hitbox center (so it doesn't need to be updated)

        /// <summary>
        /// Called by the Draw method of GameHandler class
        /// </summary>
        /// <returns>the object to be displayed</returns>
        public Shape Draw()
        {
            AABB shownShape = UsesVisualBox ? VisualBox : HitBox;
            Vector2f pos = HitBox.Center;
            if (UsesVisualBox)
            {
                pos += VisualBox.Center;
            }
            pos -= shownShape.HalfSize;
            RectangleShape shape = new RectangleShape
            {
                Size = shownShape.HalfSize * 2,
                Position = pos,
                FillColor = Color,
                Texture = Texture
            };

            return shape;
        }

        /// <summary>
        /// returns true if object one and two collide
        /// </summary>
        public static bool Collides(AABB one, AABB two)
        {
            return Math.Abs(one.Center.X - two.Center.X) < one.HalfSize.X + two.HalfSize.X
                && Math.Abs(one.Center.Y - two.Center.Y) < one.HalfSize.Y + two.HalfSize.Y;
        }


        /// <summary>
        /// Checks if the object 'entity' is on the 'floor', if it is, returns true and pulls entity up to the floor (so it doesnt fall through)
        /// </summary>
        public static bool CheckFloorCollision(MovingObject entity, StationaryObject floor)
        {
            if (Collides(entity.HitBox, floor.HitBox) && WasAboveFloor(entity, floor))
            {
                entity.HitBox.Center.Y = entity.HitBox.HalfSize.Y + floor.HitBox.Center.Y + floor.HitBox.HalfSize.Y;
                entity.Speed.Y = 0;
                entity.TouchedGround();
                return true;
            }
            return false;

            static bool WasAboveFloor(MovingObject entity, StationaryObject floor)
            {
                float entityBottom = entity.OldCenterPosition.Y - entity.HitBox.HalfSize.Y;
                float floorTop = floor.HitBox.Center.Y + floor.HitBox.HalfSize.Y - Utility.GameEnvironment.Epsilon;
                return floorTop < entityBottom;
            }
        }
    }
}
