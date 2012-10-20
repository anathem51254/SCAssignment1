

namespace FuzzySim.Rendering
{
    using System.Drawing;

    class Line : Drawable
    {
        /// The colour of the text
        /// </summary>
        public Brush Colour { get; set; }

        private Vec2 _endpoint;
        private float _thickness;

        public Line(Vec2 startPos, Vec2 endPos, Brush colour, float thickness)
        {
            Position = startPos;
            _endpoint = endPos;
            Colour = colour;
            _thickness = thickness;
        }

        public override void Render(Graphics g)
        {
            g.DrawLine(new Pen(Colour, _thickness), new PointF((float)Position.X, (float)Position.Y), new PointF((float)_endpoint.X, (float)_endpoint.Y));
        }
    }


}
