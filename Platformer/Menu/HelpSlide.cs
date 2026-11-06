/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System.IO;
using SFML.System;
using SFML.Graphics;
using GAP.Modules;
using GAP.Utility;

namespace GAP.Menu
{
    /// <summary>
    /// A slide displaying some condensed help info
    /// </summary>
    class HelpSlide : IMenuSlide
    {
        readonly MenuHandler menu;

        readonly RectangleShape screenShot;

        readonly Font mainFont = new Font("./RESOURCES/Debug/IBMPlexMono-Light.ttf");
        readonly Text helpText;
        readonly Text promptText;

        public HelpSlide(MenuHandler menu)
        {
            this.menu = menu;

            using (StreamReader reader = new StreamReader("./RESOURCES/HowTo.txt"))
            {
                helpText = new Text
                {
                    Position = new Vector2f(50, 50),
                    DisplayedString = reader.ReadToEnd(),
                    CharacterSize = 14,
                    FillColor = Color.White,
                    Font = mainFont
                };
            }

            promptText = new Text
            {
                Position = new Vector2f(512, 570),
                DisplayedString = $"Press {MenuHandler.Confirm} to go back",
                CharacterSize = 12,
                FillColor = Color.White,
                Font = mainFont
            };

            GameConstants.CenterText(promptText);

            screenShot = new RectangleShape
            {
                Texture = new Texture(new Image("./RESOURCES/Texture/ScreenShot.png")),
                Size = new Vector2f(400, 500),
                FillColor = Color.White,
                Position = new Vector2f(580, 50)
            };
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(screenShot);
            window.Draw(helpText);
            window.Draw(promptText);
        }

        public void OnConfirm()
        {
            menu.DeleteCurrentSlide();
        }

        public void OnDown()
        {
        }

        public void OnUp()
        {
        }
    }
}
