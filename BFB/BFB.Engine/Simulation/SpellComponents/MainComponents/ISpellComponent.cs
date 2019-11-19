using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.SpellComponents.MainComponents
{
    public interface ISpellComponent
    {
        void Update(SimulationEntity simulationEntity, Simulation simulation);
        void OnUse(SimulationEntity simulationEntity, Simulation simulation, BfbVector mouse);
    }
}
