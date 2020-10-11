using g3;
using GalaSoft.MvvmLight.Threading;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels
{
    public class CornerPanelSectionViewMolde : SidePanelSectionViewModel
    {
        private SafeGeneratedMeshData _safeNdSideMeshData = new SafeGeneratedMeshData();

        private GeometryModel3D _ndSideModel;

        private AxisAlignedBox3d _ndSideFilterBox;

        private ImplicitNaryDifference3d _ndProcessedSide;

        private int _processingNdSidePanel = 0;

        protected override void InitializeModel()
        {
            _sectionModel = CreateSectonModel();
            _sideModel = CreateSideModel();
            _ndSideModel = CreateNdSideModel();

            var model = new Model3DGroup();

            model.Children.Add(_sectionModel);
            model.Children.Add(_sideModel);
            model.Children.Add(_ndSideModel);

            Model = model;
        }

        private GeometryModel3D CreateSideModel()
        {
            GeometryModel3D model = null;

            switch (Position)
            {
                case PanelSectionPosition.CornerBottomLeft:
                case PanelSectionPosition.CornerBottomRight:
                    model = base.CreateSideModel(PanelSectionPosition.SideBottom);
                    break;
                case PanelSectionPosition.CornerTopLeft:
                case PanelSectionPosition.CornerTopRight:
                    model = base.CreateSideModel(PanelSectionPosition.SideTop);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return model;
        }

        private GeometryModel3D CreateNdSideModel()
        {
            GeometryModel3D model = null;

            switch (Position)
            {
                case PanelSectionPosition.CornerBottomLeft:
                case PanelSectionPosition.CornerTopLeft:
                    model = base.CreateSideModel(PanelSectionPosition.SideLeft);
                    break;
                case PanelSectionPosition.CornerBottomRight:
                case PanelSectionPosition.CornerTopRight:
                    model = base.CreateSideModel(PanelSectionPosition.SideRight);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return model;
        }

        protected override void InitializeProcessedSide()
        {
            GetSidesPositions(out PanelSectionPosition position, out PanelSectionPosition ndPosition);

            ImplicitFace face = CreateProcessedSide(position);
            ImplicitFace ndFace = CreateProcessedSide(ndPosition);

            _sideFilterBox = face.Bounds();
            _sideFilterBox.Expand(0.0001);
            face.Width += 2.0;
            face.Height += 2.0;

            _processedSide = new ImplicitNaryDifference3d()
            {
                A = face,
                BSet = new List<BoundedImplicitFunction3d>()
            };

            _ndSideFilterBox = ndFace.Bounds();
            _ndSideFilterBox.Expand(0.0001);
            ndFace.Width += 2.0;
            ndFace.Height += 2.0;

            _ndProcessedSide = new ImplicitNaryDifference3d()
            {
                A = ndFace,
                BSet = new List<BoundedImplicitFunction3d>()
            };
        }

        private void GetSidesPositions(out PanelSectionPosition position, out PanelSectionPosition ndPosition)
        {
            switch (Position)
            {

                case PanelSectionPosition.CornerBottomLeft:
                    position = PanelSectionPosition.SideBottom;
                    ndPosition = PanelSectionPosition.SideLeft;
                    break;
                case PanelSectionPosition.CornerBottomRight:
                    position = PanelSectionPosition.SideBottom;
                    ndPosition = PanelSectionPosition.SideRight;
                    break;
                case PanelSectionPosition.CornerTopLeft:
                    position = PanelSectionPosition.SideTop;
                    ndPosition = PanelSectionPosition.SideLeft;
                    break;
                case PanelSectionPosition.CornerTopRight:
                    position = PanelSectionPosition.SideTop;
                    ndPosition = PanelSectionPosition.SideRight;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        protected override void ProcessPendingExtension(List<BoundedImplicitFunction3d> pendingTools)
        {
            base.ProcessPendingExtension(pendingTools);

            ProcessPendingExtensionImplementation(_ndProcessedSide, pendingTools, _ndSideFilterBox, (m) => GetMeshDataAsync(m, _safeNdSideMeshData));
        }

        protected override void OnProcessPendingRemovalMessage(ProcessPendingRemovalMessage msg)
        {
            base.OnProcessPendingRemovalMessage(msg);

            _safeNdSideMeshData.ExeAndReset((positions, triangleIndices) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _ndSideModel.Geometry = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection(positions),
                        TriangleIndices = new Int32Collection(triangleIndices)
                    };
                });
            });
        }

        protected override void AddRautingToProcessedPanel(SectionRoutToolMoveMessage msg, ImplicitRouting rout)
        {
            base.AddRautingToProcessedPanel(msg, rout);
            _ndProcessedSide.BSet.Add(rout);
        }

        protected override void ProcessPendingRout()
        {
            base.ProcessPendingRout();

            if (Interlocked.CompareExchange(ref _processingNdSidePanel, 1, 0) == 0)
            {
                Task.Run(() =>
                {
                    ProcessPendingRoutImplementation(_ndProcessedSide, _ndSideFilterBox, (m) => GetMeshDataAsync(m, _safeNdSideMeshData));
                    Interlocked.Exchange(ref _processingNdSidePanel, 0);
                });
            }
        }
    }
}
