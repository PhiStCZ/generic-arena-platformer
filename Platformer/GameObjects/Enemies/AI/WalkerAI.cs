/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects.Enemies.AI
{
    class WalkerAI : EnemyAI
    {
        sbyte direction = -1;
        const float maxSpeed = 0.1f;
        const float jumpSpeed = 0.25f;

        public override int GetHealth()
        {
            return 8;
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            Self.Speed.X = maxSpeed * direction;
            sbyte edge = Self.OnEdge(game.Platforms);
            if (edge != 0)
            {
                direction = (sbyte)-edge;
                Self.Speed.Y = jumpSpeed;
            }
        }
    }
}
