using MachineViewModels.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Messages
{
    public class UpdateLinkViewModelsListMessage
    {
        public IEnumerable<ILinkViewModel> LinkViewModels { get; set; }
    }
}
