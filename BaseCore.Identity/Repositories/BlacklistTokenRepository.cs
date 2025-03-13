using BaseCore.Application.Contracts.Identity;
using BaseCore.Identity.IdentityContext;
using BaseCore.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Identity.Repositories
{
    public class BlacklistTokenRepository : IBlacklistTokenRepository
    {
        private readonly BaseCoreIdentityContext _context;

        public BlacklistTokenRepository(BaseCoreIdentityContext context)
        {
            _context = context;
        }

        public async Task AddTokenToBlacklistAsync(string token, DateTime expiryDate)
        {
            _context.BlackListTokens.Add(new BlacklistToken
            {
                Token = token,
                ExpiryDate = expiryDate
            });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _context.BlackListTokens.AnyAsync(t => t.Token == token);
        }

        public async Task RemoveExpiredTokensAsync()
        {
            var expiredTokens = await _context.BlackListTokens
                .Where(t => t.ExpiryDate < DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.BlackListTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }
    }
}
