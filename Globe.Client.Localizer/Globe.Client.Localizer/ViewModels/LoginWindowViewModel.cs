using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Prism.Commands;
using Prism.Events;
using System.Security;

namespace Globe.Client.Localizer.ViewModels
{
    internal class LoginWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IViewNavigationService _viewNavigationService;
        private readonly IAsyncLoginService _loginService;

        public LoginWindowViewModel(IViewNavigationService viewNavigationService, IAsyncLoginService loginService, IEventAggregator eventAggregator, ILocalizationAppService localizationAppService)
            : base(eventAggregator, localizationAppService)
        {
            _viewNavigationService = viewNavigationService;
            _loginService = loginService;
        }

        string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty<string>(ref _userName, value);
            }
        }
        SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                SetProperty<SecureString>(ref _password, value);
            }
        }
        LoginResult _loginResult = new LoginResult();
        public LoginResult LoginResult
        {
            get => _loginResult;
            set
            {
                SetProperty<LoginResult>(ref _loginResult, value);
            }
        }

        private DelegateCommand _loginCommand = null;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(async () =>
            {
                LoginResult = new LoginResult();
                LoginResult = await _loginService.LoginAsync(new Credentials
                {
                    UserName = this.UserName,
                    Password = this.Password.ToPlainString()
                });

                ClearFields();
                if (LoginResult.Successful)
                {
                    _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
                }
            }));

        private DelegateCommand _cancelCommand = null;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(() =>
            {
                ClearFields();
                _viewNavigationService.NavigateTo(ViewNames.HOME_VIEW);
            }));

        private void ClearFields()
        {
            this.UserName = string.Empty;
            this.Password = new SecureString();
        }
    }
}
