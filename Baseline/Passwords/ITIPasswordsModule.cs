using Autofac;

namespace ITI.Baseline.Passwords;

public class ITIPasswordsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DefaultPasswordValidator>().As<IPasswordValidator>();
        builder.RegisterType<Pkbdf2PasswordEncoder>().As<IPasswordEncoder>();
    }
}
