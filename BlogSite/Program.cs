using BlogSiteModels.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

builder.Services.AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddDbContext<BlogSiteDbContext>(
    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection")
    );



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/admin/admin/home";
            
        }
    ) ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area}/{controller}/{action}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blogs}/{action=BlogsList}/{id?}");

app.Run();
