using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

/*
 *  19th September 2012:  Added TracePath stuff - TF
  * 
 */

namespace FuzzySim.Rendering
{
    /// <summary>
    /// The frame which depicts an itteration of a Simulation Run
    /// </summary>
    public class SimFrame : Frame
    {
        public List<PrintText> TracePath;  

        /// <summary>
        /// Default Constructor - Set Background to balck, initialize Renderables
        /// </summary>
        public SimFrame()
        {
            BackGround = Color.Black;
            Renderables = new List<Drawable>();

            TracePath = new List<PrintText>();
        }

        /// <summary>
        /// Adds a line of text to the frame
        /// </summary>
        /// <param name="text">Text to add</param>
        /// <param name="pos">Position of text</param>
        /// <param name="col">Text Colour</param>
        public override void AddText(string text, Vec2 pos, Brush col)
        {
            Renderables.Add(new PrintText(text, pos, col));
        }


        public void AddText(string id, string text, Vec2 pos, Brush col)
        {
            Renderables.Add(new PrintText(text, pos, col) { Id = id } );
        }


        public PrintText PlotLocation(Vec2 location, Color col)
        {
            return new PrintText(Globals.TurnCount.ToString(), location, new SolidBrush(col));
        }

        /// <summary>
        /// Adds Text with a shodow behind-down-right of the text location 
        /// </summary>
        /// <param name="id">object ID</param>
        /// <param name="text">print text</param>
        /// <param name="pos">position</param>
        /// <param name="col">text colour</param>
        /// <param name="shadow">shadow colour</param>
        public void AddTextWithShadow(string id, string text, Vec2 pos, Brush col, Brush shadow)
        {
            Renderables.Add(new PrintText(text, new Vec2(pos.X + 1, pos.Y + 1), shadow) { Id = id+"_shadow" });
            Renderables.Add(new PrintText(text, pos, col) { Id = id });
        }

        /// <summary>
        /// Adds Text with a shodow behind-down-right of the text location 
        /// </summary>
        /// <param name="id">object ID</param>
        /// <param name="text">print text</param>
        /// <param name="pos">position</param>
        /// <param name="size">font size</param>
        /// <param name="col">text colour</param>
        /// <param name="shadow">shadow colour</param>
        public void AddTextWithShadow(string id, string text, Vec2 pos, int size, Brush col, Brush shadow)
        {
            Renderables.Add(new PrintText(text, new Vec2(pos.X + 1, pos.Y + 1), shadow, size) { Id = id + "_shadow" });
            Renderables.Add(new PrintText(text, pos, col, size) { Id = id });
        }


        /// <summary>
        /// Adds a line of text to the frame
        /// </summary>
        /// <param name="text">Text to add</param>
        /// <param name="pos">Position of text</param>
        /// <param name="col">Text Colour</param>
        /// <param name="size">Text Size</param>
        public override void AddText(string text, Vec2 pos, Brush col, int size)
        {
            Renderables.Add(new PrintText(text, pos, col, size));
        }

        /// <summary>
        /// Adds a Drawable object to the Frame
        /// </summary>
        /// <param name="obj"></param>
        public void AddRenderable(Drawable obj)
        {
            Renderables.Add(obj);
        }

        /// <summary>
        /// Renders the current frame (Text and Renderables)
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {/*
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.SmoothingMode = SmoothingMode.HighQuality;*/

            g.InterpolationMode = InterpolationMode.Low;

            g.SmoothingMode = SmoothingMode.HighSpeed;

            PrintText p;

            if(Globals.DrawTracePath)
                foreach (PrintText text in TracePath)
                {
                    text.OffSet().Render(g);
                    text.Render(g);
                }

            ///Draw stuff
            foreach (Drawable obj in Renderables)
                obj.Render(g);

            //Slap a 'FrameID' in the lower righthand corner
            p = new PrintText("Turns: " + Globals.TurnCount, new Vec2(Globals.Simulator.FrameResolution.X - 90, Globals.Simulator.FrameResolution.Y - 20), Brushes.Black, 8);
            p.Render(g);
            
            p = new PrintText("Turns: " + Globals.TurnCount, new Vec2(Globals.Simulator.FrameResolution.X - 89, Globals.Simulator.FrameResolution.Y - 19), Brushes.White, 8);
            p.Render(g);
        }

        // 15/09/2012
   
        /// <summary>
        /// Returns the first instace of a Drawable with matching ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Drawable Find(string id)
        {
            try
            {
                return Renderables.Find(x => x.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Could not find " + id + "in this frame.");
            }
            
        }


        /// <summary>
        /// Removes the first instance of Drawable with matching Id
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            Renderables.Remove(Find(id));
        }


        /// <summary>
        /// Changes the image of an existing Sprite
        /// </summary>
        /// <param name="id"></param>
        /// <param name="im"></param>
        public void ChangeImage(string id, Image im)
        {
            object o = Find(id);
            
            //make sure we can
            if(o.GetType() != typeof(Sprite))
                throw new Exception("Cannot change the image of a Drawable that is not a Sprite.");

            Remove(id);

            //ok, lets go
            ((Sprite) o).Picture = im;

            AddRenderable((Sprite)o);
        }


        /// <summary>
        /// Changes the text of a given PrintText object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public void ChangeText(string id, string text)
        {
            object o = Find(id);

            //make sure we can
            if (o.GetType() != typeof(PrintText))
                throw new Exception("Cannot change the text of a Drawable that is not a PrintText.");

            Remove(id);

            //ok, lets go

            ((PrintText)o).Text = text;


            AddRenderable((PrintText)o);
        }

    }

}
