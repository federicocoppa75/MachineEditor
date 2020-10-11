using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineViewer.Messages.Probing;
using MachineViewer.ViewModels.Probing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels
{
    public class ProbesViewModel : ViewModelBase
    {
        public ObservableCollection<ProbeViewModel> Probes { get; set; } = new ObservableCollection<ProbeViewModel>();

        public ProbesViewModel() : base()
        {
            Messenger.Default.Register<AddProbeMessage>(this, OnAddProbeMessage);
            Messenger.Default.Register<AddPointDistanceProbeMessage>(this, OnAddPointDistanceProbeMessage);
            Messenger.Default.Register<RemoveSelectedProbeMessage>(this, OnRemoveSelectedProbeMessage);
            Messenger.Default.Register<CanExecuteAddPointDistanceMessage>(this, OnCanExecuteAddPointDistanceMessage);
            Messenger.Default.Register<CanExecuteRemoveProbeMessage>(this, OnCanExecuteRemoveProbeMessage);
        }

        private void OnCanExecuteAddPointDistanceMessage(CanExecuteAddPointDistanceMessage msg)
        {
            var result = false;
            var pbrs = GetSelected();

            if(pbrs.Count() == 2)
            {
                result = pbrs.All((p) => p is PointProbeViewModel);
            }

            msg.SetValue(result);
        }

        private void OnCanExecuteRemoveProbeMessage(CanExecuteRemoveProbeMessage msg) => msg.SetValue(GetSelected().Count() > 0);

        private void OnRemoveSelectedProbeMessage(RemoveSelectedProbeMessage msg)
        {
            var prbs = GetSelected().ToList();

            foreach (var item in prbs)
            {
                item.DetachFromParent();
                Probes.Remove(item);
            }
        }

        private void OnAddPointDistanceProbeMessage(AddPointDistanceProbeMessage msg)
        {
            var pts = GetSelected().Select((p) => p as PointProbeViewModel).ToArray();
            var vm = PointsDistanceViewModel.Create(pts[0], pts[1]);

            pts[0].Children.Add(vm);

            Probes.Add(vm);
        }

        private void OnAddProbeMessage(AddProbeMessage msg) => Probes.Add(msg.Probe);

        private IEnumerable<ProbeViewModel> GetSelected() => Probes.Where((p) => p.IsSelected);
    }
}
