using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования
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




var app = builder.Build();
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
