using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MachineSteps.Plugins.IsoInterpreter.ViewModels;

namespace MachineSteps.IsoInterpreter.SimpleApp.ViewModels
{
    public class ViewModelLocator
    {
        public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<StateViewModel>();
            SimpleIoc.Default.Register<VlVariablesViewModel>();
            SimpleIoc.Default.Register<RapidPositionViewModel>();
            SimpleIoc.Default.Register<ExkVariablesViewModel>();
            SimpleIoc.Default.Register<AxesParametersViewModel>();
            SimpleIoc.Default.Register<StoragedExkVariableViewModel>();
            SimpleIoc.Default.Register<MLineViewModel>();
            SimpleIoc.Default.Register<SLineViewModel>();
        }

        public MainViewModel Main => IsInDesignMode ? DummyMainViewModel : SimpleIoc.Default.GetInstance<MainViewModel>();
        public StateViewModel State => IsInDesignMode ? DummyStateViewModel : SimpleIoc.Default.GetInstance<StateViewModel>();
        public VariablesViewModel VlVariables => IsInDesignMode ? DummyVlVariablesViewModel : SimpleIoc.Default.GetInstance<VlVariablesViewModel>();
        public RapidPositionViewModel RapidPosition => IsInDesignMode ? DummyRapidPOsitionViewModel : SimpleIoc.Default.GetInstance<RapidPositionViewModel>();
        public VariablesViewModel ExkVariables => IsInDesignMode ? DummyExkVariablesViewModel : SimpleIoc.Default.GetInstance<ExkVariablesViewModel>();
        public ParametersViewModel AxesParameters => IsInDesignMode ? DummyAxesParametersViewModel : SimpleIoc.Default.GetInstance<AxesParametersViewModel>();
        public ParametersViewModel StoragedExkParameters => IsInDesignMode ? DummyStoragedExkParameters : SimpleIoc.Default.GetInstance<StoragedExkVariableViewModel>();
        public MLineViewModel MLine => IsInDesignMode ? DummyMLine : SimpleIoc.Default.GetInstance<MLineViewModel>();
        public SLineViewModel SLine => IsInDesignMode ? DummySLine : SimpleIoc.Default.GetInstance<SLineViewModel>();

        public MainViewModel DummyMainViewModel
        {
            get
            {
                return new MainViewModel();
            }
        }

        public StateViewModel DummyStateViewModel
        {
            get
            {
                return new StateViewModel();
            }
        }

        public VariablesViewModel DummyVlVariablesViewModel
        {
            get
            {
                return new VlVariablesViewModel();
            }
        }

        public RapidPositionViewModel DummyRapidPOsitionViewModel
        {
            get
            {
                return new RapidPositionViewModel();
            }
        }

        public VariablesViewModel DummyExkVariablesViewModel
        {
            get
            {
                return new ExkVariablesViewModel();
            }
        }

        public StoragedExkVariableViewModel DummyStoragedExkParameters
        {
            get
            {
                return new StoragedExkVariableViewModel()
                {
                    Parameters = new System.Collections.ObjectModel.ObservableCollection<ParameterViewModel>()
                    {
                        new ParameterViewModel()
                        {
                            Name = "ETK[800]",
                            Value = string.Empty
                        }
                    }
                };
            }

        }

        public AxesParametersViewModel DummyAxesParametersViewModel
        {
            get
            {
                return new AxesParametersViewModel();
            }
        }

        public MLineViewModel DummyMLine => new MLineViewModel() { Value = 409 };
        public SLineViewModel DummySLine => new SLineViewModel() { Value = 3000 };
    }
}
