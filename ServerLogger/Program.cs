
// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
// app.UseStaticFiles();
//
// app.UseRouting();
//
// app.UseAuthorization();
//
// app.MapRazorPages();
//
// app.Run();

using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace ServerLogger;

public static class Program
{
    static async Task Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls("http://127.0.0.1:5001")  // Укажите желаемый адрес и порт сервера
            .ConfigureServices(ConfigureServices)
            .Configure(Configure)
            .Build();

        await host.RunAsync();
    }
    
    static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddRazorPages();
        services.AddSingleton<DataService>();
    }

    static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();

        });
        
            app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    }
}