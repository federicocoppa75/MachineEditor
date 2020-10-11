using MachineModels.Models.ToolHolders;
using MachineViewer.ViewModels.ToolHolder;
using System.Windows.Media.Media3D;

namespace MachineViewer.Extensions
{
    public static class ToolHolderViewModelExtensions
    {
        public static ToolHolderViewModel UpdateFromModel(this ToolHolderViewModel vm, ToolHolder m)
        {
            vm.ToolHolderId = m.Id;
            vm.Position = new Point3D(m.Position.X, m.Position.Y, m.Position.Z);
            vm.Direction = new Vector3D(m.Direction.X, m.Direction.Y, m.Direction.Z);

            return vm;
        }
    }
}
