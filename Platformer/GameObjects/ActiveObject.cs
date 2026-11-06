/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.GameObjects.Effects;
using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects
{
    /// <summary>
    /// An object with extra methods for making actions of its own
    /// </summary>
    class ActiveObject : GameObject
    {
        /// <summary>
        /// Called so the object can perform a custom action
        /// </summary>
        public virtual void Action(GameObjectHandler game, GameTime time)
        { 
            // by default, this is idle
        }

        /// <summary>
        /// Called by GameHandler. If the object should be destroyed, this should return true
        /// </summary>
        public virtual bool ShouldDie()
        {
            return false;
        }

        /// <summary>
        /// Any action the object wants to perform before being removed
        /// </summary>
        public virtual void OnDeath(GameObjectHandler game)
        {
            ShrinkEffect f = new ShrinkEffect(this);
            game.Effects.Add(f);
        }
    }
}
