/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.System;
using GAP.Utility;
using GAP.Modules;

namespace GAP.GameObjects.Effects
{
    /// <summary>
    /// Used mostly on dead entities, will create sprite of them that shrinks into nothingness
    /// </summary>
    class ShrinkEffect : Effect
    {
        float scale = 0.5f; // based on half-size
        const float shrinkFactor = 0.05f;
        Vector2f shrinkVector;
        public ShrinkEffect(GameObject obj)
        {
            Texture = obj.Texture;
            HitBox = obj.HitBox;
            Color = obj.Color;
            shrinkVector = shrinkFactor * HitBox.HalfSize;

            if (obj.UsesVisualBox)
            {
                HitBox.HalfSize = obj.VisualBox.HalfSize;
            }
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            scale -= shrinkFactor;
            HitBox.HalfSize -= shrinkVector;
        }

        public override bool ShouldDie()
        {
            return (scale <= shrinkFactor);
        }

        public override void OnDeath(GameObjectHandler game) { }
    }
}
