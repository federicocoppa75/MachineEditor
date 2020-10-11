using GalaSoft.MvvmLight;
using MachineModels.Enums;
using MachineViewer.Plugins.Common.Messages.PanelHolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineViewer.Plugins.Panel.SimpleManipolator.Models;
using MachineViewer.Plugins.Panel.SimpleManipolator.Messages;

namespace MachineViewer.Plugins.Panel.SimpleManipolator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public ObservableCollection<PanelHolderViewModel> PanelHolders { get; set; } = new ObservableCollection<PanelHolderViewModel>();

        public PanelData PanelData { get; set; } = new PanelData { Length = 800.0, Width = 600.0, Height = 18.0 };

        public MainViewModel()
        {
            MessengerInstance.Register<UpdateAvailablePanelHolderMessage>(this, OnUpdateAvailablePanelHolderMessage);
            MessengerInstance.Register<GetPanelDataMessage>(this, OnGetPanelDataMessage);
        }

        private void OnGetPanelDataMessage(GetPanelDataMessage msg) => msg.SetPanelData(PanelData);

        private void OnUpdateAvailablePanelHolderMessage(UpdateAvailablePanelHolderMessage obj)
        {
            PanelHolders.Clear();

            MessengerInstance.Send(new GetAvailablePanelHoldersMessage() { AvailableToolHolder = AddPanelHolder });
        }

        private void AddPanelHolder(int id, string name, PanelLoadType corner)
        {
            PanelHolders.Add(new PanelHolderViewModel() { Id = id, Name = name, Corner = corner });
        }
    }
}
