using EmlakPortal.Data;
using EmlakPortal.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVÝSLERÝN EKLENMESÝ ---

// Repository Baðlantýsý
builder.Services.AddScoped(typeof(IRepository<>), typeof(EmlakPortal.Repositories.Repository<>));

// *** YENÝ EKLENEN: Session (Oturum) Servisi ***
// Basit admin giriþi için bunu eklememiz þart.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // 20 dk iþlem yapýlmazsa oturum kapansýn
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Mevcut Cookie Ayarlarýn (Bunu bozmadým, ilerde geliþmiþ auth için durabilir)
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
    });

builder.Services.AddControllersWithViews();

// Veritabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- 2. UYGULAMA AYARLARI (MIDDLEWARE) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// *** YENÝ EKLENEN: Session Kullanýmý ***
// Burasý çok önemli, Routing'den sonra, Auth'dan önce gelmeli.
app.UseSession();

app.UseAuthentication(); // 1. Kimlik doðrulama
app.UseAuthorization();  // 2. Yetki kontrolü

// --- ROTALAR ---

// Admin Area Rotasý
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Standart Rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();