using System.Threading.Tasks;
using Estrutura.Web.API.Helpers;
using Estrutura.Web.API.Entities;

namespace Estrutura.Web.API.Data.RepoUser
{
    public interface IRepositoryUser : IRepository
    {
       Task<PageList<User>> GetAllUsersAsync(PageParams pageParams);        
        User[] GetAllUsers();
        User GetUserById(int userId);
    }
}