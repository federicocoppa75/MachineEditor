using MachineSteps.Models.Steps;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoConverterBase
{
    public abstract class BaseIstructionConverter
    {
        static int _stepId;

        static protected int GetStepId() => _stepId++;

        static protected void SetActionsIds(MachineStep step)
        {
            for (int i = 0; i < step.Actions.Count; i++)
            {
                step.Actions[i].Id = i;
            }
        }

        public static void ResetStepId() => _stepId = 0;
    }

    public abstract class BaseIstructionConverter<T> : BaseIstructionConverter where T : class
    {
        public abstract List<MachineStep> Convert(BaseIstruction istruction, T state);        
    }
}
