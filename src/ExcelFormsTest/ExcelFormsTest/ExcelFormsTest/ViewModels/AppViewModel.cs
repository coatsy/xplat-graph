using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class AppViewModel : ViewModelBase
    {



        private AuthenticationResult authResult;
        public AuthenticationResult AuthResult
        {
            get { return authResult; }
            set
            {
                if (authResult == value)
                    return;
                authResult = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsLoggedIn");
                NotifyPropertyChanged("IsNotLoggedIn");
                NotifyPropertyChanged("UserName");
            }
        }

        public bool IsLoggedIn
        {
            get { return (AuthResult != null && AuthResult.ExpiresOn > DateTimeOffset.UtcNow); }
        }

        public bool IsNotLoggedIn
        {
            get { return !IsLoggedIn; }
        }

        public string UserName
        {
            get { return AuthResult == null ? string.Empty : $"{AuthResult.User.Name} ({AuthResult.User.DisplayableId})"; }
        }

        private CommandBase loginCommand;

        public CommandBase LoginCommand
        {
            get
            {
                loginCommand = loginCommand ?? new CommandBase(DoLoginCommand);
                return loginCommand;
            }
        }

        private async void DoLoginCommand()
        {
            try
            {
                AuthResult = await App.ClientApplication.AcquireTokenSilentAsync(App.scopes);
            }
            catch 
            {
                DoForceLoginCommand();
            }
        }

        private CommandBase forceLoginCommand;

        public CommandBase ForceLoginCommand
        {
            get
            {
                forceLoginCommand = forceLoginCommand ?? new CommandBase(DoForceLoginCommand);
                return forceLoginCommand;
            }
        }

        private async void DoForceLoginCommand()
        {
            try
            {
                AuthResult = await App.ClientApplication.AcquireTokenAsync(App.scopes);
            }
            catch (MsalServiceException)
            {
                AuthResult = null;
            }
        }

        private CommandBase logoutCommand;

        public CommandBase LogoutCommand
        {
            get
            {
                logoutCommand = logoutCommand ?? new CommandBase(DoLogoutCommand);
                return logoutCommand;
            }
        }

        private void DoLogoutCommand()
        {
            AuthResult = null;
        }
    }
}
