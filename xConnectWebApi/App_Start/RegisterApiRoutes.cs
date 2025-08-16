using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.Pipelines;

namespace xConnectWebApi
{
    public class RegisterApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            SetRoutes(config);
            SetSerializerSettings(config);

        }
        private void SetRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(name: "Contact",routeTemplate: "contactapi/{controller}/{action}");
        }

        private void SetSerializerSettings(HttpConfiguration config)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            config.Formatters.JsonFormatter.SerializerSettings = settings;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.EnsureInitialized();
        }

    }
}
