using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MachineModels.Models;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolingEditor.ViewModels.MachineElements;
using ToolingEditor.ViewModels.Tools;

namespace ToolingEditor.ViewModels
{
    public class ViewModelLocator
    {
        public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ToolsViewModel>();
            SimpleIoc.Default.Register<MachineElementsViewModel>();
        }

        public MainViewModel Main => IsInDesignMode ? DummyMainViewModel : SimpleIoc.Default.GetInstance<MainViewModel>();
        public ToolsViewModel Tools => IsInDesignMode ? DummyToolsViewModel : SimpleIoc.Default.GetInstance<ToolsViewModel>();
        public MachineElementsViewModel MachineElements => IsInDesignMode ? DummyMachineElementsViewModel : SimpleIoc.Default.GetInstance<MachineElementsViewModel>();

        #region Dummy models

        public MainViewModel DummyMainViewModel
        {
            get
            {
                return new MainViewModel();
            }
        }

        public ToolsViewModel DummyToolsViewModel
        {
            get
            {
                return new ToolsViewModel()
                {
                    Tools = new List<ToolViewModel>()
                    {
                        new ToolViewModel(new SimpleTool() { Name = "Fora 8 V", Description = "Description of Fora 8 V", Diameter = 8.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }),
                        new ToolViewModel(new SimpleTool() { Name = "Fora 8 H", Description = "Description of Fora 8 H", Diameter = 8.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }),
                        new ToolViewModel(new SimpleTool() { Name = "Fora 5 V", Description = "Description of Fora 5 V", Diameter = 5.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }),
                        new ToolViewModel(new SimpleTool() { Name = "Fora 5 H", Description = "Description of Fora 5 H", Diameter = 5.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }),
                        new ToolViewModel(new SimpleTool() { Name = "Fora 10 V", Description = "Description of Fora 10 V", Diameter = 10.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }),
                        new ToolViewModel(new SimpleTool() { Name = "Fora 10 H", Description = "Description of Fora 10 H", Diameter = 10.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static })
                    }
                };
            }
        }

        public MachineElementsViewModel DummyMachineElementsViewModel
        {
            get
            {
                return new MachineElementsViewModel()
                {
                    //Elements = new List<MachineElement>()
                    //{
                    //    new MachineElement()
                    //    {
                    //        Name = "root",
                    //        Children = new List<MachineElement>()
                    //        {
                    //            new MachineElement(){ Name = "Leaf 1", LinkToParentType = MachineModels.Enums.LinkType.Static },
                    //            new MachineElement(){ Name = "Leaf 2", LinkToParentType = MachineModels.Enums.LinkType.Static }
                    //        }
                    //    }
                    //}

                    Elements = new List<MachineElementViewModel>()
                    {
                        new MachineElementViewModel()
                        {
                            Name = "Root",
                            Children = new List<MachineElementViewModel>()
                            {
                                new MachineElementViewModel() { Name = "Leaf 1" },
                                new MachineElementViewModel() { Name = "Leaf 2" }
                            }
                        }
                    }
                };
            }
        }

        #endregion
    }
}


