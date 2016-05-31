using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using ExcelFormsTest.Views;
using ExcelFormsTest.Droid;
using Microsoft.Identity.Client;

[assembly: ExportRenderer(typeof(MainView), typeof(MainPageRenderer))]
namespace ExcelFormsTest.Droid
{
    class MainPageRenderer : PageRenderer
    {
        MainView view;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            view = e.NewElement as MainView;
            var activity = this.Context as Activity;
            view.platformParameters = new PlatformParameters(activity);
        }
    }
}