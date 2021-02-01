﻿using Microsoft.Extensions.Options;
using SocialMediaCore.CustomEntities;
using SocialMediaCore.Entities;
using SocialMediaCore.Exceptions;
using SocialMediaCore.Interfaces;
using SocialMediaCore.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaCore.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly PaginationOptions _paginationOptions;
        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options) {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {


            filters.pageNumber = filters.pageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.pageNumber;
            filters.pageSize = filters.pageSize == 0 ? _paginationOptions.DefaultPagesSize : filters.pageSize;


            var posts = _unitOfWork.PostRepository.GetAll();

            if (filters.UserId != null) {
                posts = posts.Where(x => x.UserId == filters.UserId);    
            }
            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<Post>.Create(posts, filters.pageNumber, filters.pageSize);

            return pagedPosts;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessException("User doesn´t exist");
            }

            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPost.Count() < 10) {
                var lastPost = userPost.OrderByDescending(x=>x.Date).FirstOrDefault();

                if ((DateTime.Now - lastPost.Date).TotalDays < 7) {
                    throw new BusinessException("You are not able to publish the post");
                }
            }

            if (post.Description.Contains("sexo"))
            {
                throw new BusinessException("Content not allowed");
            }
         
            await _unitOfWork.PostRepository.Add(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            _unitOfWork.PostRepository.Update(post);
            _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }
    }
}
