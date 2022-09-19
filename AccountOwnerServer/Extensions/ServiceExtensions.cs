namespace AccountOwnerServer.Extensions
{
    public static class ServiceExtensions
    {
        /*
         An extension method is inherently the static method.
        They play a great role in .NET Core configuration because they increase the readability of our code for sure.
        What makes it different from the other static methods is that it accepts “this” as the first parameter,
        and “this” represents the data type of the object which uses that extension method.
        An extension method must be inside a static class.
        This kind of method extends the behavior of the types in .NET.*/
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
    }
}
