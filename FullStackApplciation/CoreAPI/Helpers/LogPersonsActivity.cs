using DataLayerAbstraction;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreAPI.Helpers
{
    public class LogPersonsActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // getting instance of cosmos service
            var repo = resultContext.HttpContext.RequestServices.GetService<ICosmosManager>();

            // getting the user
            var user = await repo.GetPersonById(userId);

            // update last active date for user
            user.LastActive = DateTime.Now;

            await repo.UpdatePersonsData(userId, user);
        }
    }
}
