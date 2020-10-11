using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineSteps.Models;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoInterpreter.Istructions;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Models;
using MachineSteps.Plugins.IsoInterpreter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MachineSteps.IsoInterpreter.SimpleApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int _lastSelectedIndex = -1;

        private bool _enableGiveSelectedStep;

        private bool _enableGiveStepRowNumber;

        private IsoLineViewModel _stepForRowNumber;

        public ObservableCollection<IsoLineViewModel> IsoLines { get; private set; } = new ObservableCollection<IsoLineViewModel>();

        private IsoLineViewModel _selected;

        public IsoLineViewModel Selected
        {
            get => _selected; 
            set
            {
                if(Set(ref _selected, value, nameof(Selected)))
                {
                    if(_selected == null)
                    {
                        _lastSelectedIndex = -1;
                        MessengerInstance.Send(new NullIsoLineSelectionMessage());
                    }
                    else
                    {
                        ProcessTillSelection();
                    }
                }
            }
        }

        public MainViewModel()
        {
            MessengerInstance.Register<GetSelectedStepMessage>(this, OnGetSelectedStepMessage);
        }

        private ICommand _openFile;
        public ICommand OpenFile => _openFile ?? (_openFile = new RelayCommand(() => OpenFileImpl()));

        private ICommand _closeFile;
        public ICommand CloseFile => _closeFile ?? (_closeFile = new RelayCommand(() => CloseFileImpl()));

        private ICommand _openParametersFile;
        public ICommand OpenParametersFile => _openParametersFile ?? (_openParametersFile = new RelayCommand(() => OpenParametersFileImpl()));

        private ICommand _saveParametersFile;
        public ICommand SaveParametersFile => _saveParametersFile ?? (_saveParametersFile = new RelayCommand(() => SaveParametersFileImpl()));

        private ICommand _oneShotProcess;
        public ICommand OneShotProcess => _oneShotProcess ?? (_oneShotProcess = new RelayCommand(() => OneShotProcessImpl()));

        private void OpenFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "iso", AddExtension = true, Filter = "Iso |*.iso" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                CloseFileImpl();

                //using (var stream = File.OpenText(dlg.FileName))
                //{
                //    var lineNumber = 1;
                //    var fileInfo = new FileInfo(dlg.FileName);
                //    var filePath = fileInfo.DirectoryName;

                //    IsoLineFactory.Reset();

                //    while (true)
                //    {
                //        var line = stream.ReadLine();

                //        if (line == null) break;

                //        if(IsJumpSubroitine(line))
                //        {
                //            CreateSubrountineIstruction(lineNumber, GetSubroutine(line, filePath), (il) => IsoLines.Add(new IsoLineViewModel(il)));
                //        }
                //        else if (!string.IsNullOrWhiteSpace(line) && !IsCommentLine(line))
                //        {
                //            //IsoLines.Add(new IsoLineViewModel(IsoLineFactory.Create(lineNumber, FilterLine(line))));
                //            IsoLineFactory.Create(lineNumber, FilterLine(line), (il) => IsoLines.Add(new IsoLineViewModel(il)));
                //        }

                //        lineNumber++;
                //    }
                //}

                OpenFileImpl(dlg.FileName, IsoLines);
            }
        }

        private void OpenFileImpl(string fileName, IList<IsoLineViewModel> isoLines)
        {
            using (var stream = File.OpenText(fileName))
            {
                var lineNumber = 1;
                var fileInfo = new FileInfo(fileName);
                var filePath = fileInfo.DirectoryName;

                IsoLineFactory.Reset();

                while (true)
                {
                    var line = stream.ReadLine();

                    if (line == null) break;

                    if (IsJumpSubroitine(line))
                    {
                        CreateSubrountineIstruction(lineNumber, GetSubroutine(line, filePath), (il) => isoLines.Add(new IsoLineViewModel(il)));
                    }
                    else if (!string.IsNullOrWhiteSpace(line) && !IsCommentLine(line))
                    {
                        //IsoLines.Add(new IsoLineViewModel(IsoLineFactory.Create(lineNumber, FilterLine(line))));
                        IsoLineFactory.Create(lineNumber, FilterLine(line), (il) => isoLines.Add(new IsoLineViewModel(il)));
                    }

                    lineNumber++;
                }
            }
        }

        private void CreateSubrountineIstruction(int lineNumber, string fileName, Action<IsoLine> addIsoline)
        {
            var fileInfo = new FileInfo(fileName);
            var isoLines = new List<IsoLineViewModel>();

            if (fileInfo.Exists)
            {
                OpenFileImpl(fileName, isoLines);
            }
        }

        private string GetSubroutine(string line, string filePath)
        {
            var subName = Regex.Match(line, "\\d+").Value;
            var fileName = $"{filePath}\\{subName}.cfu";


            return fileName;
        }

        private void CloseFileImpl()
        {
            IsoLines.Clear();

            MessengerInstance.Send(new FlushIsoLinesMessage());
        }

        private void OpenParametersFileImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "parameters", AddExtension = true, Filter = "Parameters |*.parameters" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ParametersSet));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var parametersSet = (ParametersSet)serializer.Deserialize(reader);

                    if(parametersSet != null)
                    {
                        MessengerInstance.Send(new SetAxesParametersMessage() { Parameters = parametersSet.AxesParameters });
                        MessengerInstance.Send(new SetStoragedExkParametersMessage() { Parameters = parametersSet.StoragedExkParameters });
                    }
                }
            }
        }

        private void SaveParametersFileImpl()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "parameters", AddExtension = true, Filter = "Parameters | *.parameters" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                List<Parameter> axesParameters = null;
                List<Parameter> storagedExkParameters = null;

                MessengerInstance.Send(new GetAxesParametersMessage() { GetParameters = (o) => axesParameters = o });
                MessengerInstance.Send(new GetStoragedExkParametersMessage() { GetParameters = (o) => storagedExkParameters = o });

                var parametersSet = new ParametersSet() { AxesParameters = axesParameters, StoragedExkParameters = storagedExkParameters };

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ParametersSet));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    serializer.Serialize(writer, parametersSet);
                }
            }
        }

        private void OneShotProcessImpl()
        {
            var istrToAction = new IstructionToActionConverter();

            _enableGiveStepRowNumber = true;
            istrToAction.StartListen();

            for (int i = 0; i < IsoLines.Count; i++)
            {
                _stepForRowNumber = IsoLines[i];
                IsoLines[i].Process();
            }

            istrToAction.StopListen();
            _enableGiveStepRowNumber = false;

            if((istrToAction.MachineSteps != null) && (istrToAction.MachineSteps.Count > 0))
            {
                SaveMachineSteps(istrToAction.MachineSteps);
            }
        }

        private string FilterLine(string line)
        {
            if (line.Contains(";"))
            {
                char[] separators = { ';' };
                return line.Split(separators)[0]; 
            }
            else
            {
                return line;
            }
        }

        private bool IsJumpSubroitine(string line)
        {
            return Regex.Match(line, "^\\s*JSR \\d+").Success;
        }

        private bool IsCommentLine(string line)
        {
            return Regex.Match(line, "^\\s*;.*").Success;
        }

        private void ProcessTillSelection()
        {
            var selectedIndex = IsoLines.IndexOf(_selected);

            if (_lastSelectedIndex > selectedIndex)
            {
                MessengerInstance.Send(new IsoLineSelectionChangedMessage());
                MessengerInstance.Send(new InvalidateIsoLineInibitionMessage());
            }

            _enableGiveSelectedStep = false;

            for (int i = 0; i < selectedIndex; i++) IsoLines[i].Process();

            _enableGiveSelectedStep = true;
            IsoLines[selectedIndex].Process();
            

            _lastSelectedIndex = selectedIndex;
        }


        private void OnGetSelectedStepMessage(GetSelectedStepMessage msg)
        {
            if(_enableGiveSelectedStep) msg.SetStep?.Invoke(_selected);
            else if(_enableGiveStepRowNumber) msg.SetStep?.Invoke(_stepForRowNumber);
        }

        private void SaveMachineSteps(List<MachineStep> machineSteps)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "msteps", AddExtension = true, Filter = "Machine steps |*.msteps" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var doc = new MachineStepsDocument() { Steps = machineSteps };
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    serializer.Serialize(writer, doc);
                }
            }
        }

    }
}
