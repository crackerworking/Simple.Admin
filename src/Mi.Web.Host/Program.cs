namespace Mi.Web.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapRazorPages();
            app.MapControllerRoute("api-router", "/api/[Controller]/[Action]");
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}