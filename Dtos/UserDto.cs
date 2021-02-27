using System.ComponentModel.DataAnnotations;

namespace Estrutura.Web.API.Dtos
{
    public class UserDto
    {
        /// <summary>
        /// Id usuario
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome usuario
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// SobreNome usuario
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Login
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// senha
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// perfil usuario
        /// </summary>
        [Required]
        public string Role { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}