using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class EndIfLine : IsoLine
    {
        public int IfThenLine { get; set; }

        public static bool Match(string data) => Regex.IsMatch(data, "^ENDIF\\s*(;.*)?");

        public static IsoLine Parse(string data) => new EndIfLine() { IfThenLine = IsoLineFactory.GetLastIfThenLine() };

        public override IsoLineType Type => IsoLineType.EndIf;
    }
}
