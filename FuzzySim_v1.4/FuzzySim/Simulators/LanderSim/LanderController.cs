namespace FuzzySim.Simulators
{
    using System.Collections.Generic;
    using System.Drawing;
    using CFLS;
    using Core;

    internal class  LanderController : AIController
    {
            /*      ---- Rememeber! ----
             * 
             *      1. Declare FuzzyCollections 
             *      2. Initialize FuzzyCollections - InitializeController()
             *      3. Populate FuzzyCollections with FuzzySets - InitializeFuzzySets() -> 'WrapperMethod'()
             *      4. Add FuzzyCollections to return collection - GetFuzzyLogic()
             */

            private SimVars.MoonLanderVars _lander;

            /// <summary>
            /// Declarations for FuzzyCollections
            /// </summary>

            //RULE SETS
            public FuzzyCollection RuleSets; 
                
            //'INPUT SETS '
            public FuzzyCollection HeightSets;
            public FuzzyCollection VelocitySets;
            public FuzzyCollection ThrottleSets;

            //'OUTPUT' SETS 
            public FuzzyCollection ThrottleOutputs;

            /// <summary>
            /// Declare private Locals
            /// </summary>
            double throttle = 0;
            double throttleL = 0;
            double throttleR = 0;


            /// <summary>
            /// Controller methods for MainForm Button Press functions
            /// </summary>
            public override void ButtonAPress() { throttle = 0; }
            public override void ButtonBPress() { throttle = 20; }
            public override void ButtonCPress() { throttle = 30; }
            public override void ButtonDPress() { throttle = 70; }
            public override void ButtonEPress() { if (throttleL == 0) throttleL = 3; else throttleL = 0;}
            public override void ButtonFPress() { if (throttleR==0) throttleR = 3; else throttleR = 0;}

            public override void ButtonRandomPress()
            {
                if (_lander != null)
                {
                    _lander.X = (double) new System.Random().Next(100, (int) Globals.Simulator.FrameResolution.X - 300);
                    _lander.Y = (double)new System.Random().Next(1800, (int)SimVars.MoonLanderVars.MAX_MOONY);
                }
            }


            public LanderController()
            {
                 //Obtain the variables affected by processing for this simulator (MoonLander)
                _lander = ((LanderSim) Globals.Simulator).SpaceShip;
            }


            public override void InitializeController()
            {
                

                SetHeightSets();

                SetVelocitySets();

                SetThrottleSets();

                SetThrottleOutputSet();

                SetRuleSets();
            }


            /// <summary>
            /// Sets up the Throttle Fuzzy Sets for the Throttle FuzzyCollection
            /// </summary>
            private void SetThrottleSets()
            {
                #region Throttle Set Definition - T_None, T_Soft, T_Medium, T_Hard

                ThrottleSets = new FuzzyCollection("Throttle Sets", null);

                ThrottleSets.Add("T_None", new FuzzySet());
                ThrottleSets["T_None"] = new FuzzySet("T_None", 0, 100);
                ThrottleSets["T_None"].LineColour = new SolidBrush(Color.Gray);
                ThrottleSets["T_None"].AddPoint(0, 1, false, false);
                ThrottleSets["T_None"].AddPoint(8, 0, false, false);
                ThrottleSets["T_None"].AddPoint(100, 0, false, false);



                #endregion
            }


            /// <summary>
            /// Sets up the Velocity Fuzzy Sets for the Velocity FuzzyCollection
            /// </summary>
            private void SetVelocitySets()
            {
                #region Velocity Set Definition



                #endregion
            }


            /// <summary>
            /// Sets up the Height Fuzzy Sets for the Height FuzzyCollection
            /// </summary>
            private void SetHeightSets()
            {
                #region Height Set Definition



                #endregion
            }


            /// <summary>
            /// Sets up the ThrottleOutput (Accumulator) Fuzzy Sets for the ThrottleOutput FuzzyCollection
            /// </summary>
            private void SetThrottleOutputSet()
            {
                ThrottleOutputs = new FuzzyCollection("Throttle Result", null);

                ThrottleOutputs.Add("ThrottleResult", new FuzzySet("ThrottleResult", 0, 100));
                ThrottleOutputs["ThrottleResult"].LineColour = new SolidBrush(Color.Black);
                ThrottleOutputs["ThrottleResult"].AddPoint(0, 0, false, false);
                ThrottleOutputs["ThrottleResult"].AddPoint(100, 0, false, false);
            }


            /// <summary>
            /// Generates empty, coloured sets for the rule-outputs to be Accumulated into
            /// 
            /// NOTE! Chane the NumRules int when you are adding more rules. Also look at:
            ///     AIController.ruleColours for a list of Colours that will be applied to the 
            ///     RuleSets
            /// 
            /// </summary>
            private void SetRuleSets()
            {
                //how many rules have been defined?
                int NumRules = 6;

                Dictionary<string, FuzzySet> rules = new Dictionary<string, FuzzySet>();

                for (int i = 0; i < NumRules; i++)
                {
                    rules.Add("Rule" + i.ToString(), new FuzzySet("Rule" + i.ToString(), 0, 100) { LineColour = ruleColours[i] });
                    rules["Rule" + i.ToString()].SetRangeWithPoints(0, 100);
                }

                RuleSets = new FuzzyCollection("Rules", rules);
            }

            /// <summary>
            /// Returns the Local FuzzyCollections 
            /// 
            ///  THIS MUST RETURN FuzzyCollections you have populated, otherwsie 
            /// the 'Show Output' window will not display your fuzzy Sets
            /// </summary>
            /// <returns></returns>
            public override List<FuzzyCollection> GetFuzzyLogic()
            {
                var ret = new List<FuzzyCollection>();

                ret.Add(new FuzzyCollection("Height Sets", HeightSets));
                ret.Add(new FuzzyCollection("Velocity Sets", VelocitySets));
                ret.Add(new FuzzyCollection("Throttle Sets", ThrottleSets));

                ret.Add(new FuzzyCollection("ThrottleOutput", ThrottleOutputs));

                ret.Add(RuleSets);

                return ret;
            }


            /// <summary>
            /// This is where the guts of it all goes down. 
            /// 
            /// Remember that because of the nature of C#, items within a collection
            /// CANNOT BE PASSED BY REFERENCE! Thus, if you are making an alteration to any 
            /// set within a method where the set is passed by reference (ref), you will
            /// need to assign the FuzzySet to a variable, pass that variable, and then set
            /// the oiginal FuzzySet within the Collection to the altered variable.
            /// A little bit of frigging around, but it works.
            /// </summary>
            public override void CalculateFuzzyLogic()
            {
                //Get the vars for this Simulator - dont alter this!
                _lander = ((LanderSim)Globals.Simulator).SpaceShip;


                //Assign the sets to variables
                FuzzySet tResult = new FuzzySet(ThrottleOutputs["ThrottleResult"]);


                //Clear the ThrottleResult Set so that we can repopulate it with new data
                tResult.Clear();
                tResult.SetRangeWithPoints(0, 100);


                //WRITE YOUR RULES LIKE THIS....
                //(dont forget to pass the 
                //accumulator variable by reference, and also pass the Rule itself
                //Hot Tip: COMMENT YOUR RULES SO YOU KNOW WHAT THEY DO!

                // Rule 0: IF Height is High THEN Throttle -> Soft 
                //RuleSets["Rule0"] = Rule.IS(_lander.Y, HeightSets["high"], ref tResult, ThrottleSets["soft"],RuleSets["Rule0"]);

                /*
                 * 
                 * Rules go here....
                 * 
                 * 
                 */

                //reassign the variables to the sets
                ThrottleOutputs["ThrottleResult"] = new FuzzySet(tResult);


                if (!Manual)
                {
                    //THIS IS THE THROTTLE VALUE SET BY PRESSING THE BUTTONS 
                    _lander.Throttle = throttle;

                    //Limit Throttle (incase of ridiculous values)
                    if (_lander.Throttle < 0)
                        _lander.Throttle = 0;

                    if (_lander.Throttle > 100)
                        _lander.Throttle = 100;


                    //IF YOU ARE WRITING RULES FOR THE SIDE-THRUST, 
                    // You will need to defuzzify into _lander.Left and _lander.Right
                    // Otherwise, leave these here to control your lander with the function buttons
                    _lander.Left = throttleL;
                    _lander.Right = throttleR;
                    
                }
                else
                {
                    //THIS IS THE THROTTLE VALUE DEFUZZIFIED BY THE RULESETS 
                    _lander.Throttle = Operations.DeFuzzifyCOG(ThrottleOutputs["ThrottleResult"]);
                }
               

                //Save the state of the Lander - dont alter this
                ((LanderSim) (Globals.Simulator)).SpaceShip = _lander;
            }
        }
    }
