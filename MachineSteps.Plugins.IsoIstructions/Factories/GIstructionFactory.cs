using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;


namespace MachineSteps.Plugins.IsoIstructions.Factories
{
    static public class GIstructionFactory
    {
        public static BaseIstruction Create(string data, int lineNumber)
        {
            BaseIstruction istruction = null;
            var indexRegEx = new Regex("G\\d+");

            if(indexRegEx.IsMatch(data))
            {
                var matchStr = indexRegEx.Match(data).Value;
                var sIndex = matchStr.Remove(0, 1);

                if(int.TryParse(sIndex, out int index))
                {
                    var parameters = ParseParameters(data.Replace(matchStr, string.Empty));
                    istruction = new GIstruction(index, parameters);
                    istruction.LineNumber = lineNumber;
                }
            }

            return istruction;
        }

        private static Dictionary<char, double> ParseParameters(string data)
        {
            Dictionary<char, double> parameters = null;
            var parsRegEx = new Regex("([A-Z])(\\S+)");

            foreach (var item in parsRegEx.Matches(data))
            {
                var m = item as Match;
                var p = ParseParameter(m.Value);

                if(p != null)
                {
                    if (parameters == null) parameters = new Dictionary<char, double>();

                    parameters.Add(p.Item1, p.Item2);
                }
            } 

            return parameters;
        }

        private static Tuple<char, double> ParseParameter(string data)
        {
            string val1Ex = "\\-?\\d+(\\.\\d+)?";
            string val2Ex = $"\\({val1Ex}\\)";
            string val3Ex = $"\\(\\S+\\)";
            var nameRegEx = new Regex("([A-Z])");
            var value1RegEx = new Regex(val1Ex);
            var value2RegEx = new Regex(val2Ex);
            var numExpression = new Regex($"\\(({val1Ex}|{val2Ex})(\\+({val1Ex}|{val2Ex}|{val3Ex}))+\\)");
            var name = nameRegEx.Match(data).Value[0];


            if(numExpression.IsMatch(data))
            {
                var expression = numExpression.Match(data).Value;
                var expressionElementRegEx = new Regex("\\+");
                var expElements = expressionElementRegEx.Split(expression);
                double result = 0.0;

                foreach (var item in expElements)
                {
                    if (value2RegEx.IsMatch(item))
                    {
                        result += double.Parse(value1RegEx.Match(item).Value, CultureInfo.InvariantCulture);
                    }
                }

                return new Tuple<char, double>(name, result);
            }
            else if (value2RegEx.IsMatch(data))
            {
                return new Tuple<char, double>(name, double.Parse(value1RegEx.Match(data).Value, CultureInfo.InvariantCulture));
            }
            else if (value1RegEx.IsMatch(data))
            {
                return new Tuple<char, double>(name, double.Parse(value1RegEx.Match(data).Value, CultureInfo.InvariantCulture));
            }
            else
            {
                return null;
            }

        }
    }
}
