using GalaSoft.MvvmLight.Messaging;
using MachineModels.Models.Links;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Links.SimpleManipolator.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Extensions
{
    public static class LinkExtensions
    {
        //public static MachineViewModels.ViewModels.Links.ILinkViewModel Convert(this ILink link, string description = "")
        //{
        //    if (link == null) return null;

        //    if (link is LinearPosition linPos)
        //    {
        //        var vm = new LinearPositionViewModel();

        //        MachineViewModels.Extensions.LinkExtensions.UpdateViewModel(vm, linPos);
        //        vm.Description = description;
        //        vm.Value = linPos.Pos;
        //        vm.ValueChanged += (s, e) => Messenger.Default.Send(new UpdateLinearLinkStateMessage(linPos.Id, e));

        //        return vm;
        //    }
        //    else if (link is TwoPositionsLink pnmPos)
        //    {
        //        var vm = new TwoPositionViewModel();

        //        MachineViewModels.Extensions.LinkExtensions.UpdateViewModel(vm, pnmPos);
        //        vm.Description = description;
        //        vm.ValueChanged += (s, e) => Messenger.Default.Send(new UpdateTwoPositionLinkStateMessage(pnmPos.Id, e));


        //        return vm;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}


        public static MachineModels.Enums.LinkType GetLinkType(this MachineViewModels.ViewModels.Links.ILinkViewModel link)
        {
            if(link is MachineViewModels.ViewModels.Links.LinearPositionViewModel)
            {
                return MachineModels.Enums.LinkType.LinearPosition;
            }
            else if(link is MachineViewModels.ViewModels.Links.TwoPositionLinkViewModel)
            {
                return MachineModels.Enums.LinkType.LinearPneumatic;
            }
            else
            {
                throw new ArgumentException("Invalid argument type!");
            }
        }
    }
}
