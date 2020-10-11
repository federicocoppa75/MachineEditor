using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G0Line : GLine 
    {
        public bool IsIncremental { get; set; }

        public override IsoLineType Type => IsoLineType.G0;

        public static bool Match(string data) => Regex.IsMatch(data, "\\AG0( \\S+)+");

        public static IsoLine Parse(string data, bool isIncremental)
        {
            var isoLine = new G0Line() { IsIncremental = isIncremental };

            ParseAndSetParameters(data, 0, isoLine);

            return isoLine;
        }

    }
}
