using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.Links
{
    [XmlInclude(typeof(LinearPosition))]
    [XmlInclude(typeof(LinearPneumatic))]
    [XmlInclude(typeof(RotaryPneumatic))]
    [Serializable]
    public class Link : ILink
    {        
    }
}
