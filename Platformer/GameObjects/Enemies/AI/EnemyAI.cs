/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.Utility;
using GAP.Modules;

namespace GAP.GameObjects.Enemies.AI
{
    /// <summary>
    /// A class for handling enemy behavior, it is not
    /// abstract so it can serve as a testing / idle AI
    /// </summary>
    class EnemyAI
    {
        public const int DefaultHealth = 3;

        public EnemyObject Self;

        /// <summary>
        /// Called by the enemy constructor for setting health
        /// </summary>
        public virtual int GetHealth()
        {
            return DefaultHealth;
        }

        public virtual void Action(GameObjectHandler game, GameTime time) { }

        public virtual void OnDeath(GameObjectHandler game) { }
    }
}
