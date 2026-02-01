using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Zellaya.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");

    options
        .UseMySql(
            conn,
            ServerVersion.AutoDetect(conn),                
            mySql => mySql
                .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                .CommandTimeout(30))                     
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()); 

    // Пишем SQL и ошибки в консоль/лог
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddSession();

builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorage"));


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "orders",
    pattern: "{controller=Orders}/{action=Index}/{id?}");


app.Run();
