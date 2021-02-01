using SocialMediaCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaCore.Interfaces
{
     public interface IPostRepository : IRepository<Post>
    {
       Task<IEnumerable<Post>> GetPostsByUser(int UserId);
      
    }
}
