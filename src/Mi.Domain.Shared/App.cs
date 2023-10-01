using Microsoft.Extensions.Configuration;

namespace Mi.Domain.Shared
{
    public static class App
    {
        public static bool IsDevelopment { get; private set; }

        public static string WebRoot { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public static void Running(bool isDev, string webRoot, IConfiguration configuration)
        {
            IsDevelopment = isDev;
            WebRoot = webRoot;
            Configuration = configuration;
        }
    }
}