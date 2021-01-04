using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using g3;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Concurrent;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels
{
    public class PanelViewModel : ViewModelBase
    {
        const int _numCells = 16;

        int _nxSection;
        int _nySection;

        private double _cornerX;
        private double _cornerY;
        private double _cornerZ;

        private ConcurrentDictionary<int, ImplicitRouting> _lastRoutings = new ConcurrentDictionary<int, ImplicitRouting>();

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }


        private Model3DGroup _panelModel;
        public Model3DGroup PanelModel
        {
            get => _panelModel;
            set => Set(ref _panelModel, value, nameof(PanelModel));
        }

        public ObservableCollection<PanelSectionViewModel> Sections { get; set; } = new ObservableCollection<PanelSectionViewModel>();

        public PanelViewModel()
        {
            MessengerInstance.Register<ToolMoveMessage>(this, OnToolMoveMessage);
            MessengerInstance.Register<RoutToolMoveMessage>(this, OnRoutToolMoveMessage);
        }

        public void Initialize()
        {
            InitializeSectionsNumber();

            double xSectionSize = SizeX / _nxSection;
            double ySectionSize = SizeY / _nySection;
            double xStartOffset = - SizeX / 2.0;
            double yStartOffset = - SizeY / 2.0;
            var panelModel = new Model3DGroup();

            _cornerX = CenterX + xStartOffset;
            _cornerY = CenterY + yStartOffset;
            _cornerZ = CenterZ - SizeZ / 2.0;

            for (int i = 0; i < _nxSection; i++)
            {
                var xCenter = _cornerX + xSectionSize / 2.0 + xSectionSize * i;

                for (int j = 0; j < _nySection; j++)
                {
                    var yCenter = _cornerY + ySectionSize / 2.0 + ySectionSize * j;
                    var center = new Point3D(xCenter, yCenter, CenterZ);
                    var section = CreatePanelSection(center, xSectionSize, ySectionSize, i, j);

                    Sections.Add(section);
                    panelModel.Children.Add(section.Model);

                    //panelModel.Children.Add(section.FilterBox);
                    //panelModel.Children.Add(section.BoundingBox);
                }
            }

            PanelModel = panelModel;
        }

        private void InitializeSectionsNumber()
        {
            const int sectionsX100mm = 3;
            _nxSection = (int)Math.Ceiling(SizeX / 100.0) * sectionsX100mm;
            _nySection = (int)Math.Ceiling(SizeY / 100.0) * sectionsX100mm;

            ImproveSectionsNumber();
        }

        private void ImproveSectionsNumber()
        {
            var xSize = SizeX / _nxSection;
            var ySize = SizeY / _nySection;

            if(Math.Abs(xSize - ySize) > 0.01)
            {
                if(xSize > ySize)
                {
                    ImproveSectionsNumber(ySize, SizeX, ref _nxSection);
                }
                else
                {
                    ImproveSectionsNumber(xSize, SizeY, ref _nySection);
                }
            }
        }

        private static void ImproveSectionsNumber(double refSecSize, double size, ref int n)
        {
            while ((size / n) >= refSecSize) n++;

            var v1 = Math.Abs((size / n) - refSecSize);
            var v2 = Math.Abs((size / (n - 1)) - refSecSize);

            if (v1 > v2) n--;
        }

        private PanelSectionViewModel CreatePanelSection(Point3D center, double xSectionSize, double ySectionSize, int i, int j)
        {
            var positon = GetSctionPosition(i, j);
            PanelSectionViewModel panelSection = null;

            switch (positon)
            {
                case PanelSectionPosition.Center:
                    panelSection = CreateCenterPanelSection(center, xSectionSize, ySectionSize, i, j);
                    break;
                case PanelSectionPosition.SideBottom:
                case PanelSectionPosition.SideTop:
                case PanelSectionPosition.SideRight:
                case PanelSectionPosition.SideLeft:
                    panelSection = CreateSidePanelSection(center, xSectionSize, ySectionSize, i, j, positon);
                    break;
                case PanelSectionPosition.CornerBottomLeft:
                case PanelSectionPosition.CornerBottomRight:
                case PanelSectionPosition.CornerTopLeft:
                case PanelSectionPosition.CornerTopRight:
                    panelSection = CreateCornerPanelSection(center, xSectionSize, ySectionSize, i, j, positon);
                    break;
                default:
                    break;
            }

            return panelSection;
        }

        private PanelSectionViewModel CreateSidePanelSection(Point3D center, double xSectionSize, double ySectionSize, int i, int j, PanelSectionPosition position)
        {
            var section = new SidePanelSectionViewModel()
            {
                XSectionIndex = i,
                YSectionIndex = j,
                Position = position,
                NumCells = _numCells,
                SizeX = xSectionSize,
                SizeY = ySectionSize,
                SizeZ = SizeZ,
                Center = center
            };

            section.Initialize();
            return section;
        }

        private PanelSectionViewModel CreateCornerPanelSection(Point3D center, double xSectionSize, double ySectionSize, int i, int j, PanelSectionPosition position)
        {
            var section = new CornerPanelSectionViewMolde()
            {
                XSectionIndex = i,
                YSectionIndex = j,
                Position = position,
                NumCells = _numCells,
                SizeX = xSectionSize,
                SizeY = ySectionSize,
                SizeZ = SizeZ,
                Center = center
            };

            section.Initialize();
            return section;
        }


        private PanelSectionViewModel CreateCenterPanelSection(Point3D center, double xSectionSize, double ySectionSize, int i, int j)
        {
            var section = new PanelSectionViewModel()
            {
                XSectionIndex = i,
                YSectionIndex = j,
                Position = PanelSectionPosition.Center,
                NumCells = _numCells,
                SizeX = xSectionSize,
                SizeY = ySectionSize,
                SizeZ = SizeZ,
                Center = center
            };

            section.Initialize();
            return section;
        }

        private PanelSectionPosition GetSctionPosition(int i, int j)
        {
            PanelSectionPosition result = PanelSectionPosition.Center;
            bool isLeft = i == 0;
            bool isRight = i == _nxSection - 1;
            bool isBottom = j == 0;
            bool isTop = j == _nySection - 1;

            if(isLeft)
            {
                if (isBottom) result = PanelSectionPosition.CornerBottomLeft;
                else if (isTop) result = PanelSectionPosition.CornerTopLeft;
                else result = PanelSectionPosition.SideLeft;
            }
            else if(isRight)
            {
                if (isBottom) result = PanelSectionPosition.CornerBottomRight;
                else if (isTop) result = PanelSectionPosition.CornerTopRight;
                else result = PanelSectionPosition.SideRight;
            }
            else
            {
                if (isBottom) result = PanelSectionPosition.SideBottom;
                else if (isTop) result = PanelSectionPosition.SideTop;
                else result = PanelSectionPosition.Center;
            }

            return result;
        }

        private void OnToolMoveMessage(ToolMoveMessage msg)
        {
            //var position = PanelModel.Transform.Inverse.Transform(msg.Position);
            //var tool = ImplicitToolFactory.Create(position.ToVector3d(), msg.Direction.ToVector3d(), msg.Length, msg.Radius);
            var tool = ImplicitToolFactory.Create(msg.Position.ToVector3d(), msg.Direction.ToVector3d(), msg.Length, msg.Radius);
            var panel = new AxisAlignedBox3d(new Vector3d(_cornerX, _cornerY, _cornerZ), new Vector3d(_cornerX + SizeX, _cornerY + SizeY, _cornerZ + SizeZ));

            //{
            //    var builder = new MeshBuilder();

            //    builder.AddSphere(msg.Position, 10);
            //    PanelModel.Children.Add(new GeometryModel3D() { Geometry = builder.ToMesh(), Material = MaterialHelper.CreateMaterial(Colors.Yellow) });
            //}

            Task.Run(() =>
            {
                var toolBound = tool.Bounds();

                if(panel.Intersects(toolBound))
                {
                    var intersect = panel.Intersect(toolBound);
                    var xMinIndex = GetSectionIndex(intersect.Min.x, panel.Min.x, panel.Max.x, _nxSection);
                    var xMaxIndex = GetSectionIndex(intersect.Max.x, panel.Min.x, panel.Max.x, _nxSection);
                    var yMinIndex = GetSectionIndex(intersect.Min.y, panel.Min.y, panel.Max.y, _nySection);
                    var yMaxIndex = GetSectionIndex(intersect.Max.y, panel.Min.y, panel.Max.y, _nySection);

                    if (xMinIndex < 0) xMinIndex = 0;
                    if (yMinIndex < 0) yMinIndex = 0;

                    for (int i = xMinIndex; i <= xMaxIndex; i++)
                    {
                        for (int j = yMinIndex; j <= yMaxIndex; j++)
                        {
                            MessengerInstance.Send(new SectionToolMoveMessage() { XSectionIndex = i, YSectionIndex = j, Tool = tool });
                        }
                    }
                }
            });
        }

        private void OnRoutToolMoveMessage(RoutToolMoveMessage msg)
        {
            var tool = ImplicitToolFactory.Create(msg.Position.ToVector3d(), msg.Direction.ToVector3d(), msg.Length, msg.Radius);
            var panel = new AxisAlignedBox3d(new Vector3d(_cornerX, _cornerY, _cornerZ), new Vector3d(_cornerX + SizeX, _cornerY + SizeY, _cornerZ + SizeZ));

            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
            //{
            //    var builder = new MeshBuilder();

            //    builder.AddSphere(msg.Position, 10);
            //    PanelModel.Children.Add(new GeometryModel3D() { Geometry = builder.ToMesh(), Material = MaterialHelper.CreateMaterial(Colors.Yellow) });
            //});

            Task.Run(() =>
            {
                var toolBound = tool.Bounds();

                if (panel.Intersects(toolBound))
                {
                    var routing = _lastRoutings.GetOrAdd(msg.ToolId, (id) => ImplicitRoutingFactory.Create(id, msg.Direction.ToVector3d(), msg.Length, msg.Radius));
                    var pt = msg.Position.ToVector3d();
                    var box = routing.Add(ref pt);

                    var intersect = panel.Intersect(box);
                    var xMinIndex = GetSectionIndex(intersect.Min.x, panel.Min.x, panel.Max.x, _nxSection);
                    var xMaxIndex = GetSectionIndex(intersect.Max.x, panel.Min.x, panel.Max.x, _nxSection);
                    var yMinIndex = GetSectionIndex(intersect.Min.y, panel.Min.y, panel.Max.y, _nySection);
                    var yMaxIndex = GetSectionIndex(intersect.Max.y, panel.Min.y, panel.Max.y, _nySection);

                    if (xMinIndex < 0) xMinIndex = 0;
                    if (yMinIndex < 0) yMinIndex = 0;

                    for (int i = xMinIndex; i <= xMaxIndex; i++)
                    {
                        for (int j = yMinIndex; j <= yMaxIndex; j++)
                        {
                            MessengerInstance.Send(new SectionRoutToolMoveMessage() { XSectionIndex = i, YSectionIndex = j, Rout = routing });
                        }
                    }
                }
                else
                {
                    _lastRoutings.TryRemove(msg.ToolId, out ImplicitRouting ir);
                }
            });
        }

        private static int GetSectionIndex(double value, double min, double max, int nSections)
        {
            var f = nSections * (value - min) / (max - min);
            var i= (int)Math.Ceiling(f) - 1;

            return i;
        }

        public void AppendMeshGeometry3D(MeshGeometry3D model, Color color)
        {
            _panelModel.Children.Add(new GeometryModel3D()
            {
                Geometry = model,
                Material = MaterialHelper.CreateMaterial(color)
            });
        }
    }
}
