/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using SFML.System;
using SFML.Graphics;
using GAP.Utility;
using GAP.Modules;

namespace GAP.Menu
{
    /// <summary>
    /// The First slide to display after starting the program
    /// </summary>
    class MainSlide : IMenuSlide
    {
        readonly MenuHandler menu;

        readonly Font mainFont = new Font("./RESOURCES/Debug/IBMPlexMono-Light.ttf");

        readonly Shape selectArrow = MenuConstants.SelectArrow;
        int arrowPointer = 0;
        void UpdateArrowPosition()
        {
            Text currentText = selectFunctions[arrowPointer].DisplayText;
            selectArrow.Position = new Vector2f(currentText.Position.X - 64, currentText.Position.Y);
        }

        readonly SelectFunction[] selectFunctions;
        readonly Vector2f screenCenter;

        const int fontSize = 30;
        const int fontDistancing = 60;

        readonly Text titleText;
        readonly Text promptText;

        // FUNCTIONS FOR SELECTFUNCTIONS   --- ---

        /// <summary>
        /// Called when the 'Display Help' action is selected and confirmed
        /// </summary>
        void DisplayHelp()
        {
            HelpSlide slide = new HelpSlide(menu);
            menu.InsertSlideAsCurrent(slide);
        }

        /// <summary>
        /// Called when the 'Configure keybinds' action is selected and confirmed
        /// </summary>
        void DisplayConfig()
        {
            ConfigSlide slide = new ConfigSlide(menu);
            menu.InsertSlideAsCurrent(slide);
        }

        // --- --- --- --- --- --- --- --- --- ---

        public MainSlide(MenuHandler menu)
        {
            this.menu = menu;
            this.screenCenter = menu.Game.Constants.ScreenDimensions / 2;

            selectFunctions = new SelectFunction[]
            {
                new SelectFunction("PLAY", menu.Game.ToBattle),
                new SelectFunction("HOW TO PLAY", DisplayHelp),
                new SelectFunction("CONFIGURE KEYBINDS", DisplayConfig),
                new SelectFunction("EXIT", menu.Game.GameWindow.Close)
            };

            titleText = new Text()
            {
                Font = mainFont,
                CharacterSize = 80,
                Position = new Vector2f(screenCenter.X, 40),
                FillColor = Color.White,
                DisplayedString = " G.A.P."
            };
            GameConstants.CenterText(titleText);

            promptText = new Text()
            {
                Position = new Vector2f(512, 570),
                DisplayedString = $"Press {MenuHandler.Up} and {MenuHandler.Down} to select an action, and {MenuHandler.Confirm} to confirm selection",
                CharacterSize = 12,
                FillColor = Color.White,
                Font = mainFont
            };
            GameConstants.CenterText(promptText);

            Vector2f basePosition = new Vector2f(screenCenter.X, screenCenter.Y - 64);
            for (int i = 0; i < selectFunctions.Length; i++)
            {
                selectFunctions[i].DisplayText = new Text()
                {
                    Font = mainFont,
                    CharacterSize = fontSize,
                    Position = new Vector2f(basePosition.X, basePosition.Y + (fontDistancing * i)),
                    FillColor = Color.White,
                    DisplayedString = selectFunctions[i].DisplayString
                };
                GameConstants.CenterText(selectFunctions[i].DisplayText);
            }

            UpdateArrowPosition();
        }


        public void OnConfirm()
        {
            selectFunctions[arrowPointer].Action();
        }

        public void OnDown()
        {
            arrowPointer = (arrowPointer + 1) % selectFunctions.Length;
            UpdateArrowPosition();
        }

        public void OnUp()
        {
            arrowPointer = (arrowPointer + selectFunctions.Length - 1) % selectFunctions.Length;
            UpdateArrowPosition();
        }

        /// <summary>
        /// A class to connect the representation of an action
        /// and the corresponding text on the screen
        /// </summary>
        class SelectFunction
        {
            public string DisplayString;
            public Text DisplayText; // will be initialised automatically

            public delegate void OnActivate();
            public OnActivate Action;

            public SelectFunction(string display, OnActivate action)
            {
                this.DisplayString = display;
                this.Action = action;
            }
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(titleText);
            window.Draw(selectArrow);
            window.Draw(promptText);
            foreach (SelectFunction f in selectFunctions)
            {
                window.Draw(f.DisplayText);
            }
        }
    }
}
