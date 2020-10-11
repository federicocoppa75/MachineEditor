using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class G91 : IsoLine
    {
        public static bool Match(string data) => Regex.IsMatch(data, "\\AG91($|\\s)");

        public static bool IsSingle(string data) => Regex.IsMatch(data, "\\AG91($|(\\s)+$)");

        public static string Remove(string data) => Regex.Replace(data, "\\AG91(\\s)+", "");

        public static IsoLine Parse(string data) => new G91();

        public override IsoLineType Type => IsoLineType.G91;
    }
}
