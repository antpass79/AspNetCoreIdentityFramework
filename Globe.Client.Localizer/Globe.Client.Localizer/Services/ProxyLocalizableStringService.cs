﻿using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    public interface IProxyLocalizableStringService : IAsyncLocalizableStringService
    {}

    public class ProxyLocalizableStringService : IProxyLocalizableStringService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;
        private readonly ICheckConnectionService _checkConnectionService;
        private readonly IAsyncLocalizableStringService _fileSystemLocalizableStringService;
        private readonly IAsyncLocalizableStringService _httpLocalizableStringService;
        private readonly ILocalizationAppService _localizationService;
        
        public ProxyLocalizableStringService(
            IEventAggregator eventAggregator,
            ILogger logger,
            ICheckConnectionService checkConnectionService,
            IFileSystemLocalizableStringService fileSystemLocalizableStringService,
            IHttpLocalizableStringService httpLocalizableStringService,
            ILocalizationAppService localizationService)
        {
            _eventAggregator = eventAggregator;
            _logger = logger;
            _checkConnectionService = checkConnectionService;
            _fileSystemLocalizableStringService = fileSystemLocalizableStringService;
            _httpLocalizableStringService = httpLocalizableStringService;
            _localizationService = localizationService;
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

            IEnumerable<LocalizableString> strings = new List<LocalizableString>();

            try
            {
                if (_checkConnectionService.IsConnectionAvailable())
                {
                    try
                    {
                        strings = await _httpLocalizableStringService.GetAllAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, "Http communication Failed");

                        _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                        {
                            MessageType = MessageType.Error,
                            Text = _localizationService.Resolve(LanguageKeys.Error_during_server_communication)
                        });

                        try
                        {
                            strings = await _fileSystemLocalizableStringService.GetAllAsync();

                            _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                            {
                                MessageType = MessageType.Warning,
                                Text = _localizationService.Resolve(LanguageKeys.Strings_from_file_system)
                            });
                        }
                        catch (Exception innerException)
                        {
                            _logger.Error(innerException, "File System communication Failed");

                            _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                            {
                                MessageType = MessageType.Error,
                                Text = _localizationService.Resolve(LanguageKeys.Impossible_to_retrieve_strings)
                            });
                        }
                    }
                }
                else
                    strings = await _fileSystemLocalizableStringService.GetAllAsync();
            }
            finally
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
            }

            return strings;
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);
            try
            {
                if (_checkConnectionService.IsConnectionAvailable())
                {
                    try
                    {
                        await _httpLocalizableStringService.SaveAsync(strings);
                        await _fileSystemLocalizableStringService.SaveAsync(strings);

                        _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                        {
                            MessageType = MessageType.Information,
                            Text = _localizationService.Resolve(LanguageKeys.Operation_successfully_completed)
                        });
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, "Http communication Failed");

                        _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                        {
                            MessageType = MessageType.Error,
                            Text = _localizationService.Resolve(LanguageKeys.Error_during_server_communication)
                        });

                        try
                        {
                            await _fileSystemLocalizableStringService.SaveAsync(strings);

                            _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                            {
                                MessageType = MessageType.Warning,
                                Text = _localizationService.Resolve(LanguageKeys.Strings_saved_in_file_system)
                            });
                        }
                        catch (Exception innerException)
                        {
                            _logger.Error(innerException, "File System communication Failed");

                            _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                            {
                                MessageType = MessageType.Error,
                                Text = _localizationService.Resolve(LanguageKeys.Impossible_to_save_strings)
                            });
                        }
                    }
                }
                else
                {
                    await _fileSystemLocalizableStringService.SaveAsync(strings);

                    _eventAggregator.GetEvent<StatusBarMessageChangedEvent>().Publish(new StatusBarMessage
                    {
                        MessageType = MessageType.Warning,
                        Text = _localizationService.Resolve(LanguageKeys.Strings_saved_in_file_system)
                    });
                }
            }
            finally
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
            }
        }
    }
}
