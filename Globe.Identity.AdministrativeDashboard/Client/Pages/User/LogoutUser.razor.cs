using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class LogoutUserDataModel : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
            UrlNavigationManager.NavigateTo("/");
        }
    }
}