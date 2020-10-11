using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;
using System;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoInterpreter.Converters
{
    public class SIstructionConverter : MachIstructionConverter<State>
    {
        public override List<MachineStep> Convert(BaseIstruction istruction, State state)
        {
            return Convert(istruction as SIstruction, state);
        }

        public virtual List<MachineStep> Convert(SIstruction istruction, State state)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(SIstruction).Name}");

            bool updateRotationSpeed = state.RotationSpeed > 0;
            int lastRotationSpeed = state.RotationSpeed;

            state.RotationSpeed = istruction.Value;

            if (updateRotationSpeed)
            {
                double duration = (lastRotationSpeed != state.RotationSpeed) ? 0.5 : 0.0;

                return new List<MachineStep>()
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "Update rotation speed",
                        Actions = new List<BaseAction>()
                        {
                            new UpdateRotationSpeedAction()
                            {
                                NewRotationSpeed = state.RotationSpeed,
                                OldRotationSpeed = lastRotationSpeed,
                                Duration = duration
                            }
                        }
                    }
                };
            }
            else
            {
                return null;
            }
        }
    }

}
