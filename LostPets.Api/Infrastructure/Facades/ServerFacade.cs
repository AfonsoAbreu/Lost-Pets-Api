using Infrastructure.Exceptions;
using Infrastructure.Facades.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Infrastructure.Facades
{
    public class ServerFacade : IServerFacade
    {
        private readonly IServer _server;

        public ServerFacade(IServer server)
        {
            _server = server;
        }

        public string GetHostedUrl()
        {
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
