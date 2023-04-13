using DemoSignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/Account/Login";
                     options.AccessDeniedPath = "/Account/AccessDenied";
                 });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    endpoints.MapHub<signalrServer>("/signalrServer");
});


app.Run();
