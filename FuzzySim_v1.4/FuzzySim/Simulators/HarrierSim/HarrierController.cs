﻿using CFLS;

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
            sendToOutputWindow.Add(DistanceSets);

            sendToOutputWindow.Add(YVelocitySets);
            sendToOutputWindow.Add(XVelocitySets);

            sendToOutputWindow.Add(ThrottleSets);
            sendToOutputWindow.Add(ThrustVecSets);

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

            if (    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Easy)     || 
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Medium)   ||
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Hard)     )
                height = harrier.Y - 23;
            else
                height = harrier.Y - 5;

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

            #region Throttle Rules

            if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Hard) ||
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.VeryHard))
            {

                #region Y Velocity Height Rules

                // if Y vel is up then throttle is no
                RuleSetThrottle["Rule0"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule0"]);

                // if height is high and Y vel is high then throttle is medium
                RuleSetThrottle["Rule1"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule1"]);

                // if height is high and Y vel is moderate then throttle is medium
                RuleSetThrottle["Rule2"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule2"]);

                // if height is high and Y vel is low then throttle is low
                RuleSetThrottle["Rule3"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule3"]);

                // if height is high and Y vel is safe then throttle is low
                RuleSetThrottle["Rule4"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule4"]);


                // if height is medium and Y vel is high then throttle is high
                RuleSetThrottle["Rule5"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule5"]);

                // if height is medium and Y vel is moderate then throttle is high
                RuleSetThrottle["Rule6"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule6"]);

                // if height is medium and Y vel is low then throttle is medium
                RuleSetThrottle["Rule7"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule7"]);

                // if height is medium and Y vel is safe then throttle is low
                RuleSetThrottle["Rule8"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule8"]);


                // if height is low and Y vel is high then throttle is high
                RuleSetThrottle["Rule9"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule9"]);

                // if height is low and Y vel is moderate then throttle is medium
                RuleSetThrottle["Rule10"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule10"]);

                // if height is low and Y vel is low then throttle is medium
                RuleSetThrottle["Rule11"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule11"]);

                // if height is low and Y vel is safe then throttle is low
                RuleSetThrottle["Rule12"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule12"]);


                // if height is landing and Y vel is high then throttle is high
                RuleSetThrottle["Rule13"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule13"]);

                // if height is landing and Y vel is moderate then throttle is high
                RuleSetThrottle["Rule14"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule14"]);

                // if height is landing and Y vel is low then throttle  is medium
                RuleSetThrottle["Rule15"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule15"]);

                // if height is landing and Y vel is safe then throttle is low
                RuleSetThrottle["Rule16"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule16"]);


                RuleSetThrottle["Rule17"] = Rule.IS(height + 23, HeightSets["Below Deck Height"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule17"]);

                RuleSetThrottle["Rule18"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule18"]);

                RuleSetThrottle["Rule19"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule19"]);

                RuleSetThrottle["Rule20"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule20"]);

                #endregion

            }

            if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Easy) ||
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Medium))
            {

                #region Y Velocity Height Rules

                // if Y vel is up then throttle is no
                RuleSetThrottle["Rule0"] = Rule.AND(height, HeightSets["High Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule0"]);

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
                RuleSetThrottle["Rule8"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule8"]);


                // if height is low and Y vel is high then throttle is high
                RuleSetThrottle["Rule9"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule9"]);

                // if height is low and Y vel is moderate then throttle is medium
                RuleSetThrottle["Rule10"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule10"]);

                // if height is low and Y vel is low then throttle is medium
                RuleSetThrottle["Rule11"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule11"]);

                // if height is low and Y vel is safe then throttle is low
                RuleSetThrottle["Rule12"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule12"]);


                // if height is landing and Y vel is high then throttle is high
                RuleSetThrottle["Rule13"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["High Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule13"]);

                // if height is landing and Y vel is moderate then throttle is high
                RuleSetThrottle["Rule14"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Moderate Velocity"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule14"]);

                // if height is landing and Y vel is low then throttle  is medium
                RuleSetThrottle["Rule15"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Low Velocity"], ref throttleOutput, ThrottleSets["Medium Throttle"], RuleSetThrottle["Rule15"]);

                // if height is landing and Y vel is safe then throttle is low
                RuleSetThrottle["Rule16"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Safe Velocity"], ref throttleOutput, ThrottleSets["Low Throttle"], RuleSetThrottle["Rule16"]);


                RuleSetThrottle["Rule17"] = Rule.IS(height + 23, HeightSets["Below Deck Height"], ref throttleOutput, ThrottleSets["High Throttle"], RuleSetThrottle["Rule17"]);

                RuleSetThrottle["Rule18"] = Rule.AND(height, HeightSets["Medium Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule18"]);

                RuleSetThrottle["Rule19"] = Rule.AND(height, HeightSets["Low Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule19"]);

                RuleSetThrottle["Rule20"] = Rule.AND(height, HeightSets["Landing Height"], speedY, YVelocitySets["Up Velocity"], ref throttleOutput, ThrottleSets["No Throttle"], RuleSetThrottle["Rule20"]);

                #endregion

            }

            #endregion
            
            #region Thrust Vector Rules

            if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Hard) ||
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.VeryHard))
            {

                #region  X Velocity Left Dangerzone Rules

                #region Rule 0: if distance is left high dangerzone then thrust vector is forward high thrust

                RuleSetThrustVec["Rule0"] = Rule.IS(safeX, DistanceSets["Left High Dangerzone"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule0"]);


                // if distance is left high dangerzone and X vel is high left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule0"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule0"]);

                // if distance is left high dangerzone and X vel is moderate left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule1"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule1"]);

                // if distance is left high dangerzone and X vel is Low left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule2"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule3"]);

                // if distance is left high dangerzone and X vel is Neutral then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule3"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule3"]);


                #endregion

                // if distance is left moderate dangerzone and X vel is high left then thrust vector is forward high thrust
                RuleSetThrustVec["Rule1"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule1"]);

                // if distance is left moderate dangerzone and X vel is moderate left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule2"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule2"]);

                // if distance is left moderate dangerzone and X vel is low left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule3"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule3"]);

                // if distance is left moderate dangerzone and X vel is neutral then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule4"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule4"]);


                // if distance is left low dangerzone and X vel is high left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule5"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule5"]);

                // if distance is left low dangerzone and X vel is moderate left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule6"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule6"]);

                // if distance is left low dangerzone and X vel is low left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule7"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule7"]);

                // if distance is left low dangerzone and X vel is high left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule8"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule8"]);

                #endregion

                #region X Velocity Safezone Rules

                // if distance is safe zone and X vel is high left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule9"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule9"]);

                // if distance is safe zone and X vel is moderate left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule10"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule10"]);

                // if distance is safe zone and X vel is low left then thrust vector is forward low thrust
                RuleSetThrustVec["Rule11"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule11"]);


                // if distance is safe zone and X vel is neutral then thrust vector is neutral thrust
                RuleSetThrustVec["Rule12"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Neutral Thrust"], RuleSetThrustVec["Rule12"]);


                // if distance is safe zone and X vel is low right then thrust vector is backward low thrust
                RuleSetThrustVec["Rule13"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule13"]);

                // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule14"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule14"]);

                // if distance is safe zone and X vel is high right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule15"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule15"]);


                // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                //RuleSetThrustVec[""] = Rule.AND(safeX, DistanceSets[""], speedX, XVelocitySets[""], ref thrustVectorOutput, ThrustVecSets[""], RuleSetThrustVec[""]);

                #endregion

                #region X Velocity Right Dangerzone Rules

                #region Rule 16: if distance is right high dangerzone then thrust vector is backward high thrust

                RuleSetThrustVec["Rule16"] = Rule.IS(safeX, DistanceSets["Right High Dangerzone"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule16"]);

                /*
                // if distance is left high dangerzone and X vel is high left then thrust vector is forward high thrust
                RuleSetThrustVec["Rule0"] = Rule.AND(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule0"]);

                // if distance is left high dangerzone and X vel is moderate left then thrust vector is forward high thrust
                RuleSetThrustVec["Rule1"] = Rule.AND(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule1"]);

                // if distance is left high dangerzone and X vel is Low left then thrust vector is forward high thrust
                RuleSetThrustVec["Rule2"] = Rule.AND(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule3"]);

                // if distance is left high dangerzone and X vel is Neutral then thrust vector is forward high thrust
                RuleSetThrustVec["Rule3"] = Rule.AND(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule3"]);
                */

                #endregion

                // if distance is right moderate dangerzone and X vel is high right then thrust vector is backward high thrust
                RuleSetThrustVec["Rule17"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule17"]);

                // if distance is right moderate dangerzone and X vel is moderate right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule18"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule18"]);

                // if distance is right moderate dangerzone and X vel is low right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule19"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule19"]);

                // if distance is right moderate dangerzone and X vel is neutral then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule20"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule20"]);


                // if distance is right low dangerzone and X vel is high right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule21"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule21"]);

                // if distance is right low dangerzone and X vel is moderate right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule22"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule22"]);

                // if distance is right low dangerzone and X vel is low right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule23"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule23"]);

                // if distance is right low dangerzone and X vel is high right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule24"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule24"]);

                #endregion

            }

            if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Easy) ||
                    Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Medium))
            {

                #region  X Velocity Left Dangerzone Rules

                #region Rule 0: if distance is left high dangerzone then thrust vector is forward high thrust

                RuleSetThrustVec["Rule0"] = Rule.IS(safeX, DistanceSets["Left High Dangerzone"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule0"]);


                //// if distance is left high dangerzone and X vel is high left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule0"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule0"]);

                //// if distance is left high dangerzone and X vel is moderate left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule29"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule29"]);

                //// if distance is left high dangerzone and X vel is Low left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule30"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule30"]);

                //// if distance is left high dangerzone and X vel is Neutral then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule31"] = Rule.OR(safeX, DistanceSets["Left High Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule31"]);


                #endregion

                // if distance is left moderate dangerzone and X vel is high left then thrust vector is forward high thrust
                RuleSetThrustVec["Rule1"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule1"]);

                // if distance is left moderate dangerzone and X vel is moderate left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule2"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule2"]);

                // if distance is left moderate dangerzone and X vel is low left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule3"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule3"]);

                // if distance is left moderate dangerzone and X vel is neutral then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule4"] = Rule.AND(safeX, DistanceSets["Left Moderate Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule4"]);


                // if distance is left low dangerzone and X vel is high left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule5"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule5"]);

                // if distance is left low dangerzone and X vel is moderate left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule6"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule6"]);

                // if distance is left low dangerzone and X vel is low left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule7"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule7"]);

                // if distance is left low dangerzone and X vel is high left then thrust vector is forward moderate thrust
                RuleSetThrustVec["Rule8"] = Rule.AND(safeX, DistanceSets["Left Low Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule8"]);

                #endregion

                #region X Velocity Safezone Rules

                if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Easy))
                {

                    // if distance is safe zone and X vel is high left then thrust vector is forward moderate thrust
                    RuleSetThrustVec["Rule9"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule9"]);

                    // if distance is safe zone and X vel is moderate left then thrust vector is forward moderate thrust
                    RuleSetThrustVec["Rule10"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule10"]);

                    // if distance is safe zone and X vel is low left then thrust vector is forward low thrust
                    RuleSetThrustVec["Rule11"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule11"]);


                    // if distance is safe zone and X vel is neutral then thrust vector is neutral thrust
                    RuleSetThrustVec["Rule12"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Neutral Thrust"], RuleSetThrustVec["Rule12"]);


                    // if distance is safe zone and X vel is low right then thrust vector is backward low thrust
                    RuleSetThrustVec["Rule13"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule13"]);

                    // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                    RuleSetThrustVec["Rule14"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule14"]);

                    // if distance is safe zone and X vel is high right then thrust vector is backward moderate thrust
                    RuleSetThrustVec["Rule15"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule15"]);


                    // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                    //RuleSetThrustVec[""] = Rule.AND(safeX, DistanceSets[""], speedX, XVelocitySets[""], ref thrustVectorOutput, ThrustVecSets[""], RuleSetThrustVec[""]);
                }

                if (Globals.Simulator.Difficulty.Equals(SimDifficultyEnum.Medium))
                {

                    // if distance is safe zone and X vel is high left then thrust vector is forward moderate thrust
                    RuleSetThrustVec["Rule9"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward High Thrust"], RuleSetThrustVec["Rule9"]);

                    // if distance is safe zone and X vel is moderate left then thrust vector is forward moderate thrust
                    RuleSetThrustVec["Rule10"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Moderate Thrust"], RuleSetThrustVec["Rule10"]);

                    // if distance is safe zone and X vel is low left then thrust vector is forward low thrust
                    RuleSetThrustVec["Rule11"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Forward Low Thrust"], RuleSetThrustVec["Rule11"]);


                    // if distance is safe zone and X vel is neutral then thrust vector is neutral thrust
                    RuleSetThrustVec["Rule12"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Neutral Thrust"], RuleSetThrustVec["Rule12"]);


                    // if distance is safe zone and X vel is low right then thrust vector is backward low thrust
                    RuleSetThrustVec["Rule13"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule13"]);

                    // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                    RuleSetThrustVec["Rule14"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule14"]);

                    // if distance is safe zone and X vel is high right then thrust vector is backward moderate thrust
                    RuleSetThrustVec["Rule15"] = Rule.AND(safeX, DistanceSets["Safe Zone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule15"]);


                    // if distance is safe zone and X vel is moderate right then thrust vector is backward moderate thrust
                    //RuleSetThrustVec[""] = Rule.AND(safeX, DistanceSets[""], speedX, XVelocitySets[""], ref thrustVectorOutput, ThrustVecSets[""], RuleSetThrustVec[""]);
                }

                #endregion

                #region X Velocity Right Dangerzone Rules

                #region Rule 16: if distance is right high dangerzone then thrust vector is backward high thrust

                RuleSetThrustVec["Rule16"] = Rule.IS(safeX, DistanceSets["Right High Dangerzone"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule16"]);

                
                //// if distance is left high dangerzone and X vel is high left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule25"] = Rule.AND(safeX, DistanceSets["Right High Dangerzone"], speedX, XVelocitySets["High Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule25"]);

                //// if distance is left high dangerzone and X vel is moderate left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule26"] = Rule.AND(safeX, DistanceSets["Right High Dangerzone"], speedX, XVelocitySets["Moderate Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule26"]);

                //// if distance is left high dangerzone and X vel is Low left then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule27"] = Rule.AND(safeX, DistanceSets["Right High Dangerzone"], speedX, XVelocitySets["Low Left Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule27"]);

                //// if distance is left high dangerzone and X vel is Neutral then thrust vector is forward high thrust
                //RuleSetThrustVec["Rule28"] = Rule.AND(safeX, DistanceSets["Right High Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule28"]);
                

                #endregion

                // if distance is right moderate dangerzone and X vel is high right then thrust vector is backward high thrust
                RuleSetThrustVec["Rule17"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward High Thrust"], RuleSetThrustVec["Rule17"]);

                // if distance is right moderate dangerzone and X vel is moderate right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule18"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule18"]);

                // if distance is right moderate dangerzone and X vel is low right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule19"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule19"]);

                // if distance is right moderate dangerzone and X vel is neutral then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule20"] = Rule.AND(safeX, DistanceSets["Right Moderate Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule20"]);


                // if distance is right low dangerzone and X vel is high right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule21"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["High Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule21"]);

                // if distance is right low dangerzone and X vel is moderate right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule22"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Moderate Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Moderate Thrust"], RuleSetThrustVec["Rule22"]);

                // if distance is right low dangerzone and X vel is low right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule23"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Low Right Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule23"]);

                // if distance is right low dangerzone and X vel is high right then thrust vector is backward moderate thrust
                RuleSetThrustVec["Rule24"] = Rule.AND(safeX, DistanceSets["Right Low Dangerzone"], speedX, XVelocitySets["Neutral Velocity"], ref thrustVectorOutput, ThrustVecSets["Backward Low Thrust"], RuleSetThrustVec["Rule24"]);

                #endregion

            }

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

            SetupDistanceSets();

            SetupThrottleSets();

            SetupXVelocitySets();

            SetupYVelocitySets();

            SetupThrustVecSets();

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
            HeightSets["Below Deck Height"].AddPoint(-2, 1, false, false);
            HeightSets["Below Deck Height"].AddPoint(5, 0, false, false);
            HeightSets["Below Deck Height"].AddPoint(400, 0, false, false);

            HeightSets["Landing Height"].AddPoint(-20, 0, false, false);
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
                new FuzzySet("Left High Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Blue) },
                new FuzzySet("Left Moderate Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Green) },
                new FuzzySet("Left Low Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Yellow) },

                new FuzzySet("Safe Zone", -1000, 1000) { LineColour = new SolidBrush(Color.Orange) },

                new FuzzySet("Right Low Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Red) },
                new FuzzySet("Right Moderate Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Purple) },
                new FuzzySet("Right High Dangerzone", -1000, 1000) { LineColour = new SolidBrush(Color.Pink) }
            };

            DistanceSets["Left High Dangerzone"].AddPoint(-1000, 1, false, false);
            DistanceSets["Left High Dangerzone"].AddPoint(-150, 1, false, false);
            DistanceSets["Left High Dangerzone"].AddPoint(-130, 0, false, false);
            DistanceSets["Left High Dangerzone"].AddPoint(1000, 0, false, false);

            DistanceSets["Left Moderate Dangerzone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Left Moderate Dangerzone"].AddPoint(-140, 0, false, false);
            DistanceSets["Left Moderate Dangerzone"].AddPoint(-120, 1, false, false);
            DistanceSets["Left Moderate Dangerzone"].AddPoint(-100, 1, false, false);
            DistanceSets["Left Moderate Dangerzone"].AddPoint(-80, 0, false, false);
            DistanceSets["Left Moderate Dangerzone"].AddPoint(1000, 0, false, false);

            DistanceSets["Left Low Dangerzone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Left Low Dangerzone"].AddPoint(-100, 0, false, false);
            DistanceSets["Left Low Dangerzone"].AddPoint(-80, 1, false, false);
            DistanceSets["Left Low Dangerzone"].AddPoint(-60, 1, false, false);
            DistanceSets["Left Low Dangerzone"].AddPoint(-50, 0, false, false);
            DistanceSets["Left Low Dangerzone"].AddPoint(1000, 0, false, false);

            DistanceSets["Safe Zone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(-70, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(-50, 1, false, false);
            DistanceSets["Safe Zone"].AddPoint(50, 1, false, false);
            DistanceSets["Safe Zone"].AddPoint(70, 0, false, false);
            DistanceSets["Safe Zone"].AddPoint(1000, 0, false, false);

            DistanceSets["Right Low Dangerzone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Right Low Dangerzone"].AddPoint(50, 0, false, false);
            DistanceSets["Right Low Dangerzone"].AddPoint(60, 1, false, false);
            DistanceSets["Right Low Dangerzone"].AddPoint(80, 1, false, false);
            DistanceSets["Right Low Dangerzone"].AddPoint(100, 0, false, false);
            DistanceSets["Right Low Dangerzone"].AddPoint(1000, 0, false, false);

            DistanceSets["Right Moderate Dangerzone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Right Moderate Dangerzone"].AddPoint(80, 0, false, false);
            DistanceSets["Right Moderate Dangerzone"].AddPoint(100, 1, false, false);
            DistanceSets["Right Moderate Dangerzone"].AddPoint(120, 1, false, false);
            DistanceSets["Right Moderate Dangerzone"].AddPoint(140, 0, false, false);
            DistanceSets["Right Moderate Dangerzone"].AddPoint(1000, 0, false, false);

            DistanceSets["Right High Dangerzone"].AddPoint(-1000, 0, false, false);
            DistanceSets["Right High Dangerzone"].AddPoint(140, 0, false, false);
            DistanceSets["Right High Dangerzone"].AddPoint(150, 1, false, false);
            DistanceSets["Right High Dangerzone"].AddPoint(1000, 1, false, false);
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
            YVelocitySets["High Velocity"].AddPoint(-9, 0, false, false);
            YVelocitySets["High Velocity"].AddPoint(-12, 1, false, false);
            YVelocitySets["High Velocity"].AddPoint(-50, 1, false, false);

            YVelocitySets["Moderate Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-6, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-8, 1, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-10, 0, false, false);
            YVelocitySets["Moderate Velocity"].AddPoint(-50, 0, false, false);

            YVelocitySets["Low Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-1, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-3, 1, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-5, 1, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-7, 0, false, false);
            YVelocitySets["Low Velocity"].AddPoint(-50, 0, false, false);

            YVelocitySets["Safe Velocity"].AddPoint(50, 0, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(1, 0, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(0, 1, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(-1, 1, false, false);
            YVelocitySets["Safe Velocity"].AddPoint(-2, 0, false, false);
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

                new FuzzySet("Neutral Velocity", -20, 20) { LineColour = new SolidBrush(Color.Orange) },

                new FuzzySet("Low Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Red) },
                new FuzzySet("Moderate Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Purple) },
                new FuzzySet("High Right Velocity", -20, 20) { LineColour = new SolidBrush(Color.Pink) },
            };

            XVelocitySets["High Left Velocity"].AddPoint(-20, 1, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(-15, 1, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(-12, 0, false, false);
            XVelocitySets["High Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Moderate Left Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-13, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-10, 1, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(-7, 0, false, false);
            XVelocitySets["Moderate Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Low Left Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-8, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-5, 1, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(-1, 0, false, false);
            XVelocitySets["Low Left Velocity"].AddPoint(20, 0, false, false);
            
            XVelocitySets["Neutral Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Neutral Velocity"].AddPoint(-2, 0, false, false);
            XVelocitySets["Neutral Velocity"].AddPoint(0, 1, false, false);
            XVelocitySets["Neutral Velocity"].AddPoint(2, 0, false, false);
            XVelocitySets["Neutral Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["Low Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(1, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(5, 1, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(8, 0, false, false);
            XVelocitySets["Low Right Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["Moderate Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(7, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(10, 1, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(13, 0, false, false);
            XVelocitySets["Moderate Right Velocity"].AddPoint(20, 0, false, false);

            XVelocitySets["High Right Velocity"].AddPoint(-20, 0, false, false);
            XVelocitySets["High Right Velocity"].AddPoint(12, 0, false, false);
            XVelocitySets["High Right Velocity"].AddPoint(15, 1, false, false);
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
            ThrottleSets["No Throttle"].AddPoint(10, 1, false, false);
            ThrottleSets["No Throttle"].AddPoint(35, 0, false, false);
            ThrottleSets["No Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Low Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(30, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(45, 1, false, false);
            ThrottleSets["Low Throttle"].AddPoint(60, 1, false, false);
            ThrottleSets["Low Throttle"].AddPoint(70, 0, false, false);
            ThrottleSets["Low Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Medium Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(60, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(70, 1, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(90, 1, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(95, 0, false, false);
            ThrottleSets["Medium Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["Hover Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(80, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(83, 1, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(86, 0, false, false);
            ThrottleSets["Hover Throttle"].AddPoint(120, 0, false, false);

            ThrottleSets["High Throttle"].AddPoint(0, 0, false, false);
            ThrottleSets["High Throttle"].AddPoint(90, 0, false, false);
            ThrottleSets["High Throttle"].AddPoint(100, 1, false, false);
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
                new FuzzySet("Forward Low Thrust", -5, 5) { LineColour = new SolidBrush(Color.Yellow) },

                new FuzzySet("Neutral Thrust", -5, 5) { LineColour = new SolidBrush(Color.Orange) },
                
                new FuzzySet("Backward Low Thrust", -5, 5) { LineColour = new SolidBrush(Color.Red) },
                new FuzzySet("Backward Moderate Thrust", -5, 5) { LineColour = new SolidBrush(Color.Purple) },
                new FuzzySet("Backward High Thrust", -5, 5) { LineColour = new SolidBrush(Color.Pink) }
            };

            ThrustVecSets["Forward High Thrust"].AddPoint(-5, 1, false, false);
            ThrustVecSets["Forward High Thrust"].AddPoint(-4, 1, false, false);
            ThrustVecSets["Forward High Thrust"].AddPoint(-3.5, 0, false, false);
            ThrustVecSets["Forward High Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Forward Moderate Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(-4, 0, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(-3, 1, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(-2, 0, false, false);
            ThrustVecSets["Forward Moderate Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Forward Low Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Forward Low Thrust"].AddPoint(-2.5, 0, false, false);
            ThrustVecSets["Forward Low Thrust"].AddPoint(-1.5, 1, false, false);
            ThrustVecSets["Forward Low Thrust"].AddPoint(-0.5, 0, false, false);
            ThrustVecSets["Forward Low Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Neutral Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Neutral Thrust"].AddPoint(-1, 0, false, false);
            ThrustVecSets["Neutral Thrust"].AddPoint(0, 1, false, false);
            ThrustVecSets["Neutral Thrust"].AddPoint(1, 0, false, false);
            ThrustVecSets["Neutral Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Backward Low Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Backward Low Thrust"].AddPoint(0.5, 0, false, false);
            ThrustVecSets["Backward Low Thrust"].AddPoint(1.5, 1, false, false);
            ThrustVecSets["Backward Low Thrust"].AddPoint(2.5, 0, false, false);
            ThrustVecSets["Backward Low Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Backward Moderate Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(2, 0, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(3, 1, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(4, 0, false, false);
            ThrustVecSets["Backward Moderate Thrust"].AddPoint(5, 0, false, false);

            ThrustVecSets["Backward High Thrust"].AddPoint(-5, 0, false, false);
            ThrustVecSets["Backward High Thrust"].AddPoint(3.5, 0, false, false);
            ThrustVecSets["Backward High Thrust"].AddPoint(4, 1, false, false);
            ThrustVecSets["Backward High Thrust"].AddPoint(5, 1, false, false);
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
            int tRules = 21;
            int tvRules = 25;

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
