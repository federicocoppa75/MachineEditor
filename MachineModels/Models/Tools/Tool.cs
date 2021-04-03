using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.Tools
{
    [XmlInclude(typeof(SimpleTool))]
    [XmlInclude(typeof(DiskTool))]
    [XmlInclude(typeof(PointedTool))]
    [XmlInclude(typeof(TwoSectionTool))]
    [XmlInclude(typeof(CountersinkTool))]
    [XmlInclude(typeof(DiskOnConeTool))]
    [XmlInclude(typeof(AngolarTransmission))]
    [Serializable]
    public abstract class Tool
    {
        public string Name { get; set; }

        public string Description { get; set; }        

        public abstract double GetTotalDiameter();

        public abstract double GetTotalLength();

        public virtual ToolType ToolType => ToolType.Base;

        public ToolLinkType ToolLinkType { get; set; }

        public string ConeModelFile { get; set; }
    }
}
