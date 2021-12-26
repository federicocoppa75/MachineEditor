using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using MachineModels.Models;
using MachineModels.Models.Tools;
using MachineModels.Models.Tooling;

namespace MachineModels.IO
{
    public class ZipArchiveHelper
    {
        private const string _machineFileName = "machine.xml";
        private const string _toolsSetFileName = "tools.tools";
        private const string _oldToolsSetFileName = "toos.tools";
        private const string _toolingFileName = "tooling.tooling";

        private List<string> _entrieeNames = new List<string>();

        public static bool ExportEnvironment(string exportFile, string machProjectFile, string toolsFile, string toolingFile)
        {
            var h = new ZipArchiveHelper();

            return h.ExportEnvironmentImplementation(exportFile, machProjectFile, toolsFile, toolingFile);
        }

        public static bool ImportEnvironment(string importFile, out string machProjectFile, out string toolsFile, out string toolingFile)
        {
            var h = new ZipArchiveHelper();

            return h.ImportEnvironmentImplementation(importFile, out machProjectFile, out toolsFile, out toolingFile);
        }

        public static bool Export(MachineElement machine, string filePath)
        {
            var h = new ZipArchiveHelper();

            return h.ExportImplementation(machine, filePath);
        }

        public static MachineElement Import(string filePath, Action<string> setMachProjectFile = null)
        {
            var h = new ZipArchiveHelper();

            return h.ImportImplementation(filePath, setMachProjectFile);
        }

        private bool ExportEnvironmentImplementation(string exportFile, string machProjectFile, string toolsFile, string toolingFile)
        {
            bool result = false;
            var machine = GetMachine(machProjectFile);
            var toolSet = GetToolSet(toolsFile);
            var tooling = GetTooling(toolingFile);
            var info = new FileInfo(exportFile);

            if (info.Exists) info.Delete();

            if((machine != null) && (toolSet != null) && (tooling != null))
            {
                using (var archive = ZipFile.Open(exportFile, ZipArchiveMode.Create))
                {
                    if (!AddModelsFilesToArchive(machine, archive))
                    {
                        // log errore
                    }
                    else if (!AddProjectFileToArchive(machine, archive))
                    {
                        // log error
                    }
                    else if(!AddConeModelsFilesToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if (!AddBodyModelsFilesToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if(!AddToolsetFileToArchive(toolSet, archive))
                    {
                        // log error
                    }
                    else if(!AddToolingFileToArchive(tooling, archive))
                    {
                        // log error
                    }
                }
            }

            return result;
        }

        private bool ImportEnvironmentImplementation(string importFile, out string machProjectFile, out string toolsFile, out string toolingFile)
        {
            bool result = false;

            machProjectFile = string.Empty;
            toolsFile = string.Empty;
            toolingFile = string.Empty;

            if (!string.IsNullOrEmpty(importFile))
            {
                var info = new FileInfo(importFile);
                var extractPath = $"{info.DirectoryName}\\{Path.GetFileNameWithoutExtension(info.Name)}";
                var dirInfo = new DirectoryInfo(extractPath);

                if (dirInfo.Exists) dirInfo.Delete(true);

                ZipFile.ExtractToDirectory(importFile, extractPath);

                var toolsFileName = dirInfo.GetFiles(_oldToolsSetFileName).Length > 0 ? _oldToolsSetFileName : _toolsSetFileName;
                var loadToolsFile = $"{extractPath}\\{toolsFileName}";

                machProjectFile = $"{extractPath}\\{_machineFileName}";
                toolsFile = $"{extractPath}\\{_toolsSetFileName}";
                toolingFile = $"{extractPath}\\{_toolingFileName}";

                var machine = GetMachine(machProjectFile);
                var toolSet = GetToolSet(loadToolsFile);
                var tooling = GetTooling(toolingFile);

                UpdateModelsFiles(machine, extractPath, true);
                UpdateModelsFiles(toolSet, extractPath, true);
                UpdateModelsFiles(tooling, extractPath, true);

                result = true;
            }

            return result;
        }

        private MachineElement ImportImplementation(string filePath, Action<string> setMachProjectFile = null)
        {
            MachineElement m = null;

            if (!string.IsNullOrEmpty(filePath))
            {
                var info = new FileInfo(filePath);
                //var extractPath = GetExtractionPath();
                var extractPath = $"{info.DirectoryName}\\{Path.GetFileNameWithoutExtension(info.Name)}";
                var dirInfo = new DirectoryInfo(extractPath);
                var projectFile = $"{extractPath}\\{_machineFileName}";

                if (dirInfo.Exists) dirInfo.Delete(true);

                ZipFile.ExtractToDirectory(filePath, extractPath);

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

                using (var reader = new StreamReader(projectFile))
                {
                    m = (MachineElement)serializer.Deserialize(reader);
                }

                if (m != null) UpdateModelsFiles(m, extractPath);
                setMachProjectFile?.Invoke(projectFile);
            }

            return m;
        }

        private void UpdateModelsFiles(MachineElement m, string extractPath, bool save = false)
        {
            if (!string.IsNullOrEmpty(m.ModelFile))
            {
                m.ModelFile = $"{extractPath}\\{m.ModelFile}";
            }

            foreach (var item in m.Children)
            {
                UpdateModelsFiles(item, extractPath);
            }

            if(save)
            {
                var machProjectFile = $"{extractPath}\\{_machineFileName}";
                SaveMachineProjectFile(m, machProjectFile);
            }
        }

        private static void SaveMachineProjectFile(MachineElement m, string machProjectFile)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

            using (var writer = new System.IO.StreamWriter(machProjectFile))
            {
                serializer.Serialize(writer, m);
            }
        }

        private static void SaveToolSetFile(ToolSet toolSet, string toolSetFile)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

            using (var writer = new System.IO.StreamWriter(toolSetFile))
            {
                serializer.Serialize(writer, toolSet);
            }
        }

        private static void SaveToolingFile(Tooling tooling, string toolingFile)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));

            using (var writer = new System.IO.StreamWriter(toolingFile))
            {
                serializer.Serialize(writer, tooling);
            }
        }

        private void UpdateModelsFiles(ToolSet toolSet, string extractPath, bool save = false)
        {
            foreach (var item in toolSet.Tools)
            {
                if (!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    item.ConeModelFile = $"{extractPath}\\{item.ConeModelFile}";
                }

                if((item is AngolarTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    at.BodyModelFile = $"{extractPath}\\{at.BodyModelFile}";
                }
            }

            if(save)
            {
                var toolSetFile = $"{extractPath}\\{_toolsSetFileName}";
                SaveToolSetFile(toolSet, toolSetFile);
            }
        }

        private void UpdateModelsFiles(Tooling tooling, string extractPath, bool save = false)
        {
            tooling.MachineFile = $"{extractPath}\\{_machineFileName}";
            tooling.ToolsFile = $"{extractPath}\\{_toolsSetFileName}";

            if(save)
            {
                var toolingFile = $"{extractPath}\\{_toolingFileName}";
                SaveToolingFile(tooling, toolingFile);
            }
        }

        private string GetExtractionPath()
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\MachineEditor\\tmp";
        }

        private bool ExportImplementation(MachineElement machine, string filePath)
        {
            bool result = true;
            FileInfo info = new FileInfo(filePath);

            if (info.Exists) info.Delete();

            using (var archive = ZipFile.Open(filePath, ZipArchiveMode.Create))
            {
                if (!AddModelsFilesToArchive(machine, archive))
                {
                    // log errore
                }
                else if (!AddProjectFileToArchive(machine, archive))
                {
                    // log error
                }


            }

            return result;
        }

        private bool AddProjectFileToArchive(MachineElement me, ZipArchive archive)
        {
            bool result = true;

            FilterModelsNames(me);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));
            var entry = archive.CreateEntry(_machineFileName);

            using (var writer = new StreamWriter(entry.Open()))
            {
                serializer.Serialize(writer, me);
            }

            return result;
        }

        private void FilterModelsNames(MachineElement me)
        {
            if (!string.IsNullOrEmpty(me.ModelFile))
            {
                var info = new FileInfo(me.ModelFile);
                //var name = $"{info.Name}.{info.Extension}";
                var name = info.Name;

                me.ModelFile = name;
            }

            foreach (var item in me.Children)
            {
                FilterModelsNames(item);
            }
        }

        private bool AddModelsFilesToArchive(MachineElement me, ZipArchive archive)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(me.ModelFile))
            {
                FileInfo info = new FileInfo(me.ModelFile);

                if (info.Exists)
                {
                    var name = info.Name;

                    if (!_entrieeNames.Contains(name))
                    {
                        var entry = archive.CreateEntryFromFile(me.ModelFile, name);
                        _entrieeNames.Add(name);
                    }
                }
            }

            foreach (var item in me.Children)
            {
                AddModelsFilesToArchive(item, archive);
            }

            return result;
        }

        private ToolSet GetToolSet(string toolsFile)
        {
            FileInfo info = new FileInfo(toolsFile);

            if (!info.Exists) throw new ArgumentException($"The file {toolsFile} not exsits!");

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));

            using (var reader = new System.IO.StreamReader(toolsFile))
            {
                return (ToolSet)serializer.Deserialize(reader);
            }
        }

        private Tooling GetTooling(string toolingFile)
        {
            FileInfo info = new FileInfo(toolingFile);

            if (!info.Exists) throw new ArgumentException($"The file {toolingFile} not exsits!");

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));

            using (var reader = new System.IO.StreamReader(toolingFile))
            {
                return (Tooling)serializer.Deserialize(reader);
            }
        }

        private MachineElement GetMachine(string machProjectFile)
        {
            FileInfo info = new FileInfo(machProjectFile);

            if (!info.Exists) throw new ArgumentException($"The file {machProjectFile} not exsits!");

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineElement));

            using (var reader = new StreamReader(machProjectFile))
            {
                return (MachineElement)serializer.Deserialize(reader);
            }
        }

        private bool AddToolingFileToArchive(Tooling tooling, ZipArchive archive)
        {
            FilterModelsNames(tooling);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Tooling));
            var entry = archive.CreateEntry(_toolingFileName);

            using (var writer = new StreamWriter(entry.Open()))
            {
                serializer.Serialize(writer, tooling);
            }

            return true;
        }

        private bool AddToolsetFileToArchive(ToolSet toolSet, ZipArchive archive)
        {
            FilterModelsNames(toolSet);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ToolSet));
            var entry = archive.CreateEntry(_toolsSetFileName);

            using (var writer = new StreamWriter(entry.Open()))
            {
                serializer.Serialize(writer, toolSet);
            }


            return true;
        }

        private bool AddConeModelsFilesToArchive(ToolSet toolSet, ZipArchive archive)
        {
            bool result = true;

            foreach (var item in toolSet.Tools)
            {
                if(!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    FileInfo info = new FileInfo(item.ConeModelFile);

                    if (info.Exists)
                    {
                        var name = info.Name;

                        if (!_entrieeNames.Contains(name))
                        {
                            var entry = archive.CreateEntryFromFile(item.ConeModelFile, name);
                            _entrieeNames.Add(name);
                        }
                    }
                }
            }

            return result;
        }

        private bool AddBodyModelsFilesToArchive(ToolSet toolSet, ZipArchive archive)
        {
            bool result = true;

            foreach (var item in toolSet.Tools)
            {
                if ((item is AngolarTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    FileInfo info = new FileInfo(at.BodyModelFile);

                    if (info.Exists)
                    {
                        var name = info.Name;

                        if (!_entrieeNames.Contains(name))
                        {
                            var entry = archive.CreateEntryFromFile(at.BodyModelFile, name);
                            _entrieeNames.Add(name);
                        }
                    }
                }
            }

            return result;
        }

        private void FilterModelsNames(ToolSet toolSet)
        {
            foreach (var item in toolSet.Tools)
            {
                if(!string.IsNullOrEmpty(item.ConeModelFile))
                {
                    var info = new FileInfo(item.ConeModelFile);
                    item.ConeModelFile = info.Name;
                }

                if((item is AngolarTransmission at) && !string.IsNullOrEmpty(at.BodyModelFile))
                {
                    var info = new FileInfo(at.BodyModelFile);
                    at.BodyModelFile = info.Name;
                }
            }
        }

        private void FilterModelsNames(Tooling tooling)
        {            
            tooling.MachineFile = _machineFileName;
            tooling.ToolsFile = _toolsSetFileName;
        }
    }
}
