using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G210Line : GLine
    {
        public override IsoLineType Type => IsoLineType.G210;

        public static bool Match(string data) => Regex.IsMatch(data, "\\AG210( \\S+)+");

        public static IsoLine Parse(string data)
        {
            var isoLine = new G210Line();

            ParseAndSetParameters(data, 210, isoLine);

            return isoLine;
        }
    }
}
