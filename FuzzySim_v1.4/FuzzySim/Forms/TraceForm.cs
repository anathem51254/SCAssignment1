namespace FuzzySim.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Simulators;
    using Core;

    public partial class TraceForm : Form
    {
        /// <summary>
        /// The Fuzzy Logic that is currently being used
        /// </summary>
        private AIController _sets;

        public TraceForm()
        {
            InitializeComponent();

            //This stuff should eliminate the shearing issue on Redraw
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.Scroll += (s, e) => RedrawStuff();
            this.Resize += (s, e) => RedrawStuff();
        }


        /// <summary>
        /// Callable from the outside world... 
        /// Get the FuzzySets from the Globals Cache, 
        /// Update the FuzzyRenderer's FuzzySets (which will trigger a Redraw)
        /// </summary>
        public void RedrawStuff()
        {
            GetSets(); //Sets are updated from the Globals

            if (null == _sets) return;
            
            int i = 0;

            foreach (FuzzyRenderer fuzz in this.Controls)
            {
                fuzz.FuzzySets = _sets.GetFuzzyLogic()[i]; //Update each renderer's Sets

                i++;
            }
        }


        /// <summary>
        /// Get the Sets.
        /// Draw the Sets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraceForm_Load(object sender, EventArgs e)
        {
            GetSets();
        }
        

        /// <summary>
        /// Blows away any FuzzyRenderers in the collection of Controls, and re-adds/re-draws them
        /// (Should only happen on a change of Simulator FuzzyLogic)
        /// </summary>
        private void DrawSets()
        {
            //clear the form's sets
            this.Controls.Clear();

            //The height of the FuzzyRenderers 
            int height = 120;
            
            List<FuzzyCollection> setsToDraw = _sets.GetFuzzyLogic();

            if (null == setsToDraw)
            {
                MessageBox.Show(
                    "Null object passed from the GetFuzzyLogic() method on the Controller. \n\n Check your GetFuzzyLogic() method and make sure you are returning a List<FuzzyCollection> which contains your Controller's FuzzySet Collections."
                    );
                return;
            }

            if(setsToDraw.Count < 1)
            {
                MessageBox.Show(
                    "GetFuzzyLogic() method on the Controller is returning no FuzzyCollections. \n\n Check your GetFuzzyLogic() method and make sure you are returning a List<FuzzyCollection> which contains your Controller's FuzzySet Collections."
                    );
                return;
            }

            for (int i = 0; i < setsToDraw.Count; i++)
            {
                if(setsToDraw[i] == null) continue;

                FuzzyRenderer fuzzRen = new FuzzyRenderer(setsToDraw[i].SetName, setsToDraw[i]);

                fuzzRen.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
                fuzzRen.Height = height;
                fuzzRen.Width = this.Width - 35;

                fuzzRen.Location = new Point(10, 10 + (i * height));

                this.Controls.Add(fuzzRen);
            }
        }


        /// <summary>
        /// Update the FuzzySets from the Global cache
        /// </summary>
        public void GetSets()
        {
            Type oldSets = null;

            if(_sets != null)
                oldSets = _sets.GetType(); //Scope out what sets are coming in...

            _sets = Globals.Controller; //Update the Sets from Global cache

            if(_sets != null) //Incase there have been no sets defined for the Simulator...
                if(_sets.GetType() != oldSets) //Check for Simulator Type Change
                    DrawSets(); //If they're new, draw 'em
        }
    }
}
