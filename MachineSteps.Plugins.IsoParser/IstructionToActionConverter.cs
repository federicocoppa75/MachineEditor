using MachineSteps.Models;
using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Converters;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser
{
    class IstructionToActionConverter
    {
        private List<MachineStep> _machineSteps;

        public static MachineStepsDocument Convert(List<BaseIstruction> Istructions)
        {
            var converter = new IstructionToActionConverter();
            MachineStepsDocument msd = null;

            BaseIstructionConverter.ResetStepId();

            if (converter.ConvertImplementation(Istructions))
            {
                msd = new MachineStepsDocument() { Steps = converter._machineSteps };
            }

            return msd;
        }

        private bool ConvertImplementation(List<BaseIstruction> Istructions)
        {
            bool result = true;
            var cm = new ConvertersManager();

            foreach (var item in Istructions)
            {
                try
                {
                    var mss = cm.Convert(item);

                    if (mss != null && mss.Count > 0)
                    {
                        if (_machineSteps == null) _machineSteps = new List<MachineStep>();

                        _machineSteps.AddRange(mss);
                    }
                }
                catch (System.Exception e)
                {
                    if(e is System.NotImplementedException)
                    {
                        throw new System.NotImplementedException($"Line {item.LineNumber}", e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return result;
        }
    }
}
