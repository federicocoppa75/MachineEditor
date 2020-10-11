using GalaSoft.MvvmLight.Threading;
using MachineViewer.Messages.Inserter;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.Links;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MachineViewer.ViewModels.Links
{
    public class LinearPneumaticViewModel : MachineViewModels.ViewModels.Links.LinearPneumaticViewModel, ILinkViewModelDescriptionProvider, IPneumaticPresserExtensionProvider, IUpdatableValueLink<bool>, IToolActivatorLinkExtension
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

        public LinearPneumaticViewModel() : base()
        {
            MessengerInstance.Register<UpdateTwoPositionLinkStateMessage>(this, OnUpdateStateMessage);
            MessengerInstance.Register<EnableGradualTransitionMessage>(this, OnEnableGradualTransitionMessage);
            MessengerInstance.Register<ReadTwoPositionLinkStateMessage>(this, OnReadStateMessage);
            MessengerInstance.Register<UpdateLinearLinkStateMessage>(this, OnUpdateLinearLinkStateMessage);
            MessengerInstance.Register<ReadTwoPositionLinkDurationMessage>(this, OnReadTwoPositionLinkDurationMessage);
            MessengerInstance.Register<UpdateLinearLinkStateToTargetMessage>(this, OnUpdateLinearLinkStateToTargetMessage);
            MessengerInstance.Register<HookLinkByInserterMessage>(this, OnHookLinkByInserterMessage);
        }

        private void OnReadStateMessage(ReadTwoPositionLinkStateMessage msg)
        {
            if(msg.LinkId == Id) msg.Read(this);
        }

        private void OnEnableGradualTransitionMessage(EnableGradualTransitionMessage msg) => IsGradualTransactionEnabled = msg.Value;

        private void OnUpdateStateMessage(UpdateTwoPositionLinkStateMessage msg)
        {
            if (msg.LinkId == Id)
            {
                if (msg.Value != Value)
                {
                    msg.Update(this);

                    if (IsGradualTransactionEnabled)
                    {
                        _backNotifyId = msg.BackNotifyId;
                    }
                    else if (msg.BackNotifyId > 0)
                    {
                        MessengerInstance.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
                    }
                }
                else
                {
                    MessengerInstance.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
                }
            }
        }

        private void OnUpdateLinearLinkStateMessage(UpdateLinearLinkStateMessage msg)
        {
            if(Id == msg.LinkId) SetPosition(msg.Value);
        }

        private void OnReadTwoPositionLinkDurationMessage(ReadTwoPositionLinkDurationMessage msg)
        {
            if(Id == msg.LinkId) msg.SetDuration?.Invoke(msg.RequestedState ? TOn : TOff);
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
        
        private void OnHookLinkByInserterMessage(HookLinkByInserterMessage msg)
        {
            if(Id == msg.LinkId)
            {
                InserterId = msg.InserterId;
            }
        }
        
        public void RegisterToolActivation(Action<bool> action) => ValueChanged += (s, e) => action(e);

        public void ManageInserter(bool value) => MessengerInstance.Send(new DriveInserterByLinkMessage() { InserterId = InserterId, LinkId = Id, Value = value });
    }
}
