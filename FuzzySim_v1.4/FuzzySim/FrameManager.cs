namespace FuzzySim
{
    using System.Collections.Generic;
    using Rendering;
    using Core;

    /*
     *      THIS IS A PROTOTYPE OF A FRAMEMANAGER. 
     *              Unlikely to be released. 
     * 
     *  CONCEPT:
     * 
     *      Want to have this class managing the SimFrames and FuzzySetFrames produced 
     *  by the Simulator... Capture and enque them, so that they can be played
     *  back later on. 
     *      In throry, would also allow for faster-than-framerate Simulation Instance 
     *  processing; ie, Fill the buffer as fast as possible, then draw the frames at the 
     *  set rate.
     *              --TF, September 2012
     */

    class FrameManager
    {

        private Queue<SimFrame> _simFrameBuffer;
        private Queue<List<FuzzyCollection>> _fuzzyFrameBuffer;

        public SimFrame CurrentFrame
        {
            get { return _simFrameBuffer.Peek(); } //'set' by call to NextFrame()
        }


        public FrameManager()
        {
            _simFrameBuffer = new Queue<SimFrame>(Globals.FrameBufferDepth);
            _fuzzyFrameBuffer = new Queue<List<FuzzyCollection>>(Globals.FrameBufferDepth);
        }


        /// <summary>
        /// Enqueues a Simulation Frame to the buffer
        /// </summary>
        public void NextFrame()
        {
            //Globals.Simulator.DoTurn(); ??? 

            _simFrameBuffer.Enqueue(Globals.Simulator.DrawTurn(Globals.Simulator.FrameResolution));
        }

        //Should FrameManager also manage the 'DoTurn'? 

    }
}
 