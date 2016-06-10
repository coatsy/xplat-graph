using ExcelFormsTest.ViewModels;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ExcelFormsTest.Views
{
    public partial class MainView : ContentPage
    {
        public IPlatformParameters platformParameters { get; set; }

        public MainView()
        {
            InitializeComponent();
            this.BindingContext = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            App.ClientApplication.PlatformParameters = platformParameters;
            base.OnAppearing();
        }

        protected async void  ShowExpenses(object sender, object args)
        {
            await Navigation.PushAsync(new ExpensesView());
        }
    }
}
