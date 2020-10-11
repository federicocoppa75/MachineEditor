using MachineViewer.ViewModels;
using MachineViewer.ViewModels.ToolHolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Extensions
{
    public static class MachineELementViewModelExtennsions
    {
        public static Transform3D GetChainTansform(this MachineElementViewModel vm)
        {
            if (vm != null)
            {
                var tg = new Transform3DGroup();

                AddParentTansform(vm, tg);

                return tg;
            }
            else
            {
                return null;
            }
        }

        private static void AddParentTansform(MachineElementViewModel vm, Transform3DGroup tg)
        {
            tg.Children.Add(vm.Transform);

            if (vm.Parent != null)
            {
                AddParentTansform(vm.Parent, tg);                
            }
        }

        public static void ManageToolActivation(this MachineElementViewModel vm, bool value)
        {
            if(vm is ToolHolderViewModel thvm)
            {
                thvm.ActiveTool = value;
            }
            else
            {
                foreach (var item in vm.Children)
                {
                    var child = item as MachineElementViewModel;

                    child.ManageToolActivation(value);
                }
            }
        }
    }
}
