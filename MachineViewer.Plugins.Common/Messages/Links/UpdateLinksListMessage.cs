using MachineModels.Models.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class UpdateLinksListMessage
    {
        public IEnumerable<Tuple<ILink, string>> Links { get; set; }
    }
}
