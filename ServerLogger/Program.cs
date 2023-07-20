namespace ServerLogger;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddRazorPages();
        services.AddSingleton<DataService>();
    }

    static void Configure(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next();
        });

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }
}