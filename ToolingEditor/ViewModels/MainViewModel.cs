using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Models;
using MachineModels.Models.Tooling;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolingEditor.Messages;

namespace ToolingEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int _toolsLoaded = 0;

        private MachineElements.IToolHolderManagement _toolHolder;
        public MachineElements.IToolHolderManagement ToolHolder
        {
            get { return _toolHolder; }
            set
            {
                if(Set(ref _toolHolder, value, nameof(ToolHolder)))
                {
                    RaisePropertyChanged(nameof(CanLoadTool));
                    RaisePropertyChanged(nameof(CanUnloadTool));
                }
            }
        }

        private Tool _tool;
        public Tool Tool
        {
            get { return _tool; }
            set
            {
                if(Set(ref _tool, value, nameof(Tool)))
                {
                    RaisePropertyChanged(nameof(CanLoadTool));
                    RaisePropertyChanged(nameof(CanUnloadTool));
                }
            }
        }

        public bool CanLoadTool => (_tool != null) && (_toolHolder != null) && !_toolHolder.IsToolPresent;

        public bool CanUnloadTool => (_toolHolder != null) && _toolHolder.IsToolPresent;

        public string MachineFile { get; set; }

        public string ToolsFile { get; set; }


        private ICommand _openFile;
        public ICommand OpenFile => _openFile ?? (_openFile = new RelayCommand(OpenFileImpl));

        private ICommand _saveFile;
        public ICommand SaveFile => _saveFile ?? (_saveFile = new RelayCommand(SaveFileImpl, () => !string.IsNullOrEmpty(ToolsFile) && !string.IsNullOrEmpty(MachineFile)));

        private ICommand _openMachineFile;
        public ICommand OpenMachineFile => _openMachineFile ?? (_openMachineFile = new RelayCommand(OpenMachineFileImpl));

        private ICommand _openToolsFile;
        public ICommand OpenToolsFile => _openToolsFile ?? (_openToolsFile = new RelayCommand(OpenToolsFileImpl));

        private ICommand _loadTool;
        public ICommand LoadTool => _loadTool ?? (_loadTool = new RelayCommand(LoadToolImpl, () => CanLoadTool));

        private ICommand _unloadTool;
        public ICommand UnloadTool => _unloadTool ?? (_unloadTool = new RelayCommand(UnloadToolImpl, () => CanUnloadTool));

        private ICommand _unloadAllTool;
        public ICommand UnloadAllTool => _unloadAllTool ?? (_unloadAllTool = new RelayCommand(UnloadAllToolImpl, () => _toolsLoaded > 0));

        public MainViewModel()
        {
            Messenger.Default.Register<ToolSelectionChanged>(this, OnToolSelectionChanged);
            Messenger.Default.Register<ToolHolderSelectionChanged>(this, OnToolHolderSelectionChanged);
        }

        private void OpenFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tooling", AddExtension = true, Filter = "Tooling |*.tooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var tooling = (Tooling)serializer.Deserialize(reader);

                    if (tooling != null)
                    {
                        OpenMachineFileImpl(tooling.MachineFile);
                        OpenToolsFileImpl(tooling.ToolsFile);
                        Messenger.Default.Send(new SetActiveTooling() { Tooling = tooling });
                    }
                }
            }
        }

        private void SaveFileImpl()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "tooling", AddExtension = true, Filter = "Tooling |*.tooling" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                Tooling tooling = null;

                Messenger.Default.Send(new GetActiveTooling() { Set = (t) => tooling = t });

                if(tooling != null)
                {
                    tooling.ToolsFile = ToolsFile;
                    tooling.MachineFile = MachineFile;

                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));

                    using (var writer = new System.IO.StreamWriter(dlg.FileName))
                    {
                        serializer.Serialize(writer, tooling);
                    }
                }
                else
                {
                    throw new Exception("Tooling data is not correct!");
                }
            }
        }

        private void OpenMachineFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "xml", AddExtension = true, Filter = "Machine struct |*.xml" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                OpenMachineFileImpl(dlg.FileName);
            }

        }

        private void OpenMachineFileImpl(string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var me = (MachineElement)serializer.Deserialize(reader);

                if (me != null)
                {
                    me = FilterMachineElementsWithoutToolholder(me);

                    if (me != null)
                    {
                        Messenger.Default.Send(new MachineElementsLoaded() { Elements = me });
                        MachineFile = fileName;
                    }
                }
            }
        }

        private void OpenToolsFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "tools", AddExtension = true, Filter = "Tools |*.tools" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                OpenToolsFileImpl(dlg.FileName);
            }
        }

        private void OpenToolsFileImpl(string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var ts = (ToolSet)serializer.Deserialize(reader);

                if (ts != null)
                {
                    Messenger.Default.Send(new ToolSetLoaded() { ToolSet = ts });
                    ToolsFile = fileName;
                }
            }
        }

        private void LoadToolImpl()
        {
            _toolHolder.LoadTool(_tool);
            _toolsLoaded++;
        }

        private void UnloadToolImpl()
        {
            _toolHolder.UnloadTool();
            _toolsLoaded--;
        }

        private void UnloadAllToolImpl()
        {
            Messenger.Default.Send(new UnloadAllTools());
            _toolsLoaded = 0;
        }

        private MachineElement FilterMachineElementsWithoutToolholder(MachineElement me)
        {
            MachineElement result = null;
            var children = new List<MachineElement>();

            if(me.Children != null && me.Children.Count > 0)
            {
                foreach (var item in me.Children)
                {
                    var e = FilterMachineElementsWithoutToolholder(item);

                    if (e != null) children.Add(e);
                }

                if(children.Count > 0)
                {
                    result = me;
                    result.Children = children;
                }
            }
            else if(me.ToolHolderType != MachineModels.Enums.ToolHolderType.None)
            {
                result = me;
            }

            return result;
        }

        private void OnToolHolderSelectionChanged(ToolHolderSelectionChanged msg) => ToolHolder = msg.ToolHolder;

        private void OnToolSelectionChanged(ToolSelectionChanged msg) => Tool = msg.Tool;
    }
}
