using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaCore.CustomEntities
{
    //ESTA CLASE ES PARA PAGINAR CUALQUIER TIPO DE LISTADO, POST, COMENTARIOS ...ETC
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; } 
        public int PageSize { get; set; } 
        public int TotalCount { get; set; }


        public bool HasPreviusPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;
        public int? PreviusPageNumber => HasPreviusPage ? CurrentPage - 1 : (int?)null;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            //Con esta linea se convierten los decimales al numero entero superior, y así evitar perder registros
            TotalPages = (int)Math.Ceiling( count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize) 
        {
            var count = source.Count();
           //Con está linea se toma el numero de la pagina actual y eso se mulriplica por el pageSize.
           //este numero de registros son los que se deben omitir al continuar a la siguiente consulta
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            return new PagedList<T>(items, count, pageNumber, pageSize);
        }



    }
}
