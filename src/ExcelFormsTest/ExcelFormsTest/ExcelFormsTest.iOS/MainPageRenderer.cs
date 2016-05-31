using ExcelFormsTest.iOS;
using ExcelFormsTest.Views;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainView), typeof(MainPageRenderer))]
namespace ExcelFormsTest.iOS
{
    class MainPageRenderer : PageRenderer
    {
        MainView view;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            view = e.NewElement as MainView;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            view.platformParameters = new PlatformParameters(this);
        }
    }
}
