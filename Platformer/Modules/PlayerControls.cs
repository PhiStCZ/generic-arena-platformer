/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

using System.IO;
using SFML.Graphics;
using SFML.Window;
using GAP.Utility;

namespace GAP.Modules
{
    /// <summary>
    /// Manages ingame player input
    /// </summary>
    class PlayerControls : IModule
    {
        const byte coyoteTimeFrameCount = 8; // ~= 0.125 seconds

        /// <summary>
        /// Class for managing data of one key
        /// </summary>
        public class KeyData
        {
            public Keyboard.Key Key;
            public string Name;

            bool pressStatus;
            byte wholePressStatus;
            bool keyWasPressedLastFrame;

            /// <summary>
            /// True if the button is being held down
            /// </summary>
            public bool IsPressed => pressStatus;

            /// <summary>
            /// True if the button has been (very) recently pressed
            /// </summary>
            public bool WholePress => wholePressStatus > 0;

            /// <summary>
            /// True only on the first frame of the button press
            /// </summary>
            public bool FirstFramePress
                => wholePressStatus == coyoteTimeFrameCount;

            public KeyData(Keyboard.Key key, string name)
            {
                Key = key;
                Name = name;

                keyWasPressedLastFrame = false;
                wholePressStatus = 0;
                pressStatus = false;
            }

            public void Update()
            {
                wholePressStatus = (wholePressStatus == 0)
                    ? (byte)0
                    : (byte)(wholePressStatus - 1);

                if (Keyboard.IsKeyPressed(this.Key))
                {
                    if (!keyWasPressedLastFrame)
                    {
                        keyWasPressedLastFrame = true;
                        wholePressStatus = coyoteTimeFrameCount;
                    }

                    pressStatus = true;
                }
                else
                {
                    wholePressStatus = (wholePressStatus == 0) ? (byte)0 : (byte)(wholePressStatus - 1);

                    pressStatus = false;
                    keyWasPressedLastFrame = false;
                }
            }
        }

        public KeyData Left;
        public KeyData Right;
        public KeyData Down;

        public KeyData Dash;
        public KeyData Jump;
        public KeyData Shoot;

        public KeyData DebugData;

        public KeyData[] AllKeys;

        public PlayerControls()
        {
            Reset();
        }

        public void Draw(RenderWindow window) { /* empty */ }

        public void Load()
        {
            LoadConfiguration();
        }

        public void Update(GameTime time)
        {
            foreach (KeyData key in AllKeys)
            {
                key.Update();
            }
        }

        readonly string pathToSave = "./RESOURCES/SavedData/KeyBindings.txt";

        public void LoadConfiguration()
        {
            if (File.Exists(pathToSave))
            {
                using StreamReader reader = new StreamReader(pathToSave);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string name = line.Split()[0];
                    Keyboard.Key key = (Keyboard.Key)(int.Parse(line.Split()[1]));

                    foreach (KeyData data in AllKeys)
                    {
                        if (data.Name == name)
                        {
                            data.Key = key;
                            break;
                        }
                    }
                }
            }
        }

        public void SaveCurrentConfiguration()
        {
            using StreamWriter writer = new StreamWriter(pathToSave);
            foreach (KeyData data in AllKeys)
            {
                writer.WriteLine(data.Name + " " + (int)(data.Key));
            }
        }

        public void Reset()
        {
            Left = new KeyData(Keyboard.Key.Left, "Left");
            Right = new KeyData(Keyboard.Key.Right, "Right");
            Down = new KeyData(Keyboard.Key.Down, "Down");

            Dash = new KeyData(Keyboard.Key.W, "Dash");
            Jump = new KeyData(Keyboard.Key.S, "Jump");
            Shoot = new KeyData(Keyboard.Key.D, "Shoot");

            DebugData = new KeyData(Keyboard.Key.F1, "DebugData");

            AllKeys = new KeyData[]
            {
                this.Left,
                this.Right,
                this.Down,
                this.Jump,
                this.Dash,
                this.Shoot,
                this.DebugData,
            };
        }
    }
}
