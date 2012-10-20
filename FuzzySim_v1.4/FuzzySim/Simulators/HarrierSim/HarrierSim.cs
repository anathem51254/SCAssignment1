/*
 *              ===== Harrier Simulator ===== 
 * 
 *  Original Simulation design:         Robert Cox [C++]
 *          C# Version ReWrite:         Timothy Fox
 *                        Date:         September 2012
 * 
*/

namespace FuzzySim.Simulators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Core;
    using Rendering;


    class HarrierSim : AISimulator
    {
        public SimVars.HarrierVars Harrier;

        public override void Succeed()
        {
            throw new NotImplementedException();
        }

        public override void Fail()
        {

        }

        public override SimFrame Init(Vec2 rez)
        {
            TracePath = new List<PrintText>();

            #region Initializes the Harrier Simulator

            //set up the sea background
/*            _sea = new Sprite("sea")
            {
                Picture = SpriteList.SeaSurface,
                Scale = new Vec2(3, 3),

            };*/
        
            Harrier = new SimVars.HarrierVars();

            Harrier.GroundEffect = false;

            Harrier.MetersToPixels = 2.5;
            Harrier.PixelsToMeters = 1 / Harrier.MetersToPixels;

            Harrier.TickPerSecond = 3;

            Harrier.MaxWindGust = 0;

            InitDifficulty();

            Harrier.Landed = false;
            Harrier.Boom = false;

            Harrier.MaxFuelIn1Sec = 0.5;
            Harrier.MinFuelIn1Sec = 0.1;

            Harrier.Mass = 5655 + Harrier.Fuel;  // 5655 + fuel

            Harrier.Boom = false;
            Harrier.Landed = false;

            #endregion

            State = SimulatorStateEnum.Initialised;

            return new SimFrame();
        }

        private void InitDifficulty()
        {
            #region Sets up the Harrier Simulator depending on difficulty

            Harrier.PlaneImage = SpriteList.Harrier;

            if (Difficulty == SimDifficultyEnum.Easy)
            {

                Harrier.ShipImage = SpriteList.ShipEnterprise;

                Harrier.GroundEffect = false;   

                Harrier.RelativeXVel = 0; // ship air speed
                Harrier.MaxWindGust = 0; // max wind gusts
                Harrier.shipSpeed = 0;

                Harrier.Fuel = 900;  // fuel reserve
                Harrier.XVel = 4;  // start x speed
                Harrier.YVel = -5;  // start y speed

                Harrier.CarrierHeightPixels = 163;  // size of png image
                Harrier.CarrierWidthPixels = 837;
                Harrier.CarrierLandingHeightPixels = 50;

                Harrier.CarrierImageX = 0;       // Where to blit the carrier in pixels
     

                Harrier.UnsafeX1 = 4;           // safe to land here meters
                Harrier.SafeX2 = 323 + Harrier.UnsafeX1;           // safe to land here meters

                Harrier.UnsafeY1 = 0;           // in meters
                Harrier.SafeY2 = Harrier.CarrierLandingHeightPixels * Harrier.PixelsToMeters;      // in meters
                Harrier.SafeYDraw = Harrier.SafeY2 - 4;
                Harrier.UnsafeY3 =  0;           // in meters

                Harrier.MidSafeX = (Harrier.UnsafeX1 + Harrier.SafeX2) / 2; // calc middle of landing platform

                Harrier.UnFueledMass = 5942;    // kg
                Harrier.MaxForce = 84500;       // newton

                Harrier.Throttle = 74;

            }

            if (Difficulty == SimDifficultyEnum.Medium)
            {
                Harrier.ShipImage = SpriteList.ShipAustralia;

                Harrier.GroundEffect = true; 

                Harrier.RelativeXVel = 12; // ship air speed

                Harrier.WindSpeed = 5; //Wind speed
                Harrier.MaxWindGust = 5; // max wind gusts

                Harrier.Fuel = 900;  // fuel reserve
                Harrier.XVel = 27;  // start x speed
                Harrier.YVel = -5;  // start y speed
                Harrier.shipSpeed = 12;

                Harrier.CarrierHeightPixels = 112;  // size of png image
                Harrier.CarrierWidthPixels = 575;
                Harrier.CarrierLandingHeightPixels = 60;

                Harrier.CarrierImageX = 120;       // Where to blit the carrier in pixels
                
                Harrier.UnsafeX1 = 8 + Harrier.CarrierImageX * Harrier.PixelsToMeters;           // safe to land here meters
                Harrier.SafeX2 = 190 + Harrier.UnsafeX1;           // safe to land here meters

                Harrier.UnsafeY1 = 0;           // in meters
                Harrier.SafeY2 = Harrier.CarrierLandingHeightPixels * Harrier.PixelsToMeters;      // in meters
                Harrier.SafeYDraw = Harrier.SafeY2 - 4;                 
                Harrier.UnsafeY3 =0;           // in meters

                Harrier.MidSafeX = (Harrier.UnsafeX1 + Harrier.SafeX2) / 2; // calc middle of landing platform

                Harrier.UnFueledMass = 5942;    // kg
                Harrier.MaxForce = 84500;       // newton

                Harrier.Throttle = 84;
            }

            if (Difficulty == SimDifficultyEnum.Hard)
            {
                Harrier.ShipImage = SpriteList.ShipAustralia;

                Harrier.GroundEffect = true;

                Harrier.RelativeXVel = 20; // ship air speed

                Harrier.WindSpeed = 15; //Wind speed
                Harrier.MaxWindGust = 15; // max wind gusts


                Harrier.Fuel = 100;  // fuel reserve
                Harrier.XVel = 32;  // start x speed
                Harrier.YVel = -1;  // start y speed
                Harrier.shipSpeed = 20;

                Harrier.CarrierHeightPixels = 112;  // size of png image
                Harrier.CarrierWidthPixels = 575;
                Harrier.CarrierLandingHeightPixels = 60;

                Harrier.CarrierImageX = 120;       // Where to blit the carrier in pixels

                Harrier.UnsafeX1 = 8 + Harrier.CarrierImageX * Harrier.PixelsToMeters;           // safe to land here meters
                Harrier.SafeX2 = 190 + Harrier.UnsafeX1;           // safe to land here meters

                Harrier.UnsafeY1 = 0;           // in meters
                Harrier.SafeY2 = Harrier.CarrierLandingHeightPixels * Harrier.PixelsToMeters;      // in meters
                Harrier.SafeYDraw = Harrier.SafeYDraw = Harrier.SafeY2 - 4; 
                Harrier.UnsafeY3 = 0;           // in meters

                Harrier.MidSafeX = (Harrier.UnsafeX1 + Harrier.SafeX2) / 2; // calc middle of landing platform

                Harrier.UnFueledMass = 5700 + 3500;    // kg
                Harrier.MaxForce = 106000;       // newton

                Harrier.Throttle = 89;
            }

            if (Difficulty == SimDifficultyEnum.VeryHard)
            {
                Harrier.ShipImage = SpriteList.ShipAnzac;

                Harrier.GroundEffect = true;

                Harrier.RelativeXVel = 22; // ship air speed

                Harrier.WindSpeed = 10; //Wind speed
                Harrier.MaxWindGust = 20; // max wind gusts


                Harrier.Fuel = 80;  // fuel reserve
                Harrier.XVel = 46;  // start x speed
                Harrier.YVel = 2;  // start y speed
                Harrier.shipSpeed = 22;

                Harrier.CarrierHeightPixels = 74;  // size of png image
                Harrier.CarrierWidthPixels = 309;
                Harrier.CarrierLandingHeightPixels = 27;

                Harrier.CarrierImageX = 300;       // Where to blit the carrier in pixels

                Harrier.UnsafeX1 = 2 + Harrier.CarrierImageX * Harrier.PixelsToMeters;     // safe to land here meters
                Harrier.SafeX2 = 25 + Harrier.UnsafeX1;           // safe to land here meters

                Harrier.UnsafeY1 = 0;           // in meters
                Harrier.SafeY2 = Harrier.CarrierLandingHeightPixels * Harrier.PixelsToMeters;      // in meters
                Harrier.SafeYDraw = Harrier.SafeYDraw = Harrier.SafeY2 - 4; 
                Harrier.UnsafeY3 = 15;           // in meters

                Harrier.MidSafeX = (Harrier.UnsafeX1 + Harrier.SafeX2) / 2; // calc middle of landing platform

                Harrier.UnFueledMass = 5700 + 3500;    // kg
                Harrier.MaxForce = 106000;       // newton

                Harrier.Throttle = 89;
            }

            Harrier.InitSpeedKnots = Harrier.shipSpeed*Harrier.ConvertKnotsToMps;
            Harrier.X = 15;
            Harrier.Y = 170; // harrier location (init y=4,000m)
            Harrier.XVel = Harrier.InitSpeedKnots * Harrier.ConvertKnotsToMps; // convert knots to meters per second
            Harrier.ThrustVector = 92;

            Harrier.MaxLandingSpeedY = 3.4;       // 3 meters per second max
            Harrier.MaxLandingSpeedX = 3.4;       // 3 meters per second max
            
            #endregion
        }

        public override SimFrame DrawTurn(Vec2 scale) //scale provides the height (Y)
        {
            double height = scale.Y;

            Harrier.CarrierImageY = scale.Y - Harrier.CarrierHeightPixels - 28;

            SimFrame ret = new SimFrame
                            {
                                Error = false, 
                                ToBeDrawn = true, 
                                BackGround = Color.FromArgb(255, 0, 255, 255),
                                Renderables = new List<Drawable> {}
                            };

            AddTracePathBlip(ret);

            ret.AddRenderable((new Sprite("ship")
            {
                Picture = Harrier.ShipImage,
                Position = new Vec2(Harrier.CarrierImageX, Harrier.CarrierImageY)
            }
                    ));

            DrawLines(ret);

            CheckForFinishText(ret);

            if (Globals.DrawTracePath)
                DrawTracePath(ret);

            ret.AddRenderable(new Sprite("hs")
                                  {
                                      Picture = Harrier.PlaneImage,
                                      Position = new Vec2(ToX(Harrier.X-8),ToY(Harrier.Y+4)),
                                      Scale = new Vec2(1.5,1.5)

                                  }
                                );
            

            
            DrawText(30, ref ret);
            
            if(Globals.DrawDebug)
                DrawDebug(ret);

            ret.TracePath = TracePath;

            return ret;
        }

        private void AddTracePathBlip(SimFrame ret)
        {
            if (Globals.TurnCount%20 == 0)
                TracePath.Add(ret.PlotLocation(new Vec2(ToX(Harrier.X - 8), ToY(Harrier.Y + 4)), Color.Firebrick));
        }

        private void CheckForFinishText(SimFrame ret)
        {
            if(Harrier.Landed)
                if (Harrier.Boom)
                {
                    
                    ret.AddTextWithShadow("Failure", "FAIL", new Vec2(150, 150), 100 , new SolidBrush(Color.Red), new SolidBrush(Color.DimGray));
                }
                else
                {
                    ret.AddTextWithShadow("Success", "SAFE", new Vec2(150, 150), 100, new SolidBrush(Color.Green), new SolidBrush(Color.DimGray));
                }
        }


        

        private void DrawDebug(SimFrame ret)
        {
            Brush colour = new SolidBrush(Color.Red);
            Brush shadow = new SolidBrush(Color.White);
            
            double xx = 0, yy = 0;
            Move2DInDirHS(Harrier.X, Harrier.Y, DegToRad(270 - Harrier.ThrustVector), Harrier.Throttle / 4, ref xx, ref yy);
            
            ret.AddTextWithShadow("debug_TV", String.Format("{0}", Harrier.ThrustVector.ToString("F")), new Vec2(ToX(Harrier.X+1), ToY(Harrier.Y-5)), 10, colour, shadow);
            ret.AddTextWithShadow("debug_Thrust", String.Format("{0}", Harrier.Throttle.ToString("F")), new Vec2(ToX(xx+2), ToY(yy)), 10, colour, shadow);
            
        }

        private void DrawLines(SimFrame ret)
        {
            double xx = 0, yy = 0;

            float throttleThickness = (float)((((Harrier.Throttle) - 60)/40) * 8f);

            //throttleThickness = 0.05f; 

            if (throttleThickness < .05f)
                throttleThickness = .05f; 

            Move2DInDirHS(Harrier.X, Harrier.Y, DegToRad(270 - Harrier.ThrustVector), Harrier.Throttle/4, ref xx, ref yy);

            ret.AddRenderable(new Line(new Vec2(ToX(Harrier.X+1), ToY(Harrier.Y-2)), new Vec2(ToX(xx+1), ToY(yy)),
                                       new SolidBrush(Color.Red), throttleThickness) { Id = "ThrustLine" });

            ret.AddRenderable(new Line(new Vec2(ToX(Harrier.UnsafeX1), ToY(Harrier.SafeYDraw + 1)),
                                       new Vec2(ToX(Harrier.SafeX2), ToY(Harrier.SafeYDraw + 1)), new SolidBrush(Color.Red), 3f)
                                  {Id = "LandingLine"});

            ret.AddRenderable(new Line(new Vec2(ToX(Harrier.MidSafeX - 3), ToY(Harrier.SafeYDraw + 1)),
                                       new Vec2(ToX(Harrier.MidSafeX + 3), ToY(Harrier.SafeYDraw + 1)),
                                       new SolidBrush(Color.Green), 3f) {Id = "MiddleSafe"});
        }

        private void DrawText(int x, ref  SimFrame ret)
        {
            Brush colour = new SolidBrush(Color.Black);
            Brush shadow = new SolidBrush(Color.White);

            ret.AddTextWithShadow("location", String.Format("X, Y: \t\t\t {0}, {1}", Harrier.X.ToString("F"), Harrier.Y.ToString("F")), new Vec2(x, 20), colour, shadow);
            ret.AddTextWithShadow("velocity", String.Format("X-Vel, Y-Vel: \t\t {0}, {1}", Harrier.XVel.ToString("F"), Harrier.YVel.ToString("F")), new Vec2(x, 40), colour, shadow);
            ret.AddTextWithShadow("consumption", String.Format("Secs, Fuel: \t\t {0}, {1}", ((double)Globals.TurnCount / Harrier.TickPerSecond).ToString("F"), Harrier.Fuel.ToString("F")), new Vec2(x, 60), colour, shadow);
            ret.AddTextWithShadow("physics", String.Format("Mass, Throttle: \t\t {0}, {1}", Harrier.Mass.ToString("F"), Harrier.Throttle.ToString("F")), new Vec2(x, 80), colour, shadow);
            ret.AddTextWithShadow("wind", String.Format("ThrustVec, WindGust: \t {0}, {1}", Harrier.ThrustVector.ToString("F"), Harrier.WindSpeed.ToString("F")), new Vec2(x, 100), colour, shadow);
            ret.AddTextWithShadow("relative", String.Format("Speed Relative to Ship: \t  {0}", Harrier.RelativeXVel.ToString("F")), new Vec2(x, 120), colour, shadow);
            ret.AddTextWithShadow("shipsSpeed", String.Format("Ship Speed knots (mps) \t  {0}, {1}", Harrier.shipSpeed.ToString("F"), (Harrier.shipSpeed*Harrier.ConvertKnotsToMps).ToString("F")), new Vec2(x, 140), colour, shadow);
        }

        public override SimFrame DrawTurn(SimFrame frame)
        {
            

            return frame;
        }

        /// <summary>
        /// Does all siome 
        /// </summary>
        public override void DoTurn()
        {
            #region Harrier Physics

            Globals.Controller.CalculateFuzzyLogic();

            if (Harrier.Landed)
            {
                State = SimulatorStateEnum.Complete;
                return;
            }

            if (Harrier.Throttle <= 0)
                Harrier.Throttle = 0;

            if (Harrier.Throttle > 100)
                Harrier.Throttle = 100;

            //weigh
            Harrier.Mass = Harrier.UnFueledMass + Harrier.Fuel;

            Harrier.YVel = Harrier.YVel - 9.8 / Harrier.TickPerSecond;

            double tfuel = (Harrier.MaxFuelIn1Sec - Harrier.MinFuelIn1Sec) * Harrier.Throttle / 100 + Harrier.MinFuelIn1Sec; //Now Burn Fuel
            Harrier.Fuel = Harrier.Fuel - tfuel / Harrier.TickPerSecond;
            if (Harrier.Fuel < 0) Harrier.Fuel = 0;

            Harrier.Force = Harrier.Throttle * Harrier.MaxForce / 100 * 0.95; // 0.95 allows for cold air bleed
            if (Harrier.Fuel <= 0) Harrier.Force = 0;

            // compute new x and y velocity
            double xx = 0, yy = 0;
            Move2DInDirHS(0, 0, DegToRad(270 - Harrier.ThrustVector), Harrier.Force, ref xx, ref yy);
            xx = xx * -1;
            yy = yy * -1;
            Harrier.YVel = Harrier.YVel + (yy / Harrier.Mass) / Harrier.TickPerSecond;  // thrust vector and throttle
            Harrier.XVel = Harrier.XVel + (xx / Harrier.Mass) / Harrier.TickPerSecond;

            if (Harrier.GroundEffect && Harrier.Y <= 16 + Harrier.SafeY2) // ground effect
            {
                // 16 meters when ground efect happens
                // max ground effect is 0.5 m/s acceleration
                double ge = apportionI(0 + Harrier.SafeY2, 16 + Harrier.SafeY2, Harrier.Y, 0, 0.5) / Harrier.TickPerSecond;
                Harrier.YVel = Harrier.YVel - ge / Harrier.TickPerSecond;
            }

            // adjust for base wind speed
            double wind_inKnots = Harrier.WindSpeed; // wind and ship speed //Form5->Edit6->Text.ToIntDef(0); 
            double wind_inMps = wind_inKnots * Harrier.ConvertKnotsToMps;
            double frictionHarrier = 0.013; // very dubious source for this
            double wind_effect = wind_inMps * frictionHarrier;
            Harrier.XVel = Harrier.XVel - wind_effect / Harrier.TickPerSecond;

            // adjust for wind gusts

            Harrier.WindSpeed = Harrier.MaxWindGust + (double)new Random().Next(100) / 50 + (Globals.TurnCount % 31 - 16) / 4.0;
            if (Harrier.WindSpeed < 0) Harrier.WindSpeed = 0;
            if (Harrier.WindSpeed > Harrier.MaxWindGust) Harrier.WindSpeed = Harrier.MaxWindGust;

            double windGust_inMps = Harrier.MaxWindGust * Harrier.ConvertKnotsToMps;
            double windGust_effect = windGust_inMps * frictionHarrier;
            Harrier.XVel = Harrier.XVel - windGust_effect / Harrier.TickPerSecond;


            // adjust for ship speed
            Harrier.X = Harrier.X - (Harrier.shipSpeed * Harrier.ConvertKnotsToMps / Harrier.TickPerSecond);

            Harrier.RelativeXVel = Harrier.XVel - Harrier.shipSpeed * Harrier.ConvertKnotsToMps;

            // compute new position
            Harrier.Y = Harrier.Y + Harrier.YVel / Harrier.TickPerSecond;
            Harrier.X = Harrier.X + Harrier.XVel / Harrier.TickPerSecond;


            double sh = 0;
            bool b = SafeZoneHS(Harrier.X, ref sh);
            if (Harrier.Y <= sh + 3)
            {
                // aircraft has Landed
                Harrier.Y = sh + 3;
                if (!b)
                {
                    Harrier.PlaneImage = SpriteList.SpaceShip_Explode;

                    Harrier.Boom = true;
                    Harrier.Landed = true;
                    Harrier.Y = sh + 3;
                    Harrier.Throttle = 0;
                    Harrier.ThrustVector = 0;
                    State = SimulatorStateEnum.Complete;
                    
                    Fail();

                    return;
                }
                //Failed Landing
                if (Math.Abs(Harrier.RelativeXVel) > Harrier.MaxLandingSpeedX || Math.Abs(Harrier.YVel) > Harrier.MaxLandingSpeedY)
                {
                    Harrier.PlaneImage = SpriteList.SpaceShip_Explode;

                    Harrier.Boom = true;
                    Harrier.Landed = true;
                    Harrier.Y = sh + 3;
                    Harrier.Throttle = 0;
                    Harrier.ThrustVector = 0;
                    
                    State = SimulatorStateEnum.Complete;

                    Fail();

                    return;
                }

                // Success we did not blow up
                Harrier.Landed = true;
                Harrier.PlaneImage = SpriteList.HarrierLanded;
                
                State = SimulatorStateEnum.Complete;

                Harrier.Y = sh + 3;
                Harrier.Throttle = 0;
                Harrier.ThrustVector = 0;
                
                //drawHS();
                
                return;
            }

            #endregion
        }

        #region Harrier Physics Dependancies 

        double ToX(double x)
        {
            return Harrier.MetersToPixels * x;
        }

        double ToY(double y)
        {
            double tmp = Harrier.MetersToPixels * y;
            return Globals.Simulator.FrameResolution.Y - tmp - 1;
        }


        /// <summary>
        /// returns true if in safe zone
        /// </summary>
        /// <param name="x"></param>
        /// <param name="safeHeight"></param>
        /// <returns></returns>
        bool SafeZoneHS(double x, ref double safeHeight)
        {
            //returns true if in safe zone
            if (x < Harrier.UnsafeX1)
            {
                safeHeight = Harrier.UnsafeY1;
                return false;
            }
            if (x > Harrier.SafeX2)
            {
                safeHeight = Harrier.UnsafeY3;
                return false;
            }
            
                safeHeight = Harrier.SafeY2;
            return true;
        }

        void Move2DInDirHS(double oldx, double oldz,
                 double yaw, double speed,
                 ref double resultx, ref double resultz)
        {
            double tmpx, tmpz;
            double newx, newz;

            /* set to origin */
            tmpx = 0;
            tmpz = speed; /* move us forward by speed */

            newx = (tmpz * (double)Math.Sin(yaw)) + (tmpx * (double)Math.Cos(yaw));
            newz = (tmpz * (double)Math.Cos(yaw)) - (tmpx * (double)Math.Sin(yaw));
            tmpx = newx;
            tmpz = newz;

            newx = tmpx + oldx;
            newz = tmpz + oldz;

            resultx = newx;
            resultz = newz;
        }

        double DegToRad(double deg)
        {
            return (deg * 3.14159 / 180.0);
        }


        /// <summary>
        /// This assumes that test val is in the range lowVal to highVal and finds its proportion
        /// it then returns the equivalent number in the range  returnRangeHigh to returnRangeLow
        /// this is identical to apportion except that is a proportion of 1-p
        /// </summary>
        /// <param name="lowVal"></param>
        /// <param name="highVal"></param>
        /// <param name="testVal"></param>
        /// <param name="returnRangeLow"></param>
        /// <param name="returnRangeHigh"></param>
        /// <returns></returns>
        double apportionI(double lowVal, double highVal, double testVal, double returnRangeLow, double returnRangeHigh)
        {
            double retv;
            if (highVal - lowVal == 0) return 0; // invalid equasion
            retv = (testVal - lowVal) / (highVal - lowVal); // retv now in range 0..1
            return lerp(1 - retv, returnRangeLow, returnRangeHigh);
            
            
        }

        /// <summary>
        ///  for lerp t is in the range 0 to 1 a=minval b=maxval
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        double lerp(double t, double a,  double b)
        {
            return a + t*(b - a);
        }

        #endregion 
    }
}
