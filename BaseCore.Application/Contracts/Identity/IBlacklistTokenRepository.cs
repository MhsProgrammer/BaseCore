using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Application.Contracts.Identity
{
    public interface IBlacklistTokenRepository
    {
        public Task AddTokenToBlacklistAsync(string token, DateTime expiryDate);

        public Task<bool> IsTokenBlacklistedAsync(string token);

        public Task RemoveExpiredTokensAsync();

    }
}
