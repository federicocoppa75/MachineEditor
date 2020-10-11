using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.Links;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Links
{
    public class RotaryPneumaticViewModel : MachineViewModels.ViewModels.Links.RotaryPneumaticViewModel, ILinkViewModelDescriptionProvider, IPneumaticPresserExtensionProvider, IUpdatableValueLink<bool>
    {
        private int _backNotifyId = 0;

        public string Description { get; set; }

        [Browsable(false)]
        public double CollisionOnPos { get; set; }

        [Browsable(false)]
        public bool HasCollision { get; set; }

        public double Pos
        {
            get => GetPosition();
            set => SetPosition(value);
        }

        [Browsable(false)]
        public Action<double> SetPosition { get; set; }

        [Browsable(false)]
        public Func<double> GetPosition { get; set; }

        [Browsable(false)]
        public Action<IPneumaticColliderExtensionProvider> EvaluateCollision { get; set; }

        [Browsable(false)]
        public Action<IPneumaticColliderExtensionProvider> OnMovementCompleted { get; set; }

        [Browsable(false)]
        public Action<IPneumaticColliderExtensionProvider> OnMovementStarting { get; set; }

        [Browsable(false)]
        public bool IsGradualTransactionEnabled { get; set; }

        public RotaryPneumaticViewModel() : base()
        {
            MessengerInstance.Register<UpdateTwoPositionLinkStateMessage>(this, OnUpdateStateMessage);
            MessengerInstance.Register<EnablePneumaticTransactionMessage>(this, OnEnablePneumaticTransactionMessage);
            MessengerInstance.Register<ReadTwoPositionLinkStateMessage>(this, OnReadStateMessage);
            MessengerInstance.Register<UpdateLinearLinkStateMessage>(this, OnUpdateLinearLinkStateMessage);
            MessengerInstance.Register<ReadTwoPositionLinkDurationMessage>(this, OnReadTwoPositionLinkDurationMessage);
            MessengerInstance.Register<UpdateLinearLinkStateToTargetMessage>(this, OnUpdateLinearLinkStateToTargetMessage);
        }

        private void OnReadStateMessage(ReadTwoPositionLinkStateMessage msg)
        {
            if (msg.LinkId == Id) msg.Read(this);
        }

        private void OnEnablePneumaticTransactionMessage(EnablePneumaticTransactionMessage msg) => IsGradualTransactionEnabled = msg.Value;

        private void OnUpdateStateMessage(UpdateTwoPositionLinkStateMessage msg)
        {
            if (msg.LinkId == Id) msg.Update(this);

            if (IsGradualTransactionEnabled)
            {
                _backNotifyId = msg.BackNotifyId;
            }
            else if (msg.BackNotifyId > 0)
            {
                MessengerInstance.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
        }

        private void OnUpdateLinearLinkStateMessage(UpdateLinearLinkStateMessage msg)
        {
            if (Id == msg.LinkId) SetPosition(msg.Value);
        }

        private void OnReadTwoPositionLinkDurationMessage(ReadTwoPositionLinkDurationMessage msg)
        {
            if (Id == msg.LinkId) msg.SetDuration?.Invoke(msg.RequestedState ? TOn : TOff);
        }

        private void OnUpdateLinearLinkStateToTargetMessage(UpdateLinearLinkStateToTargetMessage msg)
        {
            if (Id == msg.LinkId)
            {
                SetPosition(msg.Value);
                if (msg.IsCompleted && (_backNotifyId > 0))
                {
                    MessengerInstance.Send(new BackNotificationMessage() { DestinationId = _backNotifyId });
                    _backNotifyId = 0;
                }
            }
        }
    }
}
