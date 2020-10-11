using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoIstructions.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoParser
{
    class IsoTextParser
    {
        private List<BaseIstruction> _istructions = new List<BaseIstruction>();

        public List<BaseIstruction> Istructions => _istructions;

        public void ParseLine(string line, int lineNumber)
        {
            var commentRgx = new Regex("^\\s*;.*");

            if (commentRgx.Match(line).Success)
            {

            }
            else
            {
                // non è un commento
                if(line.Contains(";"))
                {
                    char[] separators = { ';' };
                    var data = line.Split(separators)[0];
                    ParseData(data, lineNumber);
                }
                else
                {
                    ParseData(line, lineNumber);
                }
            }
        }

        void ParseData(string data, int lineNumber)
        {
            var var1 = new Regex("\\?%[A-Z]+\\d+=\\d+(\\.\\d+)?");
            var var2 = new Regex("\\?%[A-Z]+\\[\\d+\\]=\\d+(\\.\\d+)?");
            var var3 = new Regex("\\?%[A-Z]+\\[\\d+\\]=\\[\\d+(\\.\\d+)?\\]");
            //var mc = new Regex("(M\\d+)+\\s?");
            var mc = new Regex("^((((M|S){1})(\\d+))+)");
            var g = new Regex("G\\d+(\\s+\\w\\d+(\\.\\d+)?)*");

            // varibili senza parentesi quadre
            if (var1.Match(data).Success)
            {
                _istructions.Add(SetVariableIstructionFactory.Create(data, false, lineNumber));
            }
            // variabili con parentesi quadre
            else if(var2.Match(data).Success)
            {
                _istructions.Add(SetVariableIstructionFactory.Create(data, true, lineNumber));
            }
            // variabili con parentesi quadre e valore con parentesi quadre
            else if (var3.Match(data).Success)
            {
                _istructions.Add(SetVariableIstructionFactory.Create(data, true, lineNumber));
            }
            // istruzioni M/S
            else if(mc.Match(data).Success)
            {
                var ii = MachIstructionFactory.Create(data, lineNumber);

                if(ii.Count() == 1)
                {
                    _istructions.Add(ii.First());
                }
                else if(ii.Count() > 1)
                {
                    _istructions.Add(new MultipleMachIstructions() { Istructions = ii.Select(o => o as MachIstruction), LineNumber = lineNumber });
                }

            }
            // istruzioni G
            else if(g.IsMatch(data))
            {
                //var i = GIstructionFactory.Create(g.Match(data).Value);
                var i = GIstructionFactory.Create(data, lineNumber);
                _istructions.Add(i);
            }

        }
    }
}
