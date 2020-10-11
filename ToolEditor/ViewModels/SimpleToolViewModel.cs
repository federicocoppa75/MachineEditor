using GalaSoft.MvvmLight.Messaging;
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
using ToolEditor.Messages;

namespace ToolEditor.ViewModels
{
    public class SimpleToolViewModel : ToolViewModel<SimpleTool>
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
                
        private double _length;
        [Category("Geometry")]
        public double Length
        {
            get { return _length; }
            set
            {
                if (Set(ref _length, value, nameof(Length)))
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
            set { if (Set(ref _usefulLength, value, nameof(UsefulLength))) UpdateModel(); }
        }

        protected override ToolType ToolType => ToolType.Simple;

        public SimpleToolViewModel() : base()
        {
        }

        protected override void UpdateViewModel()
        {
            Diameter = _tool.Diameter;
            Length = _tool.Length;
            UsefulLength = _tool.UsefulLength;

            base.UpdateViewModel();
        }

        protected override void UpdateModel(SimpleTool tool)
        {
            tool.Diameter = Diameter;
            tool.Length = Length;
            tool.UsefulLength = UsefulLength;

            base.UpdateModel(tool);
        }

        protected override MeshGeometry3D GetGeometry() => ToolsMeshHelper.GetSimpleMesh(_tool, new Point3D(), new Vector3D(0.0, 0.0, -1.0));
    }
}
