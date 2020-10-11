using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewModels.ViewModels.Links
{
    public class TwoPositionLinkViewModel : DynamicLinkViewModel
    {
        public double OffPos { get; set; }
        public double OnPos { get; set; }
        public double TOff { get; set; }
        public double TOn { get; set; }
        public bool ToolActivator { get; set; }
        public int InserterId { get; set; }

        private bool _value;
        public bool Value
        {
            get { return _value; }
            set
            {
                if (Set(ref _value, value, nameof(Value)))
                {
                    OnValueChanged();
                    ValueChanged?.Invoke(this, _value);
                }
            }
        }

        [Browsable(false)]
        public EventHandler<bool> ValueChanged;

        protected virtual void OnValueChanged() { }
    }
}
