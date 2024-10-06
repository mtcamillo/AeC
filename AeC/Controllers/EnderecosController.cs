using AeC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

[Authorize] // Garante que apenas usuários autenticados podem acessar os métodos deste controlador.
public class EnderecosController : Controller
{
    private readonly ApplicationDbContext _context;

    // Construtor que injeta o contexto do banco de dados
    public EnderecosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Exibe a lista de endereços do usuário logado.
    public async Task<IActionResult> Index()
    {
        // Obtém o ID do usuário logado.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Busca os endereços no banco de dados que pertencem ao usuário logado.
        var enderecos = await _context.Enderecos.Where(e => e.UserId == userId).ToListAsync();

        // Obtém o nome do usuário logado.
        var userName = User.Identity.Name;

        // Recupera o nome completo do usuário no banco de dados e armazena na ViewBag.
        var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        ViewBag.UserName = usuario?.Nome;

        // Retorna a view com os endereços encontrados.
        return View(enderecos);
    }

    // Exibe a página de criação de um novo endereço.
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Método para criar um novo endereço associado ao usuário logado.
    [HttpPost]
    [ValidateAntiForgeryToken] // Protege contra ataques de falsificação de solicitação entre sites (CSRF).
    public async Task<IActionResult> Create(Endereco endereco)
    {
        // Verifica se o usuário está autenticado.
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        // Atribui o ID do usuário logado ao novo endereço.
        endereco.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Remove o erro de validação para o campo UserId, pois ele é atribuído no backend.
        ModelState.Remove("UserId");

        // Verifica se o modelo é válido.
        if (!ModelState.IsValid)
        {
            // Exibe os erros de validação no console para depuração.
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(endereco); // Retorna a view se houver erros.
        }

        // Adiciona o novo endereço ao banco de dados e salva as mudanças.
        _context.Add(endereco);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Exibe a página de edição para um endereço específico.
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Busca o endereço no banco de dados pelo ID.
        var endereco = await _context.Enderecos.FindAsync(id);

        // Verifica se o endereço existe e pertence ao usuário logado.
        if (endereco == null || endereco.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return NotFound();
        }

        return View(endereco);
    }

    // Método para atualizar um endereço existente.
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Endereco endereco)
    {
        // Verifica se o usuário está autenticado.
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        // Verifica se o ID fornecido corresponde ao endereço sendo editado.
        if (id != endereco.Id)
        {
            return NotFound();
        }

        // Atribui o ID do usuário logado ao endereço.
        endereco.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Remove o erro de validação para o campo UserId.
        ModelState.Remove("UserId");

        // Verifica se o modelo é válido.
        if (!ModelState.IsValid)
        {
            // Exibe os erros no console para depuração.
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(endereco); // Retorna a view se houver erros.
        }

        // Tenta atualizar o endereço no banco de dados.
        try
        {
            _context.Update(endereco);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Verifica se o endereço ainda existe no banco de dados.
            if (!EnderecoExists(endereco.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // Exibe a página de confirmação para deletar um endereço.
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Busca o endereço no banco de dados.
        var endereco = await _context.Enderecos.FirstOrDefaultAsync(m => m.Id == id);

        // Verifica se o endereço existe e pertence ao usuário logado.
        if (endereco == null || endereco.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return NotFound();
        }

        return View(endereco);
    }

    // Método para deletar um endereço.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Busca o endereço no banco de dados.
        var endereco = await _context.Enderecos.FindAsync(id);

        if (endereco == null)
        {
            return NotFound();
        }

        // Remove o endereço do banco de dados e salva as mudanças.
        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Método auxiliar que verifica se um endereço existe no banco de dados.
    private bool EnderecoExists(int id)
    {
        return _context.Enderecos.Any(e => e.Id == id);
    }

    // Método que busca informações de um endereço pela API ViaCEP, usando o CEP.
    [HttpGet("Endereco/BuscarEnderecoPorCep")]
    public async Task<IActionResult> BuscarEnderecoPorCep(string cep)
    {
        if (string.IsNullOrEmpty(cep))
        {
            return BadRequest("O CEP é obrigatório.");
        }

        using (var httpClient = new HttpClient())
        {
            try
            {
                // Faz uma requisição para a API ViaCEP.
                var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                if (response.IsSuccessStatusCode)
                {
                    // Retorna o JSON do endereço caso a requisição tenha sucesso.
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return Content(jsonResponse, "application/json");
                }
                else
                {
                    return NotFound("Endereço não encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o CEP: {ex.Message}");
            }
        }
    }

    // Método que exporta a lista de endereços do usuário logado em formato CSV.
    public async Task<IActionResult> ExportarCSV()
    {
        // Obtém o ID do usuário logado.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Busca os endereços no banco de dados.
        var enderecos = await _context.Enderecos.Where(e => e.UserId == userId).ToListAsync();

        // Verifica se o usuário tem endereços para exportar.
        if (enderecos == null || !enderecos.Any())
        {
            TempData["MensagemErro"] = "Nenhum endereço encontrado para exportação.";
            return RedirectToAction("Index");
        }

        // Cria o conteúdo do CSV.
        var builder = new StringBuilder();
        builder.AppendLine("CEP,Logradouro,Complemento,Bairro,Cidade,UF,Numero");

        foreach (var endereco in enderecos)
        {
            builder.AppendLine($"{endereco.Cep},{endereco.Logradouro},{endereco.Complemento},{endereco.Bairro},{endereco.Cidade},{endereco.Uf},{endereco.Numero}");
        }

        // Converte o conteúdo para bytes e retorna o arquivo CSV para download.
        var bytes = Encoding.UTF8.GetBytes(builder.ToString());
        return File(bytes, "text/csv", "enderecos.csv");
    }
}
