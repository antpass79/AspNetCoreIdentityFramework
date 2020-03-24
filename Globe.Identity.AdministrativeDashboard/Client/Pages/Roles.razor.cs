using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class RolesDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        protected string SearchString { get; set; }

        protected ApplicationRole[] roles;
        protected ApplicationRole selectedRole;


        protected override async Task OnInitializedAsync()
        {
            await GetRoles();
        }

        async protected Task EditRole(ApplicationRole role)
        {
            await Http.SendJsonAsync(HttpMethod.Put, "/api/Role", role);
        }

        protected void DeleteRoleConfirm(ApplicationRole role)
        {
            this.selectedRole = roles.FirstOrDefault(item => item.Id == role.Id);
        }

        async protected Task DeleteRole(ApplicationRole role)
        {
            await Http.DeleteAsync($"api/Role/{role.Id}");
        }

        async protected Task SearchRoles()
        {
            await GetRoles();
            if (!string.IsNullOrEmpty(SearchString))
            {
                roles = roles.Where(role => role.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            }
        }

        async private Task GetRoles()
        {
            roles = await Http.GetJsonAsync<ApplicationRole[]>("api/Role");
        }
    }
}