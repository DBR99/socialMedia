using SocialMediaCore.QueryFilters;
using System;

namespace SocialMediaInfraestructure.Interfaces
{
    public interface IUriService
    {
         Uri GetPostPaginatonUri(PostQueryFilter filter, string actionUrl);

    }
}