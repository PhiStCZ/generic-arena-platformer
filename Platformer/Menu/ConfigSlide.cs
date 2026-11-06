/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.System;
using SFML.Graphics;
using SFML.Window;
using GAP.Utility;
using GAP.Modules;

namespace GAP.Menu
{
    /// <summary>
    /// A slide for configuring keybinds
    /// </summary>
    class ConfigSlide : IMenuSlide
    {
        readonly Font mainFont = new Font("./RESOURCES/Debug/IBMPlexMono-Light.ttf");
        readonly Vector2f ScreenCenter;

        public bool ConfigMode = false;

        readonly Text titleText;
        readonly Text promptText;

        readonly MenuHandler menu;
        readonly PlayerControls controls;
        readonly SelectKey[] selectKeys;
        PlayerControls.KeyData[] keys;
        int arrowPointer = 0;

        const int fontSize = 25;
        const int fontDistancing = 50;

        readonly Shape selectArrow = MenuConstants.SelectArrow;
        void UpdateArrowPosition()
        {
            Text currentText = selectKeys[arrowPointer].DisplayText;
            selectArrow.Position = new Vector2f(currentText.Position.X - 64, currentText.Position.Y);
        }

        public ConfigSlide(MenuHandler menu)
        {
            this.menu = menu;
            this.ScreenCenter = menu.Game.Constants.ScreenDimensions / 2;
            this.controls = menu.Game.Controls;
            this.keys = controls.AllKeys;

            titleText = new Text()
            {
                Font = mainFont,
                CharacterSize = 30,
                Position = new Vector2f(ScreenCenter.X, 20),
                FillColor = Color.White,
                DisplayedString = "ACTION : KEY"
            };
            GameConstants.CenterText(titleText);

            promptText = new Text()
            {
                Position = new Vector2f(512, 570),
                DisplayedString = $"Press {MenuHandler.Up} and {MenuHandler.Down} to select a key, and {MenuHandler.Confirm} to configure it",
                CharacterSize = 12,
                FillColor = Color.White,
                Font = mainFont
            };
            GameConstants.CenterText(promptText);

            selectKeys = new SelectKey[keys.Length + 2];

            int index;
            Vector2f basePosition = new Vector2f(ScreenCenter.X, ScreenCenter.Y - 210);
            for (index = 0; index < keys.Length; index++)
            {
                selectKeys[index] = new SelectKey(keys[index].Name + " : ", menu.Game.Constants.ScreenDimensions.X / 2)
                {
                    DisplayText = ReturnStandardText(index)
                };
                selectKeys[index].UpdateText(keys[index].Key.ToString());
            }

            index = selectKeys.Length - 2;
            selectKeys[index] = new SelectKey("Restore defaults", menu.Game.Constants.ScreenDimensions.X / 2)
            {
                DisplayText = ReturnStandardText(index)
            };
            selectKeys[index].UpdateText("");

            index = selectKeys.Length - 1;
            selectKeys[index] = new SelectKey("Save & exit", menu.Game.Constants.ScreenDimensions.X / 2)
            {
                DisplayText = ReturnStandardText(index)
            };
            selectKeys[index].UpdateText("");

            UpdateArrowPosition();

            Text ReturnStandardText(int i)
            {
                return new Text()
                {
                    Font = mainFont,
                    CharacterSize = fontSize,
                    Position = new Vector2f(basePosition.X, basePosition.Y + (fontDistancing * i)),
                    FillColor = Color.White
                };
            }
        }

        /// <summary>
        /// Called only when waiting for a new key to be pressed to overwrite current configuration
        /// and a KeyDown event is received (it will change the current key to the pressed one, or
        /// cancel the configuration if the pressed key was 'escape')
        /// </summary>
        public void HandleKeyChange(Keyboard.Key key)
        {
            if (key != Keyboard.Key.Escape)
            {
                keys[arrowPointer].Key = key;
            }
            selectKeys[arrowPointer].UpdateText(keys[arrowPointer].Key.ToString());
            UpdateArrowPosition();
        }

        public void OnConfirm()
        {
            if (arrowPointer == selectKeys.Length - 2)
            {
                controls.Reset();
                keys = controls.AllKeys;
                for (int i = 0; i < keys.Length; i++)
                {
                    selectKeys[i].UpdateText(keys[i].Key.ToString());
                }
            }
            else if (arrowPointer == selectKeys.Length - 1)
            {
                menu.Game.Controls.SaveCurrentConfiguration();
                menu.DeleteCurrentSlide();
                menu.GetCurrentSlide().OnUp();
                menu.GetCurrentSlide().OnDown();
            }
            else
            {
                selectKeys[arrowPointer].UpdateText("SELECT KEY");
                ConfigMode = true;
                UpdateArrowPosition();
            }
        }

        public void OnDown()
        {
            arrowPointer = (arrowPointer + 1) % selectKeys.Length;
            UpdateArrowPosition();
        }

        public void OnUp()
        {
            arrowPointer = (arrowPointer + selectKeys.Length - 1) % selectKeys.Length;
            UpdateArrowPosition();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(titleText);
            window.Draw(promptText);
            window.Draw(selectArrow);
            foreach (SelectKey key in selectKeys)
            {
                window.Draw(key.DisplayText);
            }
        }

        /// <summary>
        /// A class to connect the representation of a key
        /// and the corresponding text on the screen
        /// </summary>
        class SelectKey
        {
            public string DisplayString;
            public Text DisplayText;
            public readonly float defaultHorizontalPosition;

            public SelectKey(string display, float defaultHorizontalPosition)
            {
                this.DisplayString = display;
                this.defaultHorizontalPosition = defaultHorizontalPosition;
            }

            public void UpdateText(string customString)
            {
                DisplayText.DisplayedString = DisplayString + customString;

                DisplayText.Position = new Vector2f(defaultHorizontalPosition, DisplayText.Position.Y);
                GameConstants.CenterText(DisplayText);
            }
        }
    }
}
