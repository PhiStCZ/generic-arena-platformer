/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;
using GAP.Utility;
using GAP.Modules;
using GAP.GameObjects.Enemies;

namespace GAP.GameObjects.Effects
{
    /// <summary>
    /// Used as a warning from spawning enemies, will create a portal animation
    /// </summary>
    class SpawnEffect : Effect
    {
        static Image[] images; // the frames of this effect's animation
        void LoadImages()
        {
            images = new Image[8];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Image($"./RESOURCES/Texture/Spawn{i}.png");
            }
        }

        public SpawnEffect(Vector2f position, EnemySpawnClass.Spawn SpawnFunction)
        {
            if (images == null)
            {
                LoadImages();
            }

            HitBox = new AABB(position, new Vector2f(1, 1));
            whatToSpawn = SpawnFunction;
            Texture = new Texture(images[0]);
            Color = Color.White;
        }

        readonly EnemySpawnClass.Spawn whatToSpawn;
        const float frameTime = 0.08f;
        float counter = frameTime;
        byte state = 0;

        public override void Action(GameObjectHandler game, GameTime time)
        {
            if (counter <= 0)
            {
                counter = frameTime;
                state++;
                Texture.Update(images[state]);
                if (state == 3)
                {
                    EnemyObject enemy = whatToSpawn(HitBox.Center);
                    game.Enemies.Add(enemy);
                }
            }

            counter -= time.DeltaTime;
        }

        public override bool ShouldDie()
        {
            return (state >= images.Length - 1);
        }

        public override void OnDeath(GameObjectHandler game) { }
    }
}
