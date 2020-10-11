using GalaSoft.MvvmLight;
using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewModels.ViewModels.Links
{
    public class DynamicLinkViewModel : ViewModelBase, ILinkViewModel
    {
        public int Id { get; set; }
        public LinkDirection Direction { get; set; }
    }
}
