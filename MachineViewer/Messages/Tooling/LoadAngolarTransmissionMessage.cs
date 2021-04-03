using MachineModels.Models.Tools;

namespace MachineViewer.Messages.Tooling
{
    public class LoadAngolarTransmissionMessage : LoadToolMessage
    {
        public Tool SubTool { get; set; }
    }
}
