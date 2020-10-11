using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class ExkVariableLine : VariableLine
    {
        public bool SetBit { get; set; }

        public int BitIndex { get; set; }

        public static bool Match(string data) => Match1(data).Success || Match2(data).Success;

        private static Match Match1(string data) => Regex.Match(data, "\\A\\??\\%?(ETK|EOK|EDK)\\[[0-9]+\\](\\.[0-9]+)?=.+");

        private static Match Match2(string data) => Regex.Match(data, "\\A\\??\\%?(ETK|EOK|EDK)[0-9]+=.+");

        public static IsoLine Parse(string data)
        {
            var match1 = Match1(data);
            
            if(match1.Success)
            {
                return Parse1(data);  
            }
            else
            {
                var match2 = Match2(data);

                if(match2.Success)
                {
                    return Parse2(data);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        private static IsoLine Parse1(string data)
        {
            var str = Regex.Match(data, "\\A\\?\\%(ETK|EOK|EDK)\\[[0-9]+\\](\\.[0-9]+)?=").Value;
            //var str = Match1(data).Value;
            var bitMatch = Regex.Match(str, "\\.[0-9]+");
            var setBit = false;
            int bitIndex = -1;

            if (bitMatch.Success && int.TryParse(bitMatch.Value, out bitIndex))
            {
                setBit = true;

            }

            var name = Regex.Match(str, "(ETK|EOK|EDK)\\[[0-9]+\\]").Value;
            var value = data.Substring(str.Length);

            return new ExkVariableLine()
            {
                Name = name,
                Value = value,
                SetBit = setBit,
                BitIndex = bitIndex
            };
        }

        private static IsoLine Parse2(string data)
        {
            //var str = Match2(data).Value;
            var str = Regex.Match(data, "\\A\\%(ETK|EOK|EDK)[0-9]+=").Value;
            var vName = Regex.Match(str, "ETK|EOK|EDK").Value;
            var vIdx = Regex.Match(str, "[0-9]+").Value;
            var value = data.Substring(str.Length);

            return new ExkVariableLine()
            {
                Name = $"{vName}[{vIdx}]",
                Value = value
            };
        }

        public override IsoLineType Type => IsoLineType.ExkVariable;
    }
}
