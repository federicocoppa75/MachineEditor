using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineModels.Enums;
using MachineModels.Models.Tools;
using MachineViewModelUtils.Extensions;
using MachineViewModelUtils.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ToolEditor.Messages;
using ToolEditor.Views;

namespace ToolEditor.ViewModels
{
    public class ToolViewModel<T> : ViewModelBase where T : Tool
    {
        protected T _tool;

        protected bool _fromModelToView;

        private string _name;
        [Category("Base")]

        public string Name
        {
            get { return _name; }
            set { if (Set(ref _name, value, nameof(Name))) UpdateModel(); }
        }

        private string _description;
        [Category("Base")]
        public string Description
        {
            get { return _description; }
            set { if (Set(ref _description, value, nameof(Description))) UpdateModel(); }
        }

        private ToolLinkType _toolLinkType;
        [Category("Base")]
        public ToolLinkType ToolLinkType
        {
            get { return _toolLinkType; }
            set { if (Set(ref _toolLinkType, value, nameof(ToolLinkType))) UpdateModel(); }
        }

        [Category("Size")]
        public double TotalDiameter => (_tool?.GetTotalDiameter()).GetValueOrDefault();

        [Category("Size")]
        public double TotalLength => (_tool?.GetTotalLength()).GetValueOrDefault();

        [Browsable(false)]
        public MeshGeometry3D Geometry => GetGeometry();

        private string _coneModelFile;
        [Category("Base")]
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string ConeModelFile
        {
            get { return _coneModelFile; }
            set
            {
                if (Set(ref _coneModelFile, value, nameof(ConeModelFile)))
                {
                    UpdateModel();
                    UpdateConeGeometry();
                    RaisePropertyChanged(nameof(ConeGeometry));
                }
            }
        }

        [Browsable(false)]
        public MeshGeometry3D ConeGeometry { get; set; }

        public ToolViewModel()
        {
            Messenger.Default.Register<SelectedToolChanged<T>>(this, OnSelectedChanged);

            Messenger.Default.Send(new ToolViewReady() { ToolType = ToolType });
        }

        /// <summary>
        /// Aggiornamento dei campi da viewmodel a modello.
        /// </summary>
        /// <param name="tool">Modello da aggiornare</param>
        protected virtual void UpdateModel(T tool)
        {
            tool.Name = Name;
            tool.Description = Description;
            tool.ToolLinkType = ToolLinkType;
            tool.ConeModelFile = ConeModelFile;
        }

        /// <summary>
        /// Aggiornamento dei campi del modello e notifica tramite messaggio.
        /// </summary>
        protected void UpdateModel()
        {
            if (!_fromModelToView)
            {
                UpdateModel(_tool);

                Messenger.Default.Send(new ToolModelDataChanged() { Tool = _tool });
                RaisePropertyChanged(nameof(Geometry));
            }
        }

        /// <summary>
        /// Aggiornamento campi base del view model che corrispondono alla classe base dei modelli <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">Riferimento al modello da cui prendere i dati.</param>
        protected void UpdateViewModel(T tool)
        {
            Name = tool.Name;
            Description = tool.Description;
            ToolLinkType = tool.ToolLinkType;
            ConeModelFile = tool.ConeModelFile;
        }

        /// <summary>
        /// Aggiornamento campi del view model dal modello.
        /// </summary>
        protected virtual void UpdateViewModel()
        {
            UpdateViewModel(_tool);
        }

        /// <summary>
        /// Tipo di utensile gestito.
        /// </summary>
        protected virtual ToolType ToolType => ToolType.Base;

        private void OnSelectedChanged(SelectedToolChanged<T> msg)
        {
            _fromModelToView = true;
            _tool = msg.Tool;

            if (_tool != null) UpdateViewModel();

            _fromModelToView = false;
        }

        protected virtual MeshGeometry3D GetGeometry() => null;

        private void UpdateConeGeometry()
        {
            
            if (!string.IsNullOrEmpty(ConeModelFile) && System.IO.File.Exists(ConeModelFile))
            {
                ConeGeometry = ToolsMeshHelper.LoadModelMeshGeometry(ConeModelFile);
            }
            else
            {
                ConeGeometry = null;
            }

            RaisePropertyChanged(nameof(ConeGeometry));
        }
    }
}
