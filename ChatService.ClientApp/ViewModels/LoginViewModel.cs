using ChatService.ClientApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.ClientApp.ViewModels
{
    /// <summary>
    /// Model for login window
    /// </summary>
    public class LoginViewModel : BindableBase
    {
        public LoginViewModel()
        {
            loginCommand = new RelayCommand(Login);
        }

        #region Properties

        private string username;

        /// <summary>
        /// Stores user login name typed in input
        /// </summary>
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private RelayCommand loginCommand;

        /// <summary>
        /// Fires when user clicked Login button
        /// </summary>
        public RelayCommand LoginCommand
        {
            get { return loginCommand; }
            set { SetProperty(ref loginCommand, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logins user into service
        /// </summary>
        private void Login()
        {
            if (string.IsNullOrEmpty(Username))
                return;
            Proxy.Instance.LoginUser(Username);
        }

        #endregion
    }
}
