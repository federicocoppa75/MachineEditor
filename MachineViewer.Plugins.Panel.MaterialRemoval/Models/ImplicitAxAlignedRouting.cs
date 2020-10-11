using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using g3;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public abstract class ImplicitAxAlignedRouting : ImplicitRouting
    {
        private ReaderWriterLockSlim _guard = new ReaderWriterLockSlim();

        protected double _axisComponent;

        protected abstract int DirKey { get; }

        #region ctor

        public ImplicitAxAlignedRouting(double length, double radius, int toolId, double axisComponent) : base(length, radius, toolId)
        {
            _axisComponent = axisComponent;
            _direction = new Vector3d();
            _direction[DirKey] = axisComponent;
        }

        #endregion

        #region abstract

        protected abstract Vector2d GetPlaneComponent(ref Vector3d v);

        #endregion

        #region overridden

        public override double Value(ref Vector3d pt)
        {
            double result = double.MaxValue;

            _guard.EnterReadLock();

            try
            {
                if (_bound.Contains(pt))
                {
                    result = GetDistance(ref pt);
                }
                else
                {
                    result = _bound.Distance(pt);
                }
            }
            finally
            {
                _guard.ExitReadLock();
            }            

            return result;
        }

        public override AxisAlignedBox3d Add(ref Vector3d pt)
        {
            AxisAlignedBox3d box;

            _guard.EnterWriteLock();

            try
            {
                int count = _positions.Count;

                if (count == 0)
                {
                    box = AddFirst(ref pt);
                }
                else if (count == 1)
                {
                    box = AddSecond(ref pt);
                }
                else
                {
                    box = AddAfterSecond(ref pt);
                }
            }
            finally
            {
                _guard.ExitWriteLock();
            }            

            return box;
        }

        protected override bool IsParallelWithToolDirection(ref Vector3d step, out bool isOppositDirection)
        {
            bool result = false;
            var v = GetPlaneComponent(ref step);
            var zeroPlane = (Math.Abs(v.x) < MathUtil.ZeroTolerance) && (Math.Abs(v.y) < MathUtil.ZeroTolerance);

            if (zeroPlane)
            {
                result = true;
                isOppositDirection = step[DirKey] < 0.0;
            }
            else
            {
                isOppositDirection = false;
            }

            return result;
        }

        protected override bool IsOrtoghonalWithToolDiretion(ref Vector3d step) => Math.Abs(step[DirKey]) < MathUtil.ZeroTolerance;

        protected override bool HasSameDirectionOnPlane(ref Vector3d step1, ref Vector3d step2)
        {
            var result = false;
            var dirKey = DirKey; 

            if((Math.Abs(step1[dirKey]) < MathUtil.ZeroTolerance) && (Math.Abs(step2[dirKey]) < MathUtil.ZeroTolerance))
            {
                var s1 = GetPlaneComponent(ref step1);
                var s2 = GetPlaneComponent(ref step2);
                var d1 = s1.Dot(s2);
                var d2 = s1.DotPerp(s2);

                result = (d1 > MathUtil.ZeroTolerance) && (Math.Abs(d2) < MathUtil.ZeroTolerance);
            }
            else
            {
                throw new ArgumentException("The segments must be on the plane perpendicolar to the tool direction!");
            }

            return result;
        }

        protected override double GetVolumeDistance(ref Vector3d pt, ref Volume v)
        {
            var result = double.MaxValue;

            if(v.startIndex == v.endIndex)
            {
                result = GetCilinderDistance(ref pt, v.startIndex);
            }
            else
            {
                var vs = GetVolumeSegment(v.startIndex, v.endIndex);
                var d = vs.Normalized;
                var sp = _positions[v.startIndex];
                var s = GetPlaneComponent(ref pt) - GetPlaneComponent(ref sp);
                var ps = d.Dot(s);

                if (ps <= 0.0) // punto posteriore allo start
                {
                    result = GetCilinderDistance(ref pt, v.startIndex);
                }
                else if (ps >= vs.Length) // punto anteriore alla fine
                {
                    result = GetCilinderDistance(ref pt, v.endIndex);
                }
                else // punto lungo il volume
                {
                    var pr = d.DotPerp(s);
                    result = Math.Abs(pr) - _radius;
                }
            }

            return result;
        }

        #endregion

        #region private

        private double GetCilinderDistance(ref Vector3d pt, int posIndex)
        {
            var p = _positions[posIndex];
            var s = GetPlaneComponent(ref pt) - GetPlaneComponent(ref p);

            return s.Length - _radius;
        }

        private Vector2d GetVolumeSegment(int startIndex, int endIndex)
        {
            var p1 = _positions[startIndex];
            var p2 = _positions[endIndex];

            return GetPlaneComponent(ref p2) - GetPlaneComponent(ref p1);
        }

        private Vector2d GetVolumeDirection(int startIndex, int endIndex) => GetVolumeSegment(startIndex, endIndex).Normalized;

        private double GetDistance(ref Vector3d pt)
        {
            var p = pt;
            var result = double.MaxValue;

            for (int i = 0; i < _volumes.Count; i++)
            {
                var v = _volumes[i];
                var d = v.bound.SignedDistance(p);

                if(d < 0.0) d = GetVolumeDistance(ref p, ref v);
                if (d < result) result = d;
            }

            return result;

        }

        private void InitToolPositionBox(ref Vector3d pt) => _bound = ToolPositionBox(ref pt);

        private AxisAlignedBox3d AddAfterSecond(ref Vector3d pt)
        {
            var lastPos = GetlastPosition();
            var positionType = GetPositionType(ref pt);
            var lastPositionType = GetLastPositionType();

            if (positionType == lastPositionType)
            {
                if ((positionType == PositionType.StepFarward) || (positionType == PositionType.StepBack))
                {
                    UpdateLastPosition(ref pt);
                }
                else if ((positionType == PositionType.StepPlane) && IsParallelWithLastOnPlane(ref pt))
                {
                    UpdateLastPosition(ref pt);
                }
                else
                {
                    AddPosition(ref pt, positionType);
                }
            }
            else
            {
                AddPosition(ref pt, positionType);
            }

            return GetStepBox(ref lastPos, ref pt);
        }

        private AxisAlignedBox3d AddSecond(ref Vector3d pt)
        {
            var lastPos = GetlastPosition();
            var positionType = GetPositionType(ref pt);

            if (positionType == PositionType.StepFarward)
            {
                UpdateLastPosition(ref pt);
            }
            else
            {
                AddPosition(ref pt, positionType);
            }

            return GetStepBox(ref lastPos, ref pt);
        }

        private AxisAlignedBox3d AddFirst(ref Vector3d pt)
        {
            AddPosition(ref pt, PositionType.StepFarward, false);
            InitToolPositionBox(ref pt);
            return _bound;
        }

        #endregion
    }
}
