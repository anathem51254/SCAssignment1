using System;
using System.Drawing;

namespace CFLS
{
    /// <summary>
    /// Class which contains static operators for computation
    /// </summary>
    public static class Operations
    {

        /// <summary>
        /// Determines if the difference between two doubles is less than
        /// the Constant EPSILON value.
        /// </summary>
        /// <param name="_v1"></param>
        /// <param name="_V2"></param>
        /// <returns></returns>
        static public bool AEqual(double _v1, double _V2)
        {
            if (Math.Abs(_v1 - _V2) < Constants.EPSILON) return true;
            return false;
        }

        /// <summary>
        /// Interpolates the given points
        /// </summary>
        /// <param name="_x1"></param>
        /// <param name="_y1"></param>
        /// <param name="_x2"></param>
        /// <param name="_y2"></param>
        /// <param name="x"></param>
        /// <returns>double 'y'</returns>
        internal static double Interpolate(double _x1, double _y1, double _x2, double _y2, double x)
        {
            double y, t1, t2, t3;

            t1 = x - _x1;
            t2 = _x2 - _x1;
            t3 = _y2 - _y1;
            y = _y1 + t1 * t3 / t2; // Careful here... copied exactly from Legacy code! [Order of operations?]
            return y;
        }



        /// <summary>
        ///  Defuzzify a set using the Sugeno style aproximation function
        // if it fails it returns -1 (which may be a valid answer)
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static double DeFuzzifySFI(FuzzySet fs)
        /**
        * Defuzzify a set using the Sugeno style aproximation function
        * if it fails it returns -1 (which may be a valid answer)
        */
        {
            if (fs == null) { return -1; }
            if (fs.Invalid()) { fs.Err = 10; return -1; }
            double sumTop = 0;
            double sumBottom = 0;
            double w, s;
            int i;
            for (i = 0; i < fs.GetNumPoints(); i++)
            {
                w = fs.WorldValue[i];
                s = fs.SetValue[i];
                sumTop = sumTop + s * w;
                sumBottom = sumBottom + s;
            }
            if (Operations.AEqual(0, sumBottom)) { fs.Err = 15; return -1; }
            return sumTop / sumBottom;
        }


        /// <summary>
        /// Defizzify a FuzzySet and get its CENTER OF GRAVITY
        /// </summary>
        /// <param name="fs">Input Set</param>
        /// <returns>Center of Gravity</returns>
        public static double DeFuzzifyCOG(FuzzySet fs)
        {
            if (fs == null) { return -1; }

            int points = (int)(((double)fs.GetNumPoints()) * 1.9 + 1);

            if (points < 21) points = 21;

            if (points < 4 || points > 1000) { return -1; }
            if (fs.Invalid()) { fs.Err = 10; return -1; }
            double sumTop = 0;
            double sumBottom = 0;
            double w, s, r, step;
            r = fs.HighRange - fs.LowRange;
            step = r / (points - 1);
            int i;
            for (i = 0; i < points; i++)
            {
                w = fs.LowRange + step * i;
                s = fs.Fuzzify(w);
                sumTop = sumTop + s * w;
                sumBottom = sumBottom + s;
            }
            if (AEqual(0, sumBottom)) { fs.Err = 15; return -1; }

            return sumTop / sumBottom;
        } 

        /// <summary>
        /// Defizzify a FuzzySet and get its MEAN OF MAXIMUMS
        /// </summary>
        /// <param name="fs"></param>
        /// <returns>Mean of Maximums</returns>
        public static double DeFuzzifyMOM(FuzzySet fs)
        /**
        * Defuzzify a set using the Mean of Maximums aproximation function
        * if it fails it returns -1 (which may be a valid answer)
        */
        {
            if (fs == null) { return -1; }
            if (fs.Invalid()) { fs.Err = 10; return -1; }
            double m = fs.GetMaxSetValue();
            double sumTop = 0;
            double sumBottom = 0;
            double w;
            int i;
            for (i = 0; i < fs.GetNumPoints(); i++)
            {
                if (AEqual(fs.SetValue[i], m))
                {
                    w = fs.WorldValue[i];

                    sumTop = sumTop + w;
                    sumBottom = sumBottom + 1;
                }
            }
            if (AEqual(0, sumBottom)) { fs.Err = 15; return -1; }

            return sumTop / sumBottom;
        }


        public static int Intersect(ref double x, ref double y,

                 double x1, double y1, double x2, double y2,

                 double x3, double y3, double x4, double y4

                 )
        {

            double epsilon = Constants.EPSILON; // good enough for 2D graphics where we need nearest pixel

            // this returns the following

            // 0= all ok they inteset and the intersect is in x,y

            // 1= they are both vertical lines

            // 2= they are both horizontal lines

            // 3= they dont intersesct in range



            // a is the line x1,y1-x2,y2

            // b is the line x3,y3-x4,y4

            double aXmin, aYmin, aXmax, aYmax;

            double bXmin, bYmin, bXmax, bYmax;

            aXmin = Math.Min(x1, x2) - epsilon; // must adjust by epsilon or it does not work RC april 2005

            aYmin = Math.Min(y1, y2) - epsilon;

            aXmax = Math.Max(x1, x2) + epsilon;

            aYmax = Math.Max(y1, y2) + epsilon;



            bXmin = Math.Min(x3, x4) - epsilon;

            bYmin = Math.Min(y3, y4) - epsilon;

            bXmax = Math.Max(x3, x4) + epsilon;

            bYmax = Math.Max(y3, y4) + epsilon;



            x = 0; // sane defaults

            y = 0; // sane defaults



            if (aXmin > bXmax) return 3; // basic bounding box check for speed

            if (aYmin > bYmax) return 3;

            if (bXmin > aXmax) return 3;

            if (bYmin > aYmax) return 3;



            // if we get here they might intersect

            double xadiff = Math.Abs(x1 - x2);

            double xbdiff = Math.Abs(x3 - x4);

            double yadiff = Math.Abs(y1 - y2);

            double ybdiff = Math.Abs(y3 - y4);



            double ma, ba; // part of y=mx+b for segment a

            double mb, bb; // part of y=mx+b for segment b



            if (xadiff <= epsilon && xbdiff <= epsilon)
            {// segments a and b are both vertical lines

                return 1;

            }



            if (yadiff <= epsilon && ybdiff <= epsilon)
            {// segments a and b are both horizontal lines

                return 2;

            }



            if (xadiff <= epsilon)
            {// segmenta is a vertical line

                mb = Slope(x3, y3, x4, y4);

                bb = y3 - mb * x3;

                x = x1;

                y = mb * x + bb;

                if (y <= aYmax &&// is the hit in the bounding box ?

                    y <= bYmax &&

                    y >= aYmin &&

                    y >= bYmin) return 0; // its a hit

                return 3;

            }



            if (xbdiff <= epsilon)
            {// segmentb is a vertical line

                ma = Slope(x1, y1, x2, y2);

                ba = y1 - ma * x1;

                x = x3;

                y = ma * x + ba;

                if (y <= aYmax &&// is the hit in the bounding box ?

                    y <= bYmax &&

                    y >= aYmin &&

                    y >= bYmin) return 0; // its a hit

                return 3;

            }



            ma = Slope(x1, y1, x2, y2);

            mb = Slope(x3, y3, x4, y4);

            if (AEqual(ma, mb))
            {

                // paralell lines

                return 3;

            }



            ba = y1 - ma * x1;

            bb = y3 - mb * x3;



            x = (bb - ba) / (ma - mb); // x intersect

            y = ma * x + ba;         // y intersect



            if (x <= aXmax && // is the hit in the bounding box ?

                x <= bXmax &&

                x >= aXmin &&

                x >= bXmin &&

                y <= aYmax &&

                y <= bYmax &&

                y >= aYmin &&

                y >= bYmin) return 0; // its a hit

            return 3;

        }



        /// <summary>
        /// Returns the slope of the line x1,y1 to x2 y2
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double Slope(double x1, double y1, double x2, double y2)
        {
            return (y1 - y2) / (x1 - x2);
        }


        /// <summary>
        /// Copy one set to another if invalid it will still copy
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="copyFrom"></param>
        /// <returns></returns>
        public static bool CopyFS(FuzzySet copyTo, FuzzySet copyFrom)
        {
            if (copyTo == null || copyFrom == null) { return false; }

            copyTo.Id = copyFrom.Id;


            copyTo.LowRange = copyFrom.LowRange;
            copyTo.HighRange = copyFrom.HighRange;
            copyTo.Valid = copyFrom.Valid;
            copyTo.Err = copyFrom.Err;
            int i;
            for (i = 0; i < copyFrom.NumPoints; i++)
            {
                copyTo.WorldValue[i] = copyFrom.WorldValue[i]; // X values range lowRange .. highRange
                copyTo.SetValue[i] = copyFrom.SetValue[i]; // Y values range 0..1
            }
            return true;
        }

        /// <summary>
        /// Performs a CLIP on a Fuzzy Set
        /// </summary>
        /// <param name="fs">The Fuzzy Set to Clip</param>
        /// <param name="top">The scalar amount to Clip the Fuzzy Set to</param>
        /// <returns>Clipped Fuzzy Set</returns>
        public static FuzzySet ClipFS(FuzzySet fs, double clipVal)
        {
            FuzzySet fuzz = new FuzzySet(fs);

            fuzz.UopFS(fuzz, 1, clipVal);

            return fuzz;
        }


        /// <summary>
        /// Returns the COMPLIMENT (inverse) of the Given Fuzzy Set
        /// </summary>
        /// <param name="fs">Fuzzy Set to find the compliment of</param>
        /// <returns>The Compliment Set</returns>
        public static FuzzySet ComplimentFS(FuzzySet fs)
        {
            FuzzySet fuzz = new FuzzySet(fs);

            fuzz.NotFS(fuzz);

            return fuzz;
        }

        /// <summary>
        /// Performs a SCALE on a Fuzzy Set
        /// </summary>
        /// <param name="fs">The Fuzzy Set to Scale</param>
        /// <param name="val">The scalar amount to Scale the Set</param>
        /// <returns></returns>
        public static FuzzySet ScaleFS(FuzzySet fs, double val)
        {
            FuzzySet retSet = new FuzzySet();

            if (fs == null)
            {
                retSet.SetErr(10);
                return retSet;
            }

            retSet = new FuzzySet(fs);

            if (retSet.Invalid())
            {
                retSet.SetErr(10);
                return retSet;
            }

            int i;
            for (i = 0; i < retSet.GetNumPoints(); i++)
            {
                double w = retSet.GetWorldValue(i);
                double s = retSet.GetSetValue(i) * val;
                if (s > 1) s = 1;
                if (s < 0) s = 0;
                retSet.AdjustPoint(i, w, s);
            }

            return retSet;
        }

        /// <summary>
        /// Checks if the Low and High range of two sets are equal
        /// </summary>
        /// <param name="fs1"></param>
        /// <param name="fs2"></param>
        /// <returns></returns>
        public static bool RangeEqual(FuzzySet fs1, FuzzySet fs2) // static
        {
            if (fs1 == null || fs2 == null) { return false; }
            if (!Operations.AEqual(fs1.GetHighRange(), fs2.GetHighRange())) return false;
            if (!Operations.AEqual(fs1.GetLowRange(), fs2.GetLowRange())) return false;
            return true;
        }


        /// <summary>
        /// Performs an Intersection on two FuzzySets
        /// </summary>
        /// <param name="fs1">Fuzzy Set 1</param>
        /// <param name="fs2">Fuzzy Set 2</param>
        /// <param name="colour">The colour of the resulting set</param>
        /// <returns>The resulting Set</returns>
        public static FuzzySet IntersectionFS(FuzzySet fs1, FuzzySet fs2, Brush colour)
        {
            FuzzySet fuzz = new FuzzySet();

            fuzz.IntersectionFS(fuzz, fs1, fs2);

            fuzz.LineColour = colour;

            return fuzz;
        }


        /// <summary>
        /// Performs a UNION of two Fuzzy Sets
        /// </summary>
        /// <param name="fs1">Fuzzy Set 1</param>
        /// <param name="fs2">Fuzzy Set 2</param>
        /// <param name="colour">The colour to render the result</param>
        /// <returns>Result Set</returns>
        public static FuzzySet UnionFS(FuzzySet fs1, FuzzySet fs2, Brush colour)
        {
            FuzzySet result = new FuzzySet(); // just for error checking and returning
            
            //verify...
            if (fs1 == null || fs2 == null || fs1.Invalid() || fs2.Invalid())
            {
                result.SetErr(10);
                return result;
            }

           
            if (!RangeEqual(fs1, fs2))
            {
                result.SetErr(11); return result;
            }

            result = new FuzzySet(fs1);

            result.Clear();

            //lets get to work
            result.SetRange(fs1.GetLowRange(), fs1.GetHighRange());

            double x = 0, y = 0;
            double fs1x1, fs1x2, fs1y1, fs1y2;
            double fs2x3, fs2x4, fs2y3, fs2y4;
            double fs1s, fs1w, fs2s, fs2w;
            int i, k, rc;

            // traverse fs1 add all its points above fs2
            for (i = 0; i < fs1.GetNumPoints(); i++)
            {
                fs1w = fs1.GetWorldValue(i);
                fs1s = fs1.GetSetValue(i);
                fs2s = fs2.Fuzzify(fs1w);
                if (fs1s > fs2s || Operations.AEqual(fs1s, fs2s))
                {
                    result.AddPoint(fs1w, fs1s, false, false);
                }
            }

            // traverse fs2 add all its points above fs1
            for (i = 0; i < fs2.GetNumPoints(); i++)
            {
                fs2w = fs2.GetWorldValue(i);
                fs2s = fs2.GetSetValue(i);
                fs1s = fs1.Fuzzify(fs2w);
                if (fs2s > fs1s)
                {
                    result.AddPoint(fs2w, fs2s, false, false);
                }
            }

            // now traverse all line segments in fs1 testing for intersection with fs2
            for (i = 1; i < fs1.GetNumPoints(); i++)
            {
                fs1x1 = fs1.GetWorldValue(i - 1);
                fs1y1 = fs1.GetSetValue(i - 1);
                fs1x2 = fs1.GetWorldValue(i);
                fs1y2 = fs1.GetSetValue(i);
                for (k = 1; k < fs2.GetNumPoints(); k++)
                {
                    fs2x3 = fs2.GetWorldValue(k - 1);
                    fs2y3 = fs2.GetSetValue(k - 1);
                    fs2x4 = fs2.GetWorldValue(k);
                    fs2y4 = fs2.GetSetValue(k);


                    rc = Intersect(ref x, ref y,
                       fs1x1, fs1y1, fs1x2, fs1y2,
                       fs2x3, fs2y3, fs2x4, fs2y4);

                    if (rc == 0)
                    {
                        result.AddPoint(x, y, false, false);
                    }
                }
            }
            result.ColapseSet();

            result.LineColour = colour;

            return result;
        }

    }

}
