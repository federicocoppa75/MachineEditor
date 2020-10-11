using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.ToolHolders
{
    [XmlInclude(typeof(StaticToolHolder))]
    [XmlInclude(typeof(AutoSourceToolHolder))]
    [XmlInclude(typeof(AutoSinkToolHolder))]
    [Serializable]
    public class ToolHolder : IToolHolder
    {
        public int Id { get; set; } = -1;

        public Vector Position { get; set; } = new Vector() { X = 0.0, Y = 0.0, Z = 0.0 };

        public Vector Direction { get; set; } = new Vector() { X = 0.0, Y = 0.0, Z = -1.0 };
    }
}
