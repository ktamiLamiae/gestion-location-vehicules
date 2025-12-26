using GestionLocationVehicule.Areas.Admin.Data;
using GestionLocationVehicule.Areas.Client.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var cc = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(
    cc,
        new MySqlServerVersion(new Version(8, 0, 36)))
);
builder.Services.AddDbContext<ApplicationClientDbContext>(
    options => options.UseMySql(
    cc,
        new MySqlServerVersion(new Version(8, 0, 36)))
);

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

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}"
);

//app.MapControllerRoute(
//    name: "ContactUs",
//    pattern: "{area=Client}/{controller=ContactUs}/{action=Index}"
//);

app.Run();

