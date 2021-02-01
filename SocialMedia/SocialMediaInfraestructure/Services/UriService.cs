using SocialMediaCore.QueryFilters;
using SocialMediaInfraestructure.Interfaces;
using System;

namespace SocialMediaInfraestructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri) {
            _baseUri = baseUri;
        }

        public Uri GetPostPaginatonUri(PostQueryFilter filter, string actionUrl) 
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl); 
        }
    }
}
