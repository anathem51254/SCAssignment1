namespace FuzzySim
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using CFLS;
    using Rendering;
    using Core;

    /// <summary>
    /// The Usercontrol which displays an array of FuzzySets on a Canvas
    /// </summary>
    public partial class FuzzyRenderer : UserControl
    {
        /// <summary>
        /// The collection of FuzzySets
        /// </summary>
        private FuzzyCollection fuzzySets;

        private string setName;
        /// <summary>
        /// The Collection of Sets' Name
        /// </summary>
        public string SetName
        {
            get { return setName; }
            set 
            {
                setName = value;
                lblSetName.Text = SetName;
            }
        }

        /// <summary>
        /// The collection of FuzzySets
        /// Updating the Sets will trigger a Redraw.
        /// </summary>
        public FuzzyCollection FuzzySets
        {
            get { return fuzzySets; }
            set
            {
                fuzzySets = value;
                
                if(fuzzySets != null) 
                    Redraw();
            }
        }

        private FuzzySetFrame CurrentFrame;
        private List<string> Errors;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FuzzyRenderer()
        {
            InitializeComponent(); //Always first!
            
            FuzzySets = new FuzzyCollection("", null); // THIS WAS CHANGED FROM THE ORIGINAL IMPLEMENTATION OF AN ARRAY (Restricting, painful nullchecks in foreach loops...) :TF
            Initialize();
        }

        /// <summary>
        /// Constructor: [name]
        /// </summary>
        /// <param name="setName">The Set Name</param>
        public FuzzyRenderer(string setName, FuzzyCollection fuzzySets)
        {
            InitializeComponent(); //Always....

            FuzzySets = fuzzySets;

            Initialize();

            SetName = setName;
        }

        
        private void Initialize()
        {
            Errors = new List<string>();
            
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            
            pbxCanvas.Paint += new PaintEventHandler(pbxCanvas_Paint); //How to paint the pictureBox

            Resize += (s, e) => pbxCanvas.Refresh();  //Redraw on Resize

            lbxSetNames.View = View.List;

            ProcessCurrentFrame();

            UpdateSetText();
        }


        /// <summary>
        /// Accesses the FuzzySets in this Renderer's Collection and pulls a new FuzzySetFrame from them
        /// </summary>
        private void ProcessCurrentFrame()
        {
            //Sets the CurrentFrame to the Rendered values of the FuzzySets
            CurrentFrame = new FuzzySetFrame(new List<FuzzySet>(FuzzySets.Values.ToArray()));
        }

        /// <summary>
        /// Allows Parent controls to call for the canvas to be redrawn
        /// </summary>
        public void Redraw()
        {
            ProcessCurrentFrame();

            //Draws the CurrentFrame
            pbxCanvas.Refresh();
        }

        /// <summary>
        /// Adds The FuzzySet's ID's to the  
        /// </summary>
        private void UpdateSetText()
        {
            lbxSetNames.Items.Clear();

            int i = 0;

            foreach (FuzzySet f in FuzzySets.Values)
            {
                ListViewItem item = new ListViewItem();
                try
                {
                    item.Text = String.Format("{0}", f.Id);
                    item.ForeColor = new Pen(f.LineColour).Color;
                    
                    lbxSetNames.Items.Insert(i, item);
                }
                catch (Exception)
                {
                    item.Text = String.Format("{0}", f.Id);

                    if (f.LineColour == null)
                        item.ForeColor = Color.Black;

                    lbxSetNames.Items.Insert(i, item);
                }
                i++;
            }
        }  

        /// <summary>
        /// Happens when .Refresh() is called on the pictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbxCanvas_Paint(object sender, PaintEventArgs e)
        {
            ValidateFuzzySets();

            DrawFuzzySet(e);
        }

        /// <summary>
        /// Check if the FuzzySets contain any errors - if so, Display those errors instead
        /// </summary>
        private void ValidateFuzzySets()
        {
            Errors.Clear();

            if (FuzzySets == null)
            {
                Errors.Add("No Fuzzy Set to render!\n");
                return;
            }
            else
            {
                try
                {
                    double standardHighRange = FuzzySets.First().Value.GetHighRange();

                    foreach (FuzzySet set in FuzzySets.Values)
                    {
                        if (set.GetHighRange() != standardHighRange)
                            Errors.Add(CFLS.Errors.CFLS_errortext[11]);
                        if (set.GetHighRange() > 10000)
                            Errors.Add(CFLS.Errors.CFLS_errortext[1]);
                        if (set.GetNumPoints() < 2)
                            Errors.Add(CFLS.Errors.CFLS_errortext[6]);
                        if (set.Validate() != "")
                            Errors.Add(set.Validate());
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
                
            }
        }


        /// <summary>
        /// Testing this shiz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxCanvas_Click(object sender, EventArgs e)
        {
            //may as well just redraw now...
            Redraw();
        }

        /// <summary>
        /// Draw to Control which needs to be painted
        /// </summary>
        /// <param name="e">Incoming Graphics object</param>
        private void DrawFuzzySet(PaintEventArgs e)
        {
            CurrentFrame.Draw(e.Graphics, Errors);
            pbxCanvas.BackColor = CurrentFrame.BackGround;
        }

    }
}
