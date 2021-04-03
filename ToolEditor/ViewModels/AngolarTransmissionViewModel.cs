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

        private string _toolName = string.Empty;
        [Category("Sub spindle")]
        public string ToolName 
        { 
            get => _toolName;
            set
            {
                if (Set(ref _toolName, value, nameof(ToolName)))
                {
                    MessengerInstance.Send(new GetToolMessage() { Name = ToolName, SetTool = (t) => UpdateToolGeometry(t) });
                }
            }
        }

        private Point3D _position = new Point3D();
        [Category("Sub spindle")]
        public Point3D Position 
        { 
            get => _position;
            set
            {
                if (Set(ref _position, value, nameof(Position)))
                {
                    UpdateSpindleGeometry();
                }
            }
        }

        private Vector3D _direction = new Vector3D();
        [Category("Sub spindle")]
        public Vector3D Direction 
        { 
            get => _direction;
            set
            {
                if (Set(ref _direction, value, nameof(Direction)))
                {
                    UpdateSpindleGeometry();
                }
            }
        }

        [Browsable(false)]
        public MeshGeometry3D BodyGeometry { get; set; }

        [Browsable(false)]
        public MeshGeometry3D ToolGeometry { get; set; }

        [Browsable(false)]
        public MeshGeometry3D SpindleGeometry { get; set; }

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
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var p1 = Position;
            var p2 = Position + (Vector3D)(Direction * 20.0);

            builder.AddArrow(p1, p2, 6.0, 2.0);
            SpindleGeometry = builder.ToMesh();
            RaisePropertyChanged(nameof(SpindleGeometry));
        }

        private void UpdateToolGeometry(Tool tool)
        {
            if(tool != null)
            {
                var len = tool.GetTotalLength();
                var dia = tool.GetTotalDiameter();

                var builder = new HelixToolkit.Wpf.MeshBuilder();
                var p1 = Position;
                var p2 = Position + (Vector3D)(Direction * len);

                builder.AddCylinder(p1, p2, dia / 2.0, 32, true, true);
                ToolGeometry = builder.ToMesh();
                RaisePropertyChanged(nameof(ToolGeometry));
            }
        }

        protected override void UpdateModel(AngolarTransmission tool)
        {
            tool.BodyModelFile = BodyModelFile;
            tool.ToolName = ToolName;
            tool.Position = Position.ToVector();
            tool.Direction = Direction.ToVector();

            base.UpdateModel(tool);
        }

        protected override void UpdateViewModel()
        {
            BodyModelFile = _tool.BodyModelFile;
            ToolName = _tool.ToolName;
            Position = _tool.Position.ToPoint3D();
            Direction = _tool.Direction.ToVector3D();

            base.UpdateViewModel();
        }

        protected override MeshGeometry3D GetGeometry()
        {
            return base.GetGeometry();
        }

        protected override ToolType ToolType => ToolType.AngularTransmission;
    }
}
