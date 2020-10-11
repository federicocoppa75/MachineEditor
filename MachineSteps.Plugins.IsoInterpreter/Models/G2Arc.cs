using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G2Arc : GArc
    {
        public override IsoLineType Type => IsoLineType.G2;

        public static bool Match(string data) => Regex.IsMatch(data, "\\AG2( \\S+)+");

        public static IsoLine Parse(string data)
        {
            var isoLine = new G2Arc();

            ParseAndSetParameters(data, 2, isoLine);

            return isoLine;
        }
    }
}
