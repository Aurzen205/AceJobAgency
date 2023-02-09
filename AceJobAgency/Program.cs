using AceJobAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApp_Core_Identity.Model;
using static WebApp_Core_Identity.Model.AuthDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<Membership,IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
//Session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 3;
});

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

//Encryption
builder.Services.AddDataProtection();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
