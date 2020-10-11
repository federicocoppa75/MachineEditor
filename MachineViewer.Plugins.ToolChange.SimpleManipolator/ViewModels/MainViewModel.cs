using GalaSoft.MvvmLight;
using MachineViewer.Plugins.Common.Messages.ToolChange;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.ToolChange.SimpleManipolator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ToolSinkViewModel> ToolSinks { get; set; } = new ObservableCollection<ToolSinkViewModel>();

        public MainViewModel()
        {
            MessengerInstance.Register<UpdateAvailableToolSinkListMessage>(this, OnUpdateAvailableToolSinkListMessage);
        }

        private void OnUpdateAvailableToolSinkListMessage(UpdateAvailableToolSinkListMessage obj)
        {
            ToolSinks.Clear();
            MessengerInstance.Send(new GetAvailableToolSinkMessage() { SetAvailableToolSink = AddToolSink });
        }

        private void AddToolSink(int id, string name)
        {
            ToolSinks.Add(new ToolSinkViewModel() { Id = id, Name = name });
        }
    }
}
