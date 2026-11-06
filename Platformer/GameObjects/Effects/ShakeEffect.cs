/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.System;
using GAP.Utility;
using GAP.Modules;

namespace GAP.GameObjects.Effects
{
    /// <summary>
    /// Used on disappearing floor, will create sprite of it that will shake
    /// </summary>
    class ShakeEffect : Effect
    {
        readonly Random random;
        const float frameTime = 0.08f;
        float counter = frameTime;
        const float maxOffset = 0.2f;
        Vector2f originalPosition;
        public bool ShouldDieSettable = false;

        public ShakeEffect(StationaryObject platform)
        {
            HitBox = platform.HitBox;
            Texture = platform.Texture;
            Color = platform.Color;
            originalPosition = platform.HitBox.Center;
            random = new Random();
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            if (counter <= 0)
            {
                counter = frameTime;
                HitBox.Center = originalPosition + new Vector2f(
                    (float)random.NextDouble(), (float)random.NextDouble()
                    ) * maxOffset;
            }

            counter -= time.DeltaTime;
        }

        public override bool ShouldDie()
        {
            return ShouldDieSettable;
        }
    }
}
