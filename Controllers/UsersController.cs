using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Estrutura.Web.API.Services;
using Estrutura.Web.API.Entities;
using Estrutura.Web.API.Models;
using AutoMapper;
using System.Threading.Tasks;
using Estrutura.Web.API.Dtos;
using Estrutura.Web.API.Data.RepoUser;
using Estrutura.Web.API.Helpers;
using System.Collections.Generic;

namespace Estrutura.Web.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly IMapper _mapper;

        public readonly IRepositoryUser _repo;

        public UsersController(IUserService userService, IMapper mapper, IRepositoryUser repo)
        {
            _userService = userService;
            _mapper = mapper;
             _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Nome de usuário ou senha está incorreta." });

            var userResult = _mapper.Map<UserDto>(user);

            return Ok(userResult);
        } 
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            var users = await _repo.GetAllUsersAsync(pageParams);

            var usersResult = _mapper.Map<IEnumerable<UserDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersResult);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // só permite que administradores acessem outros registros de usuário
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = _repo.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public IActionResult Post([FromBody] UserDto model)
        {
            User user = _mapper.Map<User>(model);

            _repo.Add(user);
            if (_repo.SaveChanges())
            {
                return Created($"/user/{model.Id}", _mapper.Map<UserDto>(user));
            }

            return BadRequest("Usuario não cadastrado");
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDto model)
        {
            var user = _repo.GetUserById(id);
            if (user == null) return BadRequest("Usuario não encontrado");

            _mapper.Map(model, user);

            _repo.Update(user);
            if (_repo.SaveChanges())
            {
                return Created($"/user/{model.Id}", _mapper.Map<UserDto>(user));
            }

            return BadRequest("Usuario não Atualizado");
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] UserDto model)
        {
            var user = _repo.GetUserById(id);
            if (user == null) return BadRequest("Usuario não encontrado");

            _mapper.Map(model, user);

            _repo.Update(user);
            if (_repo.SaveChanges())
            {
                return Created($"/user/{model.Id}", _mapper.Map<UserDto>(user));
            }

            return BadRequest("Usuario não Atualizado");
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _repo.GetUserById(id);
            if (user == null) return BadRequest("Usuario não encontrado");

            _repo.Delete(user);
            if (_repo.SaveChanges())
            {
                return Ok("Usuario deletado");
            }

            return BadRequest("Usuario não deletado");
        }

    }
}
