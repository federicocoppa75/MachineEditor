using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Editor.ViewModels
{
    class ViewModelLocator
    {
        public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MachineStepViewModel>();
            SimpleIoc.Default.Register<ActionViewModel>();
        }

        public MainViewModel Main => IsInDesignMode ? DummyMainViewModel : SimpleIoc.Default.GetInstance<MainViewModel>();
        public MachineStepViewModel MachineStep => IsInDesignMode ? DummyMachineStepViewModel : SimpleIoc.Default.GetInstance<MachineStepViewModel>();
        public ActionViewModel Action => IsInDesignMode ? DummyActionViewModel : SimpleIoc.Default.GetInstance<ActionViewModel>();


        #region Dummy models

        public MainViewModel DummyMainViewModel => new MainViewModel();

        public MachineStepViewModel DummyMachineStepViewModel => new MachineStepViewModel() { MachineStep = new Models.Steps.MachineStep() };

        public ActionViewModel DummyActionViewModel => new ActionViewModel() { Action = new AddPanelAction() };

        #endregion
    }
}
