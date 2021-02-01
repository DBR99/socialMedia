using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SocialMediaInfraestructure.Filters
{
    public class ValidationsFilter : IAsyncActionFilter
    {
        //OnActionExecutionAsync es el metodo que se va ejecutar cuando se invoque el request hacia nuestro 
        //controlador
        //el ActionExecutionDelegate es para que continúe el flujo del pipeline en caso de que no ocurra nada inesperado


        //Para los filtros existen tres ambitos: Global de controlador y a nivel de acción
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            await next();
        }
    }
}
