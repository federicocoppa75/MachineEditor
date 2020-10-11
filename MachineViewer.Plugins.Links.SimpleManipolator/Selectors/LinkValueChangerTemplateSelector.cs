using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MachineViewer.Plugins.Links.SimpleManipolator.Extensions;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Selectors
{
    [ContentProperty("Templates")]
    public class LinkValueChangerTemplateSelector : DataTemplateSelector
    {

        public List<LinkValueChangerTemplateSelectorOptions> Templates { get; set; } = new List<LinkValueChangerTemplateSelectorOptions>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dt = null;

            if (item is MachineViewModels.ViewModels.Links.ILinkViewModel link)
            {
                foreach (var t in Templates)
                {
                    if (t.When == link.GetLinkType())
                    {
                        dt = t.Then;
                        break;
                    }
                }
            }

            return dt;
        }
    }

    [ContentProperty("Then")]
    public class LinkValueChangerTemplateSelectorOptions
    {
        public LinkType When { get; set; }
        public DataTemplate Then { get; set; }
    }
}
