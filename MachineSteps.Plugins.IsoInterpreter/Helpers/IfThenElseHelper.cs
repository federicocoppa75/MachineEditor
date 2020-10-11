using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Models;

namespace MachineSteps.Plugins.IsoInterpreter.Helpers
{
    public class IfThenElseHelper
    {
        public static void EvaluateIfThenLine(IfThenLine line)
        {
            if(EvaluateCondition(line.Condition, out bool result))
            {
                if(line.ElseLine > 0)
                {
                    int first = result ? line.ElseLine + 1 : line.Number + 1;
                    int last = result ? line.EndLine - 1 : line.ElseLine - 1;

                    Messenger.Default.Send(new InibitIsoLineMessage() { First = first, Last = last });
                }
                else
                {
                    if(!result)
                    {
                        Messenger.Default.Send(new InibitIsoLineMessage() { First = line.Number + 1, Last = line.EndLine - 1 });
                    }
                }
            }
            //else
            //{
            //    throw new ArgumentException($"The condition \"{line.Condition}\" could not be evaluated!");
            //}
        }

        private static bool EvaluateCondition(string condition, out bool result)
        {
            bool retval = false;
            object obj = null;

            result = false;

            if(ExpressionHelper.TryToEvaluateExpression(condition, o => obj = o))
            {
                if(obj is bool b)
                {
                    result = b;
                    retval = true;
                }
            }

            return retval;
        }
    }
}
