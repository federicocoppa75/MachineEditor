using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.StateData
{
    public class Axes
    {
        // velocità massime
        const double _maxSpeedX = 70000.0;
        const double _maxSpeedY = 40000.0;
        const double _maxSpeedZ = 30000.0;

        // quote assi
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        // correzione utensile
        public double L { get; set; }
        public double R { get; set; }

        public Axes(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void SetX(double pos, MachineStep step)
        {
            var t = CalcolateDuration(X, pos, _maxSpeedX);
            X = pos;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move X", LinkId = 1, RequestedPosition = X, Duration = t });
        }

        public void SetY(double pos, MachineStep step)
        {
            var t = CalcolateDuration(Y, pos, _maxSpeedY);
            Y = pos;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Y", LinkId = 2, RequestedPosition = Y, Duration = t });
        }

        public void SetZ(double pos, MachineStep step)
        {
            var t = CalcolateDuration(Z, pos, _maxSpeedZ);
            Z = pos;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Z", LinkId = 3, RequestedPosition = Z, Duration = t });

        }

        public void SetXI(double inc, MachineStep step)
        {
            var t = CalcolateDuration(inc, _maxSpeedX);
            X += inc;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move XI", LinkId = 1, RequestedPosition = X, Duration = t });
        }

        public void SetYI(double inc, MachineStep step)
        {
            var t = CalcolateDuration(inc, _maxSpeedY);
            Y += inc;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move YI", LinkId = 2, RequestedPosition = Y, Duration = t });
        }

        public void SetZI(double inc, MachineStep step)
        {
            var t = CalcolateDuration(inc, _maxSpeedZ);
            Z += inc;
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move ZI", LinkId = 3, RequestedPosition = Z, Duration = t });

        }

        public void SetPosition(MachineStep step, double speed, double? x, double? y, double? z)
        {
            double dif = 0.0;
            bool b = false;
            var steps = new List<double>();
            var action = new LinearInterpolatedPositionLinkAction() { Name = "G1 move", Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>() };

            if (x.HasValue)
            {
                X = UpdatePosition(X, x.Value, out dif, out b);
                steps.Add(dif);
                if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 1, RequestPosition = X });
            }

            if (y.HasValue)
            {
                Y = UpdatePosition(Y, y.Value, out dif, out b);
                steps.Add(dif);
                if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 2, RequestPosition = Y });
            }

            if (z.HasValue)
            {                
                Z = UpdatePosition(Z, z.Value + L, out dif, out b);
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

        public void SetPosition(MachineStep step, double speed, double x, double y, double i, double j, bool cw)
        {
            var v1 = new Tuple<double, double>(X - i, Y - j);
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

        private double CalcolateDuration(double axPos, double newAxPos, double speed) => CalcolateDuration(newAxPos - axPos, speed);
        //{
        //    var d = newAxPos - axPos;
        //    var t = Math.Abs((d / speed) * 60);

        //    return t;
        //}

        private double CalcolateDuration(double incrementalStep, double speed)
        {
            var t = Math.Abs((incrementalStep / speed) * 60);

            return t;
        }

        private double UpdatePosition(double pos, double newPos, out double delta, out bool changed)
        {
            delta = newPos - pos;
            changed = delta != 0.0;
            return newPos;
        }

        private double UpdatePositionIncremental(double pos, double incrementalStep, out double delta, out bool changed)
        {
            changed = incrementalStep != 0.0;
            delta = incrementalStep;
            return pos + incrementalStep;
        }
    }
}
