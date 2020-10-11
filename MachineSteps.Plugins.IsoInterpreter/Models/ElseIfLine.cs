using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class ElseIfLine : IsoLine
    {
        public int IfThenLine { get; set; }

        public static bool Match(string data) => Regex.IsMatch(data, "^ELSE\\s*(;.*)?");

        public static IsoLine Parse(string data) => new ElseIfLine() { Number = IsoLineFactory.GetLastIfThenLine() };

        public override IsoLineType Type => IsoLineType.ElseIf;
    }
}
