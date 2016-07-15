using MvvmCross.Core.ViewModels;
using PropertyManager.Services;
using System.Windows.Input;

namespace PropertyManager.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public virtual void OnResume()
        {
            RaiseAllPropertiesChanged();
        }
    }
}
