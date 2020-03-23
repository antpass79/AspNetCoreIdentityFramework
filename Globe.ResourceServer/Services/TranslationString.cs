using Globe.ResourceServer.DTOs;
using System.Collections.Generic;

namespace Globe.ResourceServer.Services
{
    public class TranslationStringService : ITranslationStringService
    {
        public IEnumerable<TranslationString> GetAll()
        {
            return new[]
            {
                new TranslationString
                {
                    Key = "Key 1",
                    Value = "Value 1"
                },
                new TranslationString
                {
                    Key = "Key 2",
                    Value = "Value 2"
                },
                new TranslationString
                {
                    Key = "Key 3",
                    Value = "Value 3"
                },
            };
        }
    }
}
