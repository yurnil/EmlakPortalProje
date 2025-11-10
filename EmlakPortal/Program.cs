using EmlakPortal.Data;
using EmlakPortal.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Projeye diyoruz ki: Ne zaman birisi IRepository<T> isterse, ona Repository<T> ver.
builder.Services.AddScoped(typeof(IRepository<>), typeof(EmlakPortal.Repositories.Repository<>));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
