using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class MlvLine : IsoLine
    {
        public int Level { get; set; }

        public static bool Match(string data) => Regex.IsMatch(data, "MLV=\\d");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Match(data, "MLV=\\d").Value;
            var v = int.Parse(Regex.Match(str, "\\d").Value);

            return new MlvLine() { Level = v };
        }

        public override IsoLineType Type => IsoLineType.Mlv;
    }
}
