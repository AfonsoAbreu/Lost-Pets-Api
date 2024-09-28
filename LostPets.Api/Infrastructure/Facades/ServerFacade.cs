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

            return addresses
                .Where(address => address.StartsWith("https"))
                .First();
        }
    }
}
