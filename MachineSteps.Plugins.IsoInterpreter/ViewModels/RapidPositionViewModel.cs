using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Messages.Conversion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class RapidPositionViewModel : ViewModelBase
    {
        private bool _notifyChange = false;

        public ObservableCollection<RapidPositionCoordinateViewModel> Coordinates { get; set; } = new ObservableCollection<RapidPositionCoordinateViewModel>();

        public RapidPositionViewModel()
        {
            MessengerInstance.Register<G0LineMessage>(this, OnG0LineMessage);
            MessengerInstance.Register<G1LineMessage>(this, OnG1LineMessage);
            MessengerInstance.Register<G2ArcMessage>(this, OnG2ArcMessage);
            MessengerInstance.Register<G3ArcMessage>(this, OnG3ArcMessage);
            MessengerInstance.Register<IsoLineSelectionChangedMessage>(this, OnIsoLineSelectionChangedMessage);
            MessengerInstance.Register<IstructionListenerSwitchOnMessage>(this, (m) => _notifyChange = true);
            MessengerInstance.Register<IstructionListenerSwitchOffMessage>(this, (m) => _notifyChange = false);
            //MessengerInstance.Register<G90Message>(this, (m) => IsIncremental = false);
            //MessengerInstance.Register<G91Message>(this, (m) => IsIncremental = true);
            MessengerInstance.Register<G210LineMessage>(this, OnG210LineMessage);
        }

        private void OnG210LineMessage(G210LineMessage msg) => OnGElementMessage(msg);

        private void OnIsoLineSelectionChangedMessage(IsoLineSelectionChangedMessage msg)
        {
            Coordinates.Clear();
        }

        private void OnG0LineMessage(G0LineMessage msg) => OnGElementMessage(msg);

        private void OnG1LineMessage(G1LineMessage msg) => OnGElementMessage(msg/*, (s) => string.Compare(s, "F") == 0*/);

        private void OnG3ArcMessage(G3ArcMessage msg) => OnGElementMessage(msg/*, (s) => string.Compare(s, "F") == 0*/);

        private void OnG2ArcMessage(G2ArcMessage msg) => OnGElementMessage(msg/*, (s) => string.Compare(s, "F") == 0*/);

        private void OnGElementMessage(GElementMessage msg, Func<string, bool> filterFunc = null)
        {
            var updatedVms = new List<RapidPositionCoordinateViewModel>();

            foreach (var item in msg.Coordinates)
            {
                if ((filterFunc != null) && filterFunc(item.Key)) continue;

                var vm = Coordinates.FirstOrDefault(o => string.Compare(o.Name, item.Key) == 0);

                if (vm != null)
                {
                    if (msg.IsIncremental)
                    {
                        vm.Add(item.Value);
                    }
                    else
                    {
                        vm.Value = item.Value;
                    }
                }
                else
                {
                    vm = new RapidPositionCoordinateViewModel() { Name = item.Key, Value = item.Value };
                    Coordinates.Add(vm);
                }

                if (_notifyChange) updatedVms.Add(vm);
            }

            if(_notifyChange && updatedVms.Count > 0)
            {
                MessengerInstance.Send(new GIstructionMessage()
                {
                    Istruction = msg.Type,
                    Step = updatedVms[0].Step,
                    Coordinates = updatedVms.ToDictionary(r => r.Name, r => double.Parse(r.ValueOnLevel, NumberStyles.Any, CultureInfo.InvariantCulture))
                });
            }

        }

    }
}
