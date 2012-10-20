using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using CFLS;

namespace FuzzySim.Rendering
{

    /// <summary>
    /// This is the Frame which depicts one of more Fuzzy Sets
    /// </summary>
    public class FuzzySetFrame : Frame
    {
        /// <summary>
        /// The FuzzySets
        /// </summary>
        public List<FuzzySet> FuzzSets { get; set; }

        /// <summary>
        /// Default constructor - sets the FuzzySets to work with
        /// </summary>
        /// <param name="fuzz">FuzzySet collection to render</param>
        public FuzzySetFrame(List<FuzzySet> fuzz)
        {
            Error = false;
            Renderables = new List<Drawable>();
            FuzzSets = fuzz;
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
        /// Renders the FuzzySet collection to a Graphics object
        /// </summary>
        /// <param name="g">Graphics Object</param>
        public override void Draw(Graphics g)
        {
            BackGround = Color.White;

            float width, height;

            float gridHeight = 30;

            float yGridWidth = 20;

            width = (g.ClipBounds.Width - 2) - yGridWidth;
            height = g.ClipBounds.Height - gridHeight;

            g.FillRectangle(new SolidBrush(Color.White), g.ClipBounds.X, 0, g.ClipBounds.Width, g.ClipBounds.Y);

            float widthUnit = 0;

            ValidateFuzzyCollection();
            
            if (!Error)
                DrawGrid(g, width, gridHeight - 3, yGridWidth, (float)FuzzSets[0].GetLowRange());

            foreach (FuzzySet fuzz in FuzzSets)
            {
                if (null == fuzz) continue;

                float range = (float)(fuzz.GetHighRange() - fuzz.GetLowRange());

                float padding = g.VisibleClipBounds.X + yGridWidth;

                //Draw each line...
                for (int i = 0; i < (fuzz.GetNumPoints() - 1); i++)
                {
                    //DRAWLINE(colour, x1, y1, x2, y2)
                    float y1 = (1 - (float)fuzz.GetSetValue(i) * height) + height;
                    float y2 = (1 - (float)fuzz.GetSetValue(i + 1) * height) + height;

                    //need to turn x-span into dynamic (width)
                    widthUnit = (float)width / range;

                    float x1 = (padding + ((float)fuzz.GetWorldValue(i) - (float)fuzz.GetLowRange()) * widthUnit);
                    float x2 = (padding + ((float)fuzz.GetWorldValue(i + 1) - (float)fuzz.GetLowRange()) * widthUnit);

                    if (fuzz.LineColour == null) fuzz.LineColour = new SolidBrush(Color.Black);

                    g.DrawLine(new Pen(fuzz.LineColour), x1, y1 + 3, x2, y2 + 3); // Adding a little bit of margin to it...
                }
            }

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;

            foreach (PrintText pt in Renderables)
            {
                pt.Render(g);
            }


        }

        private void ValidateFuzzyCollection()
        {
            if (FuzzSets.Count < 1)
            {
                Error = true;
                //System.Windows.Forms.MessageBox.Show("One of the FuzzyCollections you are trying to render contains no FuzzySets!");
                return;
            }

            foreach(FuzzySet f in FuzzSets)
            {
                if (f.GetNumPoints() < 1)
                {
                    Error = true;
                    f.SetErr(8);
                }
                    
            }
        }


        /// <summary>
        /// Draws the Grid for the FuzzySet collection
        /// </summary>
        /// <param name="g">Graphics object to draw to</param>
        /// <param name="gWidth">graphics width</param>
        /// <param name="gHeight">graphics height</param>
        /// <param name="yGridWidth">width of the Y-axis</param>
        /// <param name="lowRange">Low-Range world value of set</param>
        /// 
        private void DrawGrid(Graphics g, float gWidth, float gHeight, float yGridWidth, float lowRange)
        {
            float startY = (g.VisibleClipBounds.Height - (gHeight - 2)) - 10;
            float width = gWidth;


            //DRAW THE X-AXIS GRID
            gHeight -= 10; //REDUCE! (give space for gid numerals

            float spaceUnit = width / 10;
            float captionUnit = (float)(FuzzSets[0].GetHighRange() - FuzzSets[0].GetLowRange()) / 10; //just use the first set - they're all the same ranges anyway!

            for (int i = 0; i <= 10; i++)
            {
                g.DrawLine(new Pen(Color.Black), yGridWidth + (spaceUnit * i) - 1, startY + 5, yGridWidth + (spaceUnit * i) - 1, startY + (gHeight) - 2); // Increment
                g.DrawString(((captionUnit * i) + lowRange).ToString("F1"), new Font("Arial", 6), new SolidBrush(Color.MidnightBlue), yGridWidth + (spaceUnit * i) - 5, startY + (gHeight) - 2);
            }

            //DRAW THE Y-AXIS GRID
            g.DrawLine(new Pen(Color.Black), (yGridWidth / 2) + 4, 4, (yGridWidth / 2) + 4, g.VisibleClipBounds.Height - gHeight - 10);

            float ySpace = ((g.VisibleClipBounds.Height - gHeight - 10) - 4) / 10;
            captionUnit = .1f;

            for (int i = 10; i >= 0; i--)
            {
                g.DrawLine(new Pen(Color.Black), (yGridWidth / 2) + 2, 4 + (ySpace * i), (yGridWidth / 2) + 6, 4 + (ySpace * i)); // Increment
                g.DrawString((1 - (captionUnit * i)).ToString("F1"), new Font("Arial", 6), new SolidBrush(Color.MidnightBlue), 0, (ySpace * i));
            }
        }

        /// <summary>
        /// If there are any errors with one or more set, display those errors
        /// </summary>
        /// <param name="g"></param>
        /// <param name="err"></param>
        private void DrawErrors(Graphics g, List<string> err)
        {
            g.DrawImage(SpriteList.Error, 5, 20);
            g.DrawString("There are errors with this Fuzzy Set:", new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Red), 64, 10);

            int spacing = 14;
            int count = 0;

            foreach (string s in err)
                g.DrawString(s, new Font("Arial", 10), new SolidBrush(Color.Black), 64, 25 + (spacing * count++));
        }

        /// <summary>
        /// Draw the FuzzySets, while checking for errors
        /// </summary>
        /// <param name="g">Graphics Object to draw to</param>
        /// <param name="err">List of errors == Empty ? Render the FuzzySets : Show user the Errors</param>
        public void Draw(Graphics g, List<string> err)
        {
            if (err.Count == 0)
            {
                Error = false;
                Draw(g);
            }
            else
            {
                Error = true;
                DrawErrors(g, err);
            }
        }
    }
}
