using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G1Line : GLine
    {
        public override IsoLineType Type => IsoLineType.G1;

        public static bool Match(string data) => Regex.IsMatch(data, "\\AG1( \\S+)+");

        public static IsoLine Parse(string data)
        {
            var isoLine = new G1Line();

            ParseAndSetParameters(data, 1, isoLine);

            return isoLine;
        }
    }
}
