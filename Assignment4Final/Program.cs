using Assignment4Final.Data.Seed;
using Assignment4Final.Helpers;

namespace Assignment4Final
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = WebApplication.CreateBuilder(args);

            // NOTE: Add services to the container.
            AppServices.AddDatabaseServices(builder);
            // AppServices.AddDatabaseServicesSqlite(builder);
            AppServices.AddAuthentication(builder, Configuration);
            AppServices.AddAuthorization(builder);
            AppServices.AddControllers(builder);
            AppServices.AddSwagger(builder);
            AppServices.AddRepositoriesAndServices(builder);
            AppServices.AddAutoMapper(builder);
            AppServices.AddCors(builder);

            var app = builder.Build();

            AppServices.ConfigureApp(app);

            // AGkiz - Seeds dummy data to DB
            DbSeed.Seed(app).Wait();

            app.Run();
        }
    }
}
