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
    public class DiskOnConeToolViewModel : ToolViewModel<DiskOnConeTool>
    {
        private double _diameter;
        [Category("Geometry")]
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                if (Set(ref _diameter, value, nameof(Diameter)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }

        private double _cuttingRadialThickness;
        [Category("Geometry")]
        public double CuttingRadialThickness
        {
            get { return _cuttingRadialThickness; }
            set { if (Set(ref _cuttingRadialThickness, value, nameof(CuttingRadialThickness))) UpdateModel(); }
        }

        private double _bodyThickness;
        [Category("Geometry")]
        public double BodyThickness
        {
            get { return _bodyThickness; }
            set
            {
                if (Set(ref _bodyThickness, value, nameof(BodyThickness)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _cuttingThickness;
        [Category("Geometry")]
        public double CuttingThickness
        {
            get { return _cuttingThickness; }
            set
            {
                if (Set(ref _cuttingThickness, value, nameof(CuttingThickness)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _postponemntDiameter;
        [Category("Geometry")]
        public double PostponemntDiameter
        {
            get { return _postponemntDiameter; }
            set
            {
                if (Set(ref _postponemntDiameter, value, nameof(PostponemntDiameter)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }

        private double _postponemntLength;
        [Category("Geometry")]
        public double PostponemntLength
        {
            get { return _postponemntLength; }
            set
            {
                if (Set(ref _postponemntLength, value, nameof(PostponemntLength)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }

        private double _radialUsefulLength;
        [Category("Working")]
        public double RadialUsefulLength
        {
            get { return _radialUsefulLength; }
            set { if (Set(ref _radialUsefulLength, value, nameof(RadialUsefulLength))) UpdateModel(); }
        }

        protected override ToolType ToolType => ToolType.DiskOnCone;

        public DiskOnConeToolViewModel() : base()
        {
        }

        protected override void UpdateModel(DiskOnConeTool tool)
        {
            tool.Diameter = Diameter;
            tool.CuttingRadialThickness = CuttingRadialThickness;
            tool.BodyThickness = BodyThickness;
            tool.CuttingThickness = CuttingThickness;
            tool.RadialUsefulLength = RadialUsefulLength;
            tool.PostponemntDiameter = PostponemntDiameter;
            tool.PostponemntLength = PostponemntLength;

            base.UpdateModel(tool);
        }

        protected override void UpdateViewModel()
        {
            Diameter = _tool.Diameter;
            CuttingRadialThickness = _tool.CuttingRadialThickness;
            BodyThickness = _tool.BodyThickness;
            CuttingThickness = _tool.CuttingThickness;
            RadialUsefulLength = _tool.RadialUsefulLength;
            PostponemntDiameter = _tool.PostponemntDiameter;
            PostponemntLength = _tool.PostponemntLength;
            PostponemntLength = _tool.PostponemntLength;

            base.UpdateViewModel();
        }

        protected override MeshGeometry3D GetGeometry() => ToolsMeshHelper.GetDiskOnConeMesh(_tool, new Point3D(), new Vector3D(0.0, 0.0, -1.0));

    }
}
