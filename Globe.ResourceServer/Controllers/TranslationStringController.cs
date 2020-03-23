using Globe.ResourceServer.DTOs;
using Globe.ResourceServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Globe.ResourceServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TranslationStringController : Controller
    {
        public ITranslationStringService TranslationStringService { get; }

        public TranslationStringController(ITranslationStringService translationStringService)
        {
            TranslationStringService = translationStringService;
        }

        [HttpGet, Authorize(Policy = "ApiUser")]
        public IEnumerable<TranslationString> Get()
        {
            return this.TranslationStringService.GetAll();
        }

        [HttpGet("getfirst")]
        public TranslationString GetFirst()
        {
            return this.TranslationStringService.GetAll().First();
        }
    }
}
