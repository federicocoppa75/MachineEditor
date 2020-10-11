using MachineEditor.ViewModels.PanelHolders;
using MachineModels.Models.PanelHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Extensions
{
    public static class PanelHolderExtensions
    {
        public static PanelHolderViewModel ToViewModel(this PanelHolder m)
        {
            if (m != null)
            {
                return new PanelHolderViewModel()
                {
                    Corner = m.Corner,
                    Id = m.Id,
                    Name = m.Name,
                    Position = new System.Windows.Media.Media3D.Point3D(m.Position.X, m.Position.Y, m.Position.Z)
                };
            }
            else
            {
                return null;
            }
        }

        public static PanelHolder ToModel(this PanelHolderViewModel vm)
        {
            if (vm != null)
            {
                return new PanelHolder()
                {
                    Corner = vm.Corner,
                    Id = vm.Id,
                    Name = vm.Name,
                    Position = new MachineModels.Models.Vector() { X = vm.Position.X, Y = vm.Position.Y, Z = vm.Position.Z }
                };
            }
            else
            {
                return null;
            }
        }
    }
}
