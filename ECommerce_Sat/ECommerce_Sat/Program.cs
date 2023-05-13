using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Helpers;
using ECommerce_Sat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<DataBaseContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//Builder para llamar la clase SeederDb.cs|
builder.Services.AddTransient<SeederDb>();

//Builder para llamar la interfaz IUserHelper.cs
builder.Services.AddScoped<IUserHelper, UserHelper>();

//Builder para llamar la interfaz IDropDownListHelper.cs
builder.Services.AddScoped<IDropDownListHelper, DropDownListHelper>();

//Builder para llamar la interfaz IDropDownListHelper.cs
builder.Services.AddScoped<IAzureBlobHelper, AzureBlobHelper>();

var supportedCultures = new[]
{
    new CultureInfo("es-CO")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-CO");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddIdentity<User, IdentityRole>(io =>
{
	io.User.RequireUniqueEmail = true;
	io.Password.RequireDigit = false;
	io.Password.RequiredUniqueChars = 0;
	io.Password.RequireLowercase = false;
	io.Password.RequireNonAlphanumeric = false;
	io.Password.RequireUppercase = false;
	io.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<DataBaseContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Unauthorized";
	options.AccessDeniedPath = "/Account/Unauthorized";
});


var app = builder.Build();

app.UseRequestLocalization();

SeederData();
void SeederData()
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory.CreateScope())
    {
        SeederDb? service = scope.ServiceProvider.GetService<SeederDb>();
        service.SeedAsync().Wait();
    }
}

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

app.UseAuthentication(); //Autenticar mi usuario
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
