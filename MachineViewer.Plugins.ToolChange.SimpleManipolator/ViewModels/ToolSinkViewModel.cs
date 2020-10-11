using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineViewer.Plugins.Common.Messages.ToolChange;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineViewer.Plugins.ToolChange.SimpleManipolator.ViewModels
{
    public class ToolSinkViewModel : ViewModelBase
    {
        private Tuple<int, string> _lastSelected;

        public int Id { get; set; }

        public string Name { get; set; }

        private Tuple<int, string> _selectedTool;
        public Tuple<int, string> SelectedTool
        {
            get { return _selectedTool; }
            set
            {
                _lastSelected = _selectedTool;

                if(Set(ref _selectedTool, value, nameof(SelectedTool)))
                {
                    ApplyTooling();
                }
            }
        }

        public ObservableCollection<Tuple<int, string>> Tools { get; set; } = new ObservableCollection<Tuple<int, string>>();

        private ICommand _unloadCommand;
        public ICommand UnloadCommand => _unloadCommand ?? (_unloadCommand = new RelayCommand(() => SelectedTool = null, () => SelectedTool != null));

        public ToolSinkViewModel()
        {
            ResetToolList();
            MessengerInstance.Register<UpdateAvailableToolsListMessage>(this, OnUpdateAvailableToolsListMessage);
        }

        private void ResetToolList()
        {
            Tools.Clear();
            SelectedTool = null;
        }

        private void OnUpdateAvailableToolsListMessage(UpdateAvailableToolsListMessage msg)
        {
            ResetToolList();
            MessengerInstance.Send(new GetAvailableToolMessage() { SetAvailableTool = AddTool });
        }

        private void AddTool(int id, string name)
        {
            Tools.Add(new Tuple<int, string>(id, name));
        }

        private void ApplyTooling()
        {
            if(_lastSelected != null)
            {
                MessengerInstance.Send(new UnloadToolMessage() { ToolSource = _lastSelected.Item1, ToolSink = Id });
            }

            if(_selectedTool != null)
            {
                MessengerInstance.Send(new LoadToolMessage() { ToolSource = _selectedTool.Item1, ToolSink = Id });
            }
        }
    }
}
