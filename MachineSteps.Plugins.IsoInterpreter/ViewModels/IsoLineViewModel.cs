using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Extensions;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Models;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class IsoLineViewModel : ViewModelBase
    {
        private IsoLine _isoLine;

        public int Number => _isoLine.Number;

        public string Data => _isoLine.Data;

        private bool _executable = true;

        public bool Executable
        {
            get => _executable; 
            set => Set(ref _executable, value, nameof(Executable)); 
        }

        public IsoLineViewModel(IsoLine isoLine)
        {
            _isoLine = isoLine;

            MessengerInstance.Register<InibitIsoLineMessage>(this, OnInibitIsoLineMessage);
            MessengerInstance.Register<InvalidateIsoLineInibitionMessage>(this, OnInvalidateIsoLineInibitionMessage);
        }

        private void OnInvalidateIsoLineInibitionMessage(InvalidateIsoLineInibitionMessage msg)
        {
            if(!Executable) Executable = true;
        }

        private void OnInibitIsoLineMessage(InibitIsoLineMessage msg)
        {
            if((_isoLine.Number >= msg.First) && (_isoLine.Number <= msg.Last))
            {
                Executable = false;
            }
        }

        public void Process()
        {
            if(_executable) _isoLine.Process();
        }

        public static implicit operator IsoLine(IsoLineViewModel vm) => vm._isoLine;
    }
}
