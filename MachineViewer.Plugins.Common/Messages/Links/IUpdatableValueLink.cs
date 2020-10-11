using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public interface IUpdatableValueLink
    {
        int Id { get; }
    }

    public interface IUpdatableValueLink<T> : IUpdatableValueLink where T : struct
    {
        T Value { get; set; }
    }
}
