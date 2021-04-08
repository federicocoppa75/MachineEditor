using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Extensions
{
    public static class AngolarTransmissionExtensions
    {
        public static AngolarTransmissionImpl ToImplementation(this AngolarTransmission at, Dictionary<string, Tool> tools)
        {
            var ati = new AngolarTransmissionImpl() 
            {
                Name = at.Name,
                Description = at.Description,
                ToolLinkType = at.ToolLinkType,
                BodyModelFile = at.BodyModelFile 
            };

            foreach (var item in at.Subspindles)
            {
                if(tools.TryGetValue(item.ToolName, out Tool tool))
                {
                    ati.Subspindles.Add(new AngolarTransmissionImpl.Subspindle()
                    {
                        Tool = tool,
                        Position = item.Position,
                        Direction = item.Direction
                    });
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            return ati;
        }
    }
}
