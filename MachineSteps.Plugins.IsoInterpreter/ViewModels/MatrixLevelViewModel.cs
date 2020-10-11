using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class MatrixLevelViewModel : ViewModelBase
    {
        public int Level { get; set; }

        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                if(Set(ref _active, value, nameof(Active))) UpdateStep();
            }
        }

        private double _shiftX;
        public double ShiftX
        {
            get => _shiftX;
            set
            {
                if(Set(ref _shiftX, value, nameof(ShiftX))) UpdateStep();
            }
        }

        private double _shiftY;
        public double ShiftY
        {
            get => _shiftY;
            set
            {
                if(Set(ref _shiftY, value, nameof(ShiftY))) UpdateStep();
            }
        }


        private double _shiftZ;
        public double ShiftZ
        {
            get => _shiftZ;
            set
            {
                if(Set(ref _shiftZ, value, nameof(ShiftZ))) UpdateStep();
            }
        }

        private int _step;
        public int Step
        {
            get => _step;
            set => Set(ref _step, value, nameof(Step));
        }

        private void UpdateStep() => MessengerInstance.Send(new GetSelectedStepMessage() { SetStep = (s) => Step = s.Number });
       
    }
}
