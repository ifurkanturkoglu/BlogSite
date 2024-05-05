using BlogSite.Services;
using BlogSiteModels.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var cookiePolicyOptions = new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict };

//Log.Logger = new LoggerConfiguration().WriteTo.MSSqlServer(
//    connectionString:""

//    );

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationService>();

//builder.Services.AddSession(a =>
//{
//    a.Cookie.Name = "UserSession";
//    a.IdleTimeout = TimeSpan.FromMinutes(5);
//});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/AuthError";
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/Blogs/BlogList";
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
//app.UseSession();

app.UseCookiePolicy(cookiePolicyOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/AuthError", async context =>
        await context.Response.WriteAsync("Yetkilendirilmemiþ kullanýcý eriþimi.")
    );

    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area}/{controller}/{action}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blogs}/{action=BlogsList}/{id:int?}");

    
});


app.Run();
