using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineSteps.Models.Actions
{
    [XmlInclude(typeof(LinearPositionLinkAction))]
    [XmlInclude(typeof(TwoPositionLinkAction))]
    [XmlInclude(typeof(AddPanelAction))]
    [XmlInclude(typeof(LoadToolAction))]
    [XmlInclude(typeof(UnloadToolAction))]
    [XmlInclude(typeof(LinearPositionLinkGantryOffAction))]
    [XmlInclude(typeof(LinearPositionLinkGantryOnAction))]
    [XmlInclude(typeof(LinearInterpolatedPositionLinkAction))]
    [XmlInclude(typeof(ArcInterpolatedPositionLinkAction))]
    [XmlInclude(typeof(InjectAction))]
    [XmlInclude(typeof(TurnOffInverterAction))]
    [XmlInclude(typeof(TurnOnInverterAction))]
    [XmlInclude(typeof(UpdateRotationSpeedAction))]
    [Serializable]
    public abstract class BaseAction : IBaseAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
