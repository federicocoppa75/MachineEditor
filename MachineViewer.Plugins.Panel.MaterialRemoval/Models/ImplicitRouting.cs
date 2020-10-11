using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public abstract partial class ImplicitRouting : BoundedImplicitFunction3d
    {
        protected enum PositionType
        {
            StepFarward,// avanzamento dell'utensile lungo la direzione dell'utensile
            StepPlane,  // avanzamento dell'utensile nel piano ortogonale alla direzione dell'utensile
            StepBack,   // avanzamento dell'utensile nella direzione opposta a quella dell'utensile
            Any,        // avanzamento dell'utensile sia lungo la direzione dell'utensile, sia nel piano orgogonale all'utensile stesso
        }

        protected struct Volume
        {
            public int startIndex;
            public int endIndex;
            public AxisAlignedBox3d bound;

            public void SetEndIndex(int i) => endIndex = i;
            public void UpdateBound(ref AxisAlignedBox3d box) => ImplicitRouting.UpdateBound(ref bound, ref box);
        }

        static int _seed = 0;

        protected double _radius;
        protected double _length;
        protected Vector3d _direction;
        protected List<Vector3d> _positions;
        protected List<PositionType> _positionTypes;
        protected AxisAlignedBox3d _bound;
        protected List<Volume> _volumes;

        public int Id { get; private set; }
        public int ToolId { get; private set; }

        #region ctor

        public ImplicitRouting(double length, double radius, int toolId) : base()
        {
            Id = _seed++;
            ToolId = toolId;
            _radius = radius;
            _length = length;
            _positions = new List<Vector3d>();
            _positionTypes = new List<PositionType>();
            _volumes = new List<Volume>();
        }

        #endregion

        #region public

        public AxisAlignedBox3d Bounds() => _bound;

        #endregion

        #region abstract 

        public abstract double Value(ref Vector3d pt);

        public abstract AxisAlignedBox3d Add(ref Vector3d pt);

        protected abstract bool IsParallelWithToolDirection(ref Vector3d step, out bool isOppositDirection);

        protected abstract bool IsOrtoghonalWithToolDiretion(ref Vector3d step);

        protected abstract bool HasSameDirectionOnPlane(ref Vector3d step1, ref Vector3d step2);

        protected abstract AxisAlignedBox3d ToolPositionBox(ref Vector3d position);

        protected abstract double GetVolumeDistance(ref Vector3d pt, ref Volume v);

        #endregion

        #region protected

        protected void AddPosition(ref Vector3d point, PositionType type, bool updateBound = true)
        {
            _positions.Add(point);
            _positionTypes.Add(type);

            if (updateBound) UpdateBound(ref point);

            ManageVolumeOnAddPosition(ref point, type);
        }

        protected Vector3d GetStepFromLast(ref Vector3d pt)
        {
            if(_positions.Count == 0) throw new InvalidOperationException("Can not eval step from last point if the rout has 0 points!");

            var count = _positions.Count;

            return pt - _positions[count - 1];
        }

        protected PositionType GetPositionType(ref Vector3d pt)
        {
            var step = GetStepFromLast(ref pt);

            return GetPositionTypeFromStep(ref step);
        }

        protected PositionType GetPositionTypeFromStep(ref Vector3d step)
        {
            PositionType result = PositionType.Any;

            if(IsParallelWithToolDirection(ref step, out bool isOppositDirection))
            {
                result = isOppositDirection ? PositionType.StepBack : PositionType.StepFarward;
            }
            else if(IsOrtoghonalWithToolDiretion(ref step))
            {
                result = PositionType.StepPlane;
            }

            return result;
        }

        protected void UpdateLastPosition(ref Vector3d pt, bool updateBound = true)
        {
            if (_positions.Count == 0) throw new InvalidOperationException("Can not eval update last point if the rout has 0 points!");

            var lastPosIndex = _positions.Count - 1;

            _positions[lastPosIndex] = pt;

            if (updateBound) UpdateBound(ref pt);

            UpdateLastVolume(lastPosIndex);
        }

        protected PositionType GetLastPositionType()
        {
            if (_positions.Count == 0) throw new InvalidOperationException("Can not get last position type if the rout has 0 points!");

            var count = _positions.Count;

            return _positionTypes[count - 1];
        }

        protected Vector3d GetlastPosition()
        {
            if (_positions.Count == 0) throw new InvalidOperationException("Can not get last position if the rout has 0 points!");

            var count = _positions.Count;

            return _positions[count - 1];
        }

        protected Vector3d GetLastStep()
        {
            if (_positions.Count < 2) throw new InvalidOperationException("Can not get last step if the rout has les than 2 points!");

            var count = _positions.Count;

            return _positions[count - 1] - _positions[count - 2];
        }

        protected bool IsParallelWithLastOnPlane(ref Vector3d pt)
        {
            var step1 = GetStepFromLast(ref pt);
            var step2 = GetLastStep();

            return HasSameDirectionOnPlane(ref step1, ref step2);
        }

        protected void UpdateBound(ref Vector3d pt)
        {
            var box = ToolPositionBox(ref pt);

            UpdateBound(ref _bound, ref box);
        }

        protected static void UpdateBound(ref AxisAlignedBox3d boxToUpdate, ref AxisAlignedBox3d boxUpdater)
        {
            for (int i = 0; i < 3; i++)
            {
                if (boxToUpdate.Min[i] > boxUpdater.Min[i]) boxToUpdate.Min[i] = boxUpdater.Min[i];
            }

            for (int i = 0; i < 3; i++)
            {
                if (boxToUpdate.Max[i] < boxUpdater.Max[i]) boxToUpdate.Max[i] = boxUpdater.Max[i];
            }
        }

        protected AxisAlignedBox3d GetStepBox(ref Vector3d lastPos, ref Vector3d newPos)
        {
            var box1 = ToolPositionBox(ref lastPos);
            var box2 = ToolPositionBox(ref newPos);

            UpdateBound(ref box1, ref box2);

            return box1;
        }

        #endregion

        #region private

        private void ManageVolumeOnAddPosition(ref Vector3d point, PositionType type)
        {
            var count = _positions.Count;

            if (count > 1)
            {
                var preType = _positionTypes[count - 2];

                if ((type == PositionType.StepPlane) && (preType == PositionType.StepFarward))
                {
                    AddNewVolumeAndUpdate();
                }
                else if ((type == PositionType.StepPlane) && (preType == PositionType.StepBack))
                {
                    AddNewVolumeAndUpdate();
                }
                else if ((preType == PositionType.StepPlane) && ((type == PositionType.StepFarward) || (type == PositionType.StepBack)))
                {
                    _volumes.Add(new Volume() { startIndex = count - 1, endIndex = count - 1, bound = ToolPositionBox(ref point) });
                }
                else if ((type == PositionType.StepPlane) && (preType == PositionType.StepPlane))
                {
                    AddNewVolumeAndUpdate();
                }
            }
            else
            {
                _volumes.Add(new Volume() { startIndex = count - 1, endIndex = count - 1, bound = ToolPositionBox(ref point) });
            }
        }

        private void UpdateLastVolume(int ptIndex)
        {
            var pt = _positions[ptIndex];
            var vCount = _volumes.Count;
            var tBox = ToolPositionBox(ref pt);
            var v = _volumes[vCount - 1];
            v.SetEndIndex(ptIndex);
            v.UpdateBound(ref tBox);
            _volumes[vCount - 1] = v;
        }

        private void AddNewVolumeAndUpdate()
        {
            var count = _positions.Count();
            var p1 = _positions[count - 2];
            var p2 = _positions[count - 1];
            var box1 = ToolPositionBox(ref p1);
            var box2 = ToolPositionBox(ref p2);
            var v = new Volume() { startIndex = count - 2, endIndex = count - 1, bound = box1 };

            v.UpdateBound(ref box2);
            _volumes.Add(v);
        }

        #endregion
    }
}
