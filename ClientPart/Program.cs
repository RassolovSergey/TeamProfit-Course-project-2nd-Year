using ClientPart.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// 1) Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// 2) ProtectedSessionStorage
builder.Services.AddScoped<ProtectedSessionStorage>();

// 3) HttpClientFactory + Authorized provider
builder.Services.AddHttpClient("WebApi", cfg => {
    cfg.BaseAddress = new Uri("http://localhost:5002/api/");
});
builder.Services.AddScoped<AuthorizedHttpClientProvider>();

var app = builder.Build();



app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
