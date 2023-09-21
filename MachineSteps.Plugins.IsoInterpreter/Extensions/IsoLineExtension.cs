using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Plugins.IsoInterpreter.Enums;
using MachineSteps.Plugins.IsoInterpreter.Helpers;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Models;
using System;

namespace MachineSteps.Plugins.IsoInterpreter.Extensions
{
    public static class IsoLineExtension
    {
        public static void Process(this IsoLine isoLine)
        {
            switch (isoLine.Type)
            {
                case Enums.IsoLineType.Unknown:
                    break;
                case Enums.IsoLineType.Mlv:
                    Process((MlvLine)isoLine);
                    break;
                case Enums.IsoLineType.Shift:
                    Process((ShiftLine)isoLine);
                    break;
                case Enums.IsoLineType.VlVariable:
                    Process((VlVariableLine)isoLine);
                    break;
                case Enums.IsoLineType.ExkVariable:
                    Process((ExkVariableLine)isoLine);
                    break;
                case Enums.IsoLineType.G0:
                    Process((G0Line)isoLine);
                    break;
                case Enums.IsoLineType.G1:
                    Process((G1Line)isoLine);
                    break;
                case IsoLineType.G2:
                    Process((G2Arc)isoLine);
                    break;
                case IsoLineType.G3:
                    Process((G3Arc)isoLine);
                    break;
                case IsoLineType.IfThen:
                    Process((IfThenLine)isoLine);
                    break;
                case IsoLineType.ElseIf:
                    Process((ElseIfLine)isoLine);
                    break;
                case IsoLineType.EndIf:
                    Process((EndIfLine)isoLine);
                    break;
                case IsoLineType.M:
                    Process((MLine)isoLine);
                    break;
                case IsoLineType.S:
                    Process((SLine)isoLine);
                    break;
                case IsoLineType.M3S:
                    Process((M3SLine)isoLine);
                    break;
                case IsoLineType.G90:
                    Process((G90)isoLine);
                    break;
                case IsoLineType.G91:
                    Process((G91)isoLine);
                    break;
                case IsoLineType.G210:
                    Process((G210Line)isoLine);
                    break;
                default:
                    break;
            }
        }

        public static void Process(MlvLine isoLine) => Messenger.Default.Send(new SetMatrixLevelMessage() { Level = isoLine.Level });

        public static void Process(ShiftLine isoLine) => Messenger.Default.Send(new ShiftValueChangedMessage { Direction = isoLine.Direction, Value = isoLine.Value });

        public static void Process(VlVariableLine isoLine) => Messenger.Default.Send(new SetVariableValueMessage { Name = isoLine.Name, Value = isoLine.Value, VariableType = GetVariableType(isoLine) });

        public static void Process(G0Line isoLine) => Messenger.Default.Send(new G0LineMessage() { Coordinates = isoLine.Coordinates, IsIncremental = isoLine.IsIncremental });

        public static void Process(G1Line isoLine) => Messenger.Default.Send(new G1LineMessage() { Coordinates = isoLine.Coordinates, Type = 1 });

        public static void Process(G2Arc isoLine) => Messenger.Default.Send(new G2ArcMessage() { Coordinates = isoLine.Coordinates, Type = 2 });

        public static void Process(G3Arc isoLine) => Messenger.Default.Send(new G3ArcMessage() { Coordinates = isoLine.Coordinates, Type = 3 });
        
        public static void Process(ExkVariableLine isoLine) => Messenger.Default.Send(new SetVariableValueMessage() { Name = isoLine.Name, Value = isoLine.Value, VariableType = GetVariableType(isoLine) });

        public static void Process(IfThenLine isoLine) => IfThenElseHelper.EvaluateIfThenLine(isoLine);

        public static void Process(ElseIfLine isoLine) { }

        public static void Process(EndIfLine isoLine) { }

        public static void Process(MLine isoLine) => Messenger.Default.Send(new MLineMessage() { Value = isoLine.Value });

        public static void Process(SLine isoLine) => Messenger.Default.Send(new SLineMessage() { Value = isoLine.Value });

        public static void Process(M3SLine isoLine)
        {
            Messenger.Default.Send(new SLineMessage() { Value = isoLine.Value });
            Messenger.Default.Send(new MLineMessage() { Value = 3 });
        }

        public static void Process(G90 isoLine) => Messenger.Default.Send(new G90Message());

        public static void Process(G91 isoLine) => Messenger.Default.Send(new G91Message());

        public static void Process(G210Line isoLine) => Messenger.Default.Send(new G210LineMessage() { Coordinates = isoLine.Coordinates, IsIncremental = true, Type = 210 });

        private static VariableType GetVariableType(VariableLine isoLine)
        {
            switch (isoLine.Type)
            {
                case IsoLineType.VlVariable:
                    return VariableType.VL;
                case IsoLineType.ExkVariable:
                    return VariableType.ExK;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
