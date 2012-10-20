using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FuzzySim.Rendering
{
    class BitmapOperations
    {

        /// <summary>
        /// Sets White pixels in image to transparent.
        /// </summary>
        /// <param name="_in">Image to clean</param>
        /// <returns>Clean Image</returns>
        static public Image RemoveWhiteSpaceFromImage(Image _in)
        {
            Bitmap b = (Bitmap)_in; 
            int binOne = 240; //not quite white 

            for (int y = 0; y < _in.Height; y++)
                for (int x = 0; x < _in.Width; x++)
                {
                    Color pix = b.GetPixel(x, y);

                    if (pix.R >= binOne && pix.G >= binOne && pix.B >= binOne)
                    {
                        pix = Color.FromArgb(0, 0, 0, 0);
                    }
                    b.SetPixel(x, y, pix);
                }
            return _in;
        }

        /// <summary>
        /// Sets Black pixels in image to transparent.
        /// </summary>
        /// <param name="_in">Image to clean</param>
        /// <returns>Clean Image</returns>
        static public Image RemoveBlackSpaceFromImage(Image _in)
        {
            Bitmap b = (Bitmap)_in;
            int binOne = 30; //not quite black ;)

            for (int y = 0; y < _in.Height; y++)
                for (int x = 0; x < _in.Width; x++)
                {
                    Color pix = b.GetPixel(x, y);

                    if (pix.R <= binOne && pix.G <= binOne && pix.B <= binOne)
                    {
                        pix = Color.FromArgb(0, 0, 0, 0);
                    }
                    b.SetPixel(x, y, pix);
                }
            return b;
        }

        public static Rectangle GetBoundingBox(Image _in)
        {
            //need to read in width and height and read into array
            Color[][] data = new Color[_in.Width][];
            Point[] points = new Point[4];

            return new Rectangle();
        }

        static public void SetTextureOpacity(ref Image _in, float _o)
        {
            Bitmap b = (Bitmap) _in;

            for (int y = 0; y < _in.Height; y++)
                for (int x = 0; x < _in.Width; x++)
                {
                    Color pix = b.GetPixel(x, y);

                    pix = Color.FromArgb(((int)(_o * 255f)), pix.R, pix.G, pix.B);
                    
                    b.SetPixel(x, y, pix);
                }

            _in = b;
        }

        static public byte ConvertFloatToByte(float _in)
        {
            int opac = (int)(_in * 255f);

            if (opac > 255f) throw new Exception();

            return (byte)opac;
        }

    }
    
}
