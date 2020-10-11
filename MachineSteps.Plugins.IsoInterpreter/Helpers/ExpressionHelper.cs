using GalaSoft.MvvmLight.Messaging;
using Jint;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Helpers
{
    public static class ExpressionHelper
    {
        public static string VlVariableStringMatch => "VL[0-9]+";

        public static string ExkVariableStringMatch => "(ETK|EOK|EDK)\\[[0-9]+\\]";

        public static string AxisParameterStringMatch => "ax\\[[0-9]+\\]\\.pa\\[[0-9]+\\]";

        public static string AxisActualValue => "QCALC\\([X|Y|Z]\\)";

        public static bool GetComputeValueAsString(object value, out string computedValue)
        {
            bool result = false;

            computedValue = string.Empty;

            if (value is double d)
            {
                result = true;
                computedValue = d.ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }
        
        public static bool EvaluateExpression(string expression, out object computedValue)
        {
            bool result = false;
            var engine = new Engine();

            computedValue = string.Empty;

            try
            {
                var expressionResult = engine.Execute(expression)
                                             .GetCompletionValue()
                                             .ToObject();

                computedValue = expressionResult;

                //if (expressionResult is double d)
                //{
                //    result = true;
                //    computedValue = d.ToString(CultureInfo.InvariantCulture);
                //}

                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public static bool IsComputedValue(string str) => string.IsNullOrEmpty(str) ? false : Regex.IsMatch(str, "\\A-?[0-9]+(.[0-9]+)?");

        private static string CheckForQCALC(string expression)
        {
            var matches = Regex.Matches(expression, ExpressionHelper.AxisActualValue);

            for (int i = 0; i < matches.Count; i++)
            {
                bool ready = false;
                double value = 0.0;
                var axName = Regex.Match(matches[i].Value, "X|Y|Z").Value;

                if (!string.IsNullOrEmpty(axName))
                {
                    Messenger.Default.Send(new GetAxisValueMessage()
                    {
                        AxisName = axName,
                        SetReady = (b) => ready = b,
                        SetValue = (v) => value = v
                    });

                    if (ready)
                    {
                        var str = matches[i].Value;
                        expression.Replace(str, value.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }

            return expression;
        }

        public static bool TryToEvaluateExpression(string value, Action<object> computedValueSetter)
        {
            return TryToEvaluateExpression(string.Empty, value, string.Empty, computedValueSetter);
        }

        public static bool TryToEvaluateExpression(string name, string value, string computedValue, Action<object> computedValueSetter)
        {
            var matches = Regex.Matches(value, $"({VlVariableStringMatch})|({ExkVariableStringMatch})|({AxisParameterStringMatch})|({AxisActualValue})");
            var varValues = new Dictionary<string, double>();
            var varReadys = new Dictionary<string, bool>();
            bool result = false;

            for (int i = 0; i < matches.Count; i++)
            {
                var vName = matches[i].Value;
                double vValue = 0.0;
                bool vReady = false;
                bool vFound = false;

                if (!string.IsNullOrEmpty(name) && (string.Compare(vName, name) == 0))
                {
                    if (!string.IsNullOrEmpty(computedValue) && IsComputedValue(computedValue))
                    {
                        vReady = double.TryParse(computedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out vValue);
                    }
                    else
                    {
                        vValue = 0.0;
                        vReady = true;
                    }
                }
                else if (Regex.IsMatch(vName, AxisActualValue))
                {
                    var a = Regex.Match(vName, "X|Y|Z").Value;

                    Messenger.Default.Send(new GetAxisValueMessage()
                    {
                        AxisName = a,
                        SetValue = (d) => vValue = d,
                        SetReady = (b) => vReady = b,
                        SetFinded = () => vFound = true
                    });
                }
                else
                {
                    Messenger.Default.Send(new GetVariableValueMessage()
                    {
                        Name = vName,
                        SetValue = (d) => vValue = d,
                        SetReady = (b) => vReady = b,
                        SetFinded = () => vFound = true
                    });
                }

                varValues[vName] = vValue;
                varReadys[vName] = vReady;
                if (!vFound) NotifyNotFoundParameter(vName);
            }

            if (varReadys.Values.All(b => b))
            {
                var expression = value;

                foreach (var item in varValues)
                {
                    expression = expression.Replace(item.Key, item.Value.ToString());
                }

                expression = expression.Replace("%", "");
                expression = CheckForQCALC(expression);
                expression = expression.Replace("+-", "-");
                expression = expression.Replace("--", "+");
                expression = expression.Replace(",", ".");
                expression = expression.Replace("ABS", "Math.abs");

                if (EvaluateExpression(expression, computedValueSetter))
                {

                    result = true;
                }
            }

            return result;
        }

        //private static bool EvaluateExpression(string expression, Action<string> computedValueSetter)
        //{
        //    bool result = EvaluateExpression(expression, out string computedValue);

        //    if (result) computedValueSetter(computedValue);

        //    return result;
        //}

        private static bool EvaluateExpression(string expression, Action<object> computedValueSetter)
        {
            bool result = EvaluateExpression(expression, out object computedValue);

            if (result) computedValueSetter(computedValue);

            return result;
        }

        private static void NotifyNotFoundParameter(string name) => Messenger.Default.Send(new NotifyNotFoundParameterMessage() { Name = name });

    }
}
