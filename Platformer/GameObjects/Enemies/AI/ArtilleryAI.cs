/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.System;
using SFML.Graphics;
using GAP.Utility;
using GAP.Modules;
using GAP.GameObjects.Bullets;

namespace GAP.GameObjects.Enemies.AI
{
    class ArtilleryAI : EnemyAI
    {
        public override int GetHealth()
        {
            return 20;
        }

        BulletObject SpawnBullet(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/BigBullet.png");
            im.FlipVertically();
            BulletObject bullet = new BulletObject(ObjectGroup.Falling, Team.Enemy)
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.4f, 0.4f)),
            };
            return bullet;
        }

        sbyte direction = -1;
        const float speed = 0.05f;
        const float bulletSpeed = 0.4f;
        const float maxShootCooldown = 2f;
        float shootCooldown = 3.5f;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            sbyte edge = Self.OnEdge(game.Platforms);
            if (edge != 0)
            {
                direction = (sbyte)-edge;
            }
            Self.Speed.X = direction * speed;
            ShootAction(game, time);
        }

        void ShootAction(GameObjectHandler handler, GameTime time)
        {
            if (shootCooldown <= 0)
            {
                sbyte shootDirection;
                shootCooldown = maxShootCooldown;
                if (handler.Player.HitBox.Center.X > Self.HitBox.Center.X)
                {
                    shootDirection = 1;
                }
                else
                {
                    shootDirection = -1;
                }

                ShootBullet(handler, shootDirection, 80 * (float)Math.PI / 180);
                ShootBullet(handler, shootDirection, 70 * (float)Math.PI / 180);
                ShootBullet(handler, shootDirection, 60 * (float)Math.PI / 180);
            }
            else
            {
                shootCooldown -= time.DeltaTime;
            }
        }

        void ShootBullet(GameObjectHandler handler, sbyte direction, float angle)
        {
            BulletObject spawnedBullet = SpawnBullet(Self.HitBox.Center);
            spawnedBullet.Speed = new Vector2f(
                    (float)Math.Cos(angle) * direction * bulletSpeed,
                    (float)Math.Sin(angle) * bulletSpeed
                );

            handler.Bullets.Add(spawnedBullet);
        }
    }
}
