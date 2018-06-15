using DataContext;
using DataContext.Repositories;
using Domain.DomainServices;
using Iti.Core.Configuration;
using Iti.Core.DomainEvents;
using Iti.Core.Sequences;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Core.UserTracker;
using Iti.Email;
using Iti.Inversion;
using Iti.Logging;
using Iti.Logging.Job;
using Iti.Passwords;
using Iti.Sms;
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

            IOC.RegisterType<IUserTracker, EfUserTracker>();
            IOC.RegisterType<IUserTrackingDataContext, SampleDataContext>();

            ConfigureLogging();
            ConfigureEmail();
            ConfigureSms();
            ConfigureDomainEvents();
            ConfigureSequences();
            ConfigureApplication();
            ConfigureJobProcessors();
            ConfigurePasswords();
        }

        private static void Settings<T>()
            where T : class
        {
            var inst = _configurationLoader?.GetSettings<T>();
            if (inst == null)
                return;

            IOC.RegisterInstance<T>(inst);
        }

        private static void ConfigureJobProcessors()
        {
            Settings<EmailJobSettings>();
            IOC.RegisterType<EmailJobProcessor>();

            Settings<SmsJobSettings>();
            IOC.RegisterType<SmsJobProcessor>();

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
            DomainEvents.ContextLocator = UnitOfWork.Current<SampleDataContext>;
            IOC.RegisterType<IDomainEventProcessor, SingleTaskDomainEventProcessor>();
        }

        private static void ConfigureLogging()
        {
#if DEBUG
            Log.DebugEnabled = true;
#endif
            IOC.RegisterType<ILogDataContext, SampleDataContext>();
            IOC.RegisterType<ILogWriter, EfLogWriter>();
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

        private static void ConfigurePasswords()
        {
            IOC.RegisterType<IPasswordEncoder, DefaultPasswordEncoder>();
        }
    }
}
