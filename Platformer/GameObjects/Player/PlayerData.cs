/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;

namespace GAP.GameObjects.Player
{
    /// <summary>
    /// a class containing player-related constants/data
    /// </summary>
    class PlayerData
    {
        public const float MoveSpeed = 0.11f;
        public const float JumpSpeed = 0.3f;
        public const float JumpCancelSpeed = 0f;
        public const float AirManeuverability = 0.175f;

        public const float GlideFallingSpeed = -0.05f;
        public const float DiveSpeed = -0.75f;

        public const byte MaxDoubleJumpCharge = 1;
        public byte DoubleJumpCharge = 0;

        public sbyte Orientation = 1;
        public sbyte LastOrientation = 1;

        public bool Dashing = false;
        public bool WasDashing = false;
        public const float DashSpeed = 0.3125f;

        public const float DashTime = 0.3f;
        public float RemainingDashTime = 0;

        public const float DashCoolDown = 1f;
        public float RemainingDashCooldown = 0;

        public const float MaxInvulFrame = 2f;
        public float RemainingInvFrame = 0;
        public bool Invulnerable => RemainingInvFrame > 0 || Dashing;

        public const float BulletSpeed = 0.4f;
        public const float MaxShootCooldown = 0.4f;
        public float ShootCooldown = 0.4f;
        public const int DashDamage = 3;

        public int Health = 5;
        public int MaxHealth = 10;

        public const float MaxResidueSpawnTime = 0.04f;
        public float ResidueSpawnTime = 0f;

        public void Heal()
        {
            Health = Math.Min(Health + 1, MaxHealth);
        }
    }
}
