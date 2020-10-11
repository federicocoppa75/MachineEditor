using GalaSoft.MvvmLight;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.ViewModels.Links
{
    public class GantryLinearPositionViewModel : ViewModelBase
    {
        public int Master { get; set; }

        private int _slave;
        public int Slave
        {
            get { return _slave; }
            set
            {
                var last = _slave;

                if(Set(ref _slave, value, nameof(Slave)))
                {
                    EvaluateGantry(_slave, last);
                }
            }
        }

        public List<int> CompatibleLinks { get; set; }

        private bool _isEnable = true;
        public bool IsEnable
        {
            get { return _isEnable; }
            set { Set(ref _isEnable, value, nameof(IsEnable)); }
        }

        public GantryLinearPositionViewModel()
        {
            MessengerInstance.Register<LinearPositionLinkGantryStateChangedMessage>(this, OnLinearPositionLinkGantryStateChangedMessage);
        }

        private void EvaluateGantry(int slave, int last)
        {
            if(last == -1)
            {
                MessengerInstance.Send(new LinearPositionGantryOnMessage() { MasterId = Master, SlaveId = slave });
                MessengerInstance.Send(new LinearPositionLinkGantryStateChangedMessage() { Id = slave, IsSlave = true });
            }
            else if(slave == -1)
            {
                MessengerInstance.Send(new LinearPositionGantryOffMessage() { MasterId = Master, SlaveId = last });
                MessengerInstance.Send(new LinearPositionLinkGantryStateChangedMessage() { Id = last, IsSlave = false });
            }
            else
            {
                MessengerInstance.Send(new LinearPositionGantryOffMessage() { MasterId = Master, SlaveId = last });
                MessengerInstance.Send(new LinearPositionLinkGantryStateChangedMessage() { Id = last, IsSlave = false });
                MessengerInstance.Send(new LinearPositionGantryOnMessage() { MasterId = Master, SlaveId = slave });
                MessengerInstance.Send(new LinearPositionLinkGantryStateChangedMessage() { Id = slave, IsSlave = true });
            }
        }

        private void OnLinearPositionLinkGantryStateChangedMessage(LinearPositionLinkGantryStateChangedMessage msg)
        {
            if(msg.Id == Master)
            {
                IsEnable = !msg.IsSlave;
            }
        }
    }
}
