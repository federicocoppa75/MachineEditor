using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MachineModels.Models.Colliders;
using MachineViewer.Messages;
using MachineViewer.Messages.Panel;
using MachineViewer.Messages.Trace;
using MachineViewer.ViewModels;
using MachineViewer.ViewModels.Colladers;
using MachineViewer.ViewModels.Links;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Extensions
{
    public static class ColliderExtensions
    {
        public static MachineElementViewModel ToViewModel(this Collider m, MachineElementViewModel parent = null)
        {
            MachineElementViewModel vm = null;

            if (m is PointsCollider pcm)
            {
                switch (pcm.Type)
                {
                    case MachineModels.Enums.ColliderType.Presser:
                        {
                            var pcvm = new PointsColliderViewModel()
                            {
                                Type = pcm.Type,
                                Radius = pcm.Radius,
                                Points = pcm.Points.Select((p) => p.ToPoint3D()).ToList(),
                                Parent = parent,
                                Material = HelixToolkit.Wpf.Materials.Green
                            };

                            pcvm.MeshGeometry = GetMeshGeometry(pcvm.Radius, pcvm.Points);

                            AttachProbeToLinkForPresser(pcvm);

                            vm = pcvm;
                        }
                        break;

                    case MachineModels.Enums.ColliderType.Gripper:
                        {
                            var pcvm = new PointsColliderViewModel()
                            {
                                Type = pcm.Type,
                                Radius = pcm.Radius,
                                Points = pcm.Points.Select((p) => p.ToPoint3D()).ToList(),
                                Parent = parent,
                                Material = HelixToolkit.Wpf.Materials.Red
                            };

                            pcvm.MeshGeometry = GetMeshGeometry(pcvm.Radius, pcvm.Points);

                            AttachProbeToLink(pcvm);

                            vm = pcvm;
                        }
                        break;

                    case MachineModels.Enums.ColliderType.Detect:
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                throw new ArgumentException("Unexpected collider type!");
            }

            return vm;
        }

        private static void AttachProbeToLinkForPresser(ColliderViewModel vm)
        {
            var apvm = GetFirstAncestorWithPneumaticLink(vm);

            if (apvm != null)
            {
                var pnLink = apvm.LinkToParent as IPneumaticPresserExtensionProvider;
                var lpvm = GetFirstAncestorWithLinearPositionLink(vm, pnLink.Direction);

                if(lpvm != null)
                {
                    var lpLink = lpvm.LinkToParent as LinearPositionViewModel;
                    var sign = Math.Sign(pnLink.OnPos - pnLink.OffPos);
                    var direction = GetDirectionVector(pnLink.Direction) * sign;

                    AttachActionToCollider(vm, pnLink);

                    lpLink.ValueChanged += (s, e) =>
                    {
                        var t = vm.CheckPanelIntersectionAsync(pnLink.Value, direction).Result;
                        //var t = vm.CheckPanelIntersection(pnLink.Value, direction);
                        
                        if (t.Item1)
                        {
                            double intersectValue = t.Item2;

                            if (intersectValue <= 0)
                            {
                                pnLink.HasCollision = true;
                                vm.Collided = true;
                                pnLink.Pos = pnLink.Pos - intersectValue;
                            }
                            else if(vm.Collided)
                            {
                                pnLink.Pos = pnLink.Pos - intersectValue;

                                //verifico se il link si sia esteso completamente
                                bool b = (sign > 0) ? (pnLink.Pos > pnLink.OnPos) : (pnLink.Pos < pnLink.OnPos);

                                if (b)
                                {
                                    pnLink.HasCollision = false;
                                    vm.Collided = false;
                                    pnLink.Pos = pnLink.OnPos;
                                }
                            }
                        }
                    };
                } 
                else
                {
                    AttachActionToCollider(vm, pnLink);
                }
            }
        }

        private static void AttachProbeToLink(ColliderViewModel vm)
        {
            var avm = GetFirstAncestorWithPneumaticLink(vm);

            if (avm != null)
            {
                var link = avm.LinkToParent as IPneumaticColliderExtensionProvider;

                AttachActionToCollider(vm, link);
            }
        }

        private static void AttachActionToCollider(ColliderViewModel vm, IPneumaticColliderExtensionProvider link)
        {
            var s = Math.Sign(link.OnPos - link.OffPos);
            var max = Math.Abs(link.OnPos - link.OffPos);
            var direction = GetDirectionVector(link.Direction) * s;

            link.EvaluateCollision = (e) =>
            {
                double intersectValue = link.OnPos;
                bool state = link.Value;
                //var t = vm.CheckPanelIntersection(state, direction);
                var t = vm.CheckPanelIntersectionAsync(state, direction).Result;
                
                if (t.Item1 && (t.Item2 < max))
                {
                    intersectValue = t.Item2;
                    link.HasCollision = true;
                    link.CollisionOnPos = intersectValue * s;
                    vm.Collided = true;
                }
                else
                {
                    link.HasCollision = false;
                    vm.Collided = false;
                }
            };

            link.OnMovementCompleted = (e) =>
            {
                if (e.Value) vm.EvaluateOnState();
            };

            link.OnMovementStarting = (e) =>
            {
                if (!e.Value) vm.EvaluateOffState();
            };
        }

        private static Vector3D GetDirectionVector(MachineModels.Enums.LinkDirection d)
        {
            var direction = new Vector3D();

            switch (d)
            {
                case MachineModels.Enums.LinkDirection.X: direction.X = 1.0; break;
                case MachineModels.Enums.LinkDirection.Y: direction.Y = 1.0; break;
                case MachineModels.Enums.LinkDirection.Z: direction.Z = 1.0; break;
                default: break;
            }

            return direction;
        }

        public static Rect3D? GetPanel()
        {
            Transform3D panelTransform = null;
            Rect3D? panel = null;

            Messenger.Default.Send(new GetPanelPositionMessage()
            {
                SetData = (b, t, box) =>
                {
                    if (b)
                    {
                        panelTransform = t;

                        var p = panelTransform.Transform(box.Value.Location);
                        panel = new Rect3D(p, box.Value.Size);
                        //Messenger.Default.Send(new TraceBoxMessage() { Box = panel.Value, Brush = Brushes.Yellow });
                    }
                }                    
            });

            return panel;
        }

        private static MeshGeometry3D GetMeshGeometry(double radius, List<Point3D> points)
        {
            var builder = new HelixToolkit.Wpf.MeshBuilder();

            foreach (var p in points)
            {
                builder.AddSphere(p, radius);
            }

            return builder.ToMesh();
        }


        private static Action GetGripperOnCollideAction(ColliderViewModel vm)
        {
            return null;
        }

        private static MachineElementViewModel GetFirstAncestorWithPneumaticLink(MachineElementViewModel vm)
        {
            if(vm != null)
            {
                if((vm.LinkToParent != null) && (vm.LinkToParent is MachineViewModels.ViewModels.Links.TwoPositionLinkViewModel))
                {
                    return vm;
                }
                else
                {
                    return GetFirstAncestorWithPneumaticLink(vm.Parent);
                }
            }
            else
            {
                return null;
            }
        }

        private static MachineElementViewModel GetFirstAncestorWithLinearPositionLink(MachineElementViewModel vm)
        {
            if(vm != null)
            {
                if((vm.LinkToParent != null) && (vm.LinkToParent is MachineViewModels.ViewModels.Links.LinearPositionViewModel))
                {
                    return vm;
                }
                else
                {
                    return GetFirstAncestorWithLinearPositionLink(vm.Parent);
                }
            }
            else
            {
                return null;
            }
        }

        private static MachineElementViewModel GetFirstAncestorWithLinearPositionLink(MachineElementViewModel vm, MachineModels.Enums.LinkDirection direction)
        {
            if (vm != null)
            {
                if ((vm.LinkToParent != null) && 
                    (vm.LinkToParent is MachineViewModels.ViewModels.Links.LinearPositionViewModel lpvm) &&
                    (lpvm.Direction == direction))
                {
                    return vm;
                }
                else
                {
                    return GetFirstAncestorWithLinearPositionLink(vm.Parent, direction);
                }
            }
            else
            {
                return null;
            }
        }

    }
}
