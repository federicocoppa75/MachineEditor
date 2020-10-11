using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineViewer.Messages.Inserter;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.PanelHolder;
using MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels;

namespace MachineViewer.ViewModels
{
    public class WorkablePanelViewModel : PanelHolderViewModel
    {
        public PanelViewModel PanelViewModel { get; set; }

        public WorkablePanelViewModel() : base()
        {
            Messenger.Default.Register<InjectMessage>(this, OnInjectMessage);
            Messenger.Default.Register<InsertMessage>(this, OnInsertMessage);
        }

        protected override void ResetPanel()
        {
            base.ResetPanel();
            PanelViewModel = null;
        }

        protected override void OnLoadPanel(LoadPanelMessage msg)
        {
            if (msg.PanelHolderId == PanelHolderId)
            {
                var center = GetPanelCenter(msg.Length, msg.Width, msg.Height);

                PanelViewModel = new PanelViewModel()
                {
                    SizeX = msg.Length,
                    SizeY = msg.Width,
                    SizeZ = msg.Height,
                    CenterX = Position.X + center.X,
                    CenterY = Position.Y + center.Y,
                    CenterZ = Position.Z + center.Z
                };

                PanelViewModel.Initialize();

                _panel = new ModelVisual3D() { Content = PanelViewModel.PanelModel };
                _getBounds = () => PanelViewModel.PanelModel.Bounds;

                Children.Add(_panel);
                msg?.NotifyExecution?.Invoke(true);
                if (msg.BackNotifyId > 0) Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
        }

        private void OnInjectMessage(InjectMessage msg)
        {
            if (PanelViewModel != null)
            {
                var builder = new MeshBuilder();

                builder.AddCone(msg.Position + msg.Direction * 20.0, msg.Position, 4.0, true, 20);
                PanelViewModel.AppendMeshGeometry3D(builder.ToMesh(), msg.Color);
            }
        }


        private void OnInsertMessage(InsertMessage msg)
        {
            if(PanelViewModel != null)
            {
                var builder = new MeshBuilder();

                builder.AddCylinder(msg.Position, msg.Position + msg.Direction * msg.Length, msg.Diameter / 2.0);
                PanelViewModel.AppendMeshGeometry3D(builder.ToMesh(), msg.Color);
            }
        }
    }
}
