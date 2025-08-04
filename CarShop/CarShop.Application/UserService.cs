using Microsoft.AspNetCore.Identity;
using CarShop.Domain;
using CarShop.Application.Models;

namespace CarShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };
        
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                false, 
                false);

            return result.Succeeded;
        }
    }
}