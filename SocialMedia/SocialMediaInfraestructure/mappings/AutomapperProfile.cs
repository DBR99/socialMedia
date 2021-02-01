using AutoMapper;
using SocialMediaCore.DTOs;
using SocialMediaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaInfraestructure.mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() {
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();

        }

    }
}
