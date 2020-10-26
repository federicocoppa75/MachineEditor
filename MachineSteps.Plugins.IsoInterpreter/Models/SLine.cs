using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class SLine : IsoLine
    {
        public int Value { get; set; }

        public override IsoLineType Type => IsoLineType.S;

        public static bool Match(string data) => Regex.IsMatch(data, "^S(\\d+)");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Replace(data, "S", "");
            str = str.Replace("M3", "");
            var v = int.Parse(str);

            return new SLine() { Value = v };
        }
    }
}
