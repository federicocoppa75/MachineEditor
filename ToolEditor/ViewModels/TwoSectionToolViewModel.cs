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
    public class TwoSectionToolViewModel : ToolViewModel<TwoSectionTool>
    {
        private double _diameter1;
        [Category("Geometry")]
        public double Diameter1
        {
            get { return _diameter1; }
            set
            {
                if(Set(ref _diameter1, value, nameof(Diameter1)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }

        private double _diameter2;
        [Category("Geometry")]
        public double Diameter2
        {
            get { return _diameter2; }
            set
            {
                if (Set(ref _diameter2, value, nameof(Diameter2)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalDiameter));
                }
            }
        }

        private double _length1;
        [Category("Geometry")]
        public double Length1
        {
            get { return _length1; }
            set
            {
                if (Set(ref _length1, value, nameof(Length1)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _length2;
        [Category("Geometry")]
        public double Length2
        {
            get { return _length2; }
            set
            {
                if (Set(ref _length2, value, nameof(Length2)))
                {
                    UpdateModel();
                    RaisePropertyChanged(nameof(TotalLength));
                }
            }
        }

        private double _usefulLength;
        [Category("Working")]
        public double UsefulLength
        {
            get { return _usefulLength; }
            set { if(Set(ref _usefulLength, value, nameof(UsefulLength))) UpdateModel(); }
        }


        protected override ToolType ToolType => ToolType.TwoSection;

        public TwoSectionToolViewModel() : base()
        {

        }

        protected override void UpdateModel(TwoSectionTool tool)
        {
            tool.Diameter1 = Diameter1;
            tool.Diameter2 = Diameter2;
            tool.Length1 = Length1;
            tool.Length2 = Length2;
            tool.UsefulLength = UsefulLength;

            base.UpdateModel(tool);
        }

        protected override void UpdateViewModel()
        {
            Diameter1 = _tool.Diameter1;
            Diameter2 = _tool.Diameter2;
            Length1 = _tool.Length1;
            Length2 = _tool.Length2;
            UsefulLength = _tool.UsefulLength;

            base.UpdateViewModel();
        }

        protected override MeshGeometry3D GetGeometry() => ToolsMeshHelper.GetTwoSectioMesh(_tool, new Point3D(), new Vector3D(0.0, 0.0, -1.0));

    }
}
