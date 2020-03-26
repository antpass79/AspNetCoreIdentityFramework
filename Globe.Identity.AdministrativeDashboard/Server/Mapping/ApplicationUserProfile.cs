using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();
        }
    }
}
