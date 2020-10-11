using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMaterialRemoval.Messages;

namespace TestMaterialRemoval.ViewModels
{
    public class PanelSizesViewModel : ViewModelBase
    {
        private double _sizeX;

        public double SizeX
        {
            get => _sizeX;
            set
            {
                if(Set(ref _sizeX, value, nameof(SizeX)))
                {
                    MessengerInstance.Send(new PanelSizeChangedMessage());
                }
            }
        }

        private double _sizeY;

        public double SizeY
        {
            get => _sizeY;
            set
            {
                if(Set(ref _sizeY, value, nameof(SizeY)))
                {
                    MessengerInstance.Send(new PanelSizeChangedMessage());
                }
            }
        }

        private double _sizeZ;

        public double SizeZ
        {
            get => _sizeZ;
            set
            {
                if(Set(ref _sizeZ, value, nameof(SizeZ)))
                {
                    MessengerInstance.Send(new PanelSizeChangedMessage());
                }
            }
        }

    }
}
