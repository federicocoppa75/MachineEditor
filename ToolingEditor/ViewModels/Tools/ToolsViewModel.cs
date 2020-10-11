using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolingEditor.Messages;

namespace ToolingEditor.ViewModels.Tools
{
    public class ToolsViewModel : ViewModelBase
    {
        public List<ToolViewModel> Tools { get; set; }

        private ToolViewModel _selected;

        public ToolViewModel Selected
        {
            get { return _selected; }
            set
            {
                if (Set(ref _selected, value, nameof(Selected)))
                {
                    Messenger.Default.Send(new ToolSelectionChanged() { Tool = (_selected != null) ? _selected.GetModel() : null });
                }
            }
        }


        public ToolsViewModel()
        {
            Messenger.Default.Register<ToolSetLoaded>(this, OnToolSetLoaded);
            Messenger.Default.Register<TryToLoadTool>(this, OnTryToLoadTool);
        }

        private void OnTryToLoadTool(TryToLoadTool msg)
        {
            if(Tools.Count > 0)
            {
                var tool = Tools.FirstOrDefault((t) => string.Compare(t.Name, msg.ToolName) == 0);

                if (tool != null) msg.ToolingHolder.LoadTool(tool.GetModel());
            }
            else
            {
                throw new InvalidOperationException("Tools were not loaded!");
            }
        }

        private void OnToolSetLoaded(ToolSetLoaded msg)
        {
            Selected = null;
            Tools = msg.ToolSet.Tools.Select((t) => new ToolViewModel(t)).ToList();
            RaisePropertyChanged(nameof(Tools));
        }
    }
}
