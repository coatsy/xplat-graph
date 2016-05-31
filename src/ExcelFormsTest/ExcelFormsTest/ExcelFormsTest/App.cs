using ExcelFormsTest.ViewModels;
using ExcelFormsTest.Views;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ExcelFormsTest
{
    public class App : Application
    {

        const string AppId = "2927aace-2535-4715-a22a-2393d8da16e8";
        public static PublicClientApplication ClientApplication = new PublicClientApplication(AppId);
        public static string[] scopes = new string[] { "Files.ReadWrite" };

        public AppViewModel VM = new AppViewModel();

        public App()
        {

            // The root page of your application
            MainPage = new MainView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
