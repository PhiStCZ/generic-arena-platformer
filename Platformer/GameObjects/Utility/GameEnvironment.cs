using System;
using SFML.System;
using GAP.Utility;

namespace GAP.GameObjects.Utility
{
    /// <summary>
    /// A class containing all global environment/gameobject-related constants and methods
    /// </summary>
    static class GameEnvironment
    {
        public const int DimensionX = 32; // measured in the ingame stationary objects
        public const int DimensionY = 19; //

        const float gravity = -0.5f;
        public static void ApplyGravity(MovingObject obj, GameTime time)
        {
            if (obj.Group == ObjectGroup.Falling)
            {
                obj.Speed.Y = Math.Max(DefaultMaxFallingSpeed, obj.Speed.Y + gravity * time.DeltaTime);
            }
        }

        /// <summary>
        /// A small unit of measurement to account for float rounding errors
        /// </summary>
        public const float Epsilon = 0.05f;

        public const float DefaultMaxFallingSpeed = -1f;
        public static readonly Vector2f DefaultHalfsize = new Vector2f(0.5f, 0.5f);

        public static Vector2f GetNormalisedVector(Vector2f v)
        {
            float scale = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (scale < Epsilon)
            {
                return v;
            }

            v /= scale;
            return v;
        }

        public static Vector2f GetNormalisedOrSmallerVector(Vector2f v)
        {
            float scale = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (scale < Epsilon)
            {
                return v;
            }

            if (scale > 1)
            {
                v /= scale;
            }

            return v;
        }

        static readonly Vector2f minCoords = new Vector2f(0, -DimensionY);
        static readonly Vector2f maxCoords = new Vector2f(DimensionX, 0);
        static readonly Vector2f reserve = new Vector2f(5, 5);

        static readonly Vector2f minBounds = minCoords - reserve;
        static readonly Vector2f maxBounds = maxCoords + reserve;

        static readonly Vector2f minPositions = minCoords + reserve;
        static readonly Vector2f maxPositions = maxCoords - reserve;

        /// <summary>
        /// returns true if obj is out of bounds
        /// </summary>
        public static bool CheckOutOfBounds(MovingObject obj)
        {
            if (obj.HitBox.Center.X < minBounds.X || obj.HitBox.Center.X > maxBounds.X ||
                obj.HitBox.Center.Y < minBounds.Y || obj.HitBox.Center.Y > maxBounds.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// returns true if obj is out offscreen
        /// </summary>
        public static bool CheckOffScreen(MovingObject obj)
        {
            if (obj.HitBox.Center.X < minCoords.X || obj.HitBox.Center.X > maxCoords.X ||
                obj.HitBox.Center.Y < minCoords.Y || obj.HitBox.Center.Y > maxCoords.Y)
            {
                return true;
            }

            return false;
        }

        public static Vector2f GetRandomPosition(Random random)
        {
            float x = (float)random.NextDouble() * (maxPositions.X - minPositions.X) + minPositions.X;
            float y = (float)random.NextDouble() * (maxPositions.Y - minPositions.Y) + minPositions.Y;

            return new Vector2f(x, y);
        }
    }
}
