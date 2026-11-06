/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.Graphics;
using SFML.System;
using GAP.Modules;
using GAP.Utility;
using GAP.GameObjects.Bullets;
using GAP.GameObjects.Utility;

namespace GAP.GameObjects.Enemies.AI
{
    class ShotgunFlyAI : EnemyAI
    {
        public override int GetHealth()
        {
            return 12;
        }

        BulletObject SpawnBullet(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Bullet.png");
            im.FlipVertically();
            BulletObject bullet = new BulletObject(ObjectGroup.Flying, Team.Enemy)
            {
                HitBox = new AABB(position, new Vector2f(0.3f, 0.3f)),
                Texture = new Texture(im)
            };
            return bullet;
        }
        const float bulletSpeed = 0.075f;
        const float maxShootTime = 3f;
        const float minShootTime = 1f;
        float shootTimer = maxShootTime;

        const byte shotsToEachSide = 2;
        const float spread = 0.25f;

        readonly Random gen = new Random();
        const float speed = 0.08f;
        const float glidingSpeed = 0.15f;
        sbyte glidingDirection = 1;
        Vector2f goal;

        const float maxGoalChangeTime = 5f;
        float timer = 0f;

        const float glideDirChangeTime = 1f;
        float glideTime = 0f;

        const float maneuverability = 0.02f;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            SetGoal(time);
            Maneuver();
            ShootAction(game, time);
        }

        void SetGoal(GameTime time)
        {
            if (timer <= 0f)
            {
                timer = (float)(gen.NextDouble()) * maxGoalChangeTime;
                goal = GameEnvironment.GetRandomPosition(gen);
            }
            timer -= time.DeltaTime;

            if (glideTime <= 0)
            {
                glidingDirection = (sbyte)-glidingDirection;
                glideTime = glideDirChangeTime;
            }
            glideTime -= time.DeltaTime;
        }

        void Maneuver()
        {
            Vector2f newDir = GameEnvironment.GetNormalisedOrSmallerVector(goal - Self.HitBox.Center);
            newDir *= speed;
            newDir.Y += glidingSpeed * glidingDirection;

            Self.Speed = newDir * maneuverability + Self.Speed * (1 - maneuverability);
        }

        void ShootAction(GameObjectHandler game, GameTime time)
        {
            if (shootTimer <= 0)
            {
                shootTimer = (float)gen.NextDouble() * maxShootTime + minShootTime;
                Vector2f dir = game.Player.HitBox.Center - Self.HitBox.Center;
                float angle = (float)Math.Atan(dir.Y / dir.X);
                if (dir.X < 0)
                {
                    angle += (float)Math.PI;
                }

                ShootBullet(game, angle);
                for (int i = 1; i <= shotsToEachSide; i++)
                {
                    ShootBullet(game, angle + spread * i);
                    ShootBullet(game, angle - spread * i);
                }
            }
            shootTimer -= time.DeltaTime;
        }

        void ShootBullet(GameObjectHandler game, float angle)
        {
            BulletObject bullet = SpawnBullet(Self.HitBox.Center);
            bullet.Speed = new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * bulletSpeed;
            game.Bullets.Add(bullet);
        }
    }
}
