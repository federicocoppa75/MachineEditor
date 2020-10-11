using MachineEditor.ViewModels.Colliders;
using MachineModels.Models.Colliders;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Extensions
{
    public static class ColliderExtensions
    {
        public static ColliderViewModel ToViewMode(this Collider m)
        {
            if (m != null)
            {
                if (m is PointsCollider pc)
                {
                    switch (pc.Points.Count)
                    {
                        case 1:
                            return new PointColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D()
                            };
                        case 2:
                            return new TwoPointsColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D(),
                                Position2 = pc.Points[1].ToPoint3D()
                            };
                        case 3:
                            return new ThreePointsColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D(),
                                Position2 = pc.Points[1].ToPoint3D(),
                                Position3 = pc.Points[2].ToPoint3D()
                            };
                        case 4:
                            return new FourPointsColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D(),
                                Position2 = pc.Points[1].ToPoint3D(),
                                Position3 = pc.Points[2].ToPoint3D(),
                                Position4 = pc.Points[3].ToPoint3D()
                            };
                        case 6:
                            return new SixPointsColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D(),
                                Position2 = pc.Points[1].ToPoint3D(),
                                Position3 = pc.Points[2].ToPoint3D(),
                                Position4 = pc.Points[3].ToPoint3D(),
                                Position5 = pc.Points[4].ToPoint3D(),
                                Position6 = pc.Points[5].ToPoint3D()
                            };
                        case 8:
                            return new EightPointsColliderViewModel()
                            {
                                Type = pc.Type,
                                Radius = pc.Radius,
                                Position = pc.Points[0].ToPoint3D(),
                                Position2 = pc.Points[1].ToPoint3D(),
                                Position3 = pc.Points[2].ToPoint3D(),
                                Position4 = pc.Points[3].ToPoint3D(),
                                Position5 = pc.Points[4].ToPoint3D(),
                                Position6 = pc.Points[5].ToPoint3D(),
                                Position7 = pc.Points[6].ToPoint3D(),
                                Position8 = pc.Points[7].ToPoint3D()
                            };
                        default:
                            throw new ArgumentException("Unexpected number of collider points!");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid model!");
                }
            }
            else
            {
                return null;
            }
        }

        public static Collider ToModel(this ColliderViewModel vm)
        {
            if (vm != null)
            {
                if (vm is PointColliderViewModel pvm)
                {
                    var m = new PointsCollider();

                    m.Type = vm.Type;
                    m.Radius = pvm.Radius;
                    m.Points.Add(pvm.Position.ToVector());

                    if (vm is TwoPointsColliderViewModel pvm2) m.Points.Add(pvm2.Position2.ToVector());

                    if (vm is ThreePointsColliderViewModel pvm3) m.Points.Add(pvm3.Position3.ToVector());

                    if (vm is FourPointsColliderViewModel pvm4) m.Points.Add(pvm4.Position4.ToVector());

                    if (vm is SixPointsColliderViewModel pvm6)
                    {
                        m.Points.Add(pvm6.Position5.ToVector());
                        m.Points.Add(pvm6.Position6.ToVector());
                    }

                    if (vm is EightPointsColliderViewModel pvm8)
                    {
                        m.Points.Add(pvm8.Position7.ToVector());
                        m.Points.Add(pvm8.Position8.ToVector());
                    }
                    return m;
                }
                else
                {
                    throw new ArgumentException("Invalid view model!");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
