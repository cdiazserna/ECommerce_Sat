using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Helpers;
using ECommerce_Sat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.Services
{
	public class UserHelper : IUserHelper
	{
		private readonly DataBaseContext _context;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<User> _signInManager;

        public UserHelper(DataBaseContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

        public async Task AddRoleAsync(string roleName)
		{
			bool roleExists = await _roleManager.RoleExistsAsync(roleName);
			
			if(!roleExists)
			{
				await _roleManager.CreateAsync(new IdentityRole
				{
					Name = roleName,
				});
			}	
		}

		public async Task<IdentityResult> AddUserAsync(User user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

        public async Task<User> AddUserAsync(AddUserViewModel addUserViewModel)
        {
            User user = new()
            {
                Address = addUserViewModel.Address,
                Document = addUserViewModel.Document,
                Email = addUserViewModel.Username,
                FirstName = addUserViewModel.FirstName,
                LastName = addUserViewModel.LastName,
                ImageId = addUserViewModel.ImageId,
                PhoneNumber = addUserViewModel.PhoneNumber,
                City = await _context.Cities.FindAsync(addUserViewModel.CityId),
                UserName = addUserViewModel.Username,
                UserType = addUserViewModel.UserType,
            };

            IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);
            if (result != IdentityResult.Success) return null;

            User newUser = await GetUserAsync(addUserViewModel.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;
        }


        public async Task AddUserToRoleAsync(User user, string roleName)
		{
			await _userManager.AddToRoleAsync(user, roleName);
		}

		public async Task<User> GetUserAsync(string email)
		{
			return await _context.Users
				.Include(u => u.City)
				.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<bool> IsUserInRoleAsync(User user, string roleName)
		{
			return await _userManager.IsInRoleAsync(user, roleName);
		}

		public async Task<SignInResult> LoginAsync(LoginViewModel loginViewModel)
		{
			return await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe, false);
		}

		public async Task LogoutAsync()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
