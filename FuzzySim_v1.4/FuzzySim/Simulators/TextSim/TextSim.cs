using System;
using System.Drawing;
using FuzzySim.Core;
using FuzzySim.Rendering;

namespace FuzzySim.Simulators
{
    public class TextSim : AISimulator
    {
        public double DefuzziedValue1;
        public double DefuzziedValue2;

        public override void Succeed()
        {
            throw new NotImplementedException();
        }
        public override void Fail()
        {
            throw new NotImplementedException();
        }

        public override SimFrame Init(Vec2 rez)
        {
            State = SimulatorStateEnum.Initialised;

            return new SimFrame();
        }

        public override SimFrame DrawTurn(Vec2 scale)
        {
            Debug.Trace("Text Simulator");

            SimFrame ret = new SimFrame { Error = false, ToBeDrawn = true, BackGround = Color.White };

            ret.AddText(String.Format("Value of E1 E2 Union: {0}", DefuzziedValue1.ToString()), new Vec2(100, 200), new SolidBrush(Color.Black));
            ret.AddText(String.Format("Value of E1 E2 Inter: {0}", DefuzziedValue2.ToString()), new Vec2(100, 230), new SolidBrush(Color.Black));
            
            return ret;
        }

        public override SimFrame DrawTurn(SimFrame frame)
        {
            return frame;
        }

        public override void DoTurn()
        {
            Globals.Controller.CalculateFuzzyLogic();

            State = SimulatorStateEnum.Complete;
        }
            
    }
}
