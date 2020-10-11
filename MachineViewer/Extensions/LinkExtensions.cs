using GalaSoft.MvvmLight.Threading;
using MachineModels.Enums;
using MachineModels.Models.Links;
using MachineViewer.Utilities;
using MachineViewer.ViewModels;
using MachineViewer.ViewModels.Links;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace MachineViewer.Extensions
{
    public static class LinkExtensions
    {
        static LinkExtensions()
        {
            DispatcherHelper.Initialize();
        }

        public static MachineViewModels.ViewModels.Links.ILinkViewModel Convert(this ILink link, MachineElementViewModel mevm, Transform3DGroup transformGroup, Point3D rotationCenter)
        {
            if (link == null) return null;

            if (link is LinearPosition linPos)
            {
                var vm = new LinearPositionViewModel();
                var action = GetLinearPositionLinkAction(transformGroup, linPos.Direction, linPos.Pos);

                MachineViewModels.Extensions.LinkExtensions.UpdateViewModel(vm, linPos);
                vm.Description = mevm.Name;
                if (action != null) vm.ValueChanged += (s, e) => action(e); 

                return vm;
            }
            else if (link is LinearPneumatic pnmPos)
            {
                var vm = new LinearPneumaticViewModel();

                MachineViewModels.Extensions.LinkExtensions.UpdateViewModel(vm, pnmPos);
                vm.Description = mevm.Name;
                ApplyLinearPneumaticLinkAction(transformGroup, vm, mevm);

                return vm;
            }
            else if (link is RotaryPneumatic pnmRot)
            {
                var vm = new RotaryPneumaticViewModel();

                MachineViewModels.Extensions.LinkExtensions.UpdateViewModel(vm, pnmRot);
                vm.Description = mevm.Name;
                ApplyRotaryPneumaticLinkAction(transformGroup, rotationCenter, vm);

                return vm;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static LinkType GetLinkType(this MachineViewModels.ViewModels.Links.ILinkViewModel link) => MachineViewModels.Extensions.LinkExtensions.GetLinkTypeImplementation(link);

        private static Action<double> GetLinearPositionLinkAction(Transform3DGroup transformGroup, LinkDirection direction, double offset)
        {
            Action<double> action = null;

            if (transformGroup != null)
            {
                var tt = new TranslateTransform3D();

                transformGroup.Children.Add(tt);

                switch (direction)
                {
                    case LinkDirection.X:
                        action = (d) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            tt.OffsetX = d - offset;
                        });
                        break;
                    case LinkDirection.Y:
                        action = (d) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            tt.OffsetY = d - offset;
                        });
                        break;
                    case LinkDirection.Z:
                        action = (d) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            tt.OffsetZ = d - offset;
                        });
                        break;
                    default:
                        break;
                }
            }

            return action;
        }

        private static void ApplyLinearPneumaticLinkAction(Transform3DGroup transformGroup, LinearPneumaticViewModel vm, MachineElementViewModel mevm)
        {
            if (transformGroup != null)
            {
                var tt = new TranslateTransform3D();

                transformGroup.Children.Add(tt);

                var setAction = GetSetTraslationAction(vm, tt);

                vm.SetPosition = setAction;
                vm.GetPosition = GetGetTraslationFuncion(vm, tt);

                vm.ValueChanged += (s, e) =>
                {
                    var onPos = vm.OnPos;
                    var offPos = vm.OffPos;
                    var tOn = vm.TOn;
                    var tOff = vm.TOff;
                    var toolActivator = vm.ToolActivator;
                    var lmevm = mevm;
                    var inserterId = vm.InserterId;

                    EvaluateLinkDataByCollision(vm, e, ref onPos, offPos, ref tOn, ref tOff);

                    if (vm.IsGradualTransactionEnabled)
                    {
                        var to = e ? onPos : offPos;
                        var t = e ? tOn : tOff;
                        vm.OnMovementStarting?.Invoke(vm);
                        vm.SetPosition = (d) =>
                        {
                            setAction(d);
                            if (d == to)
                            {
                                vm.OnMovementCompleted?.Invoke(vm);
                                if (toolActivator) lmevm.ManageToolActivation(e);
                                if (inserterId > 0) vm.ManageInserter(e);
                            }
                        };
                        LinearLinkMovementManager.Add(vm.Id, vm.Pos, to, t);
                    }
                    else
                    {
                        var v = e ? onPos : offPos;
                        vm.OnMovementStarting?.Invoke(vm);
                        setAction(v);
                        vm.OnMovementCompleted?.Invoke(vm);
                        if (toolActivator) lmevm.ManageToolActivation(e);
                        if (inserterId > 0) vm.ManageInserter(e);
                    }
                };
            }
        }

        private static Action<double> GetSetTraslationAction(LinearPneumaticViewModel vm, TranslateTransform3D tt)
        {
            Action<double> action = null;

            switch (vm.Direction)
            {
                case LinkDirection.X:
                    action = (d) => tt.OffsetX = d;
                    break;
                case LinkDirection.Y:
                    action = (d) => tt.OffsetY = d;
                    break;
                case LinkDirection.Z:
                    action = (d) => tt.OffsetZ = d;
                    break;
                default:
                    throw new ArgumentException("Invalid traslation direction!");
            }

            return action;
        }

        private static Func<double> GetGetTraslationFuncion(LinearPneumaticViewModel vm, TranslateTransform3D tt)
        {
            Func<double> function = null;

            switch (vm.Direction)
            {
                case LinkDirection.X:
                    function = () => tt.OffsetX;
                    break;
                case LinkDirection.Y:
                    function = () => tt.OffsetY;
                    break;
                case LinkDirection.Z:
                    function = () => tt.OffsetZ;
                    break;
                default:
                    throw new ArgumentException("Invalid traslation direction!");
            }

            return function;
        }

        private static void ApplyRotaryPneumaticLinkAction(Transform3DGroup transformGroup, Point3D rotationCenter, RotaryPneumaticViewModel vm)
        {
            if (transformGroup != null)
            {
                var stopAnimation = new DoubleAnimation();
                Vector3D vector = GetRotationDirection(vm);
                var ar = new AxisAngleRotation3D(vector, 0.0);
                var tr = new RotateTransform3D(ar, rotationCenter);
                var dp = AxisAngleRotation3D.AngleProperty;
                Action<double> setAction = (d) => ar.Angle = d;

                vm.SetPosition = setAction;
                vm.GetPosition = () => ar.Angle;

                transformGroup.Children.Add(tr);

                vm.ValueChanged += (s, e) =>
                {
                    var onPos = vm.OnPos;
                    var offPos = vm.OffPos;
                    var tOn = vm.TOn;
                    var tOff = vm.TOff;

                    if (vm.IsGradualTransactionEnabled)
                    {
                        var to = e ? onPos : offPos;
                        var t = e ? tOn : tOff;
                        vm.OnMovementStarting?.Invoke(vm);
                        vm.SetPosition = (d) =>
                        {
                            setAction(d);
                            if (d == to) vm.OnMovementCompleted?.Invoke(vm);
                        };
                        LinearLinkMovementManager.Add(vm.Id, vm.Pos, to, t);
                    }
                    else
                    {
                        var v = e ? onPos : offPos;
                        vm.OnMovementStarting?.Invoke(vm);
                        setAction(v);
                        vm.OnMovementCompleted?.Invoke(vm);
                    }
                };
            }
        }

        private static Vector3D GetRotationDirection(RotaryPneumaticViewModel vm)
        {
            Vector3D vector;
            switch (vm.Direction)
            {
                case LinkDirection.X: vector = new Vector3D(1.0, 0.0, 0.0); break;
                case LinkDirection.Y: vector = new Vector3D(0.0, 1.0, 0.0); break;
                case LinkDirection.Z: vector = new Vector3D(0.0, 0.0, 1.0); break;
                default: throw new ArgumentException("Invalid rotation direction!");
            }

            return vector;
        }

        private static void EvaluateLinkDataByCollision(IPneumaticColliderExtensionProvider link, bool b, ref double onPos, double offPos, ref double tOn, ref double tOff)
        {
            if (b && link.EvaluateCollision != null)
            {
                link.EvaluateCollision(link);
                if (link.HasCollision)
                {
                    var d1 = onPos - offPos;
                    var d2 = link.CollisionOnPos - offPos;

                    onPos = link.CollisionOnPos;
                    tOn *= d2 / d1;
                }
            }
            else if (!b && link.HasCollision && (link is IPneumaticPresserExtensionProvider ppp))
            {
                var d1 = onPos - offPos;
                var d2 = ppp.Pos - offPos;

                tOff *= d2 / d1;
            }
        }
    }
}
