using BlogSiteModels.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cookiePolicyOptions = new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict };

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Admin";
    });

builder.Services.AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddDbContext<BlogSiteDbContext>(
    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection")
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

app.UseAuthorization();
app.UseAuthentication();

app.UseCookiePolicy(cookiePolicyOptions);

app.UseRouting();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area}/{controller}/{action}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blogs}/{action=BlogsList}/{id?}");

app.Run();
