using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToolEditor.ViewModels
{
    public class ViewModelLocator
    {
        public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GenericToolViewModel>();
            SimpleIoc.Default.Register<ToolViewModel<Tool>>();
            SimpleIoc.Default.Register<SimpleToolViewModel>();
            SimpleIoc.Default.Register<PointedToolViewModel>();
            SimpleIoc.Default.Register<TwoSectionToolViewModel>();
            SimpleIoc.Default.Register<DiskToolViewModel>();
            SimpleIoc.Default.Register<CountersinkViewModel>();
            SimpleIoc.Default.Register<DiskOnConeToolViewModel>();
            SimpleIoc.Default.Register<AngolarTransmissionViewModel>();
        }

        public MainViewModel Main => IsInDesignMode ? DummyMainViewModel : SimpleIoc.Default.GetInstance<MainViewModel>();
        public GenericToolViewModel GenericTool => IsInDesignMode ? DummyGenericToolViewModel : SimpleIoc.Default.GetInstance<GenericToolViewModel>();
        public ToolViewModel<Tool> BaseTool => IsInDesignMode ? DummyBaseToolViewMode : SimpleIoc.Default.GetInstance<ToolViewModel<Tool>>();
        public SimpleToolViewModel SimpleTool => IsInDesignMode ? DummySimpleToolViewModel : SimpleIoc.Default.GetInstance<SimpleToolViewModel>();
        public PointedToolViewModel PointedTool => IsInDesignMode ? DummyPointedToolViewModel : SimpleIoc.Default.GetInstance<PointedToolViewModel>();
        public TwoSectionToolViewModel TwoSectionTool => IsInDesignMode ? DummyTwoSectionToolViewModel : SimpleIoc.Default.GetInstance<TwoSectionToolViewModel>();
        public DiskToolViewModel DiskTool => IsInDesignMode ? DummyDiskToolViewModel : SimpleIoc.Default.GetInstance<DiskToolViewModel>();
        public CountersinkViewModel CountersinkTool => IsInDesignMode ? DummyCountersinkToolViewModel : SimpleIoc.Default.GetInstance<CountersinkViewModel>();
        public DiskOnConeToolViewModel DiskOnConeTool => IsInDesignMode ? DummyDiskOnConeToolViewModel : SimpleIoc.Default.GetInstance<DiskOnConeToolViewModel>();
        public AngolarTransmissionViewModel AngolarTransmission => IsInDesignMode ? DummyAngolarTransmissionViewModel : SimpleIoc.Default.GetInstance<AngolarTransmissionViewModel>();

        #region Dummy models


        private MainViewModel DummyMainViewModel
        {
            get
            {
                var vm = new MainViewModel();

                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 8 V", Description = "Description of Fora 8 V", Diameter = 8.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));
                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 8 H", Description = "Description of Fora 8 H", Diameter = 8.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));
                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 5 V", Description = "Description of Fora 5 V", Diameter = 5.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));
                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 5 H", Description = "Description of Fora 5 H", Diameter = 5.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));
                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 10 V", Description = "Description of Fora 10 V", Diameter = 10.0, Length = 60.0, UsefulLength = 45.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));
                vm.Tools.Add(new RowToolViewModel(new SimpleTool() { Name = "Fora 10 H", Description = "Description of Fora 10 H", Diameter = 10.0, Length = 43.0, UsefulLength = 37.0, ToolLinkType = MachineModels.Enums.ToolLinkType.Static }));

                vm.Selected = vm.Tools[2];

                return vm;
            }
        }

        private GenericToolViewModel DummyGenericToolViewModel
        {
            get
            {
                var vm = new GenericToolViewModel();

                vm.Tool = new SimpleTool()
                {
                    Name = "Fora 8 V",
                    Description = "Description of Fora 8 V",
                    Diameter = 8.0,
                    Length = 60.0,
                    UsefulLength = 45.0,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };

                return vm;
            }
        }

        public SimpleToolViewModel DummySimpleToolViewModel
        {
            get
            {
                return new SimpleToolViewModel()
                {
                    Name = "Fora 8 V",
                    Description = "Description of Fora 8 V",
                    Diameter = 8.0,
                    Length = 60.0,
                    UsefulLength = 45.0,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };
            }
        }

        public PointedToolViewModel DummyPointedToolViewModel
        {
            get
            {
                return new PointedToolViewModel()
                {
                    Name = "Lancia 10",
                    Description = "Description of Lancia 10",
                    Diameter = 10.0,
                    ConeHeight = 8.0,
                    StraightLength = 52.0,
                    UsefulLength = 40.0,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };
            }
        }

        public TwoSectionToolViewModel DummyTwoSectionToolViewModel
        {
            get
            {
                return new TwoSectionToolViewModel()
                {
                    Name = "Cerniera 35",
                    Description = "Description of Cerniera 35",
                    Diameter1 = 12.0,
                    Length1 = 45.0,
                    Diameter2 = 35.0,
                    Length2 = 15.0,
                    UsefulLength = 12.5,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };
            }
        }

        public DiskToolViewModel DummyDiskToolViewModel
        {
            get
            {
                return new DiskToolViewModel()
                {
                    Name = "Lama 120",
                    Description = "Description of Lama 120",
                    BodyThickness = 2.0,
                    CuttingThickness = 3.2,
                    CuttingRadialThickness = 4.0,
                    Diameter = 120.0,
                    RadialUsefulLength = 35.0,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };
            }
        }

        public CountersinkViewModel DummyCountersinkToolViewModel
        {
            get
            {
                return new CountersinkViewModel()
                {
                    Name = "Svasata 8",
                    Description = "Description of Svasata 8",
                    Diameter1 = 8.0,
                    Diameter2 = 16.0,
                    Length1 = 20.0,
                    Length2 = 10.0,

                };
            }
        }

        public DiskOnConeToolViewModel DummyDiskOnConeToolViewModel
        {
            get
            {
                return new DiskOnConeToolViewModel()
                {
                    Name = "Lama 120",
                    Description = "Description of Lama 120",
                    BodyThickness = 2.0,
                    CuttingThickness = 3.2,
                    CuttingRadialThickness = 4.0,
                    Diameter = 120.0,
                    RadialUsefulLength = 35.0,
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static,
                    PostponemntDiameter = 10.0,
                    PostponemntLength = 104.0
                };
            }
        }

        public ToolViewModel<Tool> DummyBaseToolViewMode
        {
            get
            {
                return new ToolViewModel<Tool>()
                {
                    Name = "Base tool",
                    Description = "Description of base tool",
                    ToolLinkType = MachineModels.Enums.ToolLinkType.Static
                };
            }
        }

        public AngolarTransmissionViewModel DummyAngolarTransmissionViewModel
        {
            get
            {
                return new AngolarTransmissionViewModel()
                {
                    Name = "AngolarTransmission",
                    Description = "Simpmle angular transmission",
                };
            }
        }

        #endregion
    }
}
