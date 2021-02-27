using System.ComponentModel.DataAnnotations;

namespace Estrutura.Web.API.Models
{
    /// <summary>
    /// Autenticação do usuario
    /// </summary>
    public class AuthenticateModel
    {
        /// <summary>
        /// Login de acesso
        /// </summary>
        /// <value></value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        /// <value></value>
        [Required]
        public string Password { get; set; }
    }
}