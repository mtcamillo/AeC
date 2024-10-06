using System.ComponentModel.DataAnnotations;

namespace AeC.Models
{
    // Definição da classe User que será mapeada para a tabela de usuários no banco de dados.
    public class User
    {
        // Chave primária (Primary Key) da tabela. A anotação [Key] define que o campo 'Id' será a chave primária.
        [Key]
        public int Id { get; set; }

        // Propriedade 'Nome' que é obrigatória. Representa o nome completo do usuário.
        [Required] // O campo deve ser preenchido.
        public string Nome { get; set; }

        // Propriedade 'Username' que é obrigatória. Representa o nome de usuário usado para login.
        [Required] 
        public string Username { get; set; }

        // Propriedade 'Password' que é obrigatória. Armazena a senha do usuário.
        // A anotação [DataType(DataType.Password)] indica que, na interface do usuário, esse campo deve ser tratado como uma senha (por exemplo, usando campos de texto protegidos).
        [Required]
        [DataType(DataType.Password)] // Especifica que o campo é do tipo senha.
        public string Password { get; set; }
    }
}
