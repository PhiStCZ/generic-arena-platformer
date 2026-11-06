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
    class BomberAI : EnemyAI
    {
        public override int GetHealth()
        {
            return 18;
        }

        readonly Random gen = new Random();
        byte phase = 0;
        const float maneuverability = 0.02f;
        const float speed = 0.08f;
        const float waitTime = 1f;
        const float launchTime = 2f;
        const float repositionTime = 3f;
        const float minRepositionTime = 1f;
        float timer = waitTime;

        const float bulletSpeed = 0.5f;
        const float launchingSpeed = 0.02f;
        Vector2f goal;


        readonly BulletObject[] followerBullets = new BulletObject[2];
        readonly BulletObject[] bombs = new BulletObject[2];


        BulletObject SpawnBombBullet(Vector2f position)
        {
            BulletObject bullet = new BulletObject(ObjectGroup.Flying, Team.Enemy)
            {
                HitBox = new AABB(position, new Vector2f(0.6f, 0.6f)),
                Color = Color.White,
                Texture = new Texture(new Image("./RESOURCES/Texture/YBigBullet.png"))
            };
            return bullet;
        }

        const float followerBulletOrbitPeriod = 5f;
        const float followerBulletDistance = 2f;
        float followerBulletAngle = 0f;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            UpdateFollowerBullets(game, time);
            UpdateState(game, time);
            Maneuver();
        }

        void UpdateFollowerBullets(GameObjectHandler game, GameTime time)
        {
            int count = followerBullets.Length;
            for (int i = 0; i < count; i++)
            {
                float angle = followerBulletAngle + (2 * (float)Math.PI * i / count);
                Vector2f dir = new Vector2f(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                );

                if (followerBullets[i] == null)
                {
                    BulletObject bullet = FollowerBullet.SpawnFollowerBullet(Self.HitBox.Center);
                    game.Bullets.Add(bullet);
                    followerBullets[i] = bullet;
                }

                followerBullets[i].HitBox.Center =
                    Self.HitBox.Center + (dir * followerBulletDistance);
            }
            followerBulletAngle = (
                followerBulletAngle +
                time.DeltaTime * ((float)Math.PI * 2) / followerBulletOrbitPeriod
            ) % ((float)Math.PI * 2);
        }

        void UpdateState(GameObjectHandler game, GameTime time)
        {
            switch (phase)
            {
                case 0: //wait
                    if (goal.X == 0)
                    {
                        goal = Self.HitBox.Center;
                    }
                    if (timer <= 0f)
                    {
                        UpdatePhaseAndSetTimer(launchTime);
                        for (int i = 0; i < bombs.Length; i++)
                        {
                            Vector2f pos = Self.HitBox.Center;
                            pos.X += 2 * followerBulletDistance * (i == 0 ? 1 : -1);
                            bombs[i] = SpawnBombBullet(pos);
                            bombs[i].Speed = new Vector2f(0f, launchingSpeed);

                            game.Bullets.Add(bombs[i]);
                        }
                    }
                    break;
                case 1: //spawn
                    if (timer <= 0f)
                    {
                        UpdatePhaseAndSetTimer(launchTime);
                        for (int i = 0; i < bombs.Length; i++)
                        {
                            Vector2f dir = game.Player.HitBox.Center - bombs[i].HitBox.Center;
                            bombs[i].Speed = GameEnvironment.GetNormalisedOrSmallerVector(dir) * bulletSpeed;
                            bombs[i] = null;
                        }
                    }
                    break;
                case 2: //launch
                    if (timer <= 0f)
                    {
                        UpdatePhaseAndSetTimer((float)(gen.NextDouble() * repositionTime) + minRepositionTime);
                        goal = GameEnvironment.GetRandomPosition(gen);
                    }
                    break;
                case 3: //reposition
                    if (timer <= 0f)
                    {
                        UpdatePhaseAndSetTimer(waitTime);
                        goal = Self.HitBox.Center;
                    };
                    break;
            }

            void UpdatePhaseAndSetTimer(float nextTime)
            {
                phase = (byte)((phase + 1) % 4);
                timer = nextTime;
            }
            timer -= time.DeltaTime;
        }

        void Maneuver()
        {
            Vector2f dir = goal - Self.HitBox.Center;
            Self.Speed = (GameEnvironment.GetNormalisedOrSmallerVector(dir) *
                speed * maneuverability) + Self.Speed * (1 - maneuverability);
        }

        public override void OnDeath(GameObjectHandler game)
        {
            foreach (FollowerBullet bullet in this.followerBullets)
            {
                bullet.parentIsDead = true;
            }
            foreach (BulletObject bullet in this.bombs)
            {
                if (bullet != null)
                {
                    bullet.Health = 0;
                }
            }
        }
    }
}
