using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolEditor.Messages;

namespace ToolEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static int _newToolIndex = 1;

        public ObservableCollection<RowToolViewModel> Tools { get; set; } = new ObservableCollection<RowToolViewModel>();

        private RowToolViewModel _selected;
        public RowToolViewModel Selected
        {
            get { return _selected; }
            set
            {
                if (Set(ref _selected, value, nameof(Selected)))
                {
                    Messenger.Default.Send(new SelectedToolChanged<Tool>() { Tool = (_selected != null) ? _selected.GetModel() : null });
                }
            }
        }

        private ICommand _openFile;
        public ICommand OpenFile => _openFile ?? (_openFile = new RelayCommand(() => OpenFileImpl()));

        private ICommand _saveFile;
        public ICommand SaveFile => _saveFile ?? (_saveFile = new RelayCommand(() => SaveFileImpl(), () => Tools.Count > 0));

        private ICommand _removeTool;
        public ICommand RemoveTool => _removeTool ?? (_removeTool = new RelayCommand(() => DelSimpleToolImpl(), () => Selected != null));

        private ICommand _addSimpleTool;
        public ICommand AddSimpleTool => _addSimpleTool ?? (_addSimpleTool = new RelayCommand(() => AddSimpleToolImpl()));

        private ICommand _addPointedTool;
        public ICommand AddPointedTool => _addPointedTool ?? (_addPointedTool = new RelayCommand(() => AddPointedToolImpl()));

        private ICommand _addTwoSectionTool;
        public ICommand AddTwoSectionTool => _addTwoSectionTool ?? (_addTwoSectionTool = new RelayCommand(() => AddTwoSectionToolImpl()));

        private ICommand _addDiskTool;
        public ICommand AddDiskTool => _addDiskTool ?? (_addDiskTool = new RelayCommand(() => AddDiskToolImpl()));

        private ICommand _addCountersinkTool;
        public ICommand AddCountersinkTool => _addCountersinkTool ?? (_addCountersinkTool = new RelayCommand(() => AddCountersinkToolImpl()));

        private ICommand _addDiskOnConeTool;
        public ICommand AddDiskOnConeTool => _addDiskOnConeTool ?? (_addDiskOnConeTool = new RelayCommand(() => AddDiskOnConeToolImpl()));

        private ICommand _addAngularTransmission;
        public ICommand AddAngularTransmission => _addAngularTransmission ?? (_addAngularTransmission = new RelayCommand(() => AddAngularTransmissionImpl()));

        private void OpenFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tools", AddExtension = true, Filter = "Tools |*.tools" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var ts = (ToolSet)serializer.Deserialize(reader);

                    if (ts != null)
                    {
                        Selected = null;
                        Tools = new ObservableCollection<RowToolViewModel>(ts.Tools.Select((t) => new RowToolViewModel(t)));
                        RaisePropertyChanged(nameof(Tools));
                    }
                }
            }
        }

        private void SaveFileImpl()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "tools", AddExtension = true, Filter = "Tools |*.tools" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var toolset = new ToolSet() { Tools = Tools.Select((r) => r.GetModel()).ToList() };
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    serializer.Serialize(writer, toolset);
                }
            }
        }

        private void AddSimpleToolImpl()
        {
            AddBaseTool(new SimpleTool()
            {
                Diameter = 10.0,
                Length = 60.0,
                UsefulLength = 45.0
            });
        }

        private void DelSimpleToolImpl()
        {
            Tools.Remove(Selected);
            Selected = null;
        }

        private void AddPointedToolImpl()
        {
            AddBaseTool(new PointedTool()
            {
                ConeHeight = 8.0,
                Diameter = 10.0,
                StraightLength = 52.0,
                UsefulLength = 40.0
            });
        }

        private void AddTwoSectionToolImpl()
        {
            AddBaseTool(new TwoSectionTool()
            {
                Diameter1 = 10.0,
                Length1 = 40.0,
                Diameter2 = 35.0,
                Length2 = 10.0,
                UsefulLength = 10.0
            });
        }

        private void AddDiskToolImpl()
        {
            AddBaseTool(new DiskTool()
            {
                BodyThickness = 2.0,
                CuttingRadialThickness = 4.0,
                CuttingThickness = 3.2,
                Diameter = 120.0,
                RadialUsefulLength = 35.0
            });
        }

        private void AddCountersinkToolImpl()
        {
            AddBaseTool(new CountersinkTool()
            {
                Diameter1 = 8.0,
                Diameter2 = 16.0,
                Length1 = 20.0,
                Length2 = 10.0,
                Length3 = 20.0,
                UsefulLength = 25
            });
        }

        private void AddDiskOnConeToolImpl()
        {
            AddBaseTool(new DiskOnConeTool()
            {
                BodyThickness = 2.0,
                CuttingRadialThickness = 4.0,
                CuttingThickness = 3.2,
                Diameter = 120.0,
                RadialUsefulLength = 35.0,
                PostponemntDiameter = 10.0,
                PostponemntLength = 104.0
            });
        }

        private void AddAngularTransmissionImpl()
        {
            AddBaseTool(new AngolarTransmission());
        }

        private void AddBaseTool(Tool tool)
        {
            tool.Name = $"Tool {GetNewToolIndex()}";
            tool.Description = $"New tool named {tool.Name}";

            AddTool(tool);
        }

        private void AddTool(Tool tool)
        {
            var vm = new RowToolViewModel(tool);

            Tools.Add(vm);

            Selected = vm;
        }

        private static int GetNewToolIndex() => _newToolIndex++;
    }
}
