using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class XYZAxes : IAxes
    {
        // velocità massime
        const double _maxSpeedX = 70000.0;
        const double _maxSpeedY = 40000.0;
        const double _maxSpeedZ = 30000.0;

        // quote assi
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public double U => throw new NotImplementedException();
        public double V => throw new NotImplementedException();
        public double W => throw new NotImplementedException();
        public double A => throw new NotImplementedException();
        public double B => throw new NotImplementedException();

        // offset assi
        public double OX { get; set; }
        public double OY { get; set; }
        public double OZ { get; set; }

        // matrice di rototraslazione
        public RotoTranslMatrix M { get; set; } = new RotoTranslMatrix();

        // correzione utensile
        public double L { get; set; }
        public double R { get; set; }

        public Gantry GantryX => Gantry.First;
        public Gantry GantryY => Gantry.First;
        public Gantry GantryZ => Gantry.First;
        public Gantry GantryZ2 => throw new NotImplementedException();

        public double GantryStepX => throw new NotImplementedException();
        public double GantryStepY => throw new NotImplementedException();
        public double GantryStepZ => throw new NotImplementedException();
        public double GantryStepZ2 => throw new NotImplementedException();

        public GantryCoupling GantryCouplingX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GantryCoupling GantryCouplingY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GantryCoupling GantryCouplingZ { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GantryCoupling GantryCouplingZ2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public void SetRapidX(double pos, MachineStep step, bool addOffset = true) => SetX(pos, step, addOffset);

        public void SetRapidY(double pos, MachineStep step, bool addOffset = true) => SetY(pos, step, addOffset);

        public void SetRapidZ(double pos, MachineStep step, bool addOffset = true) => SetZ(pos, step, addOffset);

        public void SetX(double pos, MachineStep step, bool addOffset = true)
        {
            var offset = addOffset ? OX : 0.0;
            X = GetNewPositionCalcolateDuration(X, pos + offset, _maxSpeedX, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move X", LinkId = 1, RequestedPosition = X, Duration = t });
        }

        public void SetY(double pos, MachineStep step, bool addOffset = true)
        {
            var offset = addOffset ? OY : 0.0;
            Y = GetNewPositionCalcolateDuration(Y, pos + offset, _maxSpeedY, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Y", LinkId = 2, RequestedPosition = Y, Duration = t });

        }

        public void SetZ(double pos, MachineStep step, bool addOffset = true)
        {
            var offset = addOffset ? OZ + L : 0.0;
            Z = GetNewPositionCalcolateDuration(Z, pos + offset, _maxSpeedZ, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Z", LinkId = 3, RequestedPosition = Z, Duration = t });
        }

        public void SetU(double pos, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetV(double pos, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetW(double pos, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetXU(double posX, double posU, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetYV(double posY, double posV, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetZW(double posZ, double posW, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetZWAB(double posZ, double posW, double posA, double posB, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetA(double pos, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetAB(double posA, double posB, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetB(double pos, MachineStep step, bool addOffset = true)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(MachineStep step, double speed, double x, double y, double i, double j, bool cw)
        {
            double _x = 0.0;
            double _y = 0.0;
            double _i = 0.0;
            double _j = 0.0;
            bool _cw = M.IsIdentityFlipped() ? !cw : cw;

            M.Transform(x, y, ref _x, ref _y);
            M.Transform(i, j, ref _i, ref _j);

            SetPositionBase(step, speed, _x, _y, _i, _j, _cw);
        }

        private void SetPositionBase(MachineStep step, double speed, double x, double y, double i, double j, bool cw)
        {
            x += OX;
            y += OY;
            i += OX;
            j += OY;

            var xMaster = (GantryX == Gantry.First);
            var yMaster = (GantryY == Gantry.First);
            var actualX = xMaster ? X : U;
            var actualY = yMaster ? Y : V;
            var v1 = new Tuple<double, double>(actualX - i, actualY - j);
            var v2 = new Tuple<double, double>(x - i, y - j);
            var a1 = Math.Atan2(v1.Item2, v1.Item1);
            var a2 = Math.Atan2(v2.Item2, v2.Item1);
            var a = a2 - a1;
            var r = Math.Sqrt(Math.Pow(v1.Item1, 2.0) + Math.Pow(v1.Item2, 2.0));

            if (cw && a > 0.0) a = a - 2.0 * Math.PI;
            else if (!cw && a < 0.0) a = 2.0 * Math.PI + a;

            var d = Math.Abs((a * r) / speed) * 60.0;

            var action = new ArcInterpolatedPositionLinkAction()
            {
                Name = cw ? "G2" : "G3",
                Direction = cw ? ArcInterpolatedPositionLinkAction.ArcDirection.CW : ArcInterpolatedPositionLinkAction.ArcDirection.CCW,
                Duration = d,
                Radius = r,
                StartAngle = a1,
                EndAngle = a2,
                Angle = a,
                Components = new List<ArcInterpolatedPositionLinkAction.ArcComponent>()
            };

            action.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent() { LinkId = 1, CenterCoordinate = i, TargetCoordinate = x, Type = ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.X });
            action.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent() { LinkId = 2, CenterCoordinate = j, TargetCoordinate = y, Type = ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.Y });
            step?.Actions.Add(action);

            X = x;
            Y = y;
        }

        public void SetPosition(MachineStep step, double speed, double? x, double? y, double? z, bool addOffset = true)
        {
            var _x = new Nullable<double>();
            var _y = new Nullable<double>();
            var _z = new Nullable<double>();

            M.Transform(x, y, z, ref _x, ref _y, ref _z);

            SetPositionBase(step, speed, _x, _y, _z, addOffset);
        }

        private void SetPositionBase(MachineStep step, double speed, double? x, double? y, double? z, bool addOffset = true)
        {
            double dif = 0.0;
            bool b = false;
            var steps = new List<double>();
            var action = new LinearInterpolatedPositionLinkAction() { Name = "G1 move", Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>() };

            if (x.HasValue)
            {
                var offset = addOffset ? OX : 0.0;

                X = UpdatePosition(X, x.Value + offset, out dif, out b);
                steps.Add(dif);
                if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 1, RequestPosition = X });
            }

            if (y.HasValue)
            {
                var offset = addOffset ? OY : 0.0;

                Y = UpdatePosition(Y, y.Value + offset, out dif, out b);
                steps.Add(dif);
                if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 2, RequestPosition = Y });
            }

            if (z.HasValue)
            {
                var offset = addOffset ? OZ + L : 0.0;

                Z = UpdatePosition(Z, z.Value + offset, out dif, out b);
                steps.Add(dif);
                if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 3, RequestPosition = Z });
            }

            if (action.Positions.Count() > 0)
            {
                var dist = Math.Sqrt(steps.Sum((d) => Math.Pow(d, 2.0)));
                var t = (dist / speed) * 60.0;

                action.Duration = t;
                step?.Actions.Add(action);
            }
        }

        private double UpdatePosition(double pos, double newPos, out double delta, out bool changed)
        {
            delta = newPos - pos;
            changed = delta != 0.0;
            return newPos;
        }

        public void GetParkA(ref double val)
        {
            throw new NotImplementedException();
        }

        public void GetParkB(ref double val)
        {
            throw new NotImplementedException();
        }

        public void GetParkU(ref double val)
        {
            throw new NotImplementedException();
        }

        public void GetParkV(ref double val)
        {
            throw new NotImplementedException();
        }

        public void GetParkW(ref double val)
        {
            throw new NotImplementedException();
        }

        public void GetParkX(ref double val) => GetMin(1, ref val);

        public void GetParkY(ref double val) => GetMin(2, ref val);

        public void GetParkZ(ref double val) => GetMax(3, ref val);

        public void ResetGantryX(MachineStep step)
        {
            throw new NotImplementedException();
        }

        public void ResetGantryY(MachineStep step)
        {
            throw new NotImplementedException();
        }

        public void ResetGantryZ(MachineStep step)
        {
            throw new NotImplementedException();
        }

        public void ResetGantryZ2(MachineStep step)
        {
            throw new NotImplementedException();
        }


        public void SetGantryX(double g, MachineStep step, bool slaveUnhooked = false)
        {
            throw new NotImplementedException();
        }

        public void SetGantryY(double g, MachineStep step, bool slaveUnhooked = false)
        {
            throw new NotImplementedException();
        }

        public void SetGantryZ(double g, MachineStep step, bool slaveUnhooked = false)
        {
            throw new NotImplementedException();
        }

        private void GetMin(int linkId, ref double val)
        {
            if (StateInfoServices.GetLinkLimits != null)
            {
                var range = StateInfoServices.GetLinkLimits(linkId);

                val = range.Item1;
            }
        }

        private void GetMax(int linkId, ref double val)
        {
            if (StateInfoServices.GetLinkLimits != null)
            {
                var range = StateInfoServices.GetLinkLimits(linkId);

                val = range.Item2;
            }
        }

        private double GetNewPositionCalcolateDuration(double axPos, double newAxPos, double speed, out double duration)
        {
            var d = newAxPos - axPos;

            duration = Math.Abs((d / speed) * 60);

            return newAxPos;
        }

        
    }
}
