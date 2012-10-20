using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FuzzySim.Rendering
{

    /// <summary>
    /// The Drawable Sprite class - renders an image onto a Frame
    /// </summary>
    public class Sprite : Drawable
    {
        private Image _picture;

        /// <summary>
        /// The Image to render
        /// </summary>
        public Image Picture 
        { 
            get { return _picture; }
            set
            {
                _picture = value;

                if(_originalHeight == null)
                    _originalHeight = new Vec2(_picture.Width, _picture.Height);
            }
        }

    

        private Vec2 _originalHeight;

        /// <summary>
        /// The Scale of the image 
        /// </summary>
        public Vec2 Scale = new Vec2(1, 1);

        public Sprite(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Renders the Sprite to the given Graphics Object
        /// </summary>
        /// <param name="g">Graphics object to render to</param>
        public override void Render(Graphics g)
        {
            Vec2 noScale = new Vec2(1, 1);
            //if there is any scaling to do, then scale it
            if (Scale.X != noScale.X || Scale.Y != noScale.Y)
            {
                //Scale.X = 0.01; test to create error
                if (_originalHeight.X * Scale.X < 1) return;
                if (_originalHeight.Y * Scale.Y < 1) return;
                
                Picture = new Bitmap(Picture, new Size((int)(_originalHeight.X * Scale.X), (int)(_originalHeight.Y * Scale.Y)));

                Bitmap x = new Bitmap((int)(_originalHeight.X * Scale.X), (int)(_originalHeight.Y * Scale.Y), PixelFormat.Format16bppArgb1555);

            }

            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(Picture, (float)Position.X, (float)Position.Y);
        }

        public void MoveByVelocity()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }
    }

}
