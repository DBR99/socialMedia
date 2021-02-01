using Microsoft.EntityFrameworkCore;
using SocialMediaCore.Entities;
using SocialMediaCore.Interfaces;
using SocialMediaInfraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInfraestructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {

        //Cuando se hereda de una clase que tiene un constructor que recibe un parametro la clase hija debe
        //enviarle el parametro a la clase padre

        public PostRepository(SocialMediaContext context): base(context){}
        public async Task<IEnumerable<Post>> GetPostsByUser(int UserId)
        {
            return await _entities.Where(x => x.UserId == UserId).ToListAsync();
        }
    }
} 
