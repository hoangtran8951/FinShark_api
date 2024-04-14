using apiSTockapi.Dto.Account;
using apiSTockapi.Interfaces;
using apiSTockapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace apiSTockapi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _tokeService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                Console.WriteLine($">>>>>>{registerDto.Email} {registerDto.Password}");
                // Console.WriteLine($">>>>>>{await _userManager.FindByEmailAsync(registerDto.Email)} {registerDto.Password}");
                var temp = await _userManager.FindByEmailAsync(registerDto.Email);
                Console.WriteLine($">>>>>>TEMP:  {temp}");
                if(await _userManager.FindByEmailAsync(registerDto.Email) != null)
                {
                    Console.WriteLine(">>> This Email already exited");
                    return StatusCode(500, "This Email already exited");
                }
                Console.WriteLine(registerDto);
                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if(createUser.Succeeded)
                {
                    Console.WriteLine(">>> This Email ");
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                        return Ok(new NewUserDto{
                            Email = appUser.Email,
                            UserName = appUser.UserName,
                            Token = _tokeService.CreateToken(appUser)
                        });
                    return StatusCode(500, roleResult.Errors);
                }
                return StatusCode(500, createUser.Errors);
            }
            catch (Exception e)
            {
                Console.WriteLine(">>> Error", e);
                return StatusCode(500, e);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {  
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                // Console.WriteLine($">>>>>>{loginDto.Email} {loginDto.Password}");
                var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == loginDto.Email);
                if(user == null)
                    return Unauthorized("This Email is not registed");
                
                var passworkChecking =  await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if( !passworkChecking.Succeeded)
                    return Unauthorized("this Email and/or Password are/is wrong");
                return Ok(new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokeService.CreateToken(user)
                });

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }
        
    }
}