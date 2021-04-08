using GalaSoft.MvvmLight;
using MachineModels.Enums;
using MachineModels.Models.Tools;
using MachineViewModelUtils.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using ToolEditor.Views;
using MachineViewModelUtils.Extensions;
using ToolEditor.Messages;
using System.Collections.Generic;
using System.Windows.Media;
using HelixToolkit.Wpf;
using System;

namespace ToolEditor.ViewModels
{
    public class AngolarTransmissionViewModel : ToolViewModel<AngolarTransmission>
    {
        private string _bodyModelFile;
        [Category("Base")]
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string BodyModelFile
        {
            get => _bodyModelFile; 
            set
            {
                if (Set(ref _bodyModelFile, value, nameof(BodyModelFile)))
                {
                    UpdateModel();
                    UpdateBodyGeometry();
                }
            }
        }

        [Category("Sub spindles")]
        public ObservableCollection<AngolarTransmissionSubspindleViewModel> Subspindles { get; set; } = new ObservableCollection<AngolarTransmissionSubspindleViewModel>();

        [Browsable(false)]
        public MeshGeometry3D BodyGeometry { get; set; }

        [Browsable(false)]
        //public MeshGeometry3D ToolGeometry { get; set; }
        public Model3D ToolGeometry { get; set; }

        [Browsable(false)]
        public MeshGeometry3D SpindleGeometry { get; set; }

        public AngolarTransmissionViewModel()
        {
            Subspindles.CollectionChanged += (s, e) => 
            {
                if(!_fromModelToView) UpdateSubspindlesViewModel();
            }; 
        }

        private void UpdateBodyGeometry()
        {

            if (!string.IsNullOrEmpty(BodyModelFile) && System.IO.File.Exists(BodyModelFile))
            {
                BodyGeometry = ToolsMeshHelper.LoadModelMeshGeometry(BodyModelFile);
            }
            else
            {
                BodyGeometry = null;
            }

            RaisePropertyChanged(nameof(BodyGeometry));
        }

        private void UpdateSpindleGeometry()
        {
            if(Subspindles.Count > 0)
            {
                var builder = new HelixToolkit.Wpf.MeshBuilder();

                foreach (var item in Subspindles)
                {
                    var p1 = item.Position;
                    var p2 = item.Position + (Vector3D)(item.Direction * 20.0);
                    builder.AddArrow(p1, p2, 6.0, 2.0);
                }

                SpindleGeometry = builder.ToMesh();
                RaisePropertyChanged(nameof(SpindleGeometry));
            }
            else
            {
                SpindleGeometry = null;
            }
        }

        private void UpdateToolGeometry()
        {
            if(Subspindles.Count > 0)
            {
                var modelGroup = new Model3DGroup();

                foreach (var item in Subspindles)
                {
                    MessengerInstance.Send(new GetToolMessage()
                    {
                        Name = item.ToolName,
                        SetTool = (t) => 
                        {
                            var meshGeo3D = GetToolModel(t, item.Position, item.Direction);

                            modelGroup.Children.Add(new GeometryModel3D()
                            {
                                Geometry = meshGeo3D,
                                Material = MaterialHelper.CreateMaterial(Colors.Blue),
                                BackMaterial = MaterialHelper.CreateMaterial(Colors.Blue)
                            });
                        }
                    });
                }

                ToolGeometry = modelGroup;
                RaisePropertyChanged(nameof(ToolGeometry));
            }
        }

        private MeshGeometry3D GetToolModel(Tool t, Point3D position, Vector3D direction)
        {
            switch (t.ToolType)
            {
                case ToolType.Simple:
                    return ToolsMeshHelper.GetSimpleMesh(t, position, direction);
                case ToolType.TwoSection:
                    return ToolsMeshHelper.GetTwoSectioMesh(t, position, direction);
                case ToolType.Pointed:
                    return ToolsMeshHelper.GetPointedMesh(t, position, direction);
                case ToolType.Disk:
                    return ToolsMeshHelper.GetDiskMesh(t, position, direction);
                default:
                    throw new NotImplementedException();
            }
        }


        protected override void UpdateModel(AngolarTransmission tool)
        {
            tool.BodyModelFile = BodyModelFile;

            base.UpdateModel(tool);
        }

        protected override void UpdateViewModel()
        {
            BodyModelFile = _tool.BodyModelFile;
            UpdateSubspindlesModel();

            base.UpdateViewModel();
        }

        protected override MeshGeometry3D GetGeometry() => base.GetGeometry();

        protected override ToolType ToolType => ToolType.AngularTransmission;

        private void UpdateSubspindlesViewModel()
        {
            var at = _tool as AngolarTransmission;

            at.Subspindles.Clear();

            foreach (var item in Subspindles)
            {
                at.Subspindles.Add(new AngolarTransmission.Subspindle()
                {
                    ToolName = item.ToolName,
                    Position = item.Position.ToVector(),
                    Direction = item.Direction.ToVector()
                });
            }

            UpdateSpindleGeometry();
            UpdateToolGeometry();
        }

        private void UpdateSubspindlesModel()
        {
            Subspindles.Clear();

            foreach (var item in (_tool as AngolarTransmission).Subspindles)
            {
                Subspindles.Add(new AngolarTransmissionSubspindleViewModel()
                {
                    ToolName = item.ToolName,
                    Position = item.Position.ToPoint3D(),
                    Direction = item.Direction.ToVector3D()
                });
            }

            UpdateSpindleGeometry();
            UpdateToolGeometry();
        }
    }
}
