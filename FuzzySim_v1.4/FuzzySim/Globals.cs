using FuzzySim.Core;
using FuzzySim.Simulators;

namespace FuzzySim
{
    /// <summary>
    /// The Globals.
    /// </summary>
    public static class Globals
    {

        public static int SimRate=10; // turns per second
        public static int DrawEvery = 1; // 1 = draw every frame 10=draw every 10th turn
        public static int TurnCount = 0;

        /// <summary>
        /// Toggles the rendering of any TracePath information in SimFrames (if applicable)
        /// </summary>
        public static bool DrawTracePath;

        /// <summary>
        /// Toggles Debug information being drawn (if applicable)
        /// </summary>
        public static bool DrawDebug;

        /// <summary>
        /// Selected Simulator in use
        /// </summary>
        public static AISimulator Simulator;

        /// <summary>
        /// The FuzzySets to use for the Simulation
        /// </summary>
        public static AIController Controller;

        /// <summary>
        /// The Enumerable used to determine which Simulator has been selected
        /// </summary>
        public static SimulatorsEnum SelectedSimulator;


        public static int FrameBufferDepth = 50;

        /// <summary>
        /// Resets the Globals object (for reinitialising the Simulator)
        /// </summary>
        public static void Reset()
        {
            Simulator = null;
            Controller = null;

            DrawTracePath = true;
            DrawDebug = true;

            TurnCount = 0;
        }
    }
}
