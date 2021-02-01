using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialMedia.Api.Response;
using SocialMediaCore.CustomEntities;
using SocialMediaCore.DTOs;
using SocialMediaCore.Entities;
using SocialMediaCore.Interfaces;
using SocialMediaCore.QueryFilters;
using SocialMediaInfraestructure.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //PRINCIPIO DE INVERSION DE DEPENDENCIAS
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriServices;
        //IMPORTANTE !!!!!!!!!!!!!!!!!!!!!!!!!!
        //SIEMPRE QUE SE INYECTE UNA DEPENDENCIA TENEMOS QUE DEFINIR COMO RESOLVER ESA DEPENDENCIA
        //se hace en el STARTUP.CS



        public PostController(IPostService postService, IMapper mapper, IUriService uriServices)
        {
            _postService = postService;
            _mapper = mapper;
            _uriServices = uriServices;
        }

        //Existen tres formas de retornar información desde una api
        //IActionResult // con este se retornan metodos que implementas Action Result, permite usar
        //ok, notFound, no content,unauthorize ... Como esto es genérico se presentan problemas al realizar la documen
        //tación con SWAGGER entonces hay que usar un decorador para identificar lo que devuelve el método.

        //public ApiResponse<IEnumerable<PostDto>> -----Osea con un tipado esctricto, pero esto 
        //solo se aconseja cuando no se debe realizar ningun tipo de validación ya que así siempre retorna un 200
        //asi así el servicio haya fallado, se controla por medio de un succes = true o false

        /// <summary>
        /// Retrive all posts
        /// </summary>
        /// <param name="filters">filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
        public IActionResult GetPosts([FromQuery]PostQueryFilter filters) {
            var posts =  _postService.GetPosts(filters);
            var postsDto = _mapper.Map<IEnumerable<PostDTO>>(posts);


            var metadata = new MetaData
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviusPage = posts.HasPreviusPage,
                NextPageUrl = _uriServices.GetPostPaginatonUri(filters, Url.RouteUrl(nameof(GetPost))).ToString()
                
            };

            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDto)
            {
                Meta = metadata
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDTO>(post);
            var response = new ApiResponse<PostDTO>(postDto);

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            await _postService.InsertPost(post);

            postDto = _mapper.Map<PostDTO>(post);
            var response = new ApiResponse<PostDTO>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDTO postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.id = id;

            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
