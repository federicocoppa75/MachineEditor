using MachineEditor.ViewModels;
using MachineModels.Models;
using MachineViewModels.Extensions;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineEditor.Extensions;

namespace MachineEditor.Extensions
{
    public static class MachineElementViewModelExtension
    {
        public static MachineElement ToModel(this MachineElementViewModel vm)
        {
            var mv3d = vm.Model as System.Windows.Media.Media3D.ModelVisual3D;

            return new MachineElement()
            {
                Name = vm.Name,
                ModelFile = vm.ModelFile,
                Color = vm.Color.Convert(),
                TrasformationMatrix3D = Convert((mv3d != null) ? mv3d.Transform.Value : System.Windows.Media.Media3D.Matrix3D.Identity),
                Children = vm.Children.Convert(),
                LinkToParentType = vm.LinkToParentType,
                LinkToParentData = vm.LinkToParentData.Convert(),
                ToolHolderType = vm.ToolHolderType,
                ToolHolderData = vm.ToolHolderData?.ToModel(),
                ColiderType = vm.ColliderGeometry,
                Collider = vm.Collider.ToModel(),
                HasPanelHolder = vm.HasPanelHolder,
                PanelHolder = vm.PanelHolder.ToModel(),
                InserterType = vm.InserterType,
                Inserter = vm.InserterData.ToModel()
            };
        }

        private static List<MachineElement> Convert(this IList<MachineElementViewModel> vms)
        {
            return vms.Select((e) => e.ToModel()).ToList();
        }

        private static MachineModels.Models.Matrix3D Convert(System.Windows.Media.Media3D.Matrix3D m)
        {
            return new Matrix3D()
            {
                M11 = m.M11,
                M12 = m.M12,
                M13 = m.M13,
                M14 = m.M14,
                M21 = m.M21,
                M22 = m.M22,
                M23 = m.M23,
                M24 = m.M24,
                M31 = m.M31,
                M32 = m.M32,
                M33 = m.M33,
                M34 = m.M34,
                OffsetX = m.OffsetX,
                OffsetY = m.OffsetY,
                OffsetZ = m.OffsetZ,
                M44 = m.M44,
            };
        }

    }
}
