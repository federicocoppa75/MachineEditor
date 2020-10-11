using GalaSoft.MvvmLight;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using MachineViewer.Plugins.Links.SimpleManipolator.ViewModels.Links;
using MachineViewModels.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.ViewModels
{
    public class GantryLinksViewModel : ViewModelBase
    {

        public ObservableCollection<GantryLinearPositionViewModel> Links { get; set; } = new ObservableCollection<GantryLinearPositionViewModel>();

        public GantryLinksViewModel()
        {
            MessengerInstance.Register<UpdateLinkViewModelsListMessage>(this, OnUpdateLinkViewModelsListMessage);
        }

        private void OnUpdateLinkViewModelsListMessage(UpdateLinkViewModelsListMessage msg)
        {
            Links.Clear();
            var linearLinks = msg.LinkViewModels.Where((o) => o is LinearPositionViewModel).Cast<LinearPositionViewModel>();

            foreach (var item in linearLinks)
            {
                Links.Add(CreateViewModel(item, linearLinks));
            }
        }

        private GantryLinearPositionViewModel CreateViewModel(LinearPositionViewModel master, IEnumerable<LinearPositionViewModel> links)
        {
            var vm = new GantryLinearPositionViewModel() { Master = master.Id, Slave = -1, CompatibleLinks = new List<int>() { -1 } };

            foreach (var item in links)
            {
                if((item.Id != master.Id) && (item.Direction == master.Direction))
                {
                    vm.CompatibleLinks.Add(item.Id);
                }
            }

            return vm;
        }
    }
}
