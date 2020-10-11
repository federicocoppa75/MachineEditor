using MachineModels.Models.PanelHolders;
using MachineViewer.ViewModels;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Extensions
{
    public static class PanelHolderExtensions
    {
        public static PanelHolderViewModel ToViewModel(this PanelHolder m, MachineElementViewModel parent = null)
        {
            var builder = new HelixToolkit.Wpf.MeshBuilder();

            builder.AddSphere(m.Position.ToPoint3D(), 10.0);

            //return new PanelHolderViewModel()
            return new WorkablePanelViewModel()
            {
                PanelHolderId = m.Id,
                Name = m.Name,
                Corner = m.Corner,
                Position = m.Position.ToPoint3D(),
                Parent = parent,
                MeshGeometry = builder.ToMesh(),
                Material = HelixToolkit.Wpf.Materials.Blue
            };
        }
    }
}
