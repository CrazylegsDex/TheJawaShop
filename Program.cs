// Bring in dependancy namespaces
using Microsoft.EntityFrameworkCore;
using TheJawaShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register the Database Context
builder.Services.AddDbContext<DatabaseContext>
(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("TheContext"))
);

var app = builder.Build();

// Call SeedData.Initialize to seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
