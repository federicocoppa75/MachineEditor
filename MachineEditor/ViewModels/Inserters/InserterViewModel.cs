using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.ViewModels.Inserters
{
    public class InserterViewModel : InserterBaseViewModel
    {
        private double _diameter;
        public double Diameter
        {
            get { return _diameter; }
            set { Set(ref _diameter, value, nameof(Diameter)); }
        }

        private double _length;

        public double Length
        {
            get { return _length; }
            set { Set(ref _length, value, nameof(Length)); }
        }

        private int _loaderLinkId;

        public int LoaderLinkId
        {
            get { return _loaderLinkId; }
            set { Set(ref _loaderLinkId, value, nameof(LoaderLinkId)); }
        }

        private int _dischargerLinkId;

        public int DischargerLinkId
        {
            get { return _dischargerLinkId; }
            set { Set(ref _dischargerLinkId, value, nameof(DischargerLinkId)); }
        }

    }
}
