using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Links
{
    public interface IPneumaticPresserExtensionProvider : IPneumaticColliderExtensionProvider
    {
        double Pos { get; set; }
    }
}
