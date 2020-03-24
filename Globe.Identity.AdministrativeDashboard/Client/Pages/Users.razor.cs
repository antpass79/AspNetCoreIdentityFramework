using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class UsersDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        protected string SearchString { get; set; }

        protected ApplicationUser[] users;
        protected ApplicationUser selectedUser;

        protected bool ShowConfirmation { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetUsers();
        }

        async protected Task EditUser(ApplicationUser user)
        {
            await Http.SendJsonAsync(HttpMethod.Put, "/api/User", user);
        }

        protected void DeleteUserConfirm(ApplicationUser user)
        {
            this.selectedUser = users.FirstOrDefault(item => item.Id == user.Id);
            this.ShowConfirmation = true;
        }

        async protected Task DeleteUser(ApplicationUser user)
        {
            await Http.DeleteAsync($"api/User/{user.Id}");
            ShowConfirmation = false;
        }

        async protected Task SearchUsers()
        {
            await GetUsers();
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(user => user.FullName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            }
        }

        async private Task GetUsers()
        {
            users = await Http.GetJsonAsync<ApplicationUser[]>("api/User");
        }
    }
}