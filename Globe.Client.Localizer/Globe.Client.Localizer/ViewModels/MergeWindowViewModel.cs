using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.ViewModels;
using Globe.Client.Platofrm.Events;
using Prism.Commands;
using Prism.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Globe.Client.Localizer.ViewModels
{
    internal class MergeWindowViewModel : LocalizeWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;
        private readonly IAsyncLocalizableStringService _fileSystemLocalizableStringService;
        private readonly IAsyncLocalizableStringService _httpLocalizableStringService;
        private readonly IStringMergeService _stringMergeService;

        public MergeWindowViewModel(
            IEventAggregator eventAggregator,
            ILogger logger,
            IFileSystemLocalizableStringService fileSystemLocalizableStringService,
            IHttpLocalizableStringService httpLocalizableStringService,
            IStringMergeService stringMergeService,
            ILocalizationAppService localizationService)
            : base(eventAggregator, localizationService)
        {
            _eventAggregator = eventAggregator;
            _logger = logger;
            _fileSystemLocalizableStringService = fileSystemLocalizableStringService;
            _httpLocalizableStringService = httpLocalizableStringService;
            _stringMergeService = stringMergeService;
        }

        IEnumerable<MergeableString> _fileSystemStrings;
        public IEnumerable<MergeableString> FileSystemStrings
        {
            get => _fileSystemStrings;
            set
            {
                SetProperty<IEnumerable<MergeableString>>(ref _fileSystemStrings, value);
            }
        }

        IEnumerable<MergeableString> _httpStrings;
        public IEnumerable<MergeableString> HttpStrings
        {
            get => _httpStrings;
            set
            {
                SetProperty<IEnumerable<MergeableString>>(ref _httpStrings, value);
            }
        }

        IEnumerable<LocalizableString> _mergedStrings;
        public IEnumerable<LocalizableString> MergedStrings
        {
            get => _mergedStrings;
            set
            {
                SetProperty<IEnumerable<LocalizableString>>(ref _mergedStrings, value);
            }
        }

        private DelegateCommand _loadCommand = null;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);
                try
                {
                    var fileSystemStrings = await _fileSystemLocalizableStringService.GetAllAsync();
                    var httpStrings = await _httpLocalizableStringService.GetAllAsync();
                    UpdateMergeableStrings(fileSystemStrings.ToList(), httpStrings.ToList());
                    AutoMergeCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Load Failed");

                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Error,
                        Text = this.LocalizationAppService.Resolve(LanguageKeys.Error_during_server_communication)
                    });
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            }));

        private void UpdateMergeableStrings(List<LocalizableString> fileSystemStrings, List<LocalizableString> httpStrings)
        {
            this.FileSystemStrings = fileSystemStrings.Select(fileSystemItem => new MergeableString
            {
                Id = fileSystemItem.Id,
                Key = fileSystemItem.Key,
                Language = fileSystemItem.Language,
                SavedOn = fileSystemItem.SavedOn,
                Value = fileSystemItem.Value,
                IsMergeable = !httpStrings.Exists(item => item.Key == fileSystemItem.Key) || httpStrings.Exists(httpItem => httpItem.Key == fileSystemItem.Key && httpItem.Value != fileSystemItem.Value)
            });

            this.HttpStrings = httpStrings.Select(httpItem => new MergeableString
            {
                Id = httpItem.Id,
                Key = httpItem.Key,
                Language = httpItem.Language,
                SavedOn = httpItem.SavedOn,
                Value = httpItem.Value,
                IsMergeable = !fileSystemStrings.Exists(item => item.Key == httpItem.Key) || fileSystemStrings.Exists(fileSystemItem => fileSystemItem.Key == httpItem.Key && fileSystemItem.Value != httpItem.Value)
            });
        }

        private DelegateCommand _autoMergeCommand = null;
        public DelegateCommand AutoMergeCommand =>
            _autoMergeCommand ?? (_autoMergeCommand = new DelegateCommand(() =>
            {
                this.MergedStrings = _stringMergeService.Merge(this.FileSystemStrings, this.HttpStrings);
                SaveCommand.RaiseCanExecuteChanged();
            },
            () =>
            {
                return this.FileSystemStrings != null && this.HttpStrings != null && (this.FileSystemStrings.Count() > 0 || this.HttpStrings.Count() > 0);
            }));

        private DelegateCommand _saveCommand = null;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

                try
                {
                    await _httpLocalizableStringService.SaveAsync(this.MergedStrings);
                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Information,
                        Text = this.LocalizationAppService.Resolve(LanguageKeys.Operation_successfully_completed)
                    });
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Save Failed");

                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Error,
                        Text = this.LocalizationAppService.Resolve(LanguageKeys.Error_during_server_communication)
                    });
                }
                finally
                {
                    _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
                }
            },
            () =>
            {
                return this.MergedStrings != null && this.MergedStrings.Count() > 0;
            }));

        private DelegateCommand<LocalizableString> _selectFromFileSystemCommand = null;
        public DelegateCommand<LocalizableString> SelectFromFileSystemCommand =>
            _selectFromFileSystemCommand ?? (_selectFromFileSystemCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (localizableString == null || MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));

        private DelegateCommand<LocalizableString> _selectFromServerCommand = null;
        public DelegateCommand<LocalizableString> SelectFromServerCommand =>
            _selectFromServerCommand ?? (_selectFromServerCommand = new DelegateCommand<LocalizableString>((localizableString) =>
            {
                if (localizableString == null || MergedStrings == null)
                    return;

                foreach (var item in MergedStrings)
                {
                    if (localizableString.Key == item.Key)
                    {
                        item.SavedOn = localizableString.SavedOn;
                        item.Value = localizableString.Value;

                        break;
                    }
                }

                MergedStrings = MergedStrings.ToList();
            }));

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);

            this.FileSystemStrings = new List<MergeableString>();
            this.HttpStrings = new List<MergeableString>();
            this.MergedStrings = new List<LocalizableString>();
            AutoMergeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
