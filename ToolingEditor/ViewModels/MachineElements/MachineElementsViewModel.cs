using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Models;
using MachineModels.Models.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolingEditor.Messages;

namespace ToolingEditor.ViewModels.MachineElements
{
    public class MachineElementsViewModel : ViewModelBase
    {
        public List<MachineElementViewModel> Elements { get; set; }

        private MachineElementViewModel _selected;
        public MachineElementViewModel Selected
        {
            get { return _selected; }
            set
            {
                if (Set(ref _selected, value, nameof(Selected)))
                {
                    Messenger.Default.Send(new ToolHolderSelectionChanged()
                    {
                        ToolHolder = _selected as IToolHolderManagement
                    });
                }
            }
        }

        public MachineElementsViewModel()
        {
            Messenger.Default.Register<MachineElementsLoaded>(this, OnMachineElementsLoaded);
            Messenger.Default.Register<UnloadAllTools>(this, OnUnloadAllTools);
            Messenger.Default.Register<GetActiveTooling>(this, OnGetActiveTooling);
            Messenger.Default.Register<SetActiveTooling>(this, OnSetActiveTooling);
        }

        private void OnSetActiveTooling(SetActiveTooling msg)
        {
            if(Elements.Count > 0)
            {
                var toolingDictionary = msg.Tooling.Units.ToDictionary((t) => t.ToolHolderId);
                SetActiveTooling(Elements[0], toolingDictionary);
            }
        }

        private void SetActiveTooling(MachineElementViewModel vm, Dictionary<int, ToolingUnit> td)
        {
            if (vm is IToolHolderManagement thm)
            {
                if(vm is IToolingUnitProvider tup)
                {
                    if(td.TryGetValue(tup.GetToolHolderId(), out ToolingUnit tu))
                    {
                        Messenger.Default.Send(new TryToLoadTool() { ToolingHolder = thm, ToolName = tu.ToolName });
                    }
                }                
            }
            else if (vm.Children.Count() > 0)
            {
                foreach (var item in vm.Children)
                {
                    SetActiveTooling(item, td);
                }
            }
        }

        private void OnGetActiveTooling(GetActiveTooling msg)
        {
            var tooling = new Tooling() { Units = new List<ToolingUnit>() };

            if(Elements.Count() > 0)
            {
                GetActiveTooling(Elements[0], tooling);
            }

            msg.Set(tooling);
        }

        private void GetActiveTooling(MachineElementViewModel vm, Tooling tooling)
        {
            if (vm is IToolHolderManagement thm)
            {
                if(thm.IsToolPresent)
                {
                    if(thm is IToolingUnitProvider tup)
                    {
                        tooling.Units.Add(tup.GetToolingUnit());
                    }
                }
            }
            else if (vm.Children.Count() > 0)
            {
                foreach (var item in vm.Children)
                {
                    GetActiveTooling(item, tooling);
                }
            }
        }

        private void OnUnloadAllTools(UnloadAllTools msg)
        {
            if(Elements.Count > 0) UnloadTools(Elements[0]);
        }

        private void UnloadTools(MachineElementViewModel vm)
        {
            if(vm is IToolHolderManagement thm)
            {
                thm.UnloadTool();
            }
            else if(vm.Children.Count() > 0)
            {
                foreach (var item in vm.Children)
                {
                    UnloadTools(item);
                }
            }
        }

        private void OnMachineElementsLoaded(MachineElementsLoaded msg)
        {
            Selected = null;
            Elements = new List<MachineElementViewModel>() { ToViewModel(msg.Elements) };
            RaisePropertyChanged(nameof(Elements));
        }

        private MachineElementViewModel ToViewModel(MachineElement me)
        {
            MachineElementViewModel vm = null;

            if(me != null)
            {
                if(me.ToolHolderType != MachineModels.Enums.ToolHolderType.None)
                {
                    vm = new ToolHolderViewModel() { ToolHolderId = me.ToolHolderData.Id, ToolHolderType = me.ToolHolderType };
                }
                else
                {
                    vm = new MachineElementViewModel();
                }

                vm.Name = me.Name;

                if (me.Children.Count > 0)
                {
                    var children = new List<MachineElementViewModel>();

                    foreach (var item in me.Children)
                    {
                        children.Add(ToViewModel(item));
                    }

                    vm.Children = children;
                }
            }

            return vm;
        }
    }
}
