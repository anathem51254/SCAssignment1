

namespace FuzzySim.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Core;
    using Rendering;
    using Simulators;

    /***********************************************************************
     * 
     *  -----   'FuzzySim'   -----   
     *  
     *  A conversion Project from C++ to C#
     * 
     *  
     *  Based off the original FuzzyLua Moon-Lander Simulator written by Robert Cox.
     *  C# reengineer and implementation by Timothy Fox, March-October 2012. * 
     * 
     * 
     * 
     */

    public partial class MainForm : Form
    {
        //Declare privates 
        TraceForm _trace;               //The FuzzySet Output Form ["Show Output" button opens this]
        Timer _timer;                   //The timer runs a Simulation itteration on each tick
        bool _pauseOnFunc = false;      //Boolen choice as to whether or not to pause timer on a function-button click
        Frame _currentFrame;            //The Current Simulation Itteration Frame [updated by calls to Simulator.Init()/.DoTurn() ]

        private FrameManager _frameManager;

        public MainForm()
        {
            //Always comes first
            InitializeComponent(); 

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeStuff();

            //CHANGE THIS TO ALTER THE INITIAL SIMULATION SELECTION
            comSimulatorSelection.SelectedIndex = 2;

            SetSimulator();

            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
        }

        /// <summary>
        /// Handles key presses at the Form level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar.ToString());
        }

      
         private void InitializeStuff()
         {
             tbrSimRate.Value = 15; //SIM RATE
             tbrFrameRate.Value = 1; //FRAME RATE

             //Set the Sim and Frame Rates
             Globals.SimRate = tbrSimRate.Value;
             Globals.DrawEvery = tbrFrameRate.Value;
             
             //Set up the Timer
            _timer = new Timer();
            _timer.Interval = 1000 / Globals.SimRate;
            _timer.Tick += new EventHandler(timer_Tick);
            
             //Load all the Sprites that will later be rendered
            SpriteList.Initialize();

             //_currentFrame is just black...
            _currentFrame = new SimFrame {BackGround = Color.Black};

             //Listens out for a resize operation (... on the Form, which the pictureBox is snapped-to)
            pictureBox1.Resize += new EventHandler(pictureBox1_Resize);

             //Draws an integer value of each Trackbar just below its slider
            RedrawLabel(lblSimRate, tbrSimRate);
            RedrawLabel(lblDrawRate, tbrFrameRate);
            
             //Lander is the Initial Simulator, by default... 
             Globals.SelectedSimulator = SimulatorsEnum.MoonLander; //This is overridden by the Selected option from the Simulators DropDownList!
             
             comDifficulty.SelectedIndex = 0;
         }

        //On a form-resize, set the FrameResolution to whatever
         void pictureBox1_Resize(object sender, EventArgs e)
         {
             if (Globals.Simulator != null)
                 Globals.Simulator.FrameResolution = new Vec2(pictureBox1.Width, pictureBox1.Height);
         }

        /// <summary>
        /// ========= TIMER TICK ============
        /// 1. DO SIMULATOR TURN
        /// 2. IF FRAME IS TO BE DRAWN, DRAW IT
        /// 3. CHECK FOR COMPLETION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (Globals.Simulator == null)
            {
                _timer.Stop();
                cbxRunning.Checked = false;
                MessageBox.Show("The selected simulator has not been defined or initialized.");                
                return;
            }

            //If not finished yet...
            if (Globals.Simulator.State != SimulatorStateEnum.Complete)
            {
                //TICK
                Globals.TurnCount++;

                //Do that good stuff
                Globals.Simulator.DoTurn();

                //Also, redraw the Fuzzy Sets (if the form is open!)
                CheckForTraceFormUpdate();
                    
            }

            //If time to draw a frame, and if frame is marked to be drawn, then draw it
            if (Globals.TurnCount % Globals.DrawEvery == 0 && _currentFrame.ToBeDrawn) 
                DrawFrame();

            //if finished, STOP the Simulation
            if (Globals.Simulator.State == SimulatorStateEnum.Complete)
            {
                ForceDrawFrame(); // If the frame rate is > than 1, _currentFrame may not be drawn... MAKE IT be drawn!
                cbxRunning.Checked = false;
            }
        }

        private void CheckForTraceFormUpdate()
        {
            if (_trace != null)
            {
                _trace.RedrawStuff(); //should tell the form to Re-get the sets, and draw 'em
            }
        }

        /// <summary>
        /// Forces an override of the FrameRate check.. Draws the CurrentFrame regardless.
        /// </summary>
        private void ForceDrawFrame()
        {
            pictureBox1.Refresh();
        }

        /// <summary>
        /// Tell the pictureBox to Draw itself again
        /// </summary>
        private void DrawFrame()
        {
            if(Globals.TurnCount % Globals.DrawEvery == 0)
                pictureBox1.Refresh();
        }

        /// <summary>
        /// Flushes the pictureBox and draws the _currentFrame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {        
            if(null == Globals.Simulator) return;
            
            //asks the Simulator to draw what it has

            //if (Globals.Simulator.State == SimulatorStateEnum.Initialised || Globals.Simulator.State == SimulatorStateEnum.Running) // 
            //    _currentFrame = Globals.Simulator.DrawTurn((SimFrame)_currentFrame); //Update existing elements
            //else
                _currentFrame = Globals.Simulator.DrawTurn(new Vec2(pictureBox1.Width, pictureBox1.Height)); //draw elements

            //Draw the _currentFrame to the paintbox's Graphics
            _currentFrame.Draw(e.Graphics);

            //Set the background to that of the _currentFrame's
            pictureBox1.BackColor = _currentFrame.BackGround;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Nothing to do here - magic happens later!
        }

        /// <summary>
        /// Change of the Simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxRunning.Checked = false;

            SetSimulator();
        }


        /// <summary>
        /// RESET BUTTON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //RESET THE SIMULATOR
            cbxControl.Checked = true;
            cbxTracePath.Checked = true;
            cbxDebug.Checked = true;
             
            SetSimulator();
        }


        /// <summary>
        /// Sets the current Simulator depending on the value of the Simulator DropDownList
        /// </summary>
        private void SetSimulator()
        {
            Globals.Reset();

            Enum.TryParse(comSimulatorSelection.Items[comSimulatorSelection.SelectedIndex].ToString(), out Globals.SelectedSimulator);
            
            switch(Globals.SelectedSimulator)
            {
                    //SET UP THE TEXT SIMULATOR
                case SimulatorsEnum.Text:   
                    Globals.Simulator = new TextSim();
                    Globals.Controller = new TextController();
                    break;


                    //SET UP THE MOON LANDER
                case SimulatorsEnum.MoonLander:
                    Globals.Simulator = new LanderSim();
                    ((LanderSim)Globals.Simulator).InitSpaceShip();
                    Globals.Controller = new LanderController();
                   
                    break;

                    //SET UP THE HARRIER SIM
                case SimulatorsEnum.Harrier:
                    Globals.Simulator = new HarrierSim();
                   
                    Globals.Controller = new HarrierController();
                    break;

                case SimulatorsEnum.Seahawk:
                        return;
                    break;

                case SimulatorsEnum.Parachute:
                    return;
                    break;

                case SimulatorsEnum.Randomizer:
                        Globals.Simulator = new RandomalitySimulator();
                    break;

            }

            if( Globals.Controller != null)
                Globals.Controller.InitializeController();

            Globals.Simulator.FrameResolution = new Vec2(pictureBox1.Width, pictureBox1.Height);

            Enum.TryParse(comDifficulty.Items[comDifficulty.SelectedIndex].ToString(), out Globals.Simulator.Difficulty);

            _currentFrame = Globals.Simulator.Init(Globals.Simulator.FrameResolution);
            DrawFrame();

            CheckForTraceFormUpdate();

        }


        /// <summary>
        /// Checking the 'Running' checkbox starts or stops the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRunning_CheckedChanged(object sender, EventArgs e)
        {
            if(cbxRunning.Checked)
                _timer.Start();
            else
                _timer.Stop();
         }


        private void btnA_Click(object sender, EventArgs e)
        {
            
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonAPress();
            DrawFrame();
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonBPress();
            DrawFrame();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonCPress();
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonDPress();
            DrawFrame();
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonEPress();
            DrawFrame();
        }

        private void btnF_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();
            Globals.Controller.ButtonFPress();
            DrawFrame();
        }

        private void cbxStopOnFunc_CheckedChanged(object sender, EventArgs e)
        {
            _pauseOnFunc = cbxRunning.Checked;
            DrawFrame();
        }


        private void CheckForStop()
        {
            if (_pauseOnFunc)
                cbxRunning.Checked = false;
        }

        /// <summary>
        /// Presents the user with the Trace form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Open only one at a time...
            if (_trace != null)
            {
                _trace.Focus();
                return;
            }
            
            _trace = new TraceForm();
            _trace.Closed += (s, ev) => _trace = null;
            _trace.Show();
        }

        #region Simulation Rate And Frame Rate Trackbar configuration

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Globals.SimRate = tbrSimRate.Value;
            RedrawLabel(lblSimRate, tbrSimRate);
            _timer.Interval = 1000 / Globals.SimRate;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Globals.DrawEvery = tbrFrameRate.Value;
            RedrawLabel(lblDrawRate, tbrFrameRate);
        }

        private void RedrawLabel(Label lbl, TrackBar trk)
        {
            double x = ((double)trk.Value * (((double)trk.Width / (double)trk.Maximum))) - (double)trk.Value/4;
            int xpos = (int)x;
            lbl.Text = trk.Value.ToString();
            lbl.Location = new Point(trk.Location.X + xpos, trk.Location.Y + (trk.Height / 2));
        }

        #endregion

        private void btnRandom_Click(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();

            Globals.Controller.ButtonRandomPress();
            DrawFrame();
        }


        /// <summary>
        /// Toggles FuzzySet Rule Evaluation, or User-Button control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxControl_CheckedChanged(object sender, EventArgs e)
        {
            if (Globals.Controller == null)
                return;
            CheckForStop();

            Globals.Controller.Manual = cbxControl.Checked;
        }

        /// <summary>
        /// Toggles the rendering of the TracePath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTracePath_CheckedChanged(object sender, EventArgs e)
        {
            Globals.DrawTracePath = cbxTracePath.Checked;
            DrawFrame();
        }

        private void cbxDebug_CheckedChanged(object sender, EventArgs e)
        {
            Globals.DrawDebug = cbxDebug.Checked;
            DrawFrame();
        }

     
    }
}
