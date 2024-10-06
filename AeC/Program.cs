using AeC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session; // Certifique-se de adicionar este namespace
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adicione os servi�os necess�rios, incluindo a sess�o
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicionando servi�os de autentica��o baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redireciona para a p�gina de login se n�o autenticado
        options.LogoutPath = "/Account/Logout"; // Redireciona para logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // P�gina de acesso negado
    });

// Adicione o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Usar autentica��o e sess�es no pipeline de requisi��es
app.UseAuthentication(); // Adicione esta linha
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
