using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoIstructions.Factories
{
    public class MachIstructionFactory
    {
        public static IEnumerable<BaseIstruction> Create(string data, int lineNumber)
        {
            List<BaseIstruction> istructions = new List<BaseIstruction>();
            var indexRegEx = new Regex("(M|S)\\d+");
            var matches = indexRegEx.Matches(data);

            foreach (var item in matches)
            {
                var m = item as Match;
                var sIndex = m.Value.Remove(0, 1);

                if (int.TryParse(sIndex, out int index))
                {
                    switch (m.Value[0])
                    {
                        case 'M':
                            istructions.Add(new MIstruction(index) { LineNumber = lineNumber});
                        break;

                        case 'S':
                            istructions.Add(new SIstruction(index) { LineNumber = lineNumber });
                            break;

                        default:
                            break;
                    }
                }
            }

            return istructions;
        }
    }
}
