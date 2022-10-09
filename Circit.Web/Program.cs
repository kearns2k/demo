using Circit.Github.HttpClient;
using Circit.Github.HttpClient.Interfaces;
using Circit.Web.Models.AppSettings;
using Circit.Web.Services;
using Circit.Web.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//DI
builder.Services.AddSingleton<IOAuth, OAuth>();
builder.Services.AddSingleton<IGithubApi, GithubApi>();
builder.Services.AddTransient<IRandomTokenService, RandomTokenService>();

builder.Services.AddOptions();
builder.Services.Configure<GitHubOAuthSettings>(builder.Configuration.GetSection("GitHubOAuth"));

//Add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});


//Set Security Headers
builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
});

var policy = new HeaderPolicyCollection()
        .AddFrameOptionsDeny()
        .AddXssProtectionBlock()
        .AddContentTypeOptionsNoSniff()
        .AddReferrerPolicyStrictOriginWhenCrossOrigin()
        .RemoveServerHeader()
        .AddCrossOriginOpenerPolicy(builder =>
        {
            builder.SameOrigin();
        })
        .AddCrossOriginEmbedderPolicy(builder =>
        {
            builder.RequireCorp();
        })
        .AddCrossOriginResourcePolicy(builder =>
        {
            builder.SameOrigin();
        })
        .AddContentSecurityPolicy(builder =>
        {
            builder.AddObjectSrc().None();
            builder.AddBlockAllMixedContent();
            builder.AddImgSrc().Self().From("data:");
            builder.AddFormAction().Self();
            builder.AddFontSrc().Self();
            builder.AddStyleSrc().Self(); 
            builder.AddBaseUri().Self();
            builder.AddScriptSrc().UnsafeInline().WithNonce();
            builder.AddFrameAncestors().None();
        })
        .RemoveServerHeader()
        .AddPermissionsPolicy(builder =>
        {
            builder.AddAccelerometer().None();
            builder.AddAutoplay().None();
            builder.AddCamera().None();
            builder.AddEncryptedMedia().None();
            builder.AddFullscreen().All();
            builder.AddGeolocation().None();
            builder.AddGyroscope().None();
            builder.AddMagnetometer().None();
            builder.AddMicrophone().None();
            builder.AddMidi().None();
            builder.AddPayment().None();
            builder.AddPictureInPicture().None();
            builder.AddSyncXHR().None();
            builder.AddUsb().None();
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
