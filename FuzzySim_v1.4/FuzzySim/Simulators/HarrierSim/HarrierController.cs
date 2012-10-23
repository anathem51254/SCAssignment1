using CFLS;

namespace FuzzySim.Simulators
{
    using System.Collections.Generic;
    using System.Drawing;
    using Core;

    /*      ---- Rememeber! ----
     * 
     *      1. Declare FuzzyCollections 
     *      2. Initialize FuzzyCollections - InitializeController()
     *      3. Populate FuzzyCollections with FuzzySets - InitializeFuzzySets() -> 'WrapperMethod'()
     *      4. Add FuzzyCollections to return collection - GetFuzzyLogic()
     */
    
    class HarrierController : AIController
    {
        #region Variables

        /// <summary>
        /// The variables used for this simulation (Press 'F12') on 'HarrierVars'!
        /// </summary>
        private SimVars.HarrierVars harrier;

        //RULESETS - You will need two seperate rule sets for your Throttle, and Thrust-Vector
        private FuzzyCollection RuleSetThrottle;
        private FuzzyCollection RuleSetThrustVec;

        //ACCUMULATORS
        private FuzzyCollection ThrottleAccum;
        private FuzzyCollection ThrustVecAccum;

        //FUZZY SETS
        private FuzzyCollection HeightSets;
        private FuzzyCollection DistanceSets;
        private FuzzyCollection YVelocitySets;
        private FuzzyCollection XVelocitySets;
        private FuzzyCollection ThrottleSets;
        private FuzzyCollection ThrustVecSets;

        double _throttle;
        private double _tv;

        #endregion

        #region Controls

        public override void ButtonAPress()
        {
            _throttle--;
        }
        public override void ButtonBPress()
        {
            _throttle++;
        }
        public override void ButtonCPress() { }
        public override void ButtonDPress() { }
        public override void ButtonEPress()
        {
            _tv--;
        }
        public override void ButtonFPress()
        {
            _tv++;
        }
        public override void ButtonRandomPress()
        {
            if (harrier != null)
            {
                harrier.X = (double)new System.Random().Next(5, 300);
                harrier.Y = (double)new System.Random().Next(50, 200);
            }
        }

        #endregion

        public override List<FuzzyCollection> GetFuzzyLogic()
        {
            List<FuzzyCollection> sendToOutputWindow = new List<FuzzyCollection>();

            //
            //Your Declared FuzzyCollections need to be added to 'sendToOutputWindow'
            //

            sendToOutputWindow.Add(HeightSets);
            //sendToOutputWindow.Add(DistanceSets);

            sendToOutputWindow.Add(YVelocitySets);
            //sendToOutputWindow.Add(XVelocitySets);

            sendToOutputWindow.Add(ThrottleSets);
            //sendToOutputWindow.Add(ThrustVecSets);

            sendToOutputWindow.Add(ThrottleAccum);
            sendToOutputWindow.Add(ThrustVecAccum);

            sendToOutputWindow.Add(RuleSetThrottle);
            sendToOutputWindow.Add(RuleSetThrustVec);

            return sendToOutputWindow;
        }

        public override void CalculateFuzzyLogic()
        {
            //START
            harrier = ((HarrierSim)Globals.Simulator).Harrier;

            double height;

            if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Easy) || 
                Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Medium))
                height = harrier.Y - 23;
            else
                height = harrier.Y;

            double speedY = harrier.YVel;
            double speedX = harrier.XVel;
            double safeX = harrier.X - harrier.MidSafeX - 40;

            //FuzzySet throttleOutput = ;
            //FuzzySet thrustVectorOutput = ;

            FuzzySet throttleOutput = new FuzzySet(ThrottleAccum["ThrottleOutput"]);
            FuzzySet thrustVectorOutput = new FuzzySet(ThrustVecAccum["ThrustVector"]);

            throttleOutput.Clear();
            thrustVectorOutput.Clear();
            throttleOutput.SetRangeWithPoints(0, 120);
            thrustVectorOutput.SetRangeWithPoints(-5, 5);

            //if height is high and speed is med, throttle soft		:R1
            //YourRuleSet["Rule0"] = Rule.AND(height, "Your Height Set[high]", speedY, "Your Y Speed Set[medium]", ref throttleOutput, "Your Throttle Set[soft]", YourRuleSet["Rule0"]);

            #region Easy Rules

            // if Y vel is up then throttle is no
            RuleSetThrottle["Rule0"] = Rule.IS(speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule0"]);


            // if height is high and Y vel is high then throttle is medium
            RuleSetThrottle["Rule1"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule1"]);

            // if height is high and Y vel is moderate then throttle is medium
            RuleSetThrottle["Rule2"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule2"]);

            // if height is high and Y vel is low then throttle is low
            RuleSetThrottle["Rule3"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule3"]);

            // if height is high and Y vel is safe then throttle is low
            RuleSetThrottle["Rule4"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule4"]);


            // if height is medium and Y vel is high then throttle is high
            RuleSetThrottle["Rule5"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule5"]);

            // if height is medium and Y vel is moderate then throttle is high
            RuleSetThrottle["Rule6"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule6"]);

            // if height is medium and Y vel is low then throttle is medium
            RuleSetThrottle["Rule7"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule7"]);

            // if height is medium and Y vel is safe then throttle is low
            RuleSetThrottle["Rule8"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule8"]);


            // if height is low and Y vel is high then throttle is high
            RuleSetThrottle["Rule9"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule9"]);

            // if height is low and Y vel is moderate then throttle is medium
            RuleSetThrottle["Rule10"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule10"]);

            // if height is low and Y vel is low then throttle is medium
            RuleSetThrottle["Rule11"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule11"]);

            // if height is low and Y vel is safe then throttle is low
            RuleSetThrottle["Rule12"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule12"]);


            // if height is landing and Y vel is high then throttle is high
            RuleSetThrottle["Rule13"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule13"]);

            // if height is landing and Y vel is moderate then throttle is high
            RuleSetThrottle["Rule14"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule14"]);

            // if height is landing and Y vel is low then throttle  is medium
            RuleSetThrottle["Rule15"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule15"]);

            // if height is landing and Y vel is safe then throttle is low
            RuleSetThrottle["Rule16"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule16"]);

            #endregion

            #region Medium Rules



            #endregion

            ThrottleAccum["ThrottleOutput"] = new FuzzySet(throttleOutput);
            ThrustVecAccum["ThrustVector"] = new FuzzySet(thrustVectorOutput);

            //Switch for how to adjust throttle settings ('AutoPilot' Checkbox on form)
            if (!Manual)
            {
                harrier.Throttle = _throttle;
                harrier.ThrustVector = _tv;
            }
            else
            {
               harrier.Throttle = Operations.DeFuzzifyCOG(ThrottleAccum["ThrottleOutput"]);
               harrier.ThrustVector = Operations.DeFuzzifyCOG(ThrustVecAccum["ThrustVector"]);
            }

            //THESE ARE UP TO YOU TO TUNE!!
            harrier.Throttle += 20;
            harrier.ThrustVector += 90;

            //END
            ((HarrierSim) Globals.Simulator).Harrier = harrier;
        }

        /// <summary>
        /// Initialize the controller
        /// </summary>
        public override void InitializeController()
        {
            _throttle = 60;
            _tv = 0;

            SetupHeightSets();

            //SetupDistanceSets();

            SetupThrottleSets();

            //SetupXVelocitySets();

            SetupYVelocitySets();

            //SetupThrustVecSets();

            SetupRuleSets();

            SetupAccumulators();
        }

        /// <summary>
        /// Setup Height Sets
        /// </summary>
        private void SetupHeightSets()
        {
            HeightSets = new FuzzyCollection("Height Sets", null)
            {
                new FuzzySet("Below Deck Height", -20, 400) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Landing Height", -20, 400) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Low Height", -20, 400) { LineColour = new SolidBrush(Color.Yellow) },
                new FuzzySet("Medium Height", -20, 400) { LineColour = new SolidBrush(Color.Orange) },
                new FuzzySet("High Height", -20, 400) { LineColour = new SolidBrush(Color.Red) }

            };

            HeightSets["Below Deck Height"].AddPoint(-20, 1, false, false);
            HeightSets["Below Deck Height"].AddPoint(0.5, 0, false, false);
            HeightSets["Below Deck Height"].AddPoint(400, 0, false, false);

            HeightSets["Landing Height"].AddPoint(-20, 0, false, false);
            HeightSets["Landing Height"].AddPoint(-5, 0, false, false);
            HeightSets["Landing Height"].AddPoint(0, 1, false, false);
            HeightSets["Landing Height"].AddPoint(5, 1, false, false);
            HeightSets["Landing Height"].AddPoint(10, 0, false, false);
            HeightSets["Landing Height"].AddPoint(400, 0, false, false);

            HeightSets["Low Height"].AddPoint(-20, 0, false, false);
            HeightSets["Low Height"].AddPoint(3, 0, false, false);
            HeightSets["Low Height"].AddPoint(10, 1, false, false);
            HeightSets["Low Height"].AddPoint(15, 1, false, false);
            HeightSets["Low Height"].AddPoint(20, 0, false, false);
            HeightSets["Low Height"].AddPoint(400, 0, false, false);

            HeightSets["Medium Height"].AddPoint(-20, 0, false, false);
            HeightSets["Medium Height"].AddPoint(17, 0, false, false);
            HeightSets["Medium Height"].AddPoint(40, 1, false, false);
            HeightSets["Medium Height"].AddPoint(60, 1, false, false);
            HeightSets["Medium Height"].AddPoint(90, 0, false, false);
            HeightSets["Medium Height"].AddPoint(400, 0, false, false);

            HeightSets["High Height"].AddPoint(-20, 0, false, false);
            HeightSets["High Height"].AddPoint(70, 0, false, false);
            HeightSets["High Height"].AddPoint(120, 1, false, false);
            HeightSets["High Height"].AddPoint(400, 1, false, false);

        }

        /// <summary>
        /// Setup Distance Sets
        /// </summary>
        private void SetupDistanceSets()
        {
            DistanceSets = new FuzzyCollection("Distance Sets", null)
            {
                new FuzzySet("Safe Zone", -50, 50) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Low Dangerzone", -50, 50) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Moderate Dangerzone", -50, 50) { LineColour = new SolidBrush(Color.Yellow) },
                new FuzzySet("High Dangerzone", -50, 50) { LineColour = new SolidBrush(Color.Orange) }
            };

            DistanceSets["Safe Zone"].AddPoint(-50, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(-7, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(-2, 1, false, false);
            DistanceSets["Safe Zone"].AddPoint(2, 1, false, false);
            DistanceSets["Safe Zone"].AddPoint(7, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(50, 0, false, false);

            DistanceSets["Low Dangerzone"].AddPoint(-50, 0, false, false);
            DistanceSets["Low Dangerzone"].AddPoint(0, 0, false, false);
            DistanceSets["Low Dangerzone"].AddPoint(0, 0, false, false);
            DistanceSets["Low Dangerzone"].AddPoint(0, 0, false, false);

        }

        /// <summary>
        /// Setup Y Velocity Sets
        /// </summary>
        private void SetupYVelocitySets()
        {
            YVelocitySets = new FuzzyCollection("YVelocity Sets", null)
            {
                new FuzzySet("Up Velocity", -50, 50) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Safe Velocity", -50, 50) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Low Velocity", -50, 50) { LineColour = new SolidBrush(Color.Yellow) },
                new FuzzySet("Moderate Velocity", -50, 50) { LineColour = new SolidBrush(Color.Orange) },
                new FuzzySet("High Velocity", -50, 50) { LineColour = new SolidBrush(Color.Red) }
            };

            YVelocitySets["High Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["High Velocity"].AddPoint(-12, 0, false, false);
            YVelocitySets["High Velocity"].AddPoint(-14, 1, false, false);
            YVelocitySets["High Velocity"].AddPoint(-50, 1, false, false);

            YVelocitySets["Moderate Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-8, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-11, 1, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-14, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-50, 0, false, false);

            YVelocitySets["Low Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-3, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-6, 1, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-8, 1, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-10, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-50, 0, false, false);

            YVelocitySets["Safe Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(1, 0, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(0, 1, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(-2, 1, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(-5, 0, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(-50, 0, false, false);

            YVelocitySets["Up Velocity"].AddPoint(50, 1, false, false);
            YVelocitySets["Up Velocity"].AddPoint(-1, 0, false, false);
            YVelocitySets["Up Velocity"].AddPoint(-50, 0, false, false);
        }

        /// <summary>
        /// Setup X Velocity Sets
        /// </summary>
        private void SetupXVelocitySets()
        {
            XVelocitySets = new FuzzyCollection("XVelocity Sets", null)
            {
                new FuzzySet("High Left Velocity", -20, 20) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Moderate Left Velocity", -20, 20) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Low Left Velocity", -20, 20) { LineColour = new SolidBrush(Color.Yellow) },

                new FuzzySet("Zero Velocity", -20, 20) { LineColour = new SolidBrush(Color.Orange) },

                new FuzzySet("Low Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Red) },
                new FuzzySet("Moderate Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Purple) },
                new FuzzySet("High Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Pink) },
            };

            XVelocitySets["High Left Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(-12, 0, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(-14, 1, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Moderate Left Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-8, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-11, 1, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-14, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Low Left Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-3, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-6, 1, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-8, 1, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-10, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Zero Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Zero Velocity"].AddPoint(-2, 0, false, false);
            XVelocitySets["Zero Velocity"].AddPoint(0, 1, false, false);
            XVelocitySets["Zero Velocity"].AddPoint(2, 0, false, false);
            XVelocitySets["Zero Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["Low Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(3, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(6, 1, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(8, 1, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(10, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["Moderate Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(8, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(11, 1, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(14, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["High Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["High Right Velocity"].AddPoint(12, 0, false, false);
            XVelocitySets["High Right Velocity"].AddPoint(14, 1, false, false);
            XVelocitySets["High Right Velocity"].AddPoint(20, 1, false, false);
        }

        /// <summary>
        /// Setup Throttle Sets
        /// </summary>
        private void SetupThrottleSets()
        {
            ThrottleSets = new FuzzyCollection("Throttle Sets", null)
            {
                new FuzzySet("No Throttle", 0, 120) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Hover Throttle", 0, 120) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Low Throttle", 0, 120) { LineColour = new SolidBrush(Color.Yellow) },
                new FuzzySet("Medium Throttle", 0, 120) { LineColour = new SolidBrush(Color.Orange) },
                new FuzzySet("High Throttle", 0, 120) { LineColour = new SolidBrush(Color.Red) }
            };

            ThrottleSets["No Throttle"].AddPoint(0, 1, false, false);
            ThrottleSets["No Throttle"].AddPoint(30, 0, false, false);
            ThrottleSets["No Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Low Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(20, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(35, 1, false, false);
            ThrottleSets["Low Throttle"].AddPoint(45, 1, false, false);
            ThrottleSets["Low Throttle"].AddPoint(60, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Medium Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(50, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(60, 1, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(70, 1, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(80, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Hover Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(80, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(83, 1, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(86, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["High Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["High Throttle"].AddPoint(75, 0, false, false);
            ThrottleSets["High Throttle"].AddPoint(90, 1, false, false);
            ThrottleSets["High Throttle"].AddPoint(120, 1, false, false);
        }

        /// <summary>
        /// Setup Thrust Vector Sets
        /// </summary>
        private void SetupThrustVecSets()
        {
            ThrustVecSets = new FuzzyCollection("Thrust Vector Sets", null)
            {
                new FuzzySet("Forward High Thrust", -5, 5) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Forward Moderate Thrust", -5, 5) { LineColour = new SolidBrush(Color.Green) },

                new FuzzySet("No Thrust", -5, 5) { LineColour = new SolidBrush(Color.Yellow) },
                
                new FuzzySet("Backward Moderate Thrust", -5, 5) { LineColour = new SolidBrush(Color.Orange) },
                new FuzzySet("Backward High Thrust", -5, 5) { LineColour = new SolidBrush(Color.Red) }
            };

            ThrustVecSets["Forward High Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Forward High Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["Forward High Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Forward Moderate Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["No Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["No Thrust"].AddPoint(-1, 0, false, false);
            ThrustVecSets["No Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["No Thrust"].AddPoint(1, 0, false, false);
            ThrustVecSets["No Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Backward Moderate Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Backward High Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Backward High Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["Backward High Thrust"].AddPoint(5, 0, false, false);
        }

        /// <summary>
        /// Set up Throttle and ThrustVector Output sets 
        /// </summary>
        private void SetupAccumulators()
        {
            //Throttle Accumulator
            ThrottleAccum = new FuzzyCollection("Throttle Output", null)
                                {
                                    new FuzzySet("ThrottleOutput", 0, 120)
                                };
            ThrottleAccum["ThrottleOutput"].SetRangeWithPoints(0, 120);


            //ThrustVector Accumulator
            ThrustVecAccum = new FuzzyCollection("Thrust Vector Output", null)
                                 {
                                     new FuzzySet("ThrustVector", -5, 5)
                                 };
            ThrustVecAccum["ThrustVector"].SetRangeWithPoints(-5, 5);
        }//accumulators are now ready to use

        /// <summary>
        /// Initializes and sets up Rule Sets
        /// </summary>
        private void SetupRuleSets()
        {
            RuleSetThrottle = new FuzzyCollection("Throttle Rules", null);
            RuleSetThrustVec = new FuzzyCollection("Thrust Vector Rules", null);

            // Changes these as you add rules....
            int tRules = 17;
            int tvRules = 10;

            for (int i = 0; i < tRules; i++)
            {
                RuleSetThrottle.Add(new FuzzySet("Rule" + i.ToString(), 0, 120) { LineColour = ruleColours[i] });
                RuleSetThrottle["Rule" + i.ToString()].SetRangeWithPoints(0, 120);
            }

            for (int i = 0; i < tvRules; i++)
            {
                RuleSetThrustVec.Add(new FuzzySet("Rule" + i.ToString(), 0, 100) { LineColour = ruleColours[i] });
                RuleSetThrustVec["Rule" + i.ToString()].SetRangeWithPoints(-5, 5);
            }
        }// Rulesets are now ready to use
    }
}
