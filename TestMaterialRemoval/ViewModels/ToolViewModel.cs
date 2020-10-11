using GalaSoft.MvvmLight;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMaterialRemoval.Messages;

namespace TestMaterialRemoval.ViewModels
{
    public class ToolViewModel : ViewModelBase
    {
        private double _radius = 10.0;
        public double Radius
        {
            get => _radius;
            set
            {
                if (Set(ref _radius, value, nameof(Radius))) NotifyToolChanged();
            }
        }

        private double _length = 100.0;
        public double Length
        {
            get => _length;
            set
            {
                if (Set(ref _length, value, nameof(Length))) NotifyToolChanged();
            }
        }

        private ToolDirection _toolDirection;
        public ToolDirection ToolDirection
        {
            get => _toolDirection;
            set
            {
                if (Set(ref _toolDirection, value, nameof(ToolDirection))) NotifyToolChanged();
            }
        }

        private int _repetition = 1;
        public int Repetition
        {
            get => _repetition;
            set
            {
                if (Set(ref _repetition, value, nameof(Repetition))) NotifyToolChanged();
            }
        }

        private double _repetitioStepX = 32.0;
        public double RepetitionStepX
        {
            get => _repetitioStepX;
            set
            {
                if (Set(ref _repetitioStepX, value, nameof(RepetitionStepX))) NotifyToolChanged();
            }
        }

        private double _repetitionStepY = 32.0;
        public double RepetitionStepY
        {
            get => _repetitionStepY;
            set
            {
                if (Set(ref _repetitionStepY, value, nameof(RepetitionStepY))) NotifyToolChanged();
            }
        }

        private void NotifyToolChanged() => MessengerInstance.Send(new ToolDataChangedMessage());
    }
}
