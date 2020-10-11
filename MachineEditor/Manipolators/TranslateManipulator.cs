using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MachineEditor.Manipolators
{
    public class TranslateManipulator : HelixToolkit.Wpf.TranslateManipulator
    {
        ViewModels.MachineElementViewModel _vm;

        public void Bind(ViewModels.MachineElementViewModel vm)
        {
            _vm = vm;
            base.Bind(vm.Model);
        }

        public new void UnBind()
        {
            _vm = null;
            base.UnBind();
        }

        protected override void ValueChanged(DependencyPropertyChangedEventArgs e)
        {
            base.ValueChanged(e);
            _vm?.NotifyModelLOcationChanged();
        }
    }
}
