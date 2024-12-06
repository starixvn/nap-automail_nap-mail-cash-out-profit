using Microsoft.AspNetCore.Builder;
using Fanex.COProfit.Service;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.DependencyInjection;
using Fanex.COProfit.Service.Utilities;
using Fanex.MailDelivery.Client;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddConfiguration();
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
services.AddInternalServices();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await MailClientHandler.StartAsync();

await app.RunAsync();
