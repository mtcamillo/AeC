using System.ComponentModel.DataAnnotations;

namespace AeC.Models
{
    // Definição da classe Endereco que será mapeada para uma tabela no banco de dados.
    public class Endereco
    {
        // Propriedade Id que será a chave primária (Primary Key) da tabela.
        public int Id { get; set; }

        // Propriedade Cep que é obrigatória e tem um tamanho máximo de 9 caracteres.
        [Required (ErrorMessage = "O campo CEP é obrigatório.")] // Define que o campo é obrigatório.
        [StringLength(9)] // Define o tamanho máximo da string (ex.: '89580-000').
        public string Cep { get; set; }

        // Propriedade Logradouro que é obrigatória.
        [Required (ErrorMessage = "O campo Logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        // Propriedade Complemento que é opcional (pode ser nulo). O "?" indica que o campo é anulável (nullable).
        public string? Complemento { get; set; }

        // Propriedade Bairro que é obrigatória.
        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        public string Bairro { get; set; }

        // Propriedade Cidade que é obrigatória.
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        public string Cidade { get; set; }

        // Propriedade Uf que é obrigatória e tem um tamanho máximo de 2 caracteres.
        [Required(ErrorMessage = "O campo UF é obrigatório.")]
        [StringLength(2)] // Define o tamanho máximo para UF.
        public string Uf { get; set; }

        // Propriedade Numero que é obrigatória e representa o número do endereço.
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        public int Numero { get; set; }

        // Propriedade UserId que armazena o ID do usuário associado a este endereço.
        // Essa propriedade não tem validação específica, mas será usada para mapear o relacionamento com o usuário.
        public string UserId { get; set; }
    }
}
