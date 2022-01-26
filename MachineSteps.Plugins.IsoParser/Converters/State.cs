using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoParser.Converters.StateData;

namespace MachineSteps.Plugins.IsoParser.Converters
{
    public class State
    {
        public Panel Panel { get; private set; } = new Panel();

        private IAxes _axes;
        public IAxes Axes 
        {
            get => _axes ?? (_axes = new Axes(-1000.5, 1168.0, -58.0, 1583.0, 0.0, 0.0, 0.0, 0.0)); 
            private set => _axes = value; 
        } 

        public Clamps Clamps { get; private set; } = new Clamps();

        public Driller Driller1 { get; private set; } = new Driller();

        public Driller Driller2 { get; private set; } = new Driller();

        public Doweler Doweler1 { get; private set; } = new Doweler();

        public Doweler Doweler2 { get; private set; } = new Doweler();

        public int SelectedHead { get; set; }

        public int HeadOrder { get; set; }

        public int SelectedHeadTurnedOn { get; set; }

        public int HeadOrderTurnedOn { get; set; }

        public int RotationSpeed { get; set; }

        public int FeedSpeed { get; set; }

        public ToolChange ToolChange { get; set; } = new ToolChange();

        public int StopLinkTurnedOn { get; set; }

        public Plane Plane { get; set; } = new Plane();

        public State()
        {
            if((StateInfoServices.GetLinearLinksCount != null) && (StateInfoServices.GetLinearLinksCount() == 3))
            {
                _axes = new XYZAxes();
            }
            else
            {
                _axes = new Axes(-1000.5, 1168.0, -58.0, 1583.0, 0.0, 0.0, 0.0, 0.0);
            }
        }
    }
}
