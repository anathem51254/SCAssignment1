using FuzzySim.Rendering;

namespace FuzzySim.Simulators
{
    using System.Collections.Generic;
    using System.Drawing;
    using Core;

    public abstract class AIController
    {
        /// <summary>
        /// Manual control toggle
        /// </summary>
        public bool Manual = true;

        /// <summary>
        /// GetFuzzyLogic is called by the fuzzy set display form
        /// you need to return a list of collections of fuzzy sets
        /// </summary>
        /// <returns></returns>
        public abstract List<FuzzyCollection> GetFuzzyLogic(); 

        /// <summary>
        /// Implements the rules written for the simulator
        /// Runs once in the text simulator
        /// Runs repeatedly in the Lander simulator
        /// </summary>
        public abstract void CalculateFuzzyLogic();


        /// <summary>
        /// Initializes the User-Defined Controller
        /// In here students should put their fuzzy set definitions
        /// </summary>
        public abstract void InitializeController();
        
        /// <summary>
        /// This collection just assigns an arbirtary colour to a Rule as they are Generated in the FOR loop... 
        /// </summary>
        internal Brush[] ruleColours = new Brush[]
                                                {
                                                    new SolidBrush(Color.Blue),             //1
                                                    new SolidBrush(Color.GreenYellow),
                                                    new SolidBrush(Color.OrangeRed),
                                                    new SolidBrush(Color.Turquoise),
                                                    new SolidBrush(Color.PaleVioletRed),
                                                    new SolidBrush(Color.SaddleBrown),
                                                    new SolidBrush(Color.SeaGreen),
                                                    new SolidBrush(Color.Fuchsia),
                                                    new SolidBrush(Color.Black),     
                                                    new SolidBrush(Color.DarkKhaki),        //10
                                                    new SolidBrush(Color.BurlyWood),
                                                    new SolidBrush(Color.PaleVioletRed),
                                                    new SolidBrush(Color.DarkRed),
                                                    new SolidBrush(Color.DeepSkyBlue),
                                                    new SolidBrush(Color.DimGray),
                                                    new SolidBrush(Color.ForestGreen),
                                                    new SolidBrush(Color.Gainsboro),
                                                    new SolidBrush(Color.CornflowerBlue),
                                                    new SolidBrush(Color.Chartreuse),
                                                    new SolidBrush(Color.Crimson),          //20
                                                    new SolidBrush(Color.Yellow),
                                                    new SolidBrush(Color.Tomato),
                                                    new SolidBrush(Color.SpringGreen),
                                                    new SolidBrush(Color.Teal),
                                                    new SolidBrush(Color.BlueViolet),
                                                    new SolidBrush(Color.Red),
                                                    new SolidBrush(Color.Green),
                                                    new SolidBrush(Color.Orange),
                                                    new SolidBrush(Color.HotPink),
                                                    new SolidBrush(Color.Lavender)          //30
                                                                //...and so on (for however many rules there are)
                                                };

        public virtual void ButtonAPress() {}
        public virtual void ButtonBPress() {}
        public virtual void ButtonCPress() {}
        public virtual void ButtonDPress() {}
        public virtual void ButtonEPress() {}
        public virtual void ButtonFPress() {}
        public virtual void ButtonRandomPress() { }

   
    }
}
