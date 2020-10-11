using MachineModels.Enums;
using MachineModels.Models.Links;
using MachineViewModels.ViewModels.Links;
using System;

namespace MachineViewModels.Extensions
{
    public static class LinkExtensions
    {
        public static ILinkViewModel Convert(this ILink link)
        {
            if (link == null) return null;

            if (link is LinearPosition linPos)
            {
                return UpdateViewModel(new LinearPositionViewModel(), linPos);
            }
            else if (link is LinearPneumatic pnmPos)
            {
                return UpdateViewModel(new LinearPneumaticViewModel(), pnmPos);
            }
            else if (link is RotaryPneumatic pnmRot)
            {
                return UpdateViewModel(new RotaryPneumaticViewModel(), pnmRot);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static Link Convert(this ILinkViewModel link)
        {
            if (link == null) return null;

            if (link is LinearPositionViewModel linPosVM)
            {
                return UpdateModel(new LinearPosition(), linPosVM);
            }
            else if (link is LinearPneumaticViewModel linPnmVM)
            {
                return UpdateModel(new LinearPneumatic(), linPnmVM);
            }
            else if (link is RotaryPneumaticViewModel rotPnmVM)
            {
                return UpdateModel(new RotaryPneumatic(), rotPnmVM);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static LinkType GetLinkTypeImplementation(ILinkViewModel link)
        {
            if (link == null)
            {
                throw new ArgumentNullException();
            }

            if (link is LinearPositionViewModel linPosVM)
            {
                return LinkType.LinearPosition;
            }
            else if (link is LinearPneumaticViewModel linPnmVM)
            {
                return LinkType.LinearPneumatic;
            }
            else if (link is RotaryPneumaticViewModel rotPnmVM)
            {
                return LinkType.RotaryPneumatic;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static LinkType GetLinkType(this ILinkViewModel link) => GetLinkTypeImplementation(link);

        public static LinearPosition UpdateModel(LinearPosition m, LinearPositionViewModel vm)
        {
            m.Id = vm.Id;
            m.Max = vm.Max;
            m.Min = vm.Min;
            m.Pos = vm.Pos;
            m.Direction = vm.Direction;

            return m;
        }

        public static TwoPositionsLink UpdateModel(TwoPositionsLink m, TwoPositionLinkViewModel vm)
        {
            m.Id = vm.Id;
            m.OffPos = vm.OffPos;
            m.OnPos = vm.OnPos;
            m.TOff = vm.TOff;
            m.TOn = vm.TOn;
            m.Direction = vm.Direction;
            m.ToolActivator = vm.ToolActivator;

            return m;
        }

        public static LinearPositionViewModel UpdateViewModel(LinearPositionViewModel vm, LinearPosition m)
        {
            vm.Id = m.Id;
            vm.Max = m.Max;
            vm.Min = m.Min;
            vm.Pos = m.Pos;
            vm.Direction = m.Direction;
            vm.Value = m.Pos;

            return vm;
        }

        public static TwoPositionLinkViewModel UpdateViewModel(TwoPositionLinkViewModel vm, TwoPositionsLink m)
        {
            vm.Id = m.Id;
            vm.OffPos = m.OffPos;
            vm.OnPos = m.OnPos;
            vm.TOff = m.TOff;
            vm.TOn = m.TOn;
            vm.Direction = m.Direction;
            vm.ToolActivator = m.ToolActivator;

            return vm;
        }
    }
}
