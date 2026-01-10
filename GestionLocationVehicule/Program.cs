using GestionLocationVehicule.Areas.Admin.Data;
using GestionLocationVehicule.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

//var cc = builder.Configuration.GetConnectionString("con");
//builder.Services.AddDbContext<GestionLocationContext>(
//  options => options.UseMySql(cc, ServerVersion.AutoDetect(cc))
//);

var cc = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(cc, ServerVersion.AutoDetect(cc))

);
builder.Services.AddDbContext<GestionLocationContext>(
    options => options.UseMySql(cc, ServerVersion.AutoDetect(cc))
);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Auth/Login"; // Route vers votre page de login
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=User}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Locations}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Vehicules}/{action=Index}"
);




app.Run();

