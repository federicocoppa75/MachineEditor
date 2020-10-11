using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoConverterBase
{
    public abstract class SetBitArrayIstructionConverter<T> : SetVariableIstructionConverter<T> where T : class
    {
        protected int[] _linkIds = new int[32];

        public SetBitArrayIstructionConverter() : base()
        {
            InitLinkIds();
        }

        public override List<MachineStep> Convert(SetVariableIstruction istruction, T state)
        {
            var bytes = BitConverter.GetBytes((uint)istruction.Value);
            var ar = new BitArray(bytes);
            var s = GetState(state);
            var r = new BitArray(ar).Xor(s);
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = $"{istruction.Name}[{istruction.Index}]",
                Actions = new List<BaseAction>()
            };

            for (int i = 0; i < r.Length; i++)
            {
                if((_linkIds[i] > 0) && r[i])
                {
                    AddAction(step, ar[i], _linkIds[i]);
                    s[i] = ar[i];
                }
            }

            FilterMultipleActionOnSameLink(step);
            SetActionsIds(step);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }

        protected abstract void InitLinkIds();
 
        protected abstract BitArray GetState(T state);

        protected virtual void AddAction(MachineStep step, bool rqState, int linkId)
        {
            step.Actions.Add(new TwoPositionLinkAction()
            {
                LinkId = linkId,
                RequestedState = rqState ? TwoPositionLinkActionRequestedState.On : TwoPositionLinkActionRequestedState.Off
            });
        }

        private static void FilterMultipleActionOnSameLink(MachineStep step)
        {
            var itemsToRemove = new List<BaseAction>();

            for (int i = 0; i < step.Actions.Count(); i++)
            {
                var ai = step.Actions[i] as TwoPositionLinkAction;

                for (int j = i+1; j < step.Actions.Count(); j++)
                {
                    var aj = step.Actions[j] as TwoPositionLinkAction;

                    if((ai.LinkId == aj.LinkId))
                    {
                        if(ai.RequestedState == aj.RequestedState)
                        {
                            itemsToRemove.Add(aj);
                        }
                        else
                        {
                            itemsToRemove.Add(ai);
                            itemsToRemove.Add(aj);
                        }
                    }
                }
            }

            foreach (var item in itemsToRemove)
            {
                step.Actions.Remove(item);
            }
        }
    }
}
