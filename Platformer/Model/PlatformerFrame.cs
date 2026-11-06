/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System;
using SFML.Window;
using GAP.Menu;
using GAP.Modules;

namespace GAP.Model
{
    /// <summary>
    /// A more concrete model of the game.
    /// </summary>
    class PlatformerFrame : GameFrame
    {
        public PlatformerFrame() : base(1024, 608, "G.A.P.", 60) { }

        public void WindowClose(object sender, EventArgs e)
        {
            GameWindow.Close();
        }

        public PlayerControls Controls;

        protected override void CustomLoad()
        {
            GameWindow.Closed += WindowClose;
            GameWindow.KeyPressed += KeyConfigHandler;
            GameWindow.KeyPressed += EscapeEvent;
            Controls = new PlayerControls();
            Controls.Load();

            ToMenu();
        }

        public void KeyConfigHandler(object sender, KeyEventArgs e)
        {
            if (ModuleList[0] is MenuHandler handler)
            {
                IMenuSlide slide = handler.GetCurrentSlide();
                if (slide is ConfigSlide slide1 && slide1.ConfigMode)
                {
                    slide1.ConfigMode = false;
                    slide1.HandleKeyChange(e.Code);
                }
            }
        }

        /// <summary>
        /// Returns the program to menu
        /// </summary>
        public void ToMenu()
        {
            MenuHandler menu = new MenuHandler(this);
            menu.Load();

            ModuleList = new IModule[] { menu };
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void ToBattle()
        {
            GameObjects.Player.PlayerData pData = new GameObjects.Player.PlayerData();
            GameObjectHandler gameObjects = new GameObjectHandler(Controls, pData);
            TextUserInterface UI = new TextUserInterface(gameObjects, Controls, this);

            gameObjects.Load();
            UI.Load();

            ModuleList = new IModule[] { gameObjects, UI };
        }

        readonly Keyboard.Key escapeKey = Keyboard.Key.Escape;
        public void EscapeEvent(object sender, KeyEventArgs e)
        {
            if (e.Code == escapeKey)
            {
                if (doublePressCounter > 0)
                {
                    EscapeBack();
                }
                else
                {
                    doublePressCounter = MAX_DOUBLE_PRESS_TIME;
                }
            }
        }

        const float MAX_DOUBLE_PRESS_TIME = 0.6f;
        float doublePressCounter = 0f;
        protected override void CustomUpdate()
        {
            Controls.Update(GameTime);

            doublePressCounter -= GameTime.DeltaTime;
        }

        protected override void CustomDraw() { }

        void EscapeBack()
        {
            if (ModuleList[0] is GameObjectHandler)
            {
                ToMenu();
            }
            else
            {
                GameWindow.Close();
            }
        }
    }
}
