using MachineModels.Models.Inserters;
using MachineViewer.ViewModels;
using MachineViewer.ViewModels.Inserter;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Extensions
{
    public static class InserterExtensions
    {
        public static MachineElementViewModel ToViewModel(this BaseInserter m, MachineElementViewModel parent = null)
        {
            InserterBaseViewModel vm = null;

            if(m is Inserter ins)
            {
                var insVm = new InserterViewModel()
                {
                    Length = ins.Length,
                    Diameter = ins.Diameter,
                    LoaderLinkId = ins.LoaderLinkId,
                    DischargerLinkId = ins.DischargerLinkId
                };

                vm = insVm;
            }
            else if(m is Injector inj)
            {
                vm = new InjectorViewModel();
            }

            vm?.UpdateFrom(m);
            if (vm != null && parent != null) vm.Parent = parent;

            return vm;
        }

        public static void UpdateFrom(this InserterBaseViewModel vm, BaseInserter m)
        {
            vm.InserterId = m.Id;
            vm.Position = new System.Windows.Media.Media3D.Point3D(m.Position.X, m.Position.Y, m.Position.Z);
            vm.Direction = new System.Windows.Media.Media3D.Vector3D(m.Direction.X, m.Direction.Y, m.Direction.Z);
            vm.Color = m.Color.Convert();
        }
    }
}
