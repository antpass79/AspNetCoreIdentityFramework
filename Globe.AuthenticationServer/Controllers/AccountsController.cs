﻿using Globe.Identity.Authentication.Core.Models;
using Globe.Identity.Authentication.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.AuthenticationServer.Controllers
{
    [Route("/api/accounts")]
    public class AccountsController : Controller
    {
        private readonly IAsyncRegisterService _accountsService;

        public AccountsController(IAsyncRegisterService accountsService)
        {
            this._accountsService = accountsService;
        }

        [HttpPost("register")]
        async public Task<IActionResult> Post([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid Registration", "registration");

            var result = await _accountsService.RegisterAsync(registration);
            if (result.Successful)
                return Ok();

            this.BuildErrors(result.Errors);

            return BadRequest();
        }

        protected void BuildErrors(IEnumerable<string> errors)
        {
            errors.ToList().ForEach(error =>
            {
                ModelState.AddModelError(string.Empty, error);
            });
        }
    }
}
