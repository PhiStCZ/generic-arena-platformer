/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.System;
using GAP.GameObjects.Utility;
using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects.Enemies.AI
{
    class SpawnerAI : EnemyAI
    {
        public override int GetHealth()
        {
            return 22;
        }

        const float launchingSpeed = 0.15f;
        const float spawnTime = 0.5f;
        const float waitTime = 7f;
        float spawnTimer = spawnTime;
        byte phase = 0;

        readonly Random gen = new Random();
        const float speed = 0.08f;
        const float glidingSpeed = 0.08f;
        sbyte glidingDirection = 1;
        Vector2f goal;

        readonly EnemyObject[] spawnedFlys = new EnemyObject[4];

        const float maxGoalChangeTime = 5f;
        float timer = 0f;

        const float glideDirectionChangeTime = 1f;
        float glideTime = 0f;

        const float maneuverability = 0.02f;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            SetGoal(time);
            Maneuver();
            SpawnAction(game, time);
            UpdateSpawnedList();
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
                glideTime = glideDirectionChangeTime;
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

        void SpawnAction(GameObjectHandler game, GameTime time)
        {
            if (spawnTimer <= 0)
            {
                switch (phase)
                {
                    case 0:
                    case 1:
                        spawnTimer = spawnTime;

                        for (int i = phase; i < spawnedFlys.Length; i += 2)
                        {
                            if (spawnedFlys[i] == null)
                            {
                                EnemyObject fly = EnemySpawnClass.SpawnFlyerWithAdjustedHealth(Self.HitBox.Center, 2);
                                fly.Speed = new Vector2f(
                                    (phase == 1 ? 1 : -1) * launchingSpeed,
                                    launchingSpeed
                                );
                                spawnedFlys[i] = fly;
                                game.Enemies.Add(fly);
                                break;
                            }
                        }
                        break;
                    case 2:
                        spawnTimer = waitTime;
                        break;
                }
                phase = (byte)((phase + 1) % 3);
            }
            spawnTimer -= time.DeltaTime;
        }

        void UpdateSpawnedList()
        {
            for (int i = 0; i < spawnedFlys.Length; i++)
            {
                if (spawnedFlys[i] != null && spawnedFlys[i].Health <= 0)
                {
                    spawnedFlys[i] = null;
                }
            }
        }
    }
}
