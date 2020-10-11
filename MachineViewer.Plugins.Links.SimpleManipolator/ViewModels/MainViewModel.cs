using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Links.SimpleManipolator.Extensions;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using MachineViewModels.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ILinkViewModel> Links { get; set; } = new ObservableCollection<ILinkViewModel>();

        private bool _isEnabledPneumaticTransaction;
        public bool IsEnabledPneumaticTransaction
        {
            get { return _isEnabledPneumaticTransaction; }
            set
            {
                if(Set( ref _isEnabledPneumaticTransaction, value, nameof(IsEnabledPneumaticTransaction)))
                {
                    Messenger.Default.Send(new EnablePneumaticTransactionMessage() { Value = _isEnabledPneumaticTransaction });
                }
            }
        }

        private bool _isCheckEnabled = true;

        public bool IsCheckEnabled
        {
            get { return _isCheckEnabled; }
            set { Set(ref _isCheckEnabled, value, nameof(IsCheckEnabled)); }
        }



        public MainViewModel()
        {
            //Messenger.Default.Register<UpdateLinksListMessage>(this, OnUpdateLinksListMessage);
            Messenger.Default.Register<UpdateLinkViewModelsListMessage>(this, OnUpdateLinkViewModelsListMessage);
        }

        private void OnUpdateLinkViewModelsListMessage(UpdateLinkViewModelsListMessage msg)
        {
            Links.Clear();

            foreach (var item in msg.LinkViewModels)
            {
                Links.Add(item);
            }
        }

        //private void OnUpdateLinksListMessage(UpdateLinksListMessage msg)
        //{
        //    Links.Clear();

        //    foreach (var item in msg.Links)
        //    {
        //        Links.Add(item.Item1.Convert(item.Item2));
        //    }

        //    IsCheckEnabled = true;
        //    IsEnabledPneumaticTransaction = false;
        //}
    }
}
