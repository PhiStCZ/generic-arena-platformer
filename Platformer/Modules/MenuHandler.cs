/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using GAP.Menu;
using GAP.Model;
using GAP.Utility;

namespace GAP.Modules
{
    /// <summary>
    /// Handles the displaying of and interaction with the game menu
    /// </summary>
    class MenuHandler : IModule
    {
        public readonly PlatformerFrame Game;

        public const Keyboard.Key Up = Keyboard.Key.Up;
        bool upWasPressed = true;
        public const Keyboard.Key Down = Keyboard.Key.Down;
        bool downWasPressed = true;
        public const Keyboard.Key Confirm = Keyboard.Key.Enter;
        bool confirmWasPressed = true;

        const float maxConfigTimeout = 0.125f;
        float configTimeout = 0f;

        public Stack<IMenuSlide> SlideStack; // Previous slides are kept loaded and the topmost is the recent

        /// <summary>
        /// Returns current slide
        /// </summary>
        public IMenuSlide GetCurrentSlide()
        {
            return SlideStack.Peek();
        }

        /// <summary>
        /// Returns to the previous slide
        /// </summary>
        public void DeleteCurrentSlide()
        {
            SlideStack.Pop();
        }

        /// <summary>
        /// Inserts a slide, which from then on will
        /// be looked at as the current slide
        /// </summary>
        public void InsertSlideAsCurrent(IMenuSlide slide)
        {
            SlideStack.Push(slide);
        }

        public MenuHandler(PlatformerFrame game)
        {
            Game = game;
        }

        public delegate void KeyAction();
        public void Update(GameTime time)
        {
            static void KeyFunction(Keyboard.Key key, ref bool keyWasPressed, KeyAction action)
            {
                if (Keyboard.IsKeyPressed(key))
                {
                    if (!keyWasPressed)
                    {
                        keyWasPressed = true;
                        action();
                    }
                }
                else
                {
                    keyWasPressed = false;
                }
            }

            if (GetCurrentSlide() is ConfigSlide slide)
            {
                if (slide.ConfigMode)
                {
                    configTimeout = maxConfigTimeout;
                }
            }
            if (configTimeout <= 0)
            {
                KeyFunction(Up, ref upWasPressed, GetCurrentSlide().OnUp);
                KeyFunction(Down, ref downWasPressed, GetCurrentSlide().OnDown);
                KeyFunction(Confirm, ref confirmWasPressed, GetCurrentSlide().OnConfirm);
            }
            configTimeout -= time.DeltaTime;
        }

        public void Load()
        {
            SlideStack = new Stack<IMenuSlide>();
            SlideStack.Push(new MainSlide(this));
        }

        public void Draw(RenderWindow window)
        {
            GetCurrentSlide().Draw(window);
        }
    }
}
