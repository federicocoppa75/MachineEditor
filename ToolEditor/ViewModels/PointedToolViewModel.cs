using MachineModels.Enums;
using MachineModels.Models.Tools;
using MachineViewModelUtils.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ToolEditor.ViewModels
{
    public class PointedToolViewModel : ToolViewModel<PointedTool>
    {
        private double _diameter;
        [Category("Geometry")]
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                if(Set(ref _diameter, value, nameof(Diameter)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }
        
        private double _straightLength;
        [Category("Geometry")]
        public double StraightLength
        {
            get { return _straightLength; }
            set
            {
                if(Set(ref _straightLength, value, nameof(StraightLength)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _coneHeight;
        [Category("Geometry")]
        public double ConeHeight
        {
            get { return _coneHeight; }
            set
            {
                if(Set(ref _coneHeight, value, nameof(ConeHeight)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _usefulLenght;
        [Category("Working")]
        public double UsefulLength
        {
            get { return _usefulLenght; }
            set { if(Set(ref _usefulLenght, value, nameof(UsefulLength))) UpdateModel(); }
        }

        protected override ToolType ToolType => ToolType.Pointed;

        public PointedToolViewModel() : base()
        {
        }

        protected override void UpdateViewModel()
        {
            Diameter = _tool.Diameter;
            StraightLength = _tool.StraightLength;
            ConeHeight = _tool.ConeHeight;
            UsefulLength = _tool.UsefulLength;

            base.UpdateViewModel();
        }

        protected override void UpdateModel(PointedTool tool)
        {
            tool.Diameter = Diameter;
            tool.StraightLength = StraightLength;
            tool.ConeHeight = ConeHeight;
            tool.UsefulLength = UsefulLength;

            base.UpdateModel(tool);
        }

        protected override MeshGeometry3D GetGeometry() => ToolsMeshHelper.GetPointedMesh(_tool, new Point3D(), new Vector3D(0.0, 0.0, -1.0));

    }
}
