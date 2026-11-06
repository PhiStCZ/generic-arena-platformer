/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;
using GAP.Utility;

namespace GAP.Modules
{
    /// <summary>
    /// Shows important text data to the user
    /// </summary>
    class TextUserInterface : IModule
    {
        public TextUserInterface(GameObjectHandler game, PlayerControls controls, Model.PlatformerFrame platformer)
        {
            this.game = game;
            this.controls = controls;
            this.platformer = platformer;
            this.screenCenter = platformer.Constants.ScreenDimensions / 2;
            this.startTime = platformer.GameTime.TotalTime;
        }

        readonly Model.PlatformerFrame platformer;
        readonly PlayerControls controls;
        readonly GameObjectHandler game;
        readonly Vector2f screenCenter;
        readonly float startTime;

        bool debugActive = false;

        Text healthText;
        Text levelText;

        Text fpsText;
        Text timeText;
        Text stateText;
        Text objectText;

        Text gameOverText1;
        Text gameOverText2;
        Text gameOverText3;

        float gameOverCounter = 1.5f;
        bool displayingEndMessage = false;

        public void Load()
        {
            Font mainFont = new Font("./RESOURCES/Debug/IBMPlexMono-Light.ttf");


            healthText = new Text()
            {
                Font = mainFont,
                CharacterSize = 25,
                Position = new Vector2f(10, 5),
                FillColor = Color.White
            };
            levelText = new Text()
            {
                Font = mainFont,
                CharacterSize = 25,
                Position = new Vector2f(10, 40),
                FillColor = Color.White
            };


            fpsText = new Text()
            {
                Font = mainFont,
                CharacterSize = 10,
                Position = new Vector2f(155, 5),
                FillColor = Color.White
            };
            timeText = new Text()
            {
                Font = mainFont,
                CharacterSize = 10,
                Position = new Vector2f(155, 22),
                FillColor = Color.White
            };
            stateText = new Text()
            {
                Font = mainFont,
                CharacterSize = 10,
                Position = new Vector2f(155, 39),
                FillColor = Color.White
            };
            objectText = new Text()
            {
                Font = mainFont,
                CharacterSize = 10,
                Position = new Vector2f(155, 56),
                FillColor = Color.White
            };


            gameOverText1 = new Text()
            {
                Font = mainFont,
                CharacterSize = 50,
                Position = new Vector2f(screenCenter.X, 200),
                FillColor = Color.White
            };
            gameOverText2 = new Text()
            {
                Font = mainFont,
                CharacterSize = 15,
                Position = new Vector2f(screenCenter.X, 275),
                FillColor = Color.White
            };
            gameOverText3 = new Text()
            {
                Font = mainFont,
                CharacterSize = 15,
                Position = new Vector2f(screenCenter.X, 300),
                FillColor = Color.White
            };
        }

        public void Update(GameTime time)
        {
            healthText.DisplayedString = "HP : " + (game.Player.Data.Health.ToString("00"));
            levelText.DisplayedString = "Lvl: " + (game.MapLoader.LevelNumber.ToString("00"));

            fpsText.DisplayedString = "FPS : " + (1 / time.DeltaTime).ToString("0.000");
            timeText.DisplayedString = "Time: " + (time.TotalTime - this.startTime).ToString("0.000");

            stateText.DisplayedString = "Inv : " + (game.Player.Data.Invulnerable ? "Yes" : "No");
            objectText.DisplayedString = "objs: " + (
                game.Bullets.Count
                + game.Enemies.Count
                + game.Effects.Count
            );

            if (controls.DebugData.FirstFramePress)
            {
                debugActive = !debugActive;
            }


            if (game.State != GameObjectHandler.GameState.Playing)
            {
                if (gameOverCounter <= 0)
                {
                    if (!displayingEndMessage)
                    {
                        displayingEndMessage = true;
                        if (game.State == GameObjectHandler.GameState.Won)
                        {
                            gameOverText1.DisplayedString = "YOU WIN";
                            gameOverText2.DisplayedString = $"You survived through all {game.MapLoader.LevelNumber} levels.";
                        }
                        else
                        {
                            gameOverText1.DisplayedString = "GAME OVER";
                            gameOverText2.DisplayedString = $"You lasted {game.MapLoader.LevelNumber} levels.";
                        }

                        gameOverText3.DisplayedString = $"Press \'{controls.Shoot.Key}\' to return to main menu or \'{controls.Jump.Key}\' to start over";

                        GameConstants.CenterText(gameOverText1);
                        GameConstants.CenterText(gameOverText2);
                        GameConstants.CenterText(gameOverText3);
                    }

                    if (controls.Jump.IsPressed)
                    {
                        platformer.ToBattle();
                    }
                    else if (controls.Shoot.IsPressed)
                    {
                        platformer.ToMenu();
                    }
                }

                gameOverCounter -= time.DeltaTime;
            }
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(healthText);
            window.Draw(levelText);

            if (debugActive)
            {
                window.Draw(fpsText);
                window.Draw(timeText);
                window.Draw(objectText);
                window.Draw(stateText);
            }

            if (displayingEndMessage)
            {
                window.Draw(gameOverText1);
                window.Draw(gameOverText2);
                window.Draw(gameOverText3);
            }
        }
    }
}