using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineModels.Enums;
using MachineViewer.Plugins.Common.Messages.PanelHolder;
using MachineViewer.Plugins.Panel.SimpleManipolator.Messages;
using MachineViewer.Plugins.Panel.SimpleManipolator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineViewer.Plugins.Panel.SimpleManipolator.ViewModels
{
    public class PanelHolderViewModel : ViewModelBase
    {
        private bool _panelHold = false;

        public int Id { get; set; }
        public string Name { get; set; }
        public PanelLoadType Corner { get; set; }

        private ICommand _loadPanel;
        public ICommand LoadPanel => _loadPanel ?? (_loadPanel = new RelayCommand(LoadPanelImpl, CanExecuteLoadPanel));

        private ICommand _unloadPanel;
        public ICommand UnloadPanel => _unloadPanel ?? (_unloadPanel = new RelayCommand(UnloadPanelImpl, CanExecuteUnloadPanel));

        private void LoadPanelImpl()
        {
            PanelData panel = null;

            MessengerInstance.Send(new GetPanelDataMessage() { SetPanelData = (d) => panel = d });

            if(panel != null)
            {
                MessengerInstance.Send(new LoadPanelMessage()
                {
                    PanelHolderId = Id,
                    Length = panel.Length,
                    Width = panel.Width,
                    Height = panel.Height,
                    NotifyExecution = (b) => _panelHold = b
                });
            }
            else
            {
                throw new InvalidOperationException("Panel data must not be null!");
            }
        }

        private bool CanExecuteLoadPanel() => !_panelHold;

        private void UnloadPanelImpl()
        {
            MessengerInstance.Send(new UnloadPanelMessage()
            {
                PanelHolderId = Id,
                NotifyExecution = (b) => _panelHold = !b
            });
        }

        private bool CanExecuteUnloadPanel() => _panelHold;
    }
}
