using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
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

        protected ApplicationRoleDTO[] roles;
        protected ApplicationRoleDTO selectedRole;
        protected bool ShowConfirmation { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetRoles();
        }

        async protected Task EditRole(ApplicationRoleDTO role)
        {
            await Http.SendJsonAsync(HttpMethod.Put, "/api/Role", role);
        }

        protected void DeleteRoleConfirm(ApplicationRoleDTO role)
        {
            this.selectedRole = roles.FirstOrDefault(item => item.Id == role.Id);
            this.ShowConfirmation = true;
        }

        async protected Task DeleteRole(ApplicationRoleDTO role)
        {
            await Http.DeleteAsync($"api/Role/{role.Id}");
            await GetRoles();
            ShowConfirmation = false;
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
            roles = await Http.GetJsonAsync<ApplicationRoleDTO[]>("api/Role");
        }
    }
}