/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.System;
using SFML.Graphics;

namespace GAP.Menu
{
    /// <summary>
    /// A small helper class to help keep track of menu static variables / objects
    /// </summary>
    static class MenuConstants
    {
        public static readonly Shape SelectArrow = new RectangleShape
        {
            Texture = new Texture(new Image("./RESOURCES/Texture/PlayerBullet.png")),
            Size = new Vector2f(45, 30),
            FillColor = Color.White
        };
    }
}
