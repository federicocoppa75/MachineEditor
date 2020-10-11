using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineSteps.Editor.Messages;
using MachineSteps.Models;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineSteps.Editor.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private static int _index = 0;

        public ObservableCollection<MachineStep> Steps { get; set; } = new ObservableCollection<MachineStep>();
 
        private MachineStep _selected;
        public MachineStep Selected
        {
            get { return _selected; }
            set
            {
                if(Set(ref _selected, value, nameof(Selected)))
                {
                    MessengerInstance.Send(new MachineStepSelectionChangedMessage() { MachineStep = _selected });
                }
            }
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand => _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(FileOpenCommandImpl));

        private ICommand _fileSaveCommand;
        public ICommand FileSaveCommand => _fileSaveCommand ?? (_fileSaveCommand = new RelayCommand(FileSaveCommandImpl));

        private ICommand _addStepCommand;
        public ICommand AddStepCommand => _addStepCommand ?? (_addStepCommand = new RelayCommand(AddStepCommandImpl));

        private ICommand _delStepCommand;
        public ICommand DelStepCommand => _delStepCommand ?? (_delStepCommand = new RelayCommand(DelStepCommandImpl, () => Selected != null));

        public MainViewModel()
        {
        }

        private void DelStepCommandImpl()
        {
            Steps.Remove(Selected);
            Selected = null;
        }

        private void AddStepCommandImpl()
        {
            Steps.Add(new MachineStep()
            {
                Id = _index,
                Name = $"Step {_index++}",
                Actions = new List<Models.Actions.BaseAction>()
            });
        }

        private void FileSaveCommandImpl()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "msteps", AddExtension = true, Filter = "Machine steps |*.msteps" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var doc = new MachineStepsDocument() { Steps = Steps.ToList() };
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    serializer.Serialize(writer, doc);
                }
            }            
        }

        private void FileOpenCommandImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "msteps", AddExtension = true, Filter = "Machine steps |*.msteps" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var doc = (MachineStepsDocument)serializer.Deserialize(reader);

                    Steps.Clear();

                    if (doc != null)
                    {
                        foreach (var item in doc.Steps)
                        {
                            Steps.Add(item);
                        }
                    }
                }
            }
        }
    }
}
