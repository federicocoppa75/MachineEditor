using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(690)]
    public class G690Converter : GIstructionConverter<State>
    {
        const double _injectDuration = 0.5;

        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            if (istruction.Parameters.TryGetValue('L', out double ln) && istruction.Parameters.TryGetValue('T', out double t))
            {
                MachineStep ms = null;

                if(t == 1.0)
                {
                    ms = GetMachineStepForBlowing(ln);
                }
                else if(t == 2.0)
                {
                    ms = GetMachineStepForGlue(ln);
                }
                else if(t == 3.0)
                {
                    //ms = GetMachineStepForDoweling(ln);
                    return GetMachineStepsForDoweling(ln);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid doweling operation!");
                }

                return (ms != null) && (ms.Actions.Count > 0) ? new List<MachineStep>() { ms } : null;
            }
            else
            {
                return null;
            }
        }

        private MachineStep GetMachineStepForBlowing(double line)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "G690 - Blowing", Actions = new List<BaseAction>() };

            if(line == 1)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6074, Duration = _injectDuration });
            }
            else if(line == 2)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6075, Duration = _injectDuration });
            }
            else if(line == 3)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 7074, Duration = _injectDuration });
            }
            else if(line == 4)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 7075, Duration = _injectDuration });
            }
            else if(line == 13)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6074, Duration = _injectDuration });
                step.Actions.Add(new InjectAction() { InjectorId = 7074, Duration = _injectDuration });
            }
            else if(line == 24.0)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6075, Duration = _injectDuration });
                step.Actions.Add(new InjectAction() { InjectorId = 7075, Duration = _injectDuration });
            }
            else
            {
                throw new ArgumentException("Invalid blowing line id!");
            }

            return step;
        }

        private MachineStep GetMachineStepForGlue(double line)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "G690 - Glue", Actions = new List<BaseAction>() };

            if (line == 1)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6076, Duration = _injectDuration });
            }
            else if (line == 2)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6077, Duration = _injectDuration });
            }
            else if (line == 3)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 7076, Duration = _injectDuration });
            }
            else if (line == 4)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 7077, Duration = _injectDuration });
            }
            else if (line == 13)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6076, Duration = _injectDuration });
                step.Actions.Add(new InjectAction() { InjectorId = 7076, Duration = _injectDuration });
            }
            else if (line == 24.0)
            {
                step.Actions.Add(new InjectAction() { InjectorId = 6077, Duration = _injectDuration });
                step.Actions.Add(new InjectAction() { InjectorId = 7077, Duration = _injectDuration });
            }
            else
            {
                throw new ArgumentException("Invalid glue line id!");
            }

            return step;
        }

        private MachineStep GetMachineStepForDoweling(double line)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "G690 - Doweling", Actions = new List<BaseAction>() };

            if (line == 1)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else if (line == 2)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else if (line == 3)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else if (line == 4)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else if (line == 13)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11052, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11001, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else if (line == 24.0)
            {
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.On });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11053, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 10002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
                step.Actions.Add(new TwoPositionLinkAction() { LinkId = 11002, RequestedState = Models.Enums.TwoPositionLinkActionRequestedState.Off });
            }
            else
            {
                throw new ArgumentException("Invalid doweling line id!");
            }

            return step;
        }

        private List<MachineStep> GetMachineStepsForDoweling(double line)
        {
            var machineSteps = new List<MachineStep>();

            if (line == 1)
            {
                machineSteps.Add(GetMachineStepAction(10001, Models.Enums.TwoPositionLinkActionRequestedState.On , 1));
                machineSteps.Add(GetMachineStepAction(10052, Models.Enums.TwoPositionLinkActionRequestedState.On , 2));
                machineSteps.Add(GetMachineStepAction(10052, Models.Enums.TwoPositionLinkActionRequestedState.Off, 3));
                machineSteps.Add(GetMachineStepAction(10001, Models.Enums.TwoPositionLinkActionRequestedState.Off, 4));
            }
            else if (line == 2)
            {
                machineSteps.Add(GetMachineStepAction(10002, Models.Enums.TwoPositionLinkActionRequestedState.On, 1));
                machineSteps.Add(GetMachineStepAction(10053, Models.Enums.TwoPositionLinkActionRequestedState.On, 2));
                machineSteps.Add(GetMachineStepAction(10053, Models.Enums.TwoPositionLinkActionRequestedState.Off, 3));
                machineSteps.Add(GetMachineStepAction(10002, Models.Enums.TwoPositionLinkActionRequestedState.Off, 4));
            }
            else if (line == 3)
            {
                machineSteps.Add(GetMachineStepAction(11001, Models.Enums.TwoPositionLinkActionRequestedState.On, 1));
                machineSteps.Add(GetMachineStepAction(11052, Models.Enums.TwoPositionLinkActionRequestedState.On, 2));
                machineSteps.Add(GetMachineStepAction(11052, Models.Enums.TwoPositionLinkActionRequestedState.Off, 3));
                machineSteps.Add(GetMachineStepAction(11001, Models.Enums.TwoPositionLinkActionRequestedState.Off, 4));
            }
            else if (line == 4)
            {
                machineSteps.Add(GetMachineStepAction(11002, Models.Enums.TwoPositionLinkActionRequestedState.On, 1));
                machineSteps.Add(GetMachineStepAction(11053, Models.Enums.TwoPositionLinkActionRequestedState.On, 2));
                machineSteps.Add(GetMachineStepAction(11053, Models.Enums.TwoPositionLinkActionRequestedState.Off, 3));
                machineSteps.Add(GetMachineStepAction(11002, Models.Enums.TwoPositionLinkActionRequestedState.Off, 4));
            }

            else if (line == 13)
            {
                machineSteps.Add(GetMachineStepAction(10001, Models.Enums.TwoPositionLinkActionRequestedState.On , 1));
                machineSteps.Add(GetMachineStepAction(11001, Models.Enums.TwoPositionLinkActionRequestedState.On , 2));
                machineSteps.Add(GetMachineStepAction(10052, Models.Enums.TwoPositionLinkActionRequestedState.On , 3));
                machineSteps.Add(GetMachineStepAction(11052, Models.Enums.TwoPositionLinkActionRequestedState.On , 4));
                machineSteps.Add(GetMachineStepAction(10052, Models.Enums.TwoPositionLinkActionRequestedState.Off, 5));
                machineSteps.Add(GetMachineStepAction(11052, Models.Enums.TwoPositionLinkActionRequestedState.Off, 6));
                machineSteps.Add(GetMachineStepAction(10001, Models.Enums.TwoPositionLinkActionRequestedState.Off, 7));
                machineSteps.Add(GetMachineStepAction(11001, Models.Enums.TwoPositionLinkActionRequestedState.Off, 8));
            }
            else if (line == 24)
            {
                machineSteps.Add(GetMachineStepAction(10002, Models.Enums.TwoPositionLinkActionRequestedState.On, 1));
                machineSteps.Add(GetMachineStepAction(11002, Models.Enums.TwoPositionLinkActionRequestedState.On, 2));
                machineSteps.Add(GetMachineStepAction(10053, Models.Enums.TwoPositionLinkActionRequestedState.On, 3));
                machineSteps.Add(GetMachineStepAction(11053, Models.Enums.TwoPositionLinkActionRequestedState.On, 4));
                machineSteps.Add(GetMachineStepAction(10053, Models.Enums.TwoPositionLinkActionRequestedState.Off, 5));
                machineSteps.Add(GetMachineStepAction(11053, Models.Enums.TwoPositionLinkActionRequestedState.Off, 6));
                machineSteps.Add(GetMachineStepAction(10002, Models.Enums.TwoPositionLinkActionRequestedState.Off, 7));
                machineSteps.Add(GetMachineStepAction(11002, Models.Enums.TwoPositionLinkActionRequestedState.Off, 8));
            }
            else
            {
                throw new ArgumentException("Invalid doweling line id!");
            }

            return machineSteps;
        }

        private MachineStep GetMachineStepAction(int linkId, Models.Enums.TwoPositionLinkActionRequestedState requestedState, int phaseId)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = $"G690 - Doweling ({phaseId})", Actions = new List<BaseAction>() };

            step.Actions.Add(new TwoPositionLinkAction() { LinkId = linkId, RequestedState = requestedState });

            return step;
        }
    }
}
