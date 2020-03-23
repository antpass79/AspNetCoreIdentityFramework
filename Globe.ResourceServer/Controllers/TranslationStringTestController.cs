using Globe.ResourceServer.DTOs;
using Globe.ResourceServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Esaote.TranslationServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TranslationStringTestController : Controller
    {
        public ITranslationStringService TranslationStringService { get; }

        public TranslationStringTestController(ITranslationStringService translationStringService)
        {
            TranslationStringService = translationStringService;
        }

        [HttpGet, Authorize(Policy = "AdministrativeRights")]
        public IEnumerable<TranslationString> Get()
        {
            return this.TranslationStringService.GetAll();
        }

        [HttpGet("getfirst")]
        [Authorize(Policy = "GuestRights")]
        public TranslationString GetFirst()
        {
            return this.TranslationStringService.GetAll().First();
        }
    }
}
