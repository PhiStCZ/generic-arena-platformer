/// Project : Platformer
/// Author  : Filip Štrobl
///
/// LS-2020, 19bNPRG031x06

namespace GAP.Utility
{
    /// <summary>
    /// Main class used to measure time in a game
    /// </summary>
    class GameTime
    {
        public float DeltaTime = 0f;

        public float TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Calculates TotalTime and DeltaTime for the next frame
        /// </summary>
        public float Update(float timeFromStart)
        {
            DeltaTime = timeFromStart - TotalTime;
            TotalTime = timeFromStart;
            return DeltaTime;
        }
    }
}
