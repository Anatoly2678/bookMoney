using BookMoney.Data;
using BookMoney.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NLog.Web;
using System;

var builder = WebApplication.CreateBuilder(args);

// ���������� User Secrets � ������������
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// ��������� �����������
builder.Logging.ClearProviders();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Host.UseNLog();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
////builder.Logging.AddEventLog();
//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
//builder.Services.AddLogging();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ��������� connection string �� secrets.json
var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string not found in secrets.json");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ClientStateService>();

var app = builder.Build();

// �������� ���� ������ ��� �������
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    await dbContext.Database.MigrateAsync();
//}
app.UseDeveloperExceptionPage();

IWebHostEnvironment env = app.Environment;

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
    RequestPath = "/node_modules",
    EnableDirectoryBrowsing = false
});

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


//public static IHostBuilder CreateHostBuilder(string[] args) =>
//    Host.CreateDefaultBuilder(args)
//        .ConfigureWebHostDefaults(webBuilder =>
//        {
//            webBuilder.CaptureStartupErrors(true);
//            webBuilder.UseSetting("detailedErrors", "true");
//            webBuilder.UseStartup<Startup>();
//        });