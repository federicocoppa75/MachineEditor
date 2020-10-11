using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoIstructions.Factories
{
    public class SetVariableIstructionFactory
    {
        public static BaseIstruction Create(string data, bool isIndexWithParentesis, int lineNumber)
        {
            BaseIstruction istruction = null;
            var ss = Regex.Split(data, "\\=");

            if(ss != null && ss.Count() == 2)
            {
                ss[1] = Regex.Replace(ss[1], "\\[|\\]", string.Empty);

                var value = double.Parse(ss[1], CultureInfo.InvariantCulture);
                var name = GetVariableName(ss[0], isIndexWithParentesis);

                istruction = new SetVariableIstruction(name.Item1, name.Item2, value);
                istruction.LineNumber = lineNumber;
            }

            return istruction;
        }

        private static Tuple<string, int> GetVariableName(string data, bool isIndexWithParentesis)
        {
            var ss = Regex.Replace(data, "\\?|%|\\[|\\]", string.Empty);
            var name = Regex.Match(ss, "[A-Z]+").Value;
            var index = int.Parse(Regex.Match(ss, "\\d+(\\.\\d+)?").Value);

            return new Tuple<string, int>(name, index);
        }
    }
}
