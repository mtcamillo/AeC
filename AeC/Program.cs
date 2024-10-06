using AeC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session; // Certifique-se de adicionar este namespace
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adicione os serviços necessários, incluindo a sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicionando serviços de autenticação baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redireciona para a página de login se não autenticado
        options.LogoutPath = "/Account/Logout"; // Redireciona para logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Página de acesso negado
    });

// Adicione o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Usar autenticação e sessões no pipeline de requisições
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
