/***********************************************************************
 * 
 *  -----   Moon Lander Simulator   -----   
 *  
 * Implements AISimulator. 
 *  
 *  Based off the original FuzzyLua Moon-Lander Simulator written by Robert Cox.
 *  C# Implementation anf re-write by Timothy Fox, 2012. * 
 * 
 */


namespace FuzzySim.Simulators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Core;
    using Rendering;

    /// <summary>
    /// MOON LANDER SIMULATOR 
    /// </summary>
    public class LanderSim : AISimulator
    {
        /// <summary>
        /// The repository of MoonLander-related variables  
        /// </summary>
        public SimVars.MoonLanderVars SpaceShip { get; set; }

        /// <summary>
        /// To be fired upon safe touchdown of Lander
        /// </summary>
        public override void Succeed()
        {
           
        }

        /// <summary>
        /// To be fired upon failure to land
        /// </summary>
        public override void Fail()
        {
           
        }

        public void InitSpaceShip()
        {
            SpaceShip = new SimVars.MoonLanderVars();

            #region Set up the initial state of the MoonLander's physics

            SpaceShip.X = 250;
            SpaceShip.Y = 4000; // spaceship location (init y=4,000m)
            SpaceShip.XVel = 0;
            SpaceShip.YVel = 0; // spaceship x and y velosity

            SpaceShip.LandingVelX = SimVars.MoonLanderVars.LANDING_X_BASE * 3;
            SpaceShip.LandingVelY = SimVars.MoonLanderVars.LANDING_Y_BASE * 3;
            SpaceShip.Variability = 0; // 0 = none  0.01 = pluss or -1%

            SpaceShip.Fuel = 8845 / 7; // 0-8,845 kg of Fuel mainly burned.
            SpaceShip.SafeX = 250; // safe to land here
            SpaceShip.SafeXWidth = 10001; // width 10000=no rocks


            if (Difficulty == SimDifficultyEnum.Easy)
            {
            }

            if (Difficulty == SimDifficultyEnum.Medium)
            {
                SpaceShip.Y = 4250 + new Random().Next(250);
                SpaceShip.LandingVelX = SimVars.MoonLanderVars.LANDING_X_BASE * 2;
                SpaceShip.LandingVelY = SimVars.MoonLanderVars.LANDING_Y_BASE * 2;
                SpaceShip.Variability = 0.02; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Fuel = 8845 / 7;
            }

            if (Difficulty == SimDifficultyEnum.Hard)
            {
                SpaceShip.Y = 4250 + new Random().Next(250);
                SpaceShip.LandingVelX = SimVars.MoonLanderVars.LANDING_X_BASE * 2;
                SpaceShip.LandingVelY = SimVars.MoonLanderVars.LANDING_Y_BASE * 2;
                SpaceShip.Variability = 0.05; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Fuel = 8845 / 8;
                SpaceShip.XVel = SpaceShip.LandingVelX + 0.1 + new Random().Next(20) / 20.0;
            }

            if (Difficulty == SimDifficultyEnum.VeryHard)
            {
                SpaceShip.Y = 4200 + new Random().Next(300);
                SpaceShip.X = 290 + new Random().Next(50);
                SpaceShip.LandingVelX = SimVars.MoonLanderVars.LANDING_X_BASE;
                SpaceShip.LandingVelY = SimVars.MoonLanderVars.LANDING_Y_BASE + SimVars.MoonLanderVars.MOON_GRAVITY / 4;
                SpaceShip.Variability = 0.05; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Fuel = 8845 / 8;
                SpaceShip.XVel = 0.1 + new Random().Next(20) / 20.0;
                SpaceShip.SafeX = 100 + new Random().Next(160); // safe to land here
                SpaceShip.SafeXWidth = 60; // width 10000=no rocks
            }

            SpaceShip.Right = 0;
            SpaceShip.Left = 0; // spaceship right and left (RCS) thruster status 0-5 (Hundred newtons)
            SpaceShip.Throttle = 20; // throtle setting 0-100 (0,10-60 or 90)

            SpaceShip.DownForce = 0; // main thruster status 0, 5-44 (thousand newtons)
            SpaceShip.RightForce = 0; // main thruster status 0-5 (Hundred newtons)
            SpaceShip.LeftForce = 0; // main thruster status 0-5 (Hundred newtons)

            SpaceShip.MaxFuelIn1Sec = 8845 / 15 / 60.0; // about 9.82
            SpaceShip.Mass = 5655 + SpaceShip.Fuel; // 5655 + Fuel
            SpaceShip.Boom = false;
            SpaceShip.Landed = false;

            #endregion

            State = SimulatorStateEnum.Initialised;
        }

        /// <summary>
        /// Initialises a new MoonLander Simulation run
        /// </summary>
        /// <param name="rez">Canvas size</param>
        /// <returns>Rendered instance of Simulation moment (SimFrame)</returns>
        public override SimFrame Init(Vec2 rez)
        {
            InitSpaceShip();

            TracePath = new List<PrintText>();

            return DrawTurn(FrameResolution);
        }

      

        /// <summary>
        /// Performs calculations for an instance of a Simulation step
        /// </summary>
        public override void DoTurn()
        {
            //EVALUATE ALL THE RULES
            Globals.Controller.CalculateFuzzyLogic();

            #region Physics for the MoonLander in here...
            
            //NOW SET THE VARIABLES (ITTERATE SIMULATION MOMENT)
            SpaceShip.RightForce = SpaceShip.Right * 100; // right thruster status 0-5 (Hundred newtons)
            SpaceShip.LeftForce = SpaceShip.Left * 100; // left thruster status 0-5 (Hundred newtons)

            SpaceShip.Fuel = SpaceShip.Fuel - SpaceShip.MaxFuelIn1Sec * SpaceShip.Right / (double)1000;
            SpaceShip.Fuel = SpaceShip.Fuel - SpaceShip.MaxFuelIn1Sec * SpaceShip.Left / (double)1000;

            if (SpaceShip.Throttle < 10)
            {
                SpaceShip.DownForce = 0; // main thruster status 0, 5-44 (thousand newtons)
            }
            else
            {
                SpaceShip.DownForce = SpaceShip.Throttle * (double)44000 / (double)100;
                if (SpaceShip.Variability > 0)
                {
                    double v = (new Random().Next(200) - 100) / 100.0 * SpaceShip.Variability;
                    SpaceShip.DownForce = SpaceShip.DownForce + SpaceShip.DownForce * v;
                }
                SpaceShip.Fuel = SpaceShip.Fuel - SpaceShip.MaxFuelIn1Sec * SpaceShip.Throttle / 100;

              

                if (SpaceShip.Fuel <= 0)
                {
                    SpaceShip.Fuel = 0;
                    SpaceShip.DownForce = 0;
                }
            }

            // now adjust velocity
            SpaceShip.Mass = 5655 + SpaceShip.Fuel;
            SpaceShip.YVel = SpaceShip.YVel - SimVars.MoonLanderVars.MOON_GRAVITY;

            SpaceShip.YVel = SpaceShip.YVel + (SpaceShip.DownForce / SpaceShip.Mass);
            SpaceShip.XVel = SpaceShip.XVel - (SpaceShip.RightForce / SpaceShip.Mass);
            SpaceShip.XVel = SpaceShip.XVel + (SpaceShip.LeftForce / SpaceShip.Mass);

            SpaceShip.Y = SpaceShip.Y + SpaceShip.YVel;
            SpaceShip.X = SpaceShip.X + SpaceShip.XVel;

            // detect end of run
            if (SpaceShip.Y <= 0)
            {
                //vertical velocity <= 3.05 m (10 ft) per sec
                //horizontal velocity <= 1.22 m (4 ft) per sec
                if (Math.Abs(SpaceShip.XVel) > SpaceShip.LandingVelX || Math.Abs(SpaceShip.YVel) > SpaceShip.LandingVelY
                    || !Inzone(SpaceShip.X))
                {
                    SpaceShip.Boom = true;
                    Fail();
                }
                SpaceShip.Landed = true;
                SpaceShip.Throttle = 0;
                SpaceShip.Right = 0;
                SpaceShip.Left = 0;
                SpaceShip.Y = 0;
                
                if(!SpaceShip.Boom)
                    Succeed();

                State = SimulatorStateEnum.Complete;
            }
        
            #endregion
        }


        /// <summary>
        /// Renders MoonLander visual components based on their correlating simulation values 
        /// </summary>
        /// <param name="scale"></param> 
        /// <returns>Rendered instance of Simulation moment (SimFrame)</returns>
        public override SimFrame DrawTurn(Vec2 scale)
        {
            #region Draws and spits out a SimFrame based on current SpaceShip state
            // NEW FRAME
            SimFrame ret = new SimFrame
            {
                Error = false,
                ToBeDrawn = true,
                BackGround = Color.Black,
                FrameId = Globals.TurnCount,
                Renderables = new List<Drawable>()
            };


            AddTracePathBlip(ret);

            DrawText(30, ret);

            //ADD MOON BACKGROUND
            ret.AddRenderable(new Sprite("surface1") { Picture = SpriteList.MoonSurface, Position = new Vec2(0, FrameResolution.Y - 20) });
            ret.AddRenderable(new Sprite("surface2") { Picture = SpriteList.MoonSurface_Flip, Position = new Vec2(SpriteList.MoonSurface.Width, FrameResolution.Y - 20) });

            //SET UP THE SPACESHIP
            Sprite shipSprite = new Sprite("ss");
            shipSprite.Picture = SpriteList.SpaceShip; //Set Image for ship

            //Add the thrust to the Space Ship
            Sprite downThrust = new Sprite("downThrust")
                                    {
                                        Picture = SpriteList.ThrustDown 
                                    };

            Sprite leftThrust = new Sprite("leftThrust")
            {
                Picture = SpriteList.ThrustLeft
            };

            Sprite rightThrust = new Sprite("rightThrust")
            {
                Picture = SpriteList.ThrustRight
            };


            if (Globals.DrawTracePath)
                DrawTracePath(ret);

            //CALCULATE SS RELATIVE POSITION ON SCREEN

            shipSprite.Position = new Vec2(SpaceShip.X,
                                           (FrameResolution.Y -
                                            (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 60);



            ret.AddRenderable(shipSprite);//ADD THE SS OBJECT

            if(!CheckForFinishText(ret)) // true if finished
                 //do the thrust image
            {
                downThrust.Scale = new Vec2(1, SpaceShip.Throttle * .04);
                leftThrust.Scale = new Vec2(SpaceShip.Left /3, .5);
                rightThrust.Scale = new Vec2(SpaceShip.Right/3, .5);
                
                downThrust.Position = new Vec2(SpaceShip.X + 4,
                                         (FrameResolution.Y -
                                          (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 20);


                leftThrust.Position = new Vec2(SpaceShip.X - 10 - (leftThrust.Picture.Width * leftThrust.Scale.X),
                                         (FrameResolution.Y -
                                          (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 50);

                rightThrust.Position = new Vec2(SpaceShip.X + 42,
                                         (FrameResolution.Y -
                                          (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 50);


                if (SpaceShip.Fuel > 0)
                {
                    ret.AddRenderable(downThrust); //add the thrust object to the frame
                     ret.AddRenderable(leftThrust); //add the thrust object to the frame
                     ret.AddRenderable(rightThrust); //add the thrust object to the frame
                }
            }



            //DRAW ROCKS(IF APPLICABLE)

            List<Drawable> rocks = DrawRocks();

            if (rocks != null)
                foreach (Drawable d in rocks)
                    ret.AddRenderable(d);

            if (Globals.DrawDebug && State != SimulatorStateEnum.Complete)
                DrawDebug(ret, new Vec2(leftThrust.Position.X - 15 - (SpaceShip.Left * leftThrust.Scale.X * 6), leftThrust.Position.Y) ,
                    new Vec2(downThrust.Position.X + 1, downThrust.Position.Y + (30 * downThrust.Scale.Y)), 
                    new Vec2(rightThrust.Position.X + (SpaceShip.Right * rightThrust.Scale.X * 12) , rightThrust.Position.Y));

            if (Globals.DrawTracePath)
                if(TracePath != null)
                for (int i = 1; i < TracePath.Count; i++)
                {
                    ret.AddRenderable(new Line(TracePath[i - 1].Position, TracePath[i].Position, new SolidBrush(Color.MediumBlue), .5f ));
                }

            #endregion

            ret.TracePath = TracePath;


            return ret; //RETURN THE FINAL FRAME
        }

        private void DrawDebug(SimFrame ret, Vec2 leftT, Vec2 downT, Vec2 rightT)
        {
            Brush colour = new SolidBrush(Color.Red);
            Brush shadow = new SolidBrush(Color.White);

          
            ret.AddTextWithShadow("debug_left", String.Format("{0}", SpaceShip.Left.ToString("F")), leftT, 10, colour, shadow);
            ret.AddTextWithShadow("debug_down", String.Format("{0}", SpaceShip.Throttle.ToString("F")), downT, 10, colour, shadow);
            ret.AddTextWithShadow("debug_right", String.Format("{0}", SpaceShip.Right.ToString("F")), rightT, 10, colour, shadow);
           
        }

        private void AddTracePathBlip(SimFrame ret)
        {
            if (Globals.TurnCount % 10 == 0)
                TracePath.Add(ret.PlotLocation(new Vec2(SpaceShip.X+20, ToY(SpaceShip.Y)), Color.Firebrick));
        }


        private bool CheckForFinishText(SimFrame ret)
        {
            if (SpaceShip.Landed)
            {
                if (SpaceShip.Boom)
                {

                    ret.AddTextWithShadow("Failure", "FAIL", new Vec2(150, 150), 100, new SolidBrush(Color.Red),
                                          new SolidBrush(Color.White));
                    ret.ChangeImage("ss", SpriteList.SpaceShip_Explode);
                }
                else
                {
                    ret.AddTextWithShadow("Success", "SAFE", new Vec2(150, 150), 100, new SolidBrush(Color.Green),
                                          new SolidBrush(Color.White));
                    ret.ChangeImage("ss", SpriteList.SpaceShip_Landed);
                }
                return true;
            }
            return false;
        }

        private void DrawText(int x, SimFrame ret)
        {
            Brush colour = new SolidBrush(Color.Orange);
            Brush shadow = new SolidBrush(Color.Green);

            //ADD STRINGS
            ret.AddTextWithShadow("title", "---SpaceShip---", new Vec2(x, 30), colour, shadow);
            ret.AddTextWithShadow("ySpeed", "Speed Y:  " + SpaceShip.YVel.ToString("F"), new Vec2(x, 50), colour, shadow);
            ret.AddTextWithShadow("xSpeed", "Speed X:  " + SpaceShip.XVel.ToString("F"), new Vec2(x, 70), colour, shadow);
            ret.AddTextWithShadow("height", "Height:   " + SpaceShip.Y.ToString("F"), new Vec2(x, 90), colour, shadow);
            ret.AddTextWithShadow("xPos", "X-Pos:   " + SpaceShip.X.ToString("F"), new Vec2(x, 110), colour, shadow);
            ret.AddTextWithShadow("throttle", "Throttle: " + SpaceShip.Throttle.ToString("F"), new Vec2(x, 130), colour, shadow);
            ret.AddTextWithShadow("Fuel", "Fuel:     " + SpaceShip.Fuel.ToString("F"), new Vec2(x, 150), colour, shadow);
        }


        /// <summary> //VESTIGIAL PROTOTYPE...
        /// Draws the next turn based off the previous frame
        /// Avoids all the 'new'ing that takes place in the other DrawTurn
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public override SimFrame DrawTurn(SimFrame frame)
        {
            frame.Find("ss").Position = new Vec2(SpaceShip.X,
                                           (FrameResolution.Y -
                                            (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 60);

            frame.Find("downThrust").Position = new Vec2(SpaceShip.X + 4,
                                      (FrameResolution.Y -
                                       (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 20);


            frame.Find("leftThrust").Position = new Vec2(SpaceShip.X - 10 - (32 * SpaceShip.Left / 3),
                                     (FrameResolution.Y -
                                      (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 50);

            frame.Find("rightThrust").Position = new Vec2(SpaceShip.X + 42,
                                     (FrameResolution.Y -
                                      (SpaceShip.Y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y))) - 50);



            ((Sprite)frame.Find("downThrust")).Scale = new Vec2(1, SpaceShip.Throttle * .04);
            ((Sprite)frame.Find("leftThrust")).Scale = new Vec2(SpaceShip.Left / 3, .5);
            ((Sprite)frame.Find("rightThrust")).Scale = new Vec2(SpaceShip.Right / 3, .5);
                


            if (SpaceShip.Landed)
            {
                if (SpaceShip.Boom)
                {
                    frame.AddText("FAIL", new Vec2(250, 150), new SolidBrush(Color.Red), 100);
                    frame.ChangeImage("ss", SpriteList.SpaceShip_Explode);
                    Fail();
                }
                else
                {
                    frame.AddText("SAFE", new Vec2(250, 150), new SolidBrush(Color.Green), 100);
                    frame.ChangeImage("ss", SpriteList.SpaceShip_Landed);
                }
            }

            frame.ChangeText("ySpeed", "Speed Y:  " + SpaceShip.YVel.ToString("F"));
            frame.ChangeText("xSpeed", "Speed X:  " + SpaceShip.XVel.ToString("F"));
            frame.ChangeText("height", "Height:   " + SpaceShip.Y.ToString("F"));
            frame.ChangeText("xPos", "X-Pos:   " + SpaceShip.X.ToString("F"));
            frame.ChangeText("throttle", "Throttle: " + SpaceShip.Throttle.ToString("F"));
            frame.ChangeText("Fuel", "Fuel:     " + SpaceShip.Fuel.ToString("F"));

            return frame;

        }

        private double ToX(double x)
        {
            return x;
        }

        private double ToY(double y)
        {
            return  FrameResolution.Y -
                                            (y / (SimVars.MoonLanderVars.SIM_HEIGHT / FrameResolution.Y)) - 40;
        }

        /// <summary>
        /// Draws rocks on to the Lunar Surface (VeryHard Difficulty only!)
        /// </summary>
        /// <returns>List of rock Sprites</returns>
        private List<Drawable> DrawRocks()
        {
            #region Draws Rocks...

            if (SpaceShip.SafeXWidth >= 10000) return null;


            List<Drawable> retlist = new List<Drawable>();


            double surface = FrameResolution.Y - 40;
            double rockscale = 0.9;
            double increment = SpriteList.Rock.Width*rockscale;

            //add rocks

            for (int i = 0; i < Globals.Simulator.FrameResolution.X; i += (int)increment)
            {
                if (i < SpaceShip.SafeX - (increment) || i > (SpaceShip.SafeX + SpaceShip.SafeXWidth))
                {
                    retlist.Add(
                                new Sprite("rock"+i.ToString())
                                    {
                                        Picture   = SpriteList.Rock,
                                        Position  = new Vec2(i, surface),
                                        Scale = new Vec2(rockscale,rockscale)
                                    }
                                );
                }

            }

            //go back and draw a rock on the leading and trailing edges of the landing zone...
            retlist.Add(
                                new Sprite("rockMin")
                                {
                                    Picture = SpriteList.Rock,
                                    Position = new Vec2(SpaceShip.SafeX - (increment), surface),
                                    Scale = new Vec2(rockscale, rockscale)
                                }
                        );

            retlist.Add(
                                new Sprite("rockMax")
                                {
                                    Picture = SpriteList.Rock,
                                    Position = new Vec2(SpaceShip.SafeX + SpaceShip.SafeXWidth, surface),
                                    Scale = new Vec2(rockscale, rockscale)
                                }
                        );

            
            retlist.Add(new Line(new Vec2(SpaceShip.SafeX, surface + 22), new Vec2(SpaceShip.SafeX + SpaceShip.SafeXWidth, surface  + 22), new SolidBrush(Color.Red), 3f ) { Id = "landingLine"});
            
            return retlist;

            #endregion
        }

        /// <summary>
        /// Determines if SpaceShip has Landed in safe zone
        /// </summary>
        /// <param name="x">Spaceship X posision (Horizontal)</param>
        /// <returns>True: Safe; False: Crashed;</returns>
        private bool Inzone(double x)
        {
            if (SpaceShip.SafeXWidth >= 10000) return true;
            if (Math.Abs(x - SpaceShip.SafeX) <= SpaceShip.SafeXWidth) return true;
            return false;
        }
    }
}
