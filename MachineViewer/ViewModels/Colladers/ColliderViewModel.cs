using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MachineModels.Enums;
using MachineViewer.Extensions;
using MachineViewer.Messages.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Colladers
{
    public abstract class ColliderViewModel : MachineElementViewModel
    {
        private Transform3D _chainTransform;

        public ColliderType Type { get; set; }

        public Transform3D TotalTransformation => _chainTransform ?? (_chainTransform = this.GetChainTansform());

        public bool Collided { get; set; }

        public abstract Tuple<bool, double> CheckPanelIntersection(bool targetLinkState, Vector3D linkDirection);

        public abstract Task<Tuple<bool, double>> CheckPanelIntersectionAsync(bool targetLinkState, Vector3D linkDirection);
        
        public abstract void EvaluateOnState();

        public abstract void EvaluateOffState();

        public ColliderViewModel() : base()
        {
            Messenger.Default.Register<CollidersVisibilityChangedMessage>(this, OnColliderVisibilityChanged);

            // normalmente non è necessaria la visualizzazione
            Visible = false;
        }

        private void OnColliderVisibilityChanged(CollidersVisibilityChangedMessage msg) => Visible = msg.Value;
    }
}
