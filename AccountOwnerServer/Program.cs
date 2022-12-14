using AccountOwnerServer.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "//nlog.config"));
builder.Services.AddAutoMapper(typeof(Program));
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.AddControllers();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();

var app = builder.Build();

// Configure the HTTP request pipeline.

if(app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});

app.UseCors("CorsPolicy");  // Call above UseAuthorization

app.UseAuthorization();

app.MapControllers();

app.Run();
