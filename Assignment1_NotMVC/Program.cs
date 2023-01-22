using AngleSharp;
using Assignment1_NotMVC.Models;
using Assignment1_NotMVC.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//audit services
builder.Services.AddScoped<AuditLogServices>();

//Google ReCaptcha v3
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

//Adds Authentication
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
}).AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddSingleton<DataProtectionPurposeStrings>();

//Token management
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(3));

//Session management 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});


builder.Services.AddAntiforgery();

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    Config.SlidingExpiration = true;
    Config.LoginPath = "/Login";
});

builder.Services.AddSingleton<EmailConfiguration>();

//attempting google authentication
//var configuration = builder.Configuration;
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//}).AddCookie()
//    .AddGoogle(GoogleDefaults.AuthenticationScheme,options =>
//{
//    options.ClientId = configuration["Authentication:Google:ClientId"];
//    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
//});

//var configuration = builder.Configuration;
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//  .AddGoogle(options =>
// {
//     options.ClientId = "709333917815-jv1pdc4nbhrig4o2sucagrtt1a80ftfm.apps.googleusercontent.com";
//     options.ClientSecret = "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";
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithRedirects("/errors/custom{0}");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
