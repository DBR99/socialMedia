using Microsoft.EntityFrameworkCore;
using SocialMediaCore;
using SocialMediaCore.Entities;
using SocialMediaInfraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInfraestructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        //se cambió private a protected para que _entities fuera visible a las clases que hereden base repository
        private readonly SocialMediaContext _context;
        protected  readonly DbSet<T> _entities;

        public BaseRepository(SocialMediaContext context) {
            _context = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
         //   return  _entities.ToListAsync();

            // se cambia a AsEnumerable para que no se ejecute inmediatamente y se pueda aplicar filtros
            //Con esto no se ejecuta automaticamente el método y lo carga en memoria 
            return  _entities.AsEnumerable();
        }
        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
           await _entities.AddAsync(entity);

        }
        public void Update(T entity)
        {
            _entities.Update(entity);


            //var currentPost = await GetPost(post.id);
            //currentPost.Date = post.Date;
            //currentPost.Description = post.Description;
            //currentPost.Image = post.Image;

            //int rows = await _context.SaveChangesAsync();
            //return rows > 0;

        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);


            //var currentPost = await GetPost(id);
            //_context.Posts.Remove(currentPost);

            //int rows = await _context.SaveChangesAsync();
            //return rows > 0;
        }

   
       
    }
}
