using FrontMDD.Service;
using FrontMDD.WebApplication.Pages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddScoped<ProfilServices>();
builder.Services.AddScoped<AbrisServices>();
builder.Services.AddTransient<IndexModel>();


var app = builder.Build();

// Configurer le pipeline des requêtes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // La valeur HSTS par défaut est de 30 jours. Vous pouvez la modifier pour les scénarios de production, voir https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Dans la méthode Configure de Startup.cs
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

});

app.Run();
