/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;

namespace GAP.GameObjects
{
    /// <summary>
    /// An object without any actions
    /// </summary>
    class StationaryObject : GameObject
    {
        public StationaryObject(byte variation, Vector2f position)
        {
            Image im = new Image($"./RESOURCES/Texture/Env_Ground{variation}.png");
            im.FlipVertically();
            Texture = new Texture(im);
            Color = Color.Cyan;
            HitBox = new AABB(position, new Vector2f(0.5f, 0.5f));
        }

        public StationaryObject(Vector2f position) : this(0, position) { }
    }
}
