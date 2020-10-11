using MachineModels.Enums;
using MachineModels.Models.Colliders;
using MachineModels.Models.Links;
using MachineModels.Models.PanelHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models
{
    [Serializable]
    public class MachineElement
    {
        public string Name { get; set; }

        public string ModelFile { get; set; }

        public Color Color { get; set; }

        public Matrix3D TrasformationMatrix3D { get; set; }

        public List<MachineElement> Children { get; set; } = new List<MachineElement>();

        public LinkType LinkToParentType { get; set; }

        public Link LinkToParentData { get; set; }

        public ToolHolderType ToolHolderType { get; set; }

        public ToolHolders.ToolHolder ToolHolderData { get; set; }

        public bool HasPanelHolder { get; set; }

        public PanelHolder PanelHolder { get; set; }

        public ColliderGeometry ColiderType { get; set; }

        public Collider Collider { get; set; }

        public InserterType InserterType { get; set; }

        public Inserters.BaseInserter Inserter { get; set; }
    }
}
