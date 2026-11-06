/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.System;
using GAP.Modules;
using GAP.Utility;
using GAP.GameObjects.Bullets;
using GAP.GameObjects.Player;
using GAP.GameObjects.Utility;

namespace GAP.GameObjects.Enemies.AI
{
    class SnakeAI : EnemyAI
    {
        readonly Random gen = new Random();
        Vector2f goal;
        const float maxTimeInSeconds = .8f;
        float setGoalTime = 0;
        const float speed = 0.3f;
        const float slowSpeed = 0.15f;
        const float maneuverability = 0.02f;
        const float offscreenManeuverability = 0.08f;

        public override int GetHealth()
        {
            return 14;
        }

        const float maxAggroTime = 6f;
        const float minAggroTime = 1f;
        const float maxCalmTime = 2f;
        float aggroTime = 0f;
        bool aggressive = false;

        public FollowerBullet[] bulletList;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            AggroUpdate(time);
            if (aggressive)
            {
                SetGoal(game.Player, time);
            }
            Maneuver(game.Player);
            UpdateBulletTail(game);
        }

        void Maneuver(PlayerObject player)
        {
            const float MIN_SCALE = 10f;
            float distance =
                (goal.X - Self.HitBox.Center.X) * (goal.X - Self.HitBox.Center.X) +
                (goal.Y - Self.HitBox.Center.Y) * (goal.Y - Self.HitBox.Center.Y);

            Vector2f newDir = GameEnvironment.GetNormalisedOrSmallerVector(
                (distance <= MIN_SCALE)
                ? player.HitBox.Center - Self.HitBox.Center
                : goal - Self.HitBox.Center
            );
            newDir *= aggressive ? speed : slowSpeed;

            float maneuverability = GameEnvironment.CheckOffScreen(Self)
                ? offscreenManeuverability
                : SnakeAI.maneuverability;
            Self.Speed = newDir * maneuverability + Self.Speed * (1 - maneuverability);
        }

        void SetGoal(PlayerObject player, GameTime time)
        {
            if (setGoalTime <= 0)
            {
                setGoalTime = (float)gen.NextDouble() * maxTimeInSeconds;
                goal = (2 * player.HitBox.Center) - Self.HitBox.Center;
            }

            setGoalTime -= time.DeltaTime;
        }

        void AggroUpdate(GameTime time)
        {
            if (aggroTime <= 0)
            {
                aggressive = !aggressive;
                float maxTime = aggressive ? maxAggroTime : maxCalmTime;
                aggroTime = (float)gen.NextDouble() * (maxTime - minAggroTime) + minAggroTime;
                if (aggressive)
                {
                    goal = GameEnvironment.GetRandomPosition(this.gen);
                }
            }

            aggroTime -= time.DeltaTime;
        }

        void UpdateBulletTail(GameObjectHandler game)
        {
            if (bulletList == null)
            {
                SpawnTail(game);
            }
            static void Follow(MovingObject first, MovingObject follower)
            {
                Vector2f newDir = (first.HitBox.Center - follower.HitBox.Center) / 8;
                follower.Speed = newDir;
            }

            if (bulletList.Length > 0)
            {
                Follow(Self, bulletList[0]);
            }

            for (int i = 1; i < bulletList.Length; i++)
            {
                Follow(bulletList[i - 1], bulletList[i]);
            }
        }

        void SpawnTail(GameObjectHandler game)
        {
            bulletList = new FollowerBullet[7];
            for (int i = 0; i < bulletList.Length; i++)
            {
                FollowerBullet bullet = FollowerBullet.SpawnFollowerBullet(Self.HitBox.Center);
                bulletList[i] = bullet;
                game.Bullets.Add(bullet);
            }
        }

        public override void OnDeath(GameObjectHandler game)
        {
            foreach (FollowerBullet bullet in bulletList)
            {
                bullet.parentIsDead = true;
            }
        }
    }
}
