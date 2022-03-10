using MachineSteps.Models.Steps;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public enum Gantry
    {
        None,
        First,
        Second
    };

    public enum GantryCoupling
    {
        None,
        Couple,
        Single
    }
    public interface IAxes
    {
        double X { get; }
        double Y { get; }
        double Z { get; }
        double U { get; }
        double V { get; }
        double W { get; }
        double A { get; }
        double B { get; }

        double OX { get; set; }
        double OY { get; set; }
        double OZ { get; set; }

        RotoTranslMatrix M { get; set; }

        double L { get; set; }
        double R { get; set; }

        Gantry GantryX { get; }
        Gantry GantryY { get; }
        Gantry GantryZ { get; }
        Gantry GantryZ2 { get; }
        double GantryStepX { get; }
        double GantryStepY { get; }
        double GantryStepZ { get; }
        double GantryStepZ2 { get; }
        GantryCoupling GantryCouplingX { get; set; }
        GantryCoupling GantryCouplingY { get; set; }
        GantryCoupling GantryCouplingZ { get; set; }
        GantryCoupling GantryCouplingZ2 { get; set; }
        bool Is3AxesWithVirtuals { get; set; }

        void SetRapidX(double pos, MachineStep step, bool addOffset = true);
        void SetRapidY(double pos, MachineStep step, bool addOffset = true);
        void SetRapidZ(double pos, MachineStep step, bool addOffset = true);
        void SetRapid(double? nx, double? ny, double? nz, MachineStep step, bool addOffset = true);
        void SetU(double pos, MachineStep step, bool addOffset = true);
        void SetV(double pos, MachineStep step, bool addOffset = true);
        void SetW(double pos, MachineStep step, bool addOffset = true);
        void SetX(double pos, MachineStep step, bool addOffset = true);
        void SetXU(double posX, double posU, MachineStep step, bool addOffset = true);
        void SetY(double pos, MachineStep step, bool addOffset = true);
        void SetYV(double posY, double posV, MachineStep step, bool addOffset = true);
        void SetZ(double pos, MachineStep step, bool addOffset = true);
        void SetZW(double posZ, double posW, MachineStep step, bool addOffset = true);
        void SetZWAB(double posZ, double posW, double posA, double posB, MachineStep step, bool addOffset = true);
        void SetA(double pos, MachineStep step, bool addOffset = true);
        void SetAB(double posA, double posB, MachineStep step, bool addOffset = true);
        void SetB(double pos, MachineStep step, bool addOffset = true);

        void SetPosition(MachineStep step, double speed, double x, double y, double i, double j, bool cw);
        void SetPosition(MachineStep step, double speed, double? x, double? y, double? z, bool addOffset = true);

        void SetGantryX(double g, MachineStep step, bool slaveUnhooked = false);
        void SetGantryY(double g, MachineStep step, bool slaveUnhooked = false);
        void SetGantryZ(double g, MachineStep step, bool slaveUnhooked = false);
        void ResetGantryX(MachineStep step);
        void ResetGantryY(MachineStep step);
        void ResetGantryZ(MachineStep step);
        void ResetGantryZ2(MachineStep step);

        void GetParkA(ref double val);
        void GetParkB(ref double val);
        void GetParkU(ref double val);
        void GetParkV(ref double val);
        void GetParkW(ref double val);
        void GetParkX(ref double val);
        void GetParkY(ref double val);
        void GetParkZ(ref double val);
    }
}