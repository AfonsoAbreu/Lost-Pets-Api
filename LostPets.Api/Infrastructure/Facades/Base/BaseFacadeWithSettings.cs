using Infrastructure.Facades.Settings.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Facades.Base
{
    public abstract class BaseFacadeWithSettings<TSettings>(IOptions<TSettings> options) where TSettings : class, IBaseFacadeSettings
    {
        protected readonly TSettings _settings = options.Value;
    }
}
