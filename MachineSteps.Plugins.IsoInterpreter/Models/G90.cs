using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G90 : IsoLine
    {
        public static bool Match(string data) => Regex.IsMatch(data, "\\AG90($|\\s)");

        public static bool IsSingle(string data) => Regex.IsMatch(data, "\\AG90($|(\\s)+$)");

        public static string Remove(string data) => Regex.Replace(data, "\\AG90(\\s)+", "");

        public static IsoLine Parse(string data) => new G90();

        public override IsoLineType Type => IsoLineType.G90;
    }
}
