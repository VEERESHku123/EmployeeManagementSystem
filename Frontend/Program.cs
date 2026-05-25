using Frontend.ApiServices.Implements;
using Frontend.ApiServices.Interfaces;
using Frontend.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

//Microsoft login
builder.Services.AddSession();

builder.Services.AddDistributedMemoryCache();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })

    .AddMicrosoftIdentityWebApp(
        builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

// Api Services
builder.Services.AddScoped<IEmployeeApiService, EmployeeApiService>();
builder.Services.AddScoped<IDepartmentApiService, DepartmentApiService>();
builder.Services.AddScoped<IManagerApiService, ManagerApiService>();
builder.Services.AddScoped<IUserApiService, UserApiService>();

// HttpClient 
builder.Services.AddHttpClient("Backend", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl:Backend"]);
});

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl:User"]);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
