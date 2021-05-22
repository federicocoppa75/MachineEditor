using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HelixToolkit.Wpf;
using MachineModels.Models.Tools;
using MachineViewer.Messages.MaterialRemoval;
using MachineViewer.Messages.Panel;
using MachineViewer.Messages.Trace;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Common.Models.Links.Interpolation;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Utilities
{
    public class LinearLinkMovementManager
    {
        public enum ArcComponentItem
        {
            X,
            Y
        };

        static List<LinearLinkMovementItem> _items = new List<LinearLinkMovementItem>();

        static Dictionary<int, LinksMovementsGroup> _itemsGroups = new Dictionary<int, LinksMovementsGroup>();

        static object _lockObj1 = new object();

        static object _lockObj2 = new object();

        static RenderingEventListener _renderListener = new RenderingEventListener(OnRendering);

        static int _pendingMovement = 0;

        static DateTime _lastMaterialRemovalProcess = DateTime.Now;

        static bool _removalTaskActive;

        static ManualResetEventSlim _removalTaskManualResetEvent = new ManualResetEventSlim();

        public static bool EnableMaterialRemoval { get; set; }

        static LinearLinkMovementManager()
        {
            RenderingEventManager.AddListener(_renderListener);
        }

        public static void Add(int linkId, double value, double targetValue, double duration)
        {
            lock (_lockObj1)
            {
                _items.Add(new LinearLinkMovementItem(linkId, value, targetValue, duration));
            }
        }

        public static void Add(int groupId, int linkId, double value, double targetValue, double duration)
        {
            lock(_lockObj2)
            {
                if (!_itemsGroups.TryGetValue(groupId, out LinksMovementsGroup group))
                {
                    group = new LinksMovementsGroup(groupId, duration);
                    _itemsGroups.Add(groupId, group);
                }

                group.Add(linkId, value, targetValue);
            }
        }

        public static void Add(int linkId, double targetValue, double duration, ArcComponentData data)
        {
            lock (_lockObj2)
            {
                if (!_itemsGroups.TryGetValue(data.GroupId, out LinksMovementsGroup group))
                {
                    group = new LinksMovementsGroup(data.GroupId, duration);
                    _itemsGroups.Add(data.GroupId, group);
                }

                group.Add(linkId, targetValue, data);
            }
        }

        public static void ForceMaterialRemoval()
        {
            if (EnableMaterialRemoval) Interlocked.Exchange(ref _pendingMovement, 1);
        }

        public static void ForceInitialize() { }

        private static void OnRendering(object sender, RenderingEventArgs e) => Evaluate();

        private static void Evaluate()
        {
            var now = DateTime.Now;
            var elapse = now - _lastMaterialRemovalProcess;

            if (elapse > TimeSpan.FromMilliseconds(100))
            {
                EvaluateItems(now);
                EvaluateGroups(now);
                EvaluateMaterialRemovalEvo(now);
            }
        }

        private static void EvaluateGroups(DateTime now)
        {
            lock(_lockObj2)
            {
                if (_itemsGroups.Count > 0) Interlocked.Exchange(ref _pendingMovement, 1);

                foreach (var ig in _itemsGroups.Values)
                {
                    ig.Progress(now);

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        ig.Items.ForEach((i) => Messenger.Default.Send(new UpdateLinearLinkStateToTargetMessage(i.LinkId, i.ActualValue, ig.IsCompleted)));
                    });
                }

                _itemsGroups = _itemsGroups.Where(ig => !ig.Value.IsCompleted).ToDictionary(kp => kp.Key, kp => kp.Value);
            }
        }

        private static void EvaluateItems(DateTime now)
        {
            lock (_lockObj1)
            {
                if (_items.Count > 0) Interlocked.Exchange(ref _pendingMovement, 1);

                _items.ForEach(i =>
                {
                    i.Progress(now);

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Messenger.Default.Send(new UpdateLinearLinkStateToTargetMessage(i.LinkId, i.ActualValue, i.IsCompleted));
                    });
                });

                _items = _items.Where((ii) => !ii.IsCompleted).ToList();
            }
        }

        private static void EvaluateMaterialRemoval(DateTime now)
        {
            if (EnableMaterialRemoval)
            {
                var elapse = now - _lastMaterialRemovalProcess;

               
                if (Interlocked.CompareExchange(ref _pendingMovement, 0, 1) == 1)
                {
                    Messenger.Default.Send(new GetPanelTransformMessage()
                    {
                        SetData = (b, t) =>
                        {
                            if (b && (t != null))
                            {
                                EvaluateMaterialRemovalForRoutTool(t);
                            }
                        }
                    });
                }
                                
                Task.Run(() => Messenger.Default.Send(new ProcessPendingRemovalMessage()));
                _lastMaterialRemovalProcess = now;
                 
            }
        }

        private static void EvaluateMaterialRemovalEvo(DateTime now)
        {
            if(EnableMaterialRemoval)
            {
                if(!_removalTaskActive)
                {
                    Task.Run(() =>
                    {
                        _removalTaskActive = true;

                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref _pendingMovement, 0, 1) == 1)
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    Messenger.Default.Send(new GetPanelTransformMessage()
                                    {
                                        SetData = (b, t) =>
                                        {
                                            if (b) EvaluateMaterialRemovalForRoutTool(t);
                                        }
                                    });
                                });
                            }
                            else
                            {
                                _removalTaskManualResetEvent.Wait();
                                _removalTaskManualResetEvent.Reset();
                            }

                            if (!EnableMaterialRemoval) break;
                        }

                        _removalTaskActive = false;
                    });
                }
                else
                {
                    _removalTaskManualResetEvent.Set();
                }

                Messenger.Default.Send(new ProcessPendingRemovalMessage());
            }
            else
            {
                if (!_removalTaskManualResetEvent.IsSet) _removalTaskManualResetEvent.Set();
            }
        }

        private static void EvaluateMaterialRemovalForRoutTool(Matrix3D panelTransform)
        {
            var invT = panelTransform.Inverse();

            Messenger.Default.Send(new GetActiveRoutToolMessage()
            {
                SetData = (p, d, t, id) =>
                {
                    Task.Run(() =>
                    {
                        var pos = invT.Transform(p);

                        Messenger.Default.Send(new RoutToolMoveMessage()
                        {
                            ToolId = id,
                            Position = pos,
                            Direction = d,
                            Length = t.GetTotalLength(),
                            Radius = t.GetTotalDiameter() / 2.0
                        });
                    });
                }

            });
        }

    }
}
