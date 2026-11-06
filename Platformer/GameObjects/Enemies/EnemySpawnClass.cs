/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;
using GAP.GameObjects.Enemies.AI;

namespace GAP.GameObjects.Enemies
{
    /// <summary>
    /// Contains methods to spawn all enemies
    /// </summary>
    class EnemySpawnClass
    {
        public delegate EnemyObject Spawn(Vector2f position);

        public static EnemyObject SpawnWalker(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Walker.png");
            im.FlipVertically();
            EnemyObject walker = new EnemyObject(ObjectGroup.Falling, new WalkerAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.5f, 0.5f))
            };

            return walker;
        }

        public static EnemyObject SpawnFlyer(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Flyer.png");
            im.FlipVertically();
            EnemyObject fly = new EnemyObject(ObjectGroup.Flying, new FlyerAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.5f, 0.5f))
            };
            return fly;
        }

        public static EnemyObject SpawnFlyerWithAdjustedHealth(Vector2f position, int health)
        {
            EnemyObject fly = SpawnFlyer(position);
            fly.Health = health;
            return fly;
        }

        public static EnemyObject SpawnSnake(Vector2f position)
        {
            SnakeAI ai = new SnakeAI();

            Image im = new Image("./RESOURCES/Texture/Enemy_Snake.png");
            im.FlipVertically();
            EnemyObject enemy = new EnemyObject(ObjectGroup.Flying, ai)
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.5f, 0.5f)),
            };
            return enemy;
        }

        public static EnemyObject SpawnArtillery(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Artillery.png");
            im.FlipVertically();

            position.Y += 0.25f; // adjust for its size
            EnemyObject enemy = new EnemyObject(ObjectGroup.Falling, new ArtilleryAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.75f, 0.75f)),
            };
            return enemy;
        }

        public static EnemyObject SpawnShotgunFly(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Shotgun.png");
            im.FlipVertically();

            EnemyObject enemy = new EnemyObject(ObjectGroup.Flying, new ShotgunFlyAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.5f, 0.5f)),
            };
            return enemy;
        }

        public static EnemyObject SpawnBomber(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Bomber.png");
            im.FlipVertically();

            EnemyObject enemy = new EnemyObject(ObjectGroup.Flying, new BomberAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.5f, 0.5f))
            };
            return enemy;
        }

        public static EnemyObject SpawnSpawner(Vector2f position)
        {
            Image im = new Image("./RESOURCES/Texture/Enemy_Spawner.png");
            im.FlipVertically();

            EnemyObject enemy = new EnemyObject(ObjectGroup.Flying, new SpawnerAI())
            {
                Texture = new Texture(im),
                HitBox = new AABB(position, new Vector2f(0.75f, 0.75f))
            };
            return enemy;
        }
    }

}
