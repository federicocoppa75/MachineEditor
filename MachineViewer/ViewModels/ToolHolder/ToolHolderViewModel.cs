using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MachineModels.Enums;
using MachineModels.Models.Tools;
using MachineViewer.Extensions;
using MachineViewer.Messages;
using MachineViewer.Messages.MaterialRemoval;
using MachineViewer.Messages.Tooling;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using MachineViewModelUtils.Extensions;
using MachineViewModelUtils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.ToolHolder
{
    public abstract class ToolHolderViewModel : MachineElementViewModel
    {
        private Transform3D _chainTransform;

        protected Tool _tool;

        public abstract ToolHolderType ToolHolderType { get; }

        public int ToolHolderId { get; set; }

        public Point3D Position { get; set; }

        public Vector3D Direction { get; set; }

        public string ToolName { get; set; } = string.Empty;

        public bool ActiveTool { get; set; }

        public ToolHolderViewModel() : base()
        {
            Messenger.Default.Register<LoadToolMessage>(this, OnLoadTool);
            Messenger.Default.Register<UnloadToolMessage>(this, OnUnloadTool);
            Messenger.Default.Register<GetActiveToolMessage>(this, OnGetActiveToolMessage);
            Messenger.Default.Register<GetActiveRoutToolMessage>(this, OnGetActiveRoutToolMessage);
        }

        private void OnUnloadTool(UnloadToolMessage msg)
        {
            Children.Clear();
            ToolName = string.Empty;
            _tool = null;
        }

        private void OnLoadTool(LoadToolMessage msg)
        {
            if(msg.ToolHolderId == ToolHolderId)
            {
                Children.Add(GetToolModel(msg.Tool));
                ToolName = msg.Tool.Name;
                _tool = msg.Tool;
            }
        }


        private void OnGetActiveToolMessage(GetActiveToolMessage msg)
        {
            if(ActiveTool)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var t = this.GetChainTansform();
                    var p = t.Transform(Position);
                    var v = t.Transform(Direction);
                    msg.SetData(p, v, _tool);
                });                
            }
        }

        private void OnGetActiveRoutToolMessage(GetActiveRoutToolMessage msg)
        {
            if (ActiveTool)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (_chainTransform == null) _chainTransform = this.GetChainTansform();

                    var t = _chainTransform.Value;

                    Task.Run(() =>
                    {
                        var p = t.Transform(Position);
                        var v = t.Transform(Direction);
                        if (_tool != null) msg.SetData(p, v, _tool, ToolHolderId);
                    });                   
                });
            }
        }

        private ModelVisual3D GetToolModel(Tool tool)
        {
            ModelVisual3D mv = null;
            MachineElementViewModel modelFactory(string s) => new MachineElementViewModel() { Name = s };

            switch (tool.ToolType)
            {
                case ToolType.None:
                    break;
                case ToolType.Base:
                    break;
                case ToolType.Simple:
                    mv = ToolsMeshHelper.GetSimpleModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.TwoSection:
                    mv = ToolsMeshHelper.GetTwoSectionModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.Pointed:
                    mv = ToolsMeshHelper.GetPointedModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.Disk:
                    mv = ToolsMeshHelper.GetDiskModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.BullNoseConcave:
                    break;
                case ToolType.BullNoseConvex:
                    break;
                case ToolType.Composed:
                    break;
                case ToolType.Countersink:
                    mv = ToolsMeshHelper.GetCountersinkModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.DiskOnCone:
                    mv = ToolsMeshHelper.GetDiskOnConeModel(tool, Position, Direction, modelFactory);
                    break;
                case ToolType.AngularTransmissionImpl:
                    mv = ToolsMeshHelper.GetAngolarTransmissionModell(tool, Position, Direction, modelFactory);
                    break;
                default:
                    break;
            }

            if (mv == null) throw new NotImplementedException();

            if (mv is MachineElementViewModel mevm) mevm.Name = tool.Name;

            return mv;
        }
    }
}
