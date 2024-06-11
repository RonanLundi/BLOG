using Blog.Data;
using Blog.Models;
using BlogAPI.Extension;
using BlogAPI.Services;
using BlogAPI.ViewModels;
using BlogAPI.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace BlogAPI.Controllers
{
    //[Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //[AllowAnonymous] 
        //permite acesso de não autorizados
        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post(
            [FromBody] RegisterViewModel model, 
            [FromServices] BlogDataContext context,
            [FromServices] EmailService emailService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
                Bio = "",
                Image = ""
            };

            var password = PasswordGenerator.Generate(25, true, true);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                

                if(emailService.Send(
                    user.Name, 
                    user.Email, 
                    "Bem vindo ao Blog!", 
                    $"Sua Senha é <strong>{password} </strong>"
                    ))
                {
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }

                return Ok(new ResultViewModel<dynamic>(new { 
                    user = user.Email, password
                }));
            }catch(DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05x99 - Este E-mail já está cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05x04 - Falha Interna no Servidor"));
            }
        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model,
            [FromServices] BlogDataContext context,
            [FromServices]TokenService tokenService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            try 
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            } 
            catch 
            {
                return StatusCode(500, new ResultViewModel<string>("05x04 - Falha interna no Servidor"));
            }
        }


        //pode ser colocado por método ou em todo o controller acima da classe
        [Authorize(Roles ="user")]
        [Authorize(Roles = "admin")]
        [HttpGet("v1/user")]
        public IActionResult GetUser() => Ok(User.Identity.Name);


        [Authorize(Roles = "author")]
        [Authorize(Roles = "admin")]
        [HttpGet("v1/author")]
        public IActionResult GetAuthor() => Ok(User.Identity.Name);

        [Authorize(Roles = "admin")]
        [HttpGet("v1/admin")]
        public IActionResult GetAdmin() => Ok(User.Identity.Name);

        [Authorize]//dessa forma ele só muda a imagem dele mesmo
        [HttpPost("v1/accounts/upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel model,
            [FromServices] BlogDataContext context)
        {
            var fileName = $"{ Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64").Replace(model.Base64Image, "");
            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 Falha interna no servidor"));
            }
            //como está registrado o User.Indentity na classe RoleClaimsExtension como Email então comparamos com o email, caso utilizasse o ID então deveriamos comparar x.Id com User.Identity.Name
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if(user != null)
            {
                return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));
            }

            user.Image = $"https://localhost:0000/images/{fileName}";

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 Falha interna no servidor"));
            }

            return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!" , null));
        }
    }
}
