using System;
using FuzzySim.Core;
using FuzzySim.Rendering;

namespace FuzzySim.Simulators
{
        /// <summary>
        /// A whole new simulator for Debugging purposes, just for lulz.
        /// Renders sprites across the screen, and floats them up gradually.
        /// Initial sprite placement is random - aims to determine the influence 
        /// of computation rate on [psuedo]Randomality. - TF
        /// </summary>
        
       class RandomalitySimulator : AISimulator
       {
            private Sprite _template;

            private int _population = 40;
            private int _spacing = 30;
     
            private double[] _doubles;

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
                _doubles = new double[_population];
                _template = new Sprite("x")
                               {
                                   Picture = SpriteList.SpaceShip_Explode
                               };

                for (int i = 0; i < _population; i++)
                {
                    _doubles[i] = ((double) (new Random().NextDouble()*Globals.Simulator.FrameResolution.X));
                }

                State = SimulatorStateEnum.Initialised;

                return new SimFrame();
            }

            public override SimFrame DrawTurn(Vec2 scale)
            {
                State = SimulatorStateEnum.Running;

                SimFrame ret = new SimFrame();

                for (int i = 0; i < _population; i++)
                {
                    ret.AddRenderable(
                        new Sprite("x"+i)
                            {
                                Picture =  _template.Picture, 
                                Position = new Vec2(_doubles[i], i * _spacing)
                            });
                }

                ret.ToBeDrawn = true;

                return ret;
            }

            public override SimFrame DrawTurn(SimFrame frame)
            {
                throw new NotImplementedException();
            }

            public override void DoTurn()
            {
                for (int i = 0; i < _population - 1; i++)
                {
                    _doubles[i] = _doubles[i + 1];
                }


                _doubles[_population - 1] = ((double)(new Random().NextDouble() * Globals.Simulator.FrameResolution.X)
                                    ); //+(double) (Globals.Simulator.FrameResolution.X/2)
            }
    }


}
