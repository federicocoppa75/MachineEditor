using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class M3SLine : IsoLine
    {
        public int Value { get; set; }

        public override IsoLineType Type => IsoLineType.M3S;

        public static bool Match(string data) => Regex.IsMatch(data, "^M3S(\\d+)");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Replace(data, "M3S", "");
            var v = int.Parse(str);

            return new M3SLine() { Value = v };
        }
    }
}
