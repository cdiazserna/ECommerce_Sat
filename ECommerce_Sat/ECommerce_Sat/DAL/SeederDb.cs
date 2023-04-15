using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Enum;
using ECommerce_Sat.Helpers;

namespace ECommerce_Sat.DAL
{
	public class SeederDb
	{
		private readonly DataBaseContext _context;
		private readonly IUserHelper _userHelper;

		public SeederDb(DataBaseContext context, IUserHelper userHelper)
		{
			_context = context;
			_userHelper = userHelper;
		}

		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();
			await PopulateCategoriesAsync();
			await PopulateCountriesStatesCitiesAsync();
			await PopulateRolesAsync();
			await PopulateUserAsync("First Name Admin", "Last Name Role", "adminrole@yopmail.com", "Phone 3002323232", "Add Street Fighter", "Doc 102030", UserType.Admin);
			await PopulateUserAsync("First Name User", "Last Name Role", "userrole@yopmail.com", "Phone 3502323232", "Address Street Fighter 2", "Doc 405060", UserType.User);

			await _context.SaveChangesAsync();
		}

		private async Task PopulateCategoriesAsync()
		{
			if (!_context.Categories.Any())
			{
				_context.Categories.Add(new Category { Name = "Tecnología", Description = "Elementos tech", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Implementos de Aseo", Description = "Detergente, jabón, etc.", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Ropa interior", Description = "Tanguitas, narizonas", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Gamers", Description = "PS5, XBOX SERIES", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Mascotas", Description = "Concentrado, jabón para pulgas.", CreatedDate = DateTime.Now });
			}
		}

		private async Task PopulateCountriesStatesCitiesAsync()
		{
			if (!_context.Countries.Any())
			{
				_context.Countries.Add(
				new Country
				{
					Name = "Colombia",
					CreatedDate = DateTime.Now,
					States = new List<State>()
					{
						new State
						{
							Name = "Antioquia",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Medellín", CreatedDate= DateTime.Now },
								new City { Name = "Bello", CreatedDate= DateTime.Now },
								new City { Name = "Itagüí", CreatedDate= DateTime.Now },
								new City { Name = "Sabaneta", CreatedDate= DateTime.Now },
								new City { Name = "Envigado", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "Cundinamarca",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Bogotá", CreatedDate= DateTime.Now },
								new City { Name = "Fusagasugá", CreatedDate= DateTime.Now },
								new City { Name = "Funza", CreatedDate= DateTime.Now },
								new City { Name = "Sopó", CreatedDate= DateTime.Now },
								new City { Name = "Chía", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "Atlántico",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Barranquilla", CreatedDate= DateTime.Now },
								new City { Name = "La Chinita", CreatedDate= DateTime.Now },
							}
						},
					}
				});

				_context.Countries.Add(
				new Country
				{
					Name = "Argentina",
					CreatedDate = DateTime.Now,
					States = new List<State>()
					{
						new State
						{
							Name = "Buenos Aires",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Avellaneda", CreatedDate= DateTime.Now },
								new City { Name = "Ezeiza", CreatedDate= DateTime.Now },
								new City { Name = "La Boca", CreatedDate= DateTime.Now },
								new City { Name = "Río de la Plata", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "La Pampa",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Santa María", CreatedDate= DateTime.Now },
								new City { Name = "Obrero", CreatedDate= DateTime.Now },
								new City { Name = "Rosario", CreatedDate= DateTime.Now }
							}
						}
					}
				});
			}
		}


		private async Task PopulateRolesAsync()
		{
			await _userHelper.AddRoleAsync(UserType.Admin.ToString());
			await _userHelper.AddRoleAsync(UserType.User.ToString());
		}

		private async Task PopulateUserAsync(string firstName, string lastName, string email, string phone, string address, string document, UserType userType)
		{
			User user = await _userHelper.GetUserAsync(email);

			if (user == null)
			{
				user = new User
				{
					CreatedDate = DateTime.Now,
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					UserName = email,
					PhoneNumber = phone,
					Address = address,
					Document = document,
					City = _context.Cities.FirstOrDefault(),
					UserType = userType,
				};

				await _userHelper.AddUserAsync(user, "123456");
				await _userHelper.AddUserToRoleAsync(user, userType.ToString());
			}
		}
	}
}
