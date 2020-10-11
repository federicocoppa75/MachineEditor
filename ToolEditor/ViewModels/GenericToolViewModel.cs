using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineModels.Enums;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolEditor.Messages;

namespace ToolEditor.ViewModels
{
    public class GenericToolViewModel : ViewModelBase
    {
        private Tool _tool;
        public Tool Tool
        {
            get { return _tool; }
            set
            {
                if(Set(ref _tool, value, nameof(Tool)))
                {
                    ToolType = (_tool != null) ? _tool.ToolType : ToolType.None;
                }
            }
        }

        private ToolType _toolType;
        public ToolType ToolType
        {
            get { return _toolType; }
            set { Set(ref _toolType, value, nameof(ToolType)); }
        }

        public GenericToolViewModel()
        {
            Messenger.Default.Register<SelectedToolChanged<Tool>>(this, OnSelectedChanged);
            Messenger.Default.Register<ToolViewReady>(this, OnToolViewReady);
        }

        /// <summary>
        /// Questa callback serve perchè alla prima visualizzazione di utensili di un tipo il messaggio 
        /// viene lanciato prima della creazione della vista e, quindi, del relativo view model.
        /// SAREBBE BENE trovare il modo di risolvere il problema posticipando il rilancio del messaggio
        /// a dopo la creazione della vista!
        /// </summary>
        /// <param name="msg"></param>
        private void OnToolViewReady(ToolViewReady msg)
        {
            if(Tool != null)
            {
                if(Tool.ToolType == msg.ToolType)
                {
                    switch (Tool.ToolType)
                    {
                        case ToolType.None:
                            break;
                        case ToolType.Base:
                            break;
                        case ToolType.Simple:
                            Messenger.Default.Send(new SelectedToolChanged<SimpleTool>() { Tool = Tool as SimpleTool });
                            break;
                        case ToolType.TwoSection:
                            Messenger.Default.Send(new SelectedToolChanged<TwoSectionTool>() { Tool = Tool as TwoSectionTool });
                            break;
                        case ToolType.Pointed:
                            Messenger.Default.Send(new SelectedToolChanged<PointedTool>() { Tool = Tool as PointedTool });
                            break;
                        case ToolType.Disk:
                            Messenger.Default.Send(new SelectedToolChanged<DiskTool>() { Tool = Tool as DiskTool });
                            break;
                        case ToolType.BullNoseConcave:
                            break;
                        case ToolType.BullNoseConvex:
                            break;
                        case ToolType.Composed:
                            break;
                        case ToolType.Countersink:
                            Messenger.Default.Send(new SelectedToolChanged<CountersinkTool>() { Tool = Tool as CountersinkTool });
                            break;
                        case ToolType.DiskOnCone:
                            Messenger.Default.Send(new SelectedToolChanged<DiskOnConeTool>() { Tool = Tool as DiskOnConeTool });
                            break;
                         default:
                            break;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Tool type and view mismetch!");
                }

            }
        }

        private void OnSelectedChanged(SelectedToolChanged<Tool> msg)
        {
            var toolType = (msg.Tool == null) ? _tool.ToolType : msg.Tool.ToolType;


            Tool = msg.Tool;

            if (Tool != null)
            {

                switch (toolType)
                {
                    case ToolType.None:
                        break;
                    case ToolType.Base:
                        break;
                    case ToolType.Simple:
                        Messenger.Default.Send(msg.DownCast<SimpleTool>());
                        break;
                    case ToolType.TwoSection:
                        Messenger.Default.Send(msg.DownCast<TwoSectionTool>());
                        break;
                    case ToolType.Pointed:
                        Messenger.Default.Send(msg.DownCast<PointedTool>());
                        break;
                    case ToolType.Disk:
                        Messenger.Default.Send(msg.DownCast<DiskTool>());
                        break;
                    case ToolType.BullNoseConcave:
                        break;
                    case ToolType.BullNoseConvex:
                        break;
                    case ToolType.Composed:
                        break;
                    case ToolType.Countersink:
                        Messenger.Default.Send(msg.DownCast<CountersinkTool>());
                        break;
                    case ToolType.DiskOnCone:
                        Messenger.Default.Send(msg.DownCast<DiskOnConeTool>());
                        break;
                    default:
                        break;
                }
            }
        }



    }
}
