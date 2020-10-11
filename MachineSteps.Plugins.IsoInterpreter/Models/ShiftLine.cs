using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class ShiftLine : IsoLine
    {
        public ShiftDirection Direction { get; set; }

        public double Value { get; set; }

        public static bool Match(string data) => Regex.IsMatch(data, "SHF\\[(X|Y|Z)\\]=-?[0-9]+(.[0-9]+)?");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Match(data, "SHF\\[(X|Y|Z)\\]=-?[0-9]+(.[0-9]+)?").Value;
            var v = double.Parse(Regex.Match(str, "-?[0-9]+(.[0-9]+)?").Value, CultureInfo.InvariantCulture);
            var d = Regex.Match(data, "\\[(X|Y|Z)\\]").Value.Replace("[", "").Replace("]", "");
            
            return new ShiftLine() {  Value = v, Direction = Convert(d) };
        }

        private static ShiftDirection Convert(string dir)
        {
            if(string.Compare(dir, "X") == 0)
            {
                return ShiftDirection.X;
            }
            else if (string.Compare(dir, "Y") == 0)
            {
                return ShiftDirection.Y;
            }
            else if (string.Compare(dir, "Z") == 0)
            {
                return ShiftDirection.Z;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override IsoLineType Type => IsoLineType.Shift;
    }
}
