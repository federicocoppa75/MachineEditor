using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G3Arc : GArc
    {
        public override IsoLineType Type => IsoLineType.G3;

        public static bool Match(string data) => Regex.IsMatch(data, "\\AG3( \\S+)+");

        public static IsoLine Parse(string data)
        {
            var isoLine = new G3Arc();

            ParseAndSetParameters(data, 3, isoLine);

            return isoLine;
        }
    }
}
