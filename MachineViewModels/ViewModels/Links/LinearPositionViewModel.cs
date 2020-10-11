using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewModels.ViewModels.Links
{
    public class LinearPositionViewModel : DynamicLinkViewModel
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Pos { get; set; }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                double oldValue = _value;

                if (Set(ref _value, value, nameof(Value)))
                {
                    OldValue = oldValue;
                    OnValueChanged();
                    ValueChanged?.Invoke(this, _value); 
                }
            }
        }

        [Browsable(false)]
        public double OldValue { get; private set; }

        [Browsable(false)]
        public EventHandler<double> ValueChanged;

        protected virtual void OnValueChanged() { }

        public void OnMasterValueChanged(object s, double e)
        {
            if (s is LinearPositionViewModel m)
            {
                Value += (m.Value - m.OldValue);
            }
            else
            {
                throw new ArgumentException("The master link is not the same type!");
            }
        }
     }
}
