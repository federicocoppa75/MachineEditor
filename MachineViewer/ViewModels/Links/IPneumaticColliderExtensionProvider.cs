using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Links
{
    public interface IPneumaticColliderExtensionProvider
    {
        double CollisionOnPos { get; set; }

        bool HasCollision { get; set; }

        double OffPos { get; }

        double OnPos { get; }

        double TOff { get; }

        double TOn { get; }

        bool Value { get; }

        LinkDirection Direction { get; }

        Action<IPneumaticColliderExtensionProvider> EvaluateCollision { get; set; }

        Action<IPneumaticColliderExtensionProvider> OnMovementCompleted { get; set; }

        Action<IPneumaticColliderExtensionProvider> OnMovementStarting { get; set; }

        bool IsGradualTransactionEnabled { get; }
    }
}
