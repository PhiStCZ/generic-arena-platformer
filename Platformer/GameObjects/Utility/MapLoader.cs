/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using GAP.GameObjects.Effects;
using GAP.GameObjects.Enemies;
using GAP.Modules;
using GAP.Utility;

namespace GAP.GameObjects.Utility
{
    /// <summary>
    /// A class for loading all levels. All levels are loaded from files
    /// </summary>
    class MapLoader
    {
        readonly string levelsPath = "./RESOURCES/Levels/";
        enum MapLoadState
        {
            Idle,
            RemovingPlatforms,
            Spawning,
            CoolingDown,
        }

        MapLoadState state = MapLoadState.Idle;
        const float counterTime = 0.5f;
        const float shakingTime = 2.5f;
        const float spawningTime = 1.5f;
        const float cooldownTime = 3f;
        float counter = 0;

        readonly Random random = new Random(87654321);
        readonly int dimensionX;
        readonly int dimensionY;

        public MapLoader(int dimensionX, int dimensionY)
        {
            this.dimensionX = dimensionX;
            this.dimensionY = dimensionY;
        }

        const int maxLevelNumber = 20;
        int nextLevelNumber = 1;
        public int LevelNumber => nextLevelNumber - 1;


        List<Effect> toSpawn;

        List<int> removingIndeces;
        List<ShakeEffect> shakingTiles;

        public void Update(GameObjectHandler game, GameTime time)
        {
            switch (state)
            {
                case MapLoadState.Idle:
                    if (game.Enemies.Count == 0)
                    {
                        if (nextLevelNumber > maxLevelNumber)
                        {
                            if (game.State == GameObjectHandler.GameState.Playing)
                            {
                                game.State = GameObjectHandler.GameState.Won;
                            }
                        }
                        else if (counter <= 0)
                        {
                            LoadLevelAsNext(game, nextLevelNumber);
                        }

                        counter -= time.DeltaTime;
                    }
                    break;
                case MapLoadState.RemovingPlatforms:
                    counter -= time.DeltaTime;
                    if (counter <= 0)
                    {
                        removingIndeces.Sort();
                        for (int i = 0; i < removingIndeces.Count; i++)
                        {
                            game.Platforms.RemoveAt(removingIndeces[i] - i);
                            shakingTiles[i].ShouldDieSettable = true;
                        }
                        removingIndeces = null;
                        shakingTiles = null;
                        state = MapLoadState.Spawning;
                        counter = spawningTime;
                    }
                    break;
                case MapLoadState.Spawning:
                    counter -= time.DeltaTime;
                    if (counter <= 0)
                    {
                        game.Effects = toSpawn;
                        toSpawn = null;

                        counter = cooldownTime;
                        state = MapLoadState.CoolingDown;
                    }
                    break;
                case MapLoadState.CoolingDown:
                    counter -= time.DeltaTime;
                    if (counter <= 0)
                    {
                        counter = counterTime;
                        state = MapLoadState.Idle;
                    }
                    break;
            }
        }

        /// <summary>
        /// Base class for loading levels
        /// </summary>
        void LoadAnyLevel(GameObjectHandler objects, int levelNumber, int lastLevelNumber)
        {
            List<Vector2f> toRemove = new List<Vector2f>();
            toSpawn = new List<Effect>();

            using System.IO.StreamReader newReader = new System.IO.StreamReader
                (levelsPath + levelNumber.ToString("000") + ".txt");
            using System.IO.StreamReader oldReader = new System.IO.StreamReader
                (levelsPath + (lastLevelNumber).ToString("000") + ".txt");

            int y = 0;
            while (!newReader.EndOfStream && y < dimensionY)
            {
                string newLine = newReader.ReadLine();
                string oldLine = oldReader.ReadLine();
                int x = 0;
                while (x < newLine.Length && x < dimensionX)
                {
                    char oldX = x < oldLine.Length ? oldLine[x] : ' ';

                    SpawnEntityFromSymbol(objects, toRemove, oldX, newLine[x], x, -y);
                    x++;
                }
                y++;
            }

            MarkPlatforms(objects, toRemove);

            counter = shakingTime;
            state = MapLoadState.RemovingPlatforms;
            nextLevelNumber++;
        }

        /// <summary>
        /// Mostly used as a debug option (the nextLevelNumber variable is set to a number greater than one at the start)
        /// </summary>
        public void LoadLevelAsStart(GameObjectHandler objects)
        {
            LoadAnyLevel(objects, nextLevelNumber, 0);
        }

        /// <summary>
        /// Main method called for loading levels
        /// </summary>
        public void LoadLevelAsNext(GameObjectHandler objects, int levelNumber)
        {
            LoadAnyLevel(objects, levelNumber, levelNumber - 1);
        }

        /// <summary>
        /// All symbols the maploader recognises and can load
        /// </summary>
        enum Tiles
        {
            Empty               = ' ',
            Platform            = '-',
            PlayerSpawn         = 'p',

            EnemyWalker         = 'w',
            EnemyArtillery      = 'W',
            EnemyFlyer          = 'f',
            EnemySnake          = 'F',
            EnemyShotgunFly     = 's',
            EnemyBomber         = 'B',
            EnemySpawner        = 'S',
        };

        /// <summary>
        /// given a symbol and a position, this will add a platform, or spawn an enemy 
        /// (it will also store to-be-removed platforms in the toRemove list)
        /// </summary>
        void SpawnEntityFromSymbol(
            GameObjectHandler game,
            List<Vector2f> toRemove,
            char oldSymbol,
            char newSymbol,
            int x, int y
        )
        {
            bool wasPlatform = (oldSymbol == (char)Tiles.Platform);

            Vector2f position = new Vector2f(x, y);
            switch (newSymbol)
            {
                case (char)Tiles.Empty:
                    break;
                case (char)Tiles.PlayerSpawn:
                    game.Player.RespawnPosition = position;
                    break;
                case (char)Tiles.Platform:
                    if (wasPlatform)
                    {
                        wasPlatform = false;
                    }
                    else
                    {
                        StationaryObject platform = new StationaryObject((byte)random.Next(4), position);
                        game.Platforms.Add(platform);
                    }
                    break;

                case (char)Tiles.EnemyWalker:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnWalker));
                    break;
                case (char)Tiles.EnemyFlyer:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnFlyer));
                    break;
                case (char)Tiles.EnemySnake:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnSnake));
                    break;
                case (char)Tiles.EnemyArtillery:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnArtillery));
                    break;
                case (char)Tiles.EnemyShotgunFly:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnShotgunFly));
                    break;
                case (char)Tiles.EnemyBomber:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnBomber));
                    break;
                case (char)Tiles.EnemySpawner:
                    toSpawn.Add(new SpawnEffect(position, EnemySpawnClass.SpawnSpawner));
                    break;

                default:
                    throw new Exception("Unknown Symbol");
            }

            if (wasPlatform)
            {
                toRemove.Add(position);
            }
        }

        /// <summary>
        /// applies the shaking effect to platforms before removing them
        /// </summary>
        void MarkPlatforms(GameObjectHandler game, List<Vector2f> toRemove)
        {
            int i = 0;
            int j = 0;
            removingIndeces = new List<int>();
            shakingTiles = new List<ShakeEffect>();

            while (j < toRemove.Count)
            {
                if (game.Platforms[i].HitBox.Center == toRemove[j])
                {
                    removingIndeces.Add(i);
                    ShakeEffect eff = new ShakeEffect(game.Platforms[i]);
                    game.Platforms[i].Color = Color.Transparent;
                    game.Effects.Add(eff);
                    shakingTiles.Add(eff);

                    j++;
                }
                else
                {
                    i = (i + 1) % game.Platforms.Count;
                }
            }
        }
    }
}
