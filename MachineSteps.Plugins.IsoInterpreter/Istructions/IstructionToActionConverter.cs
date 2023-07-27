using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoInterpreter.Converters;
using MachineSteps.Plugins.IsoInterpreter.Messages.Conversion;
using MachineSteps.Plugins.IsoIstructions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Istructions
{
    public class IstructionToActionConverter
    {
        private ConvertersManager _converterManager;

        public List<MachineStep> MachineSteps { get; private set; }

        public IstructionToActionConverter()
        {            
            Messenger.Default.Register<GIstructionMessage>(this, OnGIstructionMessage);
            Messenger.Default.Register<MIstructionMessage>(this, OnMIstructionMessage);
            Messenger.Default.Register<SIstructionMessage>(this, OnSIstructionMessage);
            Messenger.Default.Register<SetVariableMessage>(this, OnSetVariableMessage);

            _converterManager = new ConvertersManager();
            MachineSteps = new List<MachineStep>();
        }

        private void OnGIstructionMessage(GIstructionMessage msg)
        {
            var istruction = new GIstruction(msg.Istruction, msg.Coordinates.ToDictionary(c => c.Key[0], c => c.Value)) { LineNumber = msg.Step };
            var steps = _converterManager.Convert(istruction);

            if ((steps != null) && (steps.Count > 0)) MachineSteps.AddRange(steps);
        }

        private void OnMIstructionMessage(MIstructionMessage msg)
        {
            var steps = _converterManager.Convert(new MIstruction(msg.Value) { LineNumber = msg.Step });

            if ((steps != null) && (steps.Count > 0)) MachineSteps.AddRange(steps);
        }

        private void OnSIstructionMessage(SIstructionMessage msg)
        {
            var steps = _converterManager.Convert(new SIstruction(msg.Value) { LineNumber = msg.Step });

            if ((steps != null) && (steps.Count > 0)) MachineSteps.AddRange(steps);
        }

        private void OnSetVariableMessage(SetVariableMessage msg)
        {
            var vr = ParseVariableName(msg.Name);
            var value = double.Parse(msg.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            var istruction = new SetVariableIstruction(vr.Item1, vr.Item2, value) { LineNumber = msg.Step };
            var steps = _converterManager.Convert(istruction);

            if ((steps != null) && (steps.Count > 0)) MachineSteps.AddRange(steps);
        }


        public void StartListen() => Messenger.Default.Send(new IstructionListenerSwitchOnMessage());
        public void StopListen() => Messenger.Default.Send(new IstructionListenerSwitchOffMessage());

        private Tuple<string, int> ParseVariableName(string name)
        {
            var v = Regex.Match(name, "^([A-Z])+").Value;
            var i = int.Parse(Regex.Match(name, "([0-9])+").Value);

            return new Tuple<string, int>(v, i);
        }

    }
}
