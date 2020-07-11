using Autofac;
using DataContext;
using DataContext.Repositories;
using Domain.DomainServices;
using Iti.Core.Audit;
using Iti.Core.Configuration;
using Iti.Core.DomainEventsBase;
using Iti.Core.RequestTrace;
using Iti.Core.Sequences;
using Iti.Core.UnitOfWorkBase;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Iti.Email;
using Iti.Geolocation;
using Iti.Inversion;
using Iti.Logging;
using Iti.Logging.Job;
using Iti.Passwords;
using Iti.Sms;
using Iti.Voice;
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

            IOC.RegisterInstance(new GoogleGeoLocatorSettings() { ApiKey = "AIzaSyCHs9wcZRaJ8IUbLSqk5Aji5gmcrnu8jec" });

            IOC.RegisterType<IRequestTrace, NullRequestTrace>();

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
