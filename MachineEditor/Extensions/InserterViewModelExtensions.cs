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
    public static class InserterViewModelExtensions
    {
        public static BaseInserter ToModel(this InserterBaseViewModel vm)
        {
            BaseInserter m = null;

            if(vm is InserterViewModel insVm)
            {
                var ins = new Inserter
                {
                    Diameter = insVm.Diameter,
                    Length = insVm.Length,
                    LoaderLinkId = insVm.LoaderLinkId,
                    DischargerLinkId = insVm.DischargerLinkId
                };

                m = ins;
            }
            else if(vm is InjectorViewModel injVm)
            {
                m = new Injector();
            }

            m?.UpdateFrom(vm);

            return m;
        }

        private static void UpdateFrom(this BaseInserter m, InserterBaseViewModel vm)
        {
            m.Id = vm.Id;
            m.Position = new MachineModels.Models.Vector() { X = vm.Position.X, Y = vm.Position.Y, Z = vm.Position.Z };
            m.Direction = new MachineModels.Models.Vector() { X = vm.Direction.X, Y = vm.Direction.Y, Z = vm.Direction.Z };
            m.Color = vm.Color.Convert();
        }
    }
}
