using DataContext;
using DataContext.Repositories;
using Domain.DomainServices;
using Iti.Baseline.Core.Configuration;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.RequestTrace;
using Iti.Baseline.Core.Sequences;
using Iti.Baseline.Core.UnitOfWorkBase;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Email;
using Iti.Baseline.Geolocation;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.Logging.Job;
using Iti.Baseline.Passwords;
using Iti.Baseline.Sms;
using Iti.Baseline.Voice;
using SampleApp.Application;
using SampleApp.Application.Interfaces;
using SampleApp.Auth;

namespace AppConfig
{
    public static class DefaultAppConfig
    {
        private static IConfigurationLoader _configurationLoader;

        public static void Initialize(IConfigurationLoader configurationLoader = null)
        {
            _configurationLoader = configurationLoader;

            //
            // EXTERNAL CONFIG REQUIRED:
            //
            // IOC.RegisterType<IAuthContext, AppAuthContext>();
            //

            IOC.Initialize();

            DataMapConfig.Initialize();

            IOC.RegisterLifetimeScope<IUnitOfWork, UnitOfWorkImpl>();
            IOC.RegisterLifetimeScope<DomainEvents>();

            IOC.RegisterType<SampleDataContext>();

            IOC.RegisterType<IAuthScopeResolver, SampleAuthScopeResolver>();

            IOC.RegisterInstance(new GoogleGeoLocatorSettings() { ApiKey = "AIzaSyCHs9wcZRaJ8IUbLSqk5Aji5gmcrnu8jec" });

            IOC.RegisterType<IRequestTrace, NullRequestTrace>();

            IOC.RegisterType<IAuthScopeResolver, SampleAuthScopeResolver>();

            ConfigureLogging();
            ConfigureEmail();
            ConfigureSms();
            ConfigureVoice();
            ConfigureDomainEvents();
            ConfigureSequences();
            ConfigureApplication();
            ConfigureJobProcessors();
            ConfigurePasswords();
            ConfigureGeolocation();
        }

        private static void Settings<T>()
            where T : class
        {
            var inst = _configurationLoader?.GetSettings<T>();
            if (inst == null)
                return;

            IOC.RegisterInstance(inst);
        }

        private static void ConfigureJobProcessors()
        {
            Settings<EmailJobSettings>();

            Settings<SmsJobSettings>();

            Settings<LogCleanupSettings>();
            IOC.RegisterType<LogCleanupJobProcessor>();
        }

        private static void ConfigureApplication()
        {
            IOC.RegisterType<IAppPermissions, AppPermissions>();

            IOC.RegisterType<IFooAppService, FooAppService>();
            IOC.RegisterType<IFooRepository, EfFooRepository>();
            IOC.RegisterType<IFooQueries, EfFooQueries>();

            IOC.RegisterType<IFooFighter, FooFighter>();
        }

        private static void ConfigureSequences()
        {
            IOC.RegisterType<ISequenceResolver, EfSequenceResolver<SampleDataContext>>();
        }

        private static void ConfigureDomainEvents()
        {
        }

        private static void ConfigureLogging()
        {
            IOC.RegisterType<ILogger, Logger>();

            IOC.RegisterType<ILogDataContext, SampleDataContext>();
            IOC.RegisterType<ILogWriter, DbLogWriter>();

            IOC.RegisterType<IDbLoggerSettings, DbLoggerSettings>();
        }

        private static void ConfigureEmail()
        {
            IOC.RegisterType<IEmailSender, QueuedEmailSender>();
            IOC.RegisterType<IEmailRepository, EfEmailRepository>();
        }

        private static void ConfigureSms()
        {
            IOC.RegisterType<ISmsSender, QueuedSmsSender>();
            IOC.RegisterType<ISmsRepository, EfSmsRepository>();
        }

        private static void ConfigureVoice()
        {
            IOC.RegisterType<IVoiceSender, QueuedVoiceSender>();
            IOC.RegisterType<IVoiceRepository, EfVoiceRepository>();
        }

        private static void ConfigurePasswords()
        {
            IOC.RegisterType<IPasswordEncoder<EncodedPassword>, DefaultPasswordEncoder>();

            Settings<DefaultPasswordEncoderSettings>();
        }

        private static void ConfigureGeolocation()
        {
            Settings<GoogleGeoLocatorSettings>();
            IOC.RegisterType<IGeolocator, GoogleGeoLocator>();
        }
    }
}
