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

namespace FuzzySim
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// MOON LANDER SIMULATOR 
    /// </summary>
    public class LanderSim : AISimulator
    {
        /// <summary>
        /// The repository of MoonLander-related variables  
        /// </summary>
        public SimVars.MoonLanderVars SpaceShip;

        /// <summary>
        /// Default difficulty setting...
        /// </summary>
        public SimDifficultyEnum Difficulty = SimDifficultyEnum.Easy;

        /// <summary>
        /// To be fired upon safe touchdown of Lander
        /// </summary>
        public override void Succeed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be fired upon failure to land
        /// </summary>
        public override void Fail()
        {

        }

        /// <summary>
        /// Initialises a new MoonLander Simulation run
        /// </summary>
        /// <param name="xrez">Canvas X size</param>
        /// <param name="yrez">Canvas Y size</param>
        /// <param name="dif">Difficulty of Simulation</param>
        /// <returns>Rendered instance of Simulation moment (SimFrame)</returns>
        public override SimFrame Init(Vec2 rez, SimDifficultyEnum dif)
        {
            Difficulty = dif;

            


            SpaceShip = new SimVars.MoonLanderVars();

            SpaceShip.Variables["X"] = 250;
            SpaceShip.Variables["Y"] = 4000; // spaceship location (init y=4,000m)
            SpaceShip.Variables["XVel"] = 0;
            SpaceShip.Variables["YVel"] = 0; // spaceship x and y velosity

            SpaceShip.Variables["LandingVelX"] = SimVars.LANDING_X_BASE*3;
            SpaceShip.Variables["LandingVelY"] = SimVars.LANDING_Y_BASE*3;
            SpaceShip.Variables["Variability"] = 0; // 0 = none  0.01 = pluss or -1%

            SpaceShip.Variables["Fuel"] = 8845/7; // 0-8,845 kg of fuel mainly burned.
            SpaceShip.Variables["SafeX"] = 250; // safe to land here
            SpaceShip.Variables["SafeXWidth"] = 10001; // width 10000=no rocks
            SpaceShip.Variables["Mass"] = (double) 0;

            if (Difficulty == SimDifficultyEnum.Easy)
            {
            }

            if (Difficulty == SimDifficultyEnum.Medium)
            {
                SpaceShip.Variables["Y"] = 4250 + new Random().Next(250);
                SpaceShip.Variables["LandingVelX"] = SimVars.LANDING_X_BASE*2;
                SpaceShip.Variables["LandingVelY"] = SimVars.LANDING_Y_BASE*2;
                SpaceShip.Variables["Variability"] = 0.02; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Variables["Fuel"] = 8845/7;
            }

            if (Difficulty == SimDifficultyEnum.Hard)
            {
                SpaceShip.Variables["Y"] = 4250 + new Random().Next(250);
                SpaceShip.Variables["LandingVelX"] = SimVars.LANDING_X_BASE*2;
                SpaceShip.Variables["LandingVelY"] = SimVars.LANDING_Y_BASE*2;
                SpaceShip.Variables["Variability"] = 0.05; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Variables["Fuel"] = 8845/8;
                SpaceShip.Variables["XVel"] = (double)SpaceShip.Variables["LandingVelX"] + 0.1 + new Random().Next(20)/20.0;
            }

            if (Difficulty == SimDifficultyEnum.VeryHard)
            {
                SpaceShip.Variables["Y"] = 4200 + new Random().Next(300);
                SpaceShip.Variables["X"] = 290 + new Random().Next(50);
                SpaceShip.Variables["LandingVelX"] = SimVars.LANDING_X_BASE;
                SpaceShip.Variables["LandingVelY"] = SimVars.LANDING_Y_BASE + SimVars.MOON_GRAVITY/4;
                SpaceShip.Variables["Variability"] = 0.05; // 0 = none  0.01 = pluss or -1%
                SpaceShip.Variables["Fuel"] = (double)(8845/8);
                SpaceShip.Variables["XVel"] = 0.1 + new Random().Next(20)/20.0;
                SpaceShip.Variables["SafeX"] = 100 + new Random().Next(160); // safe to land here
                SpaceShip.Variables["SafeXWidth"] = 60; // width 10000=no rocks
            }

            SpaceShip.Variables["Right"] = 0;
            SpaceShip.Variables["Left"] = 0; // spaceship right and left (RCS) thruster status 0-5 (Hundred newtons)
            SpaceShip.Variables["Throttle"] = 20; // throtle setting 0-100 (0,10-60 or 90)

            SpaceShip.Variables["DownForce"] = 0; // main thruster status 0, 5-44 (thousand newtons)
            SpaceShip.Variables["RightForce"] = 0; // main thruster status 0-5 (Hundred newtons)
            SpaceShip.Variables["LeftForce"] = 0; // main thruster status 0-5 (Hundred newtons)

            SpaceShip.Variables["MaxFuelIn1Sec"] = 8845/15/60.0; // about 9.82

            SpaceShip.Variables["Mass"] = double.Parse(SpaceShip.Variables["Fuel"].ToString()) + (double)5655;
            SpaceShip.Variables["Boom"] = false;
            SpaceShip.Variables["Landed"] = false;

            State = SimulatorStateEnum.Initialised;

            return DrawTurn(FrameResolution);
            // DO LUA FUNCTION : lua_runfunction("evt_GASSInit");
        }

        /// <summary>
        /// Performs calculations for an instance of a Simulation step
        /// </summary>
        public override void DoTurn()
        {

            SpaceShip.Variables["RightForce"] = (double.Parse(SpaceShip.Variables["Right"].ToString())*100); // main thruster status 0-5 (Hundred newtons)
            SpaceShip.Variables["LeftForce"] = (double.Parse(SpaceShip.Variables["Left"].ToString()) * 100); // main thruster status 0-5 (Hundred newtons)
            if (double.Parse(SpaceShip.Variables["Throttle"].ToString()) < (double)10)
            {
                SpaceShip.Variables["DownForce"] = 0; // main thruster status 0, 5-44 (thousand newtons)
            }
            else
            {
                SpaceShip.Variables["DownForce"] = double.Parse(SpaceShip.Variables["Throttle"].ToString()) * (44000/100);
                if (double.Parse(SpaceShip.Variables["Variability"].ToString()) > 0)
                {
                    double v = (new Random().Next(200) - 100)/100.0*double.Parse(SpaceShip.Variables["Variability"].ToString());
                    SpaceShip.Variables["DownForce"] = double.Parse(SpaceShip.Variables["DownForce"].ToString()) + double.Parse(SpaceShip.Variables["DownForce"].ToString())*v;
                }
                SpaceShip.Variables["Fuel"] = double.Parse(SpaceShip.Variables["Fuel"].ToString()) - double.Parse(SpaceShip.Variables["MaxFuelIn1Sec"].ToString())*double.Parse(SpaceShip.Variables["Throttle"].ToString())/100;
                if (double.Parse(SpaceShip.Variables["Fuel"].ToString()) <= 0)
                {
                    SpaceShip.Variables["Fuel"] = 0;
                    SpaceShip.Variables["DownForce"] = 0;
                }
            }

            // now adjust velocity
            SpaceShip.Variables["Mass"] = 5655 + double.Parse(SpaceShip.Variables["Fuel"].ToString());
            SpaceShip.Variables["YVel"] = double.Parse(SpaceShip.Variables["YVel"].ToString()) - SimVars.MOON_GRAVITY;

            SpaceShip.Variables["YVel"] = double.Parse(SpaceShip.Variables["YVel"].ToString()) + (double.Parse(SpaceShip.Variables["DownForce"].ToString())/double.Parse(SpaceShip.Variables["Mass"].ToString()));
            SpaceShip.Variables["XVel"] = double.Parse(SpaceShip.Variables["XVel"].ToString()) - (double.Parse(SpaceShip.Variables["RightForce"].ToString())/double.Parse(SpaceShip.Variables["Mass"].ToString()));
            SpaceShip.Variables["XVel"] = double.Parse(SpaceShip.Variables["XVel"].ToString()) + (double.Parse(SpaceShip.Variables["LeftForce"].ToString())/double.Parse(SpaceShip.Variables["Mass"].ToString()));

            SpaceShip.Variables["Y"] = double.Parse(SpaceShip.Variables["Y"].ToString()) + double.Parse(SpaceShip.Variables["YVel"].ToString());
            SpaceShip.Variables["X"] = double.Parse(SpaceShip.Variables["X"].ToString()) + double.Parse(SpaceShip.Variables["XVel"].ToString());

            // detect end of run
            if ((double)SpaceShip.Variables["Y"] <= 0)
            {
                //vertical velocity <= 3.05 m (10 ft) per sec
                //horizontal velocity <= 1.22 m (4 ft) per sec
                if (Math.Abs(double.Parse(SpaceShip.Variables["XVel"].ToString())) > double.Parse(SpaceShip.Variables["LandingVelX"].ToString()) || Math.Abs(double.Parse(SpaceShip.Variables["YVel"].ToString())) > double.Parse(SpaceShip.Variables["LandingVelY"].ToString())
                    || !Inzone(double.Parse(SpaceShip.Variables["X"].ToString())))
                {
                    SpaceShip.Variables["Boom"] = true;
                }
                SpaceShip.Variables["Landed"] = true;
                SpaceShip.Variables["Throttle"] = 0;
                SpaceShip.Variables["Right"] = 0;
                SpaceShip.Variables["Left"] = 0;
                SpaceShip.Variables["Y"] = 0;

                State = SimulatorStateEnum.Complete;
            }


            //LAND IT - FOR DEV AND DEBUG PURPOSES ONLY
            if (double.Parse(SpaceShip.Variables["Y"].ToString()) < 600)
                if (double.Parse(SpaceShip.Variables["YVel"].ToString()) < -3)
                {
                    SpaceShip.Variables["Throttle"] = 60;
                }
                else
                {
                    SpaceShip.Variables["Throttle"] = 18;
                }

        }


        /// <summary>
        /// Renders MoonLander visual components based on their correlating simulation values 
        /// </summary>
        /// <param name="xscale"></param>
        /// <param name="yscale"></param>
        /// <returns>Rendered instance of Simulation moment (SimFrame)</returns>
        public override SimFrame DrawTurn(Vec2 scale)
        {
            // NEW FRAME
            SimFrame ret = new SimFrame
                               {
                                   Error = false,
                                   ToBeDrawn = true,
                                   BackGround = Color.Black,
                                   FrameId = Globals.turnCount,
                                   Renderables = new List<Drawable>()
                               };

            //ADD STRINGS
            ret.AddText("---SpaceShip---", new Vec2(500, 30), new SolidBrush(Color.Cornsilk));
            ret.AddText("Speed X: " + SpaceShip.Variables["YVel"].ToString(), new Vec2(500, 50), new SolidBrush(Color.Cornsilk));
            ret.AddText("Height: " + SpaceShip.Variables["Y"].ToString(), new Vec2(500, 70), new SolidBrush(Color.Cornsilk));
            ret.AddText("Throttle: " + SpaceShip.Variables["Throttle"].ToString(), new Vec2(500, 90), new SolidBrush(Color.Cornsilk));
            ret.AddText("Fuel: " + SpaceShip.Variables["Fuel"].ToString(), new Vec2(500, 110), new SolidBrush(Color.Cornsilk));

            //ADD MOON BACKGROUND
            ret.AddRenderable(new Sprite {Picture = SpriteList.MoonSurface, Position = new Vec2(0, FrameResolution.Y - 70)});
            
            //SET UP THE SPACESHIP
            Sprite shipSprite = new Sprite();
            shipSprite.Picture = SpriteList.SpaceShip; //Set Image for ship

            if (bool.Parse(SpaceShip.Variables["Landed"].ToString()))
            {
                if (bool.Parse(SpaceShip.Variables["Boom"].ToString()))
                {
                    ret.AddText("FAIL", new Vec2(250, 150), new SolidBrush(Color.Red), 100);
                    shipSprite.Picture = SpriteList.SpaceShip_Explode;
                    Fail();
                }
                else
                {
                    ret.AddText("SAFE", new Vec2(250, 150), new SolidBrush(Color.Green), 100);
                    shipSprite.Picture = SpriteList.SpaceShip_Landed;
                }
            }

            //CALCULATE SS RELATIVE POSITION ON SCREEN
            int simHeight = 5000;
            shipSprite.Position = new Vec2(double.Parse(SpaceShip.Variables["X"].ToString()),
                                           (FrameResolution.Y -
                                            (double.Parse(SpaceShip.Variables["Y"].ToString())/(simHeight/FrameResolution.Y))) - 60);
            
            ret.AddRenderable(shipSprite);//ADD THE SS OBJECT

            return ret; //RETURN THE FINAL FRAME
        }

        /// <summary>
        /// Determines if SpaceShip has landed in safe zone
        /// </summary>
        /// <param name="x">Spaceship X posision (Horizontal)</param>
        /// <returns>True: Safe; False: Crashed;</returns>
        private bool Inzone(double x)
        {
            if (double.Parse(SpaceShip.Variables["SafeXWidth"].ToString()) >= 10000) return true;
            if (Math.Abs(x - double.Parse(SpaceShip.Variables["SafeX"].ToString())) <= double.Parse(SpaceShip.Variables["SafeXWidth"].ToString())) return true;
            return false;
        }
    }
}
