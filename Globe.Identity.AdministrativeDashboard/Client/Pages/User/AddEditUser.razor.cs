using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class AddEditUserDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Parameter]
        public string userId { get; set; }

        protected string Title = "Add";
        public ApplicationUser user = new ApplicationUser();
        //protected List<Cities> cityList = new List<Cities>();

        protected override async Task OnInitializedAsync()
        {
            await GetCityList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Title = "Edit";
                user = await Http.GetJsonAsync<ApplicationUser>("/api/User/" + userId);
            }
        }

        protected async Task GetCityList()
        {
            //cityList = await Http.GetJsonAsync<List<Cities>>("api/Employee/GetCities");
        }

        protected async Task SaveUser()
        {
            if (!string.IsNullOrWhiteSpace(user.Id))
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/User/", user);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/User/", user);
            }

            Cancel();
        }

        public void Cancel()
        {
            UrlNavigationManager.NavigateTo("/users");
        }
    }
}