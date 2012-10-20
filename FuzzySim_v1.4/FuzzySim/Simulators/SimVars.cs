
namespace FuzzySim
{
    using System.Drawing;

    public class SimVars
    {
        /// <summary>
        /// Variables consumed by the MOON LANDER Simulator
        /// </summary>
        public class MoonLanderVars 
        {
            public const float MAX_MOONX = 500;
            public const float MAX_MOONY = 4500;
            public const float MOON_SURFACE_Y = 20f;
            public const float MOON_GRAVITY = 1.633f;

            public const float LANDING_X_BASE = 1.22f;
            public const float LANDING_Y_BASE = 3.05f;

            public const float SIM_HEIGHT = 5000;


            public double X { get; set; }               // spaceship location (init y=4,000m)
            public double Y { get; set; }               
                                                        
            public double XVel { get; set; }            // spaceship x and y velocity
            public double YVel { get; set; }            
            public double Right { get; set; }              // spaceship right and left thruster status 0-5 (Hundred newtons)
            public double Left { get; set; }               
            public double Throttle { get; set; }           // throtle setting 0-100 (0,10-60 or 90)
            public double DownForce { get; set; }       // main thruster status 0, 5-44 (thousand newtons)
            public double RightForce { get; set; }      // main thruster status 0-5 (Hundred newtons)
            public double LeftForce { get; set; }       // main thruster status 0-5 (Hundred newtons)
            public double Fuel { get; set; }            // 0-8,845 kg of Fuel.
            public double Mass { get; set; }            // 5655 + Fuel

            public double WFactor { get; set; }         // width factor;
            public double HFactor { get; set; }         // height factor
            public double MaxFuelIn1Sec { get; set; }
            public bool Boom { get; set; }
            public bool Safe { get; set; }
            public bool Landed { get; set; }

            public int SafeX;                           // safe to land here
            public int SafeXWidth;                      // width 10000=no rocks

            public double LandingVelX;                  // max x velocity for landing;
            public double LandingVelY;                  // max y velocity for landing;
            public double Variability;                  // 0 = none  0.01 = [+/- 1%]

        }

        /// <summary>
        /// Variables consumed by the HARRIER Simulator
        /// </summary>
        public class HarrierVars
        {
            public double X, Y;           // harrier cog location in meters
            public double XVel, YVel;     // x and y velosity in meters/second
            public double RCSNose, RCSTail;     // RCS nose and tail thruster status 0-4.4 (kilo newtons)
            public double Throttle;      // throtle setting 0-100% (0,1=idle up to 100)
            public double MaxForce;     // main engine 0-44 (thousand newtons)
            public double Force;         // main thruster status 0-44 (thousand newtons)
            public double RightForce;   // main thruster status 0-5 (Hundred newtons)
            public double LeftForce;    // main thruster status 0-5 (Hundred newtons)
            public double Fuel;          // kg of Fuel.
            public double UnFueledMass; // kg
            public double Mass;          // unfueled_mass+Fuel
            public double ThrustVector;            // thrust vector initialy 90
            public double shipSpeed;    // Ship speed in knots
             
            public double MaxFuelIn1Sec;
            public double MinFuelIn1Sec;
            public bool Boom;
            public bool Landed;
             
            public double UnsafeX1;            // safe to land here meters
            public double SafeX2;           // safe to land here meters
            public double UnsafeX3;          // width in meters meters
            public double UnsafeY1;       // in meters
            public double SafeY2;       // in meters
            public double UnsafeY3;       // in meters
            public double MidSafeX;
            
            public double MaxLandingSpeedX;    // max x speed for landing;
            public double MaxLandingSpeedY;    // max y speed for landing;
            public double WindSpeed;          // 0 = none  0.01 = pluss or -1%
            public double WindGustTarget;    // 0 = none  0.01 = pluss or -1%
             
            public double MetersToPixels;
            public double PixelsToMeters;

            public double CarrierHeightPixels;    // size of png image
            public double CarrierWidthPixels;
            public double CarrierLandingHeightPixels;

            public double CarrierImageX;       // Where to blit the carrier in meters
            public double CarrierImageY;

            public double TickPerSecond;
            public double InitSpeedKnots;
            public double MaxWindGust;   // 0 to high
            public double RelativeXVel; // relative speed to ship

            public Image ShipImage;
            public Image PlaneImage;

            public bool GroundEffect;

            public double ConvertKnotsToMps = 0.514;
            public double SafeYDraw;
        }


        /// <summary>
        /// Variables consumed by the TEXT Simulator
        /// </summary>
        public class TextVars
        {
             
        }


        /// <summary>
        /// Variables consumed by the RANDOMALITY Simulator
        /// </summary>
        public class RandomVars
        {
            public int Step { get; set; }

            public RandomVars()
            {
                Step = 1;
            }
        }

    }
}
