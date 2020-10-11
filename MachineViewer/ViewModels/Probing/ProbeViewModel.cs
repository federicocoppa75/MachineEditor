using MachineViewer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Probing
{
    public class ProbeViewModel : MachineElementViewModel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public virtual ProbeType ProbeType => ProbeType.None;

        public void DetachFromParent() => Parent.Children.Remove(this);
        
    }
}
