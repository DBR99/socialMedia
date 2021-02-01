using Microsoft.EntityFrameworkCore;
using SocialMediaCore.Entities;
using SocialMediaInfraestructure.Data;
using System.Threading.Tasks;

namespace SocialMediaInfraestructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(SocialMediaContext context) : base(context) { }

        public async Task<Security> GetLoginByCredentials(UserLogin login) {
            return await _entities.FirstOrDefaultAsync(x => x.User == login.User && x.Password == login.Password);
            }
            
        
    }
}
