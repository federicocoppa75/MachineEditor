using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Enums
{
    public enum ActionType
    {
        Base,
        AddPanel,
        Link,
        LinearPositionLink,
        TwoPositionLink,
        LoadTool,
        UnloadTool,
        LinearPositionLinkGantryOn,
        LinearPositionLinkGantryOff,
        LinearInterpolatedPositionLink,
        ArcInterpolatedPositionLink,
        Inject,
        TurnOnInverter,
        TurnOffInverter,
        UpdateRotationSpeed
    }
}
