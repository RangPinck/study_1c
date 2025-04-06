using Microsoft.EntityFrameworkCore;
using Study1CApi.Models;

namespace Study1CApi
{
    public class DbStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return async app =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    Study1cDbContext context = scope.ServiceProvider.GetRequiredService<Study1cDbContext>();
                    context.Database.Migrate();
                }
                next(app);
            };
        }
    }
}
