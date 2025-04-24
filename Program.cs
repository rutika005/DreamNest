using Aesthetica.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<UserService>();   
builder.Services.AddScoped<EmailService>();  

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(); 

builder.Services.AddControllersWithViews(); 


builder.Services.Configure<RazorpayOptions>(builder.Configuration.GetSection("Razorpay"));


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21)))); 


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseSession(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
