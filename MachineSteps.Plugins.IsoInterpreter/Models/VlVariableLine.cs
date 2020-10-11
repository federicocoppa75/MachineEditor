using MachineSteps.Plugins.IsoInterpreter.Enums;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class VlVariableLine : VariableLine
    {
        public static bool Match(string data) => Regex.IsMatch(data, "\\AVL([0-9])+=");

        public static IsoLine Parse(string data)
        {
            var str = Regex.Match(data, "\\AVL([0-9])+=").Value;
            var v = data.Replace(str, "");
            str = str.Replace("=", "");

            return new VlVariableLine() { Name = str, Value = v };
        }

        public override IsoLineType Type => IsoLineType.VlVariable;

    }
}
