using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace ExcelFormsTest.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        private string title;
        public string Title
        {
            get { return title; }
            set { if (title == value) return; title = value; NotifyPropertyChanged(); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { if (message == value) return; message = value; NotifyPropertyChanged(); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { if (isRefreshing == value) return; isRefreshing = value; NotifyPropertyChanged(); }
        }


        public void ShowMessage(string message)
        {
            Message = message;
            Debug.WriteLine(message);
        }

        public void ClearMessage()
        {
            Message = string.Empty;
        }


        public void NotifyPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyAllPropertiesChanged()
        {

            foreach (var prop in GetType().GetRuntimeProperties())
            {
                NotifyPropertyChanged(prop.Name);
            }
        }
    }
}
