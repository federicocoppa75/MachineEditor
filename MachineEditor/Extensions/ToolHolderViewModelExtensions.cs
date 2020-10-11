using MachineEditor.ViewModels.ToolHolders;
using MachineModels.Models.ToolHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Extensions
{
    public static class ToolHolderViewModelExtensions
    {
        public static ToolHolder ToModel(this IToolHolderViewModel vm)
        {
            ToolHolder m = null;

            if(vm is StaticToolHolderViewModel svm)
            {
                var sm = new StaticToolHolder();
                UpdateModel(sm, svm);
                m = sm;
            }
            else if(vm is AutoSourceToolHolderViewModel asrcvm)
            {
                var am = new AutoSourceToolHolder();
                UpdateModel(am, asrcvm);
                m = am;
            }
            else if (vm is AutoSinkToolHolderViewModel asnkvm)
            {
                var am = new AutoSourceToolHolder();
                UpdateModel(am, asnkvm);
                m = am;
            }

            return m;
        }

        private static void UpdateModel(ToolHolder m, ToolHolderViewModel vm)
        {
            m.Id = vm.Id;
            m.Position = new MachineModels.Models.Vector() { X = vm.Position.X, Y = vm.Position.Y, Z = vm.Position.Z };
            m.Direction = new MachineModels.Models.Vector() { X = vm.Direction.X, Y = vm.Direction.Y, Z = vm.Direction.Z };
        }
    }
}
