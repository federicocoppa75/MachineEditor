using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineViewer.Extensions;
using MachineViewer.Messages.Inserter;
using MachineViewer.Messages.Panel;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Injectors.SimpleManipolator.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Inserter
{
    public class InjectorViewModel : InserterBaseViewModel
    {
        [Browsable(false)]
        public bool IsGradualTransactionEnabled { get; set; }

        public InjectorViewModel() : base()
        {
            Messenger.Default.Register<GetAvailablaInjectorsMessage>(this, OnGetAvailablaInjectorsMessage);
            Messenger.Default.Register<ExecuteInjectionMessage>(this, OnExecuteInjectionMessage);
            Messenger.Default.Register<MachineViewer.Plugins.Common.Messages.Inserter.InjectMessage>(this, OnInjectMessage);
            Messenger.Default.Register<EnableGradualTransitionMessage>(this, OnEnableGradualTransitionMessage);
        }

        private void OnGetAvailablaInjectorsMessage(GetAvailablaInjectorsMessage msg) => msg?.SetInjectorData(InserterId);

        private void OnExecuteInjectionMessage(ExecuteInjectionMessage msg)
        {
            if(msg.Id == InserterId)
            {
                ExecuteInjection();
            }
        }

        private void OnInjectMessage(MachineViewer.Plugins.Common.Messages.Inserter.InjectMessage msg)
        {
            if (msg.InjectorId == InserterId)
            {
                ExecuteInjection();

                if (msg.BackNotifyId > 0)
                {
                    if (IsGradualTransactionEnabled)
                    {
                        Task.Delay(TimeSpan.FromSeconds(msg.Duration))
                            .ContinueWith((t) =>
                            {
                                Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
                            });
                    }
                    else
                    {
                        Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
                    }
                }                
            }
        }

        private void OnEnableGradualTransitionMessage(EnableGradualTransitionMessage msg) => IsGradualTransactionEnabled = msg.Value;

        private void ExecuteInjection()
        {
            if (_chainTransform == null) _chainTransform = this.GetChainTansform();

            var t = _chainTransform.Value;
            var p = t.Transform(Position);
            var d = t.Transform(Direction);

            Messenger.Default.Send(new GetPanelTransformMessage()
            {
                SetData = (b, m) =>
                {
                    if (b)
                    {
                        var invT = m.Inverse();

                        Messenger.Default.Send(new InjectMessage()
                        {
                            Position = invT.Transform(p),
                            Direction = invT.Transform(d),
                            Color = Color
                        });
                    }
                }
            });
        }
    }
}
