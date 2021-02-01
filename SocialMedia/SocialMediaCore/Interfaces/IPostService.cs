using SocialMediaCore.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialMediaCore.QueryFilters;
using SocialMediaCore.CustomEntities;

namespace SocialMediaCore.Interfaces
{
    public interface IPostService
    {
        PagedList<Post> GetPosts(PostQueryFilter filters);
        Task<Post> GetPost(int id);
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int id);
    }
}