using Auction_System.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Auction_System.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container
builder.Services.AddOptions<MpesaSettings>()
	.Bind(builder.Configuration.GetSection("Mpesa"))
	.ValidateDataAnnotations()  // Now works with the package
	.ValidateOnStart();
// Explicit registration for direct access
builder.Services.AddSingleton(sp =>
	sp.GetRequiredService<IOptions<MpesaSettings>>().Value);

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

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});


builder.Services.AddHttpClient<IMpesaService, MpesaService>("Mpesa", client =>
{
	client.BaseAddress = new Uri("https://sandbox.safaricom.co.ke/");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
	ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});
builder.Services.AddScoped<IMpesaService>(provider =>
{
	var config = provider.GetRequiredService<IConfiguration>();
	var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

	return new MpesaService(
		config,
		httpClientFactory.CreateClient("Mpesa"), // Use named client
		provider.GetRequiredService<ApplicationDbContext>());
});


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

app.Use(async (context, next) =>
{
	if (context.Request.Path.StartsWithSegments("/Payment/Checkout"))
	{
		var itemId = context.Request.Query["itemId"].FirstOrDefault();
		if (int.TryParse(itemId, out int id))
		{
			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var item = await dbContext.Items.FindAsync(id);

			if (item?.IsPaid == true)
			{
				context.Response.Redirect($"/Items/Details?id={id}");
				return;
			}
		}
	}
	await next();
});

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
	//app.MapHub<ChatHub>("/chatHub");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
