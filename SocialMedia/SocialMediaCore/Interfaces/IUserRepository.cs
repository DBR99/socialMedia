using SocialMediaCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaCore.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUser();
        Task<User> GetUser(int id);
    }
}