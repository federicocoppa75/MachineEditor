using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Models.Actions;
using MachineSteps.Plugins.StepsViewer.Messages;
using MachineSteps.Plugins.StepsViewer.Models;
using MachineViewer.Plugins.Common.Messages.Inserter;
using MachineViewer.Plugins.Common.Messages.Inverters;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Common.Messages.PanelHolder;
using MachineViewer.Plugins.Common.Messages.ToolChange;
using MachineViewer.Plugins.Common.Models.Links.Interpolation;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.Extensions
{
    public static class ActionsExtension
    {
        static int _interpolationGroupId = 0;

        public static BaseAction CreateBackStepAction(this BaseAction a)
        {
            BaseAction ba = null;

            if (a is AddPanelAction apa) ba = apa.CreateBackStepAction();
            else if (a is LinearPositionLinkAction lpla) ba = lpla.CreateBackStepAction();
            else if (a is LoadToolAction lta) ba = lta.CreateBackStepAction();
            else if (a is TwoPositionLinkAction tpla) ba = tpla.CreateBackStepAction();
            else if (a is UnloadToolAction uta) ba = uta.CreateBackStepAction();
            else if (a is LinearPositionLinkGantryOnAction lplgona) ba = lplgona.CreateBackStepAction();
            else if (a is LinearPositionLinkGantryOffAction lplgoffa) ba = lplgoffa.CreateBackStepAction();
            else if (a is LinearInterpolatedPositionLinkAction lipla) ba = lipla.CreateBackStepAction();
            else if (a is ArcInterpolatedPositionLinkAction aipla) ba = aipla.CreateBackStepAction();
            else if (a is TurnOffInverterAction toffia) ba = toffia.CreateBackStepAction();
            else if (a is TurnOnInverterAction tonia) ba = tonia.CreateBackStepAction();
            else if (a is UpdateRotationSpeedAction ursa) ba = ursa.CreateBackStepAction();

            if (ba != null)
            {
                ba.Id = -a.Id;
                ba.Name = $"{a.Name}(bk)";
                ba.Description = $"{a.Description} (back step action)";
            }

            return ba;
        }

        public static BaseAction CreateBackStepAction(this AddPanelAction a)
        {
            return new RemovePanelAction()
            {
                CornerReference = a.CornerReference,
                PanelHolder = a.PanelHolder,
                PanelId = a.PanelId
            };
        }

        public static BaseAction CreateBackStepAction(this LinearPositionLinkAction a)
        {
            return new LinearPositionLinkLazyAction() { LinkId = a.LinkId };
        }

        public static BaseAction CreateBackStepAction(this LoadToolAction a)
        {
            return new UnloadToolAction()
            {
                ToolSink = a.ToolSink,
                ToolSource = a.ToolSource
            };
        }

        public static BaseAction CreateBackStepAction(this TwoPositionLinkAction a)
        {
            return new TwoPositionLinkLazyAction() { LinkId = a.LinkId };
        }

        public static BaseAction CreateBackStepAction(this UnloadToolAction a)
        {
            return new LoadToolAction()
            {
                ToolSink = a.ToolSink,
                ToolSource = a.ToolSource
            };
        }

        public static BaseAction CreateBackStepAction(this LinearPositionLinkGantryOnAction a)
        {
            return new LinearPositionLinkGantryOffAction()
            {
                MasterId = a.MasterId,
                SlaveId = a.SlaveId
            };
        }

        public static BaseAction CreateBackStepAction(this LinearPositionLinkGantryOffAction a)
        {
            return new LinearPositionLinkGantryOnAction()
            {
                MasterId = a.MasterId,
                SlaveId = a.SlaveId,
                SlaveUnhooked = a.SlaveUnhooked
            };
        }

        public static BaseAction CreateBackStepAction(this LinearInterpolatedPositionLinkAction a)
        {
            var ba = new LinearInterpolatedPositionLinkLazyAction()
            {
                Duration = a.Duration,
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
            };

            for (int i = 0; i < a.Positions.Count(); i++)
            {
                ba.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = a.Positions[i].LinkId });
            }

            return ba;
        }

        public static BaseAction CreateBackStepAction(this ArcInterpolatedPositionLinkAction a)
        {
            var ba = new ArcInterpolatedPositionLinkLazyAction()
            {
                Direction = a.Direction == ArcInterpolatedPositionLinkAction.ArcDirection.CW ? ArcInterpolatedPositionLinkAction.ArcDirection.CCW : ArcInterpolatedPositionLinkAction.ArcDirection.CW,
                Duration = a.Duration,
                Radius = a.Radius,
                StartAngle = a.StartAngle,
                EndAngle = a.EndAngle,
                Angle = -a.Angle,
                Components = new List<ArcInterpolatedPositionLinkAction.ArcComponent>()
            };

            for (int i = 0; i < a.Components.Count(); i++)
            {
                ba.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent()
                {
                    LinkId = a.Components[i].LinkId,
                    CenterCoordinate = a.Components[i].CenterCoordinate,
                    Type = a.Components[i].Type
                });
            }

            return ba;
        }

        public static BaseAction CreateBackStepAction(this TurnOffInverterAction a)
        {
            if (a.RotationSpeed > 0)
            {
                return new TurnOnInverterAction()
                {
                    Head = a.Head,
                    Order = a.Order,
                    RotationSpeed = a.RotationSpeed
                };
            }
            else
            {
                return null;
            }
        }

        public static BaseAction CreateBackStepAction(this TurnOnInverterAction a)
        {
            return new TurnOffInverterAction()
            {
                Head = a.Head,
                Order = a.Order,
                RotationSpeed = a.RotationSpeed
            };
        }

        public static BaseAction CreateBackStepAction(this UpdateRotationSpeedAction a)
        {
            return new UpdateRotationSpeedAction()
            {
                NewRotationSpeed = a.OldRotationSpeed,
                OldRotationSpeed = a.NewRotationSpeed,
                Duration = a.Duration
            };
        }

        public static void ExecuteAction(this BaseAction a, int actionId = 0)
        {
            if (a is AddPanelAction apa) apa.ExecuteAction(actionId);
            else if (a is RemovePanelAction rpa) rpa.ExecuteAction(actionId);
            else if (a is LinearPositionLinkAction lpla) lpla.ExecuteAction(actionId);
            else if (a is TwoPositionLinkAction tpla) tpla.ExecuteAction(actionId);
            else if (a is LoadToolAction lta) lta.ExecuteAction(actionId);
            else if (a is UnloadToolAction uta) uta.ExecuteAction(actionId);
            else if (a is LinearPositionLinkGantryOnAction lplgona) lplgona.ExecuteAction(actionId);
            else if (a is LinearPositionLinkGantryOffAction lplgoffa) lplgoffa.ExecuteAction(actionId);
            else if (a is LinearInterpolatedPositionLinkAction lipla) lipla.ExecuteAction(actionId);
            else if (a is ArcInterpolatedPositionLinkAction aipla) aipla.ExecuteAction(actionId);
            else if (a is InjectAction ia) ia.ExecuteAction(actionId);
            else if (a is TurnOffInverterAction toffia) toffia.ExecuteAction(actionId);
            else if (a is TurnOnInverterAction tonia) tonia.ExecuteAction(actionId);
            else if (a is UpdateRotationSpeedAction ursa) ursa.ExecuteAction(actionId);
            else if (a is ChannelWaiterAction cwa) cwa.ExecuteAction(actionId);
            else if (a is NotOperationAction noa) noa.ExecuteAction(actionId);
        }

        public static void ExecuteAction(this AddPanelAction a, int actionId = 0)
        {
            Messenger.Default.Send(new LoadPanelMessage()
            {
                PanelHolderId = a.PanelHolder,
                Length = a.XDimension,
                Width = a.YDimension,
                Height = a.ZDimension,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this RemovePanelAction a, int actionId = 0)
        {
            Messenger.Default.Send(new UnloadPanelMessage()
            {
                PanelHolderId = a.PanelHolder,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this LinearPositionLinkAction a, int actionId = 0)
        {
            //Messenger.Default.Send(new UpdateLinearLinkStateMessage(a.LinkId, a.RequestedPosition));
            Messenger.Default.Send(new MoveLinearLinkMessage(a.LinkId, a.RequestedPosition, a.Duration) { BackNotifyId = actionId });
        }

        public static void ExecuteAction(this LoadToolAction a, int actionId = 0)
        {
            Messenger.Default.Send(new LoadToolMessage()
            {
                ToolSource = a.ToolSource,
                ToolSink = a.ToolSink,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this TwoPositionLinkAction a, int actionId = 0)
        {
            var v = a.RequestedState == MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On;

            Messenger.Default.Send(new UpdateTwoPositionLinkStateMessage(a.LinkId, v) { BackNotifyId = actionId });
        }

        public static void ExecuteAction(this UnloadToolAction a, int actionId = 0)
        {
            Messenger.Default.Send(new UnloadToolMessage()
            {
                ToolSource = a.ToolSource,
                ToolSink = a.ToolSink,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this LinearPositionLinkGantryOnAction a, int actionId = 0)
        {
            bool virtualSlave = false;

            Messenger.Default.Send(new CheckLinkPresenceMessage() { NotifyAction = () => virtualSlave = true });

            Messenger.Default.Send(new LinearPositionGantryOnMessage()
            {
                MasterId = a.MasterId,
                SlaveId = a.SlaveId,
                BackNotifyId = actionId,
                VirtualSlave = virtualSlave,
                UnhookedSlave = a.SlaveUnhooked
            });
        }

        public static void ExecuteAction(this LinearPositionLinkGantryOffAction a, int actionId = 0)
        {
            Messenger.Default.Send(new LinearPositionGantryOffMessage()
            {
                MasterId = a.MasterId,
                SlaveId = a.SlaveId,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this LinearInterpolatedPositionLinkAction a, int actionId = 0)
        {
            bool isFirst = true;

            foreach (var p in a.Positions)
            {
                //Messenger.Default.Send(new UpdateLinearLinkStateMessage(p.LinkId, p.RequestPosition));
                Messenger.Default.Send(new LinearInterpolationLinkMessage(_interpolationGroupId, p.LinkId, p.RequestPosition, a.Duration) { BackNotifyId = isFirst ? actionId : 0 });
                //isFirst = false;
            }

            _interpolationGroupId++;
        }

        public static void ExecuteAction(this ArcInterpolatedPositionLinkAction a, int actionId = 0)
        {
            bool isFirst = true;

            foreach (var item in a.Components)
            {
                var component = item.Type == ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.X ? MachineViewer.Plugins.Common.Enums.Links.Interpolation.ArcComponent.X : MachineViewer.Plugins.Common.Enums.Links.Interpolation.ArcComponent.Y;
                var data = new ArcComponentData
                {
                    GroupId = _interpolationGroupId,
                    StartAngle = a.StartAngle,
                    Angle = a.Angle,
                    Radius = a.Radius,
                    CenterCoordinate = item.CenterCoordinate,
                    Component = component
                };

                Messenger.Default.Send(new ArcInterpolationLinkMessage(item.LinkId, item.TargetCoordinate, a.Duration, data) { BackNotifyId = isFirst ? actionId : 0});
                //isFirst = false;
            }

            _interpolationGroupId++;
        }

        public static void ExecuteAction(this InjectAction a, int actionId = 0)
        {
            Messenger.Default.Send(new InjectMessage()
            {
                InjectorId = a.InjectorId,
                Duration = a.Duration,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this TurnOffInverterAction a, int actionId = 0)
        {
            Messenger.Default.Send(new TurnOffInverterMessage()
            {
                Head = a.Head,
                Order = a.Order,
                Duration = a.Duration,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this TurnOnInverterAction a, int actionId = 0)
        {
            Messenger.Default.Send(new TurnOnInverterMessage()
            {
                Head = a.Head,
                Order = a.Order,
                Duration = a.Duration,
                RotationSpeed = a.RotationSpeed,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this UpdateRotationSpeedAction a, int actionId = 0)
        {
            Messenger.Default.Send(new UpdateInverterMessage()
            {
                RotationSpeed = a.NewRotationSpeed,
                Duration = a.Duration,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this ChannelWaiterAction a, int actionId = 0)
        {
            Messenger.Default.Send(new WaitForChannelFreeMessage()
            {
                Channel = a.ChannelToWait,
                BackNotifyId = actionId
            });
        }

        public static void ExecuteAction(this NotOperationAction a, int actionId = 0)
        {
            //Task.Run(async () =>
            //{
            //    await Task.Delay(50);
            //    Messenger.Default.Send(new MachineViewer.Plugins.Common.Messages.Generic.BackNotificationMessage() { DestinationId = actionId });
            //});

            Messenger.Default.Send(new MachineViewer.Plugins.Common.Messages.Generic.BackNotificationMessage() { DestinationId = actionId });
        }

        public static double GetDuration(this BaseAction action)
        {
            double result = 0.0;

            if (action is IGradualLinkAction gla) result = gla.Duration;
            else if (action is TwoPositionLinkAction tpla) result = tpla.GetDuration();
            else if (action is InjectAction ia) result = ia.Duration;
            else if (action is TurnOffInverterAction toffa) result = toffa.Duration;
            else if (action is TurnOnInverterAction tona) result = tona.Duration;
            else if (action is UpdateRotationSpeedAction ursa) result = ursa.Duration;

            return result;
        }

        public static double GetDuration(this TwoPositionLinkAction action)
        {
            double result = 0.0;

            Messenger.Default.Send(new ReadTwoPositionLinkDurationMessage()
            {
                LinkId = action.LinkId,
                RequestedState = action.RequestedState == MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On,
                SetDuration = (d) => result = d
            });

            return result;
        }

    }
}
