using System.Reflection;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    public static void AddClientConfigurations(this IConfigurationBuilder builder)
    {
        var assembly = Assembly.Load("BitPlatformTemplate.Client.Core");
        builder.AddJsonStream(assembly.GetManifestResourceStream("BitPlatformTemplate.Client.Core.appsettings.json")!);

        if (BuildConfiguration.IsDebug())
        {
            builder.AddJsonStream(assembly.GetManifestResourceStream("BitPlatformTemplate.Client.Core.appsettings.Development.json")!);
        }
    }
}
