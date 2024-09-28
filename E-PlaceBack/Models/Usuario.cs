using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eplace.Models
{
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario(string email, string senha)
        {
            this.Email = email;
            this.Senha = senha;
        }

        private Usuario() { }
    }
}

