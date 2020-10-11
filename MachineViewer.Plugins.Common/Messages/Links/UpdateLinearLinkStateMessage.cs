namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class UpdateLinearLinkStateMessage : UpdateLinkStateMessage<double>
    {
        public UpdateLinearLinkStateMessage(int id, double value) : base(id, value)
        {
        }
    }
}
