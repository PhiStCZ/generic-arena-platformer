/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.Modules;

namespace GAP.GameObjects.Bullets
{
    class BulletObject : MovingObject
    {
        public BulletObject(ObjectGroup group, Team team) : base(group, team) { }

        public void FloorCollision(GameObjectHandler game)
        {
            foreach (StationaryObject floor in game.Platforms)
            {
                if (CheckFloorCollision(this, floor))
                {
                    Health = 0;
                }
            }
        }
    }
}
