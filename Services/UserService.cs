using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Estrutura.Web.API.Entities;
using Estrutura.Web.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Estrutura.Web.API.Data;

namespace Estrutura.Web.API.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

    }

    public class UserService : IUserService
    {
        // usuários codificados para simplificar, armazenar em um banco de dados com senhas com hash em aplicativos de produção
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "samuel", Password = "samuel", Role = Role.Admin },
            new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "keli", Password = "keli", Role = Role.User }
        };

        private readonly AppSettings _appSettings;
        private readonly EstruturaContext _context;

        public UserService(IOptions<AppSettings> appSettings, EstruturaContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            var user = GetUserByUser(username, password);

            // retorna nulo se o usuário não for encontrado
            if (user == null)
                return null;

            // autenticação bem-sucedida, então gere token jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        private User GetUserByUser(string username, string password)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(u => u.Id)
                         .Where(user => user.Username == username)
                         .Where(user =>  user.Password == password);

            return query.FirstOrDefault();
        }
    }
}