/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects.Enemies
{
    /// <summary>
    /// A base class for all enemies
    /// </summary>
    class EnemyObject : MovingObject
    {
        public readonly AI.EnemyAI AI;
        public bool DashedThrough = false;
        public byte Damage = 1;

        public EnemyObject(ObjectGroup group, AI.EnemyAI ai) : base(group, Team.Enemy)
        {
            AI = ai;
            AI.Self = this;
            Health = AI.GetHealth();
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            AI.Action(game, time);
        }

        public override void OnDeath(GameObjectHandler game)
        {
            base.OnDeath(game);
            AI.OnDeath(game);
        }
    }
}
