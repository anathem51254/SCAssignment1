/*****************************************************
 *           --- C# Fuzzy Logis System  ---
 *           
 * Converted from C into C# by      Timothy Fox, 
 * from                             Robert Cox 
 * 's original code. 
 * Conversion date:                 14/03/2012         
 * Revision from Orig:              v1.0
 * 
 * Uses principal functions and mathematics from the 
 * study of Fuzzy Logic to perform calculations on 
 * Fuzzy Sets
 ****************************************************/

namespace CFLS
{
    using System;
    using System.Linq;
    using System.Drawing;

    /// <summary>
    /// Additional Attributes added to the original FuzzySet Class, converted from C++
    /// 
    /// - I am thinking this should be in a new inherited class in the FuzzySim Project; 
    ///     Leaving it here does not really make alot of sense? - 09/07/2012
    /// </summary>
    public partial class FuzzySet
    {
        public Brush LineColour;
    }

    /// <summary>
    /// Fuzzy Set: Holds metadata about a Fuzzy Set, 
    /// and allows functions to be performed internally.
    /// </summary>
    public partial class FuzzySet
    {

        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        int numPoints;

        public int NumPoints
        {
            get { return numPoints; }
            set { numPoints = value; }
        }

        public double[] WorldValue
        {
            get { return worldValue; }
            set { worldValue = value; }
        }

        public double[] SetValue
        {
            get { return setValue; }
            set { setValue = value; }
        }

        public double LowRange
        {
            get { return lowRange; }
            set { lowRange = value; }
        }

        public double HighRange
        {
            get { return highRange; }
            set { highRange = value; }
        }

        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public int Err
        {
            get { return err; }
            set { err = value; }
        }

        private double[] worldValue; // X values range lowRange .. highRange
        private double[] setValue; // Y values range 0..1
        private double lowRange;
        private double highRange;
        private bool valid; // true if the set is valid
        private int err; // the error kind if the set is invalid

        /// <summary>
        /// FuzzySet class - denotes a Fuzzy Set and its operations
        /// </summary>
        public FuzzySet()
        {
            Initialize();
        }

        /// <summary>
        /// main fuzzy set constructor creates a empty fuzzy set
        /// </summary>
        /// <param name="_name">Set name</param>
        /// <param name="_fuzzy">Input set</param>
        public  FuzzySet(string _name, double lo, double hi)
        {
            Initialize();
            SetName(_name);
            Clear();
            SetRange(lo, hi);
        }


        /// <summary>
        ///  This FuzzySet constructor creates a new fuzzy set that is
        /// a copy of another set
        /// </summary>
        /// <param name="copyFrom">The FuzzySet to copy</param>
        public FuzzySet(FuzzySet copyFrom)
        {
            id = copyFrom.id;
            LineColour = copyFrom.LineColour;

            numPoints = copyFrom.numPoints;
            LowRange = copyFrom.LowRange;
            HighRange = copyFrom.HighRange;
            Valid = copyFrom.Valid;
            Err = copyFrom.Err;
            int i;

            WorldValue = new double[Constants.MAX_SET];
            SetValue = new double[Constants.MAX_SET];

            for (i = 0; i < copyFrom.numPoints; i++)
            {
                WorldValue[i] = copyFrom.WorldValue[i]; // X values range lowRange .. highRange
                SetValue[i] = copyFrom.SetValue[i]; // Y values range 0..1
            }

        }


        /// <summary>
        /// Sets up private variables (arrays)
        /// </summary>
        private void Initialize()
        {
            id = "Untitled";
            WorldValue = new double[Constants.MAX_SET];
            SetValue = new double[Constants.MAX_SET];
        }

        public void SetErr(int _err)
        {
            Err = _err;
        }

        public void SetName(string _n)
        {
            id = _n;
        }

        public string GetName()
        {
            return Id;
        }

        public int GetNumPoints()
        {
            return NumPoints;
        }

        public double GetWorldValue(int _index)
        {
            //if index out of range, ret 0;
            try
            {
                return WorldValue[_index];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public double GetSetValue(int _index)
        {
            //if index out of range, ret 0;
            try
            {
                return SetValue[_index];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// retreives the min value of the fuzzy set
        /// </summary>
        /// <returns></returns>
        public double GetMinSetValue()
        {
            return SetValue.Min();
        }


        /// <summary>
        /// retreives the maximum value of the fuzzy set
        /// </summary>
        /// <returns></returns>
        public double GetMaxSetValue()
        {
            return SetValue.Max();
        }


        /// <summary>
        /// The critical fuzzification for world value to fuzzy set value
        /// </summary>
        /// <param name="_worldV"></param>
        /// <returns></returns>
        public double Fuzzify(double _worldV)
        {
            //returns -1 is set is invalid
            int i;
            double v;

            if (Invalid()) return -1;

            if ((_worldV < LowRange) || (_worldV > HighRange)) return 0;

            for (i = 0; i < numPoints; i++)
            {
                if (Operations.AEqual(_worldV, WorldValue[i]))
                {
                    //the point at this point could make a vertical line from the other
                    //if it is a vertical line, take the Max Val
                    int k;
                    v = SetValue[i];
                    k = i + 1;
                    while (k < numPoints)
                    {
                        if (!Operations.AEqual(_worldV, WorldValue[k])) break;
                        v = Math.Max(v, SetValue[k]);
                        k++;
                    }

                    if (Operations.AEqual(1, v)) v = 1;
                    if (Operations.AEqual(0, v)) v = 0;

                    return v;
                }
                if (_worldV < WorldValue[i]) break;
            }

            if (i == numPoints)
            {
                v = SetValue[numPoints - 1];
                if (Operations.AEqual(1, v)) v = 1;
                if (Operations.AEqual(0, v)) v = 0;
                return v;
            }

            v = Operations.Interpolate(WorldValue[i - 1], SetValue[i - 1], WorldValue[i], SetValue[i], _worldV);
            if (Operations.AEqual(1, v)) v = 1;
            if (Operations.AEqual(0, v)) v = 0;

            return v;
        }


        /// <summary>
        ///  Validates the Fuzzy set and returns NULL if all is well
        ///* if ther is an error it returns the error message ]
        /// </summary>
        /// <returns>String.Empty on no SetIsValid, Errorstring if not</returns>
        public string Validate()
        {
                    ///* example usage is
            ///*     if (validate != NULL)
            ///*        {
            ///*        -- get and display error using getErr()
            ///*        }
            ///*     else
            ///*        {
            ///*        ... do good stuff
            ///*        }
            // returns null if no error
            Valid = false; // ensure it runs
            if (Invalid())
            {
                return GetErr();
            }
            Err = 0;
            return String.Empty;
        }


        /// <summary>
        /// returns the error text of the last error
        /// </summary>
        /// <returns>Last Exception</returns>
        public string GetErr()
        {
            return Errors.CFLS_errortext[Err];
        }


        /// <summary>
        ///Tests to see if the Fuzzy set is invalid
        /// * Used internally in most cases validate is preferred
        /// * returns true if the set is invalid
        /// </summary>
        /// <returns>Set Validity? True/False</returns>
        public bool Invalid()
        {
            // this routine validates the set
            if (Valid) return false;
            if (numPoints < 2)
            { Err = 6; return true; }
            if (!Operations.AEqual(LowRange, WorldValue[0])) { Err = 3; return true; } //"03-Fuzzy set is missing low range entry",
            if (!Operations.AEqual(HighRange, WorldValue[numPoints - 1])) { Err = 4; return true; } //"04-Fuzzy set is missing high range entry",

            for (int i = 0; i < numPoints; i++)
            {
                double w = WorldValue[i];
                double s = SetValue[i];
                if (s < 0 || s > 1) { Err = 18; return true; }
                if (Operations.AEqual(w, LowRange) && i != 0) { Err = 19; return true; }
                if (Operations.AEqual(w, HighRange) && i != numPoints - 1) { Err = 20; return true; }
            }

            Valid = true;   
            return false;
        }


        /// <summary>
        /// Deletes a point and compresses the set
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool DelPoint(int index)
        {
            if (index < 0 || index >= numPoints) { Err = 7; return false; }
            if (numPoints <= 0) { Err = 8; return false; }
            if (index == numPoints - 1) { Valid = false; numPoints--; return true; }
            int i;
            for (i = index; i < numPoints - 1; i++)
            {
                WorldValue[i] = WorldValue[i + 1];
                SetValue[i] = SetValue[i + 1];
            }
            numPoints--;
            Valid = false;
            return true;
        }

        /// <summary>
        /// Swaps the order of two points p1 and p2
        ///* It is not for general external use but to support lua needs to be public
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void SwapPoints(int p1, int p2)
        {
            double w, s;
            w = WorldValue[p1];
            s = SetValue[p1];
            WorldValue[p1] = WorldValue[p2];
            SetValue[p1] = SetValue[p2];
            WorldValue[p2] = w;
            SetValue[p2] = s;
        }


        /// <summary>
        /// Sorts the sets and recalculates area under each segment.
        /// Not for external use, but required for Fuzzy Lua
        /// </summary>
        public void SortSet()
        {
            if (numPoints <= 1) return;
            int i;
            bool swap = true;
            while (swap)
            {
                swap = false;
                for (i = 1; i < numPoints; i++)
                { // now compare i and i-1
                    if (WorldValue[i - 1] > WorldValue[i])
                    {
                        SwapPoints(i - 1, i);
                        swap = true;
                    }
                }
                for (i = 1; i < numPoints; i++)
                { // now compare i and i-1 for the crisp bit
                    if (Operations.AEqual(WorldValue[i - 1], WorldValue[i]))
                    {
                        WorldValue[i - 1] = WorldValue[i]; // not just aequal but exactly equal
                        if (i - 1 > 0)
                        {
                            //order of crisp pair should be based on nearest
                            int p0, p1, p2;
                            p0 = i - 2;
                            p1 = i - 1;
                            p2 = i;
                            double p0s, p1s, p2s;
                            p0s = SetValue[p0];
                            p1s = SetValue[p1];
                            p2s = SetValue[p2];
                            // now which of p1,p2 is closer to p0
                            if (Math.Abs(p0s - p2s) < Math.Abs(p0s - p1s))
                            {
                                // p2 is closer than p1 so we need to swap
                                SwapPoints(i - 1, i);
                                swap = true;
                            }
                        }
                    }
                }
            }
            return;
        }


        /// <summary>
        ///If the set has more than MAX_POINTS the set will be retuced to
        /// * MAX_POINTS by aproximation
        /// * returns true if it succeeds if not it sets err
        /// * At no time will the end points be touched in this process
        /// </summary>
        /// <returns></returns>
        public bool ColapseSet()
        {
        while (numPoints > Constants.MAX_POINTS)
              {
                ColapsePoint();
              }
        return true;
        }


        /// <summary>
        /// clears the fuzzy set back to a valid value it does not adjust range
        /// </summary>
        /// <returns></returns>
        public bool ColapsePoint()
        {
            if (numPoints <= 4) return false;

            int i = 0, p1 = 0, p2 = 0;
            double[] wv = new double[Constants.MAX_SET]; // normalised word distances in range 0..1
            double minDist;
            double r, d, s; //
            r = 1 / (HighRange - LowRange);
            for (i = 0; i < numPoints; i++)
            {
                wv[i] = (WorldValue[i] - LowRange) * r;
            }

            // now find the two nearest ajacent points
            minDist = 999;
            for (i = 1; i < numPoints - 2; i++)
            {
                s = (wv[i] - wv[i + 1]) * (wv[i] - wv[i + 1]) + (SetValue[i] - SetValue[i + 1]) * (SetValue[i] - SetValue[i + 1]);
                d = Math.Sqrt(s);
                if (d <= minDist)
                {
                    p1 = i;
                    p2 = i + 1;
                    minDist = d;
                }
            }
            // at this stage p1 and p2 are the points to merge
            // special cases if it is a first or last point
            if (p1 == 0)
            {
                AdjustPoint(p1, WorldValue[p2], (SetValue[p1] + SetValue[p2]) / 2);
                DelPoint(p2);
                return true;
            }
            if (p2 == numPoints - 1)
            {
                AdjustPoint(p2, WorldValue[p2], (SetValue[p1] + SetValue[p2]) / 2);
                DelPoint(p1);
                return true;
            }

            AdjustPoint(p1, (WorldValue[p1] + WorldValue[p2]) / 2, (SetValue[p1] + SetValue[p2]) / 2);
            DelPoint(p2);
            return true;
        }

        public bool ClearVal(double val)
        {
            if (val < 0 || val > 1) return false;
            numPoints = 0;
            Valid = true;
            Err = 0;
            AddPoint(LowRange, val, false, false);
            AddPoint(HighRange, val, false, false);
            return true;
        }

        /// <summary>
        /// clears the fuzzy set back to invalid value and sets the range to 0..1
        /// </summary>
        public void Clear()
        {
            numPoints = 0;
            LowRange = 0;
            HighRange = 1;
            Valid = false;
            Err = 0;
        }

        /// <summary>
        /// Adds a point to a peicewise linear fuzzy set
        ///returns false if it fails
        /// note that a duplicate added with allowDuplicates=false still returns true
        /// allow bastards allows you to add points with a set value >1 or less than 0  
     
        /// </summary>
        /// <param name="worldVal"></param>
        /// <param name="setVal"></param>
        /// <param name="allowDuplicates"></param>
        /// <param name="allowBastard"></param>
        /// <returns></returns>
        public bool AddPoint(double worldVal, double setVal, bool allowDuplicates, bool allowBastard)
        {
            if (numPoints >= Constants.MAX_SET) { Err = 14; return false; }
            if ((!allowBastard) && (setVal < 0 || setVal > 1)) { Err = 13; return false; }
            if (worldVal < LowRange || worldVal > HighRange) { Err = 12; return false; }
            if (!allowDuplicates)
            {
                int i;
                for (i = 0; i < numPoints; i++)
                {
                    //double s=fuzzify(worldVal);
                    if (Operations.AEqual(SetValue[i], setVal) && Operations.AEqual(WorldValue[i], worldVal)) return true;
                }
            }

            WorldValue[numPoints] = worldVal;
            SetValue[numPoints] = setVal;
            numPoints++;
            SortSet();
            Valid = false;

            return true;
        }

        /// <summary>
        /// Adds a point to a peicewise linear fuzzy set
        ///returns false if it fails
        /// </summary>
        /// <param name="worldVal"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public bool AddPoint(double worldVal, double setVal)
        {
            return AddPoint(worldVal, setVal, false, false);
        }

        /// <summary>
        /// alters the world values and set value of a point
        /// WARNING: It may well be wise to run sortSet() after this unless you are sure that the
        ///         order of points does not change.
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="worldVal"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public bool AdjustPoint(int index, double worldVal, double setVal)
        {
            if (index < 0 || index >= numPoints) { Err = 7; return false; }
            WorldValue[index] = worldVal;
            SetValue[index] = setVal;
            Valid = false;
            return true;
        }

        /// <summary>
        /// Sets the range of the Fuzzy Set
        /// </summary>
        /// <param name="lowR"></param>
        /// <param name="highR"></param>
        /// <returns></returns>
        public bool SetRange(double lowR, double highR)
        {
            if (lowR >= highR) { Err = 5; return false; }
            if (highR - lowR > 10000) { Err = 1; return false; }  //"01-Range must not be greater than 10,000",
            if (highR - lowR < 0.01) { Err = 2; return false; }  //"02-Range must not be less than than 0.01",
            LowRange = lowR;
            HighRange = highR;
            return true;
        }


        /// <summary>
        /// Sets the range of the Fuzzy Set, including two points at the min and max world values.
        /// </summary>
        /// <param name="lowR"></param>
        /// <param name="highR"></param>
        /// <returns></returns>
        public bool SetRangeWithPoints(double lowR, double highR)
        {
            if (lowR >= highR) { Err = 5; return false; }
            if (highR - lowR > 10000) { Err = 1; return false; }  //"01-Range must not be greater than 10,000",
            if (highR - lowR < 0.01) { Err = 2; return false; }  //"02-Range must not be less than than 0.01",
            LowRange = lowR;
            HighRange = highR;

            AddPoint(lowR, 0, true, false);
            AddPoint(highR, 0, true, false);
            return true;
        }


        /// <summary>
        /// Gets the Low Range
        /// </summary>
        /// <returns></returns>
        public double GetLowRange()
        {
            return LowRange;
        }


        /// <summary>
        /// Gets the High Range
        /// </summary>
        /// <returns></returns>
        public double GetHighRange()
        {
            return HighRange;
        }


        /*********************** Static FUZZY SET ***********/


        /// <summary>
        /// Not Fuzzy Set (1-v)
        /// </summary>
        /// <param name="_fs"></param>
        /// <returns></returns>
        public bool NotFS(FuzzySet _fs) // static
        {
            if (_fs == null) { return false; }
            if (_fs.Invalid()) { _fs.Err = 10; return false; }
            int i;
            for (i = 0; i < _fs.numPoints; i++)
            {
                double w = _fs.WorldValue[i];
                double s = _fs.SetValue[i];
                s = 1 - s;
                _fs.AdjustPoint(i, w, s);
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="opCode"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool UopFS(FuzzySet _fs, int opCode, double val) // static
        {
            /**
            * Unary Operator On fuzzy set
            opCode = 0  clipScale
            opCode = 1  clip (top/max)
            opCode = 2  Scale
            opCode = 3  clip (bottom/min)
            opCode = 4  normalise to a max of val (usually val=1)
            opCode = 5  tessalate (runs of val are sudivided)
            opcode = 6  raise to power of val
            */
            if (_fs == null) { return false; }
            if (_fs.Invalid() && opCode != 1 && opCode != 3) { _fs.Err = 10; return false; }
            // we allow through opcode 1 and 3 to clip bastard points
            int i, p1, p2;
            double s, w;
            double m;
            bool tes;
            switch (opCode)
            {
                case 0: // clip scale
                    if (val < 0 || val > 1) { _fs.Err = 17; return false; }
                    for (i = 0; i < _fs.numPoints; i++)
                    {
                        w = _fs.WorldValue[i];
                        s = _fs.SetValue[i];
                        if (s > val) s = val;
                        _fs.AdjustPoint(i, w, s);
                    }
                    break;

                case 1: // clip
                    if (val < 0 || val > 1) { _fs.Err = 17; return false; }
                    if (_fs.SetValue[0] > val)
                    {
                        w = _fs.WorldValue[0];
                        s = val;
                        _fs.AdjustPoint(0, w, s);
                    }
                    if (_fs.SetValue[_fs.numPoints - 1] > val)
                    {
                        w = _fs.WorldValue[_fs.numPoints - 1];
                        s = val;
                        _fs.AdjustPoint(_fs.numPoints - 1, w, s);
                    }
                    // add intersection points
                    _fs.AddIntersectPoints(_fs.LowRange, val, _fs.HighRange, val);
                    for (i = 1; i < _fs.numPoints - 1; i++)
                    {
                        w = _fs.WorldValue[i];
                        s = _fs.SetValue[i];
                        if (s > val) { _fs.DelPoint(i); i--; }
                    }
                    break;

                case 2: //scale
                    return ScaleFS(_fs, val);

                case 3:   //clip (bottom/min)
                    if (val < 0 || val > 1) { _fs.Err = 17; return false; }
                    if (_fs.SetValue[0] < val)
                    {
                        w = _fs.WorldValue[0];
                        s = val;
                        _fs.AdjustPoint(0, w, s);
                    }
                    if (_fs.SetValue[_fs.numPoints - 1] < val)
                    {
                        w = _fs.WorldValue[_fs.numPoints - 1];
                        s = val;
                        _fs.AdjustPoint(_fs.numPoints - 1, w, s);
                    }
                    // add intersection points
                    _fs.AddIntersectPoints(_fs.LowRange, val, _fs.HighRange, val);
                    // delete points below
                    for (i = 1; i < _fs.numPoints - 1; i++)
                    {
                        w = _fs.WorldValue[i];
                        s = _fs.SetValue[i];
                        if (s < val) { _fs.DelPoint(i); i--; }
                    }
                    return true;

                case 4:   //normalise to a max of val (usually val=1)
                    if (_fs.numPoints < 1) return true;
                    m = _fs.GetMaxSetValue();
                    m = val / m; // m is now the multiplier
                    for (i = 0; i < _fs.numPoints; i++)
                    {
                        s = _fs.SetValue[i];
                        _fs.SetValue[i] = s * m;
                    }
                    return true;

                case 5:  // tessalate (runs of val are sudivided)
                    // note vertical runs are not teselated
                    if (_fs.numPoints < 3) return true;

                    tes = true;
                    while (tes)
                    {
                        tes = false;
                        for (i = 1; i < _fs.numPoints; i++)
                        {
                            p1 = i - 1;
                            p2 = i;
                            if (Math.Abs(_fs.SetValue[p1] - _fs.SetValue[p2]) >= val &&
                                !Operations.AEqual(_fs.WorldValue[p2], _fs.WorldValue[p1]))
                            {
                                if (_fs.numPoints < Constants.MAX_POINTS)
                                {
                                    w = (_fs.WorldValue[p2] + _fs.WorldValue[p1]) / 2;
                                    s = (_fs.SetValue[p2] + _fs.SetValue[p1]) / 2;
                                    _fs.AddPoint(w, s, false, false);
                                    tes = true;
                                }
                            }
                        }
                    }
                    return true;

                case 6:  // raise to power of val

                    if (_fs.numPoints < 1) return true;
                    for (i = 0; i < _fs.numPoints; i++)
                    {
                        s = _fs.SetValue[i];
                        if (s > 0)
                        {
                            s = Math.Pow(s, val);
                            if (s > 1) s = 1;
                            if (s < 0) s = 0;
                            _fs.SetValue[i] = s;
                        }
                    }
                    return true;

                default: _fs.Err = 16;
                    return false;
            }
            return false;
        }


        /// <summary>
        /// returns the minimum set vale of any point
        /// </summary>
        /// <returns></returns>
        public double GetMinVal()
        {
            double v;
            if (numPoints < 1) return 0;
            v = SetValue[0];
            for (int i = 1; i < numPoints; i++)
            {
                if (SetValue[i] < v) v = SetValue[i];
            }
            return v;
        }


        /// <summary>
        /// returns the max set vale of any point
        /// </summary>
        /// <returns></returns>
        public double GetMaxVal()
        {
            double v;
            if (numPoints < 1) return 0;
            v = SetValue[0];
            for (int i = 1; i < numPoints; i++)
            {
                if (SetValue[i] > v) v = SetValue[i];
            }
            return v;
        }


        /// <summary>
        /// returns the area under the graph
        /// </summary>
        /// <returns></returns>
        public double GetAreaVal()
        {
            double a = 0;
            double h;
            if (numPoints < 1) return 0;
            for (int i = 1; i < numPoints; i++)
            {
                h = (SetValue[i - 1] + SetValue[i]) / 2;
                a = a + h * (WorldValue[i] - WorldValue[i - 1]);
            }
            return a;
        }


        /// <summary>
        /// This adds all points in the fuzzy set than intersect with the
        /// * line described by x1,y1 to x2,y2
        /// * returns the nmber of points added
        /// </summary>
        /// <param name="p"></param>
        /// <param name="val"></param>
        /// <param name="p_2"></param>
        /// <param name="val_2"></param>
        public int AddIntersectPoints(double x1, double y1, double x2, double y2)
        {
            // now traverse all line segments in fs1 testing for intersection with fs2
            double x3, y3, x4, y4;
            double x =0 , y = 0;
            int k, retv = 0;
            for (k = 1; k < numPoints; k++)
            {
                x3 = WorldValue[k - 1];
                y3 = SetValue[k - 1];
                x4 = WorldValue[k];
                y4 = SetValue[k];
                int rc = Operations.Intersect(ref x, ref y,
                             x1, y1, x2, y2,
                             x3, y3, x4, y4);
                if (rc == 0)
                {
                    AddPoint(x, y, false, false);
                    retv++;
                }
            }
            return retv;
        }

        public bool ScaleFS(FuzzySet fs, double val)
        {
            if (fs == null) { return false; }
            if (fs.Invalid()) { fs.Err = 10; return false; }
            int i;
            for (i = 0; i < fs.numPoints; i++)
            {
                double w = fs.WorldValue[i];
                double s = fs.SetValue[i] * val;
                if (s > 1) s = 1;
                if (s < 0) s = 0;
                fs.AdjustPoint(i, w, s);
            }
            return true;
        }


        public int CompareFS(FuzzySet _fs1, FuzzySet _fs2) // static
        /*
         Compare one set to another
         return values
         -1 - fs1 <= fs2 at all points
         0  - Both sets are equal at all points
         1  - fs1 >= fs2 at all points
         2  - fs2 and fs1 overlap eachother or have unrqual ranges
         */
        {
            int retv = 0;
            int i;
            double w, s1, s2;
            if (_fs1 == null || _fs2 == null) { return 2; }

            //strcpy(copyTo.id,copyFrom.id);
            if (!Operations.AEqual(_fs1.LowRange, _fs2.LowRange)) return 2;
            if (!Operations.AEqual(_fs1.HighRange, _fs2.HighRange)) return 2;

            for (i = 0; i < _fs1.numPoints; i++)
            {
                w = _fs1.WorldValue[i];
                s1 = _fs1.SetValue[i];
                s2 = _fs2.Fuzzify(w);
                if (Operations.AEqual(s1, s2)) continue;
                if (s1 < s2)
                {
                    if (retv == -1) continue;
                    if (retv == 1) return 2;
                    retv = -1;
                }
                if (s1 > s2)
                {
                    if (retv == 1) continue;
                    if (retv == -1) return 2;
                    retv = 1;
                }
            }

            for (i = 0; i < _fs2.numPoints; i++)
            {
                w = _fs2.WorldValue[i];
                s2 = _fs2.SetValue[i];
                s1 = _fs1.Fuzzify(w);
                if (Operations.AEqual(s1, s2)) continue;
                if (s1 < s2)
                {
                    if (retv == -1) continue;
                    if (retv == 1) return 2;
                    retv = -1;
                }
                if (s1 > s2)
                {
                    if (retv == 1) continue;
                    if (retv == -1) return 2;
                    retv = 1;
                }
            }
            return retv;
        }


        public int CompareFSVal(FuzzySet fs1, double val) // static
        /**
        Compare one set to a value
        return values
        -1 - fs1 < val  at all points
        0  - fs1 == val at all points
        1  - fs1 > val  at all points
        2  - fs1 overlaps val at at least 1 point
        */
        {
            int retv = 0;
            int i;
            double s1, s2;
            s2 = val;
            if (fs1 == null) { return 2; }

            for (i = 0; i < fs1.numPoints; i++)
            {
                s1 = fs1.SetValue[i];
                if (Operations.AEqual(s1, s2)) continue;
                if (s1 < s2)
                {
                    if (retv == -1) continue;
                    if (retv == 1) return 2;
                    retv = -1;
                }
                if (s1 > s2)
                {
                    if (retv == 1) continue;
                    if (retv == -1) return 2;
                    retv = 1;
                }
            }
            return retv;
        }

        /// <summary>
        /// Copy one set to another if invalid it will still copy
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="copyFrom"></param>
        /// <returns></returns>
        public bool CopyFS(FuzzySet copyTo, FuzzySet copyFrom)
        {
            if (copyTo == null || copyFrom == null) { return false; }

            //strcpy(copyTo.id,copyFrom.id);
            copyTo.numPoints = copyFrom.numPoints;
            copyTo.LowRange = copyFrom.LowRange;
            copyTo.HighRange = copyFrom.HighRange;
            copyTo.Valid = copyFrom.Valid;
            copyTo.Err = copyFrom.Err;
            int i;

            WorldValue = new double[Constants.MAX_SET];
            SetValue = new double[Constants.MAX_SET];

            for (i = 0; i < copyFrom.numPoints; i++)
            {
                copyTo.WorldValue[i] = copyFrom.WorldValue[i]; // X values range lowRange .. highRange
                copyTo.SetValue[i] = copyFrom.SetValue[i]; // Y values range 0..1
            }
            return true;
        }


        public bool RangeEqual(FuzzySet fs1, FuzzySet fs2) // static
        {
            if (fs1 == null || fs2 == null) { return false; }
            if (!Operations.AEqual(fs1.GetHighRange(), fs2.GetHighRange())) return false;
            if (!Operations.AEqual(fs1.GetLowRange(), fs2.GetLowRange())) return false;
            return true;
        }


        public bool TwoSetIterator(FuzzySet fs1, FuzzySet fs2, ref int index1, ref int index2, ref double nextVal)
        {
            if (index1 >= fs1.numPoints || index2 >= fs2.numPoints) return false;

            if (Operations.AEqual(fs1.WorldValue[index1], fs2.WorldValue[index2]))
            {
                nextVal = fs1.WorldValue[index1];
                index1++;
                index2++;
                return true;
            }

            if (fs1.WorldValue[index1] < fs2.WorldValue[index2])
            {
                nextVal = fs1.WorldValue[index1];
                index1++;
                return true;
            }
            nextVal = fs2.WorldValue[index2];
            index2++;
            return true;
        }

        


        /// <summary>
        ///  Defuzzify a set using the Sugeno style aproximation function
        // if it fails it returns -1 (which may be a valid answer)
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public double DeFuzzifySFI(FuzzySet fs)
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
            for (i = 0; i < fs.numPoints; i++)
            {
                w = fs.WorldValue[i];
                s = fs.SetValue[i];
                sumTop = sumTop + s * w;
                sumBottom = sumBottom + s;
            }
            if (Operations.AEqual(0, sumBottom)) { fs.Err = 15; return -1; }
            return sumTop / sumBottom;
        }

        public double DeFuzzifyMOM(FuzzySet fs)
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
            for (i = 0; i < fs.numPoints; i++)
            {
                if (Operations.AEqual(fs.SetValue[i], m))
                {
                    w = fs.WorldValue[i];

                    sumTop = sumTop + w;
                    sumBottom = sumBottom + 1;
                }
            }
            if (Operations.AEqual(0, sumBottom)) { fs.Err = 15; return -1; }

            return sumTop / sumBottom;
        }

        public double DeFuzzifyCOG(FuzzySet fs, int points)
        {
            if (fs == null) { return -1; }
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
            if (Operations.AEqual(0, sumBottom)) { fs.Err = 15; return -1; }

            return sumTop / sumBottom;
        }


        /// <summary>
        /// Performs an OR on two sets
        /// </summary>
        /// <param name="result">The resulting set</param>
        /// <param name="fs1">FuzzySet 1</param>
        /// <param name="fs2">FuzzySet 2</param>
        /// <returns></returns>
        public bool IntersectionFS(FuzzySet result, FuzzySet fs1, FuzzySet fs2)
        {

            if (fs1 == null || fs2 == null || result == null) { return false; }
            if (fs1.Invalid()) { result.err = 10; return false; }
            if (fs2.Invalid()) { result.err = 10; return false; }
            if (!RangeEqual(fs1, fs2)) { result.err = 11; return false; }
            result.Clear();

            result.SetRange(fs1.lowRange, fs1.highRange);

            double x = 0; 
            double y = 0;
            double fs1x1, fs1x2, fs1y1, fs1y2;
            double fs2x3, fs2x4, fs2y3, fs2y4;
            int i, k, rc;

            // traverse fs1 add all its points above fs2
            for (i = 0; i < fs1.numPoints; i++)
            {
                fs1x1 = fs1.worldValue[i];
                fs1y1 = fs1.setValue[i];
                fs2y3 = fs2.Fuzzify(fs1x1);

                if (fs1y1 < fs2y3 || Operations.AEqual(fs2y3, fs1y1))
                {
                    result.AddPoint(fs1x1, fs1y1, false, false);
                }
            }

            // traverse fs2 add all its points above fs1
            for (i = 0; i < fs2.numPoints; i++)
            {
                fs2x3 = fs2.worldValue[i];
                fs2y3 = fs2.setValue[i];
                fs1y1 = fs1.Fuzzify(fs2x3);

                if (fs2y3 < fs1y1)
                {
                    result.AddPoint(fs2x3, fs2y3, false, false);
                }
            }

            // now traverse all line segments in fs1 testing for intersection with fs2
            for (i = 1; i < fs1.numPoints; i++)
            {
                fs1x1 = fs1.worldValue[i - 1];
                fs1y1 = fs1.setValue[i - 1];
                fs1x2 = fs1.worldValue[i];
                fs1y2 = fs1.setValue[i];

                for (k = 1; k < fs2.numPoints; k++)
                {
                    fs2x3 = fs2.worldValue[k - 1];
                    fs2y3 = fs2.setValue[k - 1];
                    fs2x4 = fs2.worldValue[k];
                    fs2y4 = fs2.setValue[k];

                    rc = Operations.Intersect(ref x, ref y,
                       fs1x1, fs1y1, fs1x2, fs1y2,
                       fs2x3, fs2y3, fs2x4, fs2y4);
                    if (rc == 0)
                    {
                        result.AddPoint(x, y, false, false);
                    }
                }
            }
            result.ColapseSet();

            return true;
        }


        public bool OperatorFS(FuzzySet result, FuzzySet fs1, FuzzySet fs2, int opCode, double fs1Amt, double fs2Amt, int clipStrat)
        /**
        * An array of operators
        * FL_BOUNDED_SUM    = 0 (ignores fs1Amt & fs2Amt)
        * FL_BOUNDED_DIFF   = 1 (ignores fs1Amt & fs2Amt)
        * FL_BOUNDED_PROD   = 2 (ignores fs1Amt & fs2Amt)
        * FL_BOUNDED_DIV    = 3 (ignores fs1Amt & fs2Amt)
        * FL_NPINTERSECTION = 4 (ignores fs1Amt & fs2Amt)
        * FL_PROBOR         = 5 (ignores fs1Amt & fs2Amt)
        *
        * FL_WEIGHTED_AVERAGE = 100 (ignores fs1Amt & fs2Amt)
        * May increase the number of points
        */
        {
            if (fs1 == null || fs2 == null || result == null) { return false; }
            if (fs1.Invalid()) { result.Err = 10; return false; }
            if (fs2.Invalid()) { result.Err = 10; return false; }
            if (!RangeEqual(fs1, fs2)) { result.Err = 11; return false; }
            result.Clear();
            result.SetRange(fs1.LowRange, fs1.HighRange);
            int index1 = 0;
            int index2 = 0;
            double nextVal = 0;
            double t1, t2; // temp
            double x, y;
            while (TwoSetIterator(fs1, fs2, ref index1, ref index2, ref nextVal))
            {
                t1 = fs1.Fuzzify(nextVal);
                t2 = fs2.Fuzzify(nextVal);
                x = nextVal;
                switch (opCode)
                {
                    case 0: // FL_BOUNDED_SUM
                        y = t1 + t2;
                        break;

                    case 1: // FL_BOUNDED_DIFF   fs1-fs2
                        y = t1 - t2;
                        break;

                    case 2:  //FL_BOUNDED_PROD
                        y = t1 * t2;
                        break;

                    case 3:  //FL_BOUNDED_DIV  fs1/fs2
                        if (Operations.AEqual(0, t2))
                        {
                            y = 1;
                            if (Operations.AEqual(0, t1)) y = 0;
                        }
                        else y = t1 / t2;
                        break;

                    case 4:  //FL_NPINTERSECTION
                        y = t1 + t2 - 1;
                        break;

                    case 5:  //PROBOR
                        y = (t1 + t2) - (t1 * t2);
                        break;

                    case 100: // FL_WEIGHTED_AVERAGE = 100 (uses fs1Amt & fs2Amt)
                        y = (t1 * fs1Amt) + (t2 * fs2Amt);
                        break;

                    default:
                        result.Err = 16;
                        return false;
                    //break; // the return will take us out
                }
                if (clipStrat == 0)
                {
                    if (y > 1 || Operations.AEqual(y, 1)) y = 1;
                    if (y < 0 || Operations.AEqual(y, 0)) y = 0;
                }
                result.AddPoint(x, y, false, true);
            }
            if (clipStrat == 1)
            {
                result.UopFS(result, 1, 1); //  clip (top/max)
                result.UopFS(result, 3, 0); //  clip (bottom/min)
            }
            result.ColapseSet();
            return true;
        }

        /// <summary>
        /// THIS SET WAS COMMENTED OUT FROM THE LEGACY CODE. USE WITH CAUTION, IF AT ALL
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fs1"></param>
        /// <param name="fs2"></param>
        /// <returns></returns>
        public bool UnionFS(FuzzySet result, FuzzySet fs1, FuzzySet fs2)
        {
            if (fs1 == null || fs2 == null || result == null) { return false; }
            if (fs1.Invalid()) { result.Err = 10; return false; }
            if (fs2.Invalid()) { result.Err = 10; return false; }
            if (!RangeEqual(fs1, fs2)) { result.Err = 11; return false; }
            result.Clear();
            result.SetRange(fs1.LowRange, fs1.HighRange);
            double x = 0, y = 0;
            double fs1x1, fs1x2, fs1y1, fs1y2;
            double fs2x3, fs2x4, fs2y3, fs2y4;
            double fs1s, fs1w, fs2s, fs2w;
            int i, k, rc;

            // traverse fs1 add all its points above fs2
            for (i = 0; i < fs1.numPoints; i++)
            {
                fs1w = fs1.WorldValue[i];
                fs1s = fs1.SetValue[i];
                fs2s = fs2.Fuzzify(fs1w);
                if (fs1s > fs2s || Operations.AEqual(fs1s, fs2s))
                {
                    result.AddPoint(fs1w, fs1s, false, false);
                }
            }

            // traverse fs2 add all its points above fs1
            for (i = 0; i < fs2.numPoints; i++)
            {
                fs2w = fs2.WorldValue[i];
                fs2s = fs2.SetValue[i];
                fs1s = fs1.Fuzzify(fs2w);
                if (fs2s > fs1s)
                {
                    result.AddPoint(fs2w, fs2s, false, false);
                }
            }

            // now traverse all line segments in fs1 testing for intersection with fs2
            for (i = 1; i < fs1.numPoints; i++)
            {
                fs1x1 = fs1.WorldValue[i - 1];
                fs1y1 = fs1.SetValue[i - 1];
                fs1x2 = fs1.WorldValue[i];
                fs1y2 = fs1.SetValue[i];
                for (k = 1; k < fs2.numPoints; k++)
                {
                    fs2x3 = fs2.WorldValue[k - 1];
                    fs2y3 = fs2.SetValue[k - 1];
                    fs2x4 = fs2.WorldValue[k];
                    fs2y4 = fs2.SetValue[k];
                    rc = Operations.Intersect(ref x , ref y,
                       fs1x1, fs1y1, fs1x2, fs1y2,
                       fs2x3, fs2y3, fs2x4, fs2y4);
                    if (rc == 0)
                    {
                        result.AddPoint(x, y, false, false);
                    }
                }
            }
            result.ColapseSet();
            return true;
        }

    }
}
