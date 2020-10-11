using GalaSoft.MvvmLight;
using MachineViewer.Plugins.Common.Messages.Inserter;
using MachineViewer.Plugins.Injectors.SimpleManipolator.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Injectors.SimpleManipolator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<InjectorViewModel> Injectors { get; set; } = new ObservableCollection<InjectorViewModel>();

        public MainViewModel() : base()
        {
            MessengerInstance.Register<UpdateAvailableInjectorsMessage>(this, OnUpdateAvailableInjectorsMessage);
        }

        private void OnUpdateAvailableInjectorsMessage(UpdateAvailableInjectorsMessage msg)
        {
            Injectors.Clear();

            MessengerInstance.Send(new GetAvailablaInjectorsMessage()
            {
                SetInjectorData = (id) =>
                {
                    Injectors.Add(new InjectorViewModel()
                    {
                        Id = id
                    });
                }
            });
        }
    }
}
