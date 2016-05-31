using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
