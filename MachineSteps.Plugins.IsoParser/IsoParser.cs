using MachineSteps.Models;
using System;
using System.IO;

namespace MachineSteps.Plugins.IsoParser
{
    public class IsoParser
    {
        public static MachineStepsDocument Parse(string fileName, bool traceOut = false, Func<int, Tuple<double, double>> getLinkLimits = null)
        {
            MachineStepsDocument msd = null;

            var parser = new IsoTextParser();

            using (var stream = File.OpenText(fileName))
            {
                var lineNumber = 1;

                while (true)
                {
                    var line = stream.ReadLine();

                    if (line == null) break;

                    if (!string.IsNullOrWhiteSpace(line)) parser.ParseLine(line, lineNumber++);
                }
            }

            if (parser.Istructions != null && parser.Istructions.Count > 0)
            {
                StateInfoServices.GetLinkLimits = getLinkLimits;

                msd = IstructionToActionConverter.Convert(parser.Istructions);

                if ((msd != null) && traceOut)
                {
                    var dir = System.IO.Path.GetDirectoryName(fileName);
                    var name = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    var mfile = $"{dir}\\{name}.msteps";

                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                    using (var writer = new System.IO.StreamWriter(mfile))
                    {
                        serializer.Serialize(writer, msd);
                    }
                }
            }

            return msd;
        }
    }
}
