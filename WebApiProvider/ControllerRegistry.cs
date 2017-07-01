using System.Web.Http.Controllers;

using StructureMap.Configuration.DSL;


namespace WebApiProvider
{
    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            Scan(p =>
            {
                p.TheCallingAssembly();
                p.AddAllTypesOf<IHttpController>();
            });
        }
    }
}