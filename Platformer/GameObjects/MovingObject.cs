/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using GAP.GameObjects.Utility;

namespace GAP.GameObjects
{
    /// <summary>
    /// Base object for the player and all enemies, projectiles and effects 
    /// </summary>
    abstract class MovingObject : ActiveObject
    {
        public readonly Team Team;
        public readonly ObjectGroup Group;

        public Vector2f OldCenterPosition;
        public Vector2f Speed;

        public int Health = 1;
        public bool OutOfBounds = false;

        public MovingObject(ObjectGroup group, Team team)
        {
            Group = group;
            Team = team;
            Color = Color.White;
        }

        /// <summary>
        /// Called by GameHandler class. Applies the objects current movement to its position
        /// </summary>
        public void Move()
        {
            OldCenterPosition = HitBox.Center;
            HitBox.Center += Speed;
        }

        /// <summary>
        /// Decides how an object will behave when it's hit
        /// </summary>
        public virtual void OnCollision(int damage)
        {
            Health -= damage;
        }

        public override bool ShouldDie()
        {
            return Health <= 0 || OutOfBounds;
        }

        // FOLLOWING ARE METHODS AND DATA USED FOR FALLING OBJECTS ONLY

        public bool OnGround = false;

        /// <summary>
        /// Called when an object lands on ground
        /// </summary>
        public virtual void TouchedGround()
        {
            OnGround = true;
        }

        /// <summary>
        /// Called when an object leaves the ground
        /// </summary>
        public virtual void LeftGround()
        {
            OnGround = false;
        }

        /// <summary>
        /// Determines whether object is on an edge of a platform
        /// </summary>
        /// <returns>direction in which the edge we are on is (-1 = left,1 = right), or 0 </returns>
        public sbyte OnEdge(List<StationaryObject> platforms)
        {
            sbyte returnValue = 0;
            foreach (StationaryObject platform in platforms)
            {
                Vector2f leftCorner = HitBox.Center - HitBox.HalfSize;
                if (Math.Abs(leftCorner.X - platform.HitBox.Center.X) < platform.HitBox.HalfSize.X &&
                    Math.Abs(leftCorner.Y - GameEnvironment.Epsilon - platform.HitBox.Center.Y) < platform.HitBox.HalfSize.Y)
                {
                    returnValue++;
                    break;
                }
            }
            foreach (StationaryObject platform in platforms)
            {
                Vector2f rightCorner = HitBox.Center + new Vector2f(HitBox.HalfSize.X, -HitBox.HalfSize.Y);
                if (Math.Abs(rightCorner.X - platform.HitBox.Center.X) < platform.HitBox.HalfSize.X &&
                    Math.Abs(rightCorner.Y - GameEnvironment.Epsilon - platform.HitBox.Center.Y) < platform.HitBox.HalfSize.Y)
                {
                    returnValue--;
                    break;
                }
            }

            return returnValue;
        }
    }
}
