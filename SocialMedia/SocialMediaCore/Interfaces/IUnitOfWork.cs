﻿using SocialMediaCore.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMediaCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get;}
        IRepository<User> UserRepository { get;}
        IRepository<Comment> CommentRepository { get;}
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
