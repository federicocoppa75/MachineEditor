using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineModels.Enums;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.ToolChange;

namespace MachineViewer.ViewModels.ToolHolder
{
    public class AutoSourceToolHolderViewModel : ToolHolderViewModel
    {
        // modello dell'utensile prima del caricamento verso un sink (ovvero in partenza da questo tool holder)
        private Visual3D _loadedTool;

        // trasformazione originale prima del caricamento verso un sink (ovvero in partenza da questo tool holder)
        private Transform3D _preLoadTransform;

        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSource;

        public AutoSourceToolHolderViewModel() : base()
        {
            Messenger.Default.Register<GetAvailableToolMessage>(this, OnGetAvailableToolMessage);
            Messenger.Default.Register<LoadToolMessage>(this, OnLoadToolMessage);
            Messenger.Default.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Default.Register<UnloadAllToolsMessage>(this, OnUnloadAllToolsMessage);
        }

        private void OnGetAvailableToolMessage(GetAvailableToolMessage msg)
        {
            if(!string.IsNullOrEmpty(ToolName)) msg?.SetAvailableTool(ToolHolderId, ToolName);
        }

        private void OnLoadToolMessage(LoadToolMessage msg)
        {
            if(msg.ToolSource == ToolHolderId)
            {
                if (Children.Count == 1)
                {
                    _loadedTool = Children[0];
                    _preLoadTransform = _loadedTool.Transform;
                    Children.Clear();
                    Messenger.Default.Send(new Messages.Tooling.CompleteToolLoadingMessage()
                    {
                        ToolSink = msg.ToolSink,
                        ToolModel = _loadedTool,
                        ToolData = _tool,
                        BackNotifyId = msg.BackNotifyId
                    });
                }
                else
                {
                    throw new InvalidOperationException("Load tool from empty tool holder source!");
                }
            }
        }

        private void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolSource == ToolHolderId)
            {
                ApplyUnloadTool();
                if (msg.BackNotifyId > 0) Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
        }

        private void OnUnloadAllToolsMessage(UnloadAllToolsMessage msg)
        {
            ApplyUnloadTool();
        }


        private void ApplyUnloadTool()
        {
            if((_loadedTool != null) && (_preLoadTransform != null))
            {
                _loadedTool.Transform = _preLoadTransform;
                Children.Add(_loadedTool);
                _loadedTool = null;
                _preLoadTransform = null;
            }
        }

    }
}
