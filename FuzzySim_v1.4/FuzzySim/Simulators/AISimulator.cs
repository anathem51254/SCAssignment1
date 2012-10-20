using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FuzzySim.Core;
using FuzzySim.Rendering;

namespace FuzzySim.Simulators
{
    /// <summary>
    /// The basic , abstract outline structure that Simulators for this applicaiton with inherit from
    /// </summary>
    public abstract class AISimulator
    {

        /// <summary>
        /// True if running
        /// </summary>
        public SimulatorStateEnum State = 0; // 0-Undefined 1-Initialised 2-running 3-Ended


        /// <summary>
        /// Holds TracePath information
        /// </summary>
        internal List<PrintText> TracePath; 

        /// <summary>
        /// The FuzzySets for the Simulator
        /// </summary>
        public AIController FuzzySets;

        /// <summary>
        /// The resolution of the 'Canvas' to render to
        /// </summary>
        public Vec2 FrameResolution { get; set; }

        /// <summary>
        /// The Scale of the Simulation-World:Canvas ratio
        /// </summary>
        protected Vec2 ScaleFactor { get; set; }
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected AISimulator()
        {
            State = 0;
        }

        public SimDifficultyEnum Difficulty = SimDifficultyEnum.Easy;

        /// <summary>
        /// Method marks a Successful outcome
        /// </summary>
        public abstract void Succeed();

        /// <summary>
        /// Method marks an Unsuccessfoul outcome
        /// </summary>
        public abstract void Fail();

        /// <summary>
        /// The initialization of the Simulator
        /// </summary>
        /// <param name="xrez">Canvas X size</param>
        /// <param name="yrez">Canvas Y size</param>
        /// <param name="dif">Difficulty of the simulator</param>
        /// <returns>The first frame to draw</returns>
        public abstract SimFrame Init(Vec2 rez); // initialise the param is screen in pixels
        
        /// <summary>
        /// Method which draws a Frame of Simulation
        /// </summary>
        /// <param name="xscale">Canvas X size</param>
        /// <param name="yscale">Canvas Y size</param>
        /// <returns>Drawable SimFrame object</returns>
        public abstract SimFrame DrawTurn(Vec2 scale); // 


        /// <summary>
        /// Draws a simulation instance using the previous frame
        /// </summary>
        /// <param name="frame">Previous frame</param>
        /// <returns>currentFrame</returns>
        public abstract SimFrame DrawTurn(SimFrame frame); // 

        /// <summary>
        /// Method which tells the Simulator to evaluate internals, and run Simulation for one step.
        /// </summary>
        public abstract void DoTurn(); // Some params? Nah...


        /// <summary>
        /// Adds the current TracePath to a Simframe
        /// </summary>
        /// <param name="ret"></param>
        internal void DrawTracePath(SimFrame ret)
        {
            if (TracePath != null)
                for (int i = 1; i < TracePath.Count; i++)
                {
                    ret.AddRenderable(new Line(TracePath[i - 1].Position, TracePath[i].Position, new SolidBrush(Color.MediumBlue), .5f));
                }
        }
    }

}
