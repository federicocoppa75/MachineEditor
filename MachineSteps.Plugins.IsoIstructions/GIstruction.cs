using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoIstructions
{
    public class GIstruction : BaseIstruction
    {
        private Dictionary<char, double> _parameters;

        public Dictionary<char, double> Parameters => _parameters;

        public int Istruction { get; private set; }

        public bool IsIncremental { get; set; }

        public GIstruction(int istruction)
        {
            Istruction = istruction;
        }

        public GIstruction(int istruction, Dictionary<char, double> parameters) : this(istruction)
        {
            _parameters = parameters;
        }
    }
}
