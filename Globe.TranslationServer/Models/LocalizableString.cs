using System;

namespace Globe.TranslationServer.Models
{
    public class LocalizableString
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
    }
}
