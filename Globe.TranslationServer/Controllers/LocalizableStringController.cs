using Globe.TranslationServer.Models;
using Globe.TranslationServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class LocalizableStringController
    {
        public LocalizableStringRepository LocalizableStringRepository { get; }

        public LocalizableStringController(LocalizableStringRepository localizableStringRepository)
        {
            LocalizableStringRepository = localizableStringRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        async public Task<IEnumerable<LocalizableString>> Get()
        {
            var strings = new List<LocalizableString>
            {
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_0",
                    Language = "en",
                    Value = "STRING 0"
                },
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_1",
                    Language = "en",
                    Value = "STRING 1"
                },
                new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    Key = "STRING_2",
                    Language = "en",
                    Value = "STRING 2"
                }
            };

            return await Task.FromResult(strings);
        }
    }
}
