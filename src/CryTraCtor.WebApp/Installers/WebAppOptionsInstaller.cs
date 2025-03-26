namespace CryTraCtor.WebApp.Installers;

using CryTraCtor.Common.Installers;
using Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class WebAppOptionsInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();

        serviceCollection.Configure<FileUploadOptions>(
            configuration.GetSection(nameof(FileUploadOptions)));
    }
}
