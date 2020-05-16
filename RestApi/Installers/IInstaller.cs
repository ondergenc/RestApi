using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RestApi.Installers
{
    public interface Installer
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
