using GalaSoft.MvvmLight;
using MachineSteps.Editor.Messages;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Editor.ViewModels
{
    class ActionViewModel : ViewModelBase
    {
        private BaseAction _action;

        public BaseAction Action
        {
            get { return _action; }
            set { Set(ref _action, value, nameof(Action)); }
        }

        public ActionViewModel()
        {
            MessengerInstance.Register<ActionSelectionChangedMessage>(this, OnActionSelectionChangedMessage);
        }

        private void OnActionSelectionChangedMessage(ActionSelectionChangedMessage msg)
        {
            Action = msg.Action;
        }
    }
}
