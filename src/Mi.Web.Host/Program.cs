namespace Mi.Web.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapRazorPages();

            app.Run();
        }
    }
}