using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using HelixToolkit.Wpf;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TestMaterialRemoval.Messages;

namespace TestMaterialRemoval.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Vector3D _toolDirection = new Vector3D(0.0, 0.0, -1.0);

        private readonly IHelixViewport3D _viewport;

        private double _offsetX;
        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                if (Set(ref _offsetX, value, nameof(OffsetX)))
                {
                    _traslateTrasform.OffsetX = _offsetX;
                    ProcessChangedBounds();
                    MoveRoutTool();
                }
            }
        }

        private double _offsetZ;
        public double OffsetZ
        {
            get { return _offsetZ; }
            set
            {
                if (Set(ref _offsetZ, value, nameof(OffsetZ)))
                {
                    _traslateTrasform.OffsetZ = _offsetZ;
                    ProcessChangedBounds();
                    MoveRoutTool();
                }
            }
        }

        private double _offsetY;
        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (Set(ref _offsetY, value, nameof(OffsetY)))
                {
                    _traslateTrasform.OffsetY = _offsetY;
                    ProcessChangedBounds();
                    MoveRoutTool();
                }
            }
        }

        private double _panelOffsetX;
        public double PanelOffsetX
        {
            get => _panelOffsetX; 
            set
            {
                if(Set(ref _panelOffsetX, value, nameof(PanelOffsetX)))
                {
                    _panelTranslateTrasform.OffsetX = _panelOffsetX;
                    MoveTool();
                }
            }
        }

        private double _panelOffsetY;
        public double PanelOffsetY
        {
            get => _panelOffsetY;
            set
            {
                if (Set(ref _panelOffsetY, value, nameof(PanelOffsetY)))
                {
                    _panelTranslateTrasform.OffsetY = _panelOffsetY;
                    MoveTool();
                }
            }
        }

        private double _panelOffsetZ;
        public double PanelOffsetZ
        {
            get => _panelOffsetZ;
            set
            {
                if (Set(ref _panelOffsetZ, value, nameof(PanelOffsetZ)))
                {
                    _panelTranslateTrasform.OffsetZ = _panelOffsetZ;
                    MoveTool();
                }
            }
        }

        private TranslateTransform3D _panelTranslateTrasform;

        private TranslateTransform3D _traslateTrasform;

        public TranslateTransform3D TraslateTrasform
        {
            get => _traslateTrasform;
            set => Set(ref _traslateTrasform, value, nameof(TraslateTrasform));
        }

        public GeneralTransform3D InversTraslateTrasform { get; set; }

        private Point3D _center;

        public Point3D Center
        {
            get { return _center; }
            set { Set(ref _center, value, nameof(Center)); }
        }

        private MeshGeometry3D _panelGeometry;
        public MeshGeometry3D PanelGeometry
        {
            get => _panelGeometry;
            set => Set(ref _panelGeometry, value, nameof(PanelGeometry));
        }

        public MeshGeometry3D _toolGeometry;
        public MeshGeometry3D ToolGeometry
        {
            get => _toolGeometry;
            set => Set(ref _toolGeometry, value, nameof(ToolGeometry));
        }

        private bool _boudsIntercept;
        public bool BoudsIntercept
        {
            get => _boudsIntercept;
            set => Set(ref _boudsIntercept, value, nameof(BoudsIntercept));
        }

        private bool _boudsIntercept2;
        public bool BoudsIntercept2
        {
            get => _boudsIntercept2;
            set => Set(ref _boudsIntercept2, value, nameof(BoudsIntercept2));
        }

        private Model3DGroup _panelModel;
        public Model3DGroup PanelModel
        {
            get => _panelModel;
            set => Set(ref _panelModel, value, nameof(PanelModel));
        }

        public PanelViewModel PanelViewModel { get; set; }

        public ToolViewModel ToolViewModel { get; set; } = new ToolViewModel();

        public PanelSizesViewModel PanelSizesViewModel { get; set; } = new PanelSizesViewModel() { SizeX = 800.0, SizeY = 600.0, SizeZ = 30.0 };

        RenderingEventListener _renderListener;

        public MainViewModel(IHelixViewport3D viewPort)
        {
            DispatcherHelper.Initialize();

            _viewport = viewPort;

            _traslateTrasform = new TranslateTransform3D();

            InitializePanel();
            InitializeTool();
            OffsetZ = 150.0;

            _renderListener = new RenderingEventListener(OnRendering);
            RenderingEventManager.AddListener(_renderListener);

            MessengerInstance.Register<ToolDataChangedMessage>(this, OnToolDataChangedMessage);
            MessengerInstance.Register<PanelSizeChangedMessage>(this, OnPanelSizeChangedMessage);
        }

        private void InitializeTool()
        {
            var length = ToolViewModel.Length;
            var radius = ToolViewModel.Radius;
            var repetition = ToolViewModel.Repetition;
            var repStepX = ToolViewModel.RepetitionStepX;
            var repStepY = ToolViewModel.RepetitionStepY;
            var builder = new MeshBuilder();

            ChangeToolDirection();

            for (int i = 0; i < repetition; i++)
            {
                var point = new Point3D(repStepX * i, repStepY * i, 0.0);
                builder.AddCylinder(point, point + _toolDirection * length, radius * 2.0, 20);
            }

            ToolGeometry = builder.ToMesh();
        }

        private void InitializePanel()
        {
            PanelViewModel = new PanelViewModel()
            {
                SizeX = PanelSizesViewModel.SizeX,
                SizeY = PanelSizesViewModel.SizeY,
                SizeZ = PanelSizesViewModel.SizeZ
            };

            PanelViewModel.Initialize();
            PanelModel = PanelViewModel.PanelModel;
            _panelTranslateTrasform = new TranslateTransform3D();
            PanelModel.Transform = _panelTranslateTrasform;
        }

        private void ProcessChangedBounds()
        {
            var toolBound = _traslateTrasform.TransformBounds(ToolGeometry.Bounds);
            BoudsIntercept = PanelModel.Bounds.IntersectsWith(toolBound);
        }

        private void OnRendering(object sender, RenderingEventArgs e)
        {
            MessengerInstance.Send(new ProcessPendingRemovalMessage());
        }

        private void MoveTool()
        {
            var length = ToolViewModel.Length;
            var radius = ToolViewModel.Radius;
            var repetition = ToolViewModel.Repetition;
            var repStepX = ToolViewModel.RepetitionStepX;
            var repStepY = ToolViewModel.RepetitionStepY;

            for (int i = 0; i < repetition; i++)
            {
                MessengerInstance.Send(new ToolMoveMessage()
                {
                    Position = new Point3D(OffsetX + repStepX * i, OffsetY + repStepY * i, OffsetZ),
                    Direction = _toolDirection,
                    Length = length,
                    Radius = radius
                });
            }
        }

        private void MoveRoutTool()
        {
            var length = ToolViewModel.Length;
            var radius = ToolViewModel.Radius;
            var repetition = ToolViewModel.Repetition;
            var repStepX = ToolViewModel.RepetitionStepX;
            var repStepY = ToolViewModel.RepetitionStepY;

            for (int i = 0; i < repetition; i++)
            {
                MessengerInstance.Send(new RoutToolMoveMessage()
                {
                    Position = new Point3D(OffsetX + repStepX * i, OffsetY + repStepY * i, OffsetZ),
                    Direction = _toolDirection,
                    Length = length,
                    Radius = radius,
                    ToolId = 1000 + i
                });
            }
        }

        private void ChangeToolDirection()
        {
            var toolDir = ToolViewModel.ToolDirection;

            switch (toolDir)
            {
                case ToolDirection.ZNeg:
                    _toolDirection = new Vector3D(0.0, 0.0, -1.0);
                    break;
                case ToolDirection.XPos:
                    _toolDirection = new Vector3D(1.0, 0.0, 0.0);
                    break;
                case ToolDirection.XNeg:
                    _toolDirection = new Vector3D(-1.0, 0.0, 0.0);
                    break;
                case ToolDirection.YPos:
                    _toolDirection = new Vector3D(0.0, 1.0, 0.0);
                    break;
                case ToolDirection.YNeg:
                    _toolDirection = new Vector3D(0.0, -1.0, 0.0);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void OnToolDataChangedMessage(ToolDataChangedMessage msg) => InitializeTool();
        
        private void OnPanelSizeChangedMessage(PanelSizeChangedMessage obj) => InitializePanel();
    }
}
