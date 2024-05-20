using Infrastructure.Data.Entities;

namespace Infrastructure.Facades.Interfaces
{
    public interface IJwtFacade
    {
        string GenerateJwt(User user);
    }
}
