using EmlakPortal.Data;
using EmlakPortal.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Projeye diyoruz ki: Ne zaman birisi IRepository<T> isterse, ona Repository<T> ver.
builder.Services.AddScoped(typeof(IRepository<>), typeof(EmlakPortal.Repositories.Repository<>));

// --- COOKIE GÜVENLÝK AYARI ---
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Auth/Login"; // Giriþ yapmamýþ kiþiyi buraya at
        options.LogoutPath = "/Admin/Auth/Logout";
    });
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthentication(); // 1. Kimlik kontrolü (Kimsin?)
app.UseAuthorization(); // 2. Yetki kontrolü (Girebilir misin?)

// --- ADMIN ALANI ROTASI ---
// Admin paneline giden istekleri yakalar (örn: /Admin/Dashboard)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
