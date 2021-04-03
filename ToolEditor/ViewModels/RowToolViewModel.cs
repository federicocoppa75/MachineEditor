using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Enums;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolEditor.Messages;

namespace ToolEditor.ViewModels
{
    public class RowToolViewModel : ViewModelBase
    {
        private Tool _tool;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, nameof(Name)); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value, nameof(Description)); }
        }


        private double _totalDiameter;

        public double TotalDiameter
        {
            get { return _totalDiameter; }
            set { Set(ref _totalDiameter, value, nameof(TotalDiameter)); }
        }


        private double _totlaLength;

        public double TotalLength
        {
            get { return _totlaLength; }
            set { Set(ref _totlaLength, value, nameof(TotalLength)); }
        }

        private ToolType _toolType;

        public ToolType ToolType
        {
            get { return _toolType; }
            set { Set(ref _toolType, value, nameof(ToolType)); }
        }


        private ToolLinkType _toolLinkType;

        public ToolLinkType ToolLinkType
        {
            get { return _toolLinkType; }
            set { Set(ref _toolLinkType, value, nameof(ToolLinkType)); }
        }


        private string _fullDescription;

        public string FullDescription
        {
            get { return _fullDescription; }
            set { Set(ref _fullDescription, value, nameof(FullDescription)); }
        }


        public RowToolViewModel(Tool tool)
        {
            _tool = tool;

            UpdateViewModel();

            MessengerInstance.Register<ToolModelDataChanged>(this, OnDataChanged);
            MessengerInstance.Register<GetToolMessage>(this, OnGetToolMessage);
        }

        private void OnDataChanged(ToolModelDataChanged msg)
        {
            if(ReferenceEquals(_tool, msg.Tool))
            {
                UpdateViewModel();
            }
        }

        private void OnGetToolMessage(GetToolMessage msg)
        {
            if(string.Compare(Name, msg.Name) == 0)
            {
                msg.SetTool(_tool);
            }
        }

        private void UpdateViewModel()
        {
            Name = _tool.Name;
            Description = _tool.Description;
            TotalDiameter = _tool.GetTotalDiameter();
            TotalLength = _tool.GetTotalLength();
            ToolType = _tool.ToolType;
            ToolLinkType = _tool.ToolLinkType;
            FullDescription = "Full descriptio (ToDo!)";
        }

        public Tool GetModel() => _tool;
    }
}
