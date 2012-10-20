namespace FuzzySim.Simulators
{

    using System.Collections.Generic;
    using System.Drawing;
    using Core;
    using CFLS;
    
    /*      ---- Rememeber! ----
     * 
     *      1. Declare FuzzyCollections 
     *      2. Initialize FuzzyCollections - InitializeFuzzySets()
     *      3. Populate FuzzyCollections with FuzzySets - InitializeFuzzySets() -> WrapperMethod()
     *      4. Add FuzzyCollections to return collection - GetFuzzyLogic()
     */


    class TextController : AIController
    {
        /*
         *      DECLARE YOUR FUZZY COLLECTIONS HERE 
         * */
        public FuzzyCollection ExampleSets;
        public FuzzyCollection WorkingSets1;
        public FuzzyCollection WorkingSets2;

        /// <summary>
        /// Returns the Sets defined by the user here
        /// </summary>
        /// <returns></returns>
        public override List<FuzzyCollection> GetFuzzyLogic()
        {
            List<FuzzyCollection> setsToReturn = new List<FuzzyCollection>();

            setsToReturn.Add(ExampleSets);
            setsToReturn.Add(WorkingSets1);
            setsToReturn.Add(WorkingSets2);

            return setsToReturn;
        }


        /// <summary>
        /// Performs User-Defined FuzzyLogic on Fuzzy Sets
        /// </summary>
        public override void CalculateFuzzyLogic()
        {
            ((TextSim) Globals.Simulator).DefuzziedValue1 = Operations.DeFuzzifyCOG(WorkingSets2["E1 E2 Union"]);

            ((TextSim)Globals.Simulator).DefuzziedValue2 = Operations.DeFuzzifySFI(WorkingSets2["E1 E2 Union"]);
        }

        /// <summary>
        /// Initialize and set up user-defined sets
        /// </summary>
        public override void InitializeController()
        {
            //initialize the collection
            ExampleSets = new FuzzyCollection("Example Sets", null);
            WorkingSets1 = new FuzzyCollection("Output Sets", null);
            WorkingSets2 = new FuzzyCollection("More Output", null);

            SetupExampleSet1();

            SetupExampleSet2();

            SetupWorkingSets();
        }


        private void SetupExampleSet1()
        {         
            //Add a new Fuzzy Set to the Collection
            ExampleSets.Add("Example 1", new FuzzySet());
                      
            //Initialize the new Fuzzy Set        
            ExampleSets["Example 1"] = new FuzzySet("Example 1", 0, 100);
            ExampleSets["Example 1"].LineColour = new SolidBrush(Color.Brown);
            
            //Now, define the points in the Set
            ExampleSets["Example 1"].AddPoint(0, 0);
            ExampleSets["Example 1"].AddPoint(25, 0.8);
            ExampleSets["Example 1"].AddPoint(50, 0.8);
            ExampleSets["Example 1"].AddPoint(60, 0.3);
            ExampleSets["Example 1"].AddPoint(90, 0.3);
            ExampleSets["Example 1"].AddPoint(100, 0);

        }


        private void SetupExampleSet2()
        {
            //Add a new Fuzzy Set to the Collection
            ExampleSets.Add("Example 2", new FuzzySet());

            //Initialize the new Fuzzy Set        
            ExampleSets["Example 2"] = new FuzzySet("Example 2", 0, 100);
            ExampleSets["Example 2"].LineColour = new SolidBrush(Color.Blue);

            //Now, define the points in the Set
            ExampleSets["Example 2"].AddPoint(0, 0);
            ExampleSets["Example 2"].AddPoint(10, 1);
            ExampleSets["Example 2"].AddPoint(40, 0.2);
            ExampleSets["Example 2"].AddPoint(50, 0.1);
            ExampleSets["Example 2"].AddPoint(70, 0.1);
            ExampleSets["Example 2"].AddPoint(90, 0.5);
            ExampleSets["Example 2"].AddPoint(100, 0);
        }



        private void SetupWorkingSets()
        {
            FuzzySet scale = Operations.ScaleFS(ExampleSets["Example 1"], .3);
            scale.Id = "E1 Scaled";
            scale.LineColour = new SolidBrush(Color.DarkOrange);
            WorkingSets1.Add(scale);


            FuzzySet clip = Operations.ClipFS(ExampleSets["Example 1"], .5);
            clip.Id = "E1 Clipped";
            clip.LineColour = new SolidBrush(Color.DeepPink);
            WorkingSets1.Add(clip);

            FuzzySet not = Operations.ComplimentFS(ExampleSets["Example 1"]);
            not.Id = "E1 Compliment";
            not.LineColour = new SolidBrush(Color.Green);
            WorkingSets1.Add(not);


            FuzzySet union = Operations.UnionFS(ExampleSets["Example 1"], ExampleSets["Example 2"],
                                                new SolidBrush(Color.Red));
            union.Id = "E1 E2 Union";
            WorkingSets2.Add(union);


            FuzzySet intersect = Operations.IntersectionFS(ExampleSets["Example 1"], ExampleSets["Example 2"],
                                                        new SolidBrush(Color.Black));
            intersect.Id = "E1 E2 Intersection";
            WorkingSets2.Add(intersect);

        }

    }
}
