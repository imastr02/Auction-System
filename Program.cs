using Auction_System.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Auction_System.Models;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>( options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
} );


builder.Services.AddIdentity<AppUser, IdentityRole>( options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RatingService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<WatchlistService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddHostedService<AuctionStatusService>();
// Add EmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Configure SMTP settings
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddRazorPages();

var app = builder.Build();

//Seed Roles into the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
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
