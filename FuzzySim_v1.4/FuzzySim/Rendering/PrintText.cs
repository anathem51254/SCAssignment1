using System.Drawing;

namespace FuzzySim.Rendering
{
    /// <summary>
    /// The Drawable Text class - renders Text to a Frame
    /// </summary>
    public class PrintText : Drawable
    {
        /// <summary>
        /// The Text to write
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The colour of the text
        /// </summary>
        public Brush Colour { get; set; }
        /// <summary>
        /// The Text's Font
        /// </summary>
        public Font TextFont { get; set; }

        /// <summary>
        /// Creates a new PrintText object
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="pos">Position of Text</param>
        /// <param name="col">Text Colour</param>
        public PrintText(string text, Vec2 pos, Brush col)
        {
            TextFont = new Font("Arial", 10);
            Colour = col;

            Text = text;
            Position = pos;
        }

        /// <summary>
        /// Creates a new PrintText object
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="pos">Position of Text</param>
        /// <param name="col">Text Colour</param>
        /// <param name="size">Text Size</param>
        public PrintText(string text, Vec2 pos, Brush col, int size)
        {
            TextFont = new Font("Arial", size);
            Colour = col;

            Text = text;
            Position = pos;
        }

        public PrintText OffSet()
        {
            PrintText ret = new PrintText(this.Text, this.Position, new SolidBrush(Color.Wheat));

            ret.Position = new Vec2(this.Position.X + 1, this.Position.Y + 1);

            return ret; 
        }

        /// <summary>
        /// Draws the Text onto the Graphics Object
        /// </summary>
        /// <param name="g"></param>
        public override void Render(Graphics g)
        {
            g.DrawString(Text, TextFont, Colour, (float)Position.X, (float)Position.Y);
        }

        /// <summary>
        /// Returns the Text being written by the PrintText object as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Text;
        }
    }
}
