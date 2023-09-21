using System;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public static class IsoLineFactory
    {
        private static Stack<IfThenLine> _ifThenStack = new Stack<IfThenLine>();

        private static bool _isIncremental = false;

        public static IsoLine Create(int lineNumber, string lineData, Action<IsoLine> addIsoLine)
        {
            IsoLine isoLine = null;

            if(MlvLine.Match(lineData))
            {
                isoLine = MlvLine.Parse(lineData);
            }
            else if(ShiftLine.Match(lineData))
            {
                isoLine = ShiftLine.Parse(lineData);
            }
            else if(VlVariableLine.Match(lineData))
            {
                isoLine = VlVariableLine.Parse(lineData);
            }
            else if(ExkVariableLine.Match(lineData))
            {
                isoLine = ExkVariableLine.Parse(lineData);
            }
            else if(G0Line.Match(lineData))
            {
                isoLine = G0Line.Parse(lineData, _isIncremental);
            }
            else if(G1Line.Match(lineData))
            {
                isoLine = G1Line.Parse(lineData);
            }
            else if(G2Arc.Match(lineData))
            {
                isoLine = G2Arc.Parse(lineData);
            }
            else if(G3Arc.Match(lineData))
            {
                isoLine = G3Arc.Parse(lineData);
            }
            else if(IfThenLine.Match(lineData))
            {
                isoLine = IfThenLine.Parse(lineData);
                _ifThenStack.Push((IfThenLine)isoLine);
            }
            else if(ElseIfLine.Match(lineData))
            {
                isoLine = ElseIfLine.Parse(lineData);
                UpdateElseLine(lineNumber);
                            }
            else if(EndIfLine.Match(lineData))
            {
                isoLine = EndIfLine.Parse(lineData);
                UpdateEndLine(lineNumber);
            }
            else if(M3SLine.Match(lineData))
            {
                isoLine = M3SLine.Parse(lineData);
            }
            else if(MLine.Match(lineData))
            {
                isoLine = MLine.Parse(lineData);
            }
            else if(SLine.Match(lineData))
            {
                isoLine = SLine.Parse(lineData);
            }
            else if (G90.Match(lineData))
            {
                _isIncremental = false;

                if (G90.IsSingle(lineData))
                {
                    //isoLine = G90.Parse(lineData);
                    isoLine = new IsoLine();
                }
                else
                {
                    //addIsoLine(G90.Parse(lineData));
                    isoLine = Create(lineNumber, G90.Remove(lineData), null);
                }
            }
            else if (G91.Match(lineData))
            {
                _isIncremental = true;

                if (G91.IsSingle(lineData))
                {
                    //isoLine = G91.Parse(lineData);
                    isoLine = new IsoLine();
                }
                else
                {
                    //addIsoLine(G91.Parse(lineData));
                    isoLine = Create(lineNumber, G91.Remove(lineData), null);
                }
            }
            else if(G210Line.Match(lineData))
            {
                isoLine = G210Line.Parse(lineData);
            }
            else
            {
                isoLine = new IsoLine();
            }

            isoLine.Data = lineData;
            isoLine.Number = lineNumber;

            if(addIsoLine != null)
            {
                addIsoLine(isoLine);
            }

            return isoLine;
        }

        public static int GetLastIfThenLine()
        {
            if (_ifThenStack.Count > 0)
            {
                var ifLine = _ifThenStack.Peek();

                return ifLine.Number;
            }
            else
            {
                return -1;
            }
        }

        public static void Reset()
        {
            _isIncremental = false;
            _ifThenStack.Clear();
        }

        private static void UpdateElseLine(int line)
        {
            if (_ifThenStack.Count > 0)
            {
                var ifLine = _ifThenStack.Peek();

                ifLine.ElseLine = line;
            }
        }

        private static void UpdateEndLine(int line)
        {
            if ((_ifThenStack.Count > 0) && (line >= 0))
            {
                var ifLine = _ifThenStack.Pop();

                ifLine.EndLine = line;
            }
        }
    }
}
