using NP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NP.Models;
using System;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Data;
using System.Xml.Linq;
using NP.Recommender;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContextFactory<NPDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("NPDbConnectionString")));

builder.Services.AddDbContext<NPDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("NPDbConnectionString")));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NPDbContext>();

builder.Services.AddScoped<PearsonCorrelation>();
builder.Services.AddScoped<CosineSimilarity>();
builder.Services.AddScoped<DataPreprocessor>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();



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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Seed Db
AppDbInitializer.Seed(app);

app.Run();




