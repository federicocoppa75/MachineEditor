using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.WorkActivator
{
    [XmlInclude(typeof(AnalogicActivator))]
    [XmlInclude(typeof(DigitalActivator))]
    [Serializable]
    public abstract class BaseActivator
    {
        public int Id { get; set; } = -1;

        public string Name { get; set; }

        public string Description { get; set; }

        public Vector Position { get; set; } = new Vector() { X = 0.0, Y = 0.0, Z = 0.0 };

        public Vector Direction { get; set; } = new Vector() { X = 0.0, Y = 0.0, Z = -1.0 };

        public abstract ActivatorType ActivatorType { get; }
    }
}
