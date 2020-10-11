using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineViewer.Plugins.Injectors.SimpleManipolator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineViewer.Plugins.Injectors.SimpleManipolator.ViewModels
{
    public class InjectorViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        private ICommand _inject;
        public ICommand Inject => _inject ?? (_inject = new RelayCommand(InjectImpl));

        public InjectorViewModel() : base()
        {

        }

        private void InjectImpl() => MessengerInstance.Send(new ExecuteInjectionMessage() { Id = Id });
    }
}
