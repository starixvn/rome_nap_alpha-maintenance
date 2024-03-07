using Alpha.Maintenance.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Maintenance.Service.Interface
{
    public interface IUMTokenService
    {
        Task<IDictionary<string, SiteToken>> GetTokens();

        IDictionary<string, SiteToken> GetOldTokens();
    }
}