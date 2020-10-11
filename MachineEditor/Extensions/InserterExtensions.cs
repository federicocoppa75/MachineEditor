using MachineEditor.ViewModels.Inserters;
using MachineModels.Models.Inserters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineViewModelUtils.Extensions;

namespace MachineEditor.Extensions
{
    public static class InserterExtensions
    {
        public static InserterBaseViewModel ToViewModel(this BaseInserter m)
        {
            InserterBaseViewModel vm = null;

            if(m is Injector inj)
            {
                vm = new InjectorViewModel();                
            }
            else if(m is Inserter ins)
            {
                var insVm = new InserterViewModel
                {
                    Diameter = ins.Diameter,
                    Length = ins.Length,
                    LoaderLinkId = ins.LoaderLinkId,
                    DischargerLinkId = ins.DischargerLinkId
                };
                vm = insVm;
            }

            vm?.UpdateFrom(m);

            return vm;
        }

        private static void UpdateFrom(this InserterBaseViewModel vm, BaseInserter m)
        {
            vm.Id = m.Id;
            vm.Position = new System.Windows.Media.Media3D.Point3D(m.Position.X, m.Position.Y, m.Position.Z);
            vm.Direction = new System.Windows.Media.Media3D.Vector3D(m.Direction.X, m.Direction.Y, m.Direction.Z);
            vm.Color = m.Color.Convert();
        }
    }
}
