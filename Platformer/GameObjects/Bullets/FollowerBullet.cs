/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;

namespace GAP.GameObjects.Bullets
{
    class FollowerBullet : BulletObject
    {
        public FollowerBullet() : base(ObjectGroup.Flying, Team.Enemy) { }

        public bool parentIsDead = false;
        public override void OnCollision(int damage) { }

        public override bool ShouldDie()
        {
            return parentIsDead;
        }

        public static FollowerBullet SpawnFollowerBullet(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/YBullet.png");
            im.FlipVertically();
            FollowerBullet bullet = new FollowerBullet()
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.3f, 0.3f))
            };
            return bullet;
        }
    }
}
