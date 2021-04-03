using GalaSoft.MvvmLight.CommandWpf;
using HelixToolkit.Wpf;
using MachineModels.Models;
using MachineViewer.Extensions;
using MachineViewModels.ViewModels.Links;
using MachineViewModelUtils.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using ViewModels.MachineViewModels;
using System.Linq;
using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MachineViewer.Messages;
using MachineModels.Models.ToolHolders;
using MachineModels.Models.Tooling;
using MachineModels.Models.Tools;
using MachineModels.Enums;
using MachineViewer.ViewModels.Probing;
using MachineViewer.Messages.Probing;
using MachineViewer.Messages.Visibility;
using MachineModels.IO;
using System.Threading.Tasks;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.ViewModels.ToolHolder;
using MachineViewer.Messages.Trace;
using MachineViewer.Messages.Tooling;
using MachineViewer.Messages.Panel;
using MachineViewer.Messages.Struct;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using MachineViewer.Messages.Inserter;

namespace MachineViewer.ViewModels
{
    public class MainViewModel : Machine3DModelsViewModelBase
    {
        private string _lastToolingFile = string.Empty;
        private string _lastToolsFile = string.Empty;
        private string _lastMachProjectFile = string.Empty;

        public ObservableCollection<ILinkViewModel> Links { get; set; } = new ObservableCollection<ILinkViewModel>();

        public ObservableCollection<MachineElementViewModel> Machines { get; set; } = new ObservableCollection<MachineElementViewModel>();

        private Material _selectedMaterial;
        public Material SelectedMaterial => _selectedMaterial ?? (_selectedMaterial = MaterialHelper.CreateMaterial(System.Windows.Media.Colors.Red));

        public Material OriginalSelectedMaterial { get; set; }

        public PointSelectionCommand PointSelectionCommand { get; private set; }

        private MachineElementViewModel _treeElementSelected;
        public MachineElementViewModel TreeElementSelected
        {
            get => _treeElementSelected;
            set
            {
                var old = _treeElementSelected;

                if (Set(ref _treeElementSelected, value, nameof(TreeElementSelected)))
                {
                    if (old != null) old.Material = OriginalSelectedMaterial;
                    if (_treeElementSelected != null)
                    {
                        OriginalSelectedMaterial = _treeElementSelected.Material;
                        _treeElementSelected.Material = SelectedMaterial;
                    }
                }
            }
        }

        private int _tabSelectedIndex;
        public int TabSelectedIndex
        {
            get { return _tabSelectedIndex; }
            set
            {
                if (IsStructVisible && (_tabSelectedIndex != value)) OnSelectionElementCallback(null);
                _tabSelectedIndex = value;
                RaisePropertyChanged(nameof(IsProbeVisible));
                RaisePropertyChanged(nameof(IsStructVisible));
            }
        }

        public bool IsStructVisible => TabSelectedIndex == 0;

        public bool IsProbeVisible => TabSelectedIndex == 1;

        public Func<Point3D?> GetPointOnElement { get; set; }

        private bool _addProbePoint;
        public bool AddProbePoint
        {
            get { return _addProbePoint; }
            set
            {
                if (Set(ref _addProbePoint, value, nameof(AddProbePoint)))
                {
                    if (_addProbePoint) AddProbePlane = false;
                }
            }
        }

        private bool _addProbePlane;
        public bool AddProbePlane
        {
            get { return _addProbePlane; }
            set
            {
                if (Set(ref _addProbePlane, value, nameof(AddProbePlane)))
                {
                    if (_addProbePlane) AddProbePoint = false;
                }
            }
        }

        private bool _isPanelHolderVisible;
        public bool IsPanelHolderVisible
        {
            get { return _isPanelHolderVisible; }
            set
            {
                if (Set(ref _isPanelHolderVisible, value, nameof(IsPanelHolderVisible)))
                {
                    Messenger.Default.Send(new PanelHolderVisibilityChangedMessage() { Value = _isPanelHolderVisible });
                }
            }
        }

        private bool _isCollidersVisible;
        public bool IsCollidersVisible
        {
            get { return _isCollidersVisible; }
            set
            {
                if(Set(ref _isCollidersVisible, value, nameof(IsCollidersVisible)))
                {
                    Messenger.Default.Send(new CollidersVisibilityChangedMessage() { Value = _isCollidersVisible });
                }
            }
        }


        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _fileOpenArchiveCommand;
        public ICommand FileOpenArchiveCommand { get { return _fileOpenArchiveCommand ?? (_fileOpenArchiveCommand = new RelayCommand(() => FileOpenArchiveCommandImplementation())); } }

        private ICommand _fileOpenEnvironmentCommand;
        public ICommand FileOpenEnvironmentCommand { get { return _fileOpenEnvironmentCommand ?? (_fileOpenEnvironmentCommand = new RelayCommand(() => FileOpenEnvironmentCommandImplementation())); } }

        private ICommand _fileSaveEnvironmentCommand;
        public ICommand FileSaveEnvironmentCommand { get { return _fileSaveEnvironmentCommand ?? (_fileSaveEnvironmentCommand = new RelayCommand(() => FileSaveEnvironmentCommandImplementation())); } }

        private ICommand _toolingLoadCommand;
        public ICommand ToolingLoadCommand { get { return _toolingLoadCommand ?? (_toolingLoadCommand = new RelayCommand(() => ToolingLoadCommandImplementation())); } }

        private ICommand _toolingUnloadCommand;
        public ICommand ToolingUnloadCommand { get { return _toolingUnloadCommand ?? (_toolingUnloadCommand = new RelayCommand(() => ToolingUnloadCommandImplementation())); } }

        private ICommand _addPointDistanceCommand;
        public ICommand AddPointDistanceCommand { get { return _addPointDistanceCommand ?? (_addPointDistanceCommand = new RelayCommand(() => AddPointDistanceCommandImplementation(), CanExecuteAddPointDistanceCommand)); } }

        private ICommand _removeProbeDCommand;
        public ICommand RemoveProbeCommand { get { return _removeProbeDCommand ?? (_removeProbeDCommand = new RelayCommand(() => RemoveProbeCommandImplementation(), CanExecuteRemoveProbeCommand)); } }

        public MainViewModel(IHelixViewport3D viewport, bool addLigth = true) : base(viewport)
        {
            if(addLigth) Models.Add(new SpotHeadLight());

            PointSelectionCommand = new PointSelectionCommand(viewport.Viewport, OnSelectionElement);
            viewport.Viewport.InputBindings.Add(new MouseBinding(PointSelectionCommand, new MouseGesture(MouseAction.LeftClick)));

            //Models.Add(Grid);

            RegisterMessages();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<TracePointMessage>(this, OnTracePoint);
            Messenger.Default.Register<TracePointsMessage>(this, OnTracePoints);
            Messenger.Default.Register<TraceBoxMessage>(this, OnTraceBox);
            Messenger.Default.Register<SelectedStructItemChangedMessage>(this, OnSelectedStructItemChangedMessage);
            Messenger.Default.Register<RequestLinksListMessage>(this, OnRequestLinksListMessage);
        }

        private void OnSelectedStructItemChangedMessage(SelectedStructItemChangedMessage msg) => TreeElementSelected = msg.Value;

        private void OnTracePoints(TracePointsMessage msg)
        {
            if (Models.Count > 0)
            {
                var builder = new MeshBuilder();

                foreach (var p in msg.Points)
                {
                    builder.AddSphere(p, 10.0);
                }

                (Models[0] as ModelVisual3D).Children.Add(new MeshGeometryVisual3D() { MeshGeometry = builder.ToMesh(), Fill = msg.Brush });
            }
        }

        private void OnTraceBox(TraceBoxMessage msg)
        {
            if (Models.Count > 0)
            {
                var builder = new MeshBuilder();
                var p = msg.Box.Location;
                var x = new Vector3D(msg.Box.SizeX, 0.0, 0.0);
                var y = new Vector3D(0.0, msg.Box.SizeY, 0.0);
                var z = new Vector3D(0.0, 0.0, msg.Box.SizeZ);
                var s = 10.0;

                builder.AddBox(p, s, s, s);
                builder.AddBox(p + x, s, s, s);
                builder.AddBox(p + y, s, s, s);
                builder.AddBox(p + x +y, s, s, s);
                builder.AddBox(p + z, s, s, s);
                builder.AddBox(p + x + z, s, s, s);
                builder.AddBox(p + y + z, s, s, s);
                builder.AddBox(p + x + y + z, s, s, s);

                (Models[0] as ModelVisual3D).Children.Add(new MeshGeometryVisual3D() { MeshGeometry = builder.ToMesh(), Fill = msg.Brush });
            }
        }

        private void OnTracePoint(TracePointMessage msg)
        {
            if(Models.Count > 0)
            {
                var builder = new MeshBuilder();

                builder.AddSphere(msg.Point, 10.0);
                (Models[0] as ModelVisual3D).Children.Add(new MeshGeometryVisual3D() { MeshGeometry = builder.ToMesh(), Fill = msg.Brush });
            }

        }

        private void OnRequestLinksListMessage(RequestLinksListMessage msg)
        {
            var links = Links.Select((o) => MachineViewModels.Extensions.LinkExtensions.Convert(o))
                             .Cast<MachineModels.Models.Links.ILink>()
                             .ToList();

            msg?.SetLinks(links);
        }

        private void FileOpenCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "xml", AddExtension = true, Filter = "Machine struct |*.xml" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                //_isOpeningFile = true;

                LoadMachineFromFile(dlg.FileName);

                //UpdateModelBox();
                ResetCamera();
                //
                //_isOpeningFile = false;
                _lastMachProjectFile = dlg.FileName;
            }

        }

        private void LoadMachineFromFile(string machProjectFile)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

            using (var reader = new System.IO.StreamReader(machProjectFile))
            {
                var me = (MachineElement)serializer.Deserialize(reader);
                var mv = CreateMachineElementViewModel(me);

                Models.Add(mv);
                Machines.Add(mv);
                UpdateLinksView();
                UpdateToolchangeView();
                UpdatePanelHoldersView();
                UpdateInjectorsView();
                HookLinkForManageInserters();
            }
        }

        private void FileOpenArchiveCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "mcfgx", AddExtension = true, Filter = "Machine configuration|*.mcfgx" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                LoadMachineFromArchive(dlg.FileName);
            }
        }

        private void LoadMachineFromArchive(string machineFile)
        {
            var m = ZipArchiveHelper.Import(machineFile, (s) => _lastMachProjectFile = s);
            var mv = CreateMachineElementViewModel(m);

            Models.Add(mv);
            Machines.Add(mv);
            UpdateLinksView();
            UpdateToolchangeView();
            UpdatePanelHoldersView();
            ResetCamera();
        }

        private void FileOpenEnvironmentCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "env", AddExtension = true, Filter = "Environmet|*.env" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                if(ZipArchiveHelper.ImportEnvironment(dlg.FileName, out string machProjectFile, out string toolsFile, out string toolingFile))
                {
                    LoadMachineFromFile(machProjectFile);
                    ApplayTooling(toolingFile);
                    ResetCamera();
                }
            }
        }

        private void FileSaveEnvironmentCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = "env";
            dlg.AddExtension = true;
            dlg.Filter = "Environment|*.env";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                ZipArchiveHelper.ExportEnvironment(dlg.FileName, _lastMachProjectFile, _lastToolsFile, _lastToolingFile);
            }
        }

        private void UpdatePanelHoldersView() => Messenger.Default.Send(new Plugins.Common.Messages.PanelHolder.UpdateAvailablePanelHolderMessage());

        private void UpdateToolchangeView() => Messenger.Default.Send(new Plugins.Common.Messages.ToolChange.UpdateAvailableToolSinkListMessage());

        private void UpdateLinksView()
        {
            Messenger.Default.Send(new UpdateLinkViewModelsListMessage()
            {
                LinkViewModels = Links.ToList()
            });
        }

        private void UpdateInjectorsView() => Messenger.Default.Send(new Plugins.Common.Messages.Inserter.UpdateAvailableInjectorsMessage());

        private void HookLinkForManageInserters() => Messenger.Default.Send(new HookLinkForManageInsertersMessage());

        private void ToolingLoadCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tooling", AddExtension = true, Filter = "Tooling |*.tooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                Tooling tooling = ApplayTooling(dlg.FileName);

                Messenger.Default.Send(new Plugins.Common.Messages.ToolChange.UpdateAvailableToolsListMessage());

                _lastToolingFile = dlg.FileName;
                _lastToolsFile = tooling.ToolsFile;
            }
        }

        private Tooling ApplayTooling(string toolingFile)
        {
            Tooling tooling = null;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));

            using (var reader = new System.IO.StreamReader(toolingFile))
            {
                tooling = (Tooling)serializer.Deserialize(reader);
            }

            ApplyTooling(tooling);
            return tooling;
        }

        private ToolSet GetToolSet(string toolsFile)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

            using (var reader = new System.IO.StreamReader(toolsFile))
            {
                return (ToolSet)serializer.Deserialize(reader);
            }
        }

        private void ApplyTooling(Tooling tooling)
        {
            var toolSet = GetToolSet(tooling.ToolsFile);
            var td = toolSet.Tools.ToDictionary((t) => t.Name);

            foreach (var item in tooling.Units)
            {
                if(td.TryGetValue(item.ToolName, out Tool tool))
                {
                    if(tool.ToolType == ToolType.AngularTransmission)
                    {
                        if(td.TryGetValue((tool as AngolarTransmission).ToolName, out Tool subTool))
                        {
                            Messenger.Default.Send(new LoadAngolarTransmissionMessage() { ToolHolderId = item.ToolHolderId, Tool = tool, SubTool = subTool });
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                    else
                    {
                        Messenger.Default.Send(new LoadToolMessage() { ToolHolderId = item.ToolHolderId, Tool = tool });
                    }
                }
            }
        }

        private void ToolingUnloadCommandImplementation()
        {
            Messenger.Default.Send(new Plugins.Common.Messages.ToolChange.UnloadAllToolsMessage());
            Messenger.Default.Send(new UnloadToolMessage());
        }

        private void RemoveProbeCommandImplementation() => Messenger.Default.Send(new RemoveSelectedProbeMessage());

        private void AddPointDistanceCommandImplementation() => Messenger.Default.Send(new AddPointDistanceProbeMessage());

        private bool CanExecuteAddPointDistanceCommand()
        {
            bool result = false;
            Messenger.Default.Send(new CanExecuteAddPointDistanceMessage() { SetValue = (b) => result = b });            
            return result;
        }

        private bool CanExecuteRemoveProbeCommand()
        {
            bool result = false;
            Messenger.Default.Send(new CanExecuteRemoveProbeMessage() { SetValue = (b) => result = b });
            return result;
        }

        private MachineElementViewModel CreateMachineElementViewModel(MachineElement me, MachineElementViewModel parent = null)
        {
            var mi = new ModelImporter();
            var m = string.IsNullOrEmpty(me.ModelFile) ? null : mi.Load(me.ModelFile, null);
            var mmt = new MatrixTransform3D(me.TrasformationMatrix3D.Convert());
            var material = MaterialHelper.CreateMaterial(me.Color.Convert()).Clone();
            var vm = CreateViewModel(me);

            vm.Name = me.Name;
            vm.MeshGeometry = m?.GetMeshGeometry3D();
            vm.Material = material;
            vm.BackMaterial = MaterialHelper.CreateMaterial(System.Windows.Media.Colors.Transparent);
            vm.Parent = parent;

            var tg = new Transform3DGroup();

            if (me.LinkToParentType != MachineModels.Enums.LinkType.Static)
            {
                var link = me.LinkToParentData.Convert(vm, tg, mmt.Transform(new Point3D()));

                Links.Add(link);
                vm.LinkToParent = link;
            }

            tg.Children.Add(mmt);
            //tg.Children.Add(mmt.GetRotationTransform());
            //tg.Children.Add(mmt.GetTraslationTrasform());
            vm.Transform = tg;

            foreach (var item in me.Children)
            {
                var c = CreateMachineElementViewModel(item, vm);
                vm.Children.Add(c);
            }

            if (me.HasPanelHolder) vm.Children.Add(me.PanelHolder.ToViewModel(vm));
            if (me.ColiderType != ColliderGeometry.None) vm.Children.Add(me.Collider.ToViewModel(vm));
            if (me.InserterType != InserterType.None) vm.Children.Add(me.Inserter.ToViewModel(vm));

            return vm;
        }

        private MachineElementViewModel CreateViewModel(MachineElement me)
        {
            MachineElementViewModel vm = null;

            switch (me.ToolHolderType)
            {
                case MachineModels.Enums.ToolHolderType.None:
                    vm = new MachineElementViewModel();
                    break;
                case MachineModels.Enums.ToolHolderType.Static:
                    vm = new StaticToolHolderViewModel().UpdateFromModel(me.ToolHolderData);
                    break;
                case MachineModels.Enums.ToolHolderType.AutoSource:
                    vm = new AutoSourceToolHolderViewModel().UpdateFromModel(me.ToolHolderData);
                    break;
                case MachineModels.Enums.ToolHolderType.AutoSink:
                    vm = new AutoSinkToolHolderViewModel().UpdateFromModel(me.ToolHolderData);
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }

            return vm;
        }

        private MeshGeometry3D CheckSelected(Model3D model)
        {
            MeshGeometry3D mesh = null;

            if((model != null) && 
                (model is GeometryModel3D geoModel) &&
                (geoModel.Geometry is MeshGeometry3D mg))
            {
                mesh = mg;
            }

            return mesh;
        }

        private void OnSelectionElement(object sender, ModelsSelectedEventArgs e)
        {
            if(e.SelectedModels != null && e.SelectedModels.Count > 0)
            {
                if (IsStructVisible)
                {
                    OnSelectionElementToHilight(sender, e);
                }
                else if (IsProbeVisible)
                {
                    OnSelectionElementToProbing(sender, e);
                }
            }
        }

        private void OnSelectionElementToHilight(object sender, ModelsSelectedEventArgs e)
        {
            var mesh = CheckSelected(e.SelectedModels[0]);

            if (mesh != null)
            {
                Messenger.Default.Send(new SelectionElementMessage()
                {
                    Selected = mesh,
                    SetSelected = (m) => OnSelectionElementCallback(m)
                });
            }
        }

        private void OnSelectionElementToProbing(object sender, ModelsSelectedEventArgs e)
        {
            var p = GetPointOnElement();
            var mesh = CheckSelected(e.SelectedModels[0]);

            if (p.HasValue && mesh != null)
            {

                MachineElementViewModel vm = null;

                Messenger.Default.Send(new GetMachineElementByClickGeometryMessage()
                {
                    Selected = mesh,
                    SetSelected = (m) => vm = m
                });

                if (vm != null)
                {
                    ProbeViewModel probe = null;

                    if (AddProbePoint) probe = PointProbeViewModel.Create(vm, p.Value);
                    else if (AddProbePlane) probe = PlaneProbeViewModel.Create(vm, p.Value);

                    if (probe != null)
                    {
                        vm.Children.Add(probe);
                        Messenger.Default.Send(new AddProbeMessage() { Probe = probe });
                    }
                }
            }
        }

        private void OnSelectionElementCallback(MachineElementViewModel m)
        {
            bool selectNew = m != null;

            if (TreeElementSelected != null)
            {
                if (selectNew) selectNew = !object.ReferenceEquals(m, TreeElementSelected);
                TreeElementSelected.IsSelected = false;
            }

            if (selectNew)
            {
                m.RequestTreeviewVisibility();
                m.IsSelected = true;
            }
        }

    }
}

