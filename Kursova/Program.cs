using DeliveryApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DeliveryApp.Data.AspnetIdentityRoleBasedTutorial.Data;
using DeliveryApp;
using DeliveryApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<OrdersDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersBDCnn")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<OrdersDBContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddSignInManager<SignInManager<ApplicationUser>>()
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<OrdersDBContext>()
       .AddDefaultUI()
       .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IAsortimentRepository, AsortimentRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserGoodsPerository, UserGoodsPerository>();
builder.Services.AddTransient<IDeliverReposity, DeliverRepository>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

app.Run();
