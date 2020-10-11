using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class StateViewModel : ViewModelBase
    {
        public ObservableCollection<MatrixLevelViewModel> MatrixLevels { get; set; } = new ObservableCollection<MatrixLevelViewModel>();

        public StateViewModel()
        {
            MessengerInstance.Register<NullIsoLineSelectionMessage>(this, OnNullIsoLineSelectionMessage);
            MessengerInstance.Register<SetMatrixLevelMessage>(this, OnSetMatrixLevelMessage);
            MessengerInstance.Register<ShiftValueChangedMessage>(this, OnShiftValueChangedMessage);
            MessengerInstance.Register<IsoLineSelectionChangedMessage>(this, OnIsoLineSelectionChangedMessage);
            MessengerInstance.Register<GetActiveLevelOffsetMessage>(this, OnGetActiveLevelOffsetMessage);
        }

        private void OnShiftValueChangedMessage(ShiftValueChangedMessage msg)
        {
            var vm = MatrixLevels.FirstOrDefault((m) => m.Active);

            if(vm != null)
            {
                switch (msg.Direction)
                {
                    case Enums.ShiftDirection.X:
                        vm.ShiftX = msg.Value;
                        break;
                    case Enums.ShiftDirection.Y:
                        vm.ShiftY = msg.Value;
                        break;
                    case Enums.ShiftDirection.Z:
                        vm.ShiftZ = msg.Value;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                throw new InvalidOperationException("No active MLV");
            }
        }

        private void OnSetMatrixLevelMessage(SetMatrixLevelMessage msg)
        {
            foreach (var item in MatrixLevels) item.Active = false;

            var vm = MatrixLevels.FirstOrDefault((m) => m.Level == msg.Level);

            if(vm != null)
            {
                vm.Active = true;
            }
            else
            {
                MatrixLevels.Add(new MatrixLevelViewModel() { Active = true, Level = msg.Level });
            }
        }

        private void OnNullIsoLineSelectionMessage(NullIsoLineSelectionMessage msg)
        {
            MatrixLevels.Clear();
        }

        private void OnIsoLineSelectionChangedMessage(IsoLineSelectionChangedMessage msg)
        {
            MatrixLevels.Clear();
        }

        private void OnGetActiveLevelOffsetMessage(GetActiveLevelOffsetMessage msg)
        {
            var list = MatrixLevels.OrderBy(o => o.Level).ToList();
            double x = 0.0;
            double y = 0.0;
            double z = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                x += list[i].ShiftX;
                y += list[i].ShiftY;
                z += list[i].ShiftZ;

                if (list[i].Active) break;
            }

            msg.AddOffset?.Invoke(x, y, z);
        }
    }
}
