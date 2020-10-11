using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HelixToolkit.Wpf;
using MachineModels.Enums;
using MachineViewer.Extensions;
using MachineViewer.Messages;
using MachineViewer.Messages.Panel;
using MachineViewer.Messages.Visibility;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.PanelHolder;
using MachineViewer.ViewModels.Colladers;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace MachineViewer.ViewModels
{
    public class PanelHolderViewModel : MachineElementViewModel
    {
        protected ModelVisual3D _panel;

        protected Func<Rect3D> _getBounds;

        private List<IPanelHooker> _hookers = new List<IPanelHooker>();

        private Transform3D _sourceTransformation;

        private IPanelHooker _activeHooker;

        private Matrix3D _residualTransform;

        private bool _residualTransformInitialized;

        public Point3D Position { get; set; }

        public PanelLoadType Corner { get; set; }

        public int PanelHolderId { get; set; }

        public PanelHolderViewModel() : base()
        {
            Messenger.Default.Register<LoadPanelMessage>(this, OnLoadPanel);
            Messenger.Default.Register<UnloadPanelMessage>(this, OnUnloadPanel);
            Messenger.Default.Register<GetPanelPositionMessage>(this, OnGetPanelPosition);
            Messenger.Default.Register<HookPanelMessage>(this, OnHookPanel);
            Messenger.Default.Register<UnhookPanelMessage>(this, OnUnhookPanel);
            Messenger.Default.Register<PanelHolderVisibilityChangedMessage>(this, OnPanelHolderVisibilityChanged);
            Messenger.Default.Register<GetAvailablePanelHoldersMessage>(this, OnGetAvailablePanelHoldersMessage);
            Messenger.Default.Register<GetPanelTransformMessage>(this, OnGetPanelTransformMessage);

            // normalmente non è necessaria la visualizzazione
            Visible = false;
        }

        private void OnGetAvailablePanelHoldersMessage(GetAvailablePanelHoldersMessage msg) => msg?.AvailableToolHolder(PanelHolderId, Name, Corner);

        private void OnPanelHolderVisibilityChanged(PanelHolderVisibilityChangedMessage msg) => Visible = msg.Value;

        private void OnGetPanelPosition(GetPanelPositionMessage msg)
        {
           if(_panel != null)
            {
                //var box = _panel.MeshGeometry.Bounds;
                var box = _getBounds();
                var p = _panel.Transform.Transform(box.Location);

                if (_activeHooker != null)
                {
                    msg.SetData(true, _activeHooker.TotalTransformation, new Rect3D(p, box.Size));
                }
                else
                {
                    msg.SetData(true, this.GetChainTansform(), new Rect3D(p, box.Size));
                }

            }
            else
            {
                msg.SetData(false, null, null);
            }
        }

        private void OnGetPanelTransformMessage(GetPanelTransformMessage msg)
        {
            if(_panel != null)
            {
                if(_residualTransformInitialized)
                {
                    if(_activeHooker != null)
                    {
                        var ht = _activeHooker.TotalTransformation.Value;
                        var ir = _residualTransform.Inverse();
                        var tg = ht * ir;

                        msg.SetData(true, tg);
                    }
                    else
                    {
                        msg.SetData(true, _residualTransform);
                    }
                }
                else
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => msg.SetData(true, this.GetChainTansform().Value));
                }
            }
            else
            {
                msg.SetData(false, Matrix3D.Identity);
            }
        }

        protected virtual void ResetPanel()
        {
            _panel = null;
            _getBounds = null;
        }

        private void OnUnloadPanel(UnloadPanelMessage msg)
        {
            if (msg.PanelHolderId == PanelHolderId)
            {
                if (Children.Count > 0) Children.Clear();
                ResetPanel();
                msg?.NotifyExecution?.Invoke(true);
                if (msg.BackNotifyId > 0) Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
        }

        protected virtual void OnLoadPanel(LoadPanelMessage msg)
        {
            if (msg.PanelHolderId == PanelHolderId)
            {
                var builder = new MeshBuilder();
                var center = GetPanelCenter(msg.Length, msg.Width, msg.Height);

                builder.AddBox(Position + center, msg.Length, msg.Width, msg.Height);

                _panel = new MeshGeometryVisual3D()
                {
                    MeshGeometry = builder.ToMesh(),
                    Material = Materials.Orange,
                    Fill = Brushes.Orange
                };

                _getBounds = () => (_panel as MeshGeometryVisual3D).MeshGeometry.Bounds;

                Children.Add(_panel);
                msg?.NotifyExecution?.Invoke(true);
                if (msg.BackNotifyId > 0) Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
        }

        protected Vector3D GetPanelCenter(double length, double width, double height)
        {
            Vector3D center;

            switch (Corner)
            {
                case PanelLoadType.Corner1:
                    center = new Vector3D(length / 2.0, width / 2.0, height / 2.0);
                    break;
                case PanelLoadType.Corner2:
                    center = new Vector3D(-length / 2.0, width / 2.0, height / 2.0);
                    break;
                case PanelLoadType.Corner3:
                    center = new Vector3D(-length / 2.0, -width / 2.0, height / 2.0);
                    break;
                case PanelLoadType.Corner4:
                    center = new Vector3D(length / 2.0, -width / 2.0, height / 2.0);
                    break;
                default:
                    center = new Vector3D();
                    break;
            }

            return center;
        }

        private void OnHookPanel(HookPanelMessage msg)
        {
            if (_panel != null)
            {
                if (_sourceTransformation == null) _sourceTransformation = this.GetChainTansform().StaticClone();


                if (_hookers.Count == 0)
                {
                    Children.Remove(_panel);
                    ChangeReference(_panel, _sourceTransformation, msg.Hooker.TotalTransformation);
                    msg.Hooker.HookPanel(_panel);
                    _activeHooker = msg.Hooker;
                    _residualTransform = _panel.Transform.Value.Inverse();
                    _residualTransformInitialized = true;
                }

                _hookers.Add(msg.Hooker);
            }
        }

        private void OnUnhookPanel(UnhookPanelMessage msg)
        {
            if (_panel != null)
            {
                _hookers.Remove(msg.Hooker);

                if (object.ReferenceEquals(_activeHooker, msg.Hooker))
                {
                    msg.Hooker.UnhookPanel();

                    if (_hookers.Count > 0)
                    {
                        var h = _hookers.FirstOrDefault();

                        ChangeReference(_panel, msg.Hooker.TotalTransformation, h.TotalTransformation);
                        h.HookPanel(_panel);
                        _activeHooker = h;
                        _residualTransform = _panel.Transform.Value.Inverse();
                        _residualTransformInitialized = true;
                    }
                    else
                    {
                        ChangeReference(_panel, msg.Hooker.TotalTransformation, _sourceTransformation);
                        Children.Add(_panel);
                        _sourceTransformation = null;
                        _activeHooker = null;
                    }
                }
            }
        }

        private void ChangeReference(ModelVisual3D m, Transform3D source, Transform3D destination)
        {
            var tg = new Transform3DGroup();

            tg.Children.Add(m.Transform);
            tg.Children.Add(source);
            tg.Children.Add(destination.Invert());

            m.Transform = new MatrixTransform3D(tg.Value);
        }
    }
}
