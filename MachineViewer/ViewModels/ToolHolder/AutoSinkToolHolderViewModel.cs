using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Enums;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.ToolChange;

namespace MachineViewer.ViewModels.ToolHolder
{
    public class AutoSinkToolHolderViewModel : ToolHolderViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSink;

        public AutoSinkToolHolderViewModel() : base()
        {
            Messenger.Default.Register<GetAvailableToolSinkMessage>(this, OnGetAvailableToolSinkMessage);
            Messenger.Default.Register<Messages.Tooling.CompleteToolLoadingMessage>(this, OnCompleteToolLoadingMessage);
            Messenger.Default.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Default.Register<UnloadAllToolsMessage>(this, OnUnloadAllToolsMessage);
        }

        private void OnGetAvailableToolSinkMessage(GetAvailableToolSinkMessage msg)
        {
            msg?.SetAvailableToolSink(ToolHolderId, Name);
        }

        private void OnCompleteToolLoadingMessage(Messages.Tooling.CompleteToolLoadingMessage msg)
        {
            if (msg.ToolSink == ToolHolderId)
            {
                if (Children.Count == 0)
                {
                    msg.ToolModel.Transform = new TranslateTransform3D() { OffsetX = Position.X, OffsetY = Position.Y, OffsetZ = Position.Z };
                    Children.Add(msg.ToolModel);
                    _tool = msg.ToolData;
                    if (msg.BackNotifyId > 0) Messenger.Default.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
                }
                else
                {
                    throw new InvalidOperationException("Could not load tool in full tool holder!");
                }
            }
        }

        private void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolSink == ToolHolderId)
            {
                _tool = null;
                Children.Clear();
            }
        }

        private void OnUnloadAllToolsMessage(UnloadAllToolsMessage msg)
        {
            Children.Clear();
        }

    }
}
