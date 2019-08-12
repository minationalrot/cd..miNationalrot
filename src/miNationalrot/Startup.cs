

using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;



namespace miNationalrot
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
