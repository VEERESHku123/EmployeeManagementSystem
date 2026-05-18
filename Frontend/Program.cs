using Frontend.APIs;
using Frontend.Mappers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
//Microsoft login
builder.Services.AddSession();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddScoped<EmployeeAPI>();
builder.Services.AddScoped<DepartmentAPI>();
builder.Services.AddScoped<ManagerAPI>();
builder.Services.AddScoped<AccountApi>();

builder.Services.AddHttpClient("BackEnd", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl:BackEnd"]);
});

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl:Auth"]);
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
