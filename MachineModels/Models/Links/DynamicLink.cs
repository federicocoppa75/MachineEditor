using MachineModels.Enums;
using System;

namespace MachineModels.Models.Links
{
    [Serializable]
    public class DynamicLink : Link
    {
        public int Id { get; set; } = -1;
        public LinkDirection Direction { get; set; }
    }
}