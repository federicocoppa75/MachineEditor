using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class GLine : GElement
    {
        //public Dictionary<string, string> Coordinates { get; set; }

        //public static void ParseAndSetParameters(string data, int gId, GLine gLine)
        //{
        //    var elements = data.Split(new char[] { ' ' });
        //    var coordinates = new Dictionary<string, string>();

        //    foreach (var item in elements)
        //    {
        //        if (string.IsNullOrEmpty(item)) continue;
        //        if (string.IsNullOrWhiteSpace(item)) continue;
        //        if (string.Compare(item, $"G{gId}") == 0) continue;
        //        if (Regex.IsMatch(item, "\\AG\\d+")) continue;
        //        if (Regex.IsMatch(item, "\\A[A-Z](-?[0-9]+(.[0-9]+)*)"))
        //        {
        //            var name = Regex.Match(item, "\\A[A-Z]").Value;
        //            var value = item.Substring(name.Length);

        //            coordinates.Add(name, value);
        //        }
        //        else if (Regex.IsMatch(item, "\\A[A-Z]\\(.+\\)([+|-].+)*\\Z"))
        //        {
        //            var name = Regex.Match(item, "\\A[A-Z]").Value;
        //            var value = item.Substring(name.Length, item.Length - name.Length);

        //            coordinates.Add(name, value);
        //        }
        //        else if (Regex.IsMatch(item, "\\A[A-Z]=.+"))
        //        {
        //            var name = Regex.Match(item, "\\A[A-Z]").Value;
        //            var value = item.Substring(name.Length + 1);

        //            coordinates.Add(name, value);
        //        }
        //        else if (Regex.IsMatch(item, "\\A[A-Z].+"))
        //        {
        //            var name = Regex.Match(item, "\\A[A-Z]").Value;
        //            var value = item.Substring(name.Length);

        //            coordinates.Add(name, value);
        //        }

        //    }

        //    gLine.Coordinates = coordinates;
        //}

    }
}
