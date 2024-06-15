using MiApi.DTOs;
using MiApi.Helpers;
using MiApi.Models;
using MiApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly ICommonService<UserDto, UserInsertDto, UserUpdateDto> _userService;

        public AutenticacionController(IConfiguration config,
          [FromKeyedServices("userService")]ICommonService<UserDto, UserInsertDto,UserUpdateDto> userService)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            secretKey = config.GetSection("settings")
                                   .GetSection("secretKey")
                                   .ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
           
            _userService = userService;
        }


        [HttpPost]
        [Route("Validar")]

        public async Task<IActionResult> Validar([FromBody] User request)
        {
            var user = await _userService.Find(request.Name);
            if(user != null )
            {
                var temp = HelperCryptography.EncryptarPwd(request.Password, user.Salt);
                var res = HelperCryptography.CompareArrays(user.Clave, temp);

                if (!res) return Unauthorized(new { success = false, message = "INCORRECT Password" });


                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Name));
                var tokenDescriptor = new SecurityTokenDescriptor 
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                var tokenCreado = tokenHandler.WriteToken(tokenConfig);

              
                
                return Ok( new { success =true, token = tokenCreado }); 
            }
            else return Unauthorized(new { success = false, message = "Username NOT exist." });
            
        }




        [HttpGet]
        [Route("users")]

        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllAsync();

            return users != null ? Ok( users ) : NotFound() ;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserInsertDto userInsertDto)
        {
            if(!_userService.Validate(userInsertDto)) return BadRequest(new { success = false, message = _userService.Errors });
            
            userInsertDto.IsAdmin = false;
            userInsertDto.Salt = HelperCryptography.GenerateSalt();
            userInsertDto.Clave = HelperCryptography.EncryptarPwd(userInsertDto.Password, userInsertDto.Salt);

            await _userService.AddAsync(userInsertDto);

            return Ok(new {success = true, message = "Success Registration"});
        }

        [HttpPut]
        [Route("Update_User")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (!_userService.Validate(userUpdateDto)) return BadRequest(new { success = false, message = _userService.Errors });
            var user = await _userService.Find(userUpdateDto.Name);

            userUpdateDto.IsAdmin = false;
            userUpdateDto.Salt = HelperCryptography.GenerateSalt();
            userUpdateDto.Clave = HelperCryptography.EncryptarPwd(userUpdateDto.Password, userUpdateDto.Salt);

            await _userService.Update(user.Id,userUpdateDto);
            return Ok(new {success = true, message = "Changes successfully applied."})
        }
        
    }
}
