using Frontend.APIs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EmployeeAPI>();
builder.Services.AddScoped<DepartmentAPI>();
builder.Services.AddScoped<ManagerAPI>();

builder.Services.AddHttpClient("BackEnd", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUri:BackEnd"]);
});

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=GetAllEmployees}/{id?}")
    .WithStaticAssets();


app.Run();
