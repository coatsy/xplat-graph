using ExcelFormsTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ExcelFormsTest.Views
{
    public partial class GroupsView : ContentPage
    {
        public GroupsView()
        {
            InitializeComponent();
            this.BindingContext = new GroupsViewModel();
        }
    }
}
