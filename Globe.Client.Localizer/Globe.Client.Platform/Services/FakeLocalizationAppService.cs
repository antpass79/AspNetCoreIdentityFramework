using Globe.Client.Platform.Assets.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class FakeLocalizationAppService : ILocalizationAppService
    {
        private readonly string LANGUAGE_EN = "en";
        private readonly string LANGUAGE_IT = "it";

        Dictionary<string, string> _english = new Dictionary<string, string>();
        Dictionary<string, string> _italian = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _languages = new Dictionary<string, Dictionary<string, string>>();

        Dictionary<string, string> _current;

        public FakeLocalizationAppService()
        {
            _english.Add(nameof(LanguageKeys.Hello), LanguageKeys.Hello);
            _english.Add(nameof(LanguageKeys.Options), LanguageKeys.Options);
            _english.Add(nameof(LanguageKeys.Home), LanguageKeys.Home);
            _english.Add(nameof(LanguageKeys.Translation), LanguageKeys.Translation);
            _english.Add(nameof(LanguageKeys.Merge), LanguageKeys.Merge);
            _english.Add(nameof(LanguageKeys.Load), LanguageKeys.Load);
            _english.Add(nameof(LanguageKeys.Save), LanguageKeys.Save);
            _english.Add(nameof(LanguageKeys.AutoMerge), "Auto Merge");
            _english.Add(nameof(LanguageKeys.Cancel), LanguageKeys.Cancel);
            _english.Add(nameof(LanguageKeys.UserName), "User Name");

            _english.Add(nameof(LanguageKeys.Please_enter_your_details), "Please enter your details");

            _english.Add(nameof(LanguageKeys.Operation_successfully_completed), "Operation successfully completed");
            _english.Add(nameof(LanguageKeys.Error_during_server_communication), "Error during server communication");
            _english.Add(nameof(LanguageKeys.Strings_from_file_system), "Strings from file system");
            _english.Add(nameof(LanguageKeys.Impossible_to_retrieve_strings), "Impossible to retrieve strings");
            _english.Add(nameof(LanguageKeys.Strings_saved_in_file_system), "Strings saved in file system");
            _english.Add(nameof(LanguageKeys.Impossible_to_save_strings), "Impossible to save strings");

            _italian.Add(nameof(LanguageKeys.Hello), "Ciao");
            _italian.Add(nameof(LanguageKeys.Options), "Opzioni");
            _italian.Add(nameof(LanguageKeys.Home), LanguageKeys.Home);
            _italian.Add(nameof(LanguageKeys.Translation), "Traduzione");
            _italian.Add(nameof(LanguageKeys.Merge), "Unisci");
            _italian.Add(nameof(LanguageKeys.Load), "Carica");
            _italian.Add(nameof(LanguageKeys.Save), "Salva");
            _italian.Add(nameof(LanguageKeys.AutoMerge), "Unisci");
            _italian.Add(nameof(LanguageKeys.Cancel), "Cancella");
            _italian.Add(nameof(LanguageKeys.UserName), "Nome Utente");

            _italian.Add(nameof(LanguageKeys.Please_enter_your_details), "Inserisci i dettagli");

            _italian.Add(nameof(LanguageKeys.Operation_successfully_completed), "Operazione eseguita con successo");
            _italian.Add(nameof(LanguageKeys.Error_during_server_communication), "Errore durante la comunicazione con il server");
            _italian.Add(nameof(LanguageKeys.Strings_from_file_system), "Stringhe caricate da file system");
            _italian.Add(nameof(LanguageKeys.Impossible_to_retrieve_strings), "Impossibile recuperare le stringhe");
            _italian.Add(nameof(LanguageKeys.Strings_saved_in_file_system), "Stringhe salvate nel file siystem");
            _italian.Add(nameof(LanguageKeys.Impossible_to_save_strings), "Impossibile salvare le stringhe");

            _languages.Add(LANGUAGE_EN, _english);
            _languages.Add(LANGUAGE_IT, _italian);

            _selectedLanguage = LANGUAGE_EN;
            _current = _languages[LANGUAGE_EN];
        }

        private string _selectedLanguage;
        public string SelectedLanguage { get => _selectedLanguage; }

        async public Task<Dictionary<string, string>> LoadAsync(string language)
        {
            if (_languages.ContainsKey(language))
                _selectedLanguage = language;
            else
                _selectedLanguage = LANGUAGE_EN;

            _current = _languages[_selectedLanguage];

            return await Task.FromResult(_current);
        }

        public string Resolve(string key)
        {
            if (!_current.ContainsKey(key))
                return $"{key} MISSED";

            return _current[key];
        }
    }
}
