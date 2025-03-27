using StambenaZajednica.Data;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.Repositories;
using StambenaZajednica.Data.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using StambenaZajednica.Services;
using StambenaZajednica.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodavanje Entity Framework Core servisa i povezivanje sa bazom podataka
builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registracija Identity sistema za autentifikaciju
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<RepositoryDbContext>()
    .AddDefaultTokenProviders();

// Registracija Repository interfejsa
builder.Services.AddScoped<IStambenaZajednicaRepository, StambenaZajednicaRepository>();
builder.Services.AddScoped<IStanRepository, StanRepository>();
builder.Services.AddScoped<IFinansijeRepository, FinansijeRepository>();

//Registracija Pin servisa
builder.Services.AddScoped<PinGeneratorService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
