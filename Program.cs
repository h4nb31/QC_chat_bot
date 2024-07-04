using System.Net;
using QualityControl.Models;
using Microsoft.EntityFrameworkCore;
using QualityControl.Properties;
using QualityControl;
using Telegram.Bot;
using Newtonsoft.Json;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException(nameof(connection) + "is Null");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
string IpAddress = "192.168.2.239";

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Host.UseWindowsService();
builder.Services.AddWindowsService();
builder.Services.AddHostedService<BotService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<BotHandlers>();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Parse(IpAddress), 8091); //Http
    options.Listen(IPAddress.Parse(IpAddress), 8090, ListenOptions =>
    {
        ListenOptions.UseHttps("D:/MY_FOLDER/Work/4_CODE/C#/Projects/QC_ServiceBot/QualityControl/LocalCert.pfx", "d3f4u7t"); //Https   (можно в ковычках указать пусть до своего серта с паролем)
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Маршрутизация для вебхуков
//app.MapPost("/webhook", async (HttpContext context, BotHandlers botHandlers) => 
//{
//    var ipAddres = context.Connection.RemoteIpAddress?.ToString();
//    using var reader = new StreamReader(context.Request.Body);
//    var body = await reader.ReadToEndAsync();
//    var update = JsonConvert.DeserializeObject<Update>(body);

//    if(update != null)
//    {
//        await botHandlers.HandlerUpdateAsync(new TelegramBotClient("6530522678:AAE7JIIBwbSo6cPchfp5E6CVvZSggJ9_c9M"), update, context.RequestAborted);
//    }

//    //await botHandlers.HandlerUpdateAsync(new TelegramBotClient("6530522678:AAE7JIIBwbSo6cPchfp5E6CVvZSggJ9_c9M"), update, context.RequestAborted);
//    context.Response.StatusCode = 200;
//    await context.Response.WriteAsync($"IP Address: {ipAddres}");
//});


app.Run();
