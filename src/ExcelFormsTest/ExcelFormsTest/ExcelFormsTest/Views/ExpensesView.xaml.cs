using ExcelFormsTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ExcelFormsTest.Views
{
    public partial class ExpensesView : ContentPage
    {
        ExpensesViewModel vm;

        public ExpensesView()
        {
            InitializeComponent();
            vm = new ExpensesViewModel();
            this.BindingContext = vm;
        }

        private async void DisplayChart(object sender, object args)
        {
            await Navigation.PushModalAsync(new ExpensesChartView());
        }
    }
}
