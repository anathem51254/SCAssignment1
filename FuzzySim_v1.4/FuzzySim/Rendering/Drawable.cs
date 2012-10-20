using System.Drawing;

namespace FuzzySim.Rendering
{

    /// <summary>
    /// Template Class for objects that are Drawable onto Frames
    /// </summary>
    public abstract class Drawable
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// object world position 
        /// </summary>
        public Vec2 Position { get; set; }
        /// <summary>
        /// Object's velocity
        /// </summary>
        public Vec2 Velocity { get; set; }

        /// <summary>
        /// renders the object to the Graphics given
        /// </summary>
        /// <param name="g">Graphics to draw to</param>
        public abstract void Render(Graphics g);
    }

    
  
}
