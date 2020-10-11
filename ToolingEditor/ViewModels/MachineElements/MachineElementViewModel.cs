using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.MachineElements
{
    public class MachineElementViewModel : ViewModelBase
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<MachineElementViewModel> Children { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { Set(ref _isExpanded, value, nameof(IsExpanded)); }
        }
    }
}
