using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AeC.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication; 

namespace AeC.Controllers
{
    // Controlador responsável pelas ações de cadastro, login e logout de usuários
    public class AccountController : Controller
    {
        // Injeção de dependência do contexto de banco de dados (ApplicationDbContext)
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método GET para exibir a página de cadastro de usuário
        public IActionResult Register()
        {
            return View();
        }

        // Método POST para registrar um novo usuário
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Verifica se os dados do usuário são válidos conforme as regras de validação do modelo
            if (ModelState.IsValid)
            {
                // Gera o hash da senha para não armazenar a senha em texto puro no banco
                user.Password = HashPassword(user.Password);

                // Adiciona o novo usuário no banco de dados
                _context.Users.Add(user);
                _context.SaveChanges();

                // Redireciona para a página de login após o registro
                return RedirectToAction("Login");
            }

            // Se houver erro na validação dos dados, retorna a mesma View com os dados para correção
            return View(user);
        }

        // Método GET para exibir a página de login
        public IActionResult Login()
        {
            return View();
        }

        // Método POST para autenticar o usuário
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Verifica se os campos de usuário e senha não estão vazios
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                // Adiciona uma mensagem de erro se algum campo estiver vazio
                ModelState.AddModelError("", "Usuário e senha são obrigatórios.");
                return View();
            }

            // Gera o hash da senha informada para comparar com a senha armazenada no banco
            var hashedPassword = HashPassword(password);

            // Busca o usuário no banco de dados pelo nome de usuário
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Verifica se o usuário foi encontrado e se a senha corresponde
            if (user == null || user.Password != hashedPassword)
            {
                // Adiciona uma mensagem de erro se o usuário não for encontrado ou a senha estiver incorreta
                ModelState.AddModelError("", "Usuário ou senha inválidos.");
                return View();
            }

            // Se a autenticação for bem-sucedida, cria uma lista de Claims (informações sobre o usuário)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Adiciona o ID do usuário
                new Claim(ClaimTypes.Name, user.Username) // Adiciona o nome de usuário
            };

            // Cria a identidade do usuário com base nas Claims e o tipo de autenticação (Cookie)
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Define as propriedades da autenticação, como persistência e tempo de expiração
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // A sessão de login será mantida mesmo após o fechamento do navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // A sessão expira após 30 minutos
            };

            // Realiza o login do usuário (cria um cookie de autenticação)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redireciona o usuário para a página de endereços (controlador Enderecos, ação Index)
            return RedirectToAction("Index", "Enderecos");
        }

        // Método para realizar o logout do usuário
        public async Task<IActionResult> Logout()
        {
            // Remove o cookie de autenticação, efetivamente deslogando o usuário
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona para a página de login
            return RedirectToAction("Login", "Account");
        }

        // Método para gerar o hash de uma senha usando o algoritmo SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Converte a senha para um array de bytes e calcula o hash
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Converte o hash para uma string base64 e retorna
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
