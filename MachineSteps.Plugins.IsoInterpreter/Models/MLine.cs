using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class MLine : IsoLine
    {
        public int Value { get; set; }

        public override IsoLineType Type => IsoLineType.M;

        public static bool Match(string data) => Regex.IsMatch(data, "^M(\\d+)");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Replace(data, "M", "");
            var v = int.Parse(str);

            return new MLine() { Value = v };
        }
    }
}
