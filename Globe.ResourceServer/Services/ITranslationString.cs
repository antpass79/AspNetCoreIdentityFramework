using Globe.ResourceServer.DTOs;
using System.Collections.Generic;

namespace Globe.ResourceServer.Services
{
    public interface ITranslationStringService
    {
        IEnumerable<TranslationString> GetAll();
    }
}
