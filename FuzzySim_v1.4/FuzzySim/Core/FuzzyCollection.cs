namespace FuzzySim.Core
{
    using System.Collections.Generic;
    using CFLS;

    /// <summary>
    /// A FuzzyCollection is a group of FuzzySets 
    ///  - It is assumed and required that the referenced FuzzySets have identical
    ///  Low and High ranges!
    /// </summary>
    public class FuzzyCollection : Dictionary<string, FuzzySet> 
    {
        /// <summary>
        /// The name of the collection
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// Initialises a new FuzzyCollection
        /// </summary>
        /// <param name="collectionName">Name of the Collection</param>
        /// <param name="input">The collection of FuzzySet inputs</param>
        public FuzzyCollection(string collectionName, Dictionary<string, FuzzySet> input)
        {
            SetName = collectionName;

            if(input == null)
                return;

            foreach (KeyValuePair<string, FuzzySet> f in input)
            {
                this.Add(f.Key, f.Value);
            }
        }

        public void Add(FuzzySet fs)
        {
            base.Add(fs.Id, fs);
        }

        public void Add(string id)
        {
            base.Add(id, new FuzzySet(id, 0,1));
        }

    }
}

