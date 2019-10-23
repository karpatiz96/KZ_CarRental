using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewRender
{
    public interface IRazorViewToStringRender
    {
        Task<string> RenderViewToStringAsync<TModel>(string name, TModel model);
    }
}
