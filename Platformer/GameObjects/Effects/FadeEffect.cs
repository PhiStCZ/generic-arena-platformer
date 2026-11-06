/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using GAP.Utility;
using GAP.Modules;

namespace GAP.GameObjects.Effects
{
    /// <summary>
    /// A sprite that fades into nothingness
    /// </summary>
    class FadeEffect : Effect
    {
        byte fadeFactor;
        public FadeEffect(GameObject obj, byte fadeFactor)
        {
            Texture = obj.Texture;
            HitBox = obj.HitBox;
            Color = obj.Color;

            if (obj.UsesVisualBox)
            {
                HitBox.HalfSize = obj.VisualBox.HalfSize;
            }

            this.fadeFactor = fadeFactor;
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            Color.A -= fadeFactor;
        }

        public override bool ShouldDie()
        {
            return (Color.A < fadeFactor);
        }

        public override void OnDeath(GameObjectHandler game) { }
    }
}