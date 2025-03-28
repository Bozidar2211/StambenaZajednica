using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data;
using StambenaZajednica.Data.Repositories;
using StambenaZajednica.Data.RepositoryInterfaces;
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

// Seed the database with default data (roles and Upravnik account)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Initialize(services, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
