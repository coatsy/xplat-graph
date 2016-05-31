using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class PropertyListViewModel : ViewModelBase
    {

        public PropertyListViewModel()
        {
            Title = "Property List";
        }
        private ObservableCollection<PropertyInformationViewModel> properties;
        public ObservableCollection<PropertyInformationViewModel> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new ObservableCollection<PropertyInformationViewModel>();
                }
                return properties;
            }
        }

        private CommandBase loadPropertiesCommand;

        public CommandBase LoadPropertiesCommand
        {
            get
            {
                loadPropertiesCommand = loadPropertiesCommand ?? new CommandBase(DoLoadPropertiesCommand);
                return loadPropertiesCommand;
            }
        }

        private async void DoLoadPropertiesCommand()
        {
            var props = await DataService.GetProperties();
            Properties.Clear();
            foreach (var prop in props.OrderBy(p=>p.Valuation))
            {
                Properties.Add(new PropertyInformationViewModel(prop));
            }
        }

    }
}
