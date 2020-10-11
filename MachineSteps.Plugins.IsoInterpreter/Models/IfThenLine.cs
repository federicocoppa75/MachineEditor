using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class IfThenLine : IsoLine
    {
        public string Condition { get; set; }

        public int ElseLine { get; set; } = -1;

        public int EndLine { get; set; } = -1;

        public static bool Match(string data) => Regex.IsMatch(data, "^IF .* THEN\\s*(;.*)?");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Replace(data, "^IF ", "");
            str = Regex.Replace(str, " THEN\\s*(;.*)?", "");

            return new IfThenLine() { Condition = str };
        }

        public override IsoLineType Type => IsoLineType.IfThen;
    }
}
