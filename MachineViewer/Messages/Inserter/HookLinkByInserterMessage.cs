using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Inserter
{
    public class HookLinkByInserterMessage
    {
        public int InserterId { get; set; }
        public int LinkId { get; set; }
    }
}
