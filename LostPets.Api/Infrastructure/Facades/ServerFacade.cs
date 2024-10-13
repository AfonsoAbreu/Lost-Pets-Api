using Infrastructure.Exceptions;
using Infrastructure.Facades.Base;
using Infrastructure.Facades.Interfaces;
using Infrastructure.Facades.Settings;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;

namespace Infrastructure.Facades
{
    public class ServerFacade : BaseFacadeWithSettings<ServerFacadeSettings>, IServerFacade
    {
        private readonly IServer _server;

        public ServerFacade(IServer server, IOptions<ServerFacadeSettings> options)
            : base(options)
        {
            _server = server;
        }

        public string GetHostedUrl()
        {
            if (_settings.HostedUrl != null)
            {
                return _settings.HostedUrl;
            }

            var feature = _server.Features.Get<IServerAddressesFeature>() 
                ?? throw new NoHostedUrlDetectedInfrastructureException(NoHostedUrlDetectedInfrastructureException.DefaultMessage());

            var addresses = feature.Addresses;

            if (addresses.Count == 0)
            {
                throw new NoHostedUrlDetectedInfrastructureException(NoHostedUrlDetectedInfrastructureException.DefaultMessage());
            }

            string? httpsAddress = addresses
                .Where(address => address.StartsWith("https"))
                .FirstOrDefault();

            if (httpsAddress != null)
            {
                return httpsAddress;
            }

            string? httpAddress = addresses
                .Where(address => address.StartsWith("http"))
                .FirstOrDefault();

            return httpAddress
                ?? throw new NoHostedUrlDetectedInfrastructureException(NoHostedUrlDetectedInfrastructureException.DefaultMessage());
        }
    }
}
