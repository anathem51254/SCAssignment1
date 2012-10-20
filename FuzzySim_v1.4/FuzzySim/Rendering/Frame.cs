using System.Collections.Generic;
using System.Drawing;

/*
 *              FRAME 
 * 
 *  Classes which produce a drawable Simulation 'frame' for rendering.
 *  
 *  Written by Timothy Fox, July - August 2012. 
 *  Version 1.0
 */

namespace FuzzySim.Rendering
{
    /// <summary>
    /// The Outline for the Frame class (Abstract)
    /// </summary>
    public abstract class Frame
    {
        public int FrameId; //COunt of Frame
        public bool ToBeDrawn; // flag to allow it to render
        public bool Error;

        // if no error and toBeDrawn is true then the following will be drawn
        public List<Drawable> Renderables { get; set; }
     
        public Color BackGround; // the colour of the background

        public abstract void Draw(Graphics g); //Renders the Frame to a Graphics object 
        public abstract void AddText(string text, Vec2 pos, Brush col); //Adds Text to a frame
        public abstract void AddText(string text, Vec2 pos, Brush col, int size); //Adds Text to a Frame with a FontSize param


        public void DrawLine()
        {
            int x = 0;

        }
    }
}
