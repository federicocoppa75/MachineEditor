using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineEditor.Extensions;
using MachineModels.IO;
using MachineModels.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using ViewModels.MachineViewModels;

namespace MachineEditor.ViewModels
{
    public class MainViewModel : Machine3DModelsViewModelBase
    {
        private static int _newElementIndex = 0;

        private bool _isOpeningFile = false;

        public ObservableCollection<MachineElementViewModel> Machines { get; set; } = new ObservableCollection<MachineElementViewModel> { new MachineElementViewModel() { Name = "root" } };

        private MachineElementViewModel _selected;
        public MachineElementViewModel Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged(nameof(Selected));
                RaiseCommandEnableChangedBySectionChanged();
            }
        }

        public Rect3D ModelsBox { get; set; }

        public GridLinesVisual3D Grid { get; set; } = new GridLinesVisual3D();

        public bool IsTraslationActive { get; set; }

        public bool IsRotationActive { get; set; }

        public bool IsGridVisible => Models.Contains(Grid);

        private bool _isSpindlesDirectionVisibile;

        public bool IsSpidlesDirectionVisible
        {
            get { return _isSpindlesDirectionVisibile; }
            set
            {
                if (Set(ref _isSpindlesDirectionVisibile, value, nameof(IsSpidlesDirectionVisible)))
                {
                    Messenger.Default.Send(new Messages.ChangeSpindleDirectionVisibility() { Visible = _isSpindlesDirectionVisibile });
                }
            }
        }

        private bool _isCollidersVisible;

        public bool IsCollidersVisible
        {
            get => _isCollidersVisible; 
            set
            {
                if(Set(ref _isCollidersVisible, value, nameof(IsCollidersVisible)))
                {
                    Messenger.Default.Send(new Messages.ChangeCollidersVisibility() { Visible = _isCollidersVisible });
                }
            }
        }

        private bool _isPanelHolderVisible;

        public bool IsPanelHolderVisible
        {
            get => _isPanelHolderVisible; 
            set
            {
                if(Set(ref _isPanelHolderVisible, value, nameof(IsPanelHolderVisible)))
                {
                    Messenger.Default.Send(new Messages.ChangePanelHolderVisibility() { Visible = _isPanelHolderVisible });
                }
            }
        }

        private bool _isInserterVisibile;

        public bool IsInserterVisible
        {
            get => _isInserterVisibile; 
            set
            {
               if(Set(ref _isInserterVisibile, value, nameof(IsInserterVisible)))
                {
                    Messenger.Default.Send(new Messages.ChangeInserterVisibility() { Visible = _isInserterVisibile });
                }
            }
        }


        private LightSetup _light;
        public LightSetup Light
        {
            get => _light;
            set
            {
                if ((_light == null) || (_light.GetType() != value.GetType()))
                {
                    if (_light != null) Models.Remove(_light);
                    _light = value;
                    Models.Add(_light);
                    RaisePropertyChanged(nameof(IsSunLight));
                    RaisePropertyChanged(nameof(IsDefaultLights));
                    RaisePropertyChanged(nameof(IsSpotHeadLight));
                }
            }
        }

        public bool IsSunLight
        {
            get => Light is SunLight;
            set
            {
                if (value) Light = new SunLight();
            }
        }

        public bool IsDefaultLights
        {
            get => Light is DefaultLights;
            set
            {
                if (value) Light = new DefaultLights();
            }
        }

        public bool IsSpotHeadLight
        {
            get => Light is SpotHeadLight;
            set
            {
                if (value) Light = new SpotHeadLight();
            }
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _fileImportFromArchiveCommand;
        public ICommand FileImportFromArchiveCommand { get { return _fileImportFromArchiveCommand ?? (_fileImportFromArchiveCommand = new RelayCommand(() => FileImportFromArchiveCommandImplementation())); } }

        private ICommand _fileExportToArchiveCommand;
        public ICommand FileExportToArchiveCommand { get { return _fileExportToArchiveCommand ?? (_fileExportToArchiveCommand = new RelayCommand(() => FileExportToArchiveCommandImplementation())); } }

        private ICommand _fileSaveCommand;
        public ICommand FileSaveCommand { get { return _fileSaveCommand ?? (_fileSaveCommand = new RelayCommand(() => FileSaveCommandImplementation())); } }

        private ICommand _addCommand;
        public ICommand AddCommand { get { return _addCommand ?? (_addCommand = new RelayCommand(() => AddCommandImplementation())); } }

        private ICommand _delCommand;
        public ICommand DelCommand { get { return _delCommand ?? (_delCommand = new RelayCommand(() => DelCommandImplementation(), () => DelCommandEnableFunc())); } }

        private ICommand _traslateCommand;
        public ICommand TraslateCommand { get { return _traslateCommand ??
                    (_traslateCommand = new RelayCommand<Manipolators.TranslateManipulator[]>((o) =>
                    {
                        if (IsTraslationActive)
                        {
                            BindTraslators(Selected, o);
                        }
                        else
                        {
                            UnBindTraslators(o);
                        }
                    },
                    (o) => Selected != null && Selected.Model != null)); } }

        private ICommand _rotateCommand;
        public ICommand RotateCommand { get { return _rotateCommand ??
                    (_rotateCommand = new RelayCommand<RotateManipulator[]>((o) =>
                    {
                        if (IsRotationActive)
                        {
                            BindRotators(Selected.Model as ModelVisual3D, o);
                        }
                        else
                        {
                            UnBindRotators(o);
                        }
                    },
                    (o) => Selected != null && Selected.Model != null)); } }

        private ICommand _traslateXPosCommand;
        public ICommand TraslateXPosCommand => GetOrCreateTraslateCommand(ref _traslateXPosCommand, (d) => TraslateCommandImpl(d, new Vector3D(1.0, 0.0, 0.0)));

        private ICommand _traslateXNegCommand;
        public ICommand TraslateXNegCommand => GetOrCreateTraslateCommand(ref _traslateXNegCommand, (d) => TraslateCommandImpl(d, new Vector3D(-1.0, 0.0, 0.0)));

        private ICommand _traslateYPosCommand;
        public ICommand TraslateYPosCommand => GetOrCreateTraslateCommand(ref _traslateYPosCommand, (d) => TraslateCommandImpl(d, new Vector3D(0.0, 1.0, 0.0)));

        private ICommand _traslateYNegCommand;
        public ICommand TraslateYNegCommand => GetOrCreateTraslateCommand(ref _traslateYNegCommand, (d) => TraslateCommandImpl(d, new Vector3D(0.0, -1.0, 0.0)));

        private ICommand _traslateZPosCommand;
        public ICommand TraslateZPosCommand => GetOrCreateTraslateCommand(ref _traslateZPosCommand, (d) => TraslateCommandImpl(d, new Vector3D(0.0, 0.0, 1.0)));

        private ICommand _traslateZNegCommand;
        public ICommand TraslateZNegCommand => GetOrCreateTraslateCommand(ref _traslateZNegCommand, (d) => TraslateCommandImpl(d, new Vector3D(0.0, 0.0, -1.0)));

        private ICommand _toggleGridVisibleStateCommand;
        public ICommand ToggleGridVisibleStateCommand { get { return _toggleGridVisibleStateCommand ?? (_toggleGridVisibleStateCommand = new RelayCommand(() => ToggleGridVisibleState())); } }

        static private ICommand GetOrCreateCommand<T>(ref ICommand command, Action<T> commandAction, Func<T, bool> commandCanExecuteFunc)
        {
            return command ?? (command = new RelayCommand<T>(commandAction, commandCanExecuteFunc));
        }

        private ICommand GetOrCreateTraslateCommand(ref ICommand command, Action<double> commamdAction) => GetOrCreateCommand(ref command, commamdAction, (d) => IsTraslationActive && (Selected != null));

        private void TraslateCommandImpl(double step, Vector3D direction) => TraslateCommandImpl(direction * step);

        private void TraslateCommandImpl(Vector3D traslation) => TraslateCommandImpl(traslation.X, traslation.Y, traslation.Z);

        private void TraslateCommandImpl(double stepX, double stepY, double stepZ, bool notify = true)
        {
            var tg = new Transform3DGroup();
            tg.Children.Add(Selected.Model.Transform);
            tg.Children.Add(new TranslateTransform3D(stepX, stepY, stepZ));
            Selected.Model.Transform = tg;

            if (notify) Selected.NotifyModelLOcationChanged();
        }

        private void ToggleGridVisibleState()
        {
            if(IsGridVisible)
            {
                Models.Remove(Grid);
            }
            else
            {
                Models.Add(Grid);
            }

            RaisePropertyChanged(nameof(IsGridVisible));
        }

        public MainViewModel(IHelixViewport3D viewport) : base(viewport)
        {
            Light = new SpotHeadLight();

            Models.Add(Grid);

            RegisterMessages();
        }

        private static string GetNewElementName() => $"Untittled ({_newElementIndex++})";

        private static string GetNewToolHoldertName() => $"ToolHolder ({_newElementIndex++})";

        private void FileOpenCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "xml", AddExtension = true, Filter ="Machine struct |*.xml" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                _isOpeningFile = true;

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var m = (MachineElement)serializer.Deserialize(reader);

                    if (m != null)
                    {
                        SetAbsoluterReference(m);
                        Machines.Clear();
                        Machines.Add(m.ToViewModel());
                    }
                }


                UpdateModelBox();
                ResetCamera();

                _isOpeningFile = false;
            }

        }

        private void FileSaveCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "xml", AddExtension = true, Filter = "Machine struct |*.xml" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    var m = Machines[0].ToModel();

                    SetRelativeReference(m);
                    serializer.Serialize(writer, m);
                }
            }
        }

        private void SetRelativeReference(MachineElement machineElement)
        {
            var tm = machineElement.TrasformationMatrix3D;

            foreach (var item in machineElement.Children)
            {
                SetRelativeReference(item);

                var itm = item.TrasformationMatrix3D;

                itm.ToRelativeReference(tm);
            }
        }

        private void SetAbsoluterReference(MachineElement machineElement)
        {
            var tm = machineElement.TrasformationMatrix3D;

            foreach (var item in machineElement.Children)
            {
                var itm = item.TrasformationMatrix3D;

                itm.ToAbsoluteReference(tm);
                SetAbsoluterReference(item);
            }
        }

        private void FileExportToArchiveCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = "mcfgx";
            dlg.AddExtension = true;
            dlg.Filter = "Machine configuration|*.mcfgx";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var m = Machines[0].ToModel();
                SetRelativeReference(m);
                ZipArchiveHelper.Export(m, dlg.FileName);
            }
        }

        private void FileImportFromArchiveCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = "mcfgx";
            dlg.Filter = "Machine configuration|*.mcfgx";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var m = ZipArchiveHelper.Import(dlg.FileName);

                if (m != null)
                {
                    SetAbsoluterReference(m);
                    Machines.Clear();
                    Machines.Add(m.ToViewModel());
                }
            }
        }

        private void AddCommandImplementation()
        {
            if (Selected != null)
            {
                Selected.Children.Add(new MachineElementViewModel() { Name = GetNewElementName(), Parent = Selected });
            }
        }

        private void DelCommandImplementation()
        {
            if (Selected != null && Selected.Parent != null)
            {
                var parent = Selected.Parent;
                Selected.ModelFile = string.Empty;
                parent.Children.Remove(Selected);
                Selected = null;
            }
        }

        private bool DelCommandEnableFunc() => Selected != null && Selected.Children.Count == 0;

        private void RaiseCommandEnableChangedBySectionChanged()
        {
            (_delCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (_traslateCommand as RelayCommand<Manipolators.TranslateManipulator[]>)?.RaiseCanExecuteChanged();
            (_rotateCommand as RelayCommand<RotateManipulator[]>)?.RaiseCanExecuteChanged();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Messages.AddNewModel>(this, AddNewModel);
            Messenger.Default.Register<Messages.ChangeModel>(this, ChangeModel);
            Messenger.Default.Register<Messages.ShowModel>(this, ShowModel);
            Messenger.Default.Register<Messages.HideModel>(this, HideModel);
            Messenger.Default.Register<Messages.ChangeModelLocation>(this, ChangeModelLocation);
            Messenger.Default.Register<Messages.GetSpindleDirectionVisibility>(this, GetSpindlesVisibility);
            Messenger.Default.Register<Messages.GetColliderVisibility>(this, GetColliderVisibility);
            Messenger.Default.Register<Messages.GetPanelHolderVisibility>(this, GetPanelHolderVisibility);
            Messenger.Default.Register<Messages.GetInserterVisibility>(this, GetInserterVisibility);
        }

        private void AddNewModel(Messages.AddNewModel msg)
        {
            Models.Add(msg.Model);

            if(!_isOpeningFile)
            {
                UpdateModelBox();
                ResetCamera();
            }
            
        }

        private void ChangeModel(Messages.ChangeModel msg)
        {
            if(msg.OldModel != null) Models.Remove(msg.OldModel);
            if(msg.Model != null) Models.Add(msg.Model);

            UpdateModelBox();
            ResetCamera();
        }


        private void HideModel(Messages.HideModel msg)
        {
            Models.Remove(msg.Model);
        }

        private void ShowModel(Messages.ShowModel msg)
        {
            Models.Add(msg.Model);
        }

        private void ChangeModelLocation(Messages.ChangeModelLocation msg)
        {
            TraslateCommandImpl(msg.DX, msg.DY, msg.DZ, false);
        }

        private void GetSpindlesVisibility(Messages.GetSpindleDirectionVisibility msg)
        {
            msg?.GetVisibility?.Invoke(_isSpindlesDirectionVisibile);
        }

        private void GetColliderVisibility(Messages.GetColliderVisibility msg)
        {
            msg?.GetVisibility?.Invoke(_isCollidersVisible);
        }

        private void GetPanelHolderVisibility(Messages.GetPanelHolderVisibility msg)
        {
            msg?.GetVisibility?.Invoke(_isPanelHolderVisible);
        }

        private void GetInserterVisibility(Messages.GetInserterVisibility msg)
        {
            msg?.GetVisibility?.Invoke(_isInserterVisibile);
        }

        private void UpdateModelBox()
        {
            foreach (var m in Models)
            {
                if (m is LightSetup || m is GridLinesVisual3D || (m == null)) continue;

                var mv = m as ModelVisual3D;
                Rect3D box = mv.Content.Bounds;

                box.Location = mv.Transform.Transform(box.Location);

                if (ModelsBox.SizeX == 0 && ModelsBox.SizeY == 0 && ModelsBox.SizeZ == 0)
                {
                    ModelsBox = box;
                }
                else
                {
                    ModelsBox = Rect3D.Union(ModelsBox, box);
                }
            }

            Grid.Center = ModelsBox.Location + new Vector3D(ModelsBox.SizeX, ModelsBox.SizeY, ModelsBox.SizeZ) / 2.0;
            Grid.Length = Math.Round(ModelsBox.SizeX * 1.2);
            Grid.Width = Math.Round(ModelsBox.SizeY * 1.2);
            Grid.MajorDistance = GetGridMajorDistance(Grid.Length, Grid.Width);
            Grid.MinorDistance = Grid.MajorDistance / 5.0;
            Grid.Thickness = Grid.MinorDistance / 25.0;
        }

        private double GetGridMajorDistance(double length, double width)
        {
            double d = (length > width) ? length : width;
            double e = Math.Log10(d);

            return Math.Pow(10.0, Math.Round(e) - 1);
        }

        private void BindTraslators(MachineElementViewModel vm, Manipolators.TranslateManipulator[] traslators)
        {
            for (int i = 0; i < 3; i++) BindTraslator(vm, traslators[i]);
        }

        private void BindTraslator(MachineElementViewModel vm, Manipolators.TranslateManipulator traslator)
        {
            var a = new double[] { vm.SizeX, vm.SizeY, vm.SizeZ };
            var length = a.Max() / 2.0;
            var diameter = length / 10.0;

            traslator.Length = length;
            traslator.Diameter = diameter;
            traslator.Bind(vm);

            Models.Add(traslator);
        }

        private void UnBindTraslators(Manipolators.TranslateManipulator[] traslators)
        {
            for (int i = 0; i < 3; i++) UnBindTraslators(traslators[i]);
        }

        private void UnBindTraslators(Manipolators.TranslateManipulator traslator)
        {
            traslator.UnBind();
            Models.Remove(traslator);
        }

        private void BindRotators(ModelVisual3D model, RotateManipulator[] rotators)
        {
            for (int i = 0; i < 3; i++) BindRotator(model, rotators[i]);
        }

        private void BindRotator(ModelVisual3D model, RotateManipulator rotator)
        {
            var a = new double[] { model.Content.Bounds.SizeX, model.Content.Bounds.SizeY, model.Content.Bounds.SizeZ };
            var diameter = a.Max() / 2.0;
            var innerDiameter = diameter * 0.9;

            rotator.Diameter = diameter;
            rotator.InnerDiameter = innerDiameter;
            rotator.Bind(model);
            Models.Add(rotator);
        }

        private void UnBindRotators(RotateManipulator[] rotators)
        {
            for (int i = 0; i < 3; i++) UnBindRotator(rotators[i]);
        }

        private void UnBindRotator(RotateManipulator rotator)
        {
            rotator.UnBind();
            Models.Remove(rotator);
        }

    }
}
