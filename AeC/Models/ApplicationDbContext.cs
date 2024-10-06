using Microsoft.EntityFrameworkCore;

namespace AeC.Models
{
    // Definição do contexto de banco de dados da aplicação, que herda de DbContext.
    public class ApplicationDbContext : DbContext
    {
        // Construtor da classe que recebe as opções de configuração do contexto (como string de conexão) 
        // e passa essas opções para o construtor da classe base (DbContext).
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet representa uma coleção de entidades que será mapeada para uma tabela no banco de dados.
        // Define a tabela 'Users' com base na classe 'User'. 
        public DbSet<User> Users { get; set; }

        // Define a tabela 'Enderecos' com base na classe 'Endereco'.
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
