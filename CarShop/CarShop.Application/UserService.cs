using Microsoft.AspNetCore.Identity;
using CarShop.Domain;
using CarShop.Application.Models;
using CarShop.Application.Constants;

namespace CarShop.Application.Services;

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            if (await _roleManager.RoleExistsAsync(Roles.User))
            {
                await _userManager.AddToRoleAsync(user, Roles.User);
            }

            return result;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded) return null;

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null) return null;

            var token = await _jwtTokenGenerator.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }
    }