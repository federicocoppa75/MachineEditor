using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class Axes : IAxes
    {
        // velocità massime
        const double _maxSpeedX = 70000.0;
        const double _maxSpeedY = 40000.0;
        const double _maxSpeedZ = 30000.0;

        // quote assi
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double U { get; private set; }
        public double V { get; private set; }
        public double W { get; private set; }
        public double A { get; private set; }
        public double B { get; private set; }

        // offset assi
        public double OX { get; set; }
        public double OY { get; set; }
        public double OZ { get; set; }

        // matrice di rototraslazione
        public RotoTranslMatrix M { get; set; } = new RotoTranslMatrix();

        // correzione utensile
        public double L { get; set; }
        public double R { get; set; }

        public Gantry GantryX { get; private set; }
        public Gantry GantryY { get; private set; }
        public Gantry GantryZ { get; private set; }
        public Gantry GantryZ2 { get; private set; }

        public double GantryStepX { get; private set; }
        public double GantryStepY { get; private set; }
        public double GantryStepZ { get; private set; }
        public double GantryStepZ2 { get; private set; }

        public GantryCoupling GantryCouplingX { get; set; }
        public GantryCoupling GantryCouplingY { get; set; }
        public GantryCoupling GantryCouplingZ { get; set; }
        public GantryCoupling GantryCouplingZ2 { get; set; }

        public Axes()
        {
        }

        public Axes(double x, double u, double y, double v, double z, double w, double a, double b)
        {
            X = x;
            U = u;
            Y = y;
            V = v;
            Z = z;
            W = w;
            A = a;
            B = b;
        }

        private double GetNewPositionCalcolateDuration(double axPos, double newAxPos, double speed, out double duration)
        {
            var d = newAxPos - axPos;

            duration = Math.Abs((d / speed) * 60);

            return newAxPos;
        }

        public void SetRapidX(double pos, MachineStep step, bool addOffset = true)
        {
            if (GantryX == Gantry.First) SetX(pos, step, addOffset);
            else if (GantryX == Gantry.Second) SetU(pos, step, addOffset);
            else throw new InvalidOperationException("SetRapidX without gantry is not possible");
        }

        public void SetRapidY(double pos, MachineStep step, bool addOffset = true)
        {
            if (GantryY == Gantry.First) SetY(pos, step, addOffset);
            else if (GantryY == Gantry.Second) SetV(pos, step, addOffset);
            else throw new InvalidOperationException("SetRapidY without gantry is not possible");
        }

        public void SetRapidZ(double pos, MachineStep step, bool addOffset = true)
        {
            if (GantryZ == Gantry.First) SetZ(pos, step, addOffset);
            else if (GantryZ == Gantry.Second) SetW(pos, step, addOffset);
            else if (GantryZ2 == Gantry.First) SetA(pos, step, addOffset);
            else if (GantryZ2 == Gantry.Second) SetB(pos, step, addOffset);
            else throw new InvalidOperationException("SetRapidW without gantry is not possible");
        }

        public void SetRapid(double? nx, double? ny, double? nz, MachineStep step, bool addOffset = true)
        {
            double? x = null, y = null, z = null;

            M.Transform(nx, ny, nz, ref x, ref y, ref z);

            if (x.HasValue) SetRapidX(x.Value, step, addOffset);
            if (y.HasValue) SetRapidY(y.Value, step, addOffset);
            if (z.HasValue) SetRapidZ(z.Value, step, addOffset);
        }

        public void SetX(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OX - toolOffsetX : 0.0;
            X = GetNewPositionCalcolateDuration(X, pos + offset, _maxSpeedX, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move X", LinkId = 1, RequestedPosition = X, Duration = t });
        }

        public void SetU(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OX - toolOffsetX : 0.0;
            U = GetNewPositionCalcolateDuration(U, pos + offset, _maxSpeedX, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move U", LinkId = 2, RequestedPosition = U, Duration = t });
        }

        public void SetY(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OY + toolOffsetY : 0.0;
            Y = GetNewPositionCalcolateDuration(Y, pos + offset, _maxSpeedY, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Y", LinkId = 101, RequestedPosition = Y, Duration = t });
        }

        public void SetV(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OY + toolOffsetY : 0.0;
            V = GetNewPositionCalcolateDuration(V, pos + offset, _maxSpeedY, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move V", LinkId = 201, RequestedPosition = V, Duration = t });
        }

        public void SetZ(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            Z = GetNewPositionCalcolateDuration(Z, pos + offset, _maxSpeedZ, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move Z", LinkId = 102, RequestedPosition = Z, Duration = t });
        }

        public void SetW(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            W = GetNewPositionCalcolateDuration(W, pos + offset, _maxSpeedZ, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move W", LinkId = 202, RequestedPosition = W, Duration = t });
        }

        public void SetA(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            A = GetNewPositionCalcolateDuration(A, pos + offset, _maxSpeedZ, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move A", LinkId = 112, RequestedPosition = A, Duration = t });
        }

        public void SetB(double pos, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            B = GetNewPositionCalcolateDuration(B, pos + offset, _maxSpeedZ, out double t);
            step?.Actions.Add(new LinearPositionLinkAction() { Name = "Move B", LinkId = 212, RequestedPosition = B, Duration = t });
        }

        public void SetXU(double posX, double posU, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OX - toolOffsetX : 0.0;
            X = GetNewPositionCalcolateDuration(X, posX + offset, _maxSpeedX, out double t1);
            U = GetNewPositionCalcolateDuration(U, posU + offset, _maxSpeedX, out double t2);
            var t = (t1 > t2) ? t1 : t2;
            var action = new LinearInterpolatedPositionLinkAction()
            {
                Name = "Move X U",
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
                {
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 1, RequestPosition = X },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 2, RequestPosition = U }
                },
                Duration = t
            };

            step?.Actions.Add(action);
        }

        public void SetYV(double posY, double posV, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OY + toolOffsetY : 0.0;
            Y = GetNewPositionCalcolateDuration(Y, posY + offset, _maxSpeedY, out double t1);
            V = GetNewPositionCalcolateDuration(V, posV + offset, _maxSpeedY, out double t2);
            var t = (t1 > t2) ? t1 : t2;
            var action = new LinearInterpolatedPositionLinkAction()
            {
                Name = "Move Y V",
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
                {
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 101, RequestPosition = Y },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 201, RequestPosition = V }
                },
                Duration = t
            };

            step?.Actions.Add(action);
        }

        public void SetZW(double posZ, double posW, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            Z = GetNewPositionCalcolateDuration(Z, posZ + offset, _maxSpeedZ, out double t1);
            W = GetNewPositionCalcolateDuration(W, posW + offset, _maxSpeedY, out double t2);
            var t = (t1 > t2) ? t1 : t2;
            var action = new LinearInterpolatedPositionLinkAction()
            {
                Name = "Move Z W",
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
                {
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 102, RequestPosition = Z },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 202, RequestPosition = W }
                },
                Duration = t
            };

            step?.Actions.Add(action);
        }

        public void SetAB(double posA, double posB, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            A = GetNewPositionCalcolateDuration(A, posA + offset, _maxSpeedZ, out double t1);
            B = GetNewPositionCalcolateDuration(B, posB + offset, _maxSpeedY, out double t2);
            var t = (t1 > t2) ? t1 : t2;
            var action = new LinearInterpolatedPositionLinkAction()
            {
                Name = "Move A B",
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
                {
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 112, RequestPosition = A },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 212, RequestPosition = B }
                },
                Duration = t
            };

            step?.Actions.Add(action);
        }

        public void SetZWAB(double posZ, double posW, double posA, double posB, MachineStep step, bool addOffset = true)
        {
            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);
            var offset = addOffset ? OZ + toolOffsetZ : 0.0;
            Z = GetNewPositionCalcolateDuration(Z, posZ + offset, _maxSpeedZ, out double t1);
            W = GetNewPositionCalcolateDuration(W, posW + offset, _maxSpeedY, out double t2);
            A = GetNewPositionCalcolateDuration(A, posA + offset, _maxSpeedZ, out double t3);
            B = GetNewPositionCalcolateDuration(B, posB + offset, _maxSpeedY, out double t4);
            var t = Math.Max(Math.Max(t1, t2), Math.Max(t3, t4));
            var action = new LinearInterpolatedPositionLinkAction()
            {
                Name = "Move Z W A B",
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
                {
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 102, RequestPosition = Z },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 202, RequestPosition = W },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 112, RequestPosition = A },
                    new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 212, RequestPosition = B }
                },
                Duration = t
            };

            step?.Actions.Add(action);
        }

        private double UpdatePosition(double pos, double newPos, out double delta, out bool changed)
        {
            delta = newPos - pos;
            changed = delta != 0.0;
            return newPos;
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

            GetToolOffset(out double toolOffsetX, out double toolOffsetY, out double toolOffsetZ);

            if (x.HasValue)
            {
                var offset = addOffset ? OX - toolOffsetX : 0.0;

                if (GantryX == Gantry.First)
                {
                    X = UpdatePosition(X, x.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 1, RequestPosition = X });
                }
                else if (GantryX == Gantry.Second)
                {
                    U = UpdatePosition(U, x.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 2, RequestPosition = U });
                }
                else
                {
                    throw new InvalidOperationException("SetPosition without gantry is not possible!");
                }

            }

            if (y.HasValue)
            {
                var offset = addOffset ? OY + toolOffsetY : 0.0;

                if (GantryY == Gantry.First)
                {
                    Y = UpdatePosition(Y, y.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 101, RequestPosition = Y });
                }
                else if (GantryY == Gantry.Second)
                {
                    V = UpdatePosition(V, y.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 201, RequestPosition = V });
                }
                else
                {
                    throw new InvalidOperationException("SetPosition without gantry is not possible!");
                }
            }

            if (z.HasValue)
            {
                var offset = addOffset ? OZ + toolOffsetZ : 0.0;

                if (GantryZ == Gantry.First)
                {
                    Z = UpdatePosition(Z, z.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 102, RequestPosition = Z });
                }
                else if (GantryZ == Gantry.Second)
                {
                    W = UpdatePosition(W, z.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 202, RequestPosition = W });
                }
                else if (GantryZ2 == Gantry.First)
                {
                    A = UpdatePosition(A, z.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 112, RequestPosition = A });
                }
                else if (GantryZ2 == Gantry.Second)
                {
                    B = UpdatePosition(B, z.Value + offset, out dif, out b);
                    steps.Add(dif);
                    if (b) action.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = 212, RequestPosition = B });
                }
                else
                {
                    throw new InvalidOperationException("SetPosition without gantry is not possible!");
                }
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

            action.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent() { LinkId = xMaster ? 1 : 2, CenterCoordinate = i, TargetCoordinate = x, Type = ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.X });
            action.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent() { LinkId = yMaster ? 101 : 201, CenterCoordinate = j, TargetCoordinate = y, Type = ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.Y });
            step?.Actions.Add(action);

            if (xMaster)
            {
                X = x;
            }
            else
            {
                U = x;
            }

            if (yMaster)
            {
                Y = y;
            }
            else
            {
                V = y;
            }
        }

        public void SetGantryX(double g, MachineStep step, bool slaveUnhooked = false)
        {
            var gantry = (g == 1.0) ? Gantry.First : Gantry.Second;
            int master = (g == 1.0) ? 1 : 2;
            int slave = (g == 1.0) ? 2 : 1;

            step.Actions.Add(new LinearPositionLinkGantryOnAction() { Name = "Gantry on XU", MasterId = master, SlaveId = slave, SlaveUnhooked = slaveUnhooked });
            GantryX = gantry;
            GantryCouplingX = slaveUnhooked ? GantryCoupling.Single : GantryCoupling.Couple;
            GantryStepX = (GantryCouplingX == GantryCoupling.Couple) ? U - X : 0.0;
        }

        public void SetGantryY(double g, MachineStep step, bool slaveUnhooked = false)
        {
            var gantry = (g == 1.0) ? Gantry.First : Gantry.Second;
            int master = (g == 1.0) ? 101 : 201;
            int slave = (g == 1.0) ? 201 : 101;

            step.Actions.Add(new LinearPositionLinkGantryOnAction() { Name = "Gantry on YV", MasterId = master, SlaveId = slave, SlaveUnhooked = slaveUnhooked });
            GantryY = gantry;
            GantryCouplingY = slaveUnhooked ? GantryCoupling.Single : GantryCoupling.Couple;
            GantryStepY = (GantryCouplingY == GantryCoupling.Couple) ? V - Y : 0.0;
        }

        public void SetGantryZ(double g, MachineStep step, bool slaveUnhooked = false)
        {
            if ((g == 1) || (g == 2))
            {
                var gantry = (g == 1.0) ? Gantry.First : Gantry.Second;
                int master = (g == 1.0) ? 102 : 202;
                int slave = (g == 1.0) ? 202 : 102;

                step.Actions.Add(new LinearPositionLinkGantryOnAction() { Name = "Gantry on ZW", MasterId = master, SlaveId = slave, SlaveUnhooked = slaveUnhooked });
                GantryZ = gantry;
                GantryCouplingZ = slaveUnhooked ? GantryCoupling.Single : GantryCoupling.Couple;
                GantryZ2 = Gantry.None;
                GantryCouplingZ2 = GantryCoupling.None;
            }
            else if ((g == 3) || (g == 4))
            {
                var gantry = (g == 3.0) ? Gantry.First : Gantry.Second;
                int master = (g == 3.0) ? 112 : 212;
                int slave = (g == 3.0) ? 212 : 112;

                step.Actions.Add(new LinearPositionLinkGantryOnAction() { Name = "Gantry on AB", MasterId = master, SlaveId = slave, SlaveUnhooked = slaveUnhooked });
                GantryZ2 = gantry;
                GantryCouplingZ2 = slaveUnhooked ? GantryCoupling.Single : GantryCoupling.Couple;
                GantryZ = Gantry.None;
                GantryCouplingZ = GantryCoupling.None;
            }
        }

        public void ResetGantryX(MachineStep step)
        {
            if (GantryX != Gantry.None)
            {
                if (GantryCouplingX == GantryCoupling.None) throw new InvalidOperationException("Gantry coupling could not be none!");
                var slaveUnhooked = GantryCouplingX == GantryCoupling.Single;

                if (GantryX == Gantry.First)
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off XU", MasterId = 1, SlaveId = 2, SlaveUnhooked = slaveUnhooked });
                }
                else
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off XU", MasterId = 2, SlaveId = 1, SlaveUnhooked = slaveUnhooked });
                }

                GantryX = Gantry.None;
            }

            GantryCouplingX = GantryCoupling.None;
        }

        public void ResetGantryY(MachineStep step)
        {
            if (GantryY != Gantry.None)
            {
                if (GantryCouplingY == GantryCoupling.None) throw new InvalidOperationException("Gantry coupling could not be none!");
                var slaveUnhooked = GantryCouplingY == GantryCoupling.Single;

                if (GantryY == Gantry.First)
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off YV", MasterId = 101, SlaveId = 201, SlaveUnhooked = slaveUnhooked });
                }
                else
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off YV", MasterId = 201, SlaveId = 101, SlaveUnhooked = slaveUnhooked });
                }

                GantryY = Gantry.None;
            }

            GantryCouplingY = GantryCoupling.None;
        }

        public void ResetGantryZ(MachineStep step)
        {
            if (GantryZ != Gantry.None)
            {
                if (GantryCouplingZ == GantryCoupling.None) throw new InvalidOperationException("Gantry coupling could not be none!");
                var slaveUnhooked = GantryCouplingZ == GantryCoupling.Single;

                if (GantryZ == Gantry.First)
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off ZW", MasterId = 102, SlaveId = 202, SlaveUnhooked = slaveUnhooked });
                }
                else
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off ZW", MasterId = 202, SlaveId = 102, SlaveUnhooked = slaveUnhooked });
                }

                GantryZ = Gantry.None;
            }

            GantryCouplingZ = GantryCoupling.None;
        }

        public void ResetGantryZ2(MachineStep step)
        {
            if (GantryZ2 != Gantry.None)
            {
                if (GantryCouplingZ2 == GantryCoupling.None) throw new InvalidOperationException("Gantry coupling could not be none!");
                var slaveUnhooked = GantryCouplingZ2 == GantryCoupling.Single;

                if (GantryZ2 == Gantry.First)
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off AB", MasterId = 112, SlaveId = 212, SlaveUnhooked = slaveUnhooked });
                }
                else
                {
                    step.Actions.Add(new LinearPositionLinkGantryOffAction() { Name = "Gantry off AB", MasterId = 212, SlaveId = 112, SlaveUnhooked = slaveUnhooked });
                }

                GantryZ2 = Gantry.None;
            }

            GantryCouplingZ2 = GantryCoupling.None;
        }

        public void GetParkX(ref double val) => GetMin(1, ref val);
        public void GetParkU(ref double val) => GetMax(2, ref val);
        public void GetParkY(ref double val) => GetMin(101, ref val);
        public void GetParkV(ref double val) => GetMax(201, ref val);
        public void GetParkZ(ref double val) => GetMax(102, ref val);
        public void GetParkW(ref double val) => GetMax(202, ref val);
        public void GetParkA(ref double val) => GetMax(112, ref val);
        public void GetParkB(ref double val) => GetMax(212, ref val);


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

        private void GetToolOffset(out double x, out double y, out double z)
        {
            x = L * M.M;
            y = L * M.N;
            z = L * M.O;
        }

    }
}
