/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using GAP.GameObjects;
using GAP.GameObjects.Bullets;
using GAP.GameObjects.Effects;
using GAP.GameObjects.Enemies;
using GAP.GameObjects.Player;
using GAP.GameObjects.Utility;
using GAP.Utility;

namespace GAP.Modules
{
    /// <summary>
    /// Main handler of the object model. All object related processes go through it
    /// </summary>
    class GameObjectHandler : IModule
    {
        public enum GameState
        {
            Playing,
            Lost,
            Won
        };

        public GameState State = GameState.Playing;

        public List<StationaryObject> Platforms;
        public List<EnemyObject> Enemies;
        public List<BulletObject> Bullets;
        public List<Effect> Effects;

        public readonly PlayerObject Player;

        public readonly MapLoader MapLoader;

        public GameObjectHandler(PlayerControls inputControls, PlayerData pData)
        {
            MapLoader = new MapLoader(GameEnvironment.DimensionX, GameEnvironment.DimensionY);
            Player = PlayerObject.MakePlayerObject(new Vector2f(), inputControls, pData);
        }

        public void Load()
        {
            Platforms = new List<StationaryObject>();
            Enemies = new List<EnemyObject>();
            Bullets = new List<BulletObject>();
            Effects = new List<Effect>();

            MapLoader.LoadLevelAsStart(this);
            Player.Respawn();
        }

        public void Update(GameTime time)
        {
            MapLoader.Update(this, time);

            MoveAllBullets();
            CheckBulletCollisions();

            MoveAllEnemies();
            EnemyPlayerCollision();

            UpdatePlayer();
            UpdateEffects();

            KillDeadEntities();


            void MoveAllBullets()
            {

                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletObject bullet = Bullets[i];
                    bullet.Move();

                    Bullets[i].OutOfBounds = GameEnvironment.CheckOutOfBounds(bullet);

                    if (bullet.Group == ObjectGroup.Falling)
                    {
                        GameEnvironment.ApplyGravity(bullet, time);
                        bullet.FloorCollision(this);
                    }
                }
            }

            void CheckBulletCollisions()
            {
                foreach (BulletObject bullet in Bullets)
                {
                    if (bullet.Team == Team.Player)
                    {
                        foreach (EnemyObject enemy in Enemies)
                        {
                            if (GameObject.Collides(bullet.HitBox, enemy.HitBox))
                            {
                                enemy.OnCollision(1);
                                bullet.OnCollision(1);
                                break;
                            }
                        }
                    }
                    else if (!Player.Data.Invulnerable && GameObject.Collides(bullet.HitBox, Player.HitBox))
                    {
                        Player.OnCollision(1);
                        bullet.OnCollision(1);
                    }
                }
            }

            void MoveAllEnemies()
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    EnemyObject enemy = Enemies[i];
                    enemy.Action(this, time);
                    enemy.LeftGround();


                    if (enemy.Group == ObjectGroup.Falling)
                    {
                        GameEnvironment.ApplyGravity(enemy, time);

                        enemy.Move();

                        foreach (GameObject floor in Platforms)
                        {
                            if (GameObject.CheckFloorCollision(enemy, (StationaryObject)floor))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        enemy.Move();
                    }
                }
            }

            void EnemyPlayerCollision()
            {
                if (!Player.Data.Invulnerable || Player.Data.Dashing)
                {
                    foreach (EnemyObject enemy in Enemies)
                    {
                        if (GameObject.Collides(enemy.HitBox, Player.HitBox))
                        {
                            if (Player.Data.Dashing)
                            {
                                if (!enemy.DashedThrough)
                                {
                                    enemy.OnCollision(PlayerData.DashDamage);
                                    enemy.DashedThrough = true;
                                }
                            }
                            else
                            {
                                Player.OnCollision(enemy.Damage);
                            }
                        }
                    }
                }

                if (Player.Data.Dashing != Player.Data.WasDashing)
                {
                    if (!Player.Data.Dashing && Player.Data.WasDashing)
                    {
                        foreach (EnemyObject enemy in Enemies)
                        {
                            enemy.DashedThrough = false;
                        }
                    }
                    Player.Data.WasDashing = Player.Data.Dashing;
                }
            }

            void UpdatePlayer()
            {
                GameEnvironment.ApplyGravity(Player, time);
                Player.Action(this, time);
                Player.Move();

                Player.LeftGround();

                foreach (StationaryObject floor in Platforms)
                {
                    GameObject.CheckFloorCollision(Player, floor);
                }

                if (GameEnvironment.CheckOutOfBounds(Player))
                {
                    Player.Respawn(); Player.OnCollision(1);
                }
            }

            void UpdateEffects()
            {
                foreach (Effect effect in Effects)
                {
                    effect.Action(this, time);
                }
            }

            void KillDeadEntities()
            {
                int i = 0;
                while (i < Enemies.Count)
                {
                    if (Enemies[i].ShouldDie())
                    {
                        Enemies[i].OnDeath(this);
                        Enemies.RemoveAt(i);
                        Player.Data.Heal();
                    }
                    else
                    {
                        i++;
                    }
                }

                i = 0;
                while (i < Bullets.Count)
                {
                    if (Bullets[i].ShouldDie())
                    {
                        Bullets[i].OnDeath(this);
                        Bullets.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                i = 0;
                while (i < Effects.Count)
                {
                    if (Effects[i].ShouldDie())
                    {
                        Effects[i].OnDeath(this);
                        Effects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        readonly Vector2f drawOffset = new Vector2f(0.5f, -0.5f);
        readonly Vector2f drawScale = new Vector2f(32f, -32f);
        public void Draw(RenderWindow window)
        {
            void DrawObject(GameObject obj)
            {
                Shape shape = obj.Draw();

                if (shape != null)
                {
                    Vector2f position = shape.Position;
                    position.X = (position.X + drawOffset.X) * drawScale.X;
                    position.Y = (position.Y + drawOffset.Y) * drawScale.Y;
                    shape.Position = position;

                    Vector2f scale = shape.Scale;
                    scale.X *= drawScale.X;
                    scale.Y *= drawScale.Y;
                    shape.Scale = scale;

                    window.Draw(shape);
                }
            }

            foreach (GameObject ptf in Platforms)
            {
                DrawObject(ptf);
            }

            DrawObject(Player);

            foreach (GameObject enm in Enemies)
            {
                DrawObject(enm);
            }

            foreach (GameObject blt in Bullets)
            {
                DrawObject(blt);
            }

            foreach (GameObject efx in Effects)
            {
                DrawObject(efx);
            }
        }
    }
}
