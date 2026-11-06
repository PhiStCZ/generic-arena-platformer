/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GAP.Utility;
using GAP.Modules;

namespace GAP.Model
{

    /// <summary>
    /// The event-driven core of the game.
    /// </summary>
    abstract class GameFrame
    {
        public readonly GameConstants Constants;
        public readonly GameTime GameTime;
        public readonly RenderWindow GameWindow;
        public IModule[] ModuleList;

        public GameFrame(uint width, uint height, string title, int FPS)
        {
            this.Constants = new GameConstants(FPS, width, height);
            this.GameWindow = new RenderWindow(new VideoMode(width, height), title);
            this.GameTime = new GameTime();
        }

        /// <summary>
        /// Main method of the class. Needs to be called only once.
        /// </summary>
        public void Run()
        {
            Load();

            float lastUpdateTime = 0;
            Clock clock = new Clock();

            while (GameWindow.IsOpen)
            {
                GameWindow.DispatchEvents();

                float currentTime = clock.ElapsedTime.AsSeconds();
                if (currentTime >= lastUpdateTime + Constants.TargetFrameTime)
                {
                    lastUpdateTime = currentTime;
                    GameTime.Update(currentTime);
                    Update();
                    Draw();
                    GameWindow.Display();
                }
            }

        }

        /// <summary>
        /// Method for initialising components
        /// </summary>
        void Load()
        {
            CustomLoad();
            ModuleLoad();
        }
        /// <summary>
        /// Method for updating events
        /// </summary>
        void Update()
        {
            CustomUpdate();
            ModuleUpdate();
        }
        /// <summary>
        /// Method for drawing the result on the screen
        /// </summary>
        void Draw()
        {
            this.GameWindow.Clear();

            ModuleDraw();
            CustomDraw();
        }

        void ModuleLoad()
        {
            foreach (IModule module in ModuleList)
            {
                if (module != null)
                {
                    module.Load();
                }
            }
        }
        void ModuleUpdate()
        {
            foreach (IModule module in ModuleList)
            {
                if (module != null)
                {
                    module.Update(this.GameTime);
                }
            }
        }
        void ModuleDraw()
        {
            foreach (IModule module in ModuleList)
            {
                if (module != null)
                {
                    module.Draw(GameWindow);
                }
            }
        }

        /// <summary>
        /// Method for initialising components
        /// </summary>
        protected abstract void CustomLoad();
        /// <summary>
        /// Method for updating events
        /// </summary>
        protected abstract void CustomUpdate();
        /// <summary>
        /// Method for drawing the result on the screen
        /// </summary>
        protected abstract void CustomDraw();
    }
}
