using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineViewer.Messages;
using MachineViewer.Messages.Probing;
using MachineViewModels.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels
{
    public class MachineElementViewModel : MeshGeometryVisual3D, INotifyPropertyChanged
    {
        private static int _idSeed;

        public int Id { get; private set; }

        public string Name { get; set; }

        public MachineElementViewModel Parent { get; set; }

        public ILinkViewModel LinkToParent { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(nameof(IsExpanded));
            }
        }

        private ICommand _changeChainVisibilityState;
        public ICommand ChangeChainVisibilityState { get { return _changeChainVisibilityState ?? (_changeChainVisibilityState = new RelayCommand(() => ChangeChainVisibilityStateImpl())); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public MachineElementViewModel()
        {
            Id = _idSeed++;
            Messenger.Default.Register<SelectionElementMessage>(this, OnSelectedElement);
            Messenger.Default.Register<GetMachineElementByClickGeometryMessage>(this, OnSelectedElement);
        }

        public void RequestTreeviewVisibility()
        {
            if(Parent != null && !Parent.IsExpanded)
            {
                Parent.RequestTreeviewVisibility();
                Parent.IsExpanded = true;
            }
        }

        private void OnSelectedElement(SelectionElementMessage msg)
        {
            if((msg.Selected != null) && object.ReferenceEquals(MeshGeometry, msg.Selected))
            {
                msg?.SetSelected(this);
            }
        }

        protected void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void ChangeChainVisibilityStateImpl()
        {
            ChangeVisibleProperty(this, !Visible);
        }

        private void ChangeVisibleProperty(MachineElementViewModel me, bool value)
        {
            me.Visible = value;
            ChangeChildrenVisibleProperty(me, value);
        }

        private void ChangeChildrenVisibleProperty(MachineElementViewModel me, bool value)
        {
            foreach (var item in me.Children)
            {
                if (item is MachineElementViewModel child)
                {
                    ChangeVisibleProperty(child, value);
                }
            }
        }
    }
}
