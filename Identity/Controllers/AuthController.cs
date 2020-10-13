using Data.Models.IdentityServer;
using Data.Repositories.IdentityServer.Interfaces;
using Data.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ultilities.Constants;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Authorize Schema")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthSettingConfigs _authSettings;

        //repo
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthController(IConfiguration configuration, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            ILogger<AuthController> logger, IOptions<AuthSettingConfigs> settings, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _authSettings = settings.Value;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        //[ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
                if (!result.Succeeded)
                    return BadRequest("Mật khẩu không đúng");
                var roles = await _userManager.GetRolesAsync(user);

                IdentityOptions options = new IdentityOptions();
                var claims = new[]
                {
                    new Claim("Email", user.Email),
                    new Claim(SystemConstants.UserClaim.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(SystemConstants.UserClaim.FullName, user.FullName??string.Empty),
                    new Claim(options.ClaimsIdentity.RoleClaimType, string.Join(";", roles)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_authSettings.Iss,
                    _authSettings.Aud,
                     claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return NotFound($"Không tìm thấy tài khoản {model.UserName}");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new AppUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Adress=model.Adress,
                Email = model.Email,
                About = model.About,
                DoB = model.DoB
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                //await _userManager.AddClaimAsync(user, new Claim("Customers", "Write"));

                //await _signInManager.SignInAsync(user, false);

                return Ok(model);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("assign-to-role/{userId}/{roleName}")]
        public async Task<IActionResult> AssignToRole([Required] Guid userId, [Required] string roleName)
        {
            await _userRepository.AssignToRolesAsync(userId, roleName);
            return Ok();
        }

        [HttpDelete]
        [Route("remove-role/{userId}/{roleName}")]
        public async Task<IActionResult> RemoveRoles([Required] Guid userId, [Required] string roleName)
        {
            await _userRepository.RemoveRoleToUserAsync(userId, roleName);
            return Ok();
        }
    }
}
