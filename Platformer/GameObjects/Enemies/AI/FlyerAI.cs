/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.System;
using GAP.Modules;
using GAP.Utility;
using GAP.GameObjects.Player;
using GAP.GameObjects.Utility;

namespace GAP.GameObjects.Enemies.AI
{
    class FlyerAI : EnemyAI
    {
        readonly Random gen = new Random();
        Vector2f goal;
        const float MAX_TIME_SECONDS = .8f;
        float setGoalTime = 0;
        const float speed = 0.18f;
        const float slowSpeed = 0.08f;
        const float maneuverability = 0.02f;

        const float maxAggroTime = 6f;
        const float minAggroTime = 1f;
        const float maxCalmTime = 2f;
        float aggroTime = 0f;
        bool aggressive = false;

        public override int GetHealth()
        {
            return 6;
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            AggroUpdate(time);
            if (aggressive)
            {
                SetGoal(game.Player, time);
            }
            Maneuver(game.Player);
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
            Self.Speed = newDir * maneuverability + Self.Speed * (1 - maneuverability);
        }

        void SetGoal(PlayerObject player, GameTime time)
        {
            if (setGoalTime <= 0)
            {
                setGoalTime = (float)gen.NextDouble() * MAX_TIME_SECONDS;
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
    }
}
