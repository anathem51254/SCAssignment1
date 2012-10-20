using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace FuzzySim.Rendering
{
    static class SpriteList
    {
        //Backgrounds
        public static Image MoonSurface, MoonSurface_Flip;
        public static Image SeaSurface, SeaSurface_Flip;

        //MOONLANDER
        public static Image SpaceShip;
        public static Image SpaceShip_Explode;
        public static Image SpaceShip_Landed;
        public static Image ThrustDown, ThrustLeft, ThrustRight;
        public static Image LunarMoon;
        public static Image Rock;

        //HARRIER
        public static Image Harrier;
        public static Image Tornado;
        public static Image HarrierLanded;
        public static Image TornadoLanded;
        public static Image ShipAnzac;
        public static Image ShipAustralia;
        public static Image ShipEnterprise;

        //General
        public static Image Error;


        /// <summary>
        /// Sets up the image sprites for use by the Frame class. 
        /// 
        /// ** I DONT LIKE THIS. Maybe keep image files as 'content' for the assemblies? Depends on final relase format..-TF
        /// </summary>
        public static void Initialize()
        {

            string path = @"..\..\Images\";

            #region MOON LANDER STUFF

            MoonSurface = Image.FromFile(path + "moonSurface.jpg");
            MoonSurface_Flip = Image.FromFile(path + "moonSurface1.jpg");

            SpaceShip = Image.FromFile(path + "Lander3.png");
            SpaceShip_Explode = Image.FromFile(path + "explode.png");
            SpaceShip_Landed = Image.FromFile(path + "Lander.png");

            //Cant deny slight laziness with the additional images for left/right thrust... lol. Does the trick.
            ThrustDown = Image.FromFile(path + "thrust.png");
            ThrustLeft = Image.FromFile(path + "l_thrust.png");
            ThrustRight = Image.FromFile(path + "r_thrust.png");

            Rock =Image.FromFile(path + "rock1.png");

            #endregion

            #region HARRIER STUFF

            SeaSurface = Image.FromFile(path + "sea.png");

            Harrier = Image.FromFile(path + "GR1.png");
            Tornado = Image.FromFile(path + "GR1.png");
            HarrierLanded = Image.FromFile(path + "AV8bLand.png");
            TornadoLanded = Image.FromFile(path + "GR1Land.png");
            ShipAnzac = Image.FromFile(path + "Anzac.png");
            ShipAustralia = Image.FromFile(path + "AustraliaLPH1.png");
            ShipEnterprise = Image.FromFile(path + "Enterprise.png");

            #endregion

            Error = Image.FromFile(path + "error.png");
        }
    }
}
