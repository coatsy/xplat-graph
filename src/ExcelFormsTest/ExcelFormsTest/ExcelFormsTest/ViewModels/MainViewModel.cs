using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Using Microsoft Graph API with Xamarin Forms";
        }

        public AppViewModel AppVM
        {
            get { return (App.Current as App).VM; }
        }


    }
}
