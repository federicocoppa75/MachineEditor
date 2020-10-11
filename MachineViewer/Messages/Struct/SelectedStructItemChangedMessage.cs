using MachineViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Struct
{
    public class SelectedStructItemChangedMessage
    {
        public MachineElementViewModel Value { get; set; }
    }
}
