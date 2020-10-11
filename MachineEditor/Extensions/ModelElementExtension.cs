using MachineEditor.ViewModels;
using MachineModels.Models;
using MachineViewModels.Extensions;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineEditor.Extensions;

namespace MachineEditor.Extensions
{
    public static class ModelElementExtension
    {
        public static MachineElementViewModel ToViewModel(this MachineModels.Models.MachineElement me, MachineElementViewModel parentViewModel = null)
        {
            var vm = new MachineElementViewModel()
            {
                Name = me.Name,
                ModelFile = me.ModelFile,
                Color = me.Color.Convert(),
                LinkToParentType = me.LinkToParentType,
                LinkToParentData = me.LinkToParentData.Convert(),
                Parent = parentViewModel,
                ToolHolderType = me.ToolHolderType,
                ToolHolderData = me.ToolHolderData?.ToViewModel(),
                ColliderGeometry = me.ColiderType,
                Collider = me.Collider.ToViewMode(),
                HasPanelHolder = me.HasPanelHolder,
                PanelHolder = me.PanelHolder.ToViewModel(),
                InserterType = me.InserterType,
                InserterData = me.Inserter.ToViewModel()
            };

            vm.Children = me.Children.Convert(vm);

            if(vm.Model != null) vm.Model.Transform = new System.Windows.Media.Media3D.MatrixTransform3D(me.TrasformationMatrix3D.Convert());

            return vm;
        }

        private static ObservableCollection<MachineElementViewModel> Convert(this IList<MachineElement> elements, MachineElementViewModel parentViewModel)
        {
            return new ObservableCollection<MachineElementViewModel>(elements.Select((e) => e.ToViewModel(parentViewModel)));
        }
          
    }
}
