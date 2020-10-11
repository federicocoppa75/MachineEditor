using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineEditor.ViewModels.Colliders;
using MachineEditor.ViewModels.Inserters;
using MachineEditor.ViewModels.PanelHolders;
using MachineEditor.ViewModels.ToolHolders;
using MachineModels.Enums;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels
{
    public class MachineElementViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        private string _modelFile;
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string ModelFile
        {
            get { return _modelFile; }
            set
            {
                if(string.IsNullOrEmpty(_modelFile))
                {
                    _modelFile = value;
                    RaisePropertyChanged(nameof(ModelFile));
                    AddModel();
                }
                else if(string.Compare(_modelFile, value, true) != 0)
                {
                    _modelFile = value;
                    RaisePropertyChanged(nameof(ModelFile));
                    ChangeModel();
                }
                
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;

                if (Model != null)
                {
                    if (_isVisible) Messenger.Default.Send(new Messages.ShowModel() { Model = Model });
                    else Messenger.Default.Send(new Messages.HideModel() { Model = Model });
                }
            }
        }


        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                if(_color != value)
                {
                    _color = value;
                    RaisePropertyChanged(nameof(Color));
                    Model?.Content.ChangeColor(_color);
                }
            }
        }

        private LinkType _linkToParentType;

        [Category("Link to parent")]
        public LinkType LinkToParentType
        {
            get { return _linkToParentType; }
            set
            {
                _linkToParentType = value;
                RaisePropertyChanged(nameof(LinkToParentType));
                UpdateLinkToParent();
            }
        }

        [Category("Link to parent")]
        [ExpandableObject]
        public MachineViewModels.ViewModels.Links.ILinkViewModel LinkToParentData { get; set; }

        private ToolHolderType _toolHolderType;
        [Category("Tool holder")]
        public ToolHolderType ToolHolderType
        {
            get { return _toolHolderType; }
            set
            {
                if(Set(ref _toolHolderType, value, nameof(ToolHolderType)))
                {
                    UpdateToolHolderData();
                }
            }
        }

        private IToolHolderViewModel _toolHolderData;
        [Category("Tool holder")]
        [ExpandableObject]
        public IToolHolderViewModel ToolHolderData
        {
            get { return _toolHolderData; }
            set
            {
                UpdateToolHolderData(value);
            }
        }

        private bool _hasPanelHolder;
        [Category("Panel holder")]
        public bool HasPanelHolder
        {
            get => _hasPanelHolder;
            set
            {
                if(Set(ref _hasPanelHolder, value, nameof(HasPanelHolder)))
                {
                    UpdatePanelHolderData();
                }
            }
        }

        private PanelHolderViewModel _panelHolder;
        [Category("Panel holder")]
        [ExpandableObject]
        public PanelHolderViewModel PanelHolder
        {
            get => _panelHolder;
            set => UpdatePanelHolderData(value);
        }

        private ColliderGeometry _colliderGeometry;
        [Category("Collider")]
        public ColliderGeometry ColliderGeometry
        {
            get => _colliderGeometry;
            set
            {
                if(Set(ref _colliderGeometry, value, nameof(ColliderGeometry)))
                {
                    UpdateColliderData();
                }
            }
        }

        private ColliderViewModel _collider;
        [Category("Collider")]
        [ExpandableObject]
        public ColliderViewModel Collider
        {
            get => _collider;
            set => UpdateColliderData(value);
        }

        [Category("Location")]
        [ExpandableObject]
        public Point3D Location
        {
            get { return (Model != null) ? Model.Transform.Transform(Model.Content.Bounds.Location) : new Point3D(); }
            set
            {
                if(Model != null)
                {
                    var p = Model.Transform.Transform(Model.Content.Bounds.Location);
                    var d = value - p;

                    if ((d.X != 0.0) || (d.Z != 0.0) || (d.Z != 0.0)) Messenger.Default.Send(new Messages.ChangeModelLocation() { DX = d.X, DY = d.Y, DZ = d.Z });
                }
            }
        }

        [Category("Size")]
        public double SizeX => (Model != null) ? Model.Content.Bounds.SizeX : 0.0;

        [Category("Size")]
        public double SizeY => (Model != null) ? Model.Content.Bounds.SizeY : 0.0;

        [Category("Size")]
        public double SizeZ => (Model != null) ? Model.Content.Bounds.SizeZ : 0.0;

        private InserterType _inserterType;
        [Category("Inserter")]
        public InserterType InserterType
        {
            get => _inserterType; 
            set
            {
                if(Set(ref _inserterType, value, nameof(InserterType)))
                {
                    UpdateInserterData();
                }
            }
        }

        private InserterBaseViewModel _inserterData;
        [Category("Inserter")]
        [ExpandableObject]
        public InserterBaseViewModel InserterData
        {
            get => _inserterData; 
            set
            {
                UpdateInserterData(value);
            }
        }


        private ModelVisual3D _model;

        [Browsable(false)]
        public ModelVisual3D Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(Model));
                RaisePropertyChanged(nameof(SizeX));
                RaisePropertyChanged(nameof(SizeY));
                RaisePropertyChanged(nameof(SizeZ));
                RaisePropertyChanged(nameof(Location));
            }
        }

        [Browsable(false)]
        public MachineElementViewModel Parent { get; set; }

        private ModelVisual3D _toolHodelrModel;

        private ModelVisual3D _colliderModel;

        private ModelVisual3D _panelHolderModel;

        private ModelVisual3D _inserterModel;

        [Browsable(false)]
        public ObservableCollection<MachineElementViewModel> Children { get; set; } = new ObservableCollection<MachineElementViewModel>();

        public void NotifyModelLOcationChanged() => RaisePropertyChanged(nameof(Location));

        public MachineElementViewModel()
        {
            Messenger.Default.Register<Messages.ChangeSpindleDirectionVisibility>(this, ChangeSpidleDirectionVisibility);
            Messenger.Default.Register<Messages.ChangeCollidersVisibility>(this, ChangeCollidersVisibility);
            Messenger.Default.Register<Messages.ChangePanelHolderVisibility>(this, ChangePanelHolderVisibility);
            Messenger.Default.Register<Messages.ChangeInserterVisibility>(this, ChangeInserterVisibility);
        }

        private void SetModel(ModelVisual3D model)
        {
            Model = model;
            if (model != null) Color = model.Content.GetColor();
        }

        private void ChangeModel()
        {
            var old = Model;
            SetModel(LoadAndQueueModel(_modelFile));
            Messenger.Default.Send(new Messages.ChangeModel() { Model = Model, OldModel = old });
        }

        private void AddModel()
        {
            SetModel(LoadAndQueueModel(_modelFile));
            Messenger.Default.Send(new Messages.AddNewModel() { Model = Model });
        }

        private ModelVisual3D LoadAndQueueModel(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var mi = new HelixToolkit.Wpf.ModelImporter();
                var m = mi.Load(filePath, null);

                return new ModelVisual3D() { Content = m };
            }
            else
            {
                return null;
            }
        }

        private void UpdateLinkToParent()
        {
            switch (_linkToParentType)
            {
                case LinkType.Static:
                    LinkToParentData = null;
                    break;
                case LinkType.LinearPosition:
                    LinkToParentData = new MachineViewModels.ViewModels.Links.LinearPositionViewModel();
                    break;
                case LinkType.LinearPneumatic:
                    LinkToParentData = new MachineViewModels.ViewModels.Links.LinearPneumaticViewModel();
                    break;
                case LinkType.RotaryPneumatic:
                    LinkToParentData = new MachineViewModels.ViewModels.Links.RotaryPneumaticViewModel();
                    break;
                default:
                    break;
            }

            RaisePropertyChanged(nameof(LinkToParentData));
        }

        private void UpdateToolHolderData()
        {
            IToolHolderViewModel vm = null;

            switch (_toolHolderType)
            {
                case ToolHolderType.None:
                    vm = null;
                    break;
                case ToolHolderType.Static:
                    vm = new StaticToolHolderViewModel();
                    break;
                case ToolHolderType.AutoSource:
                    vm = new AutoSourceToolHolderViewModel();
                    break;
                case ToolHolderType.AutoSink:
                    vm = new AutoSinkToolHolderViewModel();
                    break;
                default:
                    break;
            }

            UpdateToolHolderData(vm);
        }

        private void UpdateToolHolderData(IToolHolderViewModel vm)
        {
            RemoveToolHolderDataCallback();

            Set(ref _toolHolderData, vm, nameof(ToolHolderData));

            UpdateToolHolderModel();
            AddToolHolderDataCallback();
        }

        private void UpdateColliderData()
        {
            ColliderViewModel vm = null;

            switch (_colliderGeometry)
            {
                case ColliderGeometry.None:
                    vm = null;
                    break;
                case ColliderGeometry.Point:
                    vm = new PointColliderViewModel();
                    break;
                case ColliderGeometry.TwoPoints:
                    vm = new TwoPointsColliderViewModel();
                    break;
                case ColliderGeometry.ThreePoints:
                    vm = new ThreePointsColliderViewModel();
                    break;
                case ColliderGeometry.FourPoints:
                    vm = new FourPointsColliderViewModel();
                    break;
                case ColliderGeometry.SixPoints:
                    vm = new SixPointsColliderViewModel();
                    break;
                case ColliderGeometry.EightPoints:
                    vm = new EightPointsColliderViewModel();
                    break;
                default:
                    throw new ArgumentException("Unexpected collider type!");
            }

            UpdateColliderData(vm);
        }

        private void UpdateColliderData(ColliderViewModel vm)
        {
            RemoveColliderDataCallback();
            Set(ref _collider, vm, nameof(Collider));
            UpdateColliderModel();
            AddColliderDataCallback();
        }

        private void UpdatePanelHolderData()
        {
            PanelHolderViewModel vm = _hasPanelHolder ? new PanelHolderViewModel() : null;

            UpdatePanelHolderData(vm);
        }

        private void UpdatePanelHolderData(PanelHolderViewModel vm)
        {
            RemovePanelHolderDataCallback();
            Set(ref _panelHolder, vm, nameof(PanelHolder));
            UpdatePanelHolderModel();
            AddPanelHolderDataCallback();
        }

        private void UpdateInserterData()
        {
            InserterBaseViewModel vm = null;

            switch (_inserterType)
            {
                case InserterType.None:
                    break;
                case InserterType.Injector:
                    vm = new InjectorViewModel();
                    break;
                case InserterType.Inserter:
                    vm = new InserterViewModel();
                    break;
                default:
                    break;
            }

            UpdateInserterData(vm);
        }

        private void UpdateInserterData(InserterBaseViewModel vm)
        {
            RemoveInserterDataCallback();

            Set(ref _inserterData, vm, nameof(InserterData));

            UpdateInserterModel();
            AddInserterDataCallback();
        }

        private void AddToolHolderDataCallback()
        {
            if(ToolHolderData != null && ToolHolderData is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += ToolHolderDataPropertyChanged;
            }
        }

        private void RemoveToolHolderDataCallback()
        {
            if (ToolHolderData != null && ToolHolderData is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= ToolHolderDataPropertyChanged;
            }
        }

        private void ToolHolderDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateToolHolderModel();
        }

        private void AddColliderDataCallback()
        {
            if (Collider != null && Collider is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += ColliderDataPropertyChanged;
            }
        }

        private void RemoveColliderDataCallback()
        {
            if (Collider != null && Collider is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= ColliderDataPropertyChanged;
            }
        }

        private void ColliderDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateColliderModel();
        }

        private void AddPanelHolderDataCallback()
        {
            if (PanelHolder != null && PanelHolder is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += PanelHolderDataPropertyChanged;
            }
        }

        private void RemovePanelHolderDataCallback()
        {
            if (PanelHolder != null && PanelHolder is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= PanelHolderDataPropertyChanged;
            }
        }

        private void AddInserterDataCallback()
        {
            if(InserterData != null && InserterData is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += InserterDataPropertyChanged;
            }
        }

        private void RemoveInserterDataCallback()
        {
            if (InserterData != null && InserterData is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= InserterDataPropertyChanged;
            }
        }

        private void InserterDataPropertyChanged(object sendder, PropertyChangedEventArgs e)
        {
            UpdateInserterModel();
        }

        private void PanelHolderDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePanelHolderModel();
        }

        private void UpdateToolHolderModel(bool checkVisibility = true)
        {
            if(_model != null)
            {
                if(_toolHodelrModel != null) _model.Children.Remove(_toolHodelrModel);

                if(ToolHolderData != null && ToolHolderData is ToolHolderViewModel th)
                {
                    bool addModel = true;

                    if(checkVisibility)
                    {
                        Messenger.Default.Send(new Messages.GetSpindleDirectionVisibility() { GetVisibility = (b) => addModel = b });
                    }

                    if(addModel)
                    {
                        var builder = new HelixToolkit.Wpf.MeshBuilder();
                        var p1 = th.Position;
                        var p2 = th.Position + (Vector3D)(th.Direction * 20.0);

                        builder.AddArrow(p1, p2, 6.0, 2.0);
                        _toolHodelrModel = new ModelVisual3D() { Content = new GeometryModel3D(builder.ToMesh(), HelixToolkit.Wpf.Materials.Red) };
                        Model.Children.Add(_toolHodelrModel);
                    }
                }
            }     
        }

        private void UpdateColliderModel(bool checkVisibility = true)
        {
            if (_model != null)
            {
                if (_colliderModel != null) _model.Children.Remove(_colliderModel);

                if (Collider != null && Collider is PointColliderViewModel vm)
                {
                    bool addModel = true;

                    if (checkVisibility)
                    {
                        Messenger.Default.Send(new Messages.GetColliderVisibility() { GetVisibility = (b) => addModel = b });
                    }

                    if (addModel)
                    {
                        var builder = new HelixToolkit.Wpf.MeshBuilder();
                        var p1 = vm.Position;

                        builder.AddSphere(p1, vm.Radius);

                        if (Collider is TwoPointsColliderViewModel vm2) builder.AddSphere(vm2.Position2, vm.Radius);
                        if (Collider is ThreePointsColliderViewModel vm3) builder.AddSphere(vm3.Position3, vm.Radius);
                        if (Collider is FourPointsColliderViewModel vm4) builder.AddSphere(vm4.Position4, vm.Radius);
                        if (Collider is SixPointsColliderViewModel vm6)
                        {
                            builder.AddSphere(vm6.Position5, vm.Radius);
                            builder.AddSphere(vm6.Position6, vm.Radius);
                        }
                        if (Collider is EightPointsColliderViewModel vm8)
                        {
                            builder.AddSphere(vm8.Position7, vm.Radius);
                            builder.AddSphere(vm8.Position8, vm.Radius);
                        }

                        _colliderModel = new ModelVisual3D() { Content = new GeometryModel3D(builder.ToMesh(), HelixToolkit.Wpf.Materials.Red) };
                        Model.Children.Add(_colliderModel);
                    }
                }
            }
        }

        private void UpdatePanelHolderModel(bool checkVisibility = true)
        {
            if (_model != null)
            {
                if (_panelHolderModel != null) _model.Children.Remove(_panelHolderModel);

                if (PanelHolder != null && PanelHolder is PanelHolderViewModel vm)
                {
                    bool addModel = true;

                    if (checkVisibility)
                    {
                        Messenger.Default.Send(new Messages.GetPanelHolderVisibility() { GetVisibility = (b) => addModel = b });
                    }

                    if (addModel)
                    {
                        var builder = new HelixToolkit.Wpf.MeshBuilder();
                        var p1 = vm.Position;

                        builder.AddSphere(p1, 10.0);
                        _panelHolderModel = new ModelVisual3D() { Content = new GeometryModel3D(builder.ToMesh(), HelixToolkit.Wpf.Materials.Blue) };
                        Model.Children.Add(_panelHolderModel);
                    }
                }
            }
        }

        private void UpdateInserterModel(bool checkVisibility = true)
        {
            if(_model != null)
            {
                if (_inserterModel != null) _model.Children.Remove(_inserterModel);

                if(InserterData != null && InserterData is InserterBaseViewModel vm)
                {
                    bool addModel = true;

                    if(checkVisibility)
                    {
                        Messenger.Default.Send(new Messages.GetInserterVisibility() { GetVisibility = (b) => addModel = b });
                    }

                    if (addModel)
                    {
                        var builder = new HelixToolkit.Wpf.MeshBuilder();
                        var p1 = vm.Position;
                        var p2 = vm.Position + (Vector3D)(vm.Direction * 20.0);

                        builder.AddArrow(p1, p2, 6.0, 2.0);
                        _inserterModel = new ModelVisual3D() { Content = new GeometryModel3D(builder.ToMesh(), HelixToolkit.Wpf.Materials.Red) };
                        Model.Children.Add(_inserterModel);
                    }
                }
            }
        }

        private void ChangeSpidleDirectionVisibility(Messages.ChangeSpindleDirectionVisibility msg)
        {
            if(msg.Visible)
            {
                UpdateToolHolderModel(false);
            }
            else
            {
                if(_model != null && _toolHodelrModel != null)
                {
                    _model.Children.Remove(_toolHodelrModel);
                }
            }
        }

        private void ChangeCollidersVisibility(Messages.ChangeCollidersVisibility msg)
        {
            if (msg.Visible)
            {
                UpdateColliderModel(false);
            }
            else
            {
                if (_model != null && _colliderModel != null)
                {
                    _model.Children.Remove(_colliderModel);
                }
            }
        }

        private void ChangePanelHolderVisibility(Messages.ChangePanelHolderVisibility msg)
        {
            if (msg.Visible)
            {
                UpdatePanelHolderModel(false);
            }
            else
            {
                if (_model != null && _panelHolderModel != null)
                {
                    _model.Children.Remove(_panelHolderModel);
                }
            }
        }

        private void ChangeInserterVisibility(Messages.ChangeInserterVisibility msg)
        {
            if (msg.Visible)
            {
                UpdateInserterModel(false);
            }
            else
            {
                if (_model != null && _inserterModel != null)
                {
                    _model.Children.Remove(_inserterModel);
                }
            }
        }
    }
}
