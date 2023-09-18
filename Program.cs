using Blazored.LocalStorage;
using OSIET_Finance.Controller.Autorisation;
using OSIET_Finance.Data.AuthetificationState;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OSIET_Finance.Controller.OptionAvancees;
using OSIET_Finance.Models.Finance;
using OSIET_Finance.Controller.Finance;
using Microsoft.Extensions.FileProviders;
using OSIET_Finance.Models.Administration;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILoginControl, LoginControl>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAutorisationService, AutorisationService>();
builder.Services.AddScoped<IOptionAvancees, OptionAvancees>();
builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddDbContext<UTILISATEURContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("osiet_connection"));
});
builder.Services.AddDbContext<FINANCEContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("osiet_connection"));
});
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
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Pieces")),
    RequestPath = new PathString("/Pieces")
});
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Template")),
    RequestPath = new PathString("/Template")
});
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();