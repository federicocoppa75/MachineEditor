using MachineEditor.ViewModels.ToolHolders;
using MachineModels.Models.ToolHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Extensions
{
    public static class ToolHolderExtensions
    {
        public static IToolHolderViewModel ToViewModel(this IToolHolder model)
        {
            IToolHolderViewModel vm = null;

            if(model is StaticToolHolder sth)
            {
                var svm = new StaticToolHolderViewModel();
                UpdateViewModel(svm, sth);
                vm = svm;

            }
            else if (model is AutoSourceToolHolder asrcth)
            {
                var avm = new AutoSourceToolHolderViewModel();
                UpdateViewModel(avm, asrcth);
                vm = avm;
            }
            else if (model is AutoSinkToolHolder asnkth)
            {
                var avm = new AutoSourceToolHolderViewModel();
                UpdateViewModel(avm, asnkth);
                vm = avm;
            }

            return vm;
        }

        private static void UpdateViewModel(ToolHolderViewModel vm, ToolHolder m)
        {
            vm.Id = m.Id;
            vm.Position = new System.Windows.Media.Media3D.Point3D() { X = m.Position.X, Y = m.Position.Y, Z = m.Position.Z };
            vm.Direction = new System.Windows.Media.Media3D.Vector3D() { X = m.Direction.X, Y = m.Direction.Y, Z = m.Direction.Z };
        }
    }
}
