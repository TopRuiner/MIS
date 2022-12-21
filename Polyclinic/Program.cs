using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PolyclinicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<PolyclinicUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PolyclinicContext>().AddDefaultTokenProviders().AddDefaultUI();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireDoctor", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("role", "Doctor")
        .Build());
    options.AddPolicy("RequireCanRegisterAsPatient", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("role", "CanRegisterAsPatient")
        .Build());
});

//������ ��� ���������
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});


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
app.UseSession();
app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireDoctor", policy => policy.RequireRole("Doctor"));
});
*/
app.Run();

