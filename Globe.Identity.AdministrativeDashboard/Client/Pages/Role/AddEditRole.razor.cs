using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class AddEditRoleDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Parameter]
        public string roleId { get; set; }

        protected string Title = "Add";
        public ApplicationRole role = new ApplicationRole();
        //protected List<Cities> cityList = new List<Cities>();

        protected override async Task OnInitializedAsync()
        {
            await GetCityList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(roleId))
            {
                Title = "Edit";
                role = await Http.GetJsonAsync<ApplicationRole>("/api/Role/" + roleId);
            }
        }

        protected async Task GetCityList()
        {
            //cityList = await Http.GetJsonAsync<List<Cities>>("api/Employee/GetCities");
        }

        protected async Task SaveRole()
        {
            if (!string.IsNullOrWhiteSpace(role.Id))
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/Role/", role);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/Role/", role);
            }

            Cancel();
        }

        public void Cancel()
        {
            UrlNavigationManager.NavigateTo("/roles");
        }
    }
}