using System;
using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Converters.StateData;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(106)]
    public class G106Converter : GIstructionConverter<State>
    {
        const double _gantryToolchangeDistance = 224.0;
        const double _zQuoteToolEngage = -56.3;

        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            var steps = new List<MachineStep>();

            for (int i = 0; i < state.ToolChange.NumSteps; i++)
            {
                bool openToolStore = (i == 0);
                bool closeToolStore = (i == state.ToolChange.NumSteps - 1);

                steps.AddRange(CreateToolchangeStep(state, i, openToolStore, closeToolStore));
            }

            return (steps.Count() > 0) ? steps : null;
        }

        private List<MachineStep> CreateToolchangeStep(State state, int i, bool openToolStore, bool closeToolStore)
        {
            if (i >= state.ToolChange.ToolChangeSteps.Length) throw new IndexOutOfRangeException("The toolchange step index excedes the size of toolchange steps size!");

            var toolchangeStepData = state.ToolChange.ToolChangeSteps[i];

            if(toolchangeStepData.SinkPosition >= 1000)
            {
                return state.Axes.Is3AxesWithVirtuals ? CreateLoadToolStepSimple(state, toolchangeStepData, openToolStore, closeToolStore) : CreateLoadToolStep(state, toolchangeStepData, openToolStore, closeToolStore);
            }
            else
            {
                return state.Axes.Is3AxesWithVirtuals ? CreateUnloadToolStepSimple(state, toolchangeStepData, openToolStore, closeToolStore) : CreateUnloadToolStep(state, toolchangeStepData, openToolStore, closeToolStore);
            }
        }

        private List<MachineStep> CreateLoadToolStep(State state, ToolChange.ToolChangeStep toolchangeStepData, bool openToolStore, bool closeToolStore)
        {
            bool resetGantry = (state.Axes.GantryY == Gantry.Second) ||
                                ((state.Axes.GantryY == Gantry.First) && (Math.Abs(state.Axes.GantryStepY - _gantryToolchangeDistance) > 0.001));
            bool moveGantry = resetGantry || (state.Axes.GantryY == Gantry.None);
            bool setGantry = state.Axes.GantryY != Gantry.First;
            bool moveSynchro = (state.Axes.GantryY == Gantry.First) && (Math.Abs(state.Axes.GantryStepY - _gantryToolchangeDistance) <= 0.001);
            var steps = new List<MachineStep>();

            if (resetGantry) AddResetGantrySteps(state, steps);
            if (moveGantry) AddMoveGantrySteps(state, toolchangeStepData.SourcePosition, steps);
            if (setGantry) AddSetGantrySteps(state, steps);
            if (moveSynchro) AddMoveSynchroSteps(state, toolchangeStepData.SourcePosition, steps);
            if (openToolStore) AddOpenToolstore(state, steps);
            AddPneumaticDown(state, steps);
            AddMoveZDownStep(state, steps);
            AddLoadToolSteps(state, toolchangeStepData.SinkPosition, toolchangeStepData.SourcePosition, steps);
            if (closeToolStore) AddCloseToolstore(state, steps);
            AddMoveZUpStep(state, steps);
            AddPneumaticUp(state, steps);

            return (steps.Count() > 0) ? steps : null;
        }

        private List<MachineStep> CreateLoadToolStepSimple(State state, ToolChange.ToolChangeStep toolchangeStepData, bool openToolStore, bool closeToolStore)
        {
            var steps = new List<MachineStep>();

            AddLoadToolSteps(state, toolchangeStepData.SinkPosition, toolchangeStepData.SourcePosition, steps);

            return (steps.Count() > 0) ? steps : null;
        }

        private List<MachineStep> CreateUnloadToolStep(State state, ToolChange.ToolChangeStep toolchangeStepData, bool openToolStore, bool closeToolStore)
        {
            bool resetGantry = (state.Axes.GantryY == Gantry.Second) ||
                                ((state.Axes.GantryY == Gantry.First) && (state.Axes.GantryStepY != _gantryToolchangeDistance));
            bool moveGantry = resetGantry || (state.Axes.GantryY == Gantry.None);
            bool setGantry = state.Axes.GantryY != Gantry.First;
            bool moveSynchro = (state.Axes.GantryY == Gantry.First) && (state.Axes.GantryStepY == _gantryToolchangeDistance);
            var steps = new List<MachineStep>();

            if (resetGantry) AddResetGantrySteps(state, steps);
            if (moveGantry) AddMoveGantrySteps(state, toolchangeStepData.SinkPosition, steps);
            if (setGantry) AddSetGantrySteps(state, steps);
            if (moveSynchro) AddMoveSynchroSteps(state, toolchangeStepData.SinkPosition, steps);
            if (openToolStore) AddPneumaticDown(state, steps);
            AddMoveZDownStep(state, steps);
            AddOpenToolstore(state, steps);
            AddUnloadToolSteps(state, toolchangeStepData.SourcePosition, toolchangeStepData.SinkPosition, steps);
            AddMoveZUpStep(state, steps);
            if (closeToolStore) AddCloseToolstore(state, steps);
            AddPneumaticUp(state, steps);

            return (steps.Count() > 0) ? steps : null;
        }

        private List<MachineStep> CreateUnloadToolStepSimple(State state, ToolChange.ToolChangeStep toolchangeStepData, bool openToolStore, bool closeToolStore)
        {
            var steps = new List<MachineStep>();

            AddUnloadToolSteps(state, toolchangeStepData.SourcePosition, toolchangeStepData.SinkPosition, steps);

            return (steps.Count() > 0) ? steps : null;
        }

        private void AddResetGantrySteps(State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Reset previous",
                Actions = new List<BaseAction>()
            };

            state.Axes.ResetGantryY(step);
            steps.Add(step);
        }

        private void AddMoveGantrySteps(State state, int toolHolderId, List<MachineStep> steps)
        {
            double y = GetToolchangeQuote(toolHolderId);
            double v = y + _gantryToolchangeDistance;
            var step = new MachineStep() { Id = GetStepId(), Name = "TCH - Move", Actions = new List<BaseAction>() };

            state.Axes.SetYV(y, v, step, false);
            SetActionsIds(step);

            steps.Add(step);
        }

        private void AddSetGantrySteps(State state, List<MachineStep> steps)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "TCH - Gantry", Actions = new List<BaseAction>() };

            state.Axes.SetGantryY(1.0, step);
            steps.Add(step);
        }

        private void AddMoveSynchroSteps(State state, int toolHolderId, List<MachineStep> steps)
        {
            double y = GetToolchangeQuote(toolHolderId);
            var step = new MachineStep() { Id = GetStepId(), Name = "TCH - Move synchro", Actions = new List<BaseAction>() };

            state.Axes.SetY(y, step, false);
            SetActionsIds(step);

            steps.Add(step);
        }

        private double GetToolchangeQuote(int toolHolderId)
        {
            double y = 0.0;

            switch (toolHolderId)
            {
                case 1: y = 1312.0; break;
                case 2: y = 1181.0; break;
                case 3: y = 1050.0; break;
                case 4: y = 919.0; break;
                case 5: y = 788.0; break;
                case 6: y = 657.0; break;
                default:
                    throw new ArgumentOutOfRangeException($"The tool holder id {toolHolderId} is out of range!");
            }

            return y;
        }

        private void AddOpenToolstore(State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Open toolstore",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        Name = "Open toolstore",
                        LinkId = 81,
                        RequestedState = MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }

        private void AddCloseToolstore(State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Close toolstore",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        Name = "Close toolstore",
                        LinkId = 81,
                        RequestedState = MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.Off
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }

        private void AddPneumaticDown(State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Electrospindle down",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        Name = "Electrospindle down",
                        LinkId = 3001,
                        RequestedState = MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }

        private void AddPneumaticUp(State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Electrospindle Up",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        Name = "Electrospindle Up",
                        LinkId = 3001,
                        RequestedState = MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.Off
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }

        private void AddMoveZDownStep(State state, List<MachineStep> steps)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "TCH - Move Z down", Actions = new List<BaseAction>() };

            state.Axes.SetZ(_zQuoteToolEngage, step, false);
            SetActionsIds(step);

            steps.Add(step);
        }

        private void AddMoveZUpStep(State state, List<MachineStep> steps)
        {
            var step = new MachineStep() { Id = GetStepId(), Name = "TCH - Move Z up", Actions = new List<BaseAction>() };

            state.Axes.SetZ(0.0, step, false);
            SetActionsIds(step);

            steps.Add(step);
        }

        private void AddLoadToolSteps(State state, int spindleId, int toolHolderId, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Load tool",
                Actions = new List<BaseAction>()
                {
                    new LoadToolAction()
                    {
                        Name = "Load tool",
                        ToolSource = -(1000 + toolHolderId),
                        ToolSink = spindleId + 2001
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }

        private void AddUnloadToolSteps(State state, int spindleId, int toolHolderId, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "TCH - Unload tool",
                Actions = new List<BaseAction>()
                {
                    new UnloadToolAction()
                    {
                        Name = "Unload tool",
                        ToolSource = -(1000 + toolHolderId),
                        ToolSink = spindleId + 2001
                    }
                }
            };

            SetActionsIds(step);
            steps.Add(step);
        }
    }
}
