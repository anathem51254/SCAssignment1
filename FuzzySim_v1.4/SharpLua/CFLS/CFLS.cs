using System.Linq;

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
    class CFLS
    {
        /// <summary>
        /// Private Vars
        /// </summary>
        private FuzzySet[] f;
        private int numFuzzySets;


        /// <summary>
        /// Fuzzy Logic System Main Class.
        /// </summary>
        public CFLS()
        {
            Initialize();
        }

        private void Initialize()
        {
            f = new FuzzySet[Constants.MAX_FUZZYSETS];
        }


        /// <summary>
        /// Wipes all Fuzzy Sets
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Constants.MAX_FUZZYSETS; i++)
            {
                f[i] = null;
            }

            numFuzzySets = 0;
        }


        /// <summary>
        /// Gets the number of Fuzzy Sets
        /// </summary>
        /// <returns></returns>
        public int GetNumFuzzySets()
        {    // get numFuzzySets
            return numFuzzySets;
        }

        /// <summary>
        /// finds the index of a named fuzzy set
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public int FindInt(string _id)
        {
            for (int i=0;i<numFuzzySets;i++)
                if (f[i] != null)
                       if (f[i].GetName() == _id)
                          return i;       
            return -1;
        }


        /// <summary>
        /// Gets the set with the given name
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public FuzzySet Find(string _id)
        { // finds a named fuzzy set
            FuzzySet ret;
            try
            {
                ret = f.First(x => x.GetName() == _id);
            }
            catch
            {
                return null;
            }
            return ret;
        }


        /// <summary>
        /// Adds a new Fuzzy Set (Checks for naming 
        /// conflicts first)    
        /// </summary>
        /// <param name="_fs"></param>
        /// <returns></returns>
        public int Add(FuzzySet _fs)
        {     // adds a fuzzy set (checks name first)
                // -1 = fails
            FuzzySet  k = Find(_fs.GetName());
        
            if (k != null) 
                return -1;

            int i = FindEmpty();
            if (i==-1) 
                return -1;

            f[i] = _fs;

            if (i >= numFuzzySets) numFuzzySets=i+1;
            return i;
        }


        /// <summary>
        /// Deletes a Fuzzy Set
        /// </summary>
        /// <param name="i">index to delete</param>
        public void Del(int i)
        {  
            if (i >= 0 && i < numFuzzySets)
            {
                f[i] = null;
                if (i== numFuzzySets-1) numFuzzySets--;
            }
        }



        /// <summary>
        /// Returns the Fuzzy Set found at index 'i'
        /// Returns null on fail.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FuzzySet Get(int i)
        {     // gets fs at i
            if (i >= 0 && i < numFuzzySets)
               {
                return f[i];
               }
            return null;
        }


        /// <summary>
        /// Finds an empty slot and returns the index
        /// Returns -1 on fail.
        /// </summary>
        /// <returns></returns>
        private int FindEmpty()
        {
            int i;
            for (i = 0; i < Constants.MAX_FUZZYSETS; i++)
            {
                if (f[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }




    }
}
