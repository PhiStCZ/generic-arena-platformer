/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.Graphics;
using SFML.System;
using GAP.GameObjects.Bullets;
using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects.Player
{
    /// <summary>
    /// A class for the player object
    /// </summary>
    class PlayerObject : MovingObject
    {
        public PlayerControls Controls;
        public PlayerData Data;

        public Vector2f RespawnPosition;

        readonly Image facingRight;
        readonly Image facingLeft;

        readonly Image dashFacingRight;
        readonly Image dashFacingLeft;

        readonly Image dashResidueRight;
        readonly Image dashResidueLeft;

        enum Direction : sbyte { left = -1, right = 1 }
        enum State { normal, dashing }

        Image GetProperImage()
        {
            if (Data.Dashing)
            {
                if (Data.Orientation == (sbyte)Direction.right)
                {
                    return dashFacingRight;
                }

                return dashFacingLeft;
            }

            if (Data.Orientation == (sbyte)Direction.right)
            {
                return facingRight;
            }

            return facingLeft;
        }

        void MakeImageWithBothOrientations(string path, ref Image toBeKept, ref Image toBeFlipped)
        {
            toBeKept = new Image(path);
            toBeKept.FlipVertically();
            toBeFlipped = new Image(path);
            toBeFlipped.FlipVertically();
            toBeFlipped.FlipHorizontally();
        }

        public PlayerObject() : base(ObjectGroup.Falling, Team.Player)
        {
            MakeImageWithBothOrientations(
                "./RESOURCES/Texture/Player.png",
                ref facingRight,
                ref facingLeft
            );

            MakeImageWithBothOrientations(
                "./RESOURCES/Texture/PlayerDashing.png",
                ref dashFacingRight,
                ref dashFacingLeft
            );

            MakeImageWithBothOrientations(
                "./RESOURCES/Texture/PlayerDashResidue.png",
                ref dashResidueRight,
                ref dashResidueLeft
            );
        }

        public override void Action(GameObjectHandler game, GameTime time)
        {
            if (Data.Health > 0)
            {
                if (Data.Dashing)
                {
                    Dash();
                }
                else
                {
                    MoveAction();
                    JumpAction();
                    VerticalSpeedAction();
                    DashAction();
                    ShootAction();
                }
                UpdateCooldowns();
                UpdateVisuals();
            }
            else if (game.State == GameObjectHandler.GameState.Playing)
            {
                game.State = GameObjectHandler.GameState.Lost;
            }
            else
            {
                Data.Health = 0;
                if (OnGround)
                {
                    Speed.X = 0;
                }

                Color = Color.Red;
            }

            void UpdateCooldowns()
            {
                if (Data.ShootCooldown > 0)
                {
                    Data.ShootCooldown -= time.DeltaTime;
                }

                if (Data.RemainingInvFrame > 0)
                {
                    Data.RemainingInvFrame -= time.DeltaTime;
                }
            }

            void MoveAction()
            {
                if (Controls.Left.IsPressed && !Controls.Right.IsPressed)
                {
                    Data.Orientation = -1;
                    Speed.X = -PlayerData.MoveSpeed;
                }
                else if (Controls.Right.IsPressed)
                {
                    Data.Orientation = 1;
                    Speed.X = +PlayerData.MoveSpeed;
                }
                else
                {
                    Speed.X = 0;
                }
            }

            void JumpAction()
            {
                if (OnGround)
                {
                    if (Controls.Jump.WholePress)
                    {
                        OnGround = false;
                        Speed.Y = PlayerData.JumpSpeed;
                    }
                }
                else if (Data.DoubleJumpCharge > 0 && Controls.Jump.FirstFramePress)
                {
                    Data.DoubleJumpCharge--;
                    Speed.Y = PlayerData.JumpSpeed;
                }
            }

            void VerticalSpeedAction()
            {
                if (Controls.Down.IsPressed)
                {
                    Speed.Y += PlayerData.DiveSpeed * time.DeltaTime;
                }
                else if (Controls.Jump.IsPressed)
                {
                    Speed.Y = (Speed.Y >= PlayerData.GlideFallingSpeed)
                        ? Speed.Y
                        : PlayerData.GlideFallingSpeed * PlayerData.AirManeuverability
                          + Speed.Y * (1 - PlayerData.AirManeuverability);
                }
                else
                {
                    Speed.Y = Math.Min(Speed.Y, PlayerData.JumpCancelSpeed);
                }
            }

            void DashAction()
            {
                if (Data.RemainingDashCooldown <= 0f)
                {
                    if (Controls.Dash.WholePress)
                    {
                        Data.Dashing = true;
                        Data.RemainingDashTime = PlayerData.DashTime;
                        Data.RemainingDashCooldown = PlayerData.DashCoolDown;
                        Data.ResidueSpawnTime = 0f;
                    }
                }
                else
                {
                    Data.RemainingDashCooldown -= time.DeltaTime;
                }
            }

            void Dash()
            {
                Speed.X = Data.Orientation * PlayerData.DashSpeed;
                Speed.Y = 0;
                Data.RemainingDashTime -= time.DeltaTime;

                if (Data.RemainingDashTime <= 0f)
                {
                    Data.Dashing = false;
                }

                Data.ResidueSpawnTime -= time.DeltaTime;

                if (Data.ResidueSpawnTime <= 0f)
                {
                    Data.ResidueSpawnTime = PlayerData.MaxResidueSpawnTime;
                    Effects.FadeEffect residue = new Effects.FadeEffect(this, 10)
                    {
                        Texture = new Texture(Data.Orientation == (sbyte)Direction.left
                            ? dashResidueLeft
                            : dashResidueRight
                        )
                    };

                    game.Effects.Add(residue);
                }
            }

            void ShootAction()
            {
                if (Controls.Shoot.WholePress && Data.ShootCooldown <= 0)
                {
                    Data.ShootCooldown = PlayerData.MaxShootCooldown;

                    Vector2f bulletPos = HitBox.Center;
                    bulletPos.X += HitBox.HalfSize.X * Data.Orientation;
                    BulletObject bullet = SpawnPlayerBullet(bulletPos, Data.Orientation);

                    game.Bullets.Add(bullet);
                }
            }

            void UpdateVisuals()
            {
                if (Data.Orientation != Data.LastOrientation || Data.Dashing != Data.WasDashing)
                {
                    Texture.Update(GetProperImage());

                    Data.LastOrientation = Data.Orientation;
                }

                Color.A = (byte)(Data.RemainingInvFrame <= 0 || Data.Dashing 
                    ? 255 
                    : 100
                );
            }
        }

        public override void TouchedGround()
        {
            base.TouchedGround();
            Data.DoubleJumpCharge = PlayerData.MaxDoubleJumpCharge;
        }
        public override void OnCollision(int damage)
        {
            Data.RemainingInvFrame = PlayerData.MaxInvulFrame;
            Data.Health -= damage;
        }
        public override bool ShouldDie()
        {
            return (Data.Health <= 0);
        }

        /// <summary>
        /// Resets player's position to RespawnPosition
        /// </summary>
        public void Respawn()
        {
            HitBox.Center = RespawnPosition;
        }

        public static PlayerObject MakePlayerObject(Vector2f position, PlayerControls controls, PlayerData data)
        {
            PlayerObject player = new PlayerObject
            {
                Color = Color.White,
                HitBox = new AABB(position, new Vector2f(0.4375f, 0.8125f)),
                UsesVisualBox = true,
                VisualBox = new AABB(new Vector2f(0, 0), new Vector2f(1.0625f, 1.0625f)),
                Controls = controls,
                Data = data
            };
            player.Texture = new Texture(player.facingRight);

            return player;
        }

        static BulletObject SpawnPlayerBullet(Vector2f position, sbyte direction)
        {
            Image im = new Image("./RESOURCES/Texture/PlayerBullet.png");
            im.FlipVertically();
            if (direction == (sbyte)Direction.left)
            {
                im.FlipHorizontally();
            }

            BulletObject bullet = new BulletObject(ObjectGroup.Flying, Team.Player)
            {
                HitBox = new AABB(position, new Vector2f(0.3125f, 0.3125f)),
                Color = Color.White,
                UsesVisualBox = true,
                Texture = new Texture(im),
                VisualBox = new AABB(new Vector2f(0, 0), new Vector2f(0.4375f, 0.3125f)),
                Speed = new Vector2f(direction * PlayerData.BulletSpeed, 0)
            };

            return bullet;
        }
    }
}
