﻿using Globe.Identity.AdministrativeDashboard.Server.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IAsyncUserService _userService;

        public UserController(IAsyncUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationUserDTO>> Get()
        {
            return await _userService.GetAsync();
        }

        [HttpGet("{userId}")]
        async public Task<UserWithRoles> Get(string userId)
        {
            return await _userService.FindByIdAsync(userId);
        }

        [HttpPost]
        async public Task Post([FromBody] UserWithRoles userWithRoles)
        {
            await _userService.InsertAsync(userWithRoles);
        }

        [HttpPut]
        async public Task Put([FromBody] UserWithRoles userWithRoles)
        {
            await _userService.UpdateAsync(userWithRoles);
        }

        [HttpDelete("{userId}")]
        async public Task Delete(string userId)
        {
            await _userService.DeleteAsync(userId);
        }
    }
}
